using System.Data;
using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ModernToDoList.Api.Database.Factories;
using ModernToDoList.Api.Database.Migrations;
using ModernToDoList.Api.Domain.Connection;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Domain.Entities;
using ModernToDoList.Api.Domain.Providers;
using ModernToDoList.Api.Middlewares;
using ModernToDoList.Api.Repositories;
using ModernToDoList.Api.Services;
using NSwag;
using Serilog;

namespace ModernToDoList.Api;

public class Loader
{
    public async Task LoadAsync()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Host.UseSerilog();
        
        builder.Services.AddFastEndpoints();
        
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience"),
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWT:Key"))),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        builder.Services.AddSwaggerDoc(maxEndpointVersion: 1, settings: s =>
        {
            s.DocumentName = "Release 1.0";
            s.Title = "my api";
            s.Version = "v1.0";
            
            s.AddAuth(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = OpenApiSecuritySchemeType.ApiKey,
            });
        });

        var connectionString = builder.Configuration.GetValue<string>("Database:ConnectionString");

        if (builder.Environment.IsProduction())
            connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                               ?? throw new Exception("Environment Variable DB_CONNECTION_STRING is null");

        var connectionFactory = new PostgresConnectionFactory(connectionString);
        var count = builder.Configuration.GetValue<int>("Database:ConnectionsLimit");

        var connections = new IDbConnection[count];
        for (int i = 0; i < count; i++)
            connections[i] = await connectionFactory.CreateConnectionAsync();

        builder.Services.AddSingleton<IConnectionPool, ConnectionPool>(_ => new ConnectionPool(connections));

        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<IAttachmentImageRepository, AttachmentImageRepository>();
        builder.Services.AddSingleton<IToDoListRepository, ToDoListRepository>();
        builder.Services.AddSingleton<IToDoListItemRepository, ToDoListItemRepository>();

        builder.Services.AddCommandDefinitionBuilder<User>();
        builder.Services.AddCommandDefinitionBuilder<ImageAttachment>();
        builder.Services.AddCommandDefinitionBuilder<ToDoList>();
        builder.Services.AddCommandDefinitionBuilder<ToDoListItem>();
            
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddTransient<IEncryptionService, EncryptionService>();
        builder.Services.AddTransient<IStorageImageService, StorageImageService>();
        builder.Services.AddTransient<IToDoListService, ToDoListService>();

        builder.Services.AddTransient<IStorageProvider, AzureStorageProvider>();

        builder.Services
            .AddLogging(lb => lb.AddSerilog().AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(
                runner => runner
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(CreateUsersTableMigration).Assembly).For.All())
            .Configure<SelectingProcessorAccessorOptions>(
                opt => opt.ProcessorId = "PostgreSQL");

        var app = builder.Build();
        app.UseDefaultExceptionHandler();

        app.UseSerilogRequestLogging();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ValidationExceptionMiddleware>();
        app.UseFastEndpoints(c =>
        {
            c.Errors.ResponseBuilder = (failures, _, _) =>
            {
                return new ValidationFailureResponse
                {
                    Errors = failures.Select(y => y.ErrorMessage).ToList()
                };
            };
            c.Versioning.PrependToRoute = true;
            c.Versioning.Prefix = "api/v";
        });

        app.UseOpenApi();
        app.UseSwaggerUi3(s =>
        {
            s.ConfigureDefaults();
        });

        using var serviceScope = app.Services.CreateScope();
        var runner = serviceScope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();

        await app.RunAsync();
    }
}
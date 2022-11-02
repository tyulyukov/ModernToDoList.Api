using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ModernToDoList.Api.Database.Factories;
using ModernToDoList.Api.Database.Migrations;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Middlewares;
using ModernToDoList.Api.Repositories;
using ModernToDoList.Api.Services;
using NSwag;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddSwaggerDoc(s =>
{
    s.AddAuth(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
    {
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = OpenApiSecuritySchemeType.ApiKey,
    });
});

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new PostgresConnectionFactory(builder.Configuration.GetValue<string>("Database:ConnectionString")));
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IAttachmentImageRepository, AttachmentImageRepository>();

builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
builder.Services.AddSingleton<IStorageService, StorageService>();

builder.Services
    .AddLogging(lb => lb.AddDebug().AddFluentMigratorConsole())
    .AddFluentMigratorCore()
    .ConfigureRunner(
        runner => runner
            .AddPostgres()
            .WithGlobalConnectionString(builder.Configuration.GetValue<string>("Database:ConnectionString"))
            .ScanIn(typeof(CreateUsersTableMigration).Assembly).For.All())
    .Configure<SelectingProcessorAccessorOptions>(
        opt => opt.ProcessorId = "PostgreSQL");

var app = builder.Build();
app.UseDefaultExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseFastEndpoints(x =>
{
    x.Errors.ResponseBuilder = (failures, _, _) =>
    {
        return new ValidationFailureResponse
        {
            Errors = failures.Select(y => y.ErrorMessage).ToList()
        };
    };
});

app.UseOpenApi();
app.UseSwaggerUi3(s =>
{
    s.ConfigureDefaults();
});

using var serviceScope = app.Services.CreateScope();
var runner = serviceScope.ServiceProvider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();

app.Run();
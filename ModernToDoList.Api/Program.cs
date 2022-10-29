using FastEndpoints;
using FastEndpoints.Swagger;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using ModernToDoList.Api.Database.Factories;
using ModernToDoList.Api.Database.Migrations;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Repositories;
using ModernToDoList.Api.Validation;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new PostgresConnectionFactory(builder.Configuration.GetValue<string>("Database:ConnectionString")));
builder.Services.AddSingleton<IUserRepository, UserRepository>();

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
app.UseSwaggerUi3(s => s.ConfigureDefaults());

using var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;
var runner = services.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();

app.Run();
using ModernToDoList.Api.Domain.Command;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api;

public static class Extensions
{
    public static IServiceCollection AddCommandDefinitionBuilder<T>(this IServiceCollection services) where T : EntityBase
    {
        services.AddSingleton<ICommandDefinitionBuilder<T>, PostgresCommandDefinitionBuilder<T>>(
            provider => new PostgresCommandDefinitionBuilder<T>(typeof(T).Name + "s"));
        return services;
    }
}
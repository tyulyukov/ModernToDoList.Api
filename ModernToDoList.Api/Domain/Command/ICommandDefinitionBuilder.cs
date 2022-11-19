using Dapper;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Domain.Command;

public interface ICommandDefinitionBuilder<TObj> where TObj : EntityBase
{
    ICommandDefinitionBuilder<TObj> CreateQuery(TObj obj, CancellationToken ct = default);
    ICommandDefinitionBuilder<TObj> GetQuery(string id, CancellationToken ct = default);
    ICommandDefinitionBuilder<TObj> GetAllQuery(CancellationToken ct = default);
    ICommandDefinitionBuilder<TObj> UpdateQuery(TObj obj, CancellationToken ct = default);
    ICommandDefinitionBuilder<TObj> DeleteQuery(string id, CancellationToken ct = default);
    ICommandDefinitionBuilder<TObj> CustomQuery(string query, object? obj = null, CancellationToken ct = default);
    CommandDefinition Build();
}
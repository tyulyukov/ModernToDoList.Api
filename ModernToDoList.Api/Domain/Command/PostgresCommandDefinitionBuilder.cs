using System.Reflection;
using System.Text;
using Dapper;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Domain.Command;

public class PostgresCommandDefinitionBuilder<TObj> : ICommandDefinitionBuilder<TObj> where TObj : EntityBase
{
    private CommandDefinition? _commandDefinition;
    private readonly string _tableName;
    
    public PostgresCommandDefinitionBuilder(string tableName)
    {
        _tableName = tableName;
    }

    public ICommandDefinitionBuilder<TObj> CreateQuery(TObj obj, CancellationToken ct = default)
    {
        var fields = GetTypeFields();
        
        var sbFields = new StringBuilder();
        var sbParameters = new StringBuilder();

        for (var i = 0; i < fields.Length; i++)
        {
            sbFields.Append(fields[i]);
            
            if (i != fields.Length - 1)
                sbFields.Append(", ");

            sbParameters.Append('@');
            sbParameters.Append(fields[i]);
            
            if (i != fields.Length - 1)
                sbParameters.Append(", ");
        }

        var commandText = $"INSERT INTO {_tableName} ({sbFields.ToString()}) VALUES ({sbParameters.ToString()})";
        
        _commandDefinition = new CommandDefinition(
            commandText, 
            obj,
            cancellationToken: ct);
        return this;
    }
    
    public ICommandDefinitionBuilder<TObj> GetQuery(string id, CancellationToken ct = default)
    {
        var commandText = $@"SELECT * FROM {_tableName} WHERE Id = @Id LIMIT 1";
        
        _commandDefinition = new CommandDefinition(
            commandText, 
            new { Id = id },
            cancellationToken: ct);
        return this;
    }
    
    public ICommandDefinitionBuilder<TObj> GetAllQuery(CancellationToken ct = default)
    {
        var commandText = $@"SELECT * FROM {_tableName}";
        
        _commandDefinition = new CommandDefinition(
            commandText,
            cancellationToken: ct);
        return this;
    }
    
    public ICommandDefinitionBuilder<TObj> UpdateQuery(TObj obj, CancellationToken ct = default)
    {
        var fields = GetTypeFields();

        var sbSetters = new StringBuilder("SET ");
        for (var i = 0; i < fields.Length; i++)
        {
            sbSetters.Append(fields[i]);
            sbSetters.Append("= @");
            sbSetters.Append(fields[i]);
            
            if (i != fields.Length - 1)
                sbSetters.Append(", ");
        }

        var commandText = $@"UPDATE {_tableName} {sbSetters.ToString()} WHERE Id = @Id";
        
        _commandDefinition = new CommandDefinition(
            commandText, 
            obj,
            cancellationToken: ct);
        return this;
    }
    
    public ICommandDefinitionBuilder<TObj> DeleteQuery(string id, CancellationToken ct = default)
    {
        var commandText = $@"DELETE FROM {_tableName} WHERE Id = @Id";
        
        _commandDefinition = new CommandDefinition(
            commandText, 
            new { Id = id },
            cancellationToken: ct);
        return this;
    }

    public ICommandDefinitionBuilder<TObj> CustomQuery(string query, object? obj = null, CancellationToken ct = default)
    {
        _commandDefinition = new CommandDefinition(
            query, 
            obj,
            cancellationToken: ct);
        return this;
    }
    
    public CommandDefinition Build()
    {
        if (!_commandDefinition.HasValue)
            throw new ApplicationException();
        
        return _commandDefinition.Value;
    }

    // TODO cache + move to service or smth
    private ReadOnlySpan<string> GetTypeFields()
    {
        var properties = typeof(TObj).GetProperties();
        var names = new string[properties.Length];

        for (var i = 0; i < properties.Length; i++)
            names[i] = properties[i].Name;

        return new ReadOnlySpan<string>(names);
    }
}
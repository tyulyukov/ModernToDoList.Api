using System.Data;

namespace ModernToDoList.Api.Domain.Providers;

public interface IConnectionProvider : IDisposable
{
    IDbConnection Connection { get; }
}
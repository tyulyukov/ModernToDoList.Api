using System.Data;
using ModernToDoList.Api.Domain.Providers;

namespace ModernToDoList.Api.Domain.Connection;

public interface IConnectionPool : IDisposable
{
    IConnectionProvider UseConnection();
}
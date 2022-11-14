﻿using Dapper;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Connection;

namespace ModernToDoList.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IConnectionPool _connectionPool;

    public UserRepository(IConnectionPool connectionPool)
    {
        _connectionPool = connectionPool;
    }

    public async Task<bool> CreateAsync(User user)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            @"INSERT INTO Users (Id, Username, PasswordHash, EmailAddress, EmailAddressConfirmed,
                ImageAttachmentId, CreatedAt, UpdatedAt)
                VALUES (@Id, @Username, @PasswordHash, @EmailAddress, @EmailAddressConfirmed,
                        @ImageAttachmentId, @CreatedAt, @UpdatedAt)",
            user);
        return result > 0;
    }

    public async Task<User?> GetAsync(string id)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<User>(
            @"SELECT * FROM Users WHERE Id = @Id LIMIT 1",
            new { Id = id }
        );
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QueryAsync<User>(@"SELECT * FROM Users");
    }

    public async Task<bool> UpdateAsync(User user)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            @"UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, ImageAttachmentId = @ImageAttachmentId,
                EmailAddress = @EmailAddress, EmailAddressConfirmed = @EmailAddressConfirmed,
                CreatedAt = @CreatedAt, UpdatedAt = @UpdatedAt
                WHERE Id = @Id",
            user);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(@"DELETE FROM Users WHERE Id = @Id",
            new { Id = id });
        return result > 0;
    }

    public async Task<User?> FindByUsernameAsync(string username)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<User>(
            @"SELECT * FROM Users WHERE Username = @Username LIMIT 1", new { Username = username });
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<User>(
            @"SELECT * FROM Users WHERE EmailAddress = @EmailAddress LIMIT 1", new { EmailAddress = email });
    }
}
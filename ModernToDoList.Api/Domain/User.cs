﻿namespace ModernToDoList.Api.Domain;

public class User
{
    public Guid Id { get; init; }
    public String Username { get; init; }
    public String PasswordHash { get; init; }
    public String EmailAddress { get; init; }
    public bool EmailAddressConfirmed { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
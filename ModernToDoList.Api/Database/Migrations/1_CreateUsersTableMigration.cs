using FluentMigrator;

namespace ModernToDoList.Api.Database.Migrations;

[Migration(1)]
public class CreateUsersTableMigration : Migration
{
    public override void Up()
    {
        Execute.Sql(@"CREATE TABLE Users (
            Id VARCHAR (36) PRIMARY KEY, 
            Username TEXT NOT NULL,
            PasswordHash TEXT NOT NULL,
            EmailAddress TEXT NOT NULL)");
    }

    public override void Down()
    {
        Execute.Sql(@"DROP TABLE Users");
    }
}
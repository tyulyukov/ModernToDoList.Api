using FluentMigrator;

namespace ModernToDoList.Api.Database.Migrations;

[Migration(2)]
public class AlterUsersTableMigration : Migration
{
    public override void Up()
    {
        Execute.Sql(@"ALTER TABLE Users ADD EmailAddressConfirmed BOOL NOT NULL DEFAULT FALSE;
                    ALTER TABLE Users ADD CreatedAt DATE NOT NULL DEFAULT now();
                    ALTER TABLE Users ADD UpdatedAt DATE NOT NULL DEFAULT now();");
    }

    public override void Down()
    {
        Execute.Sql(@"ALTER TABLE Users DROP COLUMN EmailAddressConfirmed;
                    ALTER TABLE Users DROP COLUMN CreatedAt;
                    ALTER TABLE Users DROP COLUMN UpdatedAt;");
    }
}
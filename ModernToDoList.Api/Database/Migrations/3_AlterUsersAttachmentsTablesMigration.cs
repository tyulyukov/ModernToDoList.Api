using FluentMigrator;

namespace ModernToDoList.Api.Database.Migrations;

[Migration(3)]
public class AlterUsersAttachmentsTablesMigration : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE ImageAttachments (
                Id VARCHAR (36) PRIMARY KEY, 
                AuthorId VARCHAR(36) NOT NULL,
                FileName TEXT NOT NULL,
                Url TEXT NOT NULL,
                BlurHash TEXT NULL,
                CONSTRAINT fk_author FOREIGN KEY(AuthorId) REFERENCES Users(Id)
            );

            ALTER TABLE Users ADD ImageAttachmentId VARCHAR(36) NULL;
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP TABLE ImageAttachments;
            ALTER TABLE Users DROP COLUMN ImageAttachmentId;
        ");
    }
}
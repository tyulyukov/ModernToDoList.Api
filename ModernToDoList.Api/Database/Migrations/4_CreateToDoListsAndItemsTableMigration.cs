using FluentMigrator;

namespace ModernToDoList.Api.Database.Migrations;

[Migration(4)]
public class CreateToDoListsAndItemsTableMigration : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE ToDoLists (
                Id VARCHAR (36) PRIMARY KEY, 
                AuthorId VARCHAR(36) NOT NULL,
                Title TEXT NOT NULL,
                Description TEXT NULL,
                CONSTRAINT fk_author FOREIGN KEY(AuthorId) REFERENCES Users(Id)
            );
            
            CREATE TABLE ToDoListItems (
                Id VARCHAR (36) PRIMARY KEY,
                ToDoListId VARCHAR(36) NOT NULL,
                ItemHeader TEXT NULL,
                Body TEXT NULL,
                IsDone BOOL NOT NULL DEFAULT FALSE,
                CONSTRAINT fk_todolist FOREIGN KEY(ToDoListId) REFERENCES ToDoLists(Id)
            );
            
            ALTER TABLE ImageAttachments ADD ToDoListItemId VARCHAR(36) NULL;
            ALTER TABLE ImageAttachments ADD CONSTRAINT fk_todolistitem FOREIGN KEY(ToDoListItemId) REFERENCES ToDoListItems(Id)
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP TABLE ToDoListItems;
            DROP TABLE ToDoLists;
            ALTER TABLE ImageAttachments DROP CONSTRAINT fk_todolistitem;
            ALTER TABLE ImageAttachments DROP COLUMN ToDoListItemId;
        ");
    }
}
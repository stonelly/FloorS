BEGIN TRANSACTION;
GO

ALTER TABLE [DOT_RafStgTable] ADD [ChangedItemNumber] nvarchar(40) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211122085759_AlterDOTRafStgTable', N'5.0.3');
GO

COMMIT;
GO
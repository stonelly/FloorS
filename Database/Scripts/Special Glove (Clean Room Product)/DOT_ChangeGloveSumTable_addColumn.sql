--BEGIN TRANSACTION;
--GO

--ALTER TABLE [DOT_ChangeGloveSumTable] ADD [BatchNumber] nvarchar(50) NULL;
--GO

--INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
--VALUES (N'20211209034126_DOT_ChangeGloveSumTable_addColumn', N'5.0.3');
--GO

--COMMIT;
--GO
BEGIN TRANSACTION;
GO

ALTER TABLE [DOT_FGJournalTable] ADD [BaseQty] numeric(32,6) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [DOT_FGJournalTable] ADD [GloveQty] numeric(32,6) NOT NULL DEFAULT 0.0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211214161655_add_GloveQty_BaseQty_for_MTS', N'5.0.3');
GO

COMMIT;
GO

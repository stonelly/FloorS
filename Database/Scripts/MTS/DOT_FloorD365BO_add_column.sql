
BEGIN TRANSACTION;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [AlternateGloveCode1] nvarchar(100) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [AlternateGloveCode2] nvarchar(100) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [AlternateGloveCode3] nvarchar(100) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [DOTCustomerLotID] nvarchar(50) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [Expiry] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [GCLabel] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [GlovesInnerboxNo] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [GrossWeight] numeric(32,6) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [HartalegaCommonSize] nvarchar(max) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [InnerDateFormat] nvarchar(max) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [InnerLabelSet] nvarchar(max) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [InnerProductCode] nvarchar(30) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [InnerboxinCaseNo] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [LotVerification] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [MadeToStockStatus] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [ManufacturingDateOn] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [NetWeight] numeric(32,6) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [OuterDateFormat] nvarchar(max) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [OuterLabelSetNo] nvarchar(30) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [OuterProductCode] nvarchar(30) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [PalletCapacity] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [PreShipmentPlan] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [PrintingSize] nvarchar(max) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [Reference1] nvarchar(25) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [Reference2] nvarchar(25) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [SpecialInnerCharacter] nvarchar(25) NULL;
GO

ALTER TABLE [DOT_FloorD365BO] ADD [SpecialInnerCode] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220213173227_DOT_FloorD365BO_add_column', N'5.0.3');
GO

COMMIT;
GO
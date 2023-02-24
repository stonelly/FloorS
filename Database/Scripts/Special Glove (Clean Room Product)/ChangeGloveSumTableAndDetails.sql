CREATE TABLE [DOT_ChangeGloveSumTable] (
    [Id] bigint NOT NULL IDENTITY,
    [D365BatchNumber] nvarchar(20) NOT NULL,
    [ModuleSequence] int NOT NULL,
    [SubSequence] int NOT NULL,
    [FSIdentifier] uniqueidentifier NOT NULL,
    [FunctionIdentifier] nvarchar(20) NOT NULL,
    [ProcessingStatus] int NOT NULL,
    [ErrorMessage] nvarchar(max) NULL,
    [PickListJournalId] nvarchar(100) NULL,
    [MovementJournalId] nvarchar(max) NULL,
    [RAFJournalId] nvarchar(30) NULL,
    [RouteCardJournalId] nvarchar(30) NULL,
    [BatchOrderNumber] nvarchar(30) NULL,
    [ItemNumber] nvarchar(40) NOT NULL,
    [ChangedItemNumber] nvarchar(40) NULL,
    [ItemSize] nvarchar(50) NULL,
    [SampleCPDConsumptionWeight] numeric(32,6) NOT NULL,
    [BatchWeight] numeric(32,6) NOT NULL,
    [Warehouse] nvarchar(10) NOT NULL,
    [Location] nvarchar(50) NULL,
    [RAFGoodQty] numeric(32,6) NOT NULL,
    [RAFVTSample] numeric(32,6) NOT NULL,
    [VTBatchNumber] nvarchar(20) NULL,
    [RAFWTSample] numeric(32,6) NOT NULL,
    [WTBatchNumber] nvarchar(20) NULL,
    [RAFHBSample] numeric(32,6) NOT NULL,
    [HBBatchNumber] nvarchar(20) NULL,
    [SampleWarehouse] nvarchar(10) NOT NULL,
    [OriginalPlantNo] nvarchar(50) NULL,
    [PlantNo] nvarchar(50) NULL,
    [RecordCount] int NOT NULL,
    [D365Parameter] nvarchar(max) NULL,
    [PostingDateTime] datetime2 NOT NULL,
    [DetailStagingJSON] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [IsDeleted] bit NOT NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    CONSTRAINT [PK_DOT_ChangeGloveSumTable] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [DOT_ChangeGloveSumTableDetails] (
    [Id] bigint NOT NULL IDENTITY,
    [ParentRefRecId] bigint NOT NULL,
    [Resource] nvarchar(255) NOT NULL,
    [GloveQty] numeric(32,6) NOT NULL,
    [RAFVTSample] numeric(32,6) NOT NULL,
    [RAFWTSample] numeric(32,6) NOT NULL,
    [RAFHBSample] numeric(32,6) NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [IsDeleted] bit NOT NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    CONSTRAINT [PK_DOT_ChangeGloveSumTableDetails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DOT_ChangeGloveSumTableDetails_DOT_ChangeGloveSumTable_ParentRefRecId] FOREIGN KEY ([ParentRefRecId]) REFERENCES [DOT_ChangeGloveSumTable] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_DOT_ChangeGloveSumTableDetails_ParentRefRecId] ON [DOT_ChangeGloveSumTableDetails] ([ParentRefRecId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211126020136_addChangeGloveSumTableAndDetails', N'5.0.3');
GO

COMMIT;
GO
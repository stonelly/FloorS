-- ===================================================================  
-- Author:  <Azrul Amin>  
-- Create date: <02-Feb-2018>  
-- Description: <Insert_DOT_FLOORAXINTPARENTTABLE & DOT_FGRAFSTGTABLE>  
-- =============================================
-- Change History
-- Date			Author				Comments
-- -----		------				-----------------------------
-- 28/06/2018 	Max He				Add Reference Batch Quantity & Glove Code for SMBP.
-- 15/11/2020	Azrul				Add New Fields for SPPBC.
-- 20/03/2021	Max he				Add default value to New Fields for SPPBC.
-- 15/12/2021	Max he				Add Base Qty and Glove Qty for MTS without SO.
-- ===================================================================  
ALTER PROCEDURE [dbo].[USP_DOT_CreateFGRAFJournal]  
        @BATCHORDERNUMBER nvarchar(80),
        @REFERENCEITEMNUMBER nvarchar(80),
        @CONFIGURATION nvarchar(20),  
        @WAREHOUSE nvarchar(20), 
        @RESOURCE nvarchar(20), 
        @CUSTOMERPO nvarchar(40), 
        @CUSTOMERREFERENCE nvarchar(60), 
        @SALESORDERNUMBER nvarchar(20), 
        @INNERLOTNUMBER nvarchar(90), 
        @OUTERLOTNUMBER nvarchar(90), 
        @CUSTOMERLOTNUMBER nvarchar(90), 
        @PRESHIPMENT int,
        @PRESHIPMENTCASES int,
		@POSTINGDATETIME datetime,
        @QUANTITY int,
		@PARENTREFRECID int,
		@LOCATION nvarchar(20),
		@PALLETNUMBER nvarchar(20),
		@ExpiryDate datetime,			-- TODO:add to FG Journal Table
		@ManufacturingDate datetime,	-- TODO:add to FG Journal Table
		@IsWTS bit,						-- flag to detect is from Make to Stock
		@ItemNumber	NVARCHAR (40),--Glove Code,ony apply to SBC
		@RefNumberOfPieces1      DECIMAL (10, 2),--For SMBP
		@RefNumberOfPieces2      DECIMAL (10, 2),--For SMBP
		@RefNumberOfPieces3      DECIMAL (10, 2),--For SMBP
		@RefNumberOfPieces4      DECIMAL (10, 2),--For SMBP
		@RefNumberOfPieces5      DECIMAL (10, 2),--For SMBP
		@RefItemNumber1          NVARCHAR (40),--For SMBP
		@RefItemNumber2          NVARCHAR (40),--For SMBP
		@RefItemNumber3          NVARCHAR (40),--For SMBP
		@RefItemNumber4          NVARCHAR (40),--For SMBP
		@RefItemNumber5          NVARCHAR (40),--For SMBP
		@BatchCardNumber		 NVARCHAR (50)='',		-- Surgical Packing Plan
		@BatchNumber			 NVARCHAR (20)='',		-- Surgical Packing Plan
		@PickingListQuantity	 DECIMAL (10, 2)=0.00,	-- Surgical Packing Plan
		@BatchSequence			 INT=0,				-- Surgical Packing Plan
		@GloveSize				 NVARCHAR (20)='',		-- Surgical Packing Plan
		@GloveSampleQuantity	 NVARCHAR (20)='0.00',		-- Surgical Packing Plan
		@BaseQty				 DECIMAL (10, 4)=0.00		-- For MTS without SO
AS  
BEGIN                                                                                                                                                                                                                       
 SET NOCOUNT ON;  
 INSERT INTO [dbo].[DOT_FGJournalTable]  
		([BatchOrderNumber]
		,[Configuration]
		,[CreationTime]
		--,[CreatorUserId]
		,[CustomerLotNumber]
		,[CustomerPO]
		,[CustomerReference]
		--,[DeleterUserId]
		--,[DeletionTime]
		--,[IndustryPCScanStatus]
		,[InnerLotNumber]
		,[IsDeleted]
		--,[LastModificationTime]
		--,[LastModifierUserId]
		,[OuterLotNumber]
		,[PalletNumber]
		--,[PickingListJournal]
		--,[PostToD365]
		--,[PostToeWArenavi]
		,[PostingDateTime]
		,[Preshipment]
		,[PreshipmentCases]
		,[Quantity]
		,[ReferenceItemNumber]  -- This is FG Item Code
		--,[ReportasFinishedJournal]
		,[Resource]
		,[SalesOrderNumber]
		,[Warehouse]
		,[ParentRefRecId]
		,[Location]
		,[IsWTS]				-- flag to detect is from Make to Stock
		,[ItemNumber]			-- This is Glove Code
		,[RefNumberOfPieces1]	-- Reference Batch 1 Glove Quantity
		,[RefNumberOfPieces2]	-- Reference Batch 2 Glove Quantity
		,[RefNumberOfPieces3]	-- Reference Batch 3 Glove Quantity
		,[RefNumberOfPieces4]	-- Reference Batch 4 Glove Quantity
		,[RefNumberOfPieces5]	-- Reference Batch 5 Glove Quantity
		,[RefItemNumber1]		-- Reference Batch 1 Glove code
		,[RefItemNumber2]		-- Reference Batch 2 Glove code
		,[RefItemNumber3]		-- Reference Batch 3 Glove code
		,[RefItemNumber4]		-- Reference Batch 4 Glove code
		,[RefItemNumber5]		-- Reference Batch 5 Glove code
		,[BatchCardNumber]		-- Surgical packing Plan
		,[BatchNumber]			-- Surgical packing Plan
		,[PickingListQuantity]	-- Surgical packing Plan
		,[BatchSequence]		-- Surgical packing Plan
		,[GloveSize]			-- Surgical packing Plan
		,[GloveSampleQuantity]	-- Surgical packing Plan
		,[BaseQty]				-- For MTS without SO
		,[GloveQty]				-- For MTS without SO
		)
     VALUES  
        (@BATCHORDERNUMBER
		,@CONFIGURATION
		,GETDATE()
        ,@CUSTOMERLOTNUMBER
        ,@CUSTOMERPO
        ,@CUSTOMERREFERENCE
        ,@INNERLOTNUMBER
		,0
        ,@OUTERLOTNUMBER
		,@PALLETNUMBER
		,@POSTINGDATETIME
        ,@PRESHIPMENT
        ,@PRESHIPMENTCASES
        ,@QUANTITY
        ,@REFERENCEITEMNUMBER   -- This is FG Item Code
        ,@RESOURCE
        ,@SALESORDERNUMBER
        ,@WAREHOUSE
		,@PARENTREFRECID
		,@LOCATION
		,@IsWTS					-- flag to detect is from Make to Stock
		,@ItemNumber			-- This is Glove Code
		,@RefNumberOfPieces1	-- Reference Batch 1 Glove Quantity
		,@RefNumberOfPieces2	-- Reference Batch 2 Glove Quantity
		,@RefNumberOfPieces3	-- Reference Batch 3 Glove Quantity
		,@RefNumberOfPieces4	-- Reference Batch 4 Glove Quantity
		,@RefNumberOfPieces5	-- Reference Batch 5 Glove Quantity
		,@RefItemNumber1		-- Reference Batch 1 Glove code
		,@RefItemNumber2		-- Reference Batch 2 Glove code
		,@RefItemNumber3		-- Reference Batch 3 Glove code
		,@RefItemNumber4		-- Reference Batch 4 Glove code
		,@RefItemNumber5		-- Reference Batch 5 Glove code
		,@BatchCardNumber		-- Surgical Packing Plan
		,@BatchNumber			-- Surgical Packing Plan
		,@PickingListQuantity	-- Surgical Packing Plan
		,@BatchSequence			-- Surgical Packing Plan
		,@GloveSize 			-- Surgical Packing Plan
		,@GloveSampleQuantity	-- Surgical Packing Plan
		,@BaseQty				-- For MTS without SO
		,@BaseQty * @QUANTITY)  -- For MTS without SO
END

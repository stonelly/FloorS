
DROP Table DOT_OpenBatchCard
DROP Table DOT_InventBatchSum

/****** Object:  Table [dbo].[DOT_InventBatchSum]    Script Date: 22-Mar-22 4:58:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DOT_InventBatchSum](
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[D365CountingJournalNo] [nvarchar](250) NOT NULL,
	[BatchNumber] [nvarchar](20) NOT NULL,
	[D365BatchNumber] [nvarchar](250) NOT NULL,
	[Quantity] [numeric](32, 6) NOT NULL,
	[OnHandQty] [numeric](32, 6) NOT NULL,
	[Warehouse] [nvarchar](20) NOT NULL,
	[Location] [nvarchar](50) NULL,
	[ItemSize] [nvarchar](20) NULL,
	[LastCutOffTime] [datetime2](7) NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
	[CountedQty] [numeric](32, 6) NOT NULL,
	[IsMigratedFromAX6] [bit] NOT NULL,
	[ItemNumber] [nvarchar](200) NULL,
	[Searchname] [nvarchar](200) NULL,
 CONSTRAINT [PK_DOT_InventBatchSum] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DOT_InventBatchSum] ADD  DEFAULT ((0.0)) FOR [CountedQty]
GO

ALTER TABLE [dbo].[DOT_InventBatchSum] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsMigratedFromAX6]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_DOT_InventBatchSum_D365BatchNumber_BatchNumber_Warehouse_Location]    Script Date: 22-Mar-22 5:02:27 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_DOT_InventBatchSum_D365BatchNumber_BatchNumber_Warehouse_Location] ON [dbo].[DOT_InventBatchSum]
(
	[D365BatchNumber] ASC,
	[BatchNumber] ASC,
	[Warehouse] ASC,
	[Location] ASC
)
WHERE ([Location] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



-- ==================================================================================================    
-- Name:   [USP_DOT_ValidateSOBCPosting]    
-- Purpose:   Validate SOBC Posting Count    
-- ==================================================================================================    
-- Change History    
-- Date               Author                     Comments    
-- -----   ------   ---------------------------------------------------------------------------------    
--  7, 1,2019    Max He       SP created.    
-- ==================================================================================================    
alter PROCEDURE [dbo].[USP_DOT_ValidateSOBCPosting]    
 @serialNo NUMERIC(10,0)    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
 DECLARE @ReworkOrderCount INT    
 DECLARE @SOBCCount INT    
    
 SELECT @ReworkOrderCount=COUNT(1)    
 FROM AXPostingLog WITH (NOLOCK)     
 WHERE ServiceName='RWKCR' AND SerialNumber=@serialNo    
 group by ServiceName;    
    
     
 SELECT @SOBCCount=COUNT(1)    
 FROM AXPostingLog WITH (NOLOCK)     
 WHERE ServiceName='SOBC' AND SerialNumber=@serialNo    
 GROUP BY ServiceName;    
    
 SET @ReworkOrderCount = ISNULL(@ReworkOrderCount,0);    
 SET @SOBCCount   = ISNULL(@SOBCCount,0);    
    
 IF @ReworkOrderCount-@SOBCCount=1 -- allow insert SOBC    
  BEGIN    
   SELECT 1;    
  END    
 ELSE IF @ReworkOrderCount-@SOBCCount>1 -- allow insert SOBC if got extra rework from HBC RESAMPLE   
  BEGIN    
    DECLARE @ResampleCount INT    
  
 select @ResampleCount=COUNT(1)   
 from DOT_FloorAxIntParentTable a with (nolock)   
 join DOT_RafStgTable c with (nolock) on a.id = c.ParentRefRecId  
 join DOT_FloorD365HRGLOVERPT b with (nolock) on a.BatchNumber = b.SerialNo and b.Resource = c.Resource  
 where a.BatchNumber = convert(varchar,@serialNo) and a.FunctionIdentifier = 'HBC'   
 and a.ReferenceBatchNumber1 = 'RESAMPLE' and b.SeqNo = 1  
  
 SET @ResampleCount = ISNULL(@ResampleCount,0);   
  
 IF @ReworkOrderCount-@ResampleCount-@SOBCCount = 1  
  BEGIN  
   SELECT 1;  
  END  
 ELSE  
  BEGIN  
   SELECT 0;  
  END   
  END    
 ELSE IF @ReworkOrderCount-@SOBCCount=0  -- SOBC create before rework order, should pop error    
  BEGIN    
	IF EXISTS (select 1 from DOT_InventBatchSum with(nolock) where BatchNumber=CAST(@serialNo as nvarchar(20)) and IsMigratedFromAX6=1) --handling for open batch always return 1
		BEGIN
			SELECT 1;
		END
	ELSE
		BEGIN
			SELECT -1;
		END
  END    
 ELSE    
  BEGIN    
   Select 0; -- SOBC tally with rework, should not create SOBC any more    
  END    
 SET NOCOUNT OFF;     
    
END  
GO

-- =============================================  
-- Name:   USP_GET_BatchTargetTime  
-- Purpose:   Calculate the batch target time  
-- =============================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   -----------------------------  
-- 01/08/2014  Ruhi Gupta    SP created.  
-- 09/09/2018   Max He     Brand info  
-- 12/01/2021   Vinoden     Change [AX_AVAGLOVERELQCTYPE] to [DOT_GLOVERELQCTYPE]  
-- =============================================  
CREATE OR ALTER PROCEDURE [dbo].[USP_GET_BatchTargetTime]  
(  
 @SerialNumber   NUMERIC(10,0),  
 @QCGroupMemberCount  INT,  
 @BatchStatus   NVARCHAR(15),  
 @InnerBoxCount   INT = 0,  
 @PackingSize   INT = 0,  
 @BatchStartDate   DATETIME,  
 @GroupId    INT,  
 @Brand     NVARCHAR(40)  
)  
AS  
BEGIN  
BEGIN TRANSACTION;  
BEGIN TRY  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON  
 DECLARE @QCTargetTimeAX   INT = 3600  
 DECLARE @GlovesCountAX   FLOAT     --pieceshr/numoftester from AX_AVAGLOVERELQCTYPE  
 DECLARE @DurationInSec    BIGINT  
 DECLARE @TotalPcs    BIGINT  
 DECLARE @NumOfTester   BIGINT  
 DECLARE @PiecesHr    BIGINT  
 DECLARE @QCType           NVARCHAR(15)  
 DECLARE @GloveType    NVARCHAR(50)  
 DECLARE @BatchStatusCurrent  NVARCHAR(15)   
 DECLARE @InnerBoxCountCurrent INT = 0  
 DECLARE @PackingSizeCurrent  INT = 0  
 DECLARE @TotalPcsCurrent  INT  
 DECLARE @ReworkCount   INT    
 DECLARE @GloveSize    NVARCHAR(8)  
 DECLARE @GroupType    NVARCHAR(50)  
  
  SELECT @QCType = QCType FROM Batch WHERE SerialNumber = @SerialNumber  
  SELECT @GloveType = GloveType  FROM Batch WHERE SerialNumber = @SerialNumber  
  SELECT @TotalPcs = TotalPCs FROM Batch WHERE SerialNumber = @SerialNumber  
  
  SELECT @GloveSize = Size FROM Batch WHERE SerialNumber = @SerialNumber  
  
  Select @GroupType = GroupType FROM QCGroupMaster WHERE QCGroupId = @GroupId  
   
  IF(@GroupType = 'Packing Group')  
  BEGIN  
  
   SELECT @NumOfTester = bl.NUMOFPACKERS,@PiecesHr = bl.PACKPCSPERHR   
   FROM DOT_FSBrandLines bl   
   WHERE bl.ItemId = @Brand AND bl.CUSTOMERSIZE = @GloveSize  
  
   /** old sp  
   SELECT @NumOfTester = NUMOFPACKERS,@PiecesHr = PACKPCSPERHR FROM AX_AVABRANDLINE WHERE stopped = 0   
   and ITEMID = @Brand AND CUSTOMERSIZE = @GloveSize  
   **/  
     
  END  
 ELSE  
  BEGIN  
  
   SELECT @NumOfTester = NUMOFTESTER,@PiecesHr = PIECESHR FROM DOT_GLOVERELQCTYPE ax   
   JOIN DOT_FSGloveCode gc ON gc.AvaGlovecodeTable_Id = ax.GloveRefRecId  
   JOIN DOT_FSItemMaster im ON im.Id = gc.ItemRecordId  
   JOIN DOT_FSQCTypeTable qc ON qc.id = ax.QCTypeId  
   WHERE im.ItemId = @GloveType AND qc.QCTYPE = @QCType  
     
   /** old sp  
   SELECT @NumOfTester = NUMOFTESTER,@PiecesHr = PIECESHR FROM AX_AVAGLOVERELQCTYPE WHERE stopped = 0   
   and GLOVECODE = @GloveType AND QCTYPE = @QCType  
   **/  
  
  END  
    
 if(@NumOfTester = 0)  
  SET @GlovesCountAX = null  
 ELSE  
     SET @GlovesCountAX = CAST(@PiecesHr AS FLOAT) / CAST(@NumOfTester AS FLOAT)  
  
  
    -- Batch Target Time for a Split Batch (Scan In)  
    IF EXISTS(SELECT * FROM QCYieldAndPacking WHERE SerialNumber = @SerialNumber)  
  
     BEGIN  
      Select Top(1) @BatchStatusCurrent = BatchStatus from QCYieldandPacking WHERE SerialNumber = @SerialNumber   
      order by lastmodifiedon desc  
  
      IF (@BatchStatusCurrent = 'Split Batch')  
  
       BEGIN  
        Select Top(1) @InnerBoxCountCurrent = InnerBoxCount, @PackingSizeCurrent = PackingSize from QCYieldandPacking   
        WHERE SerialNumber = @SerialNumber order by lastmodifiedon desc  
  
        IF(@InnerBoxCountCurrent is not null)  
        BEGIN  
         SET @DurationInSec = ((@TotalPcs - (@InnerBoxCountCurrent*@PackingSizeCurrent))  
              *@QCTargetTimeAX)/(@GlovesCountAX*@QCGroupMemberCount)  
        END  
        ELSE  
        BEGIN  
         Select @TotalPcsCurrent = CONVERT(int,BatchWeight / TenPiecesWeight * 10000, 0)  
         from QCyieldandPacking  
         where serialNumber = @SerialNumber  
         order by lastmodifiedon desc  
  
         SET @DurationInSec = ((@TotalPcs - @TotalPcsCurrent)  
              *@QCTargetTimeAX)/(@GlovesCountAX*@QCGroupMemberCount)  
        END  
       END  
  
      ELSE  
  
       BEGIN  
        Select Top(1) @ReworkCount = ReworkCount, @InnerBoxCountCurrent = InnerBoxCount   
        from QCyieldandPacking  
        where serialNumber = @SerialNumber  
        order by lastmodifiedon desc  
       
  
        IF(@InnerBoxCountCurrent is not null)  
        BEGIN  
         Select @TotalPcsCurrent = sum(InnerBoxCount*PackingSize) from QCyieldandPacking  
         where serialNumber = @SerialNumber  
         and ReworkCount = @ReworkCount  
        
        END  
  
        ELSE  
        BEGIN  
         Select @TotalPcsCurrent = CONVERT(int,BatchWeight / TenPiecesWeight * 10000, 0)  
         from QCyieldandPacking  
         where serialNumber = @SerialNumber  
         order by lastmodifiedon desc  
        END  
  
        SET @DurationInSec = (@TotalPcsCurrent*@QCTargetTimeAX)/(@GlovesCountAX*@QCGroupMemberCount)  
       
       END  
     END  
  
    -- Batch Target Time for a new batch (Scan In)  
    ELSE  
  
     BEGIN  
      SET @DurationInSec = (@TotalPcs*@QCTargetTimeAX)/(@GlovesCountAX*@QCGroupMemberCount)   
     END  
    SELECT DATEADD(SECOND,@DurationInSec,@BatchStartDate)  
   
END TRY  
BEGIN CATCH     
    IF @@TRANCOUNT > 0  
        ROLLBACK TRANSACTION;  
  THROW;  
END CATCH;  
IF @@TRANCOUNT > 0  
  COMMIT TRANSACTION;  
  
  END    
GO

      
-- =============================================      
-- Author: Srikanth Balda      
-- Create date: 4/5/2014      
-- Description: Get Active SOLine Data      
      
-- Author: Pang Yik Siu      
-- Modified date: SEP 2017      
-- Description: Add two columns: InnerLabelSetDateFormat, OuterLabelSetDateFormat, STShippingDateETD, ManufacturingETD      
      
-- Author: Loo Kah Heng      
-- Modified date: 29 Jan 2019      
-- Description: Added two columns: HSB_CustPODocumentDate, HSB_CustPORecvDate      
      
 --Author: Pang Yik Siu      
 --Modified date: 12 Jun 2020      
 --Description: Added columns BARCODE for FP VIsion project      
      
-- =============================================      
      
CREATE OR ALTER PROCEDURE [dbo].[usp_GET_SurgicalPackingPlan_GetPO]       
 -- Add the parameters for the stored procedure here       
 @PONumber nvarchar(20),      
 @ItemNumber nvarchar(40),    
 @ItemSize nvarchar(30)      
AS      
BEGIN       
 SET NOCOUNT ON;      
select SalesId  as PONumber,INVENTTRANSID,      
  PurchOrderFormNum as OrderNumber,      
  INNERVERIFICATION as BarcodeVerificationRequired,      
  --CustomerSpecification,      
  PRESHIPMENTSAMPLINGPLAN as PreshipmentPlan,      
  ItemId as ItemNumber,      
  ITEMNAME as ItemName,      
  ITEMTYPE,      
  SalesQty as ItemCases,      
  SalesName as CustomerName,      
  INNERLABELSET as InnersetLayout,      
  OUTERLABELSET as OuterSetLayout,      
  --CONFIGURATION as CustomerSize,      
  CustomerSize,    -- HSB CONFIGURATION is referring to customerSize, not in NGC, 14 Apr Azrul  
  --CONFIGURATIONNAME as CUstomerSizeDesc,      
  PrintingSize as CUstomerSizeDesc, -- for outer label printing, 26 Nov 2018 Max He       
  GrossWeight,      
  NETWEIGHT as NettWeight,      
  NUMBERINNERBOXINOUTER as CaseCapacity,       
  PalletCapacity,      
  CUSTOMERLOTID as CustomerLotNumber ,      
  Customerref as CustomerReferenceNumber,      
  GloveCode as GloveCode,      
  MANUFACTURINGDATEBASIS,      
  SALESSTATUS as POStatus,      
  HartalegaCommonSize as ItemSize ,      
  NUMBERGLOVESINNERBOX as InnerBoxCapacity,      
  INNERPRODUCTCODE as InnerProductCode,      
  OUTERPRODUCTCODE as OuterProductCode,      
  Expiry,      
  --BrandName       
  REFERENCE1  as ProductReferenceNumber,REFERENCE2 ,      
  OUTERVERIFICATION as GCLabelPrintingRequired,      
  AlternateGloveCode1,      
  AlternateGloveCode2,      
  AlternateGloveCode3,      
  SPECIALINNERCODE,      
  SPECIALINNERCODECHARACTER,      
  SHIPPINGDATEREQUESTED,      
  RECEIPTDATEREQUESTED,      
  STShippingDateConfirmed,      
  ManufacturingDateETD,           --Merged from HSBPackage03    
  -- Label set Optimization project    
  INNERLABELSETDATEFORMAT as InnerLabelSetDateFormat,   --Merged from HSBPackage03   
  OUTERLABELSETDATEFORMAT as OuterLabelSetDateFormat,   --Merged from HSBPackage03   
  -- HSB_CustPODocumentDate and HSB_CustPORecvDate (29Jan2019)    
  HSB_CustPODocumentDate as HSB_CustPODocumentDate,    
  HSB_CustPORecvDate as HSB_CustPORecvDate,    
  BARCODE,              --Merged from HSBPackage0  
  BARCODEOUTERBOX            --Merged from HSBPackage0   
FROM VW_AXSOline WHERE salesId = @PONumber AND ItemId = @ItemNumber and [CONFIGURATION] = @ItemSize    
END  

GO

/****** Object:  StoredProcedure [dbo].[usp_GET_SurgicalPackingPlan_POList]    Script Date: 10/12/2020 12:58:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Azman Kasim>
-- Create date: <07/08/2020>
-- Description:	<Get PO List for eFS surgical packing plan>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[usp_GET_SurgicalPackingPlan_POList]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select DISTINCT CustomerRef + ' | ' + SalesId as [Value], SalesId as [Key] from VW_AXSOline (nolock)
	WHERE ITEMTYPE = '8' AND WorkOrderStatus = 2 -- for HSB version approve enum value is 2 

END

GO



-- =======================================================  
-- Name:             usp_QAIChangeQCType_Save  
-- Purpose:          Save Change QC Type Details  
-- =======================================================  
-- Change History  
-- Date         Author     Comments  
-- -----        ------     -----------------------------  
-- 24/08/2014        NarendraNath    SP created  
-- 23/07/2018   Azrul    SP altered  
-- 20/11/2020 Chong KH Qai Scanning enhancement  
-- 05/01/2022	Pang		ITRF:20211229160942289340 To fix the bugs for QAIDate of Batch Table 
-- =======================================================  
ALTER PROCEDURE [dbo].[usp_QAIChangeQCType_Save]  
(  
 @SerialNumber Nvarchar(10),  
 @QAIInspectorId NVARCHAR(10),  
 @ChangedQCType  NVARCHAR(50),  
 @ChangeQCTypeReason  NVARCHAR(50),  
 @workstationId NVARCHAR(20),  
 @IsChangeQCType BIT,  
 @AuthorizedBy INT  
)  
AS  
BEGIN  
BEGIN TRANSACTION;  
BEGIN TRY       
   
 DECLARE @QAIChangeReason NVARCHAR(20)  
 DECLARE @NewQAIID INT  
 DECLARE @QAIID INT    
  
 INSERT INTO QAI(SerialNumber,BatchNumber,QAIInspectorId,QCType,WTSampliingSize,InnerBox,TenPcsWeight,PackingSize, QAIDate,VTSamplingSize,  
   QAIChangeReason,QITestResult,LastModifiedDateTime,WorkStationId,SubModuleId,QAIScreenName,IsAXPostingSuccess,QCTypeAuthorizedBy)  
  SELECT SerialNumber,BatchNumber,@QAIInspectorId,@ChangedQCType,WTSampliingSize,InnerBox,TenPcsWeight,PackingSize, QAIDate,VTSamplingSize,  
   @ChangeQCTypeReason,QITestResult,GETDATE(),@workstationId,SubModuleId,'QAIChangeQCType',1,@AuthorizedBy FROM QAI WHERE SerialNumber=@SerialNumber AND LastModifiedDateTime=(SELECT MAX(LastModifiedDateTime) FROM QAI WHERE  
   SerialNumber=@SerialNumber)  
         
  SELECT @QAIID= SCOPE_IDENTITY();     
  
  SELECT @NewQAIID= QAIid FROM qai WHERE SerialNumber=@SerialNumber AND QAIid<> @QAIID   
  AND LastModifiedDateTime =(SELECT MAX(LastModifiedDateTime) FROM qai WHERE  SerialNumber=@SerialNumber AND QAIid<> @QAIID)  
  
  INSERT INTO QAIDefectMapping  
  SELECT @QAIID,DefectID,NoOfDefects FROM  QAIDefectMapping   WHERE QAIId=@NewQAIID  
  
   -- 20/11/2020 Qai Scanning enhancement Start  
  INSERT INTO QAIDefectPositionMapping (QAIID,DefectID, DefectPositionId,NoOfDefects)  
  SELECT @QAIID,DefectID, DefectPositionId,NoOfDefects FROM  QAIDefectPositionMapping   WHERE QAIId=@NewQAIID  
   -- 20/11/2020 Qai Scanning enhancement End  
   
  UPDATE b SET QCType= q.QCType, LastModifiedOn=GETDATE() FROM Batch b JOIN qai q ON b.SerialNumber=q.SerialNumber  
  WHERE q.QAIId=@QAIID  
  
  UPDATE Batch SET IsChangeQCType = @IsChangeQCType WHERE SerialNumber = @SerialNumber  
END TRY  
BEGIN CATCH  
 DECLARE @ErrorMessage NVARCHAR(4000);  
 DECLARE @ErrorSeverity INT;  
 DECLARE @ErrorState INT;  
 SELECT   
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
  RAISERROR (@ErrorMessage,   
        @ErrorSeverity,  
        @ErrorState   
        );  
  
    IF @@TRANCOUNT > 0  
        ROLLBACK TRANSACTION;  
END CATCH;  
  
IF @@TRANCOUNT > 0  
  COMMIT TRANSACTION;  
  
  END  
  GO

     
-- =======================================================  
-- Name:   USP_SAV_BatchCard  
-- Purpose:   Save offline batch details  
-- =======================================================  
-- Change History  
-- Date         Author     Comments  
-- -----        ------     -----------------------------  
-- 10/06/2014  Sujana    SP ALTERd.  
-- 06/10/2014  Nagaraju    Added QCtype for Submodule=3  
-- 6/10/2017  MyAdamas Added Submodule Reprint Reproduction =118 reproduction visual test and 119 reproduction water tight    
-- 23/1/2018  MyAdamas Alter SP parameter submoduleid to use screen name instead of screen ID  
-- 01/03/2018 Azman  Add ReProduction Module 1  
-- 15/07/2021 Azman  NGC 1.5 4 char LineId fix   
-- 12/01/2021      Vinoden  Change [AX_AVAQCTYPETABLE] to [DOT_FSQCTYPETABLE]  
-- 30/11/2021 Azrul Merged from NGC to HSB.
-- =======================================================  
CREATE OR ALTER PROCEDURE [dbo].[USP_SAV_BatchCard]  
  -- Add the parameters for the stored procedure here  
 @shift     INT,  
 @line     NVARCHAR(5),  
 @size     NVARCHAR(3),  
 @gloveType    NVARCHAR(50),  
 @batchWeight   DECIMAL(18,3),  
 @tenPcsWeight   DECIMAL(18,3),  
 @batchCardDate   DATETIME,  
 @isOnline    BIT,  
 @operatorId    NVARCHAR(10),  
 @workStationNumber  NVARCHAR(25),  
 @batchType    NCHAR(10),  
 @location    INT,  
 @module         NVARCHAR(50),  
 @subModule        NVARCHAR(50),  
 @authorizedBy      NVARCHAR(25),  
 @lostArea       NVARCHAR(15),  
 @site     NVARCHAR(20),  
 @shiftname    NVARCHAR(1),  
 @authorizedFor   INT  
AS  
BEGIN  
 BEGIN TRY  
  DECLARE @serialSequenceNumber AS INT  
  DECLARE @tempSerialNumber AS NUMERIC(25)  
  DECLARE @batchNumber AS NVARCHAR(20)  
  DECLARE @totalPcs AS INT  
  DECLARE @shiftStartDate NVARCHAR(8)  
  DECLARE @QCType NVARCHAR(50)  
  DECLARE @LineId VARCHAR(4)  
  DECLARE @ShiftCurrentDate NVARCHAR(8)  
  SET @ShiftCurrentDate = CONVERT(VARCHAR(8), GETDATE(), 112)  
  IF ((@subModule = 3) OR (@subModule = 119))  -- #Azman 22/02/2018 Add ReproductionWT Module  
   SET @QCType = (SELECT TOP(1) QCType FROM DOT_FSQCTypeTable WHERE DESCRIPTION = 'PT' AND STOPPED = 0)  
  ELSE  
   SET @QCType = NULL  
  -- If Shift is not supplied, select shift from Shift master and assign ShiftStart date  
  IF @shift = 0  
  BEGIN  
   SET @shiftname = dbo.Ufn_GetCurrentShift('PN')  
   SET @shift = (SELECT ShiftId from Shiftmaster where Name = @shiftname AND GroupType = 'PN' AND IsDeleted = 0)  
   SET @shiftStartDate = dbo.Ufn_GetShiftStartDate(@shiftname)  
  END  
   --- If Shift is supplied assign ShiftStart date  
  ELSE  
  SET @shiftStartDate = dbo.Ufn_GetShiftStartDate(@shiftname)  
  SET @serialSequenceNumber = Next VALUE FOR SerialNumberSeq  
    
  -- Set batch Type from Enum Master  
  SET @batchType = (SELECT EnumValue from EnumMaster where EnumText = @batchType and EnumType = 'BatchType')  
  
  -- Generate Serial Number  
  SET @tempSerialNumber = @site + RIGHT(CONVERT(VARCHAR(8), GETDATE(), 1),2) + FORMAT(@serialSequenceNumber,'0000000')  
  -- Calculate Total Pieces  
  SET @totalPcs = (@batchWeight /@tenPcsWeight )*10000  
  -- Generate Batch Number  
  --TOTT ID 40, 148 CR changes as below.  
  --Ror batch type -> Tumbling - PrintLostBatchCard,Tumbling - PrintWaterTightBatchCard,Tumbling - PrintVisualTestBatchCard @shiftStartDate should be current date  
  
  --Azman 150721 - Fix on 4 char LineId  
  SET @LineId = CASE LEN(@line)   
    WHEN 2 THEN '0'+RIGHT(CAST(@Line as VARCHAR(5)),1)   
    WHEN 3 THEN  RIGHT(CAST(@Line as VARCHAR(5)),2)   
    WHEN 4 THEN  RIGHT(CAST(@Line as VARCHAR(5)),3)   
   END  
  SET @batchNumber= CASE @subModule WHEN 1 THEN   
        RTRIM(@shiftname) + @LineId + '/'+ @shiftStartDate+ '/' + @size  
        WHEN 2  THEN   
        RTRIM(@batchType) + RIGHT('0'+CAST(@location AS VARCHAR(2)),2) + '/' + @ShiftCurrentDate + '/' + @size  
        WHEN 3  THEN  
        RTRIM(CASE @batchType WHEN 'PWT' Then 'P'WHEN 'QWT' Then 'Q'WHEN 'OWT' Then 'O'WHEN 'PSW' Then 'I' END)+ RIGHT('0'+CAST(@location AS VARCHAR(2)),2) +'/' + @ShiftCurrentDate+ '/' + @size  
        WHEN 4  THEN  
        RTRIM(@batchType) + RIGHT('0'+CAST(@location AS VARCHAR(2)),2) +'/' + @shiftStartDate + '/' + @size  
        WHEN 5  THEN  
        RTRIM(@batchType) + RIGHT('0'+CAST(@location AS VARCHAR(2)),2) +'/' + @ShiftCurrentDate + '/' + @size  
        WHEN 119 THEN -- #Azman 01/03/2018 Add ReproductionWT Module  
        RTRIM(@shiftname) + @LineId + '/'+ @shiftStartDate+ '/' + @size  
        WHEN 123 THEN -- #Azman 01/03/2018 Add ReproductionVT Module  
        RTRIM(@shiftname) + @LineId + '/'+ @shiftStartDate+ '/' + @size  
        END  
  BEGIN TRANSACTION  
   -- Insert in to Batch table  
   INSERT INTO Batch (SerialNumber,BatchNumber,ShiftId,LineId,GloveType,Size,TierSide,BatchWeight,TenPCsWeight,  
   BatchLostArea,BatchCardDate,QCType,QAIDate,BypassReasonId,ReferenceNumber ,ReWorkCount ,IsReprint ,IsOnline ,TotalPCs ,  
   ModuleId ,SubModuleId ,LocationId ,BatchType ,AuthorizedBy ,LastModifiedOn ,WorkStationId,  
   IsFPBatchSplit ,BatchCardCurrentLocation,AuthorizedFor )VALUES(@tempSerialNumber,@batchNumber,@shift,@line,@gloveType,  
   @size,NULL,@batchWeight,@tenPcsWeight,@lostArea,@batchCardDate,@QCType,Null,NULL ,  
   NULL,0,NULL,0,@totalPcs,@module,@subModule,@location ,@batchType ,  
   @authorizedBy,@batchCardDate,@workStationNumber,NULL,'PN',@authorizedFor)  
   -- Insert in to Log Table  
   INSERT INTO BatchLog VALUES(@tempserialNumber,@operatorId,@location,GETDATE())  
   IF ((@subModule = 3) OR (@subModule = 119))  -- #Azman 22/02/2018 Add ReproductionWT Module  
   BEGIN  
   INSERT INTO QAI (QAIDate,QAIInspectorId,SerialNumber,BatchNumber,QCType,TenPcsWeight,LastModifiedDateTime,WorkStationId,SubModuleId,IsAXPostingSuccess)  
   VALUES (@batchCardDate,@operatorId,@tempSerialNumber,@batchNumber,@QCType,@tenPcsWeight,GETDATE(),@workStationNumber,@SubModule,1)  
   UPDATE Batch SET QAIDate = GETDATE() WHERE SerialNumber = @tempSerialNumber  
   END  
 -- Return Serial Number and Batch Number  
 SELECT @batchNumber AS 'BatchNumber' ,@tempSerialNumber AS 'SerialNumber'  
 END TRY  
 BEGIN CATCH  
  IF @@TRANCOUNT > 0  
   ROLLBACK TRANSACTION;  
   THROW;  
 END CATCH;  
 IF @@TRANCOUNT > 0  
 COMMIT TRANSACTION;  
END   

GO

    
-- =============================================      
-- Author:  Srikanth Balda      
-- Create date: 17 July 2014      
-- Description: Insert the Trasaction and update the Batch or TempPack      
-- =============================================      
ALTER  PROCEDURE [dbo].[usp_FP_FinalPacking_Save]      
 -- Add the parameters for the stored procedure here      
 (      
  @LocationId int,      
  @WorkStationNumber nvarchar(25),      
  @PrinterName nvarchar(30),      
  @PackDate datetime,      
  @OuterLotNo nvarchar(15),      
  @InternalLotNumber nvarchar(15),      
  @PONumber nvarchar(20),      
  @ItemNumber nvarchar(40),      
  @Size nvarchar(10),      
  --@GroupId int,      
  @SerialNumber numeric(15,0),      
  @BoxesPacked int,      
  @PalletId nvarchar(8),      
  @CasesPacked int,      
  @PreShipmentPalletId nvarchar(8)= null,      
  @PreshipmentCasesPacked int,      
  @OperatorId int = null,      
  @InnerSetLayout nvarchar(30),      
  @OuterSetLayout nvarchar(30),      
  @palletCapacity int,      
  @TotalPcs int,      
  @isTempPack Bit,      
  @strXML nvarchar(max) = null,      
  @ManufacturingDate datetime,      
  @ExpiryDate datetime,      
  @stationNumber int = null,      
  @InventTransId nvarchar(25) = null,      
  @FGBatchOrderNo nvarchar(20),      
  @Resource nvarchar(20),      
  @FPStationNo nvarchar(5) = null      
 )      
AS      
BEGIN      
BEGIN TRANSACTION;      
 BEGIN TRY      
 DECLARE @rowsCount int      
 DECLARE @PalletCases int      
 DECLARE @prePalletCases int      
 DECLARE @idoc int      
        
  INSERT INTO dbo.FinalPacking       
  (LocationId,WorkStationId,PrinterName,PackDate,OuterLotNo,InternalLotNumber,PONumber,ItemNumber,      
  Size,SerialNumber,BoxesPacked,PalletId,CasesPacked,PreShipmentPalletId,PreshipmentCasesPacked,      
  OperatorId,LastModifiedOn,InnersetLayout,OutersetLayout,ManufacturingDate      
  ,ExpiryDate,InventTransId,FGBatchOrderNo,Resource, FPStationNo)      
  VALUES (@LocationId,@WorkStationNumber,@PrinterName,@PackDate,@OuterLotNo,@InternalLotNumber,@PONumber,      
  @ItemNumber,@Size,@SerialNumber,@BoxesPacked,@PalletId,@CasesPacked,@PreShipmentPalletId,@PreshipmentCasesPacked,      
  @OperatorId,SYSDATETIME(),@InnerSetLayout,@OuterSetLayout,      
  @ManufacturingDate ,      
  @ExpiryDate, @InventTransId,@FGBatchOrderNo,@Resource,@FPStationNo      
  )      
      
  IF @strXML is not null      
  BEGIN      
  EXEC sp_xml_preparedocument @idoc OUTPUT, @strXML      
      
  INSERT INTO FinalPackingBatchInfo (SerialNumber,BoxesPacked,CasesPacked,PreshipmentCasesPacked,InternalLotNumber)      
  SELECT  SerialNumber, BoxesPacked, CasesPacked, PreshipmentCasesPacked, @InternalLotNumber as InternalLotNumber      
  FROM OPENXML(@idoc, '/FinalPackingBatchInfoDTO')      
  WITH (SerialNumber numeric(10,0)      
  ,BoxesPacked int      
  ,CasesPacked int      
  ,PreshipmentCasesPacked int      
  ,PalletId nvarchar(8)      
  ,PreshipmentPalletId nvarchar(8)      
  )      
  EXEC sp_xml_removedocument @idoc      
  END       
      
  DECLARE @intRunningNumber int      
  IF(@stationNumber is not null)      
  BEGIN      
   SELECT @intRunningNumber = isnull(LastRunningLotNumber,0)+1 FROM WorkstationRunningNumber WHERE WorkStationId = @stationNumber      
   EXEC usp_FP_WorkStationNumber_Update @stationNumber, @intRunningNumber      
  END      
  ELSE      
  BEGIN      
   SELECT @intRunningNumber = isnull(LastRunningLotNumber,0)+1 FROM WorkstationRunningNumber WHERE WorkStationId = @WorkStationNumber           
   EXEC usp_FP_WorkStationNumber_Update @WorkStationNumber, @intRunningNumber      
  END      
  --update batch capacity      
  EXEC usp_FP_BatchPackedPcs_Update @SerialNumber,@TotalPcs ,@isTempPack      
      
    -- Update PurchaseOrderItemCases table with pallet id and internalalLot Number       
  UPDATE dbo.PurchaseOrderItemCases       
  SET  InternalotNumber = @internallotnumber      
  WHERE casenumber in ( SELECT TOP (@CasesPacked) CaseNumber FROM PurchaseOrderItemCases        
  WHERE PONumber = @PONumber and ItemNumber = @ItemNumber and Size = @Size and InternalotNumber is null ORDER BY CaseNumber )      
  and PONumber = @PONumber and ItemNumber = @ItemNumber and Size = @Size      
      
 END TRY      
 BEGIN CATCH      
  SELECT       
   ERROR_NUMBER() AS ErrorNumber      
   ,ERROR_SEVERITY() AS ErrorSeverity      
   ,ERROR_STATE() AS ErrorState      
   ,ERROR_PROCEDURE() AS ErrorProcedure      
   ,ERROR_LINE() AS ErrorLine      
   ,ERROR_MESSAGE() AS ErrorMessage;      
  IF @@TRANCOUNT > 0      
   ROLLBACK TRANSACTION;      
   throw;      
 END CATCH;      
      
IF @@TRANCOUNT > 0      
  COMMIT TRANSACTION;      
      
END   
  
GO  

  
 -- ==================================================================================================      
-- Name:   [USP_GET_SerialNo_QI]      
-- Purpose:   <Check whether QI Result>      
-- ==================================================================================================      
-- Change History      
-- Date               Author                     Comments      
-- -----   ------   ---------------------------------------------------------------------------------      
-- <21 Oct,2014>   <Srikanth Balda>          SP created.      
-- <06 Oct,2016>  <Cheah Teik Chuan>    added new checking for additional QCTypes      
-- <25 Oct,2018>  <Pang Yik Siu>    add new QCtypes      
-- <28 Sept,2018>  <Amirul Kamil>    comment quarantine/repack checking      
-- <09 Nov,2020>  <Azrul>    Surgical Glove must check PTQI & QCQI            
-- <29 July,2021> <Pang Yik Siu>  Add nolock  
-- ==================================================================================================      
-- exec usp_get_serialno_qi '2200835464'      
CREATE OR ALTER PROCEDURE [dbo].[USP_GET_SerialNo_QI]      
(      
     @serialNumber numeric(10,0)  
)      
AS      
BEGIN      
 SET NOCOUNT ON;      
 Declare @QCtype nvarchar(15)      
 Declare @BatchType nchar(40)      
 declare @QCLastModDate datetime      
 declare @PTLastModDate datetime
 declare @QAIDateForLatestScan datetime  
 declare @IsSurgicalGlove INT = 0
 declare @serialStr nvarchar(20) = CAST(@SerialNumber as nvarchar(20))
 select @QCType = QCType from Batch (NOLOCK) where serialnumber = @serialNumber       
 select @QCLastModDate=max(LastModifiedOn) from QCYieldAndPacking WITH (nolock) where serialnumber=@serialNumber      
 select @PTLastModDate=max(LastModifiedOn) from PTScanBatchCard WITH (nolock) where serialnumber=@serialNumber  
 SELECT @QAIDateForLatestScan = MAX(QAIDate) FROM QAI WITH (nolock) where serialnumber=@serialNumber   
         AND (QAIScreenName IN ('QAIScanInnerTenPcs','QAIScan') OR QAIScreenName IS NULL)     
       
 -- Bypass SP checking for surgical    
 IF exists (select 1 from Batch b with (nolock) join DOT_FSItemMaster i with (nolock) on b.GloveType = i.ItemId 
		 where b.SerialNumber = @SerialNumber and i.ItemType = 109) -- surgical glove
 BEGIN    
 SET @IsSurgicalGlove = 1    
 END    
    
 IF (@QCType != '0006020001' OR @IsSurgicalGlove = 1) -- Surgical Glove must check PTQI & QCQI     
 BEGIN       
  --if(@QCType = '0006020019') --quarantine      
  --begin      
  -- IF Exists(select 1 from FP_SerialNo_Exemption (NOLOCK) where SerialNumber=@serialNumber) --check if there is exemption. if yes, will always pass      
  -- begin      
  --  Select 'Pass'      
  -- end      
  -- else      
  -- begin      
  --  Select 'Fail1'      
  -- end      
  --end      
  if(@QCType = '0006020018') --dimension failed      
  begin      
   IF Exists(select 1 from FP_SerialNo_Exemption WITH (nolock) where SerialNumber=@serialNumber) --check if there is exemption. if yes, will always pass      
   begin      
    Select 'Pass'      
   end      
   else      
   begin      
    IF EXISTS(SELECT 1 FROM  QCYieldandPacking WITH (nolock) WHERE serialNumber = @SerialNumber)      
    BEGIN      
     SELECT @BatchType = Batchtype FROM Batch (nolock) where serialnumber = @serialNumber       
     IF (@BatchType = 'QWT' or @BatchType = 'PWT' or @BatchType = 'OWT' or @BatchType = 'PSW')      
     BEGIN         
      IF EXISTS (SELECT 1 FROM PTScanBatchCard WITH (nolock) WHERE serialnumber = @serialnumber)      
       BEGIN      
        if exists(Select 1 from QAI WITH (nolock) WHERE serialnumber = @serialNumber       
        and QAIDate > @PTLastModDate      
        and QAIScreenName='ScanQITestResult')      
        begin      
         SELECT TOP 1 case when QITestResult is null then 'Fail'      
         else case when QITestResult='Fail' then 'Fail1'      
         else QITestResult end end       
         FROM QAI with (nolock) WHERE serialnumber = @serialNumber       
         and QAIDate > @PTLastModDate      
         and QAIScreenName='ScanQITestResult'      
         order by qaiid desc      
        end      
        else      
        begin      
         select 'Fail'      
        end      
       END     
      ELSE      
       BEGIN      
        SELECT 'Fail'      
       End      
     END          
     ELSE      
     BEGIN       
      if exists(Select 1 from QAI with (nolock) WHERE serialnumber = @serialNumber       
      and QAIDate > @PTLastModDate      
      and QAIScreenName='ScanQITestResult')      
      begin      
       SELECT TOP 1 case when QITestResult is null then 'Fail'      
       else case when QITestResult='Fail' then 'Fail1'      
       else QITestResult end end       
       FROM QAI (nolock) WHERE serialnumber = @serialNumber       
       and QAIDate > @PTLastModDate      
       and QAIScreenName='ScanQITestResult'      
       order by qaiid desc      
      end      
      else      
      begin      
       select 'Fail'      
      end      
     END      
    END      
    ELSE      
    BEGIN        
     SELECT 'Fail'      
    END       
   end      
  end      
  else if (@QCType = '0006020020') --PT      
  begin      
   IF Exists(select 1 from FP_SerialNo_Exemption WITH (nolock) where SerialNumber=@serialNumber) --check if there is exemption. if yes, will always pass      
   begin      
    Select 'Pass'      
   end      
   else      
   begin      
    IF EXISTS (SELECT 1 FROM PTScanBatchCard WITH (nolock) WHERE serialnumber = @serialnumber)      
     BEGIN      
      if exists(Select 1 from QAI (nolock) WHERE serialnumber = @serialNumber       
      and QAIDate > @PTLastModDate      
      and QAIScreenName='ScanQITestResult')      
      begin      
       SELECT TOP 1 case when QITestResult is null then 'Fail'      
       else case when QITestResult='Fail' then 'Fail1'      
       else QITestResult end end       
       FROM QAI WITH (nolock) WHERE serialnumber = @serialNumber       
       and QAIDate > @PTLastModDate      
       and QAIScreenName='ScanQITestResult'      
       order by qaiid desc      
      end      
      else      
      begin      
       select 'Fail'      
      end      
     END      
    ELSE      
     BEGIN      
      SELECT 'Fail'      
     End      
   end      
  end      
  else -- QCType not PT, Dimension Failed, or Quarantine      
  begin       
   -- other non-straight pack QC Type :  -- <25 Oct,2018>  <Pang Yik Siu> add new QCtypes      
   IF Exists(select 1 from FP_SerialNo_Exemption (nolock) where SerialNumber=@serialNumber) --check if there is exemption. if yes, will always pass      
   BEGIN      
    SELECT 'Pass'      
   END      
   ELSE      
   BEGIN      
    IF EXISTS(SELECT 1 FROM  QCYieldandPacking with (nolock) WHERE serialNumber = @SerialNumber)      
    BEGIN      
     SELECT @BatchType = Batchtype FROM Batch (nolock) where serialnumber = @serialNumber       
     IF (@BatchType = 'QWT' or @BatchType = 'PWT' or @BatchType = 'OWT' or @BatchType = 'PSW')      
      BEGIN          
       IF EXISTS (SELECT 1 FROM PTScanBatchCard with (nolock) WHERE serialnumber = @serialnumber)      
        BEGIN      
         --SELECT TOP 1 QITestResult FROM QAI WHERE serialnumber = @serialNumber order by qaiid desc  
		 -- pang: add QI screen filter and 1 year QAI scan checking     
         --SELECT TOP 1 QITestResult FROM QAI (nolock) WHERE serialnumber = @serialNumber and QAIScreenName='ScanQITestResult' order by qaiid desc  
         SELECT TOP 1 QITestResult FROM QAI (NOLOCK)  
         WHERE serialnumber = @serialNumber  
          AND QAIScreenName='ScanQITestResult'   
          AND QAIDate > @QAIDateForLatestScan  
         ORDER BY QAIId DESC  
        END      
       ELSE      
        BEGIN      
         SELECT 'Fail'      
        End      
      END          
     ELSE      
      BEGIN  --Check last QCQI status               
  IF EXISTS(SELECT TOP 1 QITestResult FROM QAI (nolock) WHERE serialnumber = @serialNumber and QAIDate > @QCLastModDate order by qaiid desc)  
   BEGIN  
    SELECT TOP 1 QITestResult FROM QAI (nolock) WHERE serialnumber = @serialNumber and QAIDate > @QCLastModDate  order by qaiid desc  
   END  
  ELSE  
   BEGIN  
    SELECT 'Fail'   
   END  
      END      
    END      
    ELSE      
    BEGIN        
     SELECT 'Fail'      
    END       
   END          
  end      
 end      
 Else      
 Begin        
  IF EXISTS (SELECT 1 FROM PTScanBatchCard (nolock) WHERE serialnumber = @serialnumber)      
   BEGIN   
    -- pang: if the latest QAI Date (QAI date belong to QAI screen name  (QAIScan/QAIScanInnerTenPcs/NULL))  
    -- , is bigger than the latest PT/QC latest date & the latest Qctype is 0006020001 (SP) from Batch table  
    --  , final packing should allow to pack.  
    IF (@QAIDateForLatestScan > @PTLastModDate)  
    BEGIN  
     SELECT 'Pass'  
    END  
    ELSE  
    BEGIN  
      -- pang: add QI screen filter    
     SELECT TOP 1 QITestResult FROM QAI (nolock) where serialnumber = @serialNumber and QAIScreenName='ScanQITestResult' order by QAIDate desc  
    END  
   END
  ELSE      
   BEGIN      
      SELECT @BatchType = Batchtype FROM Batch (nolock) where serialnumber = @serialNumber      
       IF (@BatchType = 'QWT' or @BatchType = 'PWT'  or @BatchType = 'OWT' or @BatchType = 'PSW')      
       BEGIN      
      SELECT 'Fail'      
       END      
       ELSE      
       BEGIN      
      SELECT 'Pass'      
       END      
   END         
 End          
    SET NOCOUNT OFF;      
END
GO
  
-- =============================================  
-- Name:   USP_SEL_WorkOrderDetailsById  
-- Purpose:   Get Work Order details  
-- =============================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   -----------------------------  
-- 31/03/21       Yik Siu get itemcasesPacked by size, itemnumber, PONumber  
-- 24/03/22		Max	He  Add isnull checking for FG brand not yet sync, ItemCaseCount return null
-- =============================================  
  
CREATE OR ALTER PROCEDURE [dbo].[USP_SEL_WorkOrderDetailsById]  
 @Id bigint  
AS  
BEGIN   
BEGIN TRANSACTION;  
BEGIN TRY  
   
 SET NOCOUNT ON  
  
 DECLARE @Local_Id bigint   
  
 SET @Local_Id = @Id
  
 ;WITH SalesLineDS AS  
 (  
  SELECT s.Id, s.SalesId, s.ItemId, s.ItemName, s.[CONFIGURATION], s.LineNum, s.SalesQty, s.DOTBaseQty, s.ReceiptDateRequested, s.DOTCustExpDate, s.DOTCustMfgDate  
   , s.DOTCustomerLotID, s.InnerLabelSet, s.Expiry, s.SpecialInnerCode, s.SpecialInnerCharacter, s.LotVerification AS HSB_LotVerification, s.InnerPrinter, s.InnerDateFormat  
   , s.OuterLabelSetNo, s.OuterPrinter, s.GCLabel, s.OuterDateFormat, s.PreShipmentPlan, s.PalletCapacity, s.GloveCode, s.AlternateGloveCode1  
   , s.AlternateGloveCode2, s.AlternateGloveCode3, s.HartalegaCommonSize, s.CustomerSize, s.PrintingSize, s.BaseUnit, s.NumOfBaseUnitPiece, s.NetWeight  
   , s.PackagingWeight, s.PackingSize, s.GrossWeight, s.BaseQuantity, s.GlovesInnerboxNo, s.InnerboxinCaseNo, s.InnerProductCode, s.OuterProductCode  
   , s.Reference1, s.Reference2, s.NumOfPackers, s.PackPcsPerHr, s.ManufacturingDateOn, s.GloveWeight, s.BARCODE, s.BARCODEOUTERBOX, s.ShippingDateConfirmed  
   --, s.ItemCaseCount  
  FROM DOT_FloorSales st (NOLOCK)  
  JOIN DOT_FloorSalesLine s (NOLOCK) ON s.SalesId = st.SalesId AND s.IsDeleted = st.IsDeleted  
  WHERE st.Id = @Local_Id AND st.IsDeleted = 0  
 )  
  
 SELECT a.*, isnull(drv.ItemCaseCount,0) ItemCaseCount
 FROM SalesLineDS a   
  LEFT JOIN (  
   SELECT a.SalesId, a.ItemId, ISNULL(l.HARTALEGACOMMONSIZE,ISNULL(poi.ItemSize,'')) HartalegaCommonSize,   
    ISNULL(l.CUSTOMERSIZE, a.CustomerSize) CustomerSize,  
    COUNT(poic.ItemNumber)AS ItemCaseCount  
   FROM SalesLineDS a   
    LEFT JOIN DOT_FSBrandHeaders h (NOLOCK) on a.ItemId = h.ITEMID  
    LEFT JOIN VW_AX_AVABRANDLINE_SALESLINE (NOLOCK) l on l.ITEMID = a.ITEMID  
     AND l.CUSTOMERSIZE= a.CustomerSize  
    LEFT JOIN PurchaseOrderItem poi (NOLOCK)  
     ON poi.PONumber = a.Salesid  
     AND poi.ItemNumber = a.ItemId  
     AND poi.CustomerSize = a.CustomerSize  
    LEFT JOIN PurchaseOrderItemCases poic (NOLOCK)   
     ON poic.PONumber = a.SalesID AND poic.PONumber = poi.PONumber  
     AND poic.ItemNumber = a.ItemID   
     AND poic.Size = poi.ItemSize  
     AND poic.CustomerSize = a.CustomerSize  
     AND poic.InternalotNumber IS NOT NULL  
    GROUP BY a.SalesId,a.ItemId,ISNULL(l.HARTALEGACOMMONSIZE,ISNULL(poi.ItemSize,'')),   
     ISNULL(l.CUSTOMERSIZE, a.CustomerSize)  
     ) drv ON drv.SalesId = a.SalesId AND drv.ItemId = a.ItemId AND drv.CustomerSize = a.CustomerSize  
  
END TRY  
  
BEGIN CATCH  
    SELECT   
        ERROR_NUMBER() AS ErrorNumber  
        ,ERROR_SEVERITY() AS ErrorSeverity  
        ,ERROR_STATE() AS ErrorState  
        ,ERROR_PROCEDURE() AS ErrorProcedure  
        ,ERROR_LINE() AS ErrorLine  
        ,ERROR_MESSAGE() AS ErrorMessage;  
    IF @@TRANCOUNT > 0  
        ROLLBACK TRANSACTION;  
END CATCH;  
  
IF @@TRANCOUNT > 0  
  COMMIT TRANSACTION;  
  
  END   

GO

alter PROCEDURE [dbo].[USP_DOT_IsOnlineSurgicaGlove]  
(  
       @serialnumber numeric(15,0)  
)  
AS  
  
Declare @serialStr nvarchar(20) = CAST(@SerialNumber as nvarchar(20))  

   IF exists (select 1 from Batch b with (nolock) join DOT_FSItemMaster i with (nolock) on b.GloveType = i.ItemId 
		 where b.SerialNumber = @SerialNumber and i.ItemType = 109) -- surgical glove
BEGIN  
       SELECT 1  
END  
ELSE  
BEGIN  
       SELECT 0  
END  
GO

  
  
ALTER PROCEDURE [dbo].[usp_FP_SELECT_ReprintOuterCasePOList]  
(  
	@ConfiguredDayRange INT
)  
AS  
BEGIN  
  
 /*  
  Author  : <Fusionex, SoonSiang>  
  Create date : <23/01/2019>  
  Description : <Get PO List for Reprint Outer Case>  
 */  
  
 SET NOCOUNT ON;  
  
 DECLARE @CurrentDate Date = GETDATE()  
 DECLARE @FirstValidDate Date = DATEADD(d,(1 - @ConfiguredDayRange ),@CurrentDate)  
  
 SELECT vw.SalesId AS PONumber, vw.customerref AS CustomerReferenceNumber  
 FROM PurchaseOrderItem poi  
 INNER JOIN VW_Reprint_AXSOline vw  
 ON poi.PONumber = vw.SalesId  
 AND poi.ItemNumber = vw.ItemId  
 AND poi.ItemSize = vw.HARTALEGACOMMONSIZE  
 INNER JOIN SalesTable s  
 ON s.SalesId = vw.SalesId  
 LEFT JOIN PurchaseOrder po  
 ON po.PONumber = vw.SalesId  
 WHERE  (s.WorkOrderStatus IN (1,2) OR (s.WorkOrderStatus = 3 AND CAST(po.LastModifiedOn AS DATE) BETWEEN @FirstValidDate and @CurrentDate))  
 AND s.WorkOrderType <> 3  
 --GROUP BY vw.SalesId, vw.customerref  

END  
  
GO

USE [FloorSystem]
GO
/****** Object:  StoredProcedure [dbo].[USP_GET_GloveDescription]    Script Date: 31/3/2022 6:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================

-- Name:			[USP_GET_GloveDescription]

-- Purpose: 		Gets Desc by GloveType

-- =======================================================

-- Change History

-- Date         Author     Comments

-- -----        ------     -----------------------------

-- 28/05/2018 	Azrul	   SP altered.
-- =======================================================

CREATE OR ALTER PROCEDURE [dbo].[USP_GET_GloveDescription]
(
                @GloveType NVARCHAR(100)
)
AS
BEGIN
        IF ISNUMERIC(@GloveType) = 1
                        SELECT b.Name from DOT_FSGloveCode a join DOT_FSItemMaster b on a.ItemRecordId = b.Id where a.Barcode = @GloveType
        ELSE
                        SELECT b.Name from DOT_FSGloveCode a join DOT_FSItemMaster b on a.ItemRecordId = b.Id where b.ItemId = @GloveType
END

GO
-- =====================================
-- Author:  <Azrul Amin>  
-- Create date: <02-Feb-2018>  
-- Description: <Insert_DOT_RAFSTGTABLE>  
-- =====================================
CREATE PROCEDURE [dbo].[USP_DOT_CreateRAFJournal]  
		@BATCHORDERNUMBER nvarchar(20),  
		@BATCHWT numeric(32,6),  
		@CONFIGURATION nvarchar(10),  
		@ITEMNUMBER nvarchar(40),  
		@PARENTREFRECID int,
		@POSTINGDATETIME datetime, 
		@QCTYPE nvarchar(15), 
		@RAFGOODQTY numeric(32,6),   
		@RAFHBSAMPLE numeric(32,6),    
		@RAFVTSAMPLE numeric(32,6),
		@RAFWTSAMPLE numeric(32,6),
		@RESOURCE nvarchar(10),  
		@SHIFT nvarchar(5),  
		@TENPCSWT numeric(32,6),  
		@WAREHOUSE nvarchar(10),
		@LOCATION nvarchar(10),
		@RejectedQuantity numeric(32,6),
		@SecondGradeQuantity numeric(32,6),
		@RejectedSampleQuantity numeric(32,6),
		@QUANTITY numeric(32,6),
		@CHANGEDITEMNUMBER nvarchar(40)
AS  
BEGIN                                                                                                                                                                                                                       
 SET NOCOUNT ON;  
 DECLARE @RouteCategory nvarchar(10)
 SELECT  @RouteCategory = RouteCategory FROM dbo.DOT_FSQCTypeTable WITH (NOLOCK) WHERE QCTYPE = @QCTYPE


 
 DECLARE @BATCHNUMBER nvarchar(20),@Sequence int
 SELECT @BATCHNUMBER = BatchNumber,@Sequence=[Sequence] from DOT_FloorAxIntParentTable WITH (NOLOCK) where id = @PARENTREFRECID

 -- Max He 08-12-2020: not require based on Batch type
--#BUG 1166 - AZRUL 26-9-2018: PWT always PT & PVT always OQC, this will overwrite route category based on QC Type START 
 --DECLARE @BatchType nvarchar(10)
 --SELECT @BatchType = Substring(FunctionIdentifier,1,3) from DOT_FloorAxIntParentTable WITH (NOLOCK) where BatchNumber = @BATCHNUMBER AND [Sequence] = 1
 -- Max He 08-12-2020: not require based on Batch type

  --Max He 08-12-2020: check if previous staging function is RWKCR, then follow previous rework order route 
 DECLARE @PerviousFunctionIdentifier nvarchar(50),@PerviousParentId int
 SELECT @PerviousFunctionIdentifier = FunctionIdentifier,@PerviousParentId=Id from DOT_FloorAxIntParentTable where BatchNumber = @BATCHNUMBER AND [Sequence] = @Sequence -1

  -- Azrul 20201020: SRBC default to PT
  IF EXISTS (select 1 from DOT_FloorD365HRGLOVERPT a with (nolock) join DOT_FloorD365BO b with (nolock) on a.BthOrder = b.BthOrderId  
   where a.serialNo = @BATCHNUMBER and b.prodPoolId = 'SGR' and a.IsDeleted = 0 and b.IsDeleted=0)
  BEGIN
   IF NOT EXISTS (select 1 from AXPostingLog with (nolock) where serialnumber = @BATCHNUMBER and Servicename = 'SOBC')
	BEGIN
		SET @RouteCategory = 'PT'
	END
  END

  --Max He 08-12-2020: check if previous staging function is RWKCR, then follow previous rework order route 
  IF @PerviousFunctionIdentifier = 'RWKCR'
  BEGIN
	select @RouteCategory =RouteCategory from DOT_RwkBatchOrderCreationChildTable with(nolock) where ParentRefRecId=@PerviousParentId;
  END
  -- Max He 08-12-2020: not require based on Batch type

 --IF @BatchType = 'PWT'
 --BEGIN
	--SET @RouteCategory = 'PT'
 --END
 --IF @BatchType = 'PVT'
 --BEGIN
	--SET @RouteCategory = 'OQC'
 --END
 --#BUG 1166 - AZRUL 26-9-2018: PWT always PT & PVT always OQC, this will overwrite route category based on QC Type END

 -- Max He 08-12-2020: not require based on Batch type

 --Amir - 1/1/2022: water tight(WT) route must always be PT
  DECLARE @BatchType nvarchar(10)
  select @BatchType = BatchType from batch where SerialNumber =@BATCHNUMBER
  IF (@BatchType = 'PWT' or @BatchType = 'QWT' or @BatchType = 'OWT' or @BatchType = 'PSW')
   BEGIN
    SET @RouteCategory = 'PT'
   END


 INSERT INTO [dbo].[DOT_RAFSTGTABLE]  
           ([BATCHORDERNUMBER]
		   ,[BatchWeight] --[BATCHWT]
           ,[CONFIGURATION]  
           ,[CREATIONTIME]  
		   ,[CREATORUSERID]
		   ,[DELETERUSERID]
		   ,[DELETIONTIME]
           ,[HBBATCHNUMBER]  
		   ,[ISDELETED]  
           ,[ITEMNUMBER]  
		   ,[LASTMODIFICATIONTIME]
		   ,[LASTMODIFIERUSERID]
		   ,[PARENTREFRECID]
		   ,[PickListJournalId] --[PICKINGLISTJOURNAL]
           ,[POSTINGDATETIME]  
           ,[QCTYPE]  
           ,[RAFGOODQTY]  
           ,[RAFHBSAMPLE]  
           ,[RAFVTSAMPLE]  
           ,[RAFWTSAMPLE]       
		   ,[RAFJournalId] --[REPORTASFINISHEDJOURNAL]
		   ,[RESOURCE]
		   ,[RouteCardJournalId] --[ROUTEJOURNAL]
		   ,[SAMPLEWAREHOUSE]
           ,[SHIFT]  
           ,[Weightof10Pcs] --[TENPCSWT] 
           ,[VTBATCHNUMBER]  
           ,[WTBATCHNUMBER]  
           ,[WAREHOUSE]
		   ,[LOCATION]
		   ,[RejectedQuantity]
		   ,[SecondGradeQuantity]
		   ,[RejectedSampleQuantity]
		   ,[Quantity]
		   ,[RouteCategory]
		   ,[ChangedItemNumber])  
     VALUES  
          (@BATCHORDERNUMBER,
		   @BATCHWT,  
           @CONFIGURATION,   
		   GETDATE(),  
		   1,
		   NULL,
		   NULL,
		   '',
		   0,
		   @ITEMNUMBER,
		   GETDATE(),
		   1,
		   @PARENTREFRECID,
		   '',
		   @POSTINGDATETIME,
		   @QCTYPE,
		   @RAFGOODQTY,
		   @RAFHBSAMPLE,
		   @RAFVTSAMPLE,
		   @RAFWTSAMPLE,
		   '',
		   @RESOURCE,
		   '',
		   '',
		   @SHIFT,
		   @TENPCSWT,
		   '',
		   '',
           @WAREHOUSE,
		   @LOCATION,
		   @RejectedQuantity,
           @SecondGradeQuantity,
           @RejectedSampleQuantity,
		   @QUANTITY,
		   @RouteCategory,
		   @CHANGEDITEMNUMBER)  
END  

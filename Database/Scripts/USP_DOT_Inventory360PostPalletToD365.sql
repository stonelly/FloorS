
-- =====================================================      
-- Author:  <Max He>        
-- Create date: <25-11-2021>        
-- Description: <USP_DOT_Inventory360PostPalletToD365>
-- exec USP_DOT_Inventory360PostPalletToD365 'N1000152','P7','2020-11-15 18:48:17.017',1
-- =====================================================      
Create PROCEDURE [dbo].[USP_DOT_Inventory360PostPalletToD365]
    @PalletID VARCHAR(50),        
    @PlantNo VARCHAR (5),        
    @DateScanned DATETIME,  
	@Return int output        
AS        
BEGIN        
SET @Return=0;  
BEGIN TRANSACTION;        
  BEGIN TRY        
    SET NOCOUNT ON;    
    DECLARE @InternalLotNumber NVARCHAR(30)        
	DECLARE @DateStockOut DATETIME        
	DECLARE @Size NVARCHAR(20)    
    /** DOT_FLOORAXINTPARENTTABLE parameter **/        
    DECLARE @BatchCardNumber NVARCHAR(50)        
    DECLARE @BatchNumber NVARCHAR(20)        
    DECLARE @FSIdentifier UNIQUEIDENTIFIER        
    DECLARE @FunctionIdentifier NVARCHAR(20)        
    DECLARE @ReferenceBatchNumber1 NVARCHAR(20) = NULL        
    DECLARE @ReferenceBatchNumber2 NVARCHAR(20) = NULL        
    DECLARE @ReferenceBatchNumber3 NVARCHAR(20) = NULL        
    DECLARE @ReferenceBatchNumber4 NVARCHAR(20) = NULL        
    DECLARE @ReferenceBatchNumber5 NVARCHAR(20) = NULL        
    DECLARE @ReferenceBatchSequence1 INT = 0        
    DECLARE @ReferenceBatchSequence2 INT = 0        
    DECLARE @ReferenceBatchSequence3 INT = 0        
    DECLARE @ReferenceBatchSequence4 INT = 0        
    DECLARE @ReferenceBatchSequence5 INT = 0        
    DECLARE @Sequence INT        
    /** DOT_FGJournalTable parameter **/        
    DECLARE @BatchOrderNumber NVARCHAR(80)        
    DECLARE @ReferenceItemNumber NVARCHAR (40)        
    DECLARE @Configuration NVARCHAR(20)        
    DECLARE @Warehouse NVARCHAR (5) = 'FG'        
    DECLARE @Resource NVARCHAR(20)        
    DECLARE @CustomerPO NVARCHAR(40)        
    DECLARE @CustomerReference NVARCHAR(60)      
    DECLARE @SalesOrdernumber NVARCHAR(20)        
    DECLARE @InnerLotNumber NVARCHAR(90)        
    DECLARE @OuterLotNumber NVARCHAR(90)        
    DECLARE @CustomerLotNumber NVARCHAR(90)        
    DECLARE @Preshipment INT        
    DECLARE @Preshipmentcases INT        
    DECLARE @InnerBoxCapacity INT        
    DECLARE @PostingDateTime DATETIME           
	DECLARE @EWNQty INT   --#EWN qty validation with staging  
	DECLARE @AccQty INT = 0  --#EWN qty validation with staging  
    DECLARE @Quantity INT     
    DECLARE @TotalQuantity DECIMAL         --splitBatch  
    DECLARE @ParentRefRecId INT          
    DECLARE @Location NVARCHAR(20) = ''        
    DECLARE @PalletNumber NVARCHAR(20)        
    DECLARE @ExpiryDate DATETIME        
    DECLARE @Manufacturingdate DATETIME        
    DECLARE @IsWTS BIT        
    DECLARE @ItemNumber NVARCHAR (40) = NULL        
    DECLARE @CreateDateTime DATETIME        
    DECLARE @PONumber NVARCHAR(20)        
    DECLARE @PreshipmentCaseCount INT        
    DECLARE @RefNumberOfPieces1 DECIMAL (10, 2) = 0 --For SMBP        
    DECLARE @RefNumberOfPieces2 DECIMAL (10, 2) = 0 --For SMBP        
    DECLARE @RefNumberOfPieces3 DECIMAL (10, 2) = 0 --For SMBP        
    DECLARE @RefNumberOfPieces4 DECIMAL (10, 2) = 0 --For SMBP        
    DECLARE @RefNumberOfPieces5 DECIMAL (10, 2) = 0 --For SMBP        
    DECLARE @RefItemNumber1 NVARCHAR (40) = NULL --For SMBP        
    DECLARE @RefItemNumber2 NVARCHAR (40) = NULL --For SMBP        
    DECLARE @RefItemNumber3 NVARCHAR (40) = NULL --For SMBP        
    DECLARE @RefItemNumber4 NVARCHAR (40) = NULL --For SMBP        
    DECLARE @RefItemNumber5 NVARCHAR (40) = NULL --For SMBP        
    /** AXPostingLog parameter **/        
    DECLARE @PostingType NVARCHAR(20) = 'DOTFGJournalContract'        
    DECLARE @SerialNumber NVARCHAR(50)        
    DECLARE @IsPostedToAX BIT = 1        
    DECLARE @IsPostedInAX BIT = 1        
    DECLARE @ExceptionCode NVARCHAR(1000) = NULL        
    DECLARE @TransactionID NVARCHAR(100) = '-1'        
    DECLARE @Area NVARCHAR(10) = 'PS'        
    DECLARE @PalletSerialNo  UNIQUEIDENTIFIER    
    DECLARE @MaxLotCaseNumber int -- max caseNumber by internalLotNo  
    DECLARE @SPPBatchCardNumber NVARCHAR(50) = NULL --Surgical Packing Plan
    DECLARE @SPPBatchNumber NVARCHAR(50) = NULL		--Surgical Packing Plan
    DECLARE @PickingListQuantity INT = 0			--Surgical Packing Plan
    DECLARE @BatchSequence INT = 0					--Surgical Packing Plan
    DECLARE @GloveSize NVARCHAR(20) = NULL			--Surgical Packing Plan
    DECLARE @SumGloveSampleQuantity INT = 0			--Surgical Packing Plan
    DECLARE @GloveSampleQuantity INT = 0			--Surgical Packing Plan
    DECLARE @IsConsolidated BIT = 0					-- #AZRUL 17/9/2021: Open batch flag for NGC1.5
    DECLARE @IsConsolidatedMultipleBatch BIT = 0	-- #Max 27/10/2021: Detail batch check for Open batch flag for NGC1.5
  
 set @PalletSerialNo = NEWID(); -- Generate by pallet    
 if isnull(@PalletID,'')=''      
 BEGIN      
   RAISERROR ('Pallet ID is empty.', -- Message text.        
               16, -- Severity.        
               1 -- State.        
               );        
 END       
    
 if not exists(select 1 from palletmaster WITH (NOLOCK) where PalletId = @PalletID)    
  BEGIN      
   RAISERROR ('Pallet ID is not exists.', -- Message text.        
               16, -- Severity.        
               1 -- State.        
               );        
 END     
    
 /** Set additional parameter for filtering**/    
 Select @PONumber = PONumber FROM PalletMaster WITH (NOLOCK) where PalletId = @PalletID  
 print @ponumber  
 select @ItemNumber = SUBSTRING(Item,0,CHARINDEX('_',Item,0)),                         
     @Size =  SUBSTRING(Item, CHARINDEX('_', Item) + 1, LEN(Item))
 from EWN_CompletedPallet WITH (NOLOCK) WHERE PalletId = @PalletID and DateStockOut is null and PONumber = @PONumber  
 select @EWNQty = Sum(Qty) from EWN_CompletedPallet WITH (NOLOCK) WHERE PalletId = @PalletID and DateStockOut is null and PONumber = @PONumber --#EWN qty validation with staging
 select top 1 @Preshipment = isPreshipmentCase from PurchaseOrderItemCases WITH (NOLOCK) where palletid = @PalletID and ItemNumber = @ItemNumber and PONumber = @PONumber and size = @Size       
 SELECT @DateStockOut = ISNULL(MAX(DateStockOut),GETDATE()) FROM EWN_CompletedPallet WITH (NOLOCK) WHERE DateStockOut IS NOT NULL AND PalletId = @PalletID   and  PONumber = @PONumber       
        
    /** List all InternalLotNumber based on parameter when trigger executed **/        
 IF @Preshipment = 0 or EXISTS(select 1 FROM DOT_FSItemMaster WITH (NOLOCK) WHERE ItemId=@ItemNumber AND ItemType=8) --bypass for SPPBC
 BEGIN  
  SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS Id, fp.InternalotNumber as InternalLotNumber      
  INTO #temptable       
  FROM PurchaseOrderItemCases fp WITH (NOLOCK)       
  JOIN EWN_CompletedPallet ew WITH (NOLOCK) ON fp.PalletId = ew.PalletId AND fp.PONumber = @PONumber        
  WHERE fp.PalletId = @PalletID AND ew.DateCompleted < @DateScanned /*AND ew.DateCompleted> DATEADD(DAY, -30, @DateStockOut)*/ AND ew.DateStockOut IS NULL  --#AZ 12/2/2019 filtering based on PONumber from palletMaster not time duration  
  and fp.size = @Size      
  and fp.InternalotNumber not in (select InnerLotNumber from DOT_FGJournalTable WITH (NOLOCK) where Preshipment = 0 and PalletNumber = @PalletID) --#MH 17/8/2018 filter out by staging record      
  and fp.InternalotNumber in (select InternalotNumber from PurchaseOrderItemCases WITH (NOLOCK) where PalletId = @PalletID and size = @Size and itemnumber = @ItemNumber) --filter out InternalotNumber not exists in PurchaseOrderItemCases    
  group by fp.InternalotNumber      
  END    
 ELSE    
 BEGIN    
  SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS Id, fp.InternaLotNumber as InternalLotNumber  
  INTO #temptablePSI    
  FROM PurchaseOrderItemCases fp WITH (NOLOCK)       
  JOIN EWN_CompletedPallet ew WITH (NOLOCK) ON fp.PalletId = ew.PalletId AND fp.PONumber = @PONumber        
  and fp.Size = SUBSTRING(ew.Item, CHARINDEX('_', ew.Item) + 1, LEN(ew.Item))         --#AZ 7/9/2018 PSI can have multiple size in 1 pallet    
  WHERE fp.PalletId = @PalletID AND ew.DateStockOut IS NULL     
  AND ew.DateCompleted < @DateScanned -- AND ew.DateCompleted > DATEADD(DAY, -2, @DateStockOut)    --#AZ 12/2/2019 filtering based on PONumber from palletMaster not time duration  
  --and fp.size = @Size                      --#AZ 7/9/2018 PSI can have multiple size in 1 pallet    
  and fp.InternalotNumber not in (select InnerLotNumber from DOT_FGJournalTable WITH (NOLOCK) where Preshipment = 1 and PalletNumber = @PalletID) --#MH 17/8/2018 filter out by staging record    
  and isPreshipmentCase > 0                    --#MH 29/01/2019 PSI can have multiple size in 1 pallet    
  and fp.InternaLotNumber in (select InternalotNumber from PurchaseOrderItemCases WITH (NOLOCK) where PalletId = @PalletID /*and size = @Size*/ and itemnumber = @ItemNumber) --filter out InternalotNumber not exists in PurchaseOrderItemCases               
           
  group by fp.InternalotNumber;  
  
 -- To detect is the last Preshipment case  
 select count(itemid) as SOLineCount,HartalegaCommonSize,ItemId  
 into #tempSOLineCount  
 from DOT_FloorSalesLine with(nolock)   
 where IsDeleted=0 and salesid = @PONumber  
 group by HartalegaCommonSize,ItemId;  
  
 select count(ItemNumber) as POLineCount,ItemSize,ItemNumber  
 into #tempPOlineCount  
 from purchaseorderitem with(nolock)  
 where POnumber = @PONumber  
 group by ItemSize,ItemNumber  
  
   
 select PONumber,size, itemNumber,count(1) Qty,max(CaseNumber) as MaxCaseNumber  
 INTO #tempPalletMaxCase  
 from PurchaseOrderItemCases with(nolock)  
 where PONumber=@PONumber and ispreshipmentCase = 1  
 group by PONumber,size, itemNumber;  
  
 --select * from #tempPalletMaxCase  
 END    
       
    /** Looping each InternalLotNumber for posting **/        
 DECLARE @COUNT INT    
 IF @Preshipment = 0 or EXISTS(select 1 FROM DOT_FSItemMaster WITH (NOLOCK) WHERE ItemId=@ItemNumber AND ItemType=8) --bypass for SPPBC
 BEGIN  
  SET @COUNT = (SELECT MAX(Id) FROM #temptable);        
 END    
 ELSE    
 BEGIN    
  SET @COUNT = (SELECT MAX(Id) FROM #temptablePSI);        
 END    
    
DECLARE @ROW INT = 1;        
WHILE (@ROW <= @COUNT)        
BEGIN    
      
  --print @ROW      
  --clean up    
  if OBJECT_ID('tempdb.dbo.#finalpackingtemp') IS NOT NULL       
  BEGIN      
   DROP TABLE #finalpackingtemp;        
  --print 'drop #finalpackingtemp'      
  END      
  if OBJECT_ID('tempdb.dbo.#finalpackingSMBPtemp') IS NOT NULL       
  BEGIN      
   DROP TABLE #finalpackingSMBPtemp;        
  --print 'drop #finalpackingSMBPtemp'      
  END      
  if OBJECT_ID('tempdb.dbo.#finalpackingSPPBCtemp') IS NOT NULL --Surgical Packing Plan
  BEGIN															--Surgical Packing Plan
   DROP TABLE #finalpackingSPPBCtemp;      						--Surgical Packing Plan
  END  															--Surgical Packing Plan
     --print 'get internal lot number'      

  IF @Preshipment = 0 or EXISTS(select 1 FROM DOT_FSItemMaster WITH (NOLOCK) WHERE ItemId=@ItemNumber AND ItemType=8) --bypass for SPPBC
  BEGIN  
   SET @InternalLotNumber = (SELECT InternalLotNumber FROM #temptable WHERE Id = @ROW);        
  END    
  ELSE    
  BEGIN    
    SET @InternalLotNumber = (SELECT InternalLotNumber FROM #temptablePSI WHERE Id = @ROW);      
  END      
  print @InternalLotNumber      
      
  SELECT * INTO #finalpackingtemp FROM FinalPacking WITH (NOLOCK)       
     WHERE InternalLotNumber = @InternalLotNumber;        
    --print '97'      
      
  set @BatchNumber='0';      
  SELECT @BatchNumber = ISNULL(SerialNumber,'0') FROM #finalpackingtemp;       
         
  --print '99'      
  SET @FSIdentifier = NEWID();        
        
  /** If FunctionID: SBC        
     FloorSystem Method: AXPostingBLL.GetCompleteFinalPackingDetails(internalLotNumber)         
     EXEC USP_FP_Get_ScanBatchCardInnerOuterforPosting @InternalLotNumber **/       
  --print @BatchNumber      
  IF (@BatchNumber <> '0')        
  BEGIN        
  SET @FunctionIdentifier = 'SBC'        
  --print '108'      
  --print @FunctionIdentifier     
  IF @Preshipment = 0    
  BEGIN     
   SELECT DISTINCT @BatchNumber = FPB.SerialNumber,        
     @BatchCardNumber = BT.BatchNumber,        
     @BatchOrderNumber = FP.FGBatchOrderNo,        
     @Configuration = POIN.ItemSize,        
     @Resource = FP.Resource,  
     @CustomerPO = POIN.OrderNumber,        
     @CustomerReference = POIN.CustomerReferenceNumber,        
     @InnerLotNumber = FP.InternalLotNumber,        
     @OuterLotNumber = FP.OuterLotno,        
     @CustomerLotNumber = POIN.CustomerLotNumber,        
     @Manufacturingdate = FP.ManufacturingDate,        
     @ExpiryDate = FP.ExpiryDate,        
     @PalletNumber = @PalletID,        
     @CreateDateTime = BT.BatchCardDate,        
     @PostingDateTime = @DateScanned,        
     @Quantity = CT.CasesPacked,        
     @ReferenceItemNumber = FP.ItemNumber,        
     @SalesOrdernumber = FP.PONumber,        
     @PONumber = FP.PONumber,        
     @SerialNumber = FPB.SerialNumber,    
     @PreshipmentCases = 0 -- Normal Pallet always set to 0        
   FROM #finalpackingtemp FP        
   JOIN FinalPackingbatchinfo FPB WITH (NOLOCK) ON FPB.InternalLotNumber = FP.InternalLotNumber        
   JOIN PurchaseOrderItem POIN WITH (NOLOCK) ON POIN.PONumber = FP.PONumber AND POIN.ItemNumber = FP.ItemNumber AND FP.Size = POIN.ItemSize        
   JOIN Batch BT WITH (NOLOCK) ON BT.SerialNumber = FP.SerialNumber     
   JOIN (select count(CaseNumber) as CasesPacked,isPreshipmentCase,PONumber,InternalotNumber,palletid from PurchaseOrderItemCases WITH (NOLOCK)     
     where InternalotNumber = @InternalLotNumber and palletid=@PalletID     
     group by isPreshipmentCase,PONumber,InternalotNumber,palletid) CT on ct.InternalotNumber = FP.InternalLotNumber       
   WHERE FP.InternalLotNumber = @InternalLotNumber    
  
  END    
  ELSE  -- else is preshipment pallet  
  BEGIN    
  
  -- Get Max case number by internal lot no  
  select @MaxLotCaseNumber=max(CaseNumber)  
   from PurchaseOrderItemCases  WITH (NOLOCK)  
   where InternalotNumber=@InternalLotNumber and ispreshipmentCase = 1  
   group by InternalotNumber;  
  print @MaxLotCaseNumber    
  
   SELECT DISTINCT @BatchNumber = FPB.SerialNumber,        
     @BatchCardNumber = BT.BatchNumber,           
     @BatchOrderNumber = FP.FGBatchOrderNo,        
     @Configuration = POIN.ItemSize,        
     @Resource = FP.Resource,        
     @CustomerPO = POIN.OrderNumber,        
     @CustomerReference = POIN.CustomerReferenceNumber,        
     @InnerLotNumber = FP.InternalLotNumber,        
     @OuterLotNumber = FP.OuterLotno,        
     @CustomerLotNumber = POIN.CustomerLotNumber,        
     @Manufacturingdate = FP.ManufacturingDate,        
     @ExpiryDate = FP.ExpiryDate,        
     @PalletNumber = @PalletID,        
     @CreateDateTime = BT.BatchCardDate,        
     @PostingDateTime = @DateScanned,        
     @Quantity = CT.CasesPacked,        
     @ReferenceItemNumber = FP.ItemNumber,        
     @SalesOrdernumber = FP.PONumber,        
     @PONumber = FP.PONumber,        
     @SerialNumber = FPB.SerialNumber,    
     @PreshipmentCases = CASE WHEN soc.SOLineCount=poc.POLineCount and isnull(packedCount.SONotPacked,0) = 0 and psiMax.MaxCaseNumber=@MaxLotCaseNumber THEN psiMax.Qty ELSE 0 END     
   FROM #finalpackingtemp FP        
   JOIN FinalPackingbatchinfo FPB WITH (NOLOCK) ON FPB.InternalLotNumber = FP.InternalLotNumber        
   JOIN PurchaseOrderItem POIN WITH (NOLOCK) ON POIN.PONumber = FP.PONumber AND POIN.ItemNumber = FP.ItemNumber AND FP.Size = POIN.ItemSize        
   JOIN Batch BT WITH (NOLOCK) ON BT.SerialNumber = FP.SerialNumber    
   JOIN (select count(CaseNumber) as CasesPacked,InternalotNumber  
     from PurchaseOrderItemCases WITH (NOLOCK)     
     where InternalotNumber = @InternalLotNumber and palletid=@PalletID     
     group by InternalotNumber) CT on ct.InternalotNumber = FP.InternalLotNumber     
    
  -- PSI cases handing logic  
   -- calculate how many PSI packed for this FG & size  
   JOIN #tempPalletMaxCase psiMax on psiMax.ItemNumber=FP.ItemNumber AND psiMax.Size=FP.Size and psiMax.PONumber=FP.PONumber  
   -- detect is fully packed for this FG & size  
   join #tempSOLineCount soc on soc.HartalegaCommonSize=fp.Size and soc.ItemId= fp.ItemNumber   
   join #tempPOlineCount poc on poc.ItemSize=fp.Size and poc.ItemNumber= fp.ItemNumber --filter correct FG item number  
   left join (select COUNT(casenumber) as SONotPacked,ponumber,Size,ItemNumber --filter correct FG item number  
   from purchaseorderitemcases with(nolock)  
   where ispreshipmentCase = 1 and InternalotNumber is null and Palletid is null  
   group by ponumber,Size,ItemNumber) packedCount  
  on packedcount.PONumber = FP.PONumber and packedcount.Size = fp.Size and packedCount.ItemNumber=fp.ItemNumber --filter correct FG item number  
   WHERE FP.InternalLotNumber = @InternalLotNumber    
  END        
  if isnull(@BatchCardNumber,'')=''      
  BEGIN      
   RAISERROR ('FG Batch Order is ammended or deleted', -- Message text.        
   16, -- Severity.        
   1 -- State.        
   );        
  END    
    
   set @PreshipmentCaseCount = 0    
   SELECT @PreshipmentCaseCount = Count(CaseNumber) FROM PurchaseOrderItemCases WITH (NOLOCK) WHERE PONumber = @Ponumber AND ItemNumber = @ReferenceItemNumber        
   AND Size = @Configuration AND IsPreshipmentCase = 1 AND InternaLotNumber IS NULL AND PalletId IS NULL        
  END        
  ELSE        
  BEGIN        
  IF EXISTS(select * FROM DOT_FSItemMaster WITH (NOLOCK) WHERE ItemId=@ItemNumber AND ItemType=8) --Surgical Packing Plan
  BEGIN																				--Surgical Packing Plan
	SET @FunctionIdentifier = 'SPPBC';  											--Surgical Packing Plan
  END																				--Surgical Packing Plan
  ELSE
  BEGIN														
	SET @FunctionIdentifier = 'SMBP';
  END  
         
   print @FunctionIdentifier      
      
   SET @BatchNumber = '';        
   SET @BatchCardNumber = '';  
  IF @Preshipment = 0 or @FunctionIdentifier = 'SPPBC' --bypass for SPPBC  
  BEGIN   
   SELECT TOP 1 @Configuration = PO.ItemSize,        
       @Resource = FP.Resource,        
       @CustomerPO = PO.OrderNumber,        
       @CustomerReference = PO.CustomerReferenceNumber,        
       @InnerLotNumber = FP.InternalLotNumber,        
       @OuterLotNumber = FP.OuterLotno,        
       @CustomerLotNumber = PO.CustomerLotNumber,        
       @Manufacturingdate = FP.ManufacturingDate,        
       @ExpiryDate = FP.ExpiryDate,        
       @PalletNumber = @PalletID,        
       @BatchOrderNumber = FP.FGBatchOrderNo,     
       @PostingDateTime = @DateScanned,        
       @PreshipmentCases = 0, -- Normal Pallet always set to 0        
       @Quantity = CT.CasesPacked,        
       @TotalQuantity = CT2.TotalCases,  --splitBatch  
       @ReferenceItemNumber = FP.ItemNumber,        
       @SalesOrdernumber = FP.PONumber,        
       @InnerBoxCapacity = PO.innerboxcapacity        
   FROM #finalpackingtemp FP        
   JOIN PurchaseOrderItem PO WITH (NOLOCK) ON PO.PONumber = FP.PONumber AND PO.ItemNumber = FP.ItemNumber AND FP.Size = PO.ItemSize  
   JOIN (select count(CaseNumber) as CasesPacked,InternalotNumber   
  from PurchaseOrderItemCases WITH (NOLOCK)     
  where InternalotNumber = @InternalLotNumber and palletid=@PalletID     
  group by InternalotNumber) CT on ct.InternalotNumber = FP.InternalLotNumber    
 JOIN (select count(CaseNumber) as TotalCases,InternalotNumber --splitBatch  
  from PurchaseOrderItemCases WITH (NOLOCK)     
  where InternalotNumber = @InternalLotNumber   
  group by InternalotNumber) CT2 on ct.InternalotNumber = FP.InternalLotNumber    
   WHERE FP.InternalLotNumber = @InternalLotNumber     
  END  
  ELSE  
  BEGIN  
   -- Get Max case number by internal lot no  
   select @MaxLotCaseNumber=max(CaseNumber)  
    from PurchaseOrderItemCases  WITH (NOLOCK)  
    where InternalotNumber=@InternalLotNumber and ispreshipmentCase = 1  
    group by InternalotNumber;  
   print @MaxLotCaseNumber    
  
     SELECT TOP 1 @Configuration = PO.ItemSize,        
       @Resource = FP.Resource,        
       @CustomerPO = PO.OrderNumber,        
       @CustomerReference = PO.CustomerReferenceNumber,        
       @InnerLotNumber = FP.InternalLotNumber,        
       @OuterLotNumber = FP.OuterLotno,        
       @CustomerLotNumber = PO.CustomerLotNumber,        
       @Manufacturingdate = FP.ManufacturingDate,        
       @ExpiryDate = FP.ExpiryDate,        
       @PalletNumber = @PalletID,        
       @BatchOrderNumber = FP.FGBatchOrderNo,    
       @PostingDateTime = @DateScanned,        
       @PreshipmentCases = CASE WHEN soc.SOLineCount=poc.POLineCount and isnull(packedCount.SONotPacked,0) = 0 and psiMax.MaxCaseNumber=@MaxLotCaseNumber THEN psiMax.Qty ELSE 0 END,        
       @Quantity = CT.CasesPacked,  
       @TotalQuantity = CT2.TotalCases,  --splitBatch  
       @ReferenceItemNumber = FP.ItemNumber,        
       @SalesOrdernumber = FP.PONumber,        
       @InnerBoxCapacity = PO.innerboxcapacity        
   FROM #finalpackingtemp FP        
   JOIN PurchaseOrderItem PO WITH (NOLOCK) ON PO.PONumber = FP.PONumber AND PO.ItemNumber = FP.ItemNumber AND FP.Size = PO.ItemSize  
   JOIN (select count(CaseNumber) as CasesPacked,InternalotNumber   
  from PurchaseOrderItemCases WITH (NOLOCK)     
  where InternalotNumber = @InternalLotNumber and palletid=@PalletID     
  group by InternalotNumber) CT on ct.InternalotNumber = FP.InternalLotNumber    
 JOIN (select count(CaseNumber) as TotalCases,InternalotNumber  --splitBatch  
  from PurchaseOrderItemCases WITH (NOLOCK)     
  where InternalotNumber = @InternalLotNumber   
  group by InternalotNumber) CT2 on ct.InternalotNumber = FP.InternalLotNumber    
   
  -- PSI cases handing logic  
   -- calculate how many PSI packed for this FG & size  
   JOIN #tempPalletMaxCase psiMax on psiMax.ItemNumber=FP.ItemNumber AND psiMax.Size=FP.Size and psiMax.PONumber=FP.PONumber  
   -- detect is fully packed for this FG & size  
   join #tempSOLineCount soc on soc.HartalegaCommonSize=fp.Size and soc.ItemId= fp.ItemNumber   
   join #tempPOlineCount poc on poc.ItemSize=fp.Size and poc.ItemNumber= fp.ItemNumber  
   left join (select COUNT(casenumber) as SONotPacked,ponumber,Size,ItemNumber  
   from purchaseorderitemcases with(nolock)  
   where ispreshipmentCase = 1 and InternalotNumber is null and Palletid is null  
   group by ponumber,Size,ItemNumber) packedCount  
  on packedcount.PONumber = FP.PONumber and packedcount.Size = fp.Size and packedCount.ItemNumber=fp.ItemNumber  
   WHERE FP.InternalLotNumber = @InternalLotNumber     
  
  END  
  
	IF @FunctionIdentifier <> 'SPPBC' --Surgical Packing Plan
	BEGIN
	   /** List all Reference Batch from InternalLotNumber **/        
	   /** FloorSystem Method: AXPostingBLL.GetScanMultipleBatchInfoforPosting(SerialNumber)        
	        EXEC USP_FP_Get_ScanMultipleBatchInfoforPosting @InternalLotNumber **/        
	   SELECT Row_number() OVER (ORDER BY (SELECT 1)) AS Id, BT.SerialNumber, BT.Size, BT.BatchNumber, FPB.BoxesPacked, BT.GloveType    
	   INTO #finalpackingSMBPtemp FROM Batch BT WITH (NOLOCK) INNER JOIN FinalPackingbatchinfo FPB WITH (NOLOCK) ON BT.SerialNumber = FPB.SerialNumber        
	   WHERE FPB.InternalLotNumber = @InternalLotNumber        
	   --print '238'    
	   --Reference Batch 1      
	   SELECT @ReferenceBatchNumber1 = SerialNumber,      
	   @ReferenceBatchSequence1 = (SELECT Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber=a.SerialNumber),      
	   @RefItemNumber1 = GloveType,      
	   @RefNumberOfPieces1 = CASE WHEN @Quantity <> @TotalQuantity THEN ROUND(AVG(CAST(@InnerBoxCapacity * BoxesPacked * @Quantity / @TotalQuantity AS DECIMAL)) * 2, 0) / 2	 
								 ELSE @InnerBoxCapacity * BoxesPacked END --splitBatch
	   FROM #finalpackingSMBPtemp a WHERE Id = 1      
	   GROUP BY SerialNumber,GloveType,BoxesPacked
	   SET @SerialNumber = @ReferenceBatchNumber1 --Set for AXPostingLog      

	   --print '246'    
	   --Reference Batch 2      
	   SELECT @ReferenceBatchNumber2 = ISNULL(SerialNumber,''),      
	   @ReferenceBatchSequence2 = ISNULL((SELECT Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber=b.SerialNumber),0),      
	   @RefItemNumber2 = ISNULL(GloveType,0),      
	   @RefNumberOfPieces2 = CASE WHEN @Quantity <> @TotalQuantity AND @ReferenceBatchNumber2 <> '' THEN ROUND(AVG(CAST(@InnerBoxCapacity * BoxesPacked * @Quantity / @TotalQuantity AS DECIMAL)) * 2, 0) / 2	 
								 ELSE ISNULL(@InnerBoxCapacity * BoxesPacked,'') END --splitBatch
	   FROM #finalpackingSMBPtemp b WHERE Id = 2      
	   GROUP BY SerialNumber,GloveType,BoxesPacked
      
       --Reference Batch 3      
       SELECT @ReferenceBatchNumber3 = ISNULL(SerialNumber,''),      
       @ReferenceBatchSequence3 = ISNULL((SELECT Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber=c.SerialNumber),0),      
       @RefItemNumber3 = ISNULL(GloveType,0),      
       @RefNumberOfPieces3 = CASE WHEN @Quantity <> @TotalQuantity AND @ReferenceBatchNumber3 <> '' THEN ROUND(AVG(CAST(@InnerBoxCapacity * BoxesPacked * @Quantity / @TotalQuantity AS DECIMAL)) * 2, 0) / 2	 
								 ELSE ISNULL(@InnerBoxCapacity * BoxesPacked,'') END --splitBatch    
	   FROM #finalpackingSMBPtemp c WHERE Id = 3      
	   GROUP BY SerialNumber,GloveType,BoxesPacked
      
       --Reference Batch 4      
	   SELECT @ReferenceBatchNumber4 = ISNULL(SerialNumber,''),      
	   @ReferenceBatchSequence4 = ISNULL((SELECT Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber=d.SerialNumber),0),      
	   @RefItemNumber4 = ISNULL(GloveType,0),      
	   @RefNumberOfPieces4 = CASE WHEN @Quantity <> @TotalQuantity AND @ReferenceBatchNumber4 <> '' THEN ROUND(AVG(CAST(@InnerBoxCapacity * BoxesPacked * @Quantity / @TotalQuantity AS DECIMAL)) * 2, 0) / 2	 
								 ELSE ISNULL(@InnerBoxCapacity * BoxesPacked,'') END --splitBatch       
	   FROM #finalpackingSMBPtemp d WHERE Id = 4      
	   GROUP BY SerialNumber,GloveType,BoxesPacked
      
       --Reference Batch 5      
	   SELECT @ReferenceBatchNumber5 = ISNULL(SerialNumber,''),      
	   @ReferenceBatchSequence5 = ISNULL((SELECT Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber=e.SerialNumber),0),      
	   @RefItemNumber5 = ISNULL(GloveType,0),      
	   @RefNumberOfPieces5 = CASE WHEN @Quantity <> @TotalQuantity AND @ReferenceBatchNumber5 <> '' THEN ROUND(AVG(CAST(@InnerBoxCapacity * BoxesPacked * @Quantity / @TotalQuantity AS DECIMAL)) * 2, 0) / 2	 
								 ELSE ISNULL(@InnerBoxCapacity * BoxesPacked,'') END --splitBatch   
	   FROM #finalpackingSMBPtemp e WHERE Id = 5     
	   GROUP BY SerialNumber,GloveType,BoxesPacked 
     
       DROP TABLE #finalpackingSMBPtemp  
	 END
	 ELSE 
	 BEGIN
		SELECT Row_number() OVER (ORDER BY (SELECT 1)) AS Id, SPD.SerialNumber, SPP.ItemSize, SPD.GloveSize, SPD.BatchNumber, SPD.ReservedQty, SPD.GloveCode, SPP.SamplingPcsQty, SPP.RequiredPcsQty
		INTO #finalpackingSPPBCtemp FROM SurgicalPackingPlan SPP WITH (NOLOCK) JOIN SurgicalPackingPlanDetails SPD on SPP.SurgicalPackingPlanId = SPD.SurgicalPackingPlanId
		WHERE SPP.InternalLotNo = @InternalLotNumber AND SPP.PlanStatus = 3 --Surgical Packing Plan
		SELECT Top 1 @SumGloveSampleQuantity = SamplingPcsQty from #finalpackingSPPBCtemp
		SET @SumGloveSampleQuantity = ROUND(AVG(CAST(@Quantity AS DECIMAL) / @TotalQuantity * @SumGloveSampleQuantity), 0)
	 END

    END      
   -- print '277'      
    /** FloorSystem Method: AXPostingBLL.GetBatchSequence(SerialNumber)        
       EXEC USP_GET_BATCHSEQUENCE SerialNumber **/        
   if @FunctionIdentifier = 'SMBP' OR @FunctionIdentifier = 'SPPBC'
   BEGIN      
    SELECT @Sequence = 0-- Count(SerialNumber) + 1 FROM dbo.AXPostingLog WHERE SerialNumber = @ReferenceBatchNumber1       
   END      
   ELSE      
   BEGIN        
     SELECT @Sequence = Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber = @BatchNumber-- and ServiceName <> 'SMBP'        
   END      
   --print '281'      
    /** only MTS, @IsWTS is true **/        
    IF (@BatchOrderNumber = @SalesOrdernumber AND @CustomerLotNumber = '')        
    BEGIN        
   SET @IsWTS = 1        
    END        
    ELSE        
    BEGIN        
   SET @IsWTS = 0        
    END        
    
	/** Mantis# 0008589: Plant 7 Web Admin Surgical SPPBC does not generate quality order correctly **/
	/** For Surgical, no PSI cases defined in PurchaseOrderItemCases **/
	/** This part will detect last pallet defined in PurchaseOrderItemCases by PoNo and size **/
	/**	Then count PreshipmentCases from PurchaseOrderItem  by PoNo and ItemSize **/
	IF @FunctionIdentifier = 'SPPBC'
	BEGIN
		DECLARE @LastCasenumber INT
  		DECLARE @LastPalletId NVARCHAR(10)    
  		SELECT @LastCasenumber = Max(CaseNumber) FROM PurchaseOrderItemCases WITH (NOLOCK) where PONumber = @SALESORDERNUMBER and size = @Size --0008589   
		SELECT @LastPalletId = PalletId FROM PurchaseOrderItemCases WITH (NOLOCK) where PONumber = @SALESORDERNUMBER and CaseNumber = @LastCasenumber and size = @Size
		IF @LastPalletId = @PalletID AND @row = @COUNT
		BEGIN
			DECLARE @strCases NVARCHAR(4000)   --0008589
			SET @Preshipment=1
			SELECT @strCases = Preshipmentcases FROM PurchaseOrderItem WITH (NOLOCK) where PONumber = @SALESORDERNUMBER and ItemSize = @Size  --0008589
			SELECT @Preshipmentcases = SUM(len(@strCases) - len(replace(@strCases, ',', '')) +1)
		END
	END
	/** Mantis# 0008589: Plant 7 Web Admin Surgical SPPBC does not generate quality order correctly **/

	/** Open batch flag for NGC1.5 **/
	IF (@FunctionIdentifier = 'SBC')
	BEGIN
		IF (SELECT COUNT(1) FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @BATCHNUMBER AND IsConsolidated = 1) > 0
		BEGIN
			SET @IsConsolidated = 1
		END
	END
	ELSE IF (@FunctionIdentifier = 'SMBP')
	BEGIN
		IF (SELECT COUNT(1) FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @ReferenceBatchNumber1 AND IsConsolidated = 1) > 0
		BEGIN
			SET @IsConsolidated = 1
		END
	END
           
     /** AX Posting parent, child & AXPostingLog staging table        
      EXEC USP_DOT_CreateFLOORAXINTPARENTTABLE **/        
   BEGIN        
         
   INSERT INTO [dbo].[Dot_FloorAXIntParentTable]        
      ([BatchCardNumber]        
      ,[BatchNumber]        
      ,[CreationTime]        
      ,[CreatorUserId]        
      ,[DeleterUserId]        
      ,[DeletionTime]        
      ,[ErrorMessage]        
      ,[FSIdentifier]        
      ,[FunctionIdentifier]        
      ,[IsDeleted]        
      ,[LastModificationTime]        
      ,[LastModifierUserId]        
      ,[ProcessingStatus]        
      ,[PlantNo]        
      ,[ProdId]        
      ,[ReferenceBatchNumber1]        
      ,[ReferenceBatchNumber2]        
      ,[ReferenceBatchNumber3]        
      ,[ReferenceBatchNumber4]        
      ,[ReferenceBatchNumber5]        
      ,[ReferenceBatchSequence1]        
      ,[ReferenceBatchSequence2]        
      ,[ReferenceBatchSequence3]        
      ,[ReferenceBatchSequence4]        
      ,[ReferenceBatchSequence5]        
      ,[Sequence]        
      ,[PalletId]    
      ,[PalletSerialNo]
      ,[FGQuantity] 			--Surgical Packing Plan
      ,[Preshipment] 			--Surgical Packing Plan
      ,[PreshipmentCases] 		--Surgical Packing Plan
      ,[GloveSampleQuantity] 	--Surgical Packing Plan
      ,[IsConsolidated])		--NGC 1.5 Open Batch flag
    VALUES (@BATCHCARDNUMBER, @BATCHNUMBER, Getdate(), 1, NULL, NULL, '', @FSIDENTIFIER, @FUNCTIONIDENTIFIER, 0, Getdate(), 1, 1, @PLANTNO, NULL, @REFERENCEBATCHNUMBER1,        
    @REFERENCEBATCHNUMBER2, @REFERENCEBATCHNUMBER3, @REFERENCEBATCHNUMBER4, @REFERENCEBATCHNUMBER5, @REFERENCEBATCHSEQUENCE1, @REFERENCEBATCHSEQUENCE2,        
    @REFERENCEBATCHSEQUENCE3, @REFERENCEBATCHSEQUENCE4, @REFERENCEBATCHSEQUENCE5, @SEQUENCE, @PalletId, @PalletSerialNo,
    @QUANTITY, @PRESHIPMENT, @PRESHIPMENTCASES, @SumGloveSampleQuantity,  --Surgical Packing Plan      
    @IsConsolidated) --NGC 1.5 Open Batch flag
	
   SET @PARENTREFRECID = (SELECT @@IDENTITY);
   
   IF @FunctionIdentifier = 'SPPBC' --Surgical Packing Plan
   BEGIN			
   	DECLARE @SPPROW INT = 1;      																																			 
	DECLARE @SPPCOUNT INT;																																					 
   	DECLARE @ACCGloveSampleQuantity INT = 0																																		 
	SET @SPPCOUNT = (SELECT MAX(Id) FROM #finalpackingSPPBCtemp);  																											 
	WHILE (@SPPROW <= @SPPCOUNT)  																																			 
	BEGIN			
		Select @SPPBatchCardNumber = BatchNumber, @SPPBatchNumber = SerialNumber, @PickingListQuantity = ReservedQty , @Configuration = ItemSize,
		@GloveSize = GloveSize, @GloveSampleQuantity = ROUND(AVG(CAST(ReservedQty AS DECIMAL)/((RequiredPcsQty+SamplingPcsQty)/2))*(SamplingPcsQty/2), 0)
		FROM #finalpackingSPPBCtemp WITH (NOLOCK) where Id = @SPPROW
		group by BatchNumber,SerialNumber,ReservedQty,ItemSize,GloveSize,Id,SamplingPcsQty

		SET @Configuration = CASE
			WHEN @Configuration LIKE '%,%' THEN 
				LEFT(SUBSTRING(@Size, PATINDEX('%[0-9.-]%', @Configuration), 8000),
				PATINDEX('%[^0-9.-]%', SUBSTRING(@Configuration, PATINDEX('%[0-9.-]%', @Configuration), 8000) + 'X') -1) 
			ELSE @Configuration																				 
		END
		SELECT @BatchSequence = Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber = @SPPBatchNumber

		SET @GloveSampleQuantity = ROUND(AVG(CAST(@Quantity AS DECIMAL) / @TotalQuantity * @GloveSampleQuantity), 0)
		SET @PickingListQuantity = ROUND(AVG(CAST(@Quantity AS DECIMAL) / @TotalQuantity * @PickingListQuantity), 0) - @GloveSampleQuantity

		IF @SPPROW < @SPPCOUNT
		BEGIN
			SET @ACCGloveSampleQuantity = @ACCGloveSampleQuantity + @GloveSampleQuantity
		END
		ELSE
		BEGIN
			SET @GloveSampleQuantity = @SumGloveSampleQuantity - @ACCGloveSampleQuantity
   			-- after re-calc @GloveSampleQuantity need to re-calc @PickingListQuantity also, Max He 24/09/2021  
   			SET @PickingListQuantity = ROUND(AVG(CAST(@Quantity AS DECIMAL) / @TotalQuantity * @PickingListQuantity), 0) - @GloveSampleQuantity  
  		END  

		 -- check individual detail batch is it open batch or not
		IF (SELECT COUNT(1) FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @SPPBatchNumber AND IsConsolidated = 1) > 0
		BEGIN
			SET @IsConsolidatedMultipleBatch = 1;
		END
		/** EXEC USP_DOT_CreateFGRAFJournal SPPBC **/
		EXEC Usp_dot_createfgrafjournal @BATCHORDERNUMBER, @REFERENCEITEMNUMBER, @CONFIGURATION, @WAREHOUSE, @RESOURCE, @CUSTOMERPO, @CUSTOMERREFERENCE, @SALESORDERNUMBER,        
		@INNERLOTNUMBER, @OUTERLOTNUMBER, @CUSTOMERLOTNUMBER, @PRESHIPMENT, @PRESHIPMENTCASES, @POSTINGDATETIME, @QUANTITY, @PARENTREFRECID, @LOCATION, 
		@PALLETNUMBER, @ExpiryDate, @ManufacturingDate, @IsWTS, @ItemNumber, @RefNumberOfPieces1, @RefNumberOfPieces2,        
		@RefNumberOfPieces3, @RefNumberOfPieces4, @RefNumberOfPieces5, @RefItemNumber1, @RefItemNumber2, @RefItemNumber3, @RefItemNumber4,        
		@RefItemNumber5, @SPPBatchCardNumber, @SPPBatchNumber, @PickingListQuantity, @BatchSequence, @GloveSize, @GloveSampleQuantity;
		/** AXPostingLog SPPBC **/
		EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @SPPBatchNumber, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID, @Area, @IsConsolidatedMultipleBatch  
		
		-- As long as have 1 batch is new batch card set IsConsolidated = 1 then parent staging table consider as new batch and handle in WebAdmin2
		IF @IsConsolidated = 0 AND @IsConsolidatedMultipleBatch = 1
		BEGIN
			SET @IsConsolidated = 1;
		END
		SET @SPPROW = @SPPROW + 1	
	END	-- while loop end for insert SPPBC FG detail info 
	IF @IsConsolidated = 1 -- update parent table if has new batch card(consolidated=1) 
	BEGIN
		UPDATE Dot_FloorAXIntParentTable set IsConsolidated=@IsConsolidated where Id=@PARENTREFRECID;
	END
	
	DROP TABLE #finalpackingSPPBCtemp																																		 
   END
   ELSE
   BEGIN  
	   /** EXEC USP_DOT_CreateFGRAFJournal SBC/SMBP **/
	   EXEC Usp_dot_createfgrafjournal @BATCHORDERNUMBER, @REFERENCEITEMNUMBER, @CONFIGURATION, @WAREHOUSE, @RESOURCE, @CUSTOMERPO, @CUSTOMERREFERENCE, @SALESORDERNUMBER,        
		@INNERLOTNUMBER, @OUTERLOTNUMBER, @CUSTOMERLOTNUMBER, @PRESHIPMENT, @PRESHIPMENTCASES, @POSTINGDATETIME, @QUANTITY, @PARENTREFRECID,        
		@LOCATION, @PALLETNUMBER, @ExpiryDate, @ManufacturingDate, @IsWTS, @ItemNumber, @RefNumberOfPieces1, @RefNumberOfPieces2,        
		@RefNumberOfPieces3, @RefNumberOfPieces4, @RefNumberOfPieces5, @RefItemNumber1, @RefItemNumber2, @RefItemNumber3, @RefItemNumber4,        
		@RefItemNumber5, @SPPBatchCardNumber, @SPPBatchNumber, @PickingListQuantity, @BatchSequence, @GloveSize, @GloveSampleQuantity;
        
		/** dbo.AXPostingLog **/     
		IF @FunctionIdentifier = 'SMBP'  
		BEGIN  
			SELECT @BATCHCARDNUMBER = BatchNumber from batch with (nolock) where SerialNumber = @ReferenceBatchNumber1 
			-- check ReferenceBatchNumber1 is it open batch or not
			SELECT @IsConsolidatedMultipleBatch=case when COUNT(1)>0 then 1 else 0 end FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @ReferenceBatchNumber1 AND IsConsolidated = 1
			EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @ReferenceBatchNumber1, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID, @Area, @IsConsolidatedMultipleBatch       
			IF @ReferenceBatchNumber2 is not null  
			BEGIN 
				-- check ReferenceBatchNumber2 is it open batch or not
				SELECT @IsConsolidatedMultipleBatch=case when COUNT(1)>0 then 1 else 0 end FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @ReferenceBatchNumber2 AND IsConsolidated = 1
				SELECT @BATCHCARDNUMBER = BatchNumber from batch with (nolock) where SerialNumber = @ReferenceBatchNumber2  
				EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @ReferenceBatchNumber2, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID, @Area, @IsConsolidatedMultipleBatch  
				IF @ReferenceBatchNumber3 is not null  
				BEGIN
					-- check ReferenceBatchNumber3 is it open batch or not
					SELECT @IsConsolidatedMultipleBatch=case when COUNT(1)>0 then 1 else 0 end FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @ReferenceBatchNumber3 AND IsConsolidated = 1
					SELECT @BATCHCARDNUMBER = BatchNumber from batch with (nolock) where SerialNumber = @ReferenceBatchNumber3  
					EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @ReferenceBatchNumber3, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID, @Area, @IsConsolidatedMultipleBatch       
					IF @ReferenceBatchNumber4 is not null  
					BEGIN
						-- check ReferenceBatchNumber4 batch is it open batch or not
						SELECT @IsConsolidatedMultipleBatch=case when COUNT(1)>0 then 1 else 0 end FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @ReferenceBatchNumber4 AND IsConsolidated = 1
						SELECT @BATCHCARDNUMBER = BatchNumber from batch with (nolock) where SerialNumber = @ReferenceBatchNumber4  
						EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @ReferenceBatchNumber4, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID, @Area, @IsConsolidatedMultipleBatch      
						IF @ReferenceBatchNumber5 is not null  
						BEGIN
							-- check ReferenceBatchNumber5 is it open batch or not
							SELECT @IsConsolidatedMultipleBatch=case when COUNT(1)>0 then 1 else 0 end FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @ReferenceBatchNumber5 AND IsConsolidated = 1
							SELECT @BATCHCARDNUMBER = BatchNumber from batch with (nolock) where SerialNumber = @ReferenceBatchNumber5  
							EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @ReferenceBatchNumber5, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID, @Area, @IsConsolidatedMultipleBatch      
						END  
					END  
				END       
			END  
		END  
		ELSE  
		BEGIN     
			EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @SerialNumber, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID,  @Area, @IsConsolidated        
		END    
   END          
   END        
   SET @ROW = @ROW + 1      
 SET @AccQty = @AccQty + @Quantity --#EWN qty validation with staging  
    -- Reset value      
   SET @InternalLotNumber='';        
   SET @DateStockOut=null;      
   SET @BatchCardNumber='';      
   SET @BatchNumber='';       
   SET @FSIdentifier=null;      
   SET @FunctionIdentifier=null;      
   SET @ReferenceBatchNumber1=null;        
   SET @ReferenceBatchNumber2=null;        
   SET @ReferenceBatchNumber3=null;        
   SET @ReferenceBatchNumber4=null;        
   SET @ReferenceBatchNumber5=null;        
   SET @ReferenceBatchSequence1 = 0        
   SET @ReferenceBatchSequence2 = 0        
   SET @ReferenceBatchSequence3 = 0        
   SET @ReferenceBatchSequence4 = 0        
   SET @ReferenceBatchSequence5 = 0        
   SET @Sequence = 0;        
   SET @BatchOrderNumber=null;      
   SET @ReferenceItemNumber=null;      
   SET @Configuration=null;      
   SET @Warehouse = 'FG'        
   SET @Resource=null;      
   SET @CustomerPO=null;      
   --SET @CustomerReference=null;      
   --SET @SalesOrdernumber=null;      
   SET @InnerLotNumber=null;      
   SET @OuterLotNumber=null;      
   SET @CustomerLotNumber=null;      
   --SET @Preshipment = 0        
   SET @Preshipmentcases = 0        
   SET @InnerBoxCapacity = 0        
   SET @PostingDateTime = null        
   SET @Quantity = 0      
   SET @TotalQuantity = 0      --splitBatch  
   SET @ParentRefRecId = 0        
   SET @Location = ''        
   --SET @PalletNumber = ''        
   SET @ExpiryDate = null        
   SET @Manufacturingdate = null        
   SET @IsWTS = 0        
   --SET @ItemNumber = NULL        
   SET @CreateDateTime = GETDATE()        
   --SET @PONumber =''        
   SET @PreshipmentCaseCount = 0        
   SET @RefNumberOfPieces1 = 0 --For SMBP        
   SET @RefNumberOfPieces2 = 0 --For SMBP        
   SET @RefNumberOfPieces3 = 0 --For SMBP        
   SET @RefNumberOfPieces4 = 0 --For SMBP        
   SET @RefNumberOfPieces5 = 0 --For SMBP        
   SET @RefItemNumber1 = NULL --For SMBP        
   SET @RefItemNumber2 = NULL --For SMBP        
   SET @RefItemNumber3 = NULL --For SMBP        
   SET @RefItemNumber4 = NULL --For SMBP        
   SET @RefItemNumber5 = NULL --For SMBP        
   SET @PostingType = 'DOTFGJournalContract'        
   SET @SerialNumber = ''      
   SET @IsPostedToAX = 1        
   SET @IsPostedInAX = 1        
   SET @ExceptionCode = NULL        
   SET @TransactionID = '-1'        
   SET @Area = 'PS'
   SET @SPPBatchCardNumber = ''		--Surgical Packing Plan
   SET @SPPBatchNumber = ''			--Surgical Packing Plan
   SET @PickingListQuantity = 0 	--Surgical Packing Plan
   SET @BatchSequence = 0 			--Surgical Packing Plan
   SET @GloveSize = ''				--Surgical Packing Plan   
   SET @SumGloveSampleQuantity = 0	--Surgical Packing Plan   
   SET @GloveSampleQuantity = 0		--Surgical Packing Plan   
    -- Reset value      
  
END   -- while loop end    
      
 -- clean up      
 if OBJECT_ID('tempdb.dbo.#temptable') IS NOT NULL       
 BEGIN    
  DROP TABLE #temptable;       
 END  
    
 if OBJECT_ID('tempdb.dbo.#temptablePSI') IS NOT NULL       
 BEGIN    
  DROP TABLE #temptablePSI;      
 END  
      
 if OBJECT_ID('tempdb.dbo.#tempSOLineCount') IS NOT NULL       
 BEGIN    
  DROP TABLE #tempSOLineCount;      
 END     
       
 if OBJECT_ID('tempdb.dbo.#tempPOlineCount') IS NOT NULL       
 BEGIN    
  DROP TABLE #tempPOlineCount;      
 END    
  
 if OBJECT_ID('tempdb.dbo.#tempPalletMaxCase') IS NOT NULL       
 BEGIN    
  DROP TABLE #tempPalletMaxCase;      
 END     
   
   
 END TRY        
        
 BEGIN CATCH        
 DECLARE @ErrorMessage NVARCHAR(4000);        
 DECLARE @ErrorSeverity INT;        
 DECLARE @ErrorState INT;        
 SELECT @ErrorMessage = Error_message(),        
   @ErrorSeverity = Error_severity(),        
   @ErrorState = Error_state();  
 Insert into FGReceivedPallet_Error(ScanDate, PalletID, ErrorMsg)  
 values(GETDATE(), @PalletID, 'DOT-'+@ErrorMessage)   
 RAISERROR (@ErrorMessage, @ErrorSeverity,@ErrorState);       
        
 END CATCH;        
        
 IF @@TRANCOUNT > 0   
 BEGIN       
 COMMIT TRANSACTION;    
  
 --#EWN qty validation with staging  
 IF @EWNQty = @AccQty  
 BEGIN  
  SET @Return=1;  
  Insert into FGReceivedPallet_Error(ScanDate, PalletID, ErrorMsg)  
  values(GETDATE(), @PalletID, 'DOT-Insert Success.')  
 END   
 ELSE  
 BEGIN  
  DECLARE @qtyStaging INT   
  SET @qtyStaging = (Select sum(Quantity) from DOT_FGJournalTable with (nolock) where palletnumber=@PalletID and salesordernumber=@PONumber   
       and ReferenceItemNumber = @ItemNumber);  
  IF @EWNQty = @qtyStaging  
  BEGIN  
   SET @Return=1;  
   Insert into FGReceivedPallet_Error(ScanDate, PalletID, ErrorMsg)  
   values(GETDATE(), @PalletID, 'DOT-Insert Success.')  
  END  
  ELSE  
  BEGIN  
   SET @Return=0;   
   Insert into FGReceivedPallet_Error(ScanDate, PalletID, ErrorMsg)  
   values(GETDATE(), @PalletID, 'DOT-Total quantity not tally with EWN, please re-scan!')  
  END  
 END  
  
 END      
END

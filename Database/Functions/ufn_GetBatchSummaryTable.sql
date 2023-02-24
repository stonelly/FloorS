    
-- =======================================================    
-- Name:   ufn_GetBatchSummaryTable    
-- Purpose:   Gets balance pcs for a batch    
-- =======================================================    
-- Change History    
-- Date         Author     Comments    
-- -----        ------     -----------------------------    
-- 31/05/2019  Kamil    Create.    
-- 29/08/2019  Kamil    Fix PT's TotalPCs rounding issue    
-- 09/12/2019  yiksiu	Add GloveTransfer module (PSI glove)
-- 28/08/2020  Azman    SPP Enhancement   
-- 21/12/2020  Azrul    NGC merged with BB SPP       
-- =======================================================    
CREATE FUNCTION [dbo].[ufn_GetBatchSummaryTable](@SerialNumber NUMERIC)     
    
RETURNS @TempSummaryTable TABLE    
(    
 ID INT IDENTITY(1, 1)    
 ,ProcessInd NVARCHAR(10)    
 ,BalancePcs DECIMAL(18, 3)    
 ,PackedPcs DECIMAL(18, 3)    
 ,RejectPcs DECIMAL(18, 3)    
 ,GloveTransferPcs DECIMAL(18, 3)
 ,SumPackedPcs DECIMAL(18, 3)
 ,SumRejectPcs DECIMAL(18, 3)
 ,SumGloveTransferPcs DECIMAL(18, 3)
 ,ProcessDate DATETIME    
)    
    
AS    
BEGIN    
    
 DECLARE @TempBatchProcesses TABLE(    
  ProcessInd NVARCHAR(10)    
  ,Pcs DECIMAL(18, 3)    
  ,ProcessDate DATETIME    
  ,ItemId NVARCHAR(80)    
  ,BatchStatus NVARCHAR(30)    
  )    
    
 DECLARE @TempProcessingTable TABLE(    
  ID INT IDENTITY(1, 1)    
  ,ProcessInd NVARCHAR(10)    
  ,Pcs DECIMAL(18, 3)    
  ,ProcessDate DATETIME    
  ,ItemId NVARCHAR(80)    
  ,BatchStatus NVARCHAR(30)    
  )    
    
 --inser pn info    
 INSERT INTO @TempBatchProcesses    
 SELECT 'PN'    
  ,TotalPCs    
  ,BatchCardDate    
  ,NULL    
  ,NULL    
 FROM Batch WITH(NOLOCK)    
 WHERE SerialNumber = @SerialNumber    
    
 --insert qc info    
 INSERT INTO @TempBatchProcesses    
 SELECT 'QC'    
  ,(InnerBoxCount * PackingSize)    
  ,BatchEndTime    
  ,NULL    
  ,BatchStatus    
 FROM QCYieldAndPacking WITH(NOLOCK)    
 WHERE SerialNumber = @SerialNumber    
 AND BatchEndTime IS NOT NULL    
    
 --insert pt info    
 INSERT INTO @TempBatchProcesses    
 SELECT 'PT'    
  ,ROUND((BatchWeight*10000)/TenPcsWeight,-1)--(BatchWeight/TenPcsWeight) * 10000 -- yiksiu: PT rounding issue    
  ,LastModifiedOn    
  ,NULL    
  ,NULL    
 FROM PTScanBatchCard WITH(NOLOCK)    
 WHERE SerialNumber = @SerialNumber    
 AND LastModifiedOn IS NOT NULL    
    
 --if exists then "use 3 batch card to change with 1 lot number" is not already applied yet for the data    
 IF EXISTS(SELECT InternalLotNumber FROM FPChangeBatchCard WITH(NOLOCK) WHERE NewSerialNumber = @SerialNumber)    
 BEGIN    
  --insert fp info excluding from fp change batch card    
  INSERT INTO @TempBatchProcesses    
  SELECT 'FP'    
   ,(c.InnerBoxCapacity * a.BoxesPacked)    
   ,PackDate    
   ,b.ItemNumber    
   ,NULL    
  FROM FinalPackingBatchInfo a WITH(NOLOCK)    
  INNER JOIN FinalPacking b WITH(NOLOCK) ON a.InternalLotNumber = b.InternalLotNumber    
  INNER JOIN PurchaseOrderItem c WITH(NOLOCK) ON b.PONumber = c.PONumber    
   AND b.Size = c.ItemSize    
   AND b.ItemNumber = c.ItemNumber    
  WHERE a.SerialNumber = @SerialNumber    
  AND a.InternalLotNumber     
  NOT IN (SELECT InternalLotNumber FROM FPChangeBatchCard WITH(NOLOCK) WHERE NewSerialNumber = @SerialNumber)    
    
  --insert fp info from fp change batch card     
  INSERT INTO @TempBatchProcesses    
  SELECT 'FP'    
   ,(c.InnerBoxCapacity * a.BoxesPacked)    
   ,d.LastModifiedOn    
   ,b.ItemNumber    
   ,NULL    
  FROM FinalPackingBatchInfo a WITH(NOLOCK)    
  INNER JOIN FinalPacking b WITH(NOLOCK) ON a.InternalLotNumber = b.InternalLotNumber    
  INNER JOIN PurchaseOrderItem c WITH(NOLOCK) ON b.PONumber = c.PONumber    
  INNER JOIN FPChangeBatchCard d WITH(NOLOCK) ON d.ChangeBatchCardId =    
 (SELECT TOP 1 ChangeBatchCardId FROM FPChangeBatchCard WITH(NOLOCK) WHERE NewSerialNumber = a.SerialNumber AND InternalLotNumber = a.InternalLotNumber)    
  AND b.Size = c.ItemSize    
  AND b.ItemNumber = c.ItemNumber    
  WHERE a.SerialNumber = @SerialNumber
    
 END    
 ELSE    
 BEGIN    
  --insert fp info excluding from fp change batch card    
  INSERT INTO @TempBatchProcesses    
  SELECT 'FP'    
   ,CASE WHEN a.BoxesPacked = 0 THEN a.Pcs ELSE (c.InnerBoxCapacity * a.BoxesPacked) END
   ,PackDate       
   ,b.InternalLotNumber    
   ,NULL    
  FROM FinalPackingBatchInfo a WITH(NOLOCK)    
  INNER JOIN FinalPacking b WITH(NOLOCK) ON a.InternalLotNumber = b.InternalLotNumber    
  INNER JOIN PurchaseOrderItem c WITH(NOLOCK) ON b.PONumber = c.PONumber    
   AND b.Size = c.ItemSize    
   AND b.ItemNumber = c.ItemNumber    
  WHERE a.SerialNumber = @SerialNumber    
  AND a.InternalLotNumber     
  NOT IN (SELECT InternalLotNumber FROM FPChangeBatchCardLine a WITH(NOLOCK) INNER JOIN    
  FPChangeBatchCard b WITH(NOLOCK) ON a.ChangeBatchCardId = b.ChangeBatchCardId WHERE a.NewSerialNumber = @SerialNumber)    
    
  --insert fp info from fp change batch card     
  INSERT INTO @TempBatchProcesses    
  SELECT 'FP'     
   ,(e.InnerBoxCapacity * c.BoxesPacked)    
   ,b.LastModifiedOn    
   ,d.ItemNumber
   ,NULL    
  FROM FPChangeBatchCardLine a WITH(NOLOCK)    
  INNER JOIN FPChangeBatchCard b WITH(NOLOCK) ON a.ChangeBatchCardId = b.ChangeBatchCardId     
  INNER JOIN FinalPackingBatchInfo c WITH(NOLOCK) ON a.NewSerialNumber = c.SerialNumber AND b.InternalLotNumber = c.InternalLotNumber    
  INNER JOIN FinalPacking d WITH(NOLOCK) ON d.InternalLotNumber = c.InternalLotNumber    
  INNER JOIN PurchaseOrderItem e WITH(NOLOCK) ON d.PONumber = e.PONumber    
    AND d.Size = e.ItemSize    
    AND d.ItemNumber = e.ItemNumber    
  WHERE a.NewSerialNumber = @SerialNumber    
 END    

  -- insert Glove Transfer (Add GloveTransfer module (PSI glove))
  INSERT INTO @TempBatchProcesses
  SELECT 'GT'
    ,a.AssignedQuantity
    ,a.ModifiedDate
    ,NULL
    ,NULL
  FROM GloveTransferRequestAssignment a (NOLOCK)
  JOIN GloveTransferRequestDetail b (NOLOCK) ON a.GloveTransferRequestDetailId = b.GloveTransferRequestDetailId 
  JOIN GloveTransferRequest c (NOLOCK) ON b.GloveTransferRequestId = c.GloveTransferRequestId
  WHERE a.SerialNumber = @SerialNumber
    AND c.GloveTransferAssignmentStatus = 1 -- Confirmed the register list. before sent to PSI personal. PSI personal will confirm again once received (GloveTransferReceiptStatus)
    -- // Approved > Register serialNo > Confirmed  -- SerilaNo always in approved status
    --AND c.GloveTransferRequestStatus = 2 -- glove request always approved
   
 --Azman 27082020 Surgical Packing Plan Start    
    
 INSERT INTO @TempBatchProcesses    
 select 'SPP', A.ReservedQty, A.CreatedDate, NULL, NULL from SurgicalPackingPlanDetails (nolock) A    
 INNER JOIN SurgicalPackingPlan (nolock) B ON A.SurgicalPackingPlanId = B.SurgicalPackingPlanId    
 WHERE B.PlanStatus = 1        
 AND A.SerialNumber = @SerialNumber    
    
 --Azman 27082020 Surgical Packing Plan End   
    
 --insert id based on process date    
 INSERT INTO @TempProcessingTable    
 SELECT ProcessInd, Pcs, ProcessDate, ItemId, BatchStatus FROM @TempBatchProcesses ORDER BY ProcessDate ASC    
    
 DECLARE @Iteration INT    
 DECLARE @IterationMax INT    
 DECLARE @ProcessInd NVARCHAR(10)    
 DECLARE @Pcs DECIMAL(18, 3)    
 DECLARE @PrevPcs DECIMAL(18, 3)    
 DECLARE @ProcessDate DATETIME    
 DECLARE @BalancePcs DECIMAL(18, 3)    
 DECLARE @PrevBalancePcs DECIMAL(18, 3)    
 DECLARE @PNProcessDate DATETIME    
 DECLARE @ItemId NVARCHAR(80)    
 DECLARE @SumPackedPcs DECIMAL(18, 3)    
 DECLARE @SumRejectPcs DECIMAL(18, 3)    
 DECLARE @SumGloveTransferPcs DECIMAL(18, 3)
 DECLARE @BatchStatus NVARCHAR(30)    
 DECLARE @PrevBatchStatus NVARCHAR(30)    
 DECLARE @RejectPcs DECIMAL(18, 3)    
      
 SELECT @BalancePcs = Pcs, @PNProcessDate = ProcessDate FROM @TempProcessingTable WHERE ProcessInd = 'PN'    
    
 SELECT @IterationMax = COUNT(ID) FROM @TempProcessingTable    
    
 --for pn/start pcs    
 INSERT INTO @TempSummaryTable    
 VALUES ('PN',@BalancePcs,0,0,0,0,0,0,@PNProcessDate)    
    
 SET @Iteration = 1    
     
 WHILE (@Iteration) <= (@IterationMax)      
 BEGIN    
  --select current processing data    
  SELECT @ProcessInd = ProcessInd    
   ,@Pcs = Pcs    
   ,@ProcessDate = ProcessDate    
   ,@ItemId = ItemId    
   ,@BatchStatus = BatchStatus    
  FROM @TempProcessingTable    
  WHERE ID = @Iteration    
      
  SELECT @PrevBalancePcs = BalancePcs    
  FROM @TempSummaryTable    
  WHERE ID = @Iteration - 1    
    
  SELECT @PrevBatchStatus = BatchStatus, @PrevPcs = Pcs    
  FROM @TempProcessingTable    
  WHERE ID = @Iteration - 1    
    
  SELECT @SumPackedPcs = SUM(PackedPcs), @SumRejectPcs = SUM(RejectPcs), @SumGloveTransferPcs = SUM(GloveTransferPcs)    
  FROM @TempSummaryTable    
      
  IF (@BalancePcs > 0)    
  BEGIN    
   IF (@ProcessInd = 'FP' )    
   BEGIN    
    --if it is surgical, pcs is divided by 2    
    --IF EXISTS(SELECT 1 FROM DOT_FSItemMaster WHERE ITEMID = @ItemId AND ItemType = 8)    
    IF EXISTS(SELECT 1 FROM SurgicalPackingPlan WITH(NOLOCK) WHERE InternalLotNo = @ItemId AND PlanStatus IN (2,3))    
    BEGIN    
     SET @Pcs = @Pcs    
     SET @ProcessInd = @ProcessInd + '(S)'    
    END    
 --NGC D365 version START  
    ELSE IF EXISTS(SELECT 1 FROM FinalPacking A WITH(NOLOCK) INNER JOIN DOT_FSBrandLines B WITH(NOLOCK) ON A.ItemNumber = B.ITEMID 
	INNER JOIN DOT_FSItemMaster C ON B.ITEMID = C.ITEMID WHERE A.InternalLotNumber = @ItemId AND C.ITEMTYPE = 8)    
 --NGC D365 version END  
    BEGIN    
     SET @Pcs = @Pcs / 2    
     SET @ProcessInd = @ProcessInd + '(S)'    
    END    
    
    SET @BalancePcs = @PrevBalancePcs - @Pcs    
    
    INSERT INTO @TempSummaryTable    
    VALUES (@ProcessInd,@BalancePcs,@Pcs,0,0,@SumPackedPcs+@Pcs,@SumRejectPcs,@SumGloveTransferPcs,@ProcessDate)    
   END    
   ELSE IF (@ProcessInd = 'QC' OR @ProcessInd = 'PT')    
   BEGIN    
    --if previous status is split batch, add prev pcs to current pcs    
    IF (@PrevBatchStatus = 'Split Batch')    
    BEGIN    
     SET @Pcs = @Pcs + @PrevPcs    
    END    
    
    --if split batch or qc type changed, there is no rejection     
    IF (@BatchStatus = 'Split Batch') OR (@BatchStatus = 'QC Type Changed')    
    BEGIN    
     SET @BalancePcs = @PrevBalancePcs      
     SET @RejectPcs = 0    
    
     IF (@BatchStatus = 'Split Batch')    
      SET @ProcessInd = @ProcessInd + '(SB)'    
     ELSE    
      SET @ProcessInd = @ProcessInd + '(TC)'    
    
     INSERT INTO @TempSummaryTable    
     VALUES (@ProcessInd,@BalancePcs,0,@RejectPcs,0,@SumPackedPcs,@SumRejectPcs,@SumGloveTransferPcs,@ProcessDate)    
    END    
    ELSE    
    BEGIN    
     --check if latest pcs is greater than prev balance pcs. if greater, take prev balance pcs as default    
     IF (@Pcs > @PrevBalancePcs)    
     BEGIN    
      SET @BalancePcs = @PrevBalancePcs    
    
      INSERT INTO @TempSummaryTable    
      VALUES (@ProcessInd,@BalancePcs,0,0,0,@SumPackedPcs,@SumRejectPcs,@SumGloveTransferPcs,@ProcessDate)    
     END    
     ELSE    
     BEGIN    
      --PT is excluded temporarily until PT reverse calculation bugfix is implemented in NGC    
      --please remove this checking, only take the if condition only    
      IF (@ProcessInd = 'QC')    
      BEGIN           
       SET @BalancePcs = @Pcs    
       SET @RejectPcs = (@PrevBalancePcs - @Pcs)    
      END  
      ELSE    
      BEGIN    
       SET @BalancePcs = @PrevBalancePcs    
       SET @RejectPcs = 0    
      END    
    
      INSERT INTO @TempSummaryTable    
      VALUES (@ProcessInd,@BalancePcs,0,@RejectPcs,0,@SumPackedPcs,@SumRejectPcs+@RejectPcs,@SumGloveTransferPcs,@ProcessDate)    
     END    
    END    
   END
   ELSE IF (@ProcessInd = 'GT')
   BEGIN 
    SET @BalancePcs = @PrevBalancePcs - @Pcs

    INSERT INTO @TempSummaryTable
    VALUES (@ProcessInd,@BalancePcs,0,0,@Pcs,@SumPackedPcs,@SumRejectPcs,@SumGloveTransferPcs+@Pcs,@ProcessDate)
   END    
   ELSE IF (@ProcessInd = 'SPP') -- Added by Azman (SPP)    
   BEGIN     
   SET @BalancePcs = @PrevBalancePcs - @Pcs    
    
   INSERT INTO @TempSummaryTable    
   VALUES (@ProcessInd,@BalancePcs,@Pcs,0,0,0,0,0,@ProcessDate)    
   END 
  END    
    
  SET @Iteration = @Iteration + 1    
 END    
    
 RETURN    
END 
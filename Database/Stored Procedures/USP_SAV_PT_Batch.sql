  
-- ========================================================================================  
-- Name:   [USP_SAV_PT_Batch]  
-- Purpose:   <Save Details in PTScanBatchCard table>  
-- ========================================================================================  
-- Change History  
-- Date               Author                     Comments  
-- -----   ------   -----------------------------------------------------------------------  
-- <4 July,2014>  <Amrinder Singh>          SP created.  
-- <10 August,2016> <Roy Chow>                   Bug fix for Water Tight batches report dry weight.  
-- <19 Oct,2016> <Roy Chow>                   Bug fix for Water Tight batches report dry weight for BatchType 'PSW'.  
-- <19 Oct,2016> <Soon Siang>                 Update SP to return PTScanBatchCard ID.  
-- <23 Jul,2020> <PangYS>      Change @changeGloveType and @prevGloveType from 25 to 50  
-- <19 Nov,2021> <Azrul>       HTLG_HSB_002: Special Glove (Clean Room Product) - Save Batch Order Number.
  
CREATE PROCEDURE [dbo].[USP_SAV_PT_Batch]  
(  
    @serialNo             NUMERIC(10,0),  
    @shiftId              INT,  
    @reworkReasonId       INT = NULL,  
    @reworkProcess        NVARCHAR(30) = NULL,  
    @reworkCount          INT = NULL,  
    @locationId           INT,  
    @lastModifiedOn       DATETIME,  
    @tenPcsWeight         DECIMAL(18,3),  
    @batchWeight          DECIMAL(18,3),  
    @workstationId        INT = NULL,  
    @changeGloveType      NVARCHAR(50) = NULL,  
    @prevGloveType        NVARCHAR(50) = NULL,  
    @id                   INT,  
    @dryerId              INT = NULL,  
    @authorizedFor        INT = NULL,
	@oldbatchOrder		  NVARCHAR(100) = NULL, 
	@batchOrder			  NVARCHAR(100) = NULL 
)  
AS  
BEGIN  
 BEGIN TRANSACTION;  
   -- Try Block  
  BEGIN TRY  
    DECLARE @PTID INT  
       SET NOCOUNT ON;  
   IF @id = 0  
    BEGIN    
     INSERT INTO PTScanBatchCard (SerialNumber, ShiftId, ReworkReasonId, ReworkProcess, ReworkCount, LocationId,LastModifiedOn,TenPcsWeight,BatchWeight,WorkstationId,DryerId,AuthorizedFor)   
     VALUES (@serialNo, @shiftId, @reworkReasonId, @reworkProcess, @reworkCount, @locationId, @lastModifiedOn, @tenPcsWeight, @batchWeight, @workstationId, @dryerId, @authorizedFor)  
     SET @PTID = SCOPE_IDENTITY()  
     --UPDATE Batch SET PTBatchWeight = @batchWeight, PTTenPCsWeight = @tenPcsWeight WHERE SerialNumber = @serialNo   
     --IF ( SELECT BatchType FROM Batch with(nolock)  WHERE SerialNumber = @serialNo )LIKE '%WT%'  
     IF ( SELECT BatchType FROM Batch with(nolock)  WHERE SerialNumber = @serialNo ) IN (select EnumValue from EnumMaster where EnumType = 'WTType')  
      UPDATE Batch SET PTBatchWeight = @batchWeight, PTTenPCsWeight = @tenPcsWeight, BatchWeight = @batchWeight, TenPCsWeight = @tenPcsWeight, TotalPCs = ROUND((@batchWeight*10000)/@tenPcsWeight,-1) WHERE SerialNumber = @serialNo  
     ELSE  
      UPDATE Batch SET PTBatchWeight = @batchWeight, PTTenPCsWeight = @tenPcsWeight WHERE SerialNumber = @serialNo   
    END  
   ELSE  
    BEGIN  
     UPDATE PTScanBatchCard SET ChangeGloveType = @changeGloveType WHERE SerialNumber = @serialNo --#Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product), remove updates LastModifiedOn for USP_DOT_GET_PT_OR_QC_QITestResult checking.   
     AND Id = (SELECT TOP 1 Id FROM PTScanBatchCard WHERE SerialNumber = @serialNo ORDER BY LastModifiedOn DESC)  
     SET @PTID = (SELECT TOP 1 Id FROM PTScanBatchCard WHERE SerialNumber = @serialNo ORDER BY LastModifiedOn DESC)  
     UPDATE Batch SET GloveType = @changeGloveType WHERE SerialNumber = @serialNo
     INSERT INTO ChangeGloveHistory (SerialNumber, OldGloveType, NewGloveType, LastModifiedOn, WorkstationId, OldBatchOrder, NewBatchOrder) VALUES  --#Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product) 
     (@serialNo, @prevGloveType, @changeGloveType, @lastModifiedOn, @workstationId, @oldbatchOrder, @batchOrder) --#Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product)     
	 UPDATE DOT_FloorD365HRGLOVERPT SET GloveCode = @changeGloveType, BthOrder = @batchOrder WHERE SerialNo = @serialNo --#Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product)
          END  
  
    SELECT @PTID  
       SET NOCOUNT OFF;  
  END TRY  
   -- Catch Block   
  BEGIN CATCH  
   IF @@TRANCOUNT > 0  
     ROLLBACK TRANSACTION;  
  END CATCH;  
  IF @@TRANCOUNT > 0  
    COMMIT TRANSACTION;  
END  
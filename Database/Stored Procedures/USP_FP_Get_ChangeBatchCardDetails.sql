  
  
-- ========================================================================================  
-- Name:   USP_FP_Get_ChangeBatchCardDetails  
-- Purpose:   get the change batch card data data for posting to AX.
-- 2021-12-10: HSB SIT Issue: merged GloveType from NGC
-- ========================================================================================  
CREATE PROCEDURE [dbo].[USP_FP_Get_ChangeBatchCardDetails]  
(  
       @internalLotnumber nvarchar(15)  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
      
 select Top(1)  
fcb.OldSerialNumber,fcb.NewSerialNumber,POI.CustomerSize,fp.PONumber,b.BatchCardDate,b.BatchNumber,lm.LocationName, fp.BoxesPacked*poi.InnerBoxCapacity as 'TotalPieces'  
 ,b.GloveType as ItemNumber  --HSB SIT Issue: merged GloveType from NGC
 from FPChangeBatchCard fcb  
 INNER JOIN finalpacking fp on fp.InternalLotNumber = fcb.InternalLotNumber  
 INNER JOIN PurchaseOrderItem poi  on poi.PONumber = fp.PONumber and POi.ItemNumber = FP.ItemNumber and POi.ItemSize = FP.Size    
 INNER JOIN Batch b on b.SerialNumber = fp.SerialNumber  
 INNER JOIN LocationMaster lm on lm.LocationId = fcb.LocationId  
where fcb.InternalLotNumber = @internalLotnumber order by fcb.LastModifiedOn desc  
    SET NOCOUNT OFF;  
END  
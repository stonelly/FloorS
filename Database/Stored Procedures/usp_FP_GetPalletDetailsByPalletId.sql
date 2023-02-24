ALTER PROCEDURE [dbo].[usp_FP_GetPalletDetailsByPalletId]  
-- Add the parameters for the stored procedure here  
 @palletid nvarchar(10)  
AS  
BEGIN  
  
SELECT PM.PoNumber,PM.ItemNumber,PM.ItemSize,PM.IsPreshipment,LM.LocationName,PM.LocationId,PO.CustomerReferenceNumber From PalletMaster PM  
LEFT JOIN LocationMaster LM on LM.LocationId=PM.LocationId  
LEFT JOIN PurchaseOrderItem PO on PM.PoNumber=PO.PoNumber  
 where PalletId=@palletid  
  and not exists (select 1 from DOT_FloorD365BO FGBO with(nolock) where FGBO.BthOrderId like '%'+ pm.PONumber +'%' and FGBO.WarehouseId='MTS-FG') -- filter out MTS pallet by FGBO warehouse
END  

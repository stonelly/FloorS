-- =============================================    
-- Author:  Muhammad Khalid  
-- Create date: 4 June 2018    
-- Description: Get Active First Grade PO without SO list      
-- =====================================================================      
CREATE PROCEDURE [dbo].[USP_DOT_ReprintMTS_FGBO_Selection]    
AS    
BEGIN    
  
 SET NOCOUNT ON;    
    -- Insert statements for procedure here    
  SELECT a.BthOrderId, a.ProdStatus,c.ItemType, a.BatchId as CustomerRef,a.QtySched,fp.CasesPacked    
  from DOT_FloorD365BO a   
  -- left join DOT_FloorSales b on a.BatchId = b.CustomerRef   
  left join DOT_FSItemMaster c on a.ItemId = c.ItemId  
   left join  (select sum(fp.CasesPacked) as CasesPacked,bo2.BthOrderId from finalpacking FP WITH (NOLOCK)             
     join purchaseorderitem POIN WITH (NOLOCK) on POIN.ponumber = FP.ponumber and POIN.Itemnumber = FP.itemnumber and FP.Size = POIN.ItemSize            
     join DOT_FloorD365BO bo2 WITH (NOLOCK) on bo2.BthOrderId = fp.FGBatchOrderNo and FP.Size = bo2.Size and bo2.IsDeleted=0            
     WHERE bo2.ProdStatus IN ('ReportedFinished','StartedUp','Released') group by bo2.BthOrderId) as fp          
    on fp.BthOrderId = a.BthOrderId     
  WHERE -- b.CustomerRef is null   
  a.prodPoolId = 'FG' and a.WarehouseId = 'MTS-FG' -- warehouse = “MTS-FG”, Production Pool = “FG” are Made to stock batch order, update on 22/11/2021,Max He  
  and a.ProdStatus = 'StartedUp' and  c.ItemType = 5 --=>'FG'  
  and a.ReworkBatch = 'No' and a.IsDeleted=0  and fp.CasesPacked is not null
  GROUP By a.BthOrderId,a.ProdStatus, a.BatchId, c.ItemType,       
   a.QtySched,        
   fp.CasesPacked       
  having a.QtySched - ISNULL(fp.CasesPacked,0) > 0 --filter out full packed FGBO       
END  
  
  
  
  
  
  
  
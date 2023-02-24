
CREATE OR ALTER PROCEDURE [dbo].[usp_FP_DOT_GetFGOrderBySOandItemSize]       
 @Size nvarchar(30),
 @SalesId nvarchar(30),
 @ItemNumber nvarchar(30),
 @Location nvarchar(20)

AS      
BEGIN        
      
 select distinct bo.BthOrderId as BatchOrder
 from   
 DOT_FloorSalesLine FSL with (nolock)      
 left Join DOT_FloorSales ST with (nolock) on FSL.SALESID =  ST.SALESID        
 left join DOT_FSItemMaster im with (nolock) on im.ItemId = FSL.ItemId        
 left join DOT_FloorD365BO bo with (nolock) on bo.BatchId = st.CustomerRef and bo.Size = case when im.ItemType = 8 then FSL.CustomerSize else FSL.CONFIGURATION end  
 left join DOT_FloorD365BOResource res with (nolock) on res.BatchOrderId = bo.BthOrderId     
 left join  (select sum(fp.CasesPacked) as CasesPacked,bo2.BthOrderId from finalpacking FP WITH (NOLOCK)         
 join purchaseorderitem POIN WITH (NOLOCK) on POIN.ponumber = FP.ponumber and POIN.Itemnumber = FP.itemnumber and FP.Size = POIN.ItemSize        
 join DOT_FloorD365BO bo2 WITH (NOLOCK) on bo2.BthOrderId = fp.FGBatchOrderNo and FP.Size = bo2.Size and bo2.IsDeleted=0        
 WHERE bo2.ProdStatus IN ('ReportedFinished','StartedUp','Released') group by bo2.BthOrderId) as fp      
 on fp.BthOrderId = bo.BthOrderId    
 where  
  bo.ReworkBatch <> 'Yes' --filter out all rework order        
   and im.ITEMTYPE <> 4 -- ensure item type is not 'GLove'         
   and ST.SALESSTATUS = 1 --'Backorder'      
   and ST.DocumentStatus not in (0,1,2) --ensure document status is not ('None','Quotation','PurchaseOrder')      
   and st.salesid = @SalesId       
   and bo.ItemId = @ItemNumber       
   and bo.ProdPoolId = 'FG'      
   --and FSL.CONFIGURATION = @Size   
   and @Size = case when im.ItemType = 8 then FSL.CustomerSize else FSL.CONFIGURATION end     
   and bo.ProdStatus = 'StartedUp'      
   and bo.IsDeleted=0 and res.IsDeleted=0      
   --and res.PlantNo = @Location -- HSB allow to pack by different plant.     
   group by bo.BthOrderId,    
			bo.QtySched,    
			fp.CasesPacked
   having bo.QtySched - ISNULL(fp.CasesPacked,0) > 0 --filter out full packed FGBO      
END 
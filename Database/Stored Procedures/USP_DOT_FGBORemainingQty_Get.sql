-- ===========================================================    
-- Name:   USP_DOT_FGBORemainingQty_Get   
-- Purpose:  Get Glove Remaining Qty from FGBO (HSB all plant)    
-- ===========================================================    
-- Change History    
-- Date    Author   Comments    
-- -----   ------   ------------------------------------------    
-- 01/03/2022  Azrul Amin    SP created.
-- ===========================================================    
--exec USP_DOT_FGBORemainingQty_Get 'HBBON000000197','XL'  

CREATE PROCEDURE [dbo].[USP_DOT_FGBORemainingQty_Get]     
(   
 @BatchOrderId varchar(200),
 @Size varchar(10)  
)   
AS    
BEGIN     
 SET NOCOUNT ON;    

 SELECT bo.ProdStatus,bo.BthOrderId,bo.ItemId,bo.Size,bo.QtySched,bo.ProdPoolId,
 ISNULL(fg.FGReportedQty,0) as FGReportedQty,bo.QtySched - ISNULL(fg.FGReportedQty,0) AS RemainingQty
  FROM  
  DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN
  
  --FGReportedQty  
  (SELECT sum(fp.CasesPacked) as FGReportedQty,bo2.BthOrderId from finalpacking FP WITH (NOLOCK)         
  join purchaseorderitem POIN WITH (NOLOCK) on POIN.ponumber = FP.ponumber and POIN.Itemnumber = FP.itemnumber and FP.Size = POIN.ItemSize        
  join DOT_FloorD365BO bo2 WITH (NOLOCK) on bo2.BthOrderId = fp.FGBatchOrderNo and bo2.IsDeleted=0 AND bo2.Size = POIN.CustomerSize   
  WHERE bo2.ProdStatus IN ('ReportedFinished','StartedUp','Released')
  group by bo2.BthOrderId) as fg on fg.BthOrderId = bo.BthOrderId      
  
  where bo.ProdPoolID in ('FG','2FG') and bo.ProdStatus IN ('ReportedFinished','Released','StartedUp')  
  AND bo.ReworkBatch = 'No' and bo.IsDeleted = 0  
  AND bo.BthOrderId = @BatchOrderId and bo.size = @Size

END


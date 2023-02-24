  
  
-- =========================================================    
-- Name:   USP_DOT_BatchOrderDetails_Get   
-- Purpose:   Get Glove Batch Order details to populate grid    
-- =========================================================    
-- Change History    
-- Date    Author   Comments    
-- -----   ------   ----------------------------------------    
-- 26/02/2018  Azrul Amin    SP created.    
-- 09/01/2019  Azrul Amin    Include 2nd Grade FG in Final   
-- 22/03/2019  Muhd Khalid   Include Batch Order and Size in input     
--							 Packing Batch Order Screen.
-- 23/10/2020  Azrul Amin	 Add Surgical Glove logics.
-- 20/01/2021  Azrul Amin	 Add Online 2G calculation.
-- 20/03/2021  Azrul Amin	 SP Fine Tune.
-- 08/04/2021  Azrul Amin	 FGBO Surgical to match SO & BO size. 
-- 01/07/2021  Azrul Amin	 Cater Plant10 and onwards. 
-- 26/12/2021  Azrul Amin	 HTLG_HSB_002: Special Glove (Clean Room Product)
-- 12/04/2021  Azrul Amin    Fine Tune SP Performance Issue.  
-- 15/04/2022  Max	 He      Fix BO listing screen reminding and reported qty not correct Issue.    
-- =========================================================    
--exec USP_DOT_BatchOrderDetails_Get 'Glove','p6','HSBBON000000028'  
  
CREATE or ALTER  PROCEDURE [dbo].[USP_DOT_BatchOrderDetails_Get]     
(   
 @ProdPoolID varchar(100),  
 @PlantNo varchar(10),  
 @BatchOrderId varchar(200) = null,  
 @Size varchar(10) = null  
)   
AS    
BEGIN     
 SET NOCOUNT ON;       
 IF (@ProdPoolID = 'FG')  
 BEGIN  
 DECLARE @ProdPoolIDs varchar(100)   
 SET @ProdPoolIDs = 'FG,2FG'  
    SET @ProdPoolID = ',' + @ProdPoolIDs + ','  
 END  
 ELSE  
 BEGIN  
    SET @ProdPoolID = ',' + @ProdPoolID + ','  
 END  
  
 SELECT a.*  
 into #tempBO FROM  
 ( SELECT bo.BatchId,bo.SchedStart,bo.SchedFromTime,bo.BthOrderId,bo.ItemId,bo.Size,bo.QtySched,bo.ProdPoolId,bo.ProdStatus,bo.SchedEnd,bo.SchedToTime,  
  st.SalesId,st.CustomerRef,   
  res.BatchOrderId,res.ResourceGrp, res.Resource  
  ,ISNULL(hr.GloveReportedQty,0) + ISNULL(on2g.GloveReportedQty,0) + ISNULL(cg.GloveReportedQty,0) as GloveReportedQty  
  ,ISNULL(fg.FGReportedQty,0) as FGReportedQty   
  FROM  
  DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN   
  DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
  DOT_FloorSales AS st WITH (NOLOCK) ON  bo.BatchId = st.CustomerRef AND st.IsDeleted = 0 LEFT JOIN  
  
  --GloveReportedQty  
  (SELECT b.BthOrder, ISNULL(SUM(b.PackingSz * b.InBox),0) as GloveReportedQty  
  FROM DOT_FloorD365HRGLOVERPT b WITH (NOLOCK)  
  where b.Plant = @PlantNo  
  GROUP BY b.BthOrder) as hr on hr.BthOrder = bo.BthOrderId LEFT JOIN  
    
  --Online2GReportedQty  
  (SELECT b.BatchOrder, ISNULL(SUM(b.PackingSize * b.InnerBox),0) as GloveReportedQty  
  FROM DOT_FloorD365Online2G b WITH (NOLOCK)  
  where b.Plant = @PlantNo  
  GROUP BY b.BatchOrder) as on2g on on2g.BatchOrder = bo.BthOrderId LEFT JOIN  
    
  --ChengeGloveReportedQty  
  (SELECT a.OldBatchOrder, ISNULL(SUM(b.PackingSz * b.InBox),0) as GloveReportedQty  
  FROM ChangeGloveHistory a with (nolock) JOIN DOT_FloorD365HRGLOVERPT b with (nolock)  
  on a.SerialNumber = b.SerialNo  
  where b.Plant = @PlantNo  
  GROUP BY a.OldBatchOrder) as cg on cg.OldBatchOrder = bo.BthOrderId LEFT JOIN  
  
  --FGReportedQty  
  (SELECT sum(fp.CasesPacked) as FGReportedQty,bo2.BthOrderId from finalpacking FP WITH (NOLOCK) 
  join DOT_FSItemMaster im with(nolock) on FP.ItemNumber=im.ItemId and im.IsDeleted=0
  join purchaseorderitem POIN WITH (NOLOCK) on POIN.ponumber = FP.ponumber and POIN.Itemnumber = FP.itemnumber and FP.Size = POIN.ItemSize        
  join DOT_FloorD365BO bo2 WITH (NOLOCK) on bo2.BthOrderId = fp.FGBatchOrderNo and bo2.IsDeleted=0 
  AND bo2.Size = case when im.ItemType=8 then POIN.CustomerSize else POIN.ItemSize end -- surigical use customerSize, other glove use common size
  WHERE bo2.ProdStatus IN ('ReportedFinished','StartedUp','Released') and fp.FGBatchOrderNo is not null  
        --and fp.Resource = @PlantNo + '-FP' -- 6 seconds add resource filter -- remove for HSB can cross plant  
  group by bo2.BthOrderId) as fg on fg.BthOrderId = bo.BthOrderId      
  
  where PATINDEX('%,' + ProdPoolId + ',%', @ProdPoolID) > 0 and bo.ProdStatus IN ('ReportedFinished','Released','StartedUp')  
  AND bo.ReworkBatch = 'No' AND res.PlantNo = @PlantNo -- HTLG_NGC1.5_CR_007&025 Resource and Resource Group (Glove and FG) - Fixes for permofmance issue.     
  AND res.IsDeleted = 0 AND bo.IsDeleted = 0  
 ) a  
  
 select a.SchedStart,  
 DATEADD(hour, DATEDIFF(hour,0,a.SchedFromTime), 0) as SchedFromTime,a.BthOrderId,  
 ISNULL(STUFF((SELECT ', ' + c.SalesId + ' (' + c.CustomerRef + ')'  FROM #tempBO c  
    WHERE c.BthOrderId = a.BthOrderId  
    FOR XML path('') ), 1, 2, ''),a.BatchId)   
   AS SalesOrder,  
 a.ItemId,a.Size,a.QtySched  
 ,CASE WHEN a.ProdPoolId IN ('Glove','SGR') THEN a.GloveReportedQty ELSE FGReportedQty  
  END AS ReportedQty  
 ,CASE WHEN a.ProdPoolId IN ('Glove','SGR') AND (a.QtySched - a.GloveReportedQty) <= 0 THEN 0   
    WHEN a.ProdPoolId IN ('Glove','SGR') AND (a.QtySched - a.GloveReportedQty) > 0 THEN (a.QtySched - a.GloveReportedQty)   
    WHEN a.ProdPoolId = 'FG' AND (a.QtySched - a.GloveReportedQty) <= 0 THEN 0   
  ELSE a.QtySched - a.FGReportedQty  
  END AS RemainingQty  
 ,STUFF((SELECT distinct ', ' + c.ResourceGrp  
  FROM #tempBO c    
  WHERE c.BatchOrderId = a.BatchOrderId FOR XML path('')), 1, 2, '') AS ResourceGrp  
 ,STUFF((SELECT ', ' + c.Resource  
  FROM #tempBO c   
  WHERE c.BatchOrderId = a.BatchOrderId FOR XML path('') ), 1, 2, '') AS Resource  
  ,ProdPoolId,CASE WHEN ProdStatus = 'StartedUp' THEN 'Started' ELSE ProdStatus END AS ProdStatus,SchedEnd  
  ,DATEADD(hour, DATEDIFF(hour,0,SchedToTime), 0) as SchedToTime  
  from #tempBO a   
  where  a.BthOrderId = isnull(@BatchOrderId,a.BthOrderId) and a.Size = isnull(@Size,a.Size)  
  Group by a.SchedStart,a.SchedFromTime,a.BthOrderId,a.BatchId,a.ItemId,a.Size,a.QtySched,a.BatchOrderId,a.ProdPoolId,a.ProdStatus,a.SchedEnd,a.SchedToTime,  
  a.GloveReportedQty  
  ,a.FGReportedQty  
 drop table #tempBO  
END
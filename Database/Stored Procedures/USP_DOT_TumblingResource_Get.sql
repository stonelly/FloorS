-- =============================================================================      
-- Name:   USP_DOT_TumblingResource_Get  
-- Purpose:   Get Resource and Batch Order details for Print Normal Batch Card   
-- =============================================================================    
-- Change History    
-- Date    Author   Comments    
-- -----   ------   ------------------------------------------------------------    
-- 24/12/2020  Azrul Amin    SP created (HSB only).    
-- 21/01/2021  Azrul Amin	 Fixes for performance issue.
-- =============================================================================      
  
CREATE PROCEDURE [dbo].[USP_DOT_TumblingResource_Get]    
(     
 @LocationId Int,     
 @LineId varchar(20),    
 @Resource varchar(20),     
 @BO varchar(20)  
)     
AS    
BEGIN       
 SET NOCOUNT ON;    
    
 -- #1.List out all ResourceGroup (line).    
 IF (@LineId IS NULL) OR (LEN(@LineId) < 0)     
 BEGIN    
  SELECT DISTINCT    
  '' as Resource    
  ,'' as ResourceId    
  ,'' as LocationId    
  ,'' as Plant    
  ,res.[LineNo] as LineId  
  ,res.ResourceGrp as Line    
  ,'' as TierSide    
  ,'' as BatchOrder    
  ,'' as GloveCode    
  ,'' as Size    
  FROM     
   DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN    
   DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN    
   LocationMaster as loc WITH (NOLOCK) ON res.PlantNo = loc.LocationName JOIN
   DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.    
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND bo.ProdStatus = 'StartedUp'     
  AND res.[LineNo] in (select linenumber from linemaster where LocationId = @LocationId) --#AZRUL-BUG 1179: Remove Invalid Lines In Glove Output Reporting.  
  AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon') --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
 END    
    
-- #2.List out all Resource    
ELSE IF ((@Resource IS NULL) OR (LEN(@Resource) = 0))    
 BEGIN    
  IF ((@BO IS NULL) OR (LEN(@BO) = 0))    
  BEGIN    
   SELECT DISTINCT    
   res.Resource    
   ,'' as ResourceId    
   ,'' as LocationId    
   ,'' as Plant    
   ,res.[LineNo] as LineId  
   ,res.ResourceGrp as Line  
   ,res.TierSide as TierSide  
   ,'' as BatchOrder    
   ,'' as GloveCode    
   ,'' as Size    
   FROM   
    DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
    DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
    LocationMaster as loc WITH (NOLOCK) ON res.PlantNo = loc.LocationName JOIN
    DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
   WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND res.[LineNo] = @LineId AND bo.ProdStatus = 'StartedUp'   
   and bo.IsDeleted=0 AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon')  --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  END  
  ELSE    
  BEGIN    
   SELECT DISTINCT    
   res.Resource    
   ,'' as ResourceId    
   ,'' as LocationId    
   ,'' as Plant    
   ,res.[LineNo] as LineId  
   ,res.ResourceGrp as Line  
   ,res.TierSide as TierSide  
   ,'' as BatchOrder    
   ,'' as GloveCode    
   ,'' as Size    
   FROM   
    DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
    DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
    LocationMaster as loc WITH (NOLOCK) ON res.PlantNo = loc.LocationName JOIN
    DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
   WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND res.[LineNo] = @LineId AND bo.ProdStatus = 'StartedUp'   
   and bo.IsDeleted=0 and bo.BthOrderId = @BO AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon')   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  END  
 END    
    
-- #3.To list out all Batch Order based on selected Resource.    
ELSE IF ((@BO IS NULL) OR (LEN(@BO) = 0))    
 BEGIN    
  SELECT     
  res.Resource    
  ,res.Id as ResourceId    
  ,loc.LocationId    
  ,res.PlantNo as Plant  
  ,res.[LineNo] as LineId  
  ,res.ResourceGrp as Line  
  ,res.TierSide as TierSide  
  ,bo.BthOrderId as BatchOrder    
  ,bo.ItemId as GloveCode    
  ,bo.Size    
  FROM   
   DOT_FloorD365BO (NOLOCK) AS bo LEFT JOIN  
   DOT_FloorD365BOResource (NOLOCK) AS res ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc WITH (NOLOCK) ON res.PlantNo = loc.LocationName JOIN
   DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND res.[LineNo] = @LineId  
  AND res.Resource = @Resource AND bo.ProdStatus = 'StartedUp'  
  and bo.IsDeleted=0 AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon')   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
 END  
    
-- #4.To list out all Batch Order Details based on selected Resource and Batch Order.    
ELSE    
 BEGIN    
  SELECT     
  res.Resource    
  ,res.Id as ResourceId    
  ,loc.LocationId    
  ,res.PlantNo as Plant  
  ,res.[LineNo] as LineId  
  ,res.ResourceGrp as Line  
  ,res.TierSide as TierSide  
  ,bo.BthOrderId as BatchOrder    
  ,bo.ItemId as GloveCode    
  ,bo.Size    
  FROM   
   DOT_FloorD365BO AS bo  WITH (NOLOCK) LEFT JOIN  
   DOT_FloorD365BOResource  AS res  WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc WITH (NOLOCK) ON res.PlantNo = loc.LocationName JOIN
   DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND res.[LineNo] = @LineId   
  AND res.Resource = @Resource AND bo.ProdStatus = 'StartedUp' AND bo.BthOrderId = @BO  
  and bo.IsDeleted=0 AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon')  --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC. 
 END  
 SET NOCOUNT OFF;      
END  
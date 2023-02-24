-- =======================================================================  
-- Name:   USP_DOT_Online2GResources_Get
-- Purpose:   Get all Resources for Online 2nd Grade Glove Screen 
-- =======================================================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   ------------------------------------------------------  
-- 24/11/2020  Azrul Amin    SP created.  
-- 12/04/2021  Azrul Amin    Fine Tune SP Performance Issue.  
-- =======================================================================  

CREATE OR ALTER PROCEDURE [dbo].[USP_DOT_Online2GResources_Get]  
(   
 @LocationId Int,   
 @LineId varchar(20),  
 @ItemId varchar(50),   
 @Size varchar(20)
)   
AS  
BEGIN     
 SET NOCOUNT ON;  
  
 -- #1.To list out all Resources Group (lines).  
 IF (@LineId IS NULL) OR (LEN(@LineId) < 0)   
 BEGIN  
  SELECT DISTINCT  
  res.[LineNo] as LineId  
  ,'' as GloveCode  
  ,'' as Size  
  ,'' as BatchOrder  
  FROM   
   DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
   DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON res.PlantNo = loc.LocationName  
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND bo.ProdStatus = 'StartedUp'   
  AND res.[LineNo] in (select linenumber from linemaster where LocationId = @LocationId) 
 END  

-- #2.To list out all Glove Code.  
ELSE IF ((@ItemId IS NULL) OR (LEN(@ItemId) = 0))  
 BEGIN
   SELECT DISTINCT  
   '' as LineId  
   ,bo.ItemId as GloveCode  
   ,'' as Size  
   ,'' as BatchOrder   
   FROM   
    DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
    DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
    LocationMaster as loc ON res.PlantNo = loc.LocationName  
   WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND res.[LineNo] = @LineId AND bo.ProdStatus = 'StartedUp'   
   and bo.IsDeleted=0  
 END
  
-- #3.To list out all Size based on selected Line & GloveCode.  
ELSE IF ((@Size IS NULL) OR (LEN(@Size) = 0))  
 BEGIN  
   SELECT DISTINCT  
   '' as LineId  
   ,'' as GloveCode  
   ,bo.Size as Size  
   ,'' as BatchOrder   
  FROM   
   DOT_FloorD365BO (NOLOCK) AS bo LEFT JOIN  
   DOT_FloorD365BOResource (NOLOCK) AS res ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON res.PlantNo = loc.LocationName  
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND res.[LineNo] = @LineId  
  AND bo.ItemId = @ItemId AND bo.ProdStatus = 'StartedUp'  
  and bo.IsDeleted=0  
 END  
  
-- #4.To list out all Batch Order based on selected Line, GloveCode & Size.  
ELSE  
 BEGIN  
   SELECT DISTINCT  
   res.ResourceGrp as LineId
   ,'' as GloveCode  
   ,'' as Size  
   ,bo.BthOrderId as BatchOrder   
  FROM   
   DOT_FloorD365BO AS bo  WITH (NOLOCK) LEFT JOIN  
   DOT_FloorD365BOResource  AS res  WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON res.PlantNo = loc.LocationName  
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND res.[LineNo] = @LineId   
  AND bo.ItemId = @ItemId AND bo.ProdStatus = 'StartedUp' and bo.Size = @Size
  and bo.IsDeleted=0  
 END  
 SET NOCOUNT OFF;    
END  
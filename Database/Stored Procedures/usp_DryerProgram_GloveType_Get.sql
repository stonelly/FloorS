-- ============================================================    
-- Name:   [dbo].[usp_DryerProgram_GloveType_Get]    
-- Purpose:   <Fetches dryer program based on GloveType>    
-- ============================================================    
-- Change History    
-- Date   Author    Comments    
-- -----  ------    -----------------------------    
-- 09/09/2014  Kishan    SP created    
-- 26/11/2014   Kishan     Added filter for Stopped    
-- 19/01/2021   Aaron Quak     Map to new table  
-- 27/02/2021 Azman  Fix on deleted record on DOT_GloveRelCyclone  
-- 22/11/2021 Azrul  HTLG_HSB_002: Special Glove (Clean Room Product) - Rename DryerProgram. 
-- ============================================================    
CREATE PROCEDURE [dbo].[usp_DryerProgram_GloveType_Get]    
(    
@GloveType NVARCHAR(50)    
)    
AS    
BEGIN    
SET NOCOUNT ON;    
--SELECT cp.AVACYCLONEPROCESSTABLE_ID AS DryerProgramId,cp.CYCLONEPROCESS AS DryerProgramName    
--FROM AX_AVAGLOVERELCYCLONE grc    
--INNER JOIN AX_AVAGLOVECODETABLE gct    
----ON gct.RECID = grc.GLOVEREFRECID    
--ON gct.AVAGLOVECODETABLE_ID = grc.GLOVEREFRECID    
--INNER JOIN AX_AVACYCLONEPROCESSTABLE cp    
--ON grc.CYCLONEPROCESS = cp.CYCLONEPROCESS    
--WHERE gct.GloveCode = @GloveType    
--AND cp.STOPPED <> 1    
  
  
SELECT cp.Id AS DryerProgramId,cp.CYCLONEPROCESS AS DryerProgram --DryerProgramName    
FROM DOT_GloveRelCyclone grc    
INNER JOIN DOT_FSGloveCode gct    
--ON gct.RECID = grc.GLOVEREFRECID    
ON gct.AVAGLOVECODETABLE_ID = grc.GLOVEREFRECID AND grc.IsDeleted <> 1   
INNER JOIN DOT_FSCycloneProgram cp    
ON grc.CycloneProcessId = cp.Id    
INNER JOIN DOT_FSItemMaster D ON  gct.ItemRecordId = D.Id    
WHERE D.ItemId = @GloveType    
AND cp.STOPPED <> 1  and  cp.IsDeleted = 0  
END    
    
    
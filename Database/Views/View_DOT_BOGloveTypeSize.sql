---- =================================================================  
---- Author:  <Azrul Amin>  
---- Create date: <27-Mar-2018>  
---- Description: View to get tier side and golve sizes in to rows 
---- 21/01/2021  Azrul Amin	 Fixes for performance issue.
---- =================================================================  
CREATE OR ALTER VIEW [dbo].[View_DOT_BOGloveTypeSize]    
AS 
	SELECT 
	 res.Id as ResourceId
	,res.Resource
	,res.[LineNo] as LineId
	,res.TierSide as TierSide
	,bo.BthOrderId
	,bo.ItemId as GloveType
	,bo.Size as GloveSize
	,0 as IsAlternate
	,0 as IsPrintByFormer
	,1 as IsOnline
	,res.Tier as Tier
	FROM 
		DOT_FloorD365BO AS bo WITH (NOLOCK) JOIN 
		DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0
	WHERE bo.ProdPoolId = 'Glove' 
	AND bo.ProdStatus = 'StartedUp'
	AND bo.IsDeleted=0
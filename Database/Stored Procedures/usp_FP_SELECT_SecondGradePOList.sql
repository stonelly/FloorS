    
-- =============================================    
-- Author:  Srikanth Balda    
-- Create date: 20 Sep 2014    
-- Description: Get Active First Grade PO list    
-- 21/01/2021  Azrul Amin	 Fixes for performance issue.
-- =============================================    
CREATE OR ALTER PROCEDURE [dbo].[usp_FP_SELECT_SecondGradePOList]    
 @Location nvarchar(20)    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    -- Insert statements for procedure here    
 select SalesId,ItemType,customerref from VW_AXSOline with(nolock)     
 -- filter based on PlantNo. #Max    
 join DOT_FloorD365BO bo WITH (NOLOCK) on bo.BatchId = CustomerRef and CONFIGURATION = bo.Size and bo.ProdStatus = 'StartedUp'    
 left join DOT_FloorD365BOResource res with (nolock) on res.BatchOrderId = bo.BthOrderId    
 where itemtype = 6 and res.PlantNo = @Location  
 /**    
 select SalesId,ItemType,customerref from VW_AXSOline with(nolock) where itemtype = 'FG2'    
 **/    
END 

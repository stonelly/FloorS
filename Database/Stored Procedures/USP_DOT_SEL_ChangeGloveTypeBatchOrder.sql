-- =============================================  
-- Name:   [dbo].[USP_DOT_SEL_ChangeGloveTypeBatchOrder]
-- Purpose:   <Get Batch Order Number from Glove Code>  
-- =============================================  
-- Change History  
-- Date   Author    Comments  
-- -----  ------    ------------  
-- 18/11/2021  Azrul  SP created  
-- exec USP_DOT_SEL_ChangeGloveTypeBatchOrder 'P6','NB-AB-CRP-70-EC-LBLU-KCL' 
-- =============================================  
CREATE PROCEDURE [dbo].[USP_DOT_SEL_ChangeGloveTypeBatchOrder]  
@location NVARCHAR(10),
@gloveType NVARCHAR(50),
@size NVARCHAR(10)
AS  
BEGIN  
 SET NOCOUNT ON;     
	SELECT BthOrderId as BatchOrder FROM DOT_FloorD365BO with (nolock) WHERE ItemId = @gloveType AND Size = @size
	AND IsDeleted = 0 and ProdStatus = 'StartedUp'
 SET NOCOUNT OFF;   
END 
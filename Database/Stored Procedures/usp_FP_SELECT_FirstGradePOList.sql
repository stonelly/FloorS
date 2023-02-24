-- =============================================  
-- Author:  Srikanth Balda  
-- Create date: 20 Sep 2014  
-- Description: Get Active First Grade PO list  
-- 07/05/2018  Azrul Amin    SP altered.    
-- =====================================================================    
--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_FP_SELECT_FirstGradePOList]') AND type in (N'P', N'PC'))  
--DROP PROCEDURE [dbo].[usp_FP_SELECT_FirstGradePOList]  
--GO  
alter PROCEDURE [dbo].[usp_FP_SELECT_FirstGradePOList]  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
    -- Insert statements for procedure here  
  SELECT SalesId, ItemType, CustomerRef, ProdStatus  
  FROM VW_AXSOline with(nolock)   
  WHERE   
  itemtype = 5 -- =FG  
  AND PreshipmentSamplingPlan in ('1','2','3','4','5','6')  
  AND PalletCapacity > 0  
  GROUP By salesid, ItemType, CustomerRef, ProdStatus  
END  
  
--original sp  
    -- Insert statements for procedure here  
 --select A.SalesId,A.ItemType,A.customerref  from VW_AXSOline A with(nolock)   
 --INNER JOIN SalesTable st with(nolock) ON A.SalesId = st.SalesId  
 --where itemtype = 5 and PRESHIPMENTSAMPLINGPLAN in ('1','2','3','4','5','6') and PalletCapacity > 0  
 --and A.WorkOrderStatus = 2 -- to get Active and Approved SO  
 --AND st.WorkOrderType NOT IN ('3') -- exclude Actual MTS  

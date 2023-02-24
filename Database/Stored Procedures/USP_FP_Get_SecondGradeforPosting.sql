-- ========================================================================================  
-- Name:   [USP_FP_Get_SecondGradeforPosting]  
-- Purpose:   get the Second Grade inner and outer data for posting to AX.  
-- ========================================================================================  
CREATE PROCEDURE [dbo].[USP_FP_Get_SecondGradeforPosting]  
(  
       @internalLotnumber nvarchar(15)  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
      
 select   
FP.internalLotnumber , FP.OuterLotNo, FP.PONumber, FP.itemnumber,FP.PackDate,FP.CasesPacked,FP.PreshipmentCasesPacked,  
FP.manufacturingdate,FP.ExpiryDate,  
POIN.customersize,POIN.ordernumber,POIN.CustomerLotNumber,POIN.CustomerReferenceNumber, bo.BthOrderId AS BatchOrder, r.Resource  
 from finalpacking FP WITH(nolock)  
 join purchaseorderitem POIN WITH(NOLOCK) ON POIN.ponumber = FP.ponumber and POIN.Itemnumber = FP.itemnumber and FP.Size = POIN.ItemSize   
 join DOT_FloorD365BO bo WITH(NOLOCK) ON bo.batchid = POIN.CustomerReferenceNumber and bo.size = fp.Size and bo.ItemId = fp.ItemNumber   
 join DOT_FloorD365BOResource r  WITH (NOLOCK) on r.BatchOrderId = bo.BthOrderId  
 where FP.internallotnumber = @internalLotnumber and bo.ProdStatus = 'StartedUp' and bo.IsDeleted = 0 and r.IsDeleted = 0  
    SET NOCOUNT OFF;  
END  
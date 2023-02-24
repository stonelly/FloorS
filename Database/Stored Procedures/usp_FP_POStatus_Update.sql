  
  
-- =============================================  
-- Author:  Srikanth Balda  
-- Create date: 21 sep 2014  
-- Description: To Validate existance  
  
-- Author:  Pang YS  
-- Create date: 09 Mar 2021  
-- Description: eFS Std point to 9Dot tables update salestable  
-- =============================================  
CREATE PROCEDURE [dbo].[usp_FP_POStatus_Update]   
 -- Add the parameters for the stored procedure here  
 @PONumber nvarchar(20)  
AS  
BEGIN  
DECLARE @SOLintCount int  
DECLARE @PurchaseOrderItem int  
  
DECLARE @CaseCount int  
DECLARE @ItemNumber nvarchar(40)  
DECLARE @ItemSize nvarchar(30)  
DECLARE @ItemCases int  
SELECT ponumber, itemnumber, ItemSize,ItemCases into #Temp from purchaseorderitem where ponumber = @PONumber  
 WHILE((SELECT count(itemnumber) from #temp)>0)  
 BEGIN  
  SELECT @ItemNumber = (select top 1 itemnumber from #Temp)  
  SELECT @ItemSize = (select top 1 ItemSize from #Temp)  
  SELECT @ItemCases = ((select top 1 ItemCases from #Temp))  
  SELECT @CaseCount = count(casenumber) from purchaseorderitemcases   
  WHERE ponumber = @PONumber and itemnumber = @ItemNumber and Size = @ItemSize and internalotnumber is not null  
    
  IF(@ItemCases = @CaseCount)  
  BEGIN  
     UPDATE PURCHASEORDERITEM SET itemstatus = 1   
     WHERE ponumber = @PONumber and itemnumber = @ItemNumber and Itemsize = @ItemSize  
  END  
  DELETE from #temp where ponumber = @Ponumber and itemnumber = @ItemNumber and Itemsize = @ItemSize  
 END  
  
        -- Insert statements for procedure here  
SELECT @SOLintCount = Count(ItemId)  from vw_RPT_axsoline --vw_axsoline --fixed for HSB auto close SO when other size in SalesLine is still available 
WHERE salesid = @PONumber  
  
  
SELECT @PurchaseOrderItem =Count(ItemNumber) from PurchaseOrderItem   
WHERE PONumber = @PONumber and ItemStatus =1  
  
 IF @SOLintCount = @PurchaseOrderItem  
 BEGIN    
   Update PurchaseOrder set POStatus = 'Closed', LastModifiedOn = GETDATE() where PONumber = @PONumber   
   --UPDATE SalesTable set WorkOrderStatus = 3 where SalesId = @PONumber  
   UPDATE SalesTable set DOT_WorkOrderStatus = '3' where SalesId = @PONumber  
 END  
  
drop table #Temp  
  
END  
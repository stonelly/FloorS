/****** Object:  StoredProcedure [dbo].[usp_FP_SELECT_ReprintOuterCasePOList]    Script Date: 30/3/2022 1:59:13 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

  
ALTER PROCEDURE [dbo].[usp_FP_SELECT_ReprintOuterCasePOList]  
(  
	@ConfiguredDayRange INT
)  
AS  
BEGIN  
  
 /*  
  Author  : <Fusionex, SoonSiang>  
  Create date : <23/01/2019>  
  Description : <Get PO List for Reprint Outer Case>  
 */  
  
 SET NOCOUNT ON;  
  
 DECLARE @CurrentDate Date = GETDATE()  
 DECLARE @FirstValidDate Date = DATEADD(d,(1 - @ConfiguredDayRange ),@CurrentDate)  
  
 SELECT vw.SalesId AS PONumber, vw.customerref AS CustomerReferenceNumber  
 FROM PurchaseOrderItem poi  
 INNER JOIN VW_Reprint_AXSOline vw  
 ON poi.PONumber = vw.SalesId  
 AND poi.ItemNumber = vw.ItemId  
 AND poi.ItemSize = vw.HARTALEGACOMMONSIZE  
 INNER JOIN SalesTable s  
 ON s.SalesId = vw.SalesId  
 LEFT JOIN PurchaseOrder po  
 ON po.PONumber = vw.SalesId  
 WHERE  (s.WorkOrderStatus IN (1,2) OR (s.WorkOrderStatus = 3 AND CAST(po.LastModifiedOn AS DATE) BETWEEN @FirstValidDate and @CurrentDate))  
 AND s.WorkOrderType <> 3  
 --GROUP BY vw.SalesId, vw.customerref  

END  
  
GO
-- =============================================  
-- Author: Muhammad Khalid  
-- Create date: 02/05/2018  
-- Description: Get Active Batch Order Data  
-- =============================================  
  
CREATE PROCEDURE [dbo].[usp_FP_PurchaseOrderMTS_Get]   
 -- Add the parameters for the stored procedure here   
 @BONumber nvarchar(20)  
AS  
BEGIN   
 SET NOCOUNT ON;  
select a.BthOrderId  as PONumber,  
'' as INVENTTRANSID,  
a.BatchId as OrderNumber,  
a.LotVerification as BarcodeVerificationRequired,  
--CustomerSpecification,  
a.PreShipmentPlan as PreshipmentPlan,  
b.ItemId as ItemNumber,  
b.NAME as ItemName,  
b.ITEMTYPE,  
a.QtySched as ItemCases,  
'' as CustomerName,  
a.INNERLABELSET as InnersetLayout,  
a.OuterLabelSetNo as OuterSetLayout,  
a.Size as CustomerSize,  
a.PrintingSize as CUstomerSizeDesc,  
a.GrossWeight,  
a.NETWEIGHT as NettWeight,  
a.InnerboxinCaseNo as CaseCapacity,   
a.PalletCapacity,  
'' as CustomerLotNumber ,  
'' as CustomerReferenceNumber,  
b.GloveCode as GloveCode,  
a.BthOrderId as BatchOrder,  
a.ProdStatus,  
a.ManufacturingDateOn as MANUFACTURINGDATEBASIS,  
1 as POStatus, -- ='Backorder'  
a.HartalegaCommonSize as ItemSize ,  
a.GlovesInnerboxNo as InnerBoxCapacity,  
a.InnerProductCode,  
a.OuterProductCode,  
a.Expiry,  
--BrandName   
a.REFERENCE1  as ProductReferenceNumber,  
a.REFERENCE2 ,  
a.GCLabel as GCLabelPrintingRequired,  
a.AlternateGloveCode1,  
a.AlternateGloveCode2,  
a.AlternateGloveCode3,  
a.SPECIALINNERCODE,  
a.SpecialInnerCharacter as SPECIALINNERCODECHARACTER,  
a.SchedStart as SHIPPINGDATEREQUESTED,  
a.SchedStart as RECEIPTDATEREQUESTED,  
-- Label set Optimization project  
a.InnerDateFormat as InnerLabelSetDateFormat,  
a.OuterDateFormat as OuterLabelSetDateFormat  
from DOT_FloorD365BO a  
inner join DOT_FSItemMaster b on a.ItemId = b.ItemId   
--inner join DOT_FSBrandHeaders c on b.ItemId = c.ItemId  
--inner join DOT_FSBrandLines d on d.ItemId = c.ItemId and a.Size = d.customersize 
WHERE a.BthOrderId = @BONumber and a.MadeToStockStatus = 1
END  
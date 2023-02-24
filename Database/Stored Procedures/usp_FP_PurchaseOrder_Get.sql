  
-- =============================================  
-- Author: Srikanth Balda  
-- Create date: 4/5/2014  
-- Description: Get Active SOLine Data  
  
-- Author: Pang Yik Siu  
-- Modified date: JAN 2018  
-- Description: Add two columns: InnerLabelSetDateFormat, OuterLabelSetDateFormat  
  
-- Author: Pang Yik Siu  
-- Modified date: Mar 2020    
-- Description: Modify CustomerSize from Configuration to [CustomerSize]    
    
-- Author: Pang Yik Siu    
-- Modified date: 12 Jun 2020    
-- Description: Added 1 column BARCODE for FP VIsion project    
-- =============================================    
    
alter PROCEDURE [dbo].[usp_FP_PurchaseOrder_Get]     
 -- Add the parameters for the stored procedure here     
 @PONumber nvarchar(20),    
 @ItemType INT    
AS    
BEGIN     
 SET NOCOUNT ON;    
 select SalesId  as PONumber,INVENTTRANSID,    
 PurchOrderFormNum as OrderNumber,    
 InnerVerification as BarcodeVerificationRequired,    
 --CustomerSpecification,    
 PreshipmentSamplingPlan as PreshipmentPlan,    
 a.ItemId as ItemNumber,    
 ItemName as ItemName,    
 ItemType,    
 SalesQty as ItemCases,    
 SalesName as CustomerName,    
 a.InnerLabelSet as InnersetLayout,    
 a.OuterLabelSet as OuterSetLayout,    
 --Configuration as CustomerSize, // Change to VW_AXSOline.CustomerSize    
 a.CustomerSize as CustomerSize,    
 --ConfigurationName as CUstomerSizeDesc, change on 26 Nov 2018    
 a.PrintingSize as CUstomerSizeDesc, -- for outer label printing, 26 Nov 2018 Max He     
 a.GrossWeight,    
 a.NetWeight as NettWeight,    
 NumberInnerBoxInOuter as CaseCapacity,     
 a.PalletCapacity,    
 CustomerLotId as CustomerLotNumber ,    
 Customerref as CustomerReferenceNumber,    
 GloveCode as GloveCode,    
 bo.BthOrderId as BatchOrder,    
 bo.ProdStatus,    
 ManufacturingDateBasis,    
 SalesStatus as POStatus,    
 a.HartalegaCommonSize as ItemSize ,    
 NumberGlovesInnerbox as InnerBoxCapacity,    
 a.InnerProductCode as InnerProductCode,    
 a.OuterProductCode as OuterProductCode,    
 a.Expiry,    
 --BrandName     
 a.Reference1  as ProductReferenceNumber,    
 a.Reference2 ,    
 OuterVerification as GCLabelPrintingRequired,    
 a.AlternateGloveCode1,    
 a.AlternateGloveCode2,    
 a.AlternateGloveCode3,    
 a.SpecialInnerCode,    
 SpecialInnerCodeCharacter,    
 ShippingDateRequested,    
 ReceiptDateRequested,    
 -- Label set Optimization project    
 a.InnerDateFormat as InnerLabelSetDateFormat,    
 a.OuterDateFormat as OuterLabelSetDateFormat,    
 HSB_CustPODocumentDate,    
 HSB_CustPORecvDate,    
 BARCODE,    
 BARCODEOUTERBOX,    
 VisionURL    
 FROM VW_AXSOline as a    
 JOIN DOT_FloorD365BO bo WITH (NOLOCK) on bo.BatchId = CustomerRef and CONFIGURATION = bo.Size and bo.ProdStatus = 'StartedUp' and a.ItemId = bo.ItemId    
 LEFT JOIN (SELECT ItemId, HartalegaCommonSize, VisionURL FROM DOT_FSBrandLines WITH(nolock) WHERE IsDeleted = 0) bl     
  ON  bl.ItemId = a.ItemId and bl.HartalegaCommonSize = a.[CONFIGURATION]    
 WHERE salesId = @ponumber AND a.ItemType = @ItemType and bo.IsDeleted = 0    
 --where PreshipmentPlan is not null and PalletCapacity is not null and ManufacturingOrder = 1 and Postatus = 0    

 /*
 --***** start original SP *****
 select SalesId  as PONumber,INVENTTRANSID,  
PurchOrderFormNum as OrderNumber,  
INNERVERIFICATION as BarcodeVerificationRequired,  
--CustomerSpecification,  
PRESHIPMENTSAMPLINGPLAN as PreshipmentPlan,  
ItemId as ItemNumber,  
ITEMNAME as ItemName,  
ITEMTYPE,  
SalesQty as ItemCases,  
SalesName as CustomerName,  
INNERLABELSET as InnersetLayout,  
OUTERLABELSET as OuterSetLayout,  
CONFIGURATION as CustomerSize,  
CONFIGURATIONNAME as CUstomerSizeDesc,  
GrossWeight,  
NETWEIGHT as NettWeight,  
NUMBERINNERBOXINOUTER as CaseCapacity,   
PalletCapacity,  
CUSTOMERLOTID as CustomerLotNumber ,  
Customerref as CustomerReferenceNumber,  
GloveCode as GloveCode,  
MANUFACTURINGDATEBASIS,  
SALESSTATUS as POStatus,  
HartalegaCommonSize as ItemSize ,  
NUMBERGLOVESINNERBOX as InnerBoxCapacity,  
INNERPRODUCTCODE as InnerProductCode,  
OUTERPRODUCTCODE as OuterProductCode,  
Expiry,  
--BrandName   
REFERENCE1  as ProductReferenceNumber,REFERENCE2 ,  
OUTERVERIFICATION as GCLabelPrintingRequired,  
AlternateGloveCode1,  
AlternateGloveCode2,  
AlternateGloveCode3,  
SPECIALINNERCODE,  
SPECIALINNERCODECHARACTER,  
SHIPPINGDATEREQUESTED,  
RECEIPTDATEREQUESTED,  
STShippingDateConfirmed,  
ManufacturingDateETD,  
-- Label set Optimization project  
INNERLABELSETDATEFORMAT as InnerLabelSetDateFormat,  
OUTERLABELSETDATEFORMAT as OuterLabelSetDateFormat,  
-- HSB_CustPODocumentDate and HSB_CustPORecvDate (29Jan2019)  
HSB_CustPODocumentDate as HSB_CustPODocumentDate,  
HSB_CustPORecvDate as HSB_CustPORecvDate  
FROM VW_AXSOline WHERE salesId = @ponumber AND ItemType = @ItemType  

*/   

END  
  

  
-- ==================================================  
-- Author:  Azrul Amin 
-- Create date: 11 Feb 2022  
-- Description: update MTS Status in Batch Order table  
-- ===================================================  
CREATE PROCEDURE  [dbo].[USP_DOT_BatcOrderMTS_Update]  
(  
 -- Add the parameters for the stored procedure here  
 @BatchOrderNo nvarchar(150),  
 @MTSStatus int,   
 @LastModifiedBy int
 )  
AS  
BEGIN  
 UPDATE DOT_FloorD365BO set MadeToStockStatus = @MTSStatus, LastModifierUserId = @LastModifiedBy, LastModificationTime = GETDATE()
 where BthOrderId = @BatchOrderNo and WarehouseId = 'MTS-FG'   

 IF @MTSStatus = 1
 BEGIN
  update a set a.LotVerification = b.LotVerification
 			  ,a.PreShipmentPlan = b.PreShipmentPlan
 			  ,a.InnerLabelSet = b.InnerLabelSet
   			  ,a.OuterLabelSetNo = b.OuterLabelSetNo
   			  ,a.PrintingSize = c.PrintingSize
   			  ,a.GrossWeight = c.GrossWeight
   			  ,a.NetWeight = c.NetWeight
   			  ,a.InnerboxinCaseNo = c.InnerboxinCaseNo
   			  ,a.PalletCapacity = b.PalletCapacity
 			  ,a.ManufacturingDateOn = b.ManufacturingDateOn
 			  ,a.HartalegaCommonSize = c.HartalegaCommonSize
 			  ,a.GlovesInnerboxNo = c.GlovesInnerboxNo
 			  ,a.InnerProductCode = c.InnerProductCode
 			  ,a.OuterProductCode = c.OuterProductCode
 			  ,a.Expiry = b.Expiry
 			  ,a.Reference1 = c.Reference1
 			  ,a.Reference2 = c.Reference2
 			  ,a.GCLabel = b.GCLabel
 			  ,a.AlternateGloveCode1 = b.AlternateGloveCode1
 			  ,a.AlternateGloveCode2 = b.AlternateGloveCode2
 			  ,a.AlternateGloveCode3 = b.AlternateGloveCode3
 			  ,a.SpecialInnerCode = b.SpecialInnerCode
 			  ,a.SpecialInnerCharacter = b.SpecialInnerCharacter
 			  ,a.InnerDateFormat = b.InnerDateFormat
 			  ,a.OuterDateFormat = b.OuterDateFormat
  
  from DOT_FloorD365BO a with (nolock) 
  join DOT_FSBrandHeaders b with (nolock) on a.ItemId = b.ItemId
  join DOT_FSBrandLines c with (nolock) on a.ItemId = c.ItemId and c.CustomerSize = a.Size
  where BthOrderId = @BatchOrderNo and a.IsDeleted = 0 and b.IsDeleted = 0 and c.IsDeleted = 0 
							
 END						
END
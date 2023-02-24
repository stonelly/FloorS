-- =========================================================================      
-- Name:   USP_DOT_BatchOrderMTSDetails_Get      
-- Purpose:   Get MTS Batch Order details from master grid selection      
-- =========================================================================        
-- Change History        
-- Date    Author   Comments        
-- -----   ------   --------------------------------------------------------        
-- 10/02/2022  Azrul Amin    SP created.    
-- =========================================================================        
CREATE PROCEDURE [dbo].[USP_DOT_BatchOrderMTSDetails_Get]        
(       
 @BatchOrderNo varchar(150)
)       
AS        
BEGIN         
 SET NOCOUNT ON;     
  
 SELECT AlternateGloveCode1, AlternateGloveCode2, AlternateGloveCode3, DOTCustomerLotID, Expiry, GCLabel, GlovesInnerboxNo,
		GrossWeight, HartalegaCommonSize, InnerDateFormat, InnerLabelSet, InnerProductCode, InnerboxinCaseNo, LotVerification,
		ManufacturingDateOn, NetWeight, OuterDateFormat, OuterLabelSetNo, OuterProductCode, PalletCapacity, PreShipmentPlan,
		PrintingSize, Reference1, Reference2, SpecialInnerCharacter, SpecialInnerCode, WarehouseId, 
		case when MadeToStockStatus = 0 and WarehouseId = 'MTS-FG' then 'Open'
			 when MadeToStockStatus = 1 and WarehouseId = 'MTS-FG' then 'Approved'
		else 'None' end as MadeToStockStatus
 FROM DOT_FloorD365BO WITH (NOLOCK) WHERE BthOrderId = @BatchOrderNo
  
 SET NOCOUNT OFF;        
END 
-- =====================================================================    
-- Name:   USP_GET_BatchOrderBySerialNo  
-- Purpose:   Get Batch Order No from Serial No    
-- =====================================================================    
-- Change History    
-- Date    Author   Comments    
-- -----   ------   ----------------------------------------------------    
-- 24/12/2021  Azrul Amin    SP created.    
-- ===================================================================== 
CREATE PROCEDURE [dbo].[USP_GET_BatchOrderBySerialNo]    
(    
 @serialNo Decimal
)    
AS    
BEGIN    
	SELECT BthOrder FROM DOT_FloorD365HRGLOVERPT WITH (NOLOCK) WHERE SerialNo=@serialNo    
END  
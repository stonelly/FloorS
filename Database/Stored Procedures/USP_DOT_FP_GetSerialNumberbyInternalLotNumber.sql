-- =======================================================     
-- Name:              USP_DOT_FP_GetSerialNumberbyInternalLotNumber]    
-- Purpose:           Get Serial Number for SMBP   
-- =======================================================     
-- Change History     
-- Date         Author		Comments     
-- -----        ------		-----------------------------     
-- 8 Aug 2022   Azrul		SP created.     
-- =======================================================    
    
CREATE PROCEDURE [dbo].[USP_DOT_FP_GetSerialNumberbyInternalLotNumber]     
 @InternalLotNumber varchar(50)    
AS    
BEGIN    
        
 SELECT B.SerialNumber    
 FROM Finalpacking FP WITH (NOLOCK) JOIN FinalPackingBatchInfo FPB WITH (NOLOCK) ON FP.InternalLotNumber = FPB.InternalLotNumber    
 JOIN Batch B ON FPB.SerialNumber = B.SerialNumber    
 WHERE FP.InternalLotNumber = @InternalLotNumber    
    
END    
    
    
    
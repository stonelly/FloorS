  
-- ================================================================================  
-- Name:   [dbo].[USP_GET_CheckIsPTPF]  
-- Purpose:   <Checks whether this batch is PTPF glove  
-- ================================================================================  
-- Change History  
-- Date   Author      Comments  
-- ---------- ------------    ------------------------------------------------  
-- 23/07/2020  Pang Yik Siu    SP created  
-- exec USP_GET_CheckIsPTPF '2200635664'  
  
-- ================================================================================  
CREATE PROCEDURE [dbo].[USP_GET_CheckIsPTPF]  
(  
 @SerialNumber numeric  
)  
AS  
BEGIN  
  
 --DECLARE @SerialNumber numeric = 2200635664  
 SET NOCOUNT ON;  
 DECLARE @isPTPF BIT = 0  
  
 IF EXISTS (SELECT 1 FROM Batch (nolock) WHERE SerialNumber = @SerialNumber)  
 BEGIN   
  SET @isPTPF = (SELECT ISNULL(c.PTGLOVE, 0) FROM Batch (nolock) b   
      LEFT JOIN AX_AVAGLOVECODETABLE_EXTENSION (NOLOCK) c ON c.GLOVECODE = b.GloveType  
      WHERE b.SerialNumber = @SerialNumber)  
 END  
  
 SELECT @isPTPF  
END  
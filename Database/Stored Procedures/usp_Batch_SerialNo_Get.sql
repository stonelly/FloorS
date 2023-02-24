  
  
-- =============================================  
-- Name:   [dbo].[usp_Batch_SerialNo_Get]  
-- Purpose:   <Fetches BatchNumber,Glove type and Size from the batch table>  
-- =============================================  
-- Change History  
-- Date   Author    Comments  
-- -----  ------    -----------------------------  
-- 24/06/2014  Kishan Dubal  SP created.  
-- 16/07/2014  Kishan Dubal  Changes after review by Narendra Gurram  
-- 03/01/2019  Azrul    Populate QCType info for checkings  
-- 06/01/2019  Max     add BatchType info for further checking  
-- 23/07/2020 Pang    Add QCTYPE for further checking  
-- =============================================  
CREATE PROCEDURE [dbo].[usp_Batch_SerialNo_Get]  
(  
@serialNo NUMERIC  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
 SELECT b.BatchNumber, b.GloveType, b.Size, b.QCType, qc.[Description],b.BatchType  
 FROM Batch b with(nolock)  
 Left Outer Join DOT_FSQCTypeTable qc with(nolock) on b.qctype = qc.qctype  
 WHERE SerialNumber = @serialNo   
 AND BatchType IS NOT NULL AND BatchNumber IS NOT NULL  
END  
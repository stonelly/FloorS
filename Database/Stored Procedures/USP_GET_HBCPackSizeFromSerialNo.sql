-- =======================================================    
-- Name:   USP_GET_HBCPackSizeFromSerialNo  
-- Purpose:   Get HBC Packing Size from Serial Number  
-- =======================================================    
-- Change History    
-- Date         Author     Comments    
-- -----        ------     -----------------------------    
-- 27 Nov 2018  Azrul      SP created.    
 --14 Apr 2022  Azrul      Cater Open Batch.    
-- =======================================================    
CREATE OR ALTER PROCEDURE [dbo].[USP_GET_HBCPackSizeFromSerialNo]    
 @SerialNumber  BIGINT
AS    
BEGIN     
    
 SET NOCOUNT ON    
    
  --Emergency change by Azman 17/02  
  --SELECT STUFF((SELECT ', ' + CONVERT(NVARCHAR,a.PackingSz) FROM dbo.DOT_FloorD365HRGLOVERPT a WITH (NOLOCK)  
  --WHERE a.SerialNo = @SerialNumber  
  --FOR XML path('') ), 1, 2, '') AS PackingSize  
  --FROM dbo.DOT_FloorD365HRGLOVERPT WITH (NOLOCK)  
  --WHERE SerialNo = @SerialNumber  
  --GROUP BY SerialNo  
  
  Declare @PackSize Nvarchar(100)

  SELECT @PackSize = STUFF((SELECT ', ' + CONVERT(NVARCHAR,a.PackingSz) FROM dbo.DOT_FloorD365HRGLOVERPT a WITH (NOLOCK)  
  WHERE a.SerialNo = CAST(@SerialNumber as nvarchar(20))  
  FOR XML path('') ), 1, 2, '') --AS PackingSize  
  FROM dbo.DOT_FloorD365HRGLOVERPT WITH (NOLOCK)  
  WHERE SerialNo = CAST(@SerialNumber as nvarchar(20))  
  GROUP BY SerialNo

  --Get packingSize from QAI for 1 year open batch
  IF (@PackSize IS NULL) OR (LEN(@PackSize) < 0) 
  BEGIN
	select TOP 1 @PackSize = ISNULL(PackingSize, 0) from QAI WITH (NOLOCK) where serialnumber = @SerialNumber and QITestResult is null
  END
  
  select @PackSize AS PackingSize 
END  
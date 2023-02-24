-- =======================================================    
-- Name:   USP_GET_HBCInBoxFromSerialNo  
-- Purpose:   Get HBC Inner Box from Serial Number  
-- =======================================================    
-- Change History    
-- Date         Author     Comments    
-- -----        ------     -----------------------------    
-- 27 Nov 2018  Azrul      SP created.    
 --14 Apr 2022  Azrul      Cater Open Batch.    
-- =======================================================    
CREATE OR ALTER PROCEDURE [dbo].[USP_GET_HBCInBoxFromSerialNo]    
 @SerialNumber  BIGINT    
AS    
BEGIN     
    
 SET NOCOUNT ON    

   Declare @InnerBox Nvarchar(100)

  SELECT @InnerBox = STUFF((SELECT ', ' + CONVERT(NVARCHAR,a.InBox) FROM dbo.DOT_FloorD365HRGLOVERPT a WITH (NOLOCK)  
  WHERE a.SerialNo = @SerialNumber  
  FOR XML path('') ), 1, 2, '') --AS InnerBox  
  FROM dbo.DOT_FloorD365HRGLOVERPT WITH (NOLOCK)  
  WHERE SerialNo = @SerialNumber  
  GROUP BY SerialNo  

  --Get packingSize from QAI for 1 year open batch
  IF (@InnerBox IS NULL) OR (LEN(@InnerBox) < 0) 
  BEGIN
	select TOP 1 @InnerBox = ISNULL(InnerBox, 0) from QAI WITH (NOLOCK) where serialnumber = @SerialNumber and QITestResult is null
  END
  
  select @InnerBox AS InnerBox 

END  
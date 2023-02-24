  
-- =======================================================  
-- Name:             USP_GET_LAST_QI_RESULT  
-- Purpose:          Get Last Batch Audit Log 
-- =======================================================  
-- Change History  
-- Date         Author     Comments  
-- -----        ------     -----------------------------  
-- 21/11/2022   Azrul    SP created.  
-- Exec USP_GET_LAST_QI_RESULT '1220372650'
-- =======================================================  
CREATE PROCEDURE [dbo].[USP_GET_LAST_QI_RESULT]  
(  
 @SerialNumber decimal  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 -- To check if latest QI test result, return empty string to system if any  
  IF NOT EXISTS(SELECT TOP 1 1 FROM QAI WITH (NOLOCK) 
				where SerialNumber = @SerialNumber and QITestResult is not null 
				and QAIScreenName = 'ScanQITestResult')  
    BEGIN  
		SELECT ''  
    END  
  ELSE 
    BEGIN    
		SELECT TOP 1 QITestResult from QAI WITH (NOLOCK) 
		where SerialNumber = @SerialNumber and QITestResult is not null 
		and QAIScreenName = 'ScanQITestResult' 
		Order By QAIDate DESC
    END    
END   
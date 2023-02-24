    
CREATE PROCEDURE [dbo].[USP_DOT_FPExempt_SNValidation]        
(        
     @serialNumber numeric(10,0)    
)        
AS        
BEGIN        
 SET NOCOUNT ON;        
 declare @serialStr nvarchar(20) = CAST(@SerialNumber as nvarchar(20))     
         
    --check if there is exemption. if yes, will always pass     
   IF Exists(select 1 from FP_SerialNo_Exemption where SerialNumber=@serialNumber)    
   BEGIN        
    Select 'Pass'        
   END        
   ELSE        
   BEGIN        
    Select 'FAIL'        
   END          
  SET NOCOUNT OFF;        
END  
 --=========================================================    
 --Name:   USP_HBCTotalPcs_Get  
 --Purpose:   Get HBC Total Quantity by Serial No   
 --=========================================================    
 --Change History    
 --Date    Author   Comments    
 -------   ------   ----------------------------------------    
 --27/05/2018  Azrul Amin    SP created.    
 --14/04/2022  Azrul Amin    Cater Open Batch.    
 --=========================================================    
CREATE OR ALTER PROCEDURE [dbo].[USP_HBCTotalPcs_Get]    
(   
  @SerialNo varchar(150)
)   
AS    
BEGIN     
 SET NOCOUNT ON;   
 
 SELECT ISNULL(SUM(PackingSz*InBox),(SELECT TotalPcs from Batch WITH (NOLOCK) where SerialNumber = @SerialNo)) as TotalPcs 
 from DOT_FloorD365HRGLOVERPT WITH (NOLOCK) where SerialNo = @SerialNo

 SET NOCOUNT OFF;    
END  


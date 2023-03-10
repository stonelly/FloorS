-- =========================================================================    
-- Name:   Ufn_DOT_GET_BATCHSEQUENCE  
-- Purpose:   Function for get staging next sequence based on AXPostingLog    
-- =========================================================================    
-- Change History    
-- Date    Author   Comments    
-- -----   ------   ----------------------------------------    
-- 23/12/2021  Max He    Function creation
-- =========================================================================    

Create or ALTER FUNCTION [dbo].[Ufn_DOT_GET_BATCHSEQUENCE]    
(  
  @BATCHNUMBER nvarchar(20)
)   
RETURNS INT             
AS            
BEGIN   
 DECLARE @Sequence INT
 DECLARE @SerialNumber numeric(15, 0)

SET @SerialNumber=case when ISNUMERIC(@BATCHNUMBER)=0 then null else @BATCHNUMBER end;

IF @SerialNumber is not null
begin
 SELECT @Sequence = COUNT(SerialNumber) +1 FROM dbo.AXPostingLog WHERE SerialNumber=@SerialNumber  
end
ELSE
begin
 set @Sequence=0
end
 RETURN @Sequence  
END  
-- =========================================================================    
-- Name:   Ufn_DOT_GET_IsConsolidated  
-- Purpose:   Function for get open batch flag based on AXPostingLog and DOT_LocationConsoleMaster 
-- =========================================================================    
-- Change History    
-- Date    Author   Comments    
-- -----   ------   ----------------------------------------    
-- 23/12/2021  Max He    Function creation
-- =========================================================================    

Create or ALTER FUNCTION [dbo].[Ufn_DOT_GET_IsConsolidated]    
(  
 @BATCHNUMBER nvarchar(20),
 @PlantNo NVARCHAR(20)
)   
RETURNS BIT             
AS            
BEGIN   
DECLARE @Sequence INT
DECLARE @IsConsolidated BIT
DECLARE @SerialNumber numeric(15, 0)

SET @SerialNumber=case when isnull(@BATCHNUMBER,'')='' or ISNUMERIC(@BATCHNUMBER)=0 then '0' else @BATCHNUMBER end;

SELECT @Sequence = COUNT(SerialNumber) FROM dbo.AXPostingLog WITH (NOLOCK) WHERE SerialNumber=@SerialNumber  

	IF @Sequence = 0
	BEGIN
		IF EXISTS (SELECT 1 FROM DOT_LocationConsoleMaster WITH (NOLOCK) WHERE LocationName = @PlantNo AND IsConsole = 1)
		BEGIN
			SET @IsConsolidated = 1
		END
		ELSE
		BEGIN
			SET @IsConsolidated = 0
		END
	END
	ELSE
	BEGIN
		IF EXISTS (SELECT 1 FROM AXPostingLog WITH (NOLOCK) WHERE SerialNumber = @SerialNumber AND IsConsolidated = 1)
		BEGIN
			SET @IsConsolidated = 1
		END
		ELSE
		BEGIN
			SET @IsConsolidated = 0
		END
	END
 RETURN @IsConsolidated  
END 
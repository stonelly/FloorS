
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>  

-- Author:  Pang YS   
-- Modified date: 04/01/2022     
-- Description: Added checking for Inv360_CompletedPallet, Inventory360 (MTS)   

-- =============================================    
ALTER PROCEDURE [dbo].[usp_FG_GetDateScannedPalletID]    
 -- Add the parameters for the stored procedure here    
 @PalletID  varchar(50),
 @FromPlant varchar(5),
 @ScannedBy varchar(400),
 @MachineName varchar(400)
  
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 --DECLARE @retval int    
	SET NOCOUNT ON;    
    
	DECLARE @EXISTINGSCANNED DATETIME = (SELECT MAX(DateScanned) FROM FGReceivedPallet(NOLOCK) WHERE PalletID = @PalletID)
	DECLARE @ISABLETOSCAN BIT = 0
	--DECLARE @ISMTO BIT = 0
	--DECLARE @ISMTS BIT = 0  

	--------------------------------------------------------------------------------------------------------
	-- MTO(make to order look for EWN_CompletedPallet
	IF EXISTS(SELECT 1 FROM EWN_CompletedPallet(NOLOCK) WHERE PalletId = @PalletID AND DateScanned IS NULL AND DateStockOut IS NULL)    
	BEGIN    
		SET @ISABLETOSCAN = 1    
		--SET @ISMTO = 1    
	END
	-- MTS(make to stock look for Inv360_CompletedPallet
	ELSE IF EXISTS(SELECT 1 FROM Inv360_CompletedPallet(NOLOCK) WHERE PalletId = @PalletID AND DateScanned IS NULL)    
	BEGIN    
		SET @ISABLETOSCAN = 1    
		--SET @ISMTS = 1
	END
	--------------------------------------------------------------------------------------------------------
	
	
	IF (@EXISTINGSCANNED IS NOT NULL)
	BEGIN
		IF (@ISABLETOSCAN = 1)
		BEGIN
			SELECT 1
		END
		ELSE
		BEGIN
			--EXEC usp_FG_InsertScanError @PalletID,'PalletID EWN_CompletedPallet column DateScanned is NOT NULL', @FromPlant, @ScannedBy, @MachineName
			SELECT 0
		END
	END
	ELSE
	BEGIN
		SELECT 1
	END
END
GO
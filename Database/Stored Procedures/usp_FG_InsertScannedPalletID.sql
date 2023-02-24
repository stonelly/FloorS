  
-- =============================================          
-- Author:  <Author,,Name>          
-- Create date: <Create Date,,>          
-- Description: <Description,,>          
  
-- Author:  Pang YS     
-- Modified date: 04/01/2022       
-- Description: Added scan for Inventory360 (MTS)     
  
-- exec [InsertScannedPalletID] 'ABC123','2018-01-03'   
-- exec [usp_FG_InsertScannedPalletID]   'HH010024', 'P6' , 'SYSTEM', 'LOCAL'  
-- =============================================          
  
alter PROCEDURE [dbo].[usp_FG_InsertScannedPalletID]          
 -- Add the parameters for the stored procedure here          
 @PalletID  varchar(50),          
 @FromPlant  varchar(5),          
 @ScannedBy  varchar(400),          
 @MachineName varchar(400)          
AS          
BEGIN          
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.          
 SET NOCOUNT ON;          
      
 DECLARE @EXISTINGSCANNED DATETIME = (SELECT MAX(DateScanned) FROM FGReceivedPallet(NOLOCK) WHERE PalletID = @PalletID)      
 DECLARE @ISABLETOSCAN BIT = 0      
 DECLARE @ISMTO BIT = 0      
 DECLARE @ISMTS BIT = 0      
 DECLARE @ReturnValue int      
 DECLARE @CURRENTDATE DATETIME = GETDATE()      
       
 --------------------------------------------------------------------------------------------------------  
 -- MTO(make to order look for EWN_CompletedPallet  
 IF EXISTS(SELECT 1 FROM EWN_CompletedPallet(NOLOCK) WHERE PalletId = @PalletID AND DateScanned IS NULL AND DateStockOut IS NULL)      
 BEGIN      
  SET @ISABLETOSCAN = 1      
  SET @ISMTO = 1      
 END  
 -- MTS(make to stock look for Inv360_CompletedPallet  
 ELSE IF EXISTS(SELECT 1 FROM Inv360_CompletedPallet(NOLOCK) WHERE PalletId = @PalletID AND DateScanned IS NULL)      
 BEGIN      
  SET @ISABLETOSCAN = 1      
  SET @ISMTS = 1  
 END  
 --------------------------------------------------------------------------------------------------------  
   
 IF (@EXISTINGSCANNED IS NOT NULL)      
 BEGIN      
  IF (@ISABLETOSCAN = 1)      
  BEGIN      
   EXEC [dbo].[USP_DOT_FSPostD365FromFGReceivedPallet] @PalletID,@FromPlant,@CURRENTDATE, @ReturnValue output      
   IF (@ReturnValue = 1)      
   BEGIN      
    INSERT INTO FGReceivedPallet(PalletID, FromPlant, DateScanned, IsProcessed, ScannedBy, MachineName)      
     VALUES(@PalletID, @FromPlant, @CURRENTDATE, 0, @ScannedBy, @MachineName)    
  IF (@ISMTO = 1)     
  UPDATE EWN_CompletedPallet   
   SET DateScanned = @CURRENTDATE      
   WHERE PalletId = @PalletID      
   AND DateStockOut IS NULL      
  ELSE IF (@ISMTS = 1)     
  UPDATE Inv360_CompletedPallet   
   SET DateScanned = @CURRENTDATE  
   WHERE Id IN (SELECT TOP 1 Id FROM Inv360_CompletedPallet(nolock) WHERE PalletId = @PalletID ORDER BY DateCompleted DESC)  
   -- PalletId = @PalletID  
    SELECT 1      
   END      
   ELSE      
   BEGIN      
    RAISERROR ('Pallet failed to post to web admin. Please try to rescan again', 11, 1)      
   END    
  END      
  ELSE      
  BEGIN      
   RAISERROR ('PalletID EWN_CompletedPallet(MTO) or Inv360_CompletedPallet(MTS) column DateScanned is NOT NULL', 11, 3)      
  END      
 END      
 ELSE      
 BEGIN      
  IF (@ISABLETOSCAN = 1) -- EwareNavi, MTS dependent      
  BEGIN      
   EXEC [dbo].[USP_DOT_FSPostD365FromFGReceivedPallet] @PalletID,@FromPlant,@CURRENTDATE, @ReturnValue output    
   --SELECT 2  
   IF (@ReturnValue = 1)      
   BEGIN      
    INSERT INTO FGReceivedPallet(PalletID, FromPlant, DateScanned, IsProcessed, ScannedBy, MachineName)      
     VALUES(@PalletID, @FromPlant, @CURRENTDATE, 0, @ScannedBy, @MachineName)  
  IF (@ISMTO = 1)     
  UPDATE EWN_CompletedPallet   
   SET DateScanned = @CURRENTDATE      
   WHERE PalletId = @PalletID      
   AND DateStockOut IS NULL      
  ELSE IF (@ISMTS = 1)     
  UPDATE Inv360_CompletedPallet   
   SET DateScanned = @CURRENTDATE  
   WHERE Id IN (SELECT TOP 1 Id FROM Inv360_CompletedPallet(nolock) WHERE PalletId = @PalletID ORDER BY DateCompleted DESC)  
   -- PalletId = @PalletID  
    SELECT 1      
   END      
   ELSE      
   BEGIN      
    RAISERROR ('Pallet failed to post to web admin. Please try to rescan again.', 11, 1)      
   END      
  END      
  ELSE      
  BEGIN      
   -- No record in dbo.FGReceivedPallet  
   RAISERROR ('Pallet has not been evaluated in EwareNavi or Inventory360. Please check if the pallet exists in the EWN_CompletedPallet and Inv360_CompletedPallet table', 11, 2)      
  END      
 END       
END  
-- ==================================================================================    
-- Name:   Usp_DOT_PNBC_Print_Save  
-- Purpose:  Save & Print Normal Batch Card  
-- ==================================================================================    
-- Change History    
-- Date    Author   Comments    
-- -----   ------   -----------------------------------------------------------------    
-- 31/12/2021  Azrul Amin    SP created.    
-- ==================================================================================    
  
CREATE PROCEDURE [dbo].[Usp_DOT_PNBC_Print_Save]    
(    
@UserId VARCHAR(10),  
@ShiftId VARCHAR(10),  
@Line VARCHAR(10),  
@BatchCardDate DATETIME,  
@ModuleId INT,  
@SubModuleID INT,  
@SiteNumber INT,  
@WorkStationNumber INT,  
@Resource VARCHAR(50),  
@BatchOrder VARCHAR(50),  
@GloveCode VARCHAR(50),  
@Size VARCHAR(50),  
@BatchWeight DECIMAL(18,3),  
@Quantity INT,
@authorizedBy NVARCHAR(25),      
@authorizedFor INT      
)     
AS    
BEGIN    
    
DECLARE @LocationId INT    
DECLARE @SerialNumber Numeric  
DECLARE @BatchNumber VARCHAR(50)  
DECLARE @GloveCategory VARCHAR(50)  
  
SELECT @LocationId = locationid from workstationmaster with (nolock) where isdeleted=0 and workstationid= @WorkStationNumber    
SELECT @SerialNumber = dbo.Ufn_SerailNumberPart(@SiteNumber,@BatchCardDate) + dbo.Ufn_IntToChar((NEXT VALUE FOR DBO.SerialNumberSeq),7)  
SELECT @BatchNumber = dbo.Ufn_BatchNumber(@BatchCardDate,@Line,@Size)  
SELECT @GloveCategory = ISNULL(dbo.Ufn_DOT_GetGloveCategory(@GloveCode),'')  
  
BEGIN TRANSACTION;    
    
BEGIN TRY    
SET NOCOUNT ON  
  
--insert into batch table  
INSERT INTO dbo.Batch(SerialNumber,BatchNumber,ShiftId,LineId,GloveType,Size,TierSide,BatchWeight,BatchCardDate,ReWorkCount,IsOnline,TotalPCs,ModuleId,SubModuleID,LocationId,BatchType,     
LastModifiedOn,WorkstationId,batchcardcurrentlocation,AuthorizedBy,AuthorizedFor)       
SELECT @SerialNumber,@BatchNumber,@ShiftId,@Line,@GloveCode,@Size,substring(@Resource,6,2) AS TierSide,@BatchWeight,@BatchCardDate,0,0,@Quantity,@ModuleId,@SubModuleID,@LocationId,'T',  
GETDATE(),@WorkStationNumber,'PN',@authorizedBy,@authorizedFor  
    
--insert batch details into staging table  
INSERT INTO dbo.DOT_FloorD365HRGLOVERPT(SeqNo,BatchCardNumber,BthOrder,CreationTime,CreatorUserId,CurrentDateandTime,DeleterUserId,DeletionTime,  
GloveCategory,GloveCode,IsDeleted,LastModificationTime,LastModifierUserId,LineId,OutTime,Plant,Resource,SerialNo,ShiftId,Size,UserID,PackingSz,InBox)  
SELECT 1,@BatchNumber,@BatchOrder,GETDATE(),1,GETDATE(),0,NULL,@GloveCategory,@GloveCode,  
0,GETDATE(),NULL,@Line,@BatchCardDate,dbo.Ufn_DOT_GetLocationName(@LocationId),@Resource,@SerialNumber,@ShiftId,@Size,1,@Quantity,1  
  
--select result for print batch card  
SELECT @SerialNumber as SerialNumber,@BatchNumber as BatchNumber,@GloveCode as GloveCode, @GloveCategory as GloveCategory,@Size as Size,@BatchCardDate as BatchCardDate,  
@Resource as Resource,@BatchWeight as BatchWeight,@Quantity as Quantity  
  
 SET NOCOUNT OFF    
END TRY    
BEGIN CATCH    
 DECLARE @ErrorMessage NVARCHAR(4000);    
 DECLARE @ErrorSeverity INT;    
 DECLARE @ErrorState INT;    
 SELECT     
        @ErrorMessage = ERROR_MESSAGE(),    
        @ErrorSeverity = ERROR_SEVERITY(),    
        @ErrorState = ERROR_STATE();    
  RAISERROR (@ErrorMessage,     
        @ErrorSeverity,    
        @ErrorState     
        );    
    
    IF @@TRANCOUNT > 0    
        ROLLBACK TRANSACTION;    
END CATCH;    
    
IF @@TRANCOUNT > 0    
  COMMIT TRANSACTION;    
    
  END     
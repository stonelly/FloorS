  
-- =============================================  
-- Author:  Kamil  
-- Create date: 17 July 2019  
-- Description: Increase length @InnerSetLayout and @OuterSetLayout. Add nolock  
  
-- 2019-09-12 Azman Kasim  Add FPStationNo to SP    
-- =============================================  
CREATE  PROCEDURE [dbo].[usp_FP_FinalPacking_Save_MSBC]    
 -- Add the parameters for the stored procedure here    
 (    
  @LocationId int,    
  @WorkStationNumber nvarchar(25),    
  @PrinterName nvarchar(30),    
  @PackDate datetime,    
  @OuterLotNo nvarchar(15),    
  @InternalLotNumber nvarchar(15),    
  @PONumber nvarchar(20),    
  @ItemNumber nvarchar(40),    
  @Size nvarchar(10),    
  @GroupId int,    
  --@SerialNumber numeric(15,0),    
  @BoxesPacked int,    
  @PalletId nvarchar(8),    
  @CasesPacked int,    
  @PreShipmentPalletId nvarchar(8) = null,     
  @PreshipmentCasesPacked int,    
  @OperatorId int = null,    
  @InnerSetLayout nvarchar(30),    
  @OuterSetLayout nvarchar(30),    
  @palletCapacity int,    
  @TotalPcs int,    
  --@isTempPack Bit,    
  @strXML nvarchar(max) = null,    
  @ManufacturingDate datetime,    
  @ExpiryDate datetime,    
  @stationNumber int = null,    
  @InventTransId nvarchar(25) = null,    
  @FPStationNo nvarchar(5) = null,  
  @FGBatchOrderNo nvarchar(20),  
  @Resource nvarchar(20)      
 )    
AS    
BEGIN    
BEGIN TRANSACTION;    
BEGIN TRY    
    
DECLARE @rowsCount int    
DECLARE @PalletCases int    
DECLARE @prePalletCases int    
DECLARE @intRunningNumber int    
DECLARE @idoc int    
    
IF(@PreShipmentPalletId = '')     SET @PreShipmentPalletId = NULL;  
  
 INSERT INTO dbo.FinalPacking     
 (    
 LocationId,WorkStationId,PrinterName,PackDate,OuterLotNo,InternalLotNumber,PONumber,ItemNumber,    
 Size,QCGroupId,BoxesPacked,PalletId,CasesPacked,PreShipmentPalletId,PreshipmentCasesPacked,    
 OperatorId,LastModifiedOn,InnersetLayout,OutersetLayout,ManufacturingDate,ExpiryDate,InventTransId, FPStationNo,FGBatchOrderNo,Resource)    
 VALUES (@LocationId,@WorkStationNumber,@PrinterName,@PackDate,@OuterLotNo,@InternalLotNumber,@PONumber,    
 @ItemNumber,@Size,@GroupId,@BoxesPacked,@PalletId,@CasesPacked,@PreShipmentPalletId,@PreshipmentCasesPacked,    
 @OperatorId,SYSDATETIME(),@InnerSetLayout,@OuterSetLayout, @ManufacturingDate,@ExpiryDate,@InventTransId, @FPStationNo,@FGBatchOrderNo,@Resource)    
    
 IF @strXML is not null    
  BEGIN    
   EXEC sp_xml_preparedocument @idoc OUTPUT, @strXML    
    
   INSERT INTO FinalPackingBatchInfo (SerialNumber,    
   BoxesPacked,    
   CasesPacked,    
   PreshipmentCasesPacked,    
   InternalLotNumber)    
   SELECT  SerialNumber, BoxesPacked, CasesPacked, PreshipmentCasesPacked, @InternalLotNumber as InternalLotNumber    
   FROM OPENXML(@idoc, '/ArrayOfFinalPackingBatchInfoDTO/FinalPackingBatchInfoDTO')    
   WITH (SerialNumber numeric(10,0)    
     ,BoxesPacked int    
     ,CasesPacked int    
     ,PreshipmentCasesPacked int    
     ,PalletId nvarchar(8)    
     ,PreshipmentPalletId nvarchar(8)    
     )    
     EXEC sp_xml_removedocument @idoc    
  END     
         
  IF(@stationNumber is not null)    
  BEGIN    
   SELECT @intRunningNumber = isnull(LastRunningLotNumber,0)+1 from WorkstationRunningNumber where WorkStationId = @stationNumber    
   EXEC usp_FP_WorkStationNumber_Update @stationNumber, @intRunningNumber    
  END    
  Else    
  BEGIN    
   SELECT @intRunningNumber = isnull(LastRunningLotNumber,0)+1 from WorkstationRunningNumber where WorkStationId = @WorkStationNumber         
   EXEC usp_FP_WorkStationNumber_Update @WorkStationNumber, @intRunningNumber    
  END    
    
  --update batch capacity    
  EXEC usp_FP_BatchPackedPcs_Update_MSBC @strXML    
    
  -- Update PurchaseOrderItemCases table with pallet id and internalalLot Number     
  UPDATE dbo.PurchaseOrderItemCases     
  SET  InternalotNumber = @internallotnumber    
  WHERE casenumber in ( select top (@CasesPacked) CaseNumber from PurchaseOrderItemCases      
  where PONumber = @PONumber and ItemNumber = @ItemNumber and Size = @Size and internalotnumber is null order by CaseNumber )    
  and PONumber = @PONumber and ItemNumber = @ItemNumber and Size = @Size    
    
END TRY    
BEGIN CATCH    
    SELECT     
        ERROR_NUMBER() AS ErrorNumber    
        ,ERROR_SEVERITY() AS ErrorSeverity    
        ,ERROR_STATE() AS ErrorState    
        ,ERROR_PROCEDURE() AS ErrorProcedure    
        ,ERROR_LINE() AS ErrorLine    
        ,ERROR_MESSAGE() AS ErrorMessage;    
    IF @@TRANCOUNT > 0    
        ROLLBACK TRANSACTION;    
  throw;    
END CATCH;    
    
IF @@TRANCOUNT > 0    
  COMMIT TRANSACTION;    
    
END    
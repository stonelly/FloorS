  
  
-- =============================================  
-- Author:  <srikanth Balda>  
-- Create date: <1 Oct 2014>  
-- Description: <Save Second Grade>  
-- =============================================  
  
-- Author:  <Kamil>  
-- Create date: <17 July 2019>  
-- Description: <Increase length @InnerSetLayout and @OuterSetLayout. Add nolock>  
  
-- 2019-09-12 Azman Kasim  Add FPStationNo to SP    
  
-- =============================================  
  
CREATE PROCEDURE [dbo].[usp_FP_FinalPacking_SaveSecondGrade]    
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
  @BoxesPacked int,      
  @CasesPacked int,      
  @OperatorId int = null,    
  @InnerSetLayout nvarchar(30),    
  @OuterSetLayout nvarchar(30),    
  @stationNumber int = null,    
  @ManufacturingDate datetime,    
  @ExpiryDate datetime,    
  @SerialNumber nvarchar(max),    
  @InventTransId nvarchar(25),    
  @FPStationNo nvarchar(5) = null,  
  @FGBatchOrderNo nvarchar(20),  
  @Resource nvarchar(20)      
 )    
AS    
BEGIN    
BEGIN TRANSACTION;    
 BEGIN TRY    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
     
 INSERT INTO dbo.FinalPacking     
 (    
  LocationId,WorkStationId,PrinterName,PackDate,OuterLotNo,InternalLotNumber,PONumber,ItemNumber,    
  Size,QCGroupId,BoxesPacked,CasesPacked,    
  OperatorId,LastModifiedOn,InnersetLayout,OutersetLayout, manufacturingdate, expirydate,InventTransId, FPStationNo,FGBatchOrderNo,Resource     
 )    
 VALUES     
 (    
  @LocationId,@WorkStationNumber,@PrinterName,@PackDate,@OuterLotNo,@InternalLotNumber,@PONumber,    
  @ItemNumber,@Size,@GroupId,@BoxesPacked,@CasesPacked,    
  @OperatorId,SYSDATETIME(),@InnerSetLayout,@OuterSetLayout,@ManufacturingDate,@ExpiryDate,@InventTransId, @FPStationNo,@FGBatchOrderNo,@Resource    
 )    
    
 DECLARE @intRunningNumber int    
 IF(@stationNumber is not null)    
 BEGIN    
  SELECT @intRunningNumber = isnull(LastRunningLotNumber,0)+1 from WorkstationRunningNumber where WorkStationId = @stationNumber    
  EXEC usp_FP_WorkStationNumber_Update @stationNumber, @intRunningNumber    
 End    
 Else    
 BEGIN    
  SELECT @intRunningNumber = LastRunningLotNumber from WorkstationRunningNumber where WorkStationId = @WorkStationNumber    
  SET @intRunningNumber = @intRunningNumber+1    
  EXEC usp_FP_WorkStationNumber_Update @WorkStationNumber, @intRunningNumber    
 END    
  -- Update PurchaseOrderItemCases table with pallet id and internalalLot Number     
  UPDATE dbo.PurchaseOrderItemCases     
  SET  InternalotNumber = @internallotnumber    
  WHERE casenumber in ( select top (@CasesPacked) CaseNumber from PurchaseOrderItemCases      
  WHERE PONumber = @PONumber and ItemNumber = @ItemNumber and Size = @Size and internalotnumber is null order by CaseNumber )    
  and PONumber = @PONumber and ItemNumber = @ItemNumber and Size = @Size    
    
 ----- Update isPackingDone in SecondGradesticker    
 UPDATE SecondGradeSticker SET ispacked = 1, internallotnumber = @InternalLotNumber WHERE serialnumber in (SELECT * FROM ufn_CSVToTable(@SerialNumber))    
    
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
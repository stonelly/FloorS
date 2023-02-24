     
-- =======================================================  
-- Name:   USP_SAV_BatchCard  
-- Purpose:   Save offline batch details  
-- =======================================================  
-- Change History  
-- Date         Author     Comments  
-- -----        ------     -----------------------------  
-- 10/06/2014  Sujana    SP ALTERd.  
-- 06/10/2014  Nagaraju    Added QCtype for Submodule=3  
-- 6/10/2017  MyAdamas Added Submodule Reprint Reproduction =118 reproduction visual test and 119 reproduction water tight    
-- 23/1/2018  MyAdamas Alter SP parameter submoduleid to use screen name instead of screen ID  
-- 01/03/2018 Azman  Add ReProduction Module 1  
-- 15/07/2021 Azman  NGC 1.5 4 char LineId fix   
-- 12/01/2021      Vinoden  Change [AX_AVAQCTYPETABLE] to [DOT_FSQCTYPETABLE]  
-- 30/11/2021 Azrul Merged from NGC to HSB.
-- =======================================================  
CREATE PROCEDURE [dbo].[USP_SAV_BatchCard]  
  -- Add the parameters for the stored procedure here  
 @shift     INT,  
 @line     NVARCHAR(5),  
 @size     NVARCHAR(3),  
 @gloveType    NVARCHAR(50),  
 @batchWeight   DECIMAL(18,3),  
 @tenPcsWeight   DECIMAL(18,3),  
 @batchCardDate   DATETIME,  
 @isOnline    BIT,  
 @operatorId    NVARCHAR(10),  
 @workStationNumber  NVARCHAR(25),  
 @batchType    NCHAR(10),  
 @location    INT,  
 @module         NVARCHAR(50),  
 @subModule        NVARCHAR(50),  
 @authorizedBy      NVARCHAR(25),  
 @lostArea       NVARCHAR(15),  
 @site     NVARCHAR(20),  
 @shiftname    NVARCHAR(1),  
 @authorizedFor   INT  
AS  
BEGIN  
 BEGIN TRY  
  DECLARE @serialSequenceNumber AS INT  
  DECLARE @tempSerialNumber AS NUMERIC(25)  
  DECLARE @batchNumber AS NVARCHAR(20)  
  DECLARE @totalPcs AS INT  
  DECLARE @shiftStartDate NVARCHAR(8)  
  DECLARE @QCType NVARCHAR(50)  
  DECLARE @LineId VARCHAR(4)  
  DECLARE @ShiftCurrentDate NVARCHAR(8)  
  SET @ShiftCurrentDate = CONVERT(VARCHAR(8), GETDATE(), 112)  
  IF ((@subModule = 3) OR (@subModule = 119))  -- #Azman 22/02/2018 Add ReproductionWT Module  
   SET @QCType = (SELECT TOP(1) QCType FROM DOT_FSQCTypeTable WHERE DESCRIPTION = 'PT' AND STOPPED = 0)  
  ELSE  
   SET @QCType = NULL  
  -- If Shift is not supplied, select shift from Shift master and assign ShiftStart date  
  IF @shift = 0  
  BEGIN  
   SET @shiftname = dbo.Ufn_GetCurrentShift('PN')  
   SET @shift = (SELECT ShiftId from Shiftmaster where Name = @shiftname AND GroupType = 'PN' AND IsDeleted = 0)  
   SET @shiftStartDate = dbo.Ufn_GetShiftStartDate(@shiftname)  
  END  
   --- If Shift is supplied assign ShiftStart date  
  ELSE  
  SET @shiftStartDate = dbo.Ufn_GetShiftStartDate(@shiftname)  
  SET @serialSequenceNumber = Next VALUE FOR SerialNumberSeq  
    
  -- Set batch Type from Enum Master  
  SET @batchType = (SELECT EnumValue from EnumMaster where EnumText = @batchType and EnumType = 'BatchType')  
  
  -- Generate Serial Number  
  SET @tempSerialNumber = @site + RIGHT(CONVERT(VARCHAR(8), GETDATE(), 1),2) + FORMAT(@serialSequenceNumber,'0000000')  
  -- Calculate Total Pieces  
  SET @totalPcs = (@batchWeight /@tenPcsWeight )*10000  
  -- Generate Batch Number  
  --TOTT ID 40, 148 CR changes as below.  
  --Ror batch type -> Tumbling - PrintLostBatchCard,Tumbling - PrintWaterTightBatchCard,Tumbling - PrintVisualTestBatchCard @shiftStartDate should be current date  
  
  --Azman 150721 - Fix on 4 char LineId  
  SET @LineId = CASE LEN(@line)   
    WHEN 2 THEN '0'+RIGHT(CAST(@Line as VARCHAR(5)),1)   
    WHEN 3 THEN  RIGHT(CAST(@Line as VARCHAR(5)),2)   
    WHEN 4 THEN  RIGHT(CAST(@Line as VARCHAR(5)),3)   
   END  
  SET @batchNumber= CASE @subModule WHEN 1 THEN   
        RTRIM(@shiftname) + @LineId + '/'+ @shiftStartDate+ '/' + @size  
        WHEN 2  THEN   
        RTRIM(@batchType) + RIGHT('0'+CAST(@location AS VARCHAR(2)),2) + '/' + @ShiftCurrentDate + '/' + @size  
        WHEN 3  THEN  
        RTRIM(CASE @batchType WHEN 'PWT' Then 'P'WHEN 'QWT' Then 'Q'WHEN 'OWT' Then 'O'WHEN 'PSW' Then 'I' END)+ RIGHT('0'+CAST(@location AS VARCHAR(2)),2) +'/' + @ShiftCurrentDate+ '/' + @size  
        WHEN 4  THEN  
        RTRIM(@batchType) + RIGHT('0'+CAST(@location AS VARCHAR(2)),2) +'/' + @shiftStartDate + '/' + @size  
        WHEN 5  THEN  
        RTRIM(@batchType) + RIGHT('0'+CAST(@location AS VARCHAR(2)),2) +'/' + @ShiftCurrentDate + '/' + @size  
        WHEN 119 THEN -- #Azman 01/03/2018 Add ReproductionWT Module  
        RTRIM(@shiftname) + @LineId + '/'+ @shiftStartDate+ '/' + @size  
        WHEN 123 THEN -- #Azman 01/03/2018 Add ReproductionVT Module  
        RTRIM(@shiftname) + @LineId + '/'+ @shiftStartDate+ '/' + @size  
        END  
  BEGIN TRANSACTION  
   -- Insert in to Batch table  
   INSERT INTO Batch (SerialNumber,BatchNumber,ShiftId,LineId,GloveType,Size,TierSide,BatchWeight,TenPCsWeight,  
   BatchLostArea,BatchCardDate,QCType,QAIDate,BypassReasonId,ReferenceNumber ,ReWorkCount ,IsReprint ,IsOnline ,TotalPCs ,  
   ModuleId ,SubModuleId ,LocationId ,BatchType ,AuthorizedBy ,LastModifiedOn ,WorkStationId,  
   IsFPBatchSplit ,BatchCardCurrentLocation,AuthorizedFor )VALUES(@tempSerialNumber,@batchNumber,@shift,@line,@gloveType,  
   @size,NULL,@batchWeight,@tenPcsWeight,@lostArea,@batchCardDate,@QCType,Null,NULL ,  
   NULL,0,NULL,0,@totalPcs,@module,@subModule,@location ,@batchType ,  
   @authorizedBy,@batchCardDate,@workStationNumber,NULL,'PN',@authorizedFor)  
   -- Insert in to Log Table  
   INSERT INTO BatchLog VALUES(@tempserialNumber,@operatorId,@location,GETDATE())  
   IF ((@subModule = 3) OR (@subModule = 119))  -- #Azman 22/02/2018 Add ReproductionWT Module  
   BEGIN  
   INSERT INTO QAI (QAIDate,QAIInspectorId,SerialNumber,BatchNumber,QCType,TenPcsWeight,LastModifiedDateTime,WorkStationId,SubModuleId,IsAXPostingSuccess)  
   VALUES (@batchCardDate,@operatorId,@tempSerialNumber,@batchNumber,@QCType,@tenPcsWeight,GETDATE(),@workStationNumber,@SubModule,1)  
   UPDATE Batch SET QAIDate = GETDATE() WHERE SerialNumber = @tempSerialNumber  
   END  
 -- Return Serial Number and Batch Number  
 SELECT @batchNumber AS 'BatchNumber' ,@tempSerialNumber AS 'SerialNumber'  
 END TRY  
 BEGIN CATCH  
  IF @@TRANCOUNT > 0  
   ROLLBACK TRANSACTION;  
   THROW;  
 END CATCH;  
 IF @@TRANCOUNT > 0  
 COMMIT TRANSACTION;  
END   
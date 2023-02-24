CREATE PROCEDURE [dbo].[USP_SAV_RejectGlove]  
 -- Add the parameters for the stored procedure here  
 @shift INT = NULL,  
 @line NVARCHAR(5) = NULL,  
 @gloveType NVARCHAR(50) = NULL,  
 @batchWeight DECIMAL(18,3),  
 @reasonId int,  
 @batchCardDate DATETIME,  
 @operatorId int,  
 @workStationNumber NVARCHAR(25),  
 @batchType NCHAR(10),  
 @location INT,  
 @moduleName NVARCHAR(50),  
 @subModuleName NVARCHAR(50),  
 @site NVARCHAR(20),  
 @shiftname NVARCHAR(1) = NULL   
  
AS  
  
BEGIN  
 BEGIN TRY  
     DECLARE @rejectSequenceNumber AS NUMERIC(15,0)  
  DECLARE @batchNumber AS NVARCHAR(20)  
  DECLARE @totalPcs AS INT  
  DECLARE @CurrentTime time(7)  
  DECLARE @shiftStartDate NVARCHAR(8)  
  DECLARE @LineId VARCHAR(4)  
  
  IF @shift is Null  
   SET @shiftStartDate = CONVERT(VARCHAR(8), GETDATE(), 112)  
  ELSE  
   SET @shiftStartDate = dbo.Ufn_GetShiftStartDate(@shiftname)   
    
  
  SET @rejectSequenceNumber = Next VALUE FOR RejectGloveSeq  
  -- Set batch Type from Enum Master  
  SET @batchType = (SELECT EnumValue from EnumMaster where EnumText = @batchType and EnumType = 'BatchType')  
     -- Calculate Total Pieces  
  SET @totalPcs = 0   
  
  IF @line is Null  
   SET @LineId = 99  
  ELSE  
   SET @LineId = CASE LEN(@line) WHEN 2 THEN '0'+RIGHT(CAST(@Line as VARCHAR(5)),1) WHEN 3 THEN  RIGHT(CAST(@Line as VARCHAR(5)),2) END  
     
  -- Generate Batch Number  
  SELECT @batchNumber = RTRIM(@batchType) + RIGHT('0'+CAST(@location AS VARCHAR(2)),2) +'/' + @shiftStartDate + '/' + @LineId  
   
  BEGIN TRANSACTION  
   -- Insert in to Batch table  
   INSERT INTO RejectedGlove VALUES(FORMAT (@rejectSequenceNumber, '000000') ,@batchNumber,@line,@shift,@gloveType,  
         @batchWeight,@reasonId, @batchCardDate,@workStationNumber,NULL)  
   -- Return Serial Number and Batch Number  
   SELECT @batchNumber AS 'BatchNumber' ,FORMAT( @rejectSequenceNumber,'0000000000')  AS 'SerialNumber'  
     END TRY  
  
  BEGIN CATCH  
   IF @@TRANCOUNT > 0  
    ROLLBACK TRANSACTION;  
   THROW;  
  END CATCH;  
 IF @@TRANCOUNT > 0  
 COMMIT TRANSACTION;  
 END   
  
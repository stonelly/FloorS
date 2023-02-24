

-- =======================================================  
-- Name:             usp_QAIChangeQCType_Save  
-- Purpose:          Save Change QC Type Details  
-- =======================================================  
-- Change History  
-- Date         Author     Comments  
-- -----        ------     -----------------------------  
-- 24/08/2014        NarendraNath    SP created  
-- 23/07/2018   Azrul    SP altered  
-- 20/11/2020 Chong KH Qai Scanning enhancement  
-- 05/01/2022	Pang		ITRF:20211229160942289340 To fix the bugs for QAIDate of Batch Table 
-- =======================================================  
ALTER PROCEDURE [dbo].[usp_QAIChangeQCType_Save]  
(  
 @SerialNumber Nvarchar(10),  
 @QAIInspectorId NVARCHAR(10),  
 @ChangedQCType  NVARCHAR(50),  
 @ChangeQCTypeReason  NVARCHAR(50),  
 @workstationId NVARCHAR(20),  
 @IsChangeQCType BIT,  
 @AuthorizedBy INT  
)  
AS  
BEGIN  
BEGIN TRANSACTION;  
BEGIN TRY       
   
 DECLARE @QAIChangeReason NVARCHAR(20)  
 DECLARE @NewQAIID INT  
 DECLARE @QAIID INT    
  
 INSERT INTO QAI(SerialNumber,BatchNumber,QAIInspectorId,QCType,WTSampliingSize,InnerBox,TenPcsWeight,PackingSize, QAIDate,VTSamplingSize,  
   QAIChangeReason,QITestResult,LastModifiedDateTime,WorkStationId,SubModuleId,QAIScreenName,IsAXPostingSuccess,QCTypeAuthorizedBy)  
  SELECT SerialNumber,BatchNumber,@QAIInspectorId,@ChangedQCType,WTSampliingSize,InnerBox,TenPcsWeight,PackingSize, QAIDate,VTSamplingSize,  
   @ChangeQCTypeReason,QITestResult,GETDATE(),@workstationId,SubModuleId,'QAIChangeQCType',1,@AuthorizedBy FROM QAI WHERE SerialNumber=@SerialNumber AND LastModifiedDateTime=(SELECT MAX(LastModifiedDateTime) FROM QAI WHERE  
   SerialNumber=@SerialNumber)  
         
  SELECT @QAIID= SCOPE_IDENTITY();     
  
  SELECT @NewQAIID= QAIid FROM qai WHERE SerialNumber=@SerialNumber AND QAIid<> @QAIID   
  AND LastModifiedDateTime =(SELECT MAX(LastModifiedDateTime) FROM qai WHERE  SerialNumber=@SerialNumber AND QAIid<> @QAIID)  
  
  INSERT INTO QAIDefectMapping  
  SELECT @QAIID,DefectID,NoOfDefects FROM  QAIDefectMapping   WHERE QAIId=@NewQAIID  
  
   -- 20/11/2020 Qai Scanning enhancement Start  
  INSERT INTO QAIDefectPositionMapping (QAIID,DefectID, DefectPositionId,NoOfDefects)  
  SELECT @QAIID,DefectID, DefectPositionId,NoOfDefects FROM  QAIDefectPositionMapping   WHERE QAIId=@NewQAIID  
   -- 20/11/2020 Qai Scanning enhancement End  
   
  UPDATE b SET QCType= q.QCType, LastModifiedOn=GETDATE() FROM Batch b JOIN qai q ON b.SerialNumber=q.SerialNumber  
  WHERE q.QAIId=@QAIID  
  
  UPDATE Batch SET IsChangeQCType = @IsChangeQCType WHERE SerialNumber = @SerialNumber  
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
  GO
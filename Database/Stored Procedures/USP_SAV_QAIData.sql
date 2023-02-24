  
  
-- =======================================================  
-- Name:             USP_SAV_QAIData  
-- Purpose:          Save Data from QAIDTO and DefectDataDTO  
-- =======================================================  
-- Change History  
-- Date         Author     Comments  
-- -----        ------     -----------------------------  
-- 24/08/2014        NarendraNath    SP created.  
-- 16/01/2018  MyAdamas  - QAIScan & QAIScanInnerTenPcs: Update Batch's QAIDate & QCType  
--         - QAIChangeQCType & QAIResamplingScan: Update Batch's QCType only   
--         - ScanQITestResult where result is PASS: Update Batch's QCType only   
-- 30/1/2018  MYAdamas  Update qctype for ScanQI no matter failed/pass  
-- 10/4/2018  Pang YIk SIU Fix TenPcsWeight Decimal issue(5,3) to (18,3)  
-- 05/06/2020  Soon Siang  ADd New Column QITestReason  
-- 12/01/2021       Vinoden      Change [AX_AVAQCTYPETABLE] to [DOT_FSQCTYPETABLE]  
-- 13/12/2021 Azrul    Merged from NGC. 
-- 05/01/2022	Pang			ITRF:20211229160942289340 To fix the bugs for QAIDate of Batch Table 
-- 18/04/2022	Azrul			Scan QAI for 1 Year Batch 
-- =========================================================    
CREATE OR ALTER PROCEDURE [dbo].[USP_SAV_QAIData]    
(      
 @QAIDETAILS NVARCHAR(MAX),      
 @WorkStationId NVARCHAR(50),    
 @IsChangeQCType BIT    
)      
AS  
BEGIN  
BEGIN TRANSACTION;  
BEGIN TRY       
  
 DECLARE @QAIID INT  
 DECLARE @idoc INT
 DECLARE @PackingSize INT    
 DECLARE @InnerBox INT      
 DECLARE @QCTypeSelected NvarChar(200)  
 SELECT  @QCTypeSelected=QCTYPE FROM DOT_FSQCTypeTable WITH (NOLOCK) WHERE DESCRIPTION ='RESAMPLE' And STOPPED=0      
    
 DECLARE @SerialNo numeric(15, 0)    
 DECLARE @QAIExpiryDuration int    
 DECLARE @QAIScreenName nvarchar(200)    
    
 DECLARE @QAICheck int = 1    
 DECLARE @QAICheckResult nvarchar(10)    
 EXEC sp_xml_preparedocument @idoc OUTPUT, @QAIDETAILS      
    
 SET @QAIExpiryDuration = Convert(INT,Replace((Select Item from dbo.SplitString((SELECT Item FROM dbo.SplitString((select FloorConfData from SystemConfData),',') where Item like '%intQaiExpiryDuration%'),':') where Item  not like '%intQaiExpiryDuration%')
  
    
,'"',''))    
 SELECT @SerialNo = adata.SerialNumber, @QAIScreenName = adata.ScreenName FROM OPENXML(@idoc, '/QAIDetails',2) WITH (SerialNumber NUMERIC(15), ScreenName NVARCHAR(200)) adata    
    
  IF (@QAIScreenName = 'QAIScanInnerTenPcs') OR (@QAIScreenName = 'QAIScan')    
 BEGIN    
 IF EXISTS(    
 SELECT COALESCE(    
    CASE WHEN b.QAIDate IS NULL THEN 'Incomplete'  ELSE 'NoQAI' END,    
    CASE WHEN b.QAIDate < DATEADD(d, -@qaiExpiryDuration, GETDATE()) THEN 'Expired' ELSE 'Passed' END    
    ) AS Status FROM Batch b (nolock) JOIN QAI q (nolock) ON b.SerialNumber = q.SerialNumber WHERE  b.SerialNumber=@serialNo  AND Q.IsAXPostingSuccess=1 AND    
  q.QAIId=(SELECT TOP 1 QAIID FROM QAI (nolock) where SerialNumber=q.SerialNumber order by QAIId ASC  ))    
  BEGIN    
  SELECT @QAICheckResult = xy.Status FROM (    
    SELECT COALESCE(    
   CASE WHEN b.QAIDate IS NULL  THEN 'Incomplete'  ELSE 'NoQAI' END,    
   CASE WHEN b.QAIDate < DATEADD(d, -@qaiExpiryDuration, GETDATE()) THEN 'Expired' ELSE 'Passed' END    
   ) AS Status FROM Batch b (nolock) JOIN QAI q (nolock) ON b.SerialNumber = q.SerialNumber WHERE  b.SerialNumber=@serialNo  AND Q.IsAXPostingSuccess=1 AND    
   q.QAIId=(SELECT TOP 1 QAIID FROM QAI (nolock) where SerialNumber=q.SerialNumber order by QAIId ASC)    
  ) xy    
  END    
  ELSE    
  BEGIN    
   SET @QAICheckResult = 'Incomplete'    
  END    
    
  IF (@QAICheckResult = 'NoQAI') OR (@QAICheckResult = 'Incomplete')    
  BEGIN    
  SET @QAICheck = 1    
  END    
  ELSE      
  BEGIN    
  SET @QAICheck = 2    
  INSERT INTO [dbo].[Exception] ([Message],[StackTrace],[WorkStationID],[ExceptionDateTime],[InnerExceptionmessage],[SubSystem],[SystemBaseException],[ScreenName],[UIClassName],[UIControlName],[MethodParameter])    
  VALUES ('QAI Duplication Prevention','','', GETDATE(),'','','', @QAIScreenName,'','', @SerialNo)    
  END    
 END    
 ELSE    
 BEGIN    
 SET @QAICheck = 1    
 END    
    
  IF (@QAICheck = 1)    
  BEGIN    
 INSERT INTO QAI(SerialNumber,BatchNumber,QAIInspectorId,QCType,WTSampliingSize,TenPcsWeight, QAIDate,VTSamplingSize,HBSamplingSize,QAIChangeReason,    
 QITestResult,LastModifiedDateTime,IsResampling,WorkStationId,SubModuleId,QCTypeAuthorizedBy,SuggestedQCType,AQLValue,QAIScreenName, TenPcsSamplingSize,IsAXPostingSuccess,QITestReason)    
 SELECT a.SerialNumber,a.BatchNumber,a.QAIInspectorId,a.QCType,a.WTSampliingSize,a.TenPcsWeight,GETDATE() QAIDateTime,a.VTSamplingSize,a.HBSamplingSize,    
 a.QAIChangeReason,a.QAITestResult,GETDATE(),a.IsResampling,@WorkStationId,b.SubModuleId,QCTypeAuthorizedBy,SuggestedQCType,AQLValue,ScreenName, TenPcsSamplingSize,1,QITestReason FROM  OPENXML(@idoc, '/QAIDetails',2)    
 WITH (SerialNumber NUMERIC(15),BatchNumber NVARCHAR(40),QAIInspectorId NVARCHAR(10),    
 QCType NVARCHAR(20),WTSampliingSize INT,InnerBox INT,TenPcsWeight DECIMAL(18, 3),    
 PackingSize INT ,QAIDateTime DATETIME,VTSamplingSize INT,HBSamplingSize INT,QAIChangeReason NVARCHAR(20),    
 DefectTypeId INT,QAITestResult NVARCHAR(30),IsResampling bit,QCTypeAuthorizedBy NVARCHAR(10),SuggestedQCType NVARCHAR(20),AQLValue NVARCHAR(100),ScreenName NVARCHAR(200), TenPcsSamplingSize INT, QITestReason NVARCHAR(50)) a JOIN batch b ON a.SerialNumber
  
=b.SerialNumber    
    
 SELECT @QAIID= SCOPE_IDENTITY();      
      
    INSERT INTO QAIDefectMapping      
    SELECT @QAIID, DefectID,[Count] FROM   OPENXML(@idoc, '/QAIDetails/Defects/QAIDefectType/DefectList',2)      
    WITH (DefectID INT,[Count] INT) X      
    WHERE X.[count] >0      
     
 -- 21/10/2020 start    
 INSERT INTO QAIDefectPositionMapping    
 SELECT @QAIID, DefectID, DefectPositionID,[Count] FROM   OPENXML(@idoc, '/QAIDetails/Defects/QAIDefectType/DefectList/DefectPositionList',2)    
 WITH (DefectID int '../DefectID', DefectPositionID INT,[Count] INT) X    
 WHERE X.[count] >0    
 -- 21/10/2020 end    
    
    
    DECLARE @SerialNumber NVARCHAR(100)    
 SELECT @SerialNumber = SerialNumber FROM OPENXML(@idoc, '/QAIDetails',2) WITH (SerialNumber NUMERIC(15))    
      
    INSERT INTO BatchAXPostingLog(SerialNumber,BatchNumber,ShiftId,LineId,GloveType,Size,TierSide,BatchWeight,TenPCsWeight,BatchLostArea,BatchCardDate,QCType,      
    QAIDate,BypassReasonId,ReferenceNumber,ReWorkCount,IsReprint,IsOnline,TotalPCs,ModuleId,SubModuleId,LocationId,BatchType,      
    AuthorizedBy,LastModifiedOn,WorkStationId,IsFPBatchSplit,BatchCardCurrentLocation,PackedPcs,QCBatchWeight,QCTenPcsWeight,      
    PTBatchWeight,PTTenPcsWeight,CustomerRejectRefId,AXPostingDate,QAIID)      
    SELECT SerialNumber,BatchNumber,ShiftId,LineId,GloveType,Size,TierSide,BatchWeight,TenPCsWeight,BatchLostArea,BatchCardDate,QCType,      
    QAIDate,BypassReasonId,ReferenceNumber,ReWorkCount,IsReprint,IsOnline,TotalPCs,ModuleId,SubModuleId,LocationId,BatchType,      
    AuthorizedBy,LastModifiedOn,WorkStationId,IsFPBatchSplit,BatchCardCurrentLocation,PackedPcs,QCBatchWeight,QCTenPcsWeight,      
    PTBatchWeight,PTTenPcsWeight,CustomerRejectRefId,GETDATE(),@QAIID FROM BATCH  WHERE SerialNumber=@SerialNumber    
    
    DECLARE @ScreenName NVARCHAR(100)    
    SELECT @ScreenName=ScreenName FROM OPENXML(@idoc, '/QAIDetails',2) WITH (ScreenName NVARCHAR(100))      
    
 IF (@IsChangeQCType IS NOT NULL AND LEN(@IsChangeQCType) > 0)    
 BEGIN    
  UPDATE batch SET IsChangeQCType = @IsChangeQCType WHERE SerialNumber = @SerialNumber    
 END    
    
    IF @ScreenName='QAIScanInnerTenPcs'      
    BEGIN      
     DECLARE @TenPCsWeight DECIMAL     
     DECLARE @BatchWeight DECIMAL (10, 3)     
     DECLARE @TotalPCs INT      
     
	 --HSB MAS: Scan QAI for 1 Year Batch - As per old practice, the original batch weight and 10pcs weight will remain the same and only QAIDate will updated to the latest date. 
	 IF Not Exists (Select 1 from QAI With (nolock) where SerialNumber = @SerialNumber and QAIScreenName = 'QAIScanInnerTenPcs')
	 BEGIN
		 UPDATE b SET b.TotalPCs= isnull(a.TotalPCs,0),BatchWeight=(SELECT ((SUM(PackingSz*InBox))*a.TenPcsWeight)/10000 FROM DOT_FloorD365HRGLOVERPT WITH (INDEX(DOT_SerialNo_Index) NOLOCK) WHERE SerialNo = @SerialNumber),    
		 b.TenPcsWeight =a.TenPcsWeight FROM OPENXML(@idoc, '/QAIDetails',2) WITH (SerialNumber NUMERIC(15),    
		 TotalPCs INT,BatchWeight DECIMAL(18,3),TenPcsWeight DECIMAL(18, 3)) a JOIN batch b with (nolock) ON a.SerialNumber=b.SerialNumber AND b.SerialNumber=@SerialNumber     
	 END
	 ELSE
	 BEGIN
		 UPDATE b SET b.TotalPCs= isnull(a.TotalPCs,0) FROM OPENXML(@idoc, '/QAIDetails',2) WITH (SerialNumber NUMERIC(15),TotalPCs INT) a 
		 JOIN batch b with (nolock) ON a.SerialNumber=b.SerialNumber AND b.SerialNumber=@SerialNumber  
	 END
    END  
  
 --Surgical & Print Normal Batch logics START  
    IF @ScreenName='QAIScan'    
    BEGIN    
		DECLARE @IsSurgicalGlove INT = 0,
				@IsPNBC INT = 0

		IF EXISTS (select 1 from Batch b with (nolock) join DOT_FSItemMaster i with (nolock) on b.GloveType = i.ItemId where b.SerialNumber = @SerialNumber and i.ItemType = 109)  
		BEGIN  
			SET @IsSurgicalGlove = 1  
		END  

		IF EXISTS (select 1 from Batch with (nolock) where SerialNumber = @SerialNumber and SubModuleId = 1)  
		BEGIN  
			SET @IsPNBC = 1  
		END  

		IF (@IsSurgicalGlove = 1 OR @IsPNBC = 1)
		BEGIN  
		    --#Azrul 20200112: Temp disabled, Print SRBC Qty default to 0.  
		    --UPDATE b SET b.TenPcsWeight = a.TenPcsWeight FROM OPENXML(@idoc, '/QAIDetails',2) WITH (SerialNumber NUMERIC(15),TenPcsWeight DECIMAL(10, 3)) a   
		    --     JOIN batch b WITH (NOLOCK) ON a.SerialNumber=b.SerialNumber AND b.IsOnline=1  
            SELECT @BatchWeight = BatchWeight FROM batch WITH (NOLOCK) WHERE SerialNumber = @SerialNumber --AND b.IsOnline=1  --HSB UAT Issue: Include Offline Print Normal Batch Card      
            SELECT @TotalPCs = (@BatchWeight/TenPcsWeight)*10000 FROM OPENXML(@idoc, '/QAIDetails',2) WITH (SerialNumber NUMERIC(15),TenPcsWeight DECIMAL(18, 3))      
            UPDATE b SET b.TenPcsWeight = a.TenPcsWeight, b.TotalPcs = @TotalPCs FROM OPENXML(@idoc, '/QAIDetails',2) WITH (SerialNumber NUMERIC(15),TenPcsWeight DECIMAL(18, 3)) a       
            JOIN batch b WITH (NOLOCK) ON a.SerialNumber=b.SerialNumber --AND b.IsOnline=1  --HSB UAT Issue: Include Offline Print Normal Batch Card  
		    UPDATE DOT_FloorD365HRGLOVERPT SET PackingSz = @TotalPCs WHERE SerialNo = @SerialNumber  
		END  
    END     
 --Surgical & Print Normal Batch logics END     
    
    IF (@ScreenName!= 'ScanQITestResult')      
     BEGIN      
      -- ITRF:20211229160942289340 To fix the bugs for QAIDate of Batch Table 
	  IF (@ScreenName = 'QAIScan' OR @ScreenName = 'QAIScanInnerTenPcs')
	  BEGIN		
	  	UPDATE b SET b.QAIDate = GETDATE(),b.LastModifiedOn=GETDATE()
	  	FROM Batch b JOIN QAI q ON q.SerialNumber=b.SerialNumber
	  	WHERE q.QAIId=@QAIID 
	  END

	  UPDATE b SET b.QCType = q.QCType, b.LastModifiedOn=GETDATE()
	  FROM Batch b JOIN QAI q ON q.SerialNumber=b.SerialNumber
	  WHERE q.QAIId=@QAIID     
     END      
    ELSE IF (@ScreenName = 'ScanQITestResult')      
     BEGIN      
   -- #AZRUL 20180814: BUG_1127 - Update QC type either QI result is Pass or failed.    
   -- #Max 20181220, only fail update qc type    
      IF EXISTS(SELECT 1 FROM QAI WHERE QAIId=@QAIID AND QITestResult='Fail')      
      BEGIN      
       UPDATE b SET b.QCType = q.QCType,b.LastModifiedOn=GETDATE()    
       FROM Batch b JOIN QAI q ON q.SerialNumber=b.SerialNumber      
       WHERE q.QAIId=@QAIID       
      END      
      ELSE      
      BEGIN      
   Declare @Route NVARCHAR(100)    
   DECLARE @QAIQCType NvarChar(200)    
   SELECT TOP 1 @Route = qctype.RouteCategory,@QAIQCType=q.QCType FROM QAI q join DOT_FSQCTypeTable qctype with(nolock) on q.QCType=QCType.QCType WHERE QAIId=@QAIID order by qctype.Id desc    
   -- 1. for Reproduction PT batch card, PTQI pass QC type change will update batch qc type     
   -- 2. for HBC NON SP batch card,from PT change to OCQ will update batch QC Type    
   -- 3. include VT type batch card, PTQI pass QC type change will update batch qc type    
   -- 4. exclude 'SP Pass', will follow batch QC Type     
   IF @QAIQCType<>'0006020001' and EXISTS(SELECT 1 FROM Batch b with(nolock)    
    left join DOT_FSQCTypeTable qc with(nolock) on b.QCType=qc.QCType    
    where b.SerialNumber=@SerialNumber     
     and (exists (select 1 from EnumMaster with(nolock) where EnumType in ('WTType','VTType') and b.BatchType = EnumValue)     
      or (b.BatchType in ('T','X')  and qc.RouteCategory='PT' and @Route = 'OQC')))    
    BEGIN    
     UPDATE b SET b.QCType = q.QCType,b.LastModifiedOn=GETDATE()    
     FROM Batch b JOIN QAI q ON q.SerialNumber=b.SerialNumber      
     WHERE q.QAIId=@QAIID      
    END    
   ELSE    
     BEGIN          
     -- TOTT ID 142 Fix not to update QC type to batch table Qctype when QI result is Pass      
     UPDATE q SET q.QCType = b.QCType      
     FROM Batch b JOIN QAI q ON q.SerialNumber=b.SerialNumber      
     WHERE q.QAIId=@QAIID       
  END    
      END      
     END      
      
    SELECT @QAIID QAIID      
 END    
 ElSE    
 BEGIN    
 SELECT -1 QAIID    
 END    
  
 EXEC sp_xml_removedocument @idoc       
  
  
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
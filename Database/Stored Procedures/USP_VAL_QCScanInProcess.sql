-- =======================================================      
-- Name:             USP_VAL_QCScanInProcess      
-- Purpose:          Validate QC Scan In Process      
-- =======================================================      
-- Change History      
-- Date         Author     Comments      
-- -----        ------     -----------------------------      
-- 09/06/2020   Soon Siang SP created.      
-- 17/06/2020   Soon Siang Add QAI and QAQI Null Validation      
--         and handle 2nd QC Scan with Reason New    
-- 08/07/2020   Pang YS - Fix and by pass "Split Batch" QC batch  
-- 08/07/2020   Azrul - Bypass SP checking for surgical  
-- 16/12/2021   Azrul - PTPF to follow surgical logics.  
-- EXEC USP_VAL_QCScanInProcess 2200835298  
-- =======================================================      
alter PROCEDURE [dbo].[USP_VAL_QCScanInProcess]      
(      
 @SerialNumber DECIMAL      
)      
AS      
BEGIN      
 SET NOCOUNT ON;      
      
 DECLARE @ErrorMessage  NVARCHAR(MAX) = NULL,      
   @QCScanInCycleNo INT,      
   @QCType    NVARCHAR(15),      
   @QCTypeDescription NVARCHAR(30),      
   @QITestResult  NVARCHAR(10),      
   @QITestReason  NVARCHAR(50),      
   @QAIProcessDateTime DATETIME,      
   @QCProcessDateTime DATETIME,    
   @PTProcessDateTime DATETIME,  
   @IsSurgicalGlove INT = 0,  
   @serialStr NVARCHAR(20) = CAST(@SerialNumber as nvarchar(20)),
   @isPTPF INT = 0    
    
 -- Bypass SP checking for surgical  
 IF EXISTS (select 1 from DOT_FloorD365HRGLOVERPT a with (nolock) join DOT_FloorD365BO b with (nolock) on a.BthOrder = b.BthOrderId   
 where a.serialNo = @serialStr and b.prodPoolId = 'SGR' and a.IsDeleted = 0 and b.IsDeleted=0)  
 BEGIN  
 SET @IsSurgicalGlove = 1  
 END  
 -- Bypass SP checking for PTPF glove  
 select @isPTPF=(SELECT ISNULL(c.PTGLOVE, 0) FROM Batch (nolock) b LEFT JOIN AX_AVAGLOVECODETABLE_EXTENSION (NOLOCK) c ON c.GLOVECODE=b.GloveType 
 WHERE b.SerialNumber=@SerialNumber)  

 SELECT @QCScanInCycleNo = COUNT(Id) + 1      
 FROM QCYieldAndPacking WITH(NOLOCK)      
 WHERE SerialNumber = @SerialNumber AND (BatchStatus IS NULL OR BatchStatus = 'Completed')    
    
 DECLARE @QCLastBatchStatus nvarchar(50) = NULL    
 SET @QCLastBatchStatus = (SELECT TOP 1 UPPER(BatchStatus) FROM QCYieldAndPacking WITH(NOLOCK)  WHERE SerialNumber = @SerialNumber ORDER BY LastModifiedOn DESC)    
    
 -- 08/07/2020   Pang YS - Fix and by pass "Split Batch" QC batch      
 IF (@QCLastBatchStatus = 'SPLIT BATCH' OR @QCLastBatchStatus = 'QC TYPE CHANGED')    
 BEGIN    
 SET @ErrorMessage = NULL    
 END      
 ELSE IF @QCScanInCycleNo = 1      
 BEGIN      
      
  IF NOT EXISTS (SELECT TOP 1 1 FROM QAI WITH(NOLOCK) WHERE SerialNumber = @SerialNumber)      
  BEGIN      
   SELECT TOP 1 @ErrorMessage = MessageText FROM MessageMaster WITH(NOLOCK) WHERE MessageKey = 'QAI_RESULT_NOT_FOUND'      
  END      
  ELSE      
  BEGIN      
      
   SELECT TOP 1      
    @QCType = qai.QCTYPE,      
    @QCTypeDescription = qctype.DESCRIPTION      
   FROM QAI qai WITH(NOLOCK)      
   LEFT JOIN DOT_FSQCTypeTable qctype WITH(NOLOCK)      
    ON qai.QCType = qctype.QCTYPE      
   WHERE SerialNumber = @SerialNumber      
   ORDER BY QAIDate DESC, LastModifiedDateTime DESC -- YS: Change QCType shared same QAIDate. Added LastModifiedDateTime ordering to get latest record    
        
   IF @QCType = '0006020001' AND @IsSurgicalGlove = 0 AND @isPTPF = 0-- STRAIGHT PACK & Not Surgical Glove & Not PTPF Glove     
   BEGIN       
    SELECT TOP 1 @ErrorMessage = MessageText FROM MessageMaster WITH(NOLOCK) WHERE MessageKey = 'QC_VAL_QAI_NOT_FAIL'      
   END      
      
  END      
      
 END      
 ELSE      
 BEGIN      
        
  IF NOT EXISTS (SELECT TOP 1 1 FROM QAI WITH(NOLOCK) WHERE SerialNumber = @SerialNumber AND QAIScreenName = 'ScanQITestResult')      
  BEGIN      
   SELECT TOP 1 @ErrorMessage = MessageText FROM MessageMaster WITH(NOLOCK) WHERE MessageKey = 'QAQI_RESULT_NOT_FOUND'      
  END      
  ELSE      
  BEGIN      
      
   SELECT TOP 1      
    @QITestResult = QITestResult,      
    @QITestReason = QITestReason,      
    @QAIProcessDateTime = QAIDate,
    @QCType = QCType      
   FROM QAI WITH(NOLOCK)      
   WHERE SerialNumber = @SerialNumber      
    AND QAIScreenName = 'ScanQITestResult'      
   ORDER BY QAIDate DESC      
      
   SELECT TOP 1      
    @QCProcessDateTime = BatchStartTime         FROM QCYieldAndPacking WITH(NOLOCK)      
   WHERE SerialNumber = @SerialNumber      
   ORDER BY BatchStartTime DESC      
    
   --SELECT TOP 1    
   -- @PTProcessDateTime = LastModifiedOn    
   --FROM PTScanBatchCard WITH (NOLOCK)    
   --WHERE SerialNumber = @SerialNumber      
   --ORDER BY LastModifiedOn DESC    
    
      
   IF @QAIProcessDateTime < @QCProcessDateTime      
   BEGIN      
    SELECT TOP 1 @ErrorMessage = MessageText FROM MessageMaster WITH(NOLOCK) WHERE MessageKey = 'QC_VAL_DOUBLE_SCAN'      
   END      
   ELSE      
   BEGIN      
      
    IF @QITestResult <> 'FAIL'       
    BEGIN      
  --   -- to skip PTQI PASS    
  ---- test case: online batch (non sp) > QC process > QCQI (pass, SOBC post) > QAI change QC type (PT)> washer & dryer > Scan PT BC (SPBC post) > PT QI (Quick visual, Pass, RWKCR post)    
  ----            > Scan in QC (system should not block). NGC after PT process cannot do Final Packing , mandatory complete QC    
  --IF NOT (@PTProcessDateTime IS NOT NULL AND @QITestResult = 'Pass'    
  --  AND @QAIProcessDateTime > @PTProcessDateTime AND @PTProcessDateTime > @QCProcessDateTime)    
  --BEGIN    
  SELECT TOP 1 @ErrorMessage = MessageText FROM MessageMaster WITH(NOLOCK) WHERE MessageKey = 'QC_VAL_QI_NOT_FAIL'    
  --END     
    END      
    ELSE      
    BEGIN      
      
     IF @QCScanInCycleNo = 2      
     BEGIN      
            
      IF NOT (@QITestReason = 'New' OR @QITestReason = 'Normal Rework' OR @QITestReason = 'PSI Rework')      
      BEGIN      
       SELECT TOP 1 @ErrorMessage = MessageText FROM MessageMaster WITH(NOLOCK) WHERE MessageKey = 'QC_VAL_QI_NOT_REWORK'      
      END      
      
     END      
     ELSE      
     BEGIN      
      
      IF NOT (@QITestReason = 'Normal Rework' OR @QITestReason = 'PSI Rework')      
      BEGIN      
       SELECT TOP 1 @ErrorMessage = MessageText FROM MessageMaster WITH(NOLOCK) WHERE MessageKey = 'QC_VAL_QI_NOT_REWORK'      
      END      
      
     END        

     IF @QCType = '0006020020' --Azrul/Suren: Latest QCQI Fail with PT cannot proceed Scan QC.
     BEGIN
     	SELECT TOP 1 @ErrorMessage = MessageText FROM MessageMaster WITH(NOLOCK) WHERE MessageKey = 'QAI_PWT_PT_NOT_COMPLETED'       
     END

    END        
   END        
      
  END      
      
 END      
      
 SELECT @ErrorMessage      
 ----For Debug Purpose      
 --SELECT @QCScanInCycleNo AS QCScanInCycleNo,       
 --  @QCType AS QCType,       
 --  @QCTypeDescription AS QCTypeDescription,       
 --  @QITestResult AS QITestResult,       
 --  @QITestReason AS QITestReason,       
 --  @QAIProcessDateTime AS QAIProcessDateTime,       
 --  @QCProcessDateTime AS QCProcessDateTime      
      
END  
-- ==================================================================================================  
-- Name:   [USP_DOT_GET_PT_OR_QC_QITestResult]  
-- Purpose:   Get PT or QC Scan QI Test Result to detect double QAI result  
-- ==================================================================================================  
-- Change History  
-- Date               Author                     Comments  
-- -----   ------   ---------------------------------------------------------------------------------  
--  06, 01,2019    Max He       SP created.  
--  03, 17,2021   Azrul       Include surgical glove.  
--  12, 01,2021   Azrul       Include PTPF glove.  
-- ==================================================================================================  
CREATE   PROCEDURE [dbo].[USP_DOT_GET_PT_OR_QC_QITestResult]  
 @serialNumber numeric(10,0)  
  
 --set @serialNumber='1210563335'  
AS  
BEGIN  
 SET NOCOUNT ON;   
 declare @QCLastModDate datetime    
 declare @QCFirstModDate datetime    
 declare @PTLastModDate DATETIME    
 declare @QITestResult varchar(100)    
 declare @WasherScanInCount int    
 declare @DryerScanInCount int    
 declare @QCScanInCount int    
 declare @PTScanInCount int    
 declare @QITestResultCount int    
 Declare @QCType nchar(40)  
 Declare @serialStr nvarchar(20) = CAST(@SerialNumber as nvarchar(20))    
 DECLARE @isPTPF BIT = 0
 select @QCFirstModDate=min(LastModifiedOn) from QCYieldAndPacking WITH(NOLOCK) where serialnumber=@serialNumber    
 select @QCLastModDate=max(LastModifiedOn) from QCYieldAndPacking WITH(NOLOCK) where serialnumber=@serialNumber    
 select @PTLastModDate=max(LastModifiedOn) from PTScanBatchCard with(nolock) where serialnumber=@serialNumber    
 select @WasherScanInCount=count(1) from WasherScanBatchCard with(nolock) where ScanBatchEndDateTime is not null and SerialNumber=@serialNumber;    
 select @DryerScanInCount=count(1) from DryerScanBatchCard with(nolock) where ScanBatchEndDateTime is not null and SerialNumber=@serialNumber;    
 SELECT @PTScanInCount=count(1) FROM PTScanBatchCard with(nolock) WHERE serialnumber = @serialnumber;    
 select @QCScanInCount=count(1) from QCYieldAndPacking WITH(NOLOCK) where serialnumber=@serialNumber;    
 select @QITestResultCount=count(1) from QAI with(nolock) where QAIDate is not null and SerialNumber = @SerialNumber and QAIScreenName='ScanQITestResult';    
 select @isPTPF=(SELECT ISNULL(c.PTGLOVE, 0) FROM Batch (nolock) b LEFT JOIN AX_AVAGLOVECODETABLE_EXTENSION (NOLOCK) c ON c.GLOVECODE=b.GloveType WHERE b.SerialNumber=@SerialNumber)  
  
  IF EXISTS(SELECT 1 FROM  QCYieldandPacking with(nolock) WHERE serialNumber = @SerialNumber) --OQC    
  BEGIN    
   -- Try Get latest QC_QI Test Result    
   select top 1 @QITestResult = QITestResult    
   from QAI with(nolock)     
   where SerialNumber = @SerialNumber     
   --and QAIDate > @QCLastModDate  -- OLD code    
   and QAIDate > @QCLastModDate and @QCLastModDate >= ISNULL(@PTLastModDate, @QCLastModDate) -- Pang:Check if last QC date > last PT date    
   and QAIScreenName='ScanQITestResult'    
   order by qaiid desc;    
    
   --select latest QCType    
   select top 1 @QCType = b.RouteCategory    
   from QAI a with(nolock) join DOT_FSQCTypeTable b with(nolock) on a.QCType = b.QCType    
   where a.SerialNumber = @SerialNumber and a.QITestResult = 'Pass' and b.IsDeleted=0 and b.Stopped=0    
   order by qaiid desc;    
    
   IF EXISTS (SELECT 1 FROM PTScanBatchCard with(nolock) WHERE serialnumber = @serialnumber) --PT    
   BEGIN       
    if isnull(@QITestResult,'') = ''    
    BEGIN    
 -- If exam glove remain current logic  
    IF (NOT EXISTS (select 1 FROM DOT_FloorD365HRGLOVERPT a with(nolock) join DOT_FSItemMaster b WITH (NOLOCK)    
     on a.GloveCode = b.ItemId WHERE a.serialNo = @serialStr AND b.ItemType=109)  AND @isPTPF = 0)
 BEGIN  
  --  Try Get latest PT_QI Test Result and QCType: Pang add - start    
  IF EXISTS (SELECT TOP 1 1    
  FROM QAI with(nolock)     
  WHERE SerialNumber = @SerialNumber             
  AND QAIDate > @PTLastModDate and QITestResult = 'Pass' --Pang, get the QAI Test result with last PT scan in date    
  AND QAIScreenName='ScanQITestResult'     
  ORDER BY qaiid ASC)    
  BEGIN     
  SELECT @QITestResult = 'Pass'    
  END    
  --  PangYS add - end    
  ELSE    
  BEGIN    
  select top 1 @QITestResult = QITestResult    
  from QAI with(nolock)     
  where SerialNumber = @SerialNumber     
  --Try Get latest PT_QI Test Result and QCType    
  --Max, get the QAI Test result with last PT scan in date and first QC scan in date    
  and QAIDate > @PTLastModDate  and QAIDate < @QCFirstModDate    
  and QAIScreenName='ScanQITestResult'    
  order by qaiid desc;    
  END    
  END    
  ELSE  
  BEGIN  -- If Surgical or PTPF get latest PTQI result  
  select top 1 @QITestResult = QITestResult    
  from QAI with(nolock)     
  where SerialNumber = @SerialNumber     
  and QAIDate > @PTLastModDate    
  and QAIScreenName='ScanQITestResult'    
  order by qaiid desc;    
  END  
  SELECT 'PTQI' as Stage,@QITestResult as QIResult,    
  @WasherScanInCount as WasherScanInCount,@DryerScanInCount as DryerScanInCount,@PTScanInCount as PTScanInCount,    
  @QCScanInCount as QCScanInCount,@QITestResultCount as QITestResultCount,@QCType as QCType;    
  END  
    ELSE    
     BEGIN    
      SELECT 'QCQI' as Stage,@QITestResult as QIResult,    
      @WasherScanInCount as WasherScanInCount,@DryerScanInCount as DryerScanInCount,@PTScanInCount as PTScanInCount,    
      @QCScanInCount as QCScanInCount,@QITestResultCount as QITestResultCount,@QCType as QCType;    
     End    
   END        
   ELSE  -- go for OQC route    
   BEGIN     
    SELECT 'QCQI' as Stage,@QITestResult as QIResult,    
    @WasherScanInCount as WasherScanInCount,@DryerScanInCount as DryerScanInCount,@PTScanInCount as PTScanInCount,    
    @QCScanInCount as QCScanInCount,@QITestResultCount as QITestResultCount,@QCType as QCType;    
   END    
  END    
 ELSE IF EXISTS (SELECT 1 FROM PTScanBatchCard with(nolock) WHERE serialnumber = @serialnumber) --PT    
  BEGIN    
   -- If exam glove remain current logic  
   IF (NOT EXISTS (select 1 FROM DOT_FloorD365HRGLOVERPT a with(nolock) join DOT_FSItemMaster b WITH (NOLOCK)    
     on a.GloveCode = b.ItemId WHERE a.serialNo = @serialStr AND b.ItemType=109) AND @isPTPF = 0)  
   BEGIN  
    --  Try Get latest PT_QI Test Result and QCType: Pang add - start    
    IF EXISTS (SELECT TOP 1 1    
  FROM QAI with(nolock)     
  WHERE SerialNumber = @serialNumber             
  AND QAIDate > @PTLastModDate and QITestResult = 'Pass' --Pang, get the QAI Test result with last PT scan in date    
  AND QAIScreenName='ScanQITestResult'     
  ORDER BY qaiid ASC)    
    BEGIN     
  SELECT @QITestResult = 'Pass'    
    END    
    --  PangYS add - end    
    ELSE    
    BEGIN    
  -- Try Get latest PT_QI Test Result and QCType    
  select top 1 @QITestResult = QITestResult    
  from QAI with(nolock)     
  where SerialNumber = @SerialNumber     
  and QAIDate > @PTLastModDate    
  and QAIScreenName='ScanQITestResult'    
  order by qaiid desc;    
    END  
 END  
 ELSE -- If Surgical or PTPF get latest PTQI result  
 BEGIN  
  select top 1 @QITestResult = QITestResult    
  from QAI with(nolock)     
  where SerialNumber = @SerialNumber     
  and QAIDate > @PTLastModDate    
  and QAIScreenName='ScanQITestResult'    
  order by qaiid desc;    
 END  
  
   --select latest QCType    
   select top 1 @QCType = b.RouteCategory    
   from QAI a with(nolock) join DOT_FSQCTypeTable b with(nolock) on a.QCType = b.QCType    
   where a.SerialNumber = @SerialNumber and a.QITestResult = 'Pass' and b.IsDeleted=0 and b.Stopped=0    
   order by qaiid desc;    
    
   SELECT 'PTQI' as Stage,@QITestResult as QIResult,    
   @WasherScanInCount as WasherScanInCount,@DryerScanInCount as DryerScanInCount,@PTScanInCount as PTScanInCount,    
   @QCScanInCount as QCScanInCount,@QITestResultCount as QITestResultCount,@QCType as QCType;    
  END    
 ELSE    
  BEGIN    
   SELECT NULL as Stage,@QITestResult as QIResult,    
   @WasherScanInCount as WasherScanInCount,@DryerScanInCount as DryerScanInCount,@PTScanInCount as PTScanInCount,    
   @QCScanInCount as QCScanInCount,@QITestResultCount as QITestResultCount,@QCType as QCType;    
  END    
     
    SET NOCOUNT OFF;    
END  
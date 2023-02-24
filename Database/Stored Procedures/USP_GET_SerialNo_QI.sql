  
 -- ==================================================================================================      
-- Name:   [USP_GET_SerialNo_QI]      
-- Purpose:   <Check whether QI Result>      
-- ==================================================================================================      
-- Change History      
-- Date               Author                     Comments      
-- -----   ------   ---------------------------------------------------------------------------------      
-- <21 Oct,2014>   <Srikanth Balda>          SP created.      
-- <06 Oct,2016>  <Cheah Teik Chuan>    added new checking for additional QCTypes      
-- <25 Oct,2018>  <Pang Yik Siu>    add new QCtypes      
-- <28 Sept,2018>  <Amirul Kamil>    comment quarantine/repack checking      
-- <09 Nov,2020>  <Azrul>    Surgical Glove must check PTQI & QCQI            
-- <29 July,2021> <Pang Yik Siu>  Add nolock  
-- ==================================================================================================      
-- exec usp_get_serialno_qi '2200835464'      
ALTER PROCEDURE [dbo].[USP_GET_SerialNo_QI]      
(      
     @serialNumber numeric(10,0)  
)      
AS      
BEGIN      
 SET NOCOUNT ON;      
 Declare @QCtype nvarchar(15)      
 Declare @BatchType nchar(40)      
 declare @QCLastModDate datetime      
 declare @PTLastModDate datetime
 declare @QAIDateForLatestScan datetime  
 declare @IsSurgicalGlove INT = 0
 declare @serialStr nvarchar(20) = CAST(@SerialNumber as nvarchar(20))
 select @QCType = QCType from Batch (NOLOCK) where serialnumber = @serialNumber       
 select @QCLastModDate=max(LastModifiedOn) from QCYieldAndPacking WITH (nolock) where serialnumber=@serialNumber      
 select @PTLastModDate=max(LastModifiedOn) from PTScanBatchCard WITH (nolock) where serialnumber=@serialNumber  
 SELECT @QAIDateForLatestScan = MAX(QAIDate) FROM QAI WITH (nolock) where serialnumber=@serialNumber   
         AND (QAIScreenName IN ('QAIScanInnerTenPcs','QAIScan') OR QAIScreenName IS NULL)     
       
 -- Bypass SP checking for surgical    
 IF exists (select 1 from Batch b with (nolock) join DOT_FSItemMaster i with (nolock) on b.GloveType = i.ItemId 
		 where b.SerialNumber = @SerialNumber and i.ItemType = 109) -- surgical glove
 BEGIN    
 SET @IsSurgicalGlove = 1    
 END    
    
 IF (@QCType != '0006020001' OR @IsSurgicalGlove = 1) -- Surgical Glove must check PTQI & QCQI     
 BEGIN       
  --if(@QCType = '0006020019') --quarantine      
  --begin      
  -- IF Exists(select 1 from FP_SerialNo_Exemption (NOLOCK) where SerialNumber=@serialNumber) --check if there is exemption. if yes, will always pass      
  -- begin      
  --  Select 'Pass'      
  -- end      
  -- else      
  -- begin      
  --  Select 'Fail1'      
  -- end      
  --end      
  if(@QCType = '0006020018') --dimension failed      
  begin      
   IF Exists(select 1 from FP_SerialNo_Exemption WITH (nolock) where SerialNumber=@serialNumber) --check if there is exemption. if yes, will always pass      
   begin      
    Select 'Pass'      
   end      
   else      
   begin      
    IF EXISTS(SELECT 1 FROM  QCYieldandPacking WITH (nolock) WHERE serialNumber = @SerialNumber)      
    BEGIN      
     SELECT @BatchType = Batchtype FROM Batch (nolock) where serialnumber = @serialNumber       
     IF (@BatchType = 'QWT' or @BatchType = 'PWT' or @BatchType = 'OWT' or @BatchType = 'PSW')      
     BEGIN         
      IF EXISTS (SELECT 1 FROM PTScanBatchCard WITH (nolock) WHERE serialnumber = @serialnumber)      
       BEGIN      
        if exists(Select 1 from QAI WITH (nolock) WHERE serialnumber = @serialNumber       
        and QAIDate > @PTLastModDate      
        and QAIScreenName='ScanQITestResult')      
        begin      
         SELECT TOP 1 case when QITestResult is null then 'Fail'      
         else case when QITestResult='Fail' then 'Fail1'      
         else QITestResult end end       
         FROM QAI with (nolock) WHERE serialnumber = @serialNumber       
         and QAIDate > @PTLastModDate      
         and QAIScreenName='ScanQITestResult'      
         order by qaiid desc      
        end      
        else      
        begin      
         select 'Fail'      
        end      
       END     
      ELSE      
       BEGIN      
        SELECT 'Fail'      
       End      
     END          
     ELSE      
     BEGIN       
      if exists(Select 1 from QAI with (nolock) WHERE serialnumber = @serialNumber       
      and QAIDate > @PTLastModDate      
      and QAIScreenName='ScanQITestResult')      
      begin      
       SELECT TOP 1 case when QITestResult is null then 'Fail'      
       else case when QITestResult='Fail' then 'Fail1'      
       else QITestResult end end       
       FROM QAI (nolock) WHERE serialnumber = @serialNumber       
       and QAIDate > @PTLastModDate      
       and QAIScreenName='ScanQITestResult'      
       order by qaiid desc      
      end      
      else      
      begin      
       select 'Fail'      
      end      
     END      
    END      
    ELSE      
    BEGIN        
     SELECT 'Fail'      
    END       
   end      
  end      
  else if (@QCType = '0006020020') --PT      
  begin      
   IF Exists(select 1 from FP_SerialNo_Exemption WITH (nolock) where SerialNumber=@serialNumber) --check if there is exemption. if yes, will always pass      
   begin      
    Select 'Pass'      
   end      
   else      
   begin      
    IF EXISTS (SELECT 1 FROM PTScanBatchCard WITH (nolock) WHERE serialnumber = @serialnumber)      
     BEGIN      
      if exists(Select 1 from QAI (nolock) WHERE serialnumber = @serialNumber       
      and QAIDate > @PTLastModDate      
      and QAIScreenName='ScanQITestResult')      
      begin      
       SELECT TOP 1 case when QITestResult is null then 'Fail'      
       else case when QITestResult='Fail' then 'Fail1'      
       else QITestResult end end       
       FROM QAI WITH (nolock) WHERE serialnumber = @serialNumber       
       and QAIDate > @PTLastModDate      
       and QAIScreenName='ScanQITestResult'      
       order by qaiid desc      
      end      
      else      
      begin      
       select 'Fail'      
      end      
     END      
    ELSE      
     BEGIN      
      SELECT 'Fail'      
     End      
   end      
  end      
  else -- QCType not PT, Dimension Failed, or Quarantine      
  begin       
   -- other non-straight pack QC Type :  -- <25 Oct,2018>  <Pang Yik Siu> add new QCtypes      
   IF Exists(select 1 from FP_SerialNo_Exemption (nolock) where SerialNumber=@serialNumber) --check if there is exemption. if yes, will always pass      
   BEGIN      
    SELECT 'Pass'      
   END      
   ELSE      
   BEGIN      
    IF EXISTS(SELECT 1 FROM  QCYieldandPacking with (nolock) WHERE serialNumber = @SerialNumber)      
    BEGIN      
     SELECT @BatchType = Batchtype FROM Batch (nolock) where serialnumber = @serialNumber       
     IF (@BatchType = 'QWT' or @BatchType = 'PWT' or @BatchType = 'OWT' or @BatchType = 'PSW')      
      BEGIN          
       IF EXISTS (SELECT 1 FROM PTScanBatchCard with (nolock) WHERE serialnumber = @serialnumber)      
        BEGIN      
         --SELECT TOP 1 QITestResult FROM QAI WHERE serialnumber = @serialNumber order by qaiid desc  
		 -- pang: add QI screen filter and 1 year QAI scan checking     
         --SELECT TOP 1 QITestResult FROM QAI (nolock) WHERE serialnumber = @serialNumber and QAIScreenName='ScanQITestResult' order by qaiid desc  
         SELECT TOP 1 QITestResult FROM QAI (NOLOCK)  
         WHERE serialnumber = @serialNumber  
          AND QAIScreenName='ScanQITestResult'   
          AND QAIDate > @QAIDateForLatestScan  
         ORDER BY QAIId DESC  
        END      
       ELSE      
        BEGIN      
         SELECT 'Fail'      
        End      
      END          
     ELSE      
      BEGIN  --Check last QCQI status               
  IF EXISTS(SELECT TOP 1 QITestResult FROM QAI (nolock) WHERE serialnumber = @serialNumber and QAIDate > @QCLastModDate order by qaiid desc)  
   BEGIN  
    SELECT TOP 1 QITestResult FROM QAI (nolock) WHERE serialnumber = @serialNumber and QAIDate > @QCLastModDate  order by qaiid desc  
   END  
  ELSE  
   BEGIN  
    SELECT 'Fail'   
   END  
      END      
    END      
    ELSE      
    BEGIN        
     SELECT 'Fail'      
    END       
   END          
  end      
 end      
 Else      
 Begin        
  IF EXISTS (SELECT 1 FROM PTScanBatchCard (nolock) WHERE serialnumber = @serialnumber)      
   BEGIN   
    -- pang: if the latest QAI Date (QAI date belong to QAI screen name  (QAIScan/QAIScanInnerTenPcs/NULL))  
    -- , is bigger than the latest PT/QC latest date & the latest Qctype is 0006020001 (SP) from Batch table  
    --  , final packing should allow to pack.  
    IF (@QAIDateForLatestScan > @PTLastModDate)  
    BEGIN  
     SELECT 'Pass'  
    END  
    ELSE  
    BEGIN  
      -- pang: add QI screen filter    
     SELECT TOP 1 QITestResult FROM QAI (nolock) where serialnumber = @serialNumber and QAIScreenName='ScanQITestResult' order by QAIDate desc  
    END  
   END
  ELSE      
   BEGIN      
      SELECT @BatchType = Batchtype FROM Batch (nolock) where serialnumber = @serialNumber      
       IF (@BatchType = 'QWT' or @BatchType = 'PWT'  or @BatchType = 'OWT' or @BatchType = 'PSW')      
       BEGIN      
      SELECT 'Fail'      
       END      
       ELSE      
       BEGIN      
      SELECT 'Pass'      
       END      
   END         
 End          
    SET NOCOUNT OFF;      
END
GO
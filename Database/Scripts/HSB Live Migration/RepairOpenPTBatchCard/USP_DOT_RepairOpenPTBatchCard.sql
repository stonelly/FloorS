---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
-- ==================================================================================================    
-- Name:   [USP_DOT_RepairOpenPTBatchCard]    
-- Purpose:   Repair HSB open batch card PT process fail before pass     
-- ==================================================================================================    
-- Change History    
-- Date               Author                     Comments    
-- -----   ------   ---------------------------------------------------------------------------------    
--  29, 03,2022    Max He       SP created.    
-- ==================================================================================================    
-- exec USP_DOT_RepairOpenPTBatchCard '1220015526'  
alter   PROCEDURE [dbo].[USP_DOT_RepairOpenPTBatchCard]        
(    
@SerialNumber NVARCHAR(4000)     
)      
      
AS      
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.          
 SET NOCOUNT ON;      
       
IF OBJECT_ID('tempdb..#QAItemp') IS NOT NULL        
DROP TABLE #QAItemp       
  
  
CREATE TABLE #QAItemp(  
 [QAIId] [int] NOT NULL,  
 [QAIDate] [datetime] NOT NULL,  
 [QAIInspectorId] [nvarchar](10) NOT NULL,  
 [SerialNumber] [numeric](15, 0) NOT NULL,  
 [BatchNumber] [nvarchar](20) NOT NULL,  
 [QCType] [nvarchar](30) NOT NULL,  
 [WTSampliingSize] [int] NULL,  
 [VTSamplingSize] [int] NULL,  
 [InnerBox] [int] NULL,  
 [TenPcsWeight] [decimal](18, 3) NULL,  
 [PackingSize] [int] NULL,  
 [QAIChangeReason] [nvarchar](100) NULL,  
 [QITestResult] [nvarchar](10) NULL,  
 [ResamplingCount] [int] NULL,  
 [ChangeQCTypeReason] [nvarchar](20) NULL,  
 [LastModifiedDateTime] [datetime] NULL,  
 [WorkStationId] [int] NOT NULL,  
 [SubModuleId] [int] NOT NULL,  
 [IsResampling] [bit] NULL,  
 [QCTypeAuthorizedBy] [nvarchar](10) NULL,  
 [SuggestedQCType] [nvarchar](30) NULL,  
 [QAIScreenName] [nvarchar](200) NULL,  
 [AQLValue] [nvarchar](100) NULL,  
 [IsAXPostingSuccess] [bit] NULL,  
 [HBSamplingSize] [int] NULL,   
 [QITestReason] [nvarchar](50) NULL,
 [TenPcsSamplingSize] [int] NULL   
 )  
  
declare @PTLastModDate datetime, @LastQAIDate datetime;  
select @PTLastModDate=max(LastModifiedOn) from PTScanBatchCard with(nolock) where serialnumber=@serialNumber  
-- last qai date should greater than last PT scan in date  
select @LastQAIDate = DATEADD(minute, 5, @PTLastModDate)  
  
insert into #QAItemp  
select top 1 *   
from qai with(nolock)  
where SerialNumber =@SerialNumber --'1220015526'  
order by qaiDate desc  
  
INSERT INTO [dbo].[QAI]  
           ([QAIDate]  
           ,[QAIInspectorId]  
           ,[SerialNumber]  
           ,[BatchNumber]  
           ,[QCType]  
           ,[WTSampliingSize]  
           ,[VTSamplingSize]  
           ,[InnerBox]  
           ,[TenPcsWeight]  
           ,[PackingSize]  
           ,[QAIChangeReason]  
           ,[QITestResult]  
           ,[ResamplingCount]  
           ,[ChangeQCTypeReason]  
           ,[LastModifiedDateTime]  
           ,[WorkStationId]  
           ,[SubModuleId]  
           ,[IsResampling]  
           ,[QCTypeAuthorizedBy]  
           ,[SuggestedQCType]  
           ,[QAIScreenName]  
           ,[AQLValue]  
           ,[IsAXPostingSuccess]  
           ,[HBSamplingSize]   
           ,[QITestReason] 
           ,[TenPcsSamplingSize])  
select   
           [QAIDate]  
           ,[QAIInspectorId]  
           ,[SerialNumber]  
           ,[BatchNumber]  
           ,'0006020005'  
           ,[WTSampliingSize]  
           ,[VTSamplingSize]  
           ,[InnerBox]  
           ,[TenPcsWeight]  
           ,[PackingSize]  
           ,[QAIChangeReason]  
           ,[QITestResult]  
           ,[ResamplingCount]  
           ,[ChangeQCTypeReason]  
           ,'2022-03-29 14:04'  
           ,[WorkStationId]  
           ,[SubModuleId]  
           ,[IsResampling]  
           ,[QCTypeAuthorizedBy]  
           ,[SuggestedQCType]  
           ,[QAIScreenName]  
           ,[AQLValue]  
           ,[IsAXPostingSuccess]  
           ,[HBSamplingSize]  
           ,[QITestReason]
           ,[TenPcsSamplingSize]    
from #QAItemp  
  
--select *  
update a set QITestResult='Pass',QITestReason='New', QAIDate=@LastQAIDate  
from qai a with(nolock)  
join #QAItemp b on a.qaiid=b.qaiid  
  
  
end
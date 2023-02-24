
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_GetAllPalletForEWN]...';


GO
  
CREATE PROCEDURE [dbo].[USP_DOT_GetAllPalletForEWN]        
(        
 @NumberOfRecords int = 100      
)         
AS        
BEGIN      
         
declare @sql varchar(max);      
      
SET @sql = '      
SELECT TOP ' + CAST(@NumberOfRecords AS varchar(10)) + ' max(a.Id) Id,a.PalletId, a.PlantNo, a.PalletSerialNo         
FROM DOT_FloorAxIntParentTable a with(nolock)       
where FunctionIdentifier in (''SBC'',''SMBP'',''SGBC'',''SPPBC'')      
and ProcessingStatus in (0,1,3,4,5,6) and a.PalletId is not null and a.IsEwarenaviPosted = 0 and IsMigratedFromAX6 = 0      
and not exists (    
select     
palletSerialno    
from dot_ewpostinglog with(nolock)    
where palletserialno=a.PalletSerialNo and isnull(IsEwarenaviPosted,0) = 1 and isnull(EwarenaviLog,'''')<>''''  
)
group by a.PalletId, a.PlantNo, a.PalletSerialNo  
order by max(a.Id) desc;';        
      
EXEC ( @sql )         
      
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_GetAllReportAsFinishedForCGLVList]...';


GO

-- =============================================  
-- Author:  <Amir>  
-- Create date: <26-Nov-2021>  
-- Description: <Get Glove Change All Report As Finished>  
-- exec USP_DOT_GetAllReportAsFinishedForCGLVList @CutOffTime=N'2021-08-01 01:00:00.000',@BatchOrder=N'HNBON000372103'
-- =============================================  
CREATE   PROCEDURE [dbo].[USP_DOT_GetAllReportAsFinishedForCGLVList]  
  @CutOffTime varchar(100),
  @BatchOrder NVARCHAR(100)
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 
DECLARE @sql varchar(max);  
 SET @sql = '  
 select a.* from (
select b.ParentRefRecId as ParentId,b.RAFGoodQty,b.Weightof10Pcs,b.RAFHBSample,b.RAFVTSample,b.RAFWTSample,BatchWeight,b.BatchOrderNumber, 
b.Resource,b.PostingDateTime, a.PlantNo, a.ReferenceBatchNumber1, a.BatchNumber
from DOT_FloorAxIntParentTable a with(nolock)
join DOT_RafStgTable b with(nolock) on a.id=b.ParentRefRecId
where a.IsDeleted=0 and b.IsDeleted=0 and a.IsConsolidated = 1 
and a.FunctionIdentifier in (''CGLV'') 
and ISNULL(a.ReferenceBatchNumber1,'''') <> ''RESAMPLE'' 
and a.ProcessingStatus in (0,1,4,5) and a.IsDeleted=0 and a.IsMigratedFromAX6 = 0 
and a.AutoConsoleMarking=1 and a.ConsoleMarkingTime <=''' + @CutOffTime + ''' 
and b.BatchOrderNumber = ''' + @BatchOrder + ''' 
) a'

 EXEC (@sql + ' order by a.PostingDateTime') 
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_GetAllReportAsFinishedList]...';


GO
-- =============================================  
-- Author:  <Azrul>  
-- Create date: <19-May-2021>  
-- Description: <Get All Report As Finished>  
-- exec USP_DOT_GetAllReportAsFinishedList @CutOffTime=N'2021-08-01 01:00:00.000',@BatchOrder=N'HNBON000372103'
-- =============================================  
CREATE PROCEDURE [dbo].[USP_DOT_GetAllReportAsFinishedList]  
  @CutOffTime varchar(100),
  @BatchOrder NVARCHAR(100)
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 
DECLARE @sql varchar(max);  
 SET @sql = '  
 select a.* from (
select b.ParentRefRecId as ParentId,b.RAFGoodQty,b.Weightof10Pcs,b.RAFHBSample,b.RAFVTSample,b.RAFWTSample,BatchWeight,b.BatchOrderNumber, 
b.Resource,b.PostingDateTime, a.PlantNo, a.ReferenceBatchNumber1, a.BatchNumber
from DOT_FloorAxIntParentTable a with(nolock)
join DOT_RafStgTable b with(nolock) on a.id=b.ParentRefRecId
where a.IsDeleted=0 and b.IsDeleted=0 and a.IsConsolidated = 1 
and a.FunctionIdentifier in (''HBC'',''SRBC'',''PNBC'') 
and ISNULL(a.ReferenceBatchNumber1,'''') <> ''RESAMPLE'' 
and a.ProcessingStatus in (0,1,4,5) and a.IsDeleted=0 and a.IsMigratedFromAX6 = 0 
and a.AutoConsoleMarking=1 and a.ConsoleMarkingTime <=''' + @CutOffTime + ''' 
and b.BatchOrderNumber = ''' + @BatchOrder + ''' 
) a'

 EXEC (@sql + ' order by a.PostingDateTime') 
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_GetAllSMBPJournalList]...';


GO
-- =============================================    
-- Author:  <Azrul>    
-- Create date: <9-June-2021>    
-- Description: <Get All SMBP>    
-- exec [dbo].[USP_DOT_GetAllFGBatchOrderList]   
-- exec USP_DOT_GetAllSMBPJournalList '2021-08-22 23:59:59:999','HNBON000476779',1  
-- =============================================    
CREATE   PROCEDURE [dbo].[USP_DOT_GetAllSMBPJournalList]    
  @CutOffTime varchar(100),  
  @BatchOrder NVARCHAR(100),  
  @ConsolidationSequence NVARCHAR(100)  
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
   
DECLARE @sql varchar(max);    
 SET @sql = '    
 select a.* from (  
select b.ParentRefRecId as ParentId, b.BatchOrderNumber, b.Configuration as Config, CAST(a.FGQuantity as DECIMAL) as Qty, a.PlantNo, a.ReferenceBatchNumber1,   
a.ReferenceBatchNumber2, a.ReferenceBatchNumber3,a.ReferenceBatchNumber4, a.ReferenceBatchNumber5, b.RefNumberOfPieces1, b.RefNumberOfPieces2, b.RefNumberOfPieces3,   
b.RefNumberOfPieces4, b.RefNumberOfPieces5, a.ReferenceBatchSequence1, a.ReferenceBatchSequence2, a.ReferenceBatchSequence3, a.ReferenceBatchSequence4, a.ReferenceBatchSequence5,   
b.PostingDateTime,b.ReferenceItemNumber as ItemNo,b.InnerLotNumber as LotNo,b.CustomerReference as CustRef, CAST(a.PreshipmentCases as DECIMAL) as PreshipmentCases  
from DOT_FloorAxIntParentTable a with(nolock)  
join DOT_FGJournalTable b with(nolock) on a.id = b.ParentRefRecId  
where a.FunctionIdentifier in (''SMBP'',''MMTS'')  
and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 and a.IsConsolidated = 1     
and a.AutoConsoleMarking=1 and a.ConsoleMarkingTime <=''' + @CutOffTime + '''   
and a.ProcessingStatus in (0,1,4,5) -- SMBP not console   
and b.BatchOrderNumber = ''' + @BatchOrder + '''    
and a.ConsolidationSequence = ''' + @ConsolidationSequence + '''   
) a'  
  
  
 EXEC (@sql + ' order by a.PostingDateTime')    
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_GetAllStagingId]...';


GO
CREATE PROCEDURE [dbo].[USP_DOT_GetAllStagingId]            
(            
 @NumberOfRecords int = 100,          
 @SqlWhere varchar(max)='1=1',          
 @ChildTableName varchar(max),          
 @ReturnFields varchar(max)          
)             
AS            
BEGIN          
             
declare @sql varchar(max);          
DECLARE @sqlSort varchar(1000)= 'a.LastModificationTime,a.ProcessingStatus'          
SET @sql = '          
SELECT TOP ' + CAST(@NumberOfRecords AS varchar(10)) + ' a.Id, '+ @ReturnFields +'           
FROM DOT_FloorAxIntParentTable a with (nolock)          
inner join ' + @ChildTableName +' b with (nolock) on a.Id=b.ParentRefRecId          
where ProcessingStatus in (0,1,5) and a.IsDeleted=0 and IsMigratedFromAX6 = 0        
and ' + @SqlWhere        
+ ' Order by ' + @sqlSort        
          
EXEC ( @sql )             
          
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_GetAllStagingParent]...';


GO
-- =============================================    
-- Change History    
-- Date			Author		Comments    
-- -----		------		-----------------------------    
-- 16/10/2019	Max He		USP_DOT_GetAllStagingParent 100,'a.[FunctionIdentifier] In (N''SBC'', N''SMBP'', N''SGBC'')','DOT_FGJournalTable', 'b.PostingDateTime'      
-- =============================================  
    
CREATE PROCEDURE [dbo].[USP_DOT_GetAllStagingParent]            
(            
 @NumberOfRecords int = 100,          
 @SqlWhere varchar(max)='1=1',          
 @ChildTableName varchar(max),          
 @ReturnFields varchar(max)          
)             
AS            
BEGIN          
             
declare @sql varchar(max);          
DECLARE @sqlSort varchar(1000)= 'a.LastModificationTime,a.ProcessingStatus'          
SET @sql = '          
SELECT TOP ' + CAST(@NumberOfRecords AS varchar(10)) + ' a.*, '+ @ReturnFields +'           
FROM DOT_FloorAxIntParentTable a with (nolock)          
inner join ' + @ChildTableName +' b with (nolock) on a.Id=b.ParentRefRecId          
where ProcessingStatus in (0,1,5) and a.IsDeleted=0 and IsMigratedFromAX6 = 0        
and ' + @SqlWhere        
+ ' Order by ' + @sqlSort        
          
EXEC ( @sql )             
          
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_GetAllSummarizationDetails]...';


GO

-- =============================================
-- Author:		<Amir>
-- Create date: <21-Apr-2021>
-- Description:	<Get All Summarization Details>
-- =============================================
CREATE PROCEDURE [dbo].[USP_DOT_GetAllSummarizationDetails]
@filter varchar(max) = '1=1',
@MaxResultCount int = 100,
@SkipCount int = 0,
@GetAll bit = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @sql varchar(max);
	DECLARE @sql2 varchar(max);
	DECLARE @order varchar(max);

	SET @sql = '
SELECT COUNT(1) OVER() TotalCount, * from
UFN_DOT_GetAllSummary()
 as t
'

--if no filter order by console date
IF(@filter) = '1=1'
	SET @order = ' order by t.ConsolidationDateAndTime asc';
--else order by modulesequence, subsequence by order of 1,2,0, then console time
ELSE
	SET @order = ' order by D365BatchNumber,CustomerReference,t.ConsolidationDateAndTime asc, ModuleSequence, (CASE WHEN SubSequence = 0 THEN 1 ELSE 0 END) asc , SubSequence';


IF @GetAll = 1
	EXEC (@sql + ' where ' + @filter + @order)
ELSE
	EXEC (@sql + ' where ' + @filter + @order + ' OFFSET ' + @SkipCount + ' ROWS FETCH NEXT ' + @MaxResultCount + ' ROWS ONLY')


END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_GetAllSurgicalJournalList]...';


GO

-- =============================================  
-- Author:  <Azrul>  
-- Create date: <19-May-2021>  
-- Description: <Get All FG>  
-- exec USP_DOT_GetAllSurgicalJournalList '2021-08-22 23:59:59:999','HNBON000351504',1
-- =============================================  
CREATE PROCEDURE [dbo].[USP_DOT_GetAllSurgicalJournalList]  
  @CutOffTime varchar(100),
  @BatchOrder NVARCHAR(100),
  @ConsolidationSequence NVARCHAR(100)
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 
DECLARE @sql varchar(max);  
 SET @sql = '  
 select a.* from (
select a.Id as ParentId,b.BatchOrderNumber, b.GloveSize as Config,b.GloveSampleQuantity as SampleQuantity, b.batchNumber as SerialNo,
CAST(d.BaseQuantity as DECIMAL) as BaseQuantity,CAST(b.Quantity as DECIMAL) as Qty, b.PickingListQuantity as GloveQuantity, b.PostingDateTime,
b.ReferenceItemNumber as ItemNo,b.InnerLotNumber as LotNo,b.CustomerReference as CustRef, a.PlantNo,CAST(a.PreshipmentCases as DECIMAL) as PreshipmentCases 
from DOT_FloorAxIntParentTable a with(nolock)
join DOT_FGJournalTable b with(nolock) on a.id=b.ParentRefRecId
join DOT_FloorSalesLine d with(nolock) on d.SalesId = b.SalesOrderNumber AND d.ItemId = b.ItemNumber AND d.CustomerSize = b.Configuration and d.IsDeleted=0
where a.FunctionIdentifier in (''SPPBC'')
and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 and a.IsConsolidated = 1  
and a.AutoConsoleMarking=1 and a.ConsoleMarkingTime <=''' + @CutOffTime + ''' 
and a.ProcessingStatus in (0,1,4,5) -- SPPBC not console 
and b.BatchOrderNumber = ''' + @BatchOrder + '''  
and a.ConsolidationSequence = ''' + @ConsolidationSequence + '''
and d.IsDeleted = 0
) a'

 EXEC (@sql + ' order by a.PostingDateTime')  
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_GetFGStagingWithDetail]...';


GO
-- =============================================      
-- Author:  Max He    
-- Create date: <28-July-2021>      
-- Description: Get All FG Staging with Detail based on Parent Record ids     
-- exec USP_DOT_GetFGStagingWithDetail '8018001,8018036','SBC'   --split SBC SP: HBC > SBC (dev) (PN-PN)    
-- exec USP_DOT_GetFGStagingWithDetail '9302727,9302728,9302729', 'SBC' --split SBC QC:  HBC > RWKCR > SOBC > SBC > SBC (dev) (PN-QC-QC)    
--                  --split SBC SP & QC: HBC > SBC > RWKCR > SOBC > SBC    
--                  --split SBC SP & Temp pack: HBC > SBC > STPI > STPO > SBC    
--                  --split SBC QC & Temp pack: HBC > RWKCR > SOBC > SBC > STPI > STPO > SBC     
-- exec USP_DOT_GetFGStagingWithDetail '7910161,7910162','SBC'  --(uat2)    
-- exec USP_DOT_GetFGStagingWithDetail '10','SBC' --(uat2)    
-- exec USP_DOT_GetFGStagingWithDetail '7910044','SPPBC' --(uat2)    
-- exec USP_DOT_GetFGStagingWithDetail '7910167','SBC' --(uat2) split consolidate (QC)    
-- exec USP_DOT_GetFGStagingWithDetail '7910168','SBC' --(uat2) split consolidate (QC)    
-- exec USP_DOT_GetFGStagingWithDetail '8018834,8021101,8024995,8077607,8083607','SMBP' --online batch (dev)    
-- exec USP_DOT_GetFGStagingWithDetail '8060641,8064617,8068527,8068730,8070057,8076762,8080534','SMBP' --online batch (dev)    
-- exec USP_DOT_GetFGStagingWithDetail '8346930,8347282','SMBP' --offline batch (dev)    
-- =============================================      
CREATE OR ALTER PROCEDURE [dbo].[USP_DOT_GetFGStagingWithDetail]      
@ParentIds varchar(max),    
@FunctionIdentifier varchar(20)    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
     
DECLARE @sql varchar(max);      
    
IF (@FunctionIdentifier = 'SBC' or @FunctionIdentifier = 'MTS')    
BEGIN    
 SET @sql = '    
 select a.* into #tempfg  from (      
 select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQty, a.PlantNo, a.Sequence as SequenceNo, a.BatchNumber as SerialNo,        
 CAST(d.BaseQuantity as DECIMAL) as BaseQuantity,CAST(b.Quantity as DECIMAL) as Qty, CAST(b.Quantity * d.BaseQuantity as DECIMAL) as GloveQty,b.PostingDateTime,      
 b.ReferenceItemNumber as ItemNo, b.InnerLotNumber as LotNo, b.CustomerReference as CustRef, CAST(a.PreshipmentCases as DECIMAL) as PreshipmentCases,     
 a.FunctionIdentifier, d.Id as FGSumId, a.D365BatchNumber as ParentD365BatchNumber, e.D365BatchNumber as DetailsD365BatchNumber    
 from DOT_FloorAxIntParentTable a with(nolock)      
 join DOT_FGJournalTable b with(nolock) on a.id = b.ParentRefRecId      
 join DOT_FGSumTable d with(nolock) on d.SalesOrderNumber = b.SalesOrderNumber     
 join DOT_FGSumTableDetails e on d.id = e.ParentRefRecId    
 AND d.ItemNumber = b.ItemNumber     
 AND d.ItemSize = b.Configuration      
 and d.D365BatchNumber = a.D365BatchNumber    
 and d.FunctionIdentifier = a.FunctionIdentifier    
 where a.IsDeleted = 0   
 --and a.IsMigratedFromAX6 = 0   
 and (a.IsMigratedFromAX6 = 0 or (a.IsMigratedFromAX6 = 1 and EXISTS(select 1 from DOT_InventBatchSum with (nolock) where BatchNumber = a.BatchNumber and IsMigratedFromAX6 = 1)))  
 and a.Id in (' + @ParentIds + ')    
    and d.DetailStagingJSON = ''' + @ParentIds + ''' -- SBC D365 BatchNumber may not unique enough     
 ) a    
    
 select aa.*, COALESCE(Max(b.Location), Max(c.Location), MAX(d.location)) as Location, aa.DetailsD365BatchNumber as D365BatchNumber     
 from #tempfg aa with (NOLOCK)    
 join DOT_FloorAxIntParentTable a with (NOLOCK) on a.BatchNumber = aa.SerialNo    
 left join DOT_RafStgTable b with (nolock) on b.ParentRefRecId = a.id    
 left join DOT_TransferJournal c with (nolock) on c.ParentRefRecId = a.id    
 left join DOT_InventBatchSum d with (NOLOCK) on a.BatchNumber = d.BatchNumber and d.IsMigratedFromAX6 = 1  
 where a.BatchNumber = aa.SerialNo   
 and ((a.Sequence < aa.SequenceNo) or  (a.IsMigratedFromAX6 = 1 and EXISTS(select 1 from DOT_InventBatchSum with (nolock) where BatchNumber = a.BatchNumber and IsMigratedFromAX6 = 1)))  
 and a.IsDeleted = 0   
  and (a.IsMigratedFromAX6 = 0 or (a.IsMigratedFromAX6 = 1 and EXISTS(select 1 from DOT_InventBatchSum with (nolock) where BatchNumber = a.BatchNumber and IsMigratedFromAX6 = 1)))  
 and aa.DetailsD365BatchNumber = a.D365BatchNumber    
 and b.Location is not null or c.Location is not null  or d.Location is not null  
 group by aa.ParentId, aa.BatchOrderNumber, aa.Config, aa.SampleQty, aa.PlantNo, aa.SequenceNo, aa.SerialNo,     
 aa.BaseQuantity, aa.Qty, aa.GloveQty, aa.PostingDateTime, aa.ItemNo, aa.LotNo, aa.CustRef, aa.PreshipmentCases,     
 aa.FunctionIdentifier, aa.FGSumId, a.D365BatchNumber, aa.DetailsD365BatchNumber, aa.ParentD365BatchNumber     
 '     
END    
    
IF (@FunctionIdentifier = 'SPPBC')    
BEGIN    
 SET @sql = '    
 select a.id as FGSumId, 
  case when e.D365BatchNumber is null and EXISTS(select 1 from DOT_InventBatchSum with (nolock) where BatchNumber = d.BatchNumber and IsMigratedFromAX6 = 1) then d.BatchNumber else e.D365BatchNumber end as D365BatchNumber,
  d.BatchNumber as SerialNo, d.GloveSize as Config, ''QC'' as Location,     
 d.PickingListQuantity as GloveQty, d.GloveSampleQuantity as SampleQty    
 from DOT_FloorAxIntParentTable c with(nolock) join DOT_FGJournalTable d with(nolock) on c.id = d.ParentRefRecId    
 join DOT_FGSumTable a with(nolock) on a.DetailStagingJSON = ''' + @ParentIds + '''    
 left join DOT_FloorAxIntParentTable e with(nolock) on e.BatchNumber = d.BatchNumber and e.FunctionIdentifier = ''SRBC'' and e.Sequence=1
 where c.id in (' + @ParentIds + ') 
 and a.FunctionIdentifier = ''SPPBC''   
 '    
END    
    
IF (@FunctionIdentifier = 'SMBP')    
BEGIN    
 SET @sql = '      
 select a.* into #tempfg  from (     
 Select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQty, a.PlantNo, b.PostingDateTime, a.FunctionIdentifier,     
 b.InnerLotNumber as LotNo, b.CustomerReference as CustRef, a.PreshipmentCases, b.SalesOrderNumber, b.Quantity as Qty,    
 a.ReferencebatchNumber1 as SerialNo, a.ReferenceBatchSequence1 as SequenceNo, b.RefNumberOfPieces1 as GloveQty, b.RefItemNumber1 as ItemNo    
 from DOT_FloorAxIntParentTable a join DOT_FGJournalTable b    
 join DOT_FGSumTable d with(nolock) on d.SalesOrderNumber = b.SalesOrderNumber    
 on a.id = b.ParentRefRecId    
 where a.IsDeleted = 0   
 --and a.IsMigratedFromAX6 = 0   
  and (a.IsMigratedFromAX6 = 0 or (a.IsMigratedFromAX6 = 1 and EXISTS(select 1 from DOT_InventBatchSum with (nolock) where BatchNumber = a.BatchNumber and IsMigratedFromAX6 = 1)))  
 and a.id in (' + @ParentIds + ')    
    
 UNION ALL    
    
 Select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQty, a.PlantNo, b.PostingDateTime, a.FunctionIdentifier,     
 b.InnerLotNumber as LotNo, b.CustomerReference as CustRef, a.PreshipmentCases, b.SalesOrderNumber, b.Quantity as Qty,    
 a.ReferencebatchNumber2 as SerialNo, a.ReferenceBatchSequence2 as SequenceNo, b.RefNumberOfPieces2 as GloveQty, b.RefItemNumber2 as ItemNo    
 from DOT_FloorAxIntParentTable a join DOT_FGJournalTable b    
 on a.id = b.ParentRefRecId    
 where a.IsDeleted = 0   
 --a.IsMigratedFromAX6 = 0  
 and (a.IsMigratedFromAX6 = 0 or (a.IsMigratedFromAX6 = 1 and EXISTS(select 1 from DOT_InventBatchSum with (nolock) where BatchNumber = a.BatchNumber and IsMigratedFromAX6 = 1)))  
 and a.id in (' + @ParentIds + ')    
 and a.ReferenceBatchNumber2 is not null    
    
 UNION ALL    
    
 Select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQty, a.PlantNo, b.PostingDateTime, a.FunctionIdentifier,     
 b.InnerLotNumber as LotNo, b.CustomerReference as CustRef, a.PreshipmentCases, b.SalesOrderNumber, b.Quantity as Qty,     
 a.ReferencebatchNumber3 as SerialNo, a.ReferenceBatchSequence3 as SequenceNo, b.RefNumberOfPieces3 as GloveQty, b.RefItemNumber3 as ItemNo    
 from DOT_FloorAxIntParentTable a join DOT_FGJournalTable b    
 on a.id = b.ParentRefRecId    
 where a.IsDeleted = 0   
 --and a.IsMigratedFromAX6 = 0  
 and (a.IsMigratedFromAX6 = 0 or (a.IsMigratedFromAX6 = 1 and EXISTS(select 1 from DOT_InventBatchSum with (nolock) where BatchNumber = a.BatchNumber and IsMigratedFromAX6 = 1)))   and a.id in (' + @ParentIds + ')    
 and a.ReferenceBatchNumber3 is not null    
    
 UNION ALL    
    
 Select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQty, a.PlantNo, b.PostingDateTime, a.FunctionIdentifier,     
 b.InnerLotNumber as LotNo, b.CustomerReference as CustRef, a.PreshipmentCases, b.SalesOrderNumber, b.Quantity as Qty,     
 a.ReferencebatchNumber4 as SerialNo, a.ReferenceBatchSequence4 as SequenceNo, b.RefNumberOfPieces4 as GloveQty, b.RefItemNumber4 as ItemNo    
 from DOT_FloorAxIntParentTable a join DOT_FGJournalTable b    
 on a.id = b.ParentRefRecId    
 where a.IsDeleted = 0   
 --and a.IsMigratedFromAX6 = 0  
 and (a.IsMigratedFromAX6 = 0 or (a.IsMigratedFromAX6 = 1 and EXISTS(select 1 from DOT_InventBatchSum with (nolock) where BatchNumber = a.BatchNumber and IsMigratedFromAX6 = 1)))  
 and a.id in (' + @ParentIds + ')    
 and a.ReferenceBatchNumber4 is not null    
    
 UNION ALL    
    
 Select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQty, a.PlantNo, b.PostingDateTime, a.FunctionIdentifier,     
 b.InnerLotNumber as LotNo, b.CustomerReference as CustRef, a.PreshipmentCases, b.SalesOrderNumber, b.Quantity as Qty,     
 a.ReferencebatchNumber5 as SerialNo, a.ReferenceBatchSequence5 as SequenceNo, b.RefNumberOfPieces5 as GloveQty, b.RefItemNumber5 as ItemNo    
 from DOT_FloorAxIntParentTable a join DOT_FGJournalTable b    
 on a.id = b.ParentRefRecId    
 where a.IsDeleted = 0   
 --and a.IsMigratedFromAX6 = 0  
 and (a.IsMigratedFromAX6 = 0 or (a.IsMigratedFromAX6 = 1 and EXISTS(select 1 from DOT_InventBatchSum with (nolock) where BatchNumber = a.BatchNumber and IsMigratedFromAX6 = 1)))  
 and a.id in (' + @ParentIds + ')    
 and a.ReferenceBatchNumber5 is not null    
 ) a    
    
 select a.ParentId, a.BatchOrderNumber, a.SampleQty, a.PlantNo, a.SequenceNo, a.SerialNo, a.Config,      
 CAST(a.Qty as DECIMAL) as Qty, CAST(a.GloveQty as DECIMAL) as GloveQty,a.PostingDateTime,      
 a.ItemNo, a.LotNo, a.CustRef, CAST(a.PreshipmentCases as DECIMAL) as PreshipmentCases,     
 a.FunctionIdentifier, d.Id as FGSumId ,CAST(d.BaseQuantity as DECIMAL) as BaseQuantity     
 into #tempSMBP from #tempfg a    
 join DOT_FGSumTable d with(nolock) on d.SalesOrderNumber = a.SalesOrderNumber     
 where d.DetailStagingJSON = ''' + @ParentIds + '''    
    
 select aa.*, ISNULL(Max(b.Location), Max(c.Location)) as Location, a.D365BatchNumber     
 from #tempSMBP aa with (NOLOCK)    
 join DOT_FloorAxIntParentTable a with (NOLOCK) on a.BatchNumber = aa.SerialNo    
 left join DOT_RafStgTable b with (nolock) on b.ParentRefRecId = a.id    
 left join DOT_TransferJournal c with (nolock) on c.ParentRefRecId = a.id    
 where a.BatchNumber = aa.SerialNo and a.Sequence < aa.SequenceNo    
 and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0    
 and b.Location is not null or c.Location is not null    
 group by aa.ParentId, aa.BatchOrderNumber, aa.Config, aa.SampleQty, aa.PlantNo, aa.SequenceNo, aa.SerialNo,     
 aa.BaseQuantity, aa.Qty, aa.GloveQty, aa.PostingDateTime, aa.ItemNo, aa.LotNo, aa.CustRef, aa.PreshipmentCases,     
 aa.FunctionIdentifier, aa.FGSumId, a.D365BatchNumber     
 '     
END    
 EXEC (@sql)      
END
GO
PRINT N'Creating Procedure [dbo].[Usp_DOT_HBC_Print_Save]...';


GO
-- ==================================================================================  
-- Name:   Usp_DOT_HBC_Print_Save
-- Purpose:  Printing Of Batchcards  
-- ==================================================================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   -----------------------------------------------------------------  
-- 26/04/2018  Azrul Amin    SP created.  
-- ==================================================================================  
CREATE PROCEDURE [dbo].[Usp_DOT_HBC_Print_Save]  
(  
@UserId VARCHAR(10),
@ShiftId VARCHAR(10),
@OutputTime DATETIME,
@LineId VARCHAR(10),
@BatchCardDate DATETIME,
@ModuleId INT,
@SubModuleID INT,
@SiteNumber INT,
@WorkStationNumber INT,
@ResourceId VARCHAR(50),
@PackingSize VARCHAR(50),
@InnerBox VARCHAR(50)
)   
AS  
BEGIN  
  
DECLARE @LocationId INT  
  
DECLARE @TbResourceId TABLE(ResourceId INT)

DECLARE @GloveQtyToTable TABLE(ResourceId INT, PackingSize INT, InnerBox INT)
  
DECLARE @TbProductionOrderStaging TABLE(LineId NVARCHAR(5),TiersideLetter1 NVARCHAR(1),TiersideLetter2 NVARCHAR(1),GloveType NVARCHAR(50),GloveSize NVARCHAR(50),ResourceId NVARCHAR(50),BthOrderId NVARCHAR(50))

DECLARE @TbStaging TABLE(OutputTime DateTime,LineId NVARCHAR(5),TiersideLetter1 NVARCHAR(1),TiersideLetter2 NVARCHAR(1),GloveType NVARCHAR(50),GloveSize NVARCHAR(50),ResourceId NVARCHAR(50),BthOrderId NVARCHAR(50),Resource NVARCHAR(50),PackingSize INT,InnerBox INT)  
  
DECLARE @TbProductionOrder TABLE(LineId NVARCHAR(5),Tierside NVARCHAR(5),GloveType NVARCHAR(50),GloveSize nvarchar(50),BthOrderId NVARCHAR(50))
  
DECLARE @TbBatch  TABLE(BatchNumber VARCHAR(150),SerialNumber VARCHAR(20),ShiftId INT,TierSide VARCHAR(10),LineId NVARCHAR(5),  
GloveType VARCHAR(50),GloveSize VARCHAR(50),BatchCardDate DATETIME,IsOnline Bit, LastModifiedOn DATETIME,LocationId INT,WorkStationNumber INT ,  
ModuleName INT,SubModuleName INT,BatchType nchar(10),ReWorkCount INT,TotalPCs INT,batchcardcurrentlocation VARCHAR(20), ResourceId INT, BthOrderId VARCHAR(20))   
  
SELECT @LocationId =locationid from workstationmaster with (nolock) where isdeleted=0 and workstationid= @WorkStationNumber  

DECLARE @ServerCurrentTime DATETIME = GETDATE()
  
BEGIN TRANSACTION;  
  
BEGIN TRY  
SET NOCOUNT ON

  INSERT INTO @TbResourceId
  select * from dbo.SplitString(@ResourceId,',')

  INSERT INTO @GloveQtyToTable
  SELECT * FROM dbo.Ufn_DOT_GloveQtyToTable(@ResourceId,@PackingSize,@InnerBox)

  -- If Printed by Former Tier not selected, Generate Batchcard based on below logic in code LT, LB same glove code, size -1 Batch Card  
  -- Insert into production line staging table for IsPrintByFormer=0  
  INSERT INTO @TbProductionOrderStaging  
  SELECT LineId,left(TierSide,1) AS tier1,RIGHT(TierSide,1) AS tier2,GloveType,string,vg.ResourceId,BthOrderId FROM View_DOT_BOGloveTypeSize vg CROSS APPLY   
  dbo.ufn_CSVToTable(vg.GloveSize) JOIN @GloveQtyToTable AS pk on pk.ResourceId = vg.ResourceId
  WHERE vg.IsAlternate=0 AND ISNULL(vg.IsPrintByFormer,0)=0 AND vg.IsOnline=1 AND LineId=@LineId
  AND vg.ResourceId in (SELECT * from @TbResourceId)

  --Insert into production line staging table for batch details
  INSERT INTO @TbStaging  
  SELECT @OutputTime,stg.LineId,stg.TiersideLetter1,stg.TiersideLetter2,stg.GloveType,stg.GloveSize,stg.ResourceId,stg.BthOrderId,dbo.Ufn_DOT_GetResource(stg.ResourceId),
  CONVERT(INT,qty.PackingSize),CONVERT(INT,qty.InnerBox) 
  FROM @TbProductionOrderStaging stg LEFT JOIN @GloveQtyToTable qty ON qty.ResourceId = stg.ResourceId

  --Tier side should be L/R not LT/LB or RB/RT for the combined size of LT/LB and RT/RB where Combined records are >1   
  IF ((SELECT COUNT(*) from @TbProductionOrderStaging) = 4)
  BEGIN
	  INSERT INTO @TbProductionOrder(LineId,Tierside,GloveType,GloveSize,BthOrderId)   
	  SELECT TOP 1 LineId,'LR',GloveType,GloveSize,BthOrderId FROM   
	  @TbProductionOrderStaging GROUP BY LineId,TiersideLetter1,GloveType,GloveSize,BthOrderId  
	  HAVING count(GloveSize) > 1  
  END
  ELSE
  BEGIN
	  INSERT INTO @TbProductionOrder(LineId,Tierside,GloveType,GloveSize,BthOrderId)   
	  SELECT LineId,TiersideLetter1,GloveType,GloveSize,BthOrderId FROM   
	  @TbProductionOrderStaging GROUP BY LineId,TiersideLetter1,GloveType,GloveSize,BthOrderId  
	  HAVING count(GloveSize) > 1  
  END

  -- deleting from staging table where Combined records are >1 after deleting we have only non combined which can be inserted with Full tier side like LT/LB or RT/RB  
  DELETE tr FROM @TbProductionOrderStaging tr  JOIN  @TbProductionOrder pl  
  ON tr.LineId=pl.LineId AND tr.GloveType=pl.GloveType AND tr.GloveSize=pl.GloveSize AND tr.TiersideLetter1= pl.Tierside AND tr.BthOrderId=pl.BthOrderId
   
  -- inserting non combined with fill tierside name like LT/LB or RT/RB  
  IF ((SELECT COUNT(*) from @TbProductionOrderStaging) < 4)
  BEGIN
	  INSERT INTO @TbProductionOrder(LineId,Tierside,GloveType,GloveSize,BthOrderId)   
	  SELECT LineId,TiersideLetter1+TiersideLetter2,GloveType,GloveSize,BthOrderId FROM   
	  @TbProductionOrderStaging 
  END

  INSERT INTO @TbProductionOrder(LineId,Tierside,GloveType,GloveSize,BthOrderId)   
  SELECT DISTINCT LineId,TierSide,GloveType,string,BthOrderId  FROM View_DOT_BOGloveTypeSize vg CROSS APPLY   
  dbo.ufn_CSVToTable(vg.GloveSize)  WHERE vg.IsAlternate=0 AND ISNULL(vg.IsPrintByFormer,0)=1  AND vg.IsOnline=1   
  AND LineId=@LineId
  
  INSERT INTO  @TbBatch (BatchNumber,SerialNumber,ShiftId,TierSide,LineId,GloveType,GloveSize,BatchCardDate,LastModifiedOn,BatchType,LocationId,WorkStationNumber,BthOrderId)  
  SELECT dbo.Ufn_BatchNumber(@OutputTime,vw.LineId,GloveSize) AS BatchNumber,    
  dbo.Ufn_SerailNumberPart(@SiteNumber,@BatchCardDate) + dbo.Ufn_IntToChar((NEXT VALUE FOR DBO.SerialNumberSeq),7)  AS SerialNumber,  
  @ShiftId,TierSide,vw.LineId,GloveType,GloveSize,@BatchCardDate,@BatchCardDate,'T' As BatchType,@LocationId AS LocationId, @WorkStationNumber AS WorkStationNumber,BthOrderId
  FROM @TbProductionOrder vw WHERE vw.LineId=@LineId 
    
  UPDATE @TbBatch SET ModuleName= null,SubModuleName=null,IsOnline=1,ReWorkCount=0,TotalPCs=0, batchcarddate = DATEADD(HOUR, DATEDIFF(HOUR, 0, DATEADD(mi, 30, batchcarddate)), 0),batchcardcurrentlocation='PN'  
  
  --insert into batch table
  INSERT INTO dbo.Batch(BatchNumber,SerialNumber,ShiftId,TierSide,LineId,GloveType,Size,BatchCardDate,IsOnline,   
  LastModifiedOn,LocationId,WorkstationId,ModuleId,SubModuleID,BatchType,ReWorkCount,TotalPCs,batchcardcurrentlocation)     
  SELECT BatchNumber,SerialNumber,@ShiftId,TierSide,LineId,GloveType,GloveSize,@OutputTime,IsOnline,@ServerCurrentTime,LocationId,WorkStationNumber,@ModuleId,@SubModuleID,
  BatchType,ReWorkCount,TotalPCs,batchcardcurrentlocation FROM @TbBatch   

  --insert batch details into staging table
  INSERT INTO dbo.DOT_FloorD365HRGLOVERPT(SeqNo,BatchCardNumber,BthOrder,CreationTime,CreatorUserId,CurrentDateandTime,DeleterUserId,DeletionTime,
  GloveCategory,GloveCode,IsDeleted,LastModificationTime,LastModifierUserId,LineId,OutTime,Plant,Resource,SerialNo,ShiftId,Size,UserID,PackingSz,InBox)
  SELECT ROW_NUMBER() OVER (PARTITION BY bat.SerialNumber ORDER BY stg.BthOrderId) AS SeqNo,bat.BatchNumber,stg.BthOrderId,@ServerCurrentTime,1,@ServerCurrentTime,0,NULL,dbo.Ufn_DOT_GetGloveCategory(stg.GloveType),stg.GloveType,0,GETDATE(),NULL,stg.LineId,  
  @OutputTime,dbo.Ufn_DOT_GetLocationName(@LocationId),dbo.Ufn_DOT_GetResource(stg.ResourceId),bat.SerialNumber,@ShiftId,stg.GloveSize,@UserId,qty.PackingSize,qty.InnerBox
  FROM @TbStaging stg LEFT JOIN @TbBatch bat ON  stg.BthOrderId = bat.BthOrderId --and stg.TiersideLetter1 = left(bat.tierside,1) 
  LEFT JOIN @GloveQtyToTable qty ON qty.ResourceId = stg.ResourceId

  IF ((SELECT COUNT (DISTINCT PackingSize) FROM @TbStaging) = 1)
  BEGIN	  
	  --select result for print batch card if same PackingSize
	  SELECT stg.OutputTime as OutputTime,bat.SerialNumber,bat.BatchNumber,bat.GloveType, bat.GloveSize AS Size,bat.BatchCardDate,
	  STUFF((SELECT ', ' + r.Resource FROM @TbStaging r 
			WHERE r.BthOrderId = stg.BthOrderId --and r.TiersideLetter1 = stg.TiersideLetter1
			FOR XML path('') ), 1, 2, '') AS Resource,
	  stg.PackingSize,
	  sum(stg.InnerBox) as InnerBox,
	  REPLACE(CONVERT(VARCHAR,CONVERT(Money, SUM(stg.PackingSize*stg.InnerBox)),1),'.00','') as TotalGloveQty 
	  FROM @TbStaging stg LEFT JOIN @TbBatch bat ON bat.BthOrderId = stg.BthOrderId --and stg.TiersideLetter1 = left(bat.tierside,1)
	  GROUP BY stg.PackingSize,stg.OutputTime,bat.SerialNumber,bat.BatchNumber,bat.GloveType,bat.GloveSize,bat.BatchCardDate,stg.BthOrderId--,stg.TiersideLetter1
  END
  ELSE
  BEGIN
	  --select result for print batch card if diff PackingSize
	  SELECT stg.OutputTime as OutputTime,bat.SerialNumber,bat.BatchNumber,bat.GloveType, bat.GloveSize AS Size,bat.BatchCardDate,
	  STUFF((SELECT ', ' + r.Resource FROM @TbStaging r 
			WHERE r.BthOrderId = stg.BthOrderId-- and r.TiersideLetter1 = stg.TiersideLetter1
			FOR XML path('') ), 1, 2, '') AS Resource,
	  STUFF((SELECT ',' + CONVERT(NVARCHAR,p.PackingSize) FROM @TbStaging p 
			WHERE p.BthOrderId = stg.BthOrderId --and p.TiersideLetter1 = stg.TiersideLetter1
			FOR XML path('') ), 1, 1, '') AS PackingSize,
	  STUFF((SELECT ',' + CONVERT(NVARCHAR,i.InnerBox) FROM @TbStaging i 
			WHERE i.BthOrderId = stg.BthOrderId --and i.TiersideLetter1 = stg.TiersideLetter1
			FOR XML path('') ), 1, 1, '') AS InnerBox,
	  REPLACE(CONVERT(VARCHAR,CONVERT(Money, SUM(stg.PackingSize*stg.InnerBox)),1),'.00','') as TotalGloveQty 
	  FROM @TbStaging stg LEFT JOIN @TbBatch bat ON bat.BthOrderId = stg.BthOrderId --and stg.TiersideLetter1 = left(bat.tierside,1) 
	  GROUP BY stg.OutputTime,bat.SerialNumber,bat.BatchNumber,bat.GloveType,bat.GloveSize,bat.BatchCardDate,stg.BthOrderId--,stg.TiersideLetter1
  END

 SET NOCOUNT OFF  
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
PRINT N'Creating Procedure [dbo].[USP_DOT_HBCSummaryBatchCardReport]...';


GO
-- ==================================================================================================================================================    
-- Name: USP_DOT_HBCSummaryBatchCardReport  
-- Purpose: Open Batch Card Summarization Report For Online/Surgical Batch  
-- ==================================================================================================================================================    
-- Change History    
-- Date			Author   Comments    
-- -----		------   ------------------------------------------------------------  
-- 2021/10/26   Azrul    SP created.    
-- ==================================================================================================================================================   
-- exec USP_DOT_HBCSummaryBatchCardReport '2210605283,2210605145,2210605001,2210605292,2210605426', '', '', '2021-01-01','2021-10-01'
-- exec USP_DOT_HBCSummaryBatchCardReport '2210605283,2210605145,2210605001,2210605292,2210605426', '', '', '',''
-- exec USP_DOT_HBCSummaryBatchCardReport '', '', 'ANSL 301/20', '',''
-- exec USP_DOT_HBCSummaryBatchCardReport '', '', 'MEDU 806/21', '',''
-- exec USP_DOT_HBCSummaryBatchCardReport '', '', '', '2021-08-01','2021-08-31'
-- ==================================================================================================================================================

Create   PROCEDURE USP_DOT_HBCSummaryBatchCardReport  
(
@SerialNumber NVARCHAR(4000),
@D365BatchNumber NVARCHAR(4000),
@CustReferenceNumber NVARCHAR(4000),
@PostingDateTimeFrom NVARCHAR(100),
@PostingDateTimeTo NVARCHAR(100)
)

AS
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;
 
IF OBJECT_ID('tempdb..#SerialNumbers') IS NOT NULL  
DROP TABLE #SerialNumbers 
IF OBJECT_ID('tempdb..#D365BatchNumbers') IS NOT NULL  
DROP TABLE #D365BatchNumbers 
IF OBJECT_ID('tempdb..#CustReferenceNumbers') IS NOT NULL  
DROP TABLE #CustReferenceNumbers

CREATE TABLE #SerialNumbers (SerialNumber NVARCHAR(4000))
CREATE TABLE #D365BatchNumbers (D365BatchNumber NVARCHAR(4000))
CREATE TABLE #CustReferenceNumbers (CustReferenceNumber NVARCHAR(4000))

IF (LEN(@SerialNumber) > 0) 
BEGIN
	INSERT INTO #SerialNumbers 
	select * from dbo.SplitString(NULLIF(@SerialNumber, ','), ',')
END
ELSE IF (LEN(@D365BatchNumber) > 0) 
BEGIN
	INSERT INTO #D365BatchNumbers 
	select * from dbo.SplitString(NULLIF(@D365BatchNumber, ''), ',')

	INSERT INTO #SerialNumbers
	select BatchNumber from DOT_FloorAxIntParentTable with(nolock) 
	where D365BatchNumber in (select D365BatchNumber from #D365BatchNumbers where D365BatchNumber <> '')
	and IsDeleted = 0  and IsMigratedFromAX6 = 0   
END
Else IF (LEN(@CustReferenceNumber) > 0) 
BEGIN
	INSERT INTO #CustReferenceNumbers 
	select * from dbo.SplitString(NULLIF(@CustReferenceNumber, ','), ',')

	INSERT INTO #SerialNumbers
	select BatchNumber from dbo.UFN_DOT_GetSNFromCustRefForSummaryBatchCardReport(@CustReferenceNumber)
END

DECLARE @StartPostingDateTime DateTime = '2019-03-01'
DECLARE @EndPostingDateTime DateTime = DATEADD(DAY,1,GetDate())
DECLARE @SerialNoCount int
SELECT @SerialNoCount = Count(1) from #SerialNumbers

--details: online/surgical batch only, online 2G and offline batch excluded
IF (@SerialNoCount > 0)
BEGIN
	select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
	ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
	ISNULL(NULLIF(b.Warehouse,''),a.PlantNo+'-PROD') as Warehouse, b.Location, b.RAFGoodQty as TransitionQuantity, NULL as ReservedQuantity, 
	a.Sequence, a.FunctionIdentifier
	from DOT_FloorAxIntParentTable a with(nolock) join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
	where a.FunctionIdentifier in ('HBC','SRBC')
	and a.BatchNumber IN (Select Distinct SerialNumber from #SerialNumbers where SerialNumber <> '')
	and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 
	and b.PostingDateTime >= case when LEN(@PostingDateTimeFrom) > 0 then @PostingDateTimeFrom else @StartPostingDateTime end
	and b.PostingDateTime <= case when LEN(@PostingDateTimeTo) > 0 then @PostingDateTimeTo  else @EndPostingDateTime end
END
ELSE
BEGIN
	select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
	ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
	ISNULL(NULLIF(b.Warehouse,''),a.PlantNo+'-PROD') as Warehouse, b.Location, b.RAFGoodQty as TransitionQuantity, NULL as ReservedQuantity, 
	a.Sequence, a.FunctionIdentifier
	from DOT_FloorAxIntParentTable a with(nolock) join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
	where a.FunctionIdentifier in ('HBC','SRBC')
	and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 
	and b.PostingDateTime >= case when LEN(@PostingDateTimeFrom) > 0 then @PostingDateTimeFrom else @StartPostingDateTime end
	and b.PostingDateTime <= case when LEN(@PostingDateTimeTo) > 0 then @PostingDateTimeTo  else @EndPostingDateTime end
END

IF OBJECT_ID('tempdb..#SerialNumbers') IS NOT NULL  
DROP TABLE #SerialNumbers 
IF OBJECT_ID('tempdb..#D365BatchNumbers') IS NOT NULL  
DROP TABLE #D365BatchNumbers 
IF OBJECT_ID('tempdb..#CustReferenceNumbers') IS NOT NULL  
DROP TABLE #CustReferenceNumbers

END

--not use join #SerialNumbers for better performance
--select a.* from DOT_FloorAxIntParentTable a with(nolock) join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
--left join #SerialNumbers c on a.BatchNumber = c.SerialNumber --3 mins
----where a.BatchNumber in (select SerialNumber from #SerialNumbers)  --0 sec
--and a.FunctionIdentifier in ('HBC','SRBC')
--and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_Inventory360PostPalletToD365]...';


GO

-- =====================================================      
-- Author:  <Max He>        
-- Create date: <25-11-2021>        
-- Description: <USP_DOT_Inventory360PostPalletToD365>
-- exec USP_DOT_Inventory360PostPalletToD365 'N1000152','P7','2020-11-15 18:48:17.017',1
-- =====================================================      
Create PROCEDURE [dbo].[USP_DOT_Inventory360PostPalletToD365]
    @PalletID VARCHAR(50),        
    @PlantNo VARCHAR (5),        
    @DateScanned DATETIME,  
	@Return int output        
AS        
BEGIN        
SET @Return=0;  
BEGIN TRANSACTION;        
  BEGIN TRY        
    SET NOCOUNT ON;    
    DECLARE @InternalLotNumber NVARCHAR(30)        
	DECLARE @DateStockOut DATETIME        
	DECLARE @Size NVARCHAR(20)    
    /** DOT_FLOORAXINTPARENTTABLE parameter **/        
    DECLARE @BatchCardNumber NVARCHAR(50)        
    DECLARE @BatchNumber NVARCHAR(20)        
    DECLARE @FSIdentifier UNIQUEIDENTIFIER        
    DECLARE @FunctionIdentifier NVARCHAR(20)        
    DECLARE @ReferenceBatchNumber1 NVARCHAR(20) = NULL        
    DECLARE @ReferenceBatchNumber2 NVARCHAR(20) = NULL        
    DECLARE @ReferenceBatchNumber3 NVARCHAR(20) = NULL        
    DECLARE @ReferenceBatchNumber4 NVARCHAR(20) = NULL        
    DECLARE @ReferenceBatchNumber5 NVARCHAR(20) = NULL        
    DECLARE @ReferenceBatchSequence1 INT = 0        
    DECLARE @ReferenceBatchSequence2 INT = 0        
    DECLARE @ReferenceBatchSequence3 INT = 0        
    DECLARE @ReferenceBatchSequence4 INT = 0        
    DECLARE @ReferenceBatchSequence5 INT = 0        
    DECLARE @Sequence INT        
    /** DOT_FGJournalTable parameter **/        
    DECLARE @BatchOrderNumber NVARCHAR(80)        
    DECLARE @ReferenceItemNumber NVARCHAR (40)        
    DECLARE @Configuration NVARCHAR(20)        
    DECLARE @Warehouse NVARCHAR (5) = 'FG'        
    DECLARE @Resource NVARCHAR(20)        
    DECLARE @CustomerPO NVARCHAR(40)        
    DECLARE @CustomerReference NVARCHAR(60)      
    DECLARE @SalesOrdernumber NVARCHAR(20)        
    DECLARE @InnerLotNumber NVARCHAR(90)        
    DECLARE @OuterLotNumber NVARCHAR(90)        
    DECLARE @CustomerLotNumber NVARCHAR(90)        
    DECLARE @Preshipment INT        
    DECLARE @Preshipmentcases INT        
    DECLARE @InnerBoxCapacity INT        
    DECLARE @PostingDateTime DATETIME           
	DECLARE @EWNQty INT   --#EWN qty validation with staging  
	DECLARE @AccQty INT = 0  --#EWN qty validation with staging  
    DECLARE @Quantity INT     
    DECLARE @TotalQuantity DECIMAL         --splitBatch  
    DECLARE @ParentRefRecId INT          
    DECLARE @Location NVARCHAR(20) = ''        
    DECLARE @PalletNumber NVARCHAR(20)        
    DECLARE @ExpiryDate DATETIME        
    DECLARE @Manufacturingdate DATETIME        
    DECLARE @IsWTS BIT        
    DECLARE @ItemNumber NVARCHAR (40) = NULL        
    DECLARE @CreateDateTime DATETIME        
    DECLARE @PONumber NVARCHAR(20)        
    DECLARE @PreshipmentCaseCount INT        
    DECLARE @RefNumberOfPieces1 DECIMAL (10, 2) = 0 --For SMBP        
    DECLARE @RefNumberOfPieces2 DECIMAL (10, 2) = 0 --For SMBP        
    DECLARE @RefNumberOfPieces3 DECIMAL (10, 2) = 0 --For SMBP        
    DECLARE @RefNumberOfPieces4 DECIMAL (10, 2) = 0 --For SMBP        
    DECLARE @RefNumberOfPieces5 DECIMAL (10, 2) = 0 --For SMBP        
    DECLARE @RefItemNumber1 NVARCHAR (40) = NULL --For SMBP        
    DECLARE @RefItemNumber2 NVARCHAR (40) = NULL --For SMBP        
    DECLARE @RefItemNumber3 NVARCHAR (40) = NULL --For SMBP        
    DECLARE @RefItemNumber4 NVARCHAR (40) = NULL --For SMBP        
    DECLARE @RefItemNumber5 NVARCHAR (40) = NULL --For SMBP        
    /** AXPostingLog parameter **/        
    DECLARE @PostingType NVARCHAR(20) = 'DOTFGJournalContract'        
    DECLARE @SerialNumber NVARCHAR(50)        
    DECLARE @IsPostedToAX BIT = 1        
    DECLARE @IsPostedInAX BIT = 1        
    DECLARE @ExceptionCode NVARCHAR(1000) = NULL        
    DECLARE @TransactionID NVARCHAR(100) = '-1'        
    DECLARE @Area NVARCHAR(10) = 'PS'        
    DECLARE @PalletSerialNo  UNIQUEIDENTIFIER    
    DECLARE @MaxLotCaseNumber int -- max caseNumber by internalLotNo  
    DECLARE @SPPBatchCardNumber NVARCHAR(50) = NULL --Surgical Packing Plan
    DECLARE @SPPBatchNumber NVARCHAR(50) = NULL		--Surgical Packing Plan
    DECLARE @PickingListQuantity INT = 0			--Surgical Packing Plan
    DECLARE @BatchSequence INT = 0					--Surgical Packing Plan
    DECLARE @GloveSize NVARCHAR(20) = NULL			--Surgical Packing Plan
    DECLARE @SumGloveSampleQuantity INT = 0			--Surgical Packing Plan
    DECLARE @GloveSampleQuantity INT = 0			--Surgical Packing Plan
    DECLARE @IsConsolidated BIT = 0					-- #AZRUL 17/9/2021: Open batch flag for NGC1.5
    DECLARE @IsConsolidatedMultipleBatch BIT = 0	-- #Max 27/10/2021: Detail batch check for Open batch flag for NGC1.5
  
 set @PalletSerialNo = NEWID(); -- Generate by pallet    
 if isnull(@PalletID,'')=''      
 BEGIN      
   RAISERROR ('Pallet ID is empty.', -- Message text.        
               16, -- Severity.        
               1 -- State.        
               );        
 END       
    
 if not exists(select 1 from palletmaster WITH (NOLOCK) where PalletId = @PalletID)    
  BEGIN      
   RAISERROR ('Pallet ID is not exists.', -- Message text.        
               16, -- Severity.        
               1 -- State.        
               );        
 END     
    
 /** Set additional parameter for filtering**/    
 Select @PONumber = PONumber FROM PalletMaster WITH (NOLOCK) where PalletId = @PalletID  
 print @ponumber  
 select @ItemNumber = SUBSTRING(Item,0,CHARINDEX('_',Item,0)),                         
     @Size =  SUBSTRING(Item, CHARINDEX('_', Item) + 1, LEN(Item))
 from EWN_CompletedPallet WITH (NOLOCK) WHERE PalletId = @PalletID and DateStockOut is null and PONumber = @PONumber  
 select @EWNQty = Sum(Qty) from EWN_CompletedPallet WITH (NOLOCK) WHERE PalletId = @PalletID and DateStockOut is null and PONumber = @PONumber --#EWN qty validation with staging
 select top 1 @Preshipment = isPreshipmentCase from PurchaseOrderItemCases WITH (NOLOCK) where palletid = @PalletID and ItemNumber = @ItemNumber and PONumber = @PONumber and size = @Size       
 SELECT @DateStockOut = ISNULL(MAX(DateStockOut),GETDATE()) FROM EWN_CompletedPallet WITH (NOLOCK) WHERE DateStockOut IS NOT NULL AND PalletId = @PalletID   and  PONumber = @PONumber       
        
    /** List all InternalLotNumber based on parameter when trigger executed **/        
 IF @Preshipment = 0 or EXISTS(select 1 FROM DOT_FSItemMaster WITH (NOLOCK) WHERE ItemId=@ItemNumber AND ItemType=8) --bypass for SPPBC
 BEGIN  
  SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS Id, fp.InternalotNumber as InternalLotNumber      
  INTO #temptable       
  FROM PurchaseOrderItemCases fp WITH (NOLOCK)       
  JOIN EWN_CompletedPallet ew WITH (NOLOCK) ON fp.PalletId = ew.PalletId AND fp.PONumber = @PONumber        
  WHERE fp.PalletId = @PalletID AND ew.DateCompleted < @DateScanned /*AND ew.DateCompleted> DATEADD(DAY, -30, @DateStockOut)*/ AND ew.DateStockOut IS NULL  --#AZ 12/2/2019 filtering based on PONumber from palletMaster not time duration  
  and fp.size = @Size      
  and fp.InternalotNumber not in (select InnerLotNumber from DOT_FGJournalTable WITH (NOLOCK) where Preshipment = 0 and PalletNumber = @PalletID) --#MH 17/8/2018 filter out by staging record      
  and fp.InternalotNumber in (select InternalotNumber from PurchaseOrderItemCases WITH (NOLOCK) where PalletId = @PalletID and size = @Size and itemnumber = @ItemNumber) --filter out InternalotNumber not exists in PurchaseOrderItemCases    
  group by fp.InternalotNumber      
  END    
 ELSE    
 BEGIN    
  SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS Id, fp.InternaLotNumber as InternalLotNumber  
  INTO #temptablePSI    
  FROM PurchaseOrderItemCases fp WITH (NOLOCK)       
  JOIN EWN_CompletedPallet ew WITH (NOLOCK) ON fp.PalletId = ew.PalletId AND fp.PONumber = @PONumber        
  and fp.Size = SUBSTRING(ew.Item, CHARINDEX('_', ew.Item) + 1, LEN(ew.Item))         --#AZ 7/9/2018 PSI can have multiple size in 1 pallet    
  WHERE fp.PalletId = @PalletID AND ew.DateStockOut IS NULL     
  AND ew.DateCompleted < @DateScanned -- AND ew.DateCompleted > DATEADD(DAY, -2, @DateStockOut)    --#AZ 12/2/2019 filtering based on PONumber from palletMaster not time duration  
  --and fp.size = @Size                      --#AZ 7/9/2018 PSI can have multiple size in 1 pallet    
  and fp.InternalotNumber not in (select InnerLotNumber from DOT_FGJournalTable WITH (NOLOCK) where Preshipment = 1 and PalletNumber = @PalletID) --#MH 17/8/2018 filter out by staging record    
  and isPreshipmentCase > 0                    --#MH 29/01/2019 PSI can have multiple size in 1 pallet    
  and fp.InternaLotNumber in (select InternalotNumber from PurchaseOrderItemCases WITH (NOLOCK) where PalletId = @PalletID /*and size = @Size*/ and itemnumber = @ItemNumber) --filter out InternalotNumber not exists in PurchaseOrderItemCases               
           
  group by fp.InternalotNumber;  
  
 -- To detect is the last Preshipment case  
 select count(itemid) as SOLineCount,HartalegaCommonSize,ItemId  
 into #tempSOLineCount  
 from DOT_FloorSalesLine with(nolock)   
 where IsDeleted=0 and salesid = @PONumber  
 group by HartalegaCommonSize,ItemId;  
  
 select count(ItemNumber) as POLineCount,ItemSize,ItemNumber  
 into #tempPOlineCount  
 from purchaseorderitem with(nolock)  
 where POnumber = @PONumber  
 group by ItemSize,ItemNumber  
  
   
 select PONumber,size, itemNumber,count(1) Qty,max(CaseNumber) as MaxCaseNumber  
 INTO #tempPalletMaxCase  
 from PurchaseOrderItemCases with(nolock)  
 where PONumber=@PONumber and ispreshipmentCase = 1  
 group by PONumber,size, itemNumber;  
  
 --select * from #tempPalletMaxCase  
 END    
       
    /** Looping each InternalLotNumber for posting **/        
 DECLARE @COUNT INT    
 IF @Preshipment = 0 or EXISTS(select 1 FROM DOT_FSItemMaster WITH (NOLOCK) WHERE ItemId=@ItemNumber AND ItemType=8) --bypass for SPPBC
 BEGIN  
  SET @COUNT = (SELECT MAX(Id) FROM #temptable);        
 END    
 ELSE    
 BEGIN    
  SET @COUNT = (SELECT MAX(Id) FROM #temptablePSI);        
 END    
    
DECLARE @ROW INT = 1;        
WHILE (@ROW <= @COUNT)        
BEGIN    
      
  --print @ROW      
  --clean up    
  if OBJECT_ID('tempdb.dbo.#finalpackingtemp') IS NOT NULL       
  BEGIN      
   DROP TABLE #finalpackingtemp;        
  --print 'drop #finalpackingtemp'      
  END      
  if OBJECT_ID('tempdb.dbo.#finalpackingSMBPtemp') IS NOT NULL       
  BEGIN      
   DROP TABLE #finalpackingSMBPtemp;        
  --print 'drop #finalpackingSMBPtemp'      
  END      
  if OBJECT_ID('tempdb.dbo.#finalpackingSPPBCtemp') IS NOT NULL --Surgical Packing Plan
  BEGIN															--Surgical Packing Plan
   DROP TABLE #finalpackingSPPBCtemp;      						--Surgical Packing Plan
  END  															--Surgical Packing Plan
     --print 'get internal lot number'      

  IF @Preshipment = 0 or EXISTS(select 1 FROM DOT_FSItemMaster WITH (NOLOCK) WHERE ItemId=@ItemNumber AND ItemType=8) --bypass for SPPBC
  BEGIN  
   SET @InternalLotNumber = (SELECT InternalLotNumber FROM #temptable WHERE Id = @ROW);        
  END    
  ELSE    
  BEGIN    
    SET @InternalLotNumber = (SELECT InternalLotNumber FROM #temptablePSI WHERE Id = @ROW);      
  END      
  print @InternalLotNumber      
      
  SELECT * INTO #finalpackingtemp FROM FinalPacking WITH (NOLOCK)       
     WHERE InternalLotNumber = @InternalLotNumber;        
    --print '97'      
      
  set @BatchNumber='0';      
  SELECT @BatchNumber = ISNULL(SerialNumber,'0') FROM #finalpackingtemp;       
         
  --print '99'      
  SET @FSIdentifier = NEWID();        
        
  /** If FunctionID: SBC        
     FloorSystem Method: AXPostingBLL.GetCompleteFinalPackingDetails(internalLotNumber)         
     EXEC USP_FP_Get_ScanBatchCardInnerOuterforPosting @InternalLotNumber **/       
  --print @BatchNumber      
  IF (@BatchNumber <> '0')        
  BEGIN        
  SET @FunctionIdentifier = 'SBC'        
  --print '108'      
  --print @FunctionIdentifier     
  IF @Preshipment = 0    
  BEGIN     
   SELECT DISTINCT @BatchNumber = FPB.SerialNumber,        
     @BatchCardNumber = BT.BatchNumber,        
     @BatchOrderNumber = FP.FGBatchOrderNo,        
     @Configuration = POIN.ItemSize,        
     @Resource = FP.Resource,  
     @CustomerPO = POIN.OrderNumber,        
     @CustomerReference = POIN.CustomerReferenceNumber,        
     @InnerLotNumber = FP.InternalLotNumber,        
     @OuterLotNumber = FP.OuterLotno,        
     @CustomerLotNumber = POIN.CustomerLotNumber,        
     @Manufacturingdate = FP.ManufacturingDate,        
     @ExpiryDate = FP.ExpiryDate,        
     @PalletNumber = @PalletID,        
     @CreateDateTime = BT.BatchCardDate,        
     @PostingDateTime = @DateScanned,        
     @Quantity = CT.CasesPacked,        
     @ReferenceItemNumber = FP.ItemNumber,        
     @SalesOrdernumber = FP.PONumber,        
     @PONumber = FP.PONumber,        
     @SerialNumber = FPB.SerialNumber,    
     @PreshipmentCases = 0 -- Normal Pallet always set to 0        
   FROM #finalpackingtemp FP        
   JOIN FinalPackingbatchinfo FPB WITH (NOLOCK) ON FPB.InternalLotNumber = FP.InternalLotNumber        
   JOIN PurchaseOrderItem POIN WITH (NOLOCK) ON POIN.PONumber = FP.PONumber AND POIN.ItemNumber = FP.ItemNumber AND FP.Size = POIN.ItemSize        
   JOIN Batch BT WITH (NOLOCK) ON BT.SerialNumber = FP.SerialNumber     
   JOIN (select count(CaseNumber) as CasesPacked,isPreshipmentCase,PONumber,InternalotNumber,palletid from PurchaseOrderItemCases WITH (NOLOCK)     
     where InternalotNumber = @InternalLotNumber and palletid=@PalletID     
     group by isPreshipmentCase,PONumber,InternalotNumber,palletid) CT on ct.InternalotNumber = FP.InternalLotNumber       
   WHERE FP.InternalLotNumber = @InternalLotNumber    
  
  END    
  ELSE  -- else is preshipment pallet  
  BEGIN    
  
  -- Get Max case number by internal lot no  
  select @MaxLotCaseNumber=max(CaseNumber)  
   from PurchaseOrderItemCases  WITH (NOLOCK)  
   where InternalotNumber=@InternalLotNumber and ispreshipmentCase = 1  
   group by InternalotNumber;  
  print @MaxLotCaseNumber    
  
   SELECT DISTINCT @BatchNumber = FPB.SerialNumber,        
     @BatchCardNumber = BT.BatchNumber,           
     @BatchOrderNumber = FP.FGBatchOrderNo,        
     @Configuration = POIN.ItemSize,        
     @Resource = FP.Resource,        
     @CustomerPO = POIN.OrderNumber,        
     @CustomerReference = POIN.CustomerReferenceNumber,        
     @InnerLotNumber = FP.InternalLotNumber,        
     @OuterLotNumber = FP.OuterLotno,        
     @CustomerLotNumber = POIN.CustomerLotNumber,        
     @Manufacturingdate = FP.ManufacturingDate,        
     @ExpiryDate = FP.ExpiryDate,        
     @PalletNumber = @PalletID,        
     @CreateDateTime = BT.BatchCardDate,        
     @PostingDateTime = @DateScanned,        
     @Quantity = CT.CasesPacked,        
     @ReferenceItemNumber = FP.ItemNumber,        
     @SalesOrdernumber = FP.PONumber,        
     @PONumber = FP.PONumber,        
     @SerialNumber = FPB.SerialNumber,    
     @PreshipmentCases = CASE WHEN soc.SOLineCount=poc.POLineCount and isnull(packedCount.SONotPacked,0) = 0 and psiMax.MaxCaseNumber=@MaxLotCaseNumber THEN psiMax.Qty ELSE 0 END     
   FROM #finalpackingtemp FP        
   JOIN FinalPackingbatchinfo FPB WITH (NOLOCK) ON FPB.InternalLotNumber = FP.InternalLotNumber        
   JOIN PurchaseOrderItem POIN WITH (NOLOCK) ON POIN.PONumber = FP.PONumber AND POIN.ItemNumber = FP.ItemNumber AND FP.Size = POIN.ItemSize        
   JOIN Batch BT WITH (NOLOCK) ON BT.SerialNumber = FP.SerialNumber    
   JOIN (select count(CaseNumber) as CasesPacked,InternalotNumber  
     from PurchaseOrderItemCases WITH (NOLOCK)     
     where InternalotNumber = @InternalLotNumber and palletid=@PalletID     
     group by InternalotNumber) CT on ct.InternalotNumber = FP.InternalLotNumber     
    
  -- PSI cases handing logic  
   -- calculate how many PSI packed for this FG & size  
   JOIN #tempPalletMaxCase psiMax on psiMax.ItemNumber=FP.ItemNumber AND psiMax.Size=FP.Size and psiMax.PONumber=FP.PONumber  
   -- detect is fully packed for this FG & size  
   join #tempSOLineCount soc on soc.HartalegaCommonSize=fp.Size and soc.ItemId= fp.ItemNumber   
   join #tempPOlineCount poc on poc.ItemSize=fp.Size and poc.ItemNumber= fp.ItemNumber --filter correct FG item number  
   left join (select COUNT(casenumber) as SONotPacked,ponumber,Size,ItemNumber --filter correct FG item number  
   from purchaseorderitemcases with(nolock)  
   where ispreshipmentCase = 1 and InternalotNumber is null and Palletid is null  
   group by ponumber,Size,ItemNumber) packedCount  
  on packedcount.PONumber = FP.PONumber and packedcount.Size = fp.Size and packedCount.ItemNumber=fp.ItemNumber --filter correct FG item number  
   WHERE FP.InternalLotNumber = @InternalLotNumber    
  END        
  if isnull(@BatchCardNumber,'')=''      
  BEGIN      
   RAISERROR ('FG Batch Order is ammended or deleted', -- Message text.        
   16, -- Severity.        
   1 -- State.        
   );        
  END    
    
   set @PreshipmentCaseCount = 0    
   SELECT @PreshipmentCaseCount = Count(CaseNumber) FROM PurchaseOrderItemCases WITH (NOLOCK) WHERE PONumber = @Ponumber AND ItemNumber = @ReferenceItemNumber        
   AND Size = @Configuration AND IsPreshipmentCase = 1 AND InternaLotNumber IS NULL AND PalletId IS NULL        
  END        
  ELSE        
  BEGIN        
  IF EXISTS(select * FROM DOT_FSItemMaster WITH (NOLOCK) WHERE ItemId=@ItemNumber AND ItemType=8) --Surgical Packing Plan
  BEGIN																				--Surgical Packing Plan
	SET @FunctionIdentifier = 'SPPBC';  											--Surgical Packing Plan
  END																				--Surgical Packing Plan
  ELSE
  BEGIN														
	SET @FunctionIdentifier = 'SMBP';
  END  
         
   print @FunctionIdentifier      
      
   SET @BatchNumber = '';        
   SET @BatchCardNumber = '';  
  IF @Preshipment = 0 or @FunctionIdentifier = 'SPPBC' --bypass for SPPBC  
  BEGIN   
   SELECT TOP 1 @Configuration = PO.ItemSize,        
       @Resource = FP.Resource,        
       @CustomerPO = PO.OrderNumber,        
       @CustomerReference = PO.CustomerReferenceNumber,        
       @InnerLotNumber = FP.InternalLotNumber,        
       @OuterLotNumber = FP.OuterLotno,        
       @CustomerLotNumber = PO.CustomerLotNumber,        
       @Manufacturingdate = FP.ManufacturingDate,        
       @ExpiryDate = FP.ExpiryDate,        
       @PalletNumber = @PalletID,        
       @BatchOrderNumber = FP.FGBatchOrderNo,     
       @PostingDateTime = @DateScanned,        
       @PreshipmentCases = 0, -- Normal Pallet always set to 0        
       @Quantity = CT.CasesPacked,        
       @TotalQuantity = CT2.TotalCases,  --splitBatch  
       @ReferenceItemNumber = FP.ItemNumber,        
       @SalesOrdernumber = FP.PONumber,        
       @InnerBoxCapacity = PO.innerboxcapacity        
   FROM #finalpackingtemp FP        
   JOIN PurchaseOrderItem PO WITH (NOLOCK) ON PO.PONumber = FP.PONumber AND PO.ItemNumber = FP.ItemNumber AND FP.Size = PO.ItemSize  
   JOIN (select count(CaseNumber) as CasesPacked,InternalotNumber   
  from PurchaseOrderItemCases WITH (NOLOCK)     
  where InternalotNumber = @InternalLotNumber and palletid=@PalletID     
  group by InternalotNumber) CT on ct.InternalotNumber = FP.InternalLotNumber    
 JOIN (select count(CaseNumber) as TotalCases,InternalotNumber --splitBatch  
  from PurchaseOrderItemCases WITH (NOLOCK)     
  where InternalotNumber = @InternalLotNumber   
  group by InternalotNumber) CT2 on ct.InternalotNumber = FP.InternalLotNumber    
   WHERE FP.InternalLotNumber = @InternalLotNumber     
  END  
  ELSE  
  BEGIN  
   -- Get Max case number by internal lot no  
   select @MaxLotCaseNumber=max(CaseNumber)  
    from PurchaseOrderItemCases  WITH (NOLOCK)  
    where InternalotNumber=@InternalLotNumber and ispreshipmentCase = 1  
    group by InternalotNumber;  
   print @MaxLotCaseNumber    
  
     SELECT TOP 1 @Configuration = PO.ItemSize,        
       @Resource = FP.Resource,        
       @CustomerPO = PO.OrderNumber,        
       @CustomerReference = PO.CustomerReferenceNumber,        
       @InnerLotNumber = FP.InternalLotNumber,        
       @OuterLotNumber = FP.OuterLotno,        
       @CustomerLotNumber = PO.CustomerLotNumber,        
       @Manufacturingdate = FP.ManufacturingDate,        
       @ExpiryDate = FP.ExpiryDate,        
       @PalletNumber = @PalletID,        
       @BatchOrderNumber = FP.FGBatchOrderNo,    
       @PostingDateTime = @DateScanned,        
       @PreshipmentCases = CASE WHEN soc.SOLineCount=poc.POLineCount and isnull(packedCount.SONotPacked,0) = 0 and psiMax.MaxCaseNumber=@MaxLotCaseNumber THEN psiMax.Qty ELSE 0 END,        
       @Quantity = CT.CasesPacked,  
       @TotalQuantity = CT2.TotalCases,  --splitBatch  
       @ReferenceItemNumber = FP.ItemNumber,        
       @SalesOrdernumber = FP.PONumber,        
       @InnerBoxCapacity = PO.innerboxcapacity        
   FROM #finalpackingtemp FP        
   JOIN PurchaseOrderItem PO WITH (NOLOCK) ON PO.PONumber = FP.PONumber AND PO.ItemNumber = FP.ItemNumber AND FP.Size = PO.ItemSize  
   JOIN (select count(CaseNumber) as CasesPacked,InternalotNumber   
  from PurchaseOrderItemCases WITH (NOLOCK)     
  where InternalotNumber = @InternalLotNumber and palletid=@PalletID     
  group by InternalotNumber) CT on ct.InternalotNumber = FP.InternalLotNumber    
 JOIN (select count(CaseNumber) as TotalCases,InternalotNumber  --splitBatch  
  from PurchaseOrderItemCases WITH (NOLOCK)     
  where InternalotNumber = @InternalLotNumber   
  group by InternalotNumber) CT2 on ct.InternalotNumber = FP.InternalLotNumber    
   
  -- PSI cases handing logic  
   -- calculate how many PSI packed for this FG & size  
   JOIN #tempPalletMaxCase psiMax on psiMax.ItemNumber=FP.ItemNumber AND psiMax.Size=FP.Size and psiMax.PONumber=FP.PONumber  
   -- detect is fully packed for this FG & size  
   join #tempSOLineCount soc on soc.HartalegaCommonSize=fp.Size and soc.ItemId= fp.ItemNumber   
   join #tempPOlineCount poc on poc.ItemSize=fp.Size and poc.ItemNumber= fp.ItemNumber  
   left join (select COUNT(casenumber) as SONotPacked,ponumber,Size,ItemNumber  
   from purchaseorderitemcases with(nolock)  
   where ispreshipmentCase = 1 and InternalotNumber is null and Palletid is null  
   group by ponumber,Size,ItemNumber) packedCount  
  on packedcount.PONumber = FP.PONumber and packedcount.Size = fp.Size and packedCount.ItemNumber=fp.ItemNumber  
   WHERE FP.InternalLotNumber = @InternalLotNumber     
  
  END  
  
	IF @FunctionIdentifier <> 'SPPBC' --Surgical Packing Plan
	BEGIN
	   /** List all Reference Batch from InternalLotNumber **/        
	   /** FloorSystem Method: AXPostingBLL.GetScanMultipleBatchInfoforPosting(SerialNumber)        
	        EXEC USP_FP_Get_ScanMultipleBatchInfoforPosting @InternalLotNumber **/        
	   SELECT Row_number() OVER (ORDER BY (SELECT 1)) AS Id, BT.SerialNumber, BT.Size, BT.BatchNumber, FPB.BoxesPacked, BT.GloveType    
	   INTO #finalpackingSMBPtemp FROM Batch BT WITH (NOLOCK) INNER JOIN FinalPackingbatchinfo FPB WITH (NOLOCK) ON BT.SerialNumber = FPB.SerialNumber        
	   WHERE FPB.InternalLotNumber = @InternalLotNumber        
	   --print '238'    
	   --Reference Batch 1      
	   SELECT @ReferenceBatchNumber1 = SerialNumber,      
	   @ReferenceBatchSequence1 = (SELECT Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber=a.SerialNumber),      
	   @RefItemNumber1 = GloveType,      
	   @RefNumberOfPieces1 = CASE WHEN @Quantity <> @TotalQuantity THEN ROUND(AVG(CAST(@InnerBoxCapacity * BoxesPacked * @Quantity / @TotalQuantity AS DECIMAL)) * 2, 0) / 2	 
								 ELSE @InnerBoxCapacity * BoxesPacked END --splitBatch
	   FROM #finalpackingSMBPtemp a WHERE Id = 1      
	   GROUP BY SerialNumber,GloveType,BoxesPacked
	   SET @SerialNumber = @ReferenceBatchNumber1 --Set for AXPostingLog      

	   --print '246'    
	   --Reference Batch 2      
	   SELECT @ReferenceBatchNumber2 = ISNULL(SerialNumber,''),      
	   @ReferenceBatchSequence2 = ISNULL((SELECT Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber=b.SerialNumber),0),      
	   @RefItemNumber2 = ISNULL(GloveType,0),      
	   @RefNumberOfPieces2 = CASE WHEN @Quantity <> @TotalQuantity AND @ReferenceBatchNumber2 <> '' THEN ROUND(AVG(CAST(@InnerBoxCapacity * BoxesPacked * @Quantity / @TotalQuantity AS DECIMAL)) * 2, 0) / 2	 
								 ELSE ISNULL(@InnerBoxCapacity * BoxesPacked,'') END --splitBatch
	   FROM #finalpackingSMBPtemp b WHERE Id = 2      
	   GROUP BY SerialNumber,GloveType,BoxesPacked
      
       --Reference Batch 3      
       SELECT @ReferenceBatchNumber3 = ISNULL(SerialNumber,''),      
       @ReferenceBatchSequence3 = ISNULL((SELECT Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber=c.SerialNumber),0),      
       @RefItemNumber3 = ISNULL(GloveType,0),      
       @RefNumberOfPieces3 = CASE WHEN @Quantity <> @TotalQuantity AND @ReferenceBatchNumber3 <> '' THEN ROUND(AVG(CAST(@InnerBoxCapacity * BoxesPacked * @Quantity / @TotalQuantity AS DECIMAL)) * 2, 0) / 2	 
								 ELSE ISNULL(@InnerBoxCapacity * BoxesPacked,'') END --splitBatch    
	   FROM #finalpackingSMBPtemp c WHERE Id = 3      
	   GROUP BY SerialNumber,GloveType,BoxesPacked
      
       --Reference Batch 4      
	   SELECT @ReferenceBatchNumber4 = ISNULL(SerialNumber,''),      
	   @ReferenceBatchSequence4 = ISNULL((SELECT Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber=d.SerialNumber),0),      
	   @RefItemNumber4 = ISNULL(GloveType,0),      
	   @RefNumberOfPieces4 = CASE WHEN @Quantity <> @TotalQuantity AND @ReferenceBatchNumber4 <> '' THEN ROUND(AVG(CAST(@InnerBoxCapacity * BoxesPacked * @Quantity / @TotalQuantity AS DECIMAL)) * 2, 0) / 2	 
								 ELSE ISNULL(@InnerBoxCapacity * BoxesPacked,'') END --splitBatch       
	   FROM #finalpackingSMBPtemp d WHERE Id = 4      
	   GROUP BY SerialNumber,GloveType,BoxesPacked
      
       --Reference Batch 5      
	   SELECT @ReferenceBatchNumber5 = ISNULL(SerialNumber,''),      
	   @ReferenceBatchSequence5 = ISNULL((SELECT Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber=e.SerialNumber),0),      
	   @RefItemNumber5 = ISNULL(GloveType,0),      
	   @RefNumberOfPieces5 = CASE WHEN @Quantity <> @TotalQuantity AND @ReferenceBatchNumber5 <> '' THEN ROUND(AVG(CAST(@InnerBoxCapacity * BoxesPacked * @Quantity / @TotalQuantity AS DECIMAL)) * 2, 0) / 2	 
								 ELSE ISNULL(@InnerBoxCapacity * BoxesPacked,'') END --splitBatch   
	   FROM #finalpackingSMBPtemp e WHERE Id = 5     
	   GROUP BY SerialNumber,GloveType,BoxesPacked 
     
       DROP TABLE #finalpackingSMBPtemp  
	 END
	 ELSE 
	 BEGIN
		SELECT Row_number() OVER (ORDER BY (SELECT 1)) AS Id, SPD.SerialNumber, SPP.ItemSize, SPD.GloveSize, SPD.BatchNumber, SPD.ReservedQty, SPD.GloveCode, SPP.SamplingPcsQty, SPP.RequiredPcsQty
		INTO #finalpackingSPPBCtemp FROM SurgicalPackingPlan SPP WITH (NOLOCK) JOIN SurgicalPackingPlanDetails SPD on SPP.SurgicalPackingPlanId = SPD.SurgicalPackingPlanId
		WHERE SPP.InternalLotNo = @InternalLotNumber AND SPP.PlanStatus = 3 --Surgical Packing Plan
		SELECT Top 1 @SumGloveSampleQuantity = SamplingPcsQty from #finalpackingSPPBCtemp
		SET @SumGloveSampleQuantity = ROUND(AVG(CAST(@Quantity AS DECIMAL) / @TotalQuantity * @SumGloveSampleQuantity), 0)
	 END

    END      
   -- print '277'      
    /** FloorSystem Method: AXPostingBLL.GetBatchSequence(SerialNumber)        
       EXEC USP_GET_BATCHSEQUENCE SerialNumber **/        
   if @FunctionIdentifier = 'SMBP' OR @FunctionIdentifier = 'SPPBC'
   BEGIN      
    SELECT @Sequence = 0-- Count(SerialNumber) + 1 FROM dbo.AXPostingLog WHERE SerialNumber = @ReferenceBatchNumber1       
   END      
   ELSE      
   BEGIN        
     SELECT @Sequence = Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber = @BatchNumber-- and ServiceName <> 'SMBP'        
   END      
   --print '281'      
    /** only MTS, @IsWTS is true **/        
    IF (@BatchOrderNumber = @SalesOrdernumber AND @CustomerLotNumber = '')        
    BEGIN        
   SET @IsWTS = 1        
    END        
    ELSE        
    BEGIN        
   SET @IsWTS = 0        
    END        
    
	/** Mantis# 0008589: Plant 7 Web Admin Surgical SPPBC does not generate quality order correctly **/
	/** For Surgical, no PSI cases defined in PurchaseOrderItemCases **/
	/** This part will detect last pallet defined in PurchaseOrderItemCases by PoNo and size **/
	/**	Then count PreshipmentCases from PurchaseOrderItem  by PoNo and ItemSize **/
	IF @FunctionIdentifier = 'SPPBC'
	BEGIN
		DECLARE @LastCasenumber INT
  		DECLARE @LastPalletId NVARCHAR(10)    
  		SELECT @LastCasenumber = Max(CaseNumber) FROM PurchaseOrderItemCases WITH (NOLOCK) where PONumber = @SALESORDERNUMBER and size = @Size --0008589   
		SELECT @LastPalletId = PalletId FROM PurchaseOrderItemCases WITH (NOLOCK) where PONumber = @SALESORDERNUMBER and CaseNumber = @LastCasenumber and size = @Size
		IF @LastPalletId = @PalletID AND @row = @COUNT
		BEGIN
			DECLARE @strCases NVARCHAR(4000)   --0008589
			SET @Preshipment=1
			SELECT @strCases = Preshipmentcases FROM PurchaseOrderItem WITH (NOLOCK) where PONumber = @SALESORDERNUMBER and ItemSize = @Size  --0008589
			SELECT @Preshipmentcases = SUM(len(@strCases) - len(replace(@strCases, ',', '')) +1)
		END
	END
	/** Mantis# 0008589: Plant 7 Web Admin Surgical SPPBC does not generate quality order correctly **/

	/** Open batch flag for NGC1.5 **/
	IF (@FunctionIdentifier = 'SBC')
	BEGIN
		IF (SELECT COUNT(1) FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @BATCHNUMBER AND IsConsolidated = 1) > 0
		BEGIN
			SET @IsConsolidated = 1
		END
	END
	ELSE IF (@FunctionIdentifier = 'SMBP')
	BEGIN
		IF (SELECT COUNT(1) FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @ReferenceBatchNumber1 AND IsConsolidated = 1) > 0
		BEGIN
			SET @IsConsolidated = 1
		END
	END
           
     /** AX Posting parent, child & AXPostingLog staging table        
      EXEC USP_DOT_CreateFLOORAXINTPARENTTABLE **/        
   BEGIN        
         
   INSERT INTO [dbo].[Dot_FloorAXIntParentTable]        
      ([BatchCardNumber]        
      ,[BatchNumber]        
      ,[CreationTime]        
      ,[CreatorUserId]        
      ,[DeleterUserId]        
      ,[DeletionTime]        
      ,[ErrorMessage]        
      ,[FSIdentifier]        
      ,[FunctionIdentifier]        
      ,[IsDeleted]        
      ,[LastModificationTime]        
      ,[LastModifierUserId]        
      ,[ProcessingStatus]        
      ,[PlantNo]        
      ,[ProdId]        
      ,[ReferenceBatchNumber1]        
      ,[ReferenceBatchNumber2]        
      ,[ReferenceBatchNumber3]        
      ,[ReferenceBatchNumber4]        
      ,[ReferenceBatchNumber5]        
      ,[ReferenceBatchSequence1]        
      ,[ReferenceBatchSequence2]        
      ,[ReferenceBatchSequence3]        
      ,[ReferenceBatchSequence4]        
      ,[ReferenceBatchSequence5]        
      ,[Sequence]        
      ,[PalletId]    
      ,[PalletSerialNo]
      ,[FGQuantity] 			--Surgical Packing Plan
      ,[Preshipment] 			--Surgical Packing Plan
      ,[PreshipmentCases] 		--Surgical Packing Plan
      ,[GloveSampleQuantity] 	--Surgical Packing Plan
      ,[IsConsolidated])		--NGC 1.5 Open Batch flag
    VALUES (@BATCHCARDNUMBER, @BATCHNUMBER, Getdate(), 1, NULL, NULL, '', @FSIDENTIFIER, @FUNCTIONIDENTIFIER, 0, Getdate(), 1, 1, @PLANTNO, NULL, @REFERENCEBATCHNUMBER1,        
    @REFERENCEBATCHNUMBER2, @REFERENCEBATCHNUMBER3, @REFERENCEBATCHNUMBER4, @REFERENCEBATCHNUMBER5, @REFERENCEBATCHSEQUENCE1, @REFERENCEBATCHSEQUENCE2,        
    @REFERENCEBATCHSEQUENCE3, @REFERENCEBATCHSEQUENCE4, @REFERENCEBATCHSEQUENCE5, @SEQUENCE, @PalletId, @PalletSerialNo,
    @QUANTITY, @PRESHIPMENT, @PRESHIPMENTCASES, @SumGloveSampleQuantity,  --Surgical Packing Plan      
    @IsConsolidated) --NGC 1.5 Open Batch flag
	
   SET @PARENTREFRECID = (SELECT @@IDENTITY);
   
   IF @FunctionIdentifier = 'SPPBC' --Surgical Packing Plan
   BEGIN			
   	DECLARE @SPPROW INT = 1;      																																			 
	DECLARE @SPPCOUNT INT;																																					 
   	DECLARE @ACCGloveSampleQuantity INT = 0																																		 
	SET @SPPCOUNT = (SELECT MAX(Id) FROM #finalpackingSPPBCtemp);  																											 
	WHILE (@SPPROW <= @SPPCOUNT)  																																			 
	BEGIN			
		Select @SPPBatchCardNumber = BatchNumber, @SPPBatchNumber = SerialNumber, @PickingListQuantity = ReservedQty , @Configuration = ItemSize,
		@GloveSize = GloveSize, @GloveSampleQuantity = ROUND(AVG(CAST(ReservedQty AS DECIMAL)/((RequiredPcsQty+SamplingPcsQty)/2))*(SamplingPcsQty/2), 0)
		FROM #finalpackingSPPBCtemp WITH (NOLOCK) where Id = @SPPROW
		group by BatchNumber,SerialNumber,ReservedQty,ItemSize,GloveSize,Id,SamplingPcsQty

		SET @Configuration = CASE
			WHEN @Configuration LIKE '%,%' THEN 
				LEFT(SUBSTRING(@Size, PATINDEX('%[0-9.-]%', @Configuration), 8000),
				PATINDEX('%[^0-9.-]%', SUBSTRING(@Configuration, PATINDEX('%[0-9.-]%', @Configuration), 8000) + 'X') -1) 
			ELSE @Configuration																				 
		END
		SELECT @BatchSequence = Count(SerialNumber) + 1 FROM dbo.AXPostingLog with (nolock) WHERE SerialNumber = @SPPBatchNumber

		SET @GloveSampleQuantity = ROUND(AVG(CAST(@Quantity AS DECIMAL) / @TotalQuantity * @GloveSampleQuantity), 0)
		SET @PickingListQuantity = ROUND(AVG(CAST(@Quantity AS DECIMAL) / @TotalQuantity * @PickingListQuantity), 0) - @GloveSampleQuantity

		IF @SPPROW < @SPPCOUNT
		BEGIN
			SET @ACCGloveSampleQuantity = @ACCGloveSampleQuantity + @GloveSampleQuantity
		END
		ELSE
		BEGIN
			SET @GloveSampleQuantity = @SumGloveSampleQuantity - @ACCGloveSampleQuantity
   			-- after re-calc @GloveSampleQuantity need to re-calc @PickingListQuantity also, Max He 24/09/2021  
   			SET @PickingListQuantity = ROUND(AVG(CAST(@Quantity AS DECIMAL) / @TotalQuantity * @PickingListQuantity), 0) - @GloveSampleQuantity  
  		END  

		 -- check individual detail batch is it open batch or not
		IF (SELECT COUNT(1) FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @SPPBatchNumber AND IsConsolidated = 1) > 0
		BEGIN
			SET @IsConsolidatedMultipleBatch = 1;
		END
		/** EXEC USP_DOT_CreateFGRAFJournal SPPBC **/
		EXEC Usp_dot_createfgrafjournal @BATCHORDERNUMBER, @REFERENCEITEMNUMBER, @CONFIGURATION, @WAREHOUSE, @RESOURCE, @CUSTOMERPO, @CUSTOMERREFERENCE, @SALESORDERNUMBER,        
		@INNERLOTNUMBER, @OUTERLOTNUMBER, @CUSTOMERLOTNUMBER, @PRESHIPMENT, @PRESHIPMENTCASES, @POSTINGDATETIME, @QUANTITY, @PARENTREFRECID, @LOCATION, 
		@PALLETNUMBER, @ExpiryDate, @ManufacturingDate, @IsWTS, @ItemNumber, @RefNumberOfPieces1, @RefNumberOfPieces2,        
		@RefNumberOfPieces3, @RefNumberOfPieces4, @RefNumberOfPieces5, @RefItemNumber1, @RefItemNumber2, @RefItemNumber3, @RefItemNumber4,        
		@RefItemNumber5, @SPPBatchCardNumber, @SPPBatchNumber, @PickingListQuantity, @BatchSequence, @GloveSize, @GloveSampleQuantity;
		/** AXPostingLog SPPBC **/
		EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @SPPBatchNumber, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID, @Area, @IsConsolidatedMultipleBatch  
		
		-- As long as have 1 batch is new batch card set IsConsolidated = 1 then parent staging table consider as new batch and handle in WebAdmin2
		IF @IsConsolidated = 0 AND @IsConsolidatedMultipleBatch = 1
		BEGIN
			SET @IsConsolidated = 1;
		END
		SET @SPPROW = @SPPROW + 1	
	END	-- while loop end for insert SPPBC FG detail info 
	IF @IsConsolidated = 1 -- update parent table if has new batch card(consolidated=1) 
	BEGIN
		UPDATE Dot_FloorAXIntParentTable set IsConsolidated=@IsConsolidated where Id=@PARENTREFRECID;
	END
	
	DROP TABLE #finalpackingSPPBCtemp																																		 
   END
   ELSE
   BEGIN  
	   /** EXEC USP_DOT_CreateFGRAFJournal SBC/SMBP **/
	   EXEC Usp_dot_createfgrafjournal @BATCHORDERNUMBER, @REFERENCEITEMNUMBER, @CONFIGURATION, @WAREHOUSE, @RESOURCE, @CUSTOMERPO, @CUSTOMERREFERENCE, @SALESORDERNUMBER,        
		@INNERLOTNUMBER, @OUTERLOTNUMBER, @CUSTOMERLOTNUMBER, @PRESHIPMENT, @PRESHIPMENTCASES, @POSTINGDATETIME, @QUANTITY, @PARENTREFRECID,        
		@LOCATION, @PALLETNUMBER, @ExpiryDate, @ManufacturingDate, @IsWTS, @ItemNumber, @RefNumberOfPieces1, @RefNumberOfPieces2,        
		@RefNumberOfPieces3, @RefNumberOfPieces4, @RefNumberOfPieces5, @RefItemNumber1, @RefItemNumber2, @RefItemNumber3, @RefItemNumber4,        
		@RefItemNumber5, @SPPBatchCardNumber, @SPPBatchNumber, @PickingListQuantity, @BatchSequence, @GloveSize, @GloveSampleQuantity;
        
		/** dbo.AXPostingLog **/     
		IF @FunctionIdentifier = 'SMBP'  
		BEGIN  
			SELECT @BATCHCARDNUMBER = BatchNumber from batch with (nolock) where SerialNumber = @ReferenceBatchNumber1 
			-- check ReferenceBatchNumber1 is it open batch or not
			SELECT @IsConsolidatedMultipleBatch=case when COUNT(1)>0 then 1 else 0 end FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @ReferenceBatchNumber1 AND IsConsolidated = 1
			EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @ReferenceBatchNumber1, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID, @Area, @IsConsolidatedMultipleBatch       
			IF @ReferenceBatchNumber2 is not null  
			BEGIN 
				-- check ReferenceBatchNumber2 is it open batch or not
				SELECT @IsConsolidatedMultipleBatch=case when COUNT(1)>0 then 1 else 0 end FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @ReferenceBatchNumber2 AND IsConsolidated = 1
				SELECT @BATCHCARDNUMBER = BatchNumber from batch with (nolock) where SerialNumber = @ReferenceBatchNumber2  
				EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @ReferenceBatchNumber2, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID, @Area, @IsConsolidatedMultipleBatch  
				IF @ReferenceBatchNumber3 is not null  
				BEGIN
					-- check ReferenceBatchNumber3 is it open batch or not
					SELECT @IsConsolidatedMultipleBatch=case when COUNT(1)>0 then 1 else 0 end FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @ReferenceBatchNumber3 AND IsConsolidated = 1
					SELECT @BATCHCARDNUMBER = BatchNumber from batch with (nolock) where SerialNumber = @ReferenceBatchNumber3  
					EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @ReferenceBatchNumber3, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID, @Area, @IsConsolidatedMultipleBatch       
					IF @ReferenceBatchNumber4 is not null  
					BEGIN
						-- check ReferenceBatchNumber4 batch is it open batch or not
						SELECT @IsConsolidatedMultipleBatch=case when COUNT(1)>0 then 1 else 0 end FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @ReferenceBatchNumber4 AND IsConsolidated = 1
						SELECT @BATCHCARDNUMBER = BatchNumber from batch with (nolock) where SerialNumber = @ReferenceBatchNumber4  
						EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @ReferenceBatchNumber4, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID, @Area, @IsConsolidatedMultipleBatch      
						IF @ReferenceBatchNumber5 is not null  
						BEGIN
							-- check ReferenceBatchNumber5 is it open batch or not
							SELECT @IsConsolidatedMultipleBatch=case when COUNT(1)>0 then 1 else 0 end FROM AXPostingLog with (NOLOCK) WHERE SerialNumber = @ReferenceBatchNumber5 AND IsConsolidated = 1
							SELECT @BATCHCARDNUMBER = BatchNumber from batch with (nolock) where SerialNumber = @ReferenceBatchNumber5  
							EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @ReferenceBatchNumber5, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID, @Area, @IsConsolidatedMultipleBatch      
						END  
					END  
				END       
			END  
		END  
		ELSE  
		BEGIN     
			EXEC dbo.Usp_save_axpostinglog @FunctionIdentifier, @PostingType, @PostingDateTime, @BATCHCARDNUMBER, @SerialNumber, @IsPostedToAX, @IsPostedInAX, @Sequence, @ExceptionCode, @TransactionID,  @Area, @IsConsolidated        
		END    
   END          
   END        
   SET @ROW = @ROW + 1      
 SET @AccQty = @AccQty + @Quantity --#EWN qty validation with staging  
    -- Reset value      
   SET @InternalLotNumber='';        
   SET @DateStockOut=null;      
   SET @BatchCardNumber='';      
   SET @BatchNumber='';       
   SET @FSIdentifier=null;      
   SET @FunctionIdentifier=null;      
   SET @ReferenceBatchNumber1=null;        
   SET @ReferenceBatchNumber2=null;        
   SET @ReferenceBatchNumber3=null;        
   SET @ReferenceBatchNumber4=null;        
   SET @ReferenceBatchNumber5=null;        
   SET @ReferenceBatchSequence1 = 0        
   SET @ReferenceBatchSequence2 = 0        
   SET @ReferenceBatchSequence3 = 0        
   SET @ReferenceBatchSequence4 = 0        
   SET @ReferenceBatchSequence5 = 0        
   SET @Sequence = 0;        
   SET @BatchOrderNumber=null;      
   SET @ReferenceItemNumber=null;      
   SET @Configuration=null;      
   SET @Warehouse = 'FG'        
   SET @Resource=null;      
   SET @CustomerPO=null;      
   --SET @CustomerReference=null;      
   --SET @SalesOrdernumber=null;      
   SET @InnerLotNumber=null;      
   SET @OuterLotNumber=null;      
   SET @CustomerLotNumber=null;      
   --SET @Preshipment = 0        
   SET @Preshipmentcases = 0        
   SET @InnerBoxCapacity = 0        
   SET @PostingDateTime = null        
   SET @Quantity = 0      
   SET @TotalQuantity = 0      --splitBatch  
   SET @ParentRefRecId = 0        
   SET @Location = ''        
   --SET @PalletNumber = ''        
   SET @ExpiryDate = null        
   SET @Manufacturingdate = null        
   SET @IsWTS = 0        
   --SET @ItemNumber = NULL        
   SET @CreateDateTime = GETDATE()        
   --SET @PONumber =''        
   SET @PreshipmentCaseCount = 0        
   SET @RefNumberOfPieces1 = 0 --For SMBP        
   SET @RefNumberOfPieces2 = 0 --For SMBP        
   SET @RefNumberOfPieces3 = 0 --For SMBP        
   SET @RefNumberOfPieces4 = 0 --For SMBP        
   SET @RefNumberOfPieces5 = 0 --For SMBP        
   SET @RefItemNumber1 = NULL --For SMBP        
   SET @RefItemNumber2 = NULL --For SMBP        
   SET @RefItemNumber3 = NULL --For SMBP        
   SET @RefItemNumber4 = NULL --For SMBP        
   SET @RefItemNumber5 = NULL --For SMBP        
   SET @PostingType = 'DOTFGJournalContract'        
   SET @SerialNumber = ''      
   SET @IsPostedToAX = 1        
   SET @IsPostedInAX = 1        
   SET @ExceptionCode = NULL        
   SET @TransactionID = '-1'        
   SET @Area = 'PS'
   SET @SPPBatchCardNumber = ''		--Surgical Packing Plan
   SET @SPPBatchNumber = ''			--Surgical Packing Plan
   SET @PickingListQuantity = 0 	--Surgical Packing Plan
   SET @BatchSequence = 0 			--Surgical Packing Plan
   SET @GloveSize = ''				--Surgical Packing Plan   
   SET @SumGloveSampleQuantity = 0	--Surgical Packing Plan   
   SET @GloveSampleQuantity = 0		--Surgical Packing Plan   
    -- Reset value      
  
END   -- while loop end    
      
 -- clean up      
 if OBJECT_ID('tempdb.dbo.#temptable') IS NOT NULL       
 BEGIN    
  DROP TABLE #temptable;       
 END  
    
 if OBJECT_ID('tempdb.dbo.#temptablePSI') IS NOT NULL       
 BEGIN    
  DROP TABLE #temptablePSI;      
 END  
      
 if OBJECT_ID('tempdb.dbo.#tempSOLineCount') IS NOT NULL       
 BEGIN    
  DROP TABLE #tempSOLineCount;      
 END     
       
 if OBJECT_ID('tempdb.dbo.#tempPOlineCount') IS NOT NULL       
 BEGIN    
  DROP TABLE #tempPOlineCount;      
 END    
  
 if OBJECT_ID('tempdb.dbo.#tempPalletMaxCase') IS NOT NULL       
 BEGIN    
  DROP TABLE #tempPalletMaxCase;      
 END     
   
   
 END TRY        
        
 BEGIN CATCH        
 DECLARE @ErrorMessage NVARCHAR(4000);        
 DECLARE @ErrorSeverity INT;        
 DECLARE @ErrorState INT;        
 SELECT @ErrorMessage = Error_message(),        
   @ErrorSeverity = Error_severity(),        
   @ErrorState = Error_state();  
 Insert into FGReceivedPallet_Error(ScanDate, PalletID, ErrorMsg)  
 values(GETDATE(), @PalletID, 'DOT-'+@ErrorMessage)   
 RAISERROR (@ErrorMessage, @ErrorSeverity,@ErrorState);       
        
 END CATCH;        
        
 IF @@TRANCOUNT > 0   
 BEGIN       
 COMMIT TRANSACTION;    
  
 --#EWN qty validation with staging  
 IF @EWNQty = @AccQty  
 BEGIN  
  SET @Return=1;  
  Insert into FGReceivedPallet_Error(ScanDate, PalletID, ErrorMsg)  
  values(GETDATE(), @PalletID, 'DOT-Insert Success.')  
 END   
 ELSE  
 BEGIN  
  DECLARE @qtyStaging INT   
  SET @qtyStaging = (Select sum(Quantity) from DOT_FGJournalTable with (nolock) where palletnumber=@PalletID and salesordernumber=@PONumber   
       and ReferenceItemNumber = @ItemNumber);  
  IF @EWNQty = @qtyStaging  
  BEGIN  
   SET @Return=1;  
   Insert into FGReceivedPallet_Error(ScanDate, PalletID, ErrorMsg)  
   values(GETDATE(), @PalletID, 'DOT-Insert Success.')  
  END  
  ELSE  
  BEGIN  
   SET @Return=0;   
   Insert into FGReceivedPallet_Error(ScanDate, PalletID, ErrorMsg)  
   values(GETDATE(), @PalletID, 'DOT-Total quantity not tally with EWN, please re-scan!')  
  END  
 END  
  
 END      
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_IsOnlineSurgicaGlove]...';


GO
CREATE PROCEDURE [dbo].[USP_DOT_IsOnlineSurgicaGlove]
(
       @serialnumber numeric(15,0)
)
AS

Declare @serialStr nvarchar(20) = CAST(@SerialNumber as nvarchar(20))

IF exists (select 1 from DOT_FloorD365HRGLOVERPT a with (nolock) join DOT_FloorD365BO b with (nolock) on a.BthOrder = b.BthOrderId 
			where a.serialNo = @serialStr and b.prodPoolId = 'SGR' and a.IsDeleted = 0 and b.IsDeleted=0)
BEGIN
       SELECT 1
END
ELSE
BEGIN
       SELECT 0
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_IsSurgicalGlove]...';


GO
CREATE PROCEDURE [dbo].[USP_DOT_IsSurgicalGlove]
(
       @serialnumber numeric(15,0)
)
AS
declare @ItemType varchar(500)
select @ItemType=i.ItemType
from Batch b with (nolock) 
join DOT_FSItemMaster i with (nolock) on b.GloveType=i.ItemId
where b.SerialNumber=@serialnumber 
IF @ItemType=109 -- surgical glove
BEGIN
       SELECT 1
END
ELSE
BEGIN
       SELECT 0
END
GO
PRINT N'Creating Procedure [dbo].[Usp_DOT_Online2G_Print_Save]...';


GO
-- ==================================================================================  
-- Name:   Usp_DOT_Online2G_Print_Save
-- Purpose:  Save & Print 2G Batch Card
-- ==================================================================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   -----------------------------------------------------------------  
-- 05/01/2021  Azrul Amin    SP created.  
-- exec Usp_DOT_Online2G_Print_Save 1, 2, 'L79','NR-HS-OLPD-105-EC-NATL-CFDA','L','HNBON000312958',100,12,12.332,'21-Oct-20 4:55:53 PM',2,4,7,10047,'P1L79'
-- exec Usp_DOT_Online2G_Print_Save 1, 2, 'L107','NR-HS-OLPD-105-EC-NATL-CFDA','L','HNBON000312959',100,12,12.332,'21-Oct-20 4:55:53 PM',2,4,7,10047,'P10L107'
-- ==================================================================================  

CREATE PROCEDURE [dbo].[Usp_DOT_Online2G_Print_Save]  
(  
@UserId VARCHAR(10),
@ShiftId VARCHAR(10),
@Line VARCHAR(10),
@GloveCode VARCHAR(50),
@Size VARCHAR(20),
@BatchOrder VARCHAR(50),
@PackingSize INT,
@InnerBox INT,
@TenPcsWeight DECIMAL(18,3),
@BatchCardDate DATETIME,
@ModuleId INT,
@SubModuleID INT,
@SiteNumber INT,
@WorkStationNumber INT,
@Resource VARCHAR(50)
)   
AS  
BEGIN  
  
DECLARE @LocationId INT    
DECLARE @LineId VARCHAR(4)  
DECLARE @SerialNumber Numeric
DECLARE @BatchNumber VARCHAR(50)
DECLARE @GloveCategory VARCHAR(50)
DECLARE @Quantity INT 
DECLARE @BatchWeight DECIMAL (18,3)
DECLARE @shiftname VARCHAR(10)
DECLARE @shiftStartDate NVARCHAR(8)  
DECLARE @rejectSequenceNumber AS NUMERIC(15,0) 
DECLARE @BatchType VARCHAR(10) = 'PR'

BEGIN TRANSACTION;  
  
BEGIN TRY  
SET NOCOUNT ON

SELECT @LocationId = LocationId from WorkStationMaster with (nolock) where IsDeleted=0 and WorkstationId= @WorkStationNumber  
SELECT @SerialNumber = dbo.Ufn_SerailNumberPart(@SiteNumber,@BatchCardDate) + dbo.Ufn_IntToChar((NEXT VALUE FOR DBO.SerialNumberSeq),7)
--SELECT @BatchNumber = dbo.Ufn_BatchNumber(@BatchCardDate,@Line,@Size)
SELECT @GloveCategory = ISNULL(dbo.Ufn_DOT_GetGloveCategory(@GloveCode),'')
SELECT @Quantity = @PackingSize * @InnerBox
SELECT @BatchWeight = (@Quantity * @TenPcsWeight)/10000
SELECT @shiftname = Name from ShiftMaster where ShiftId=@ShiftId

SET @shiftStartDate = dbo.Ufn_GetShiftStartDate(@shiftname)   
SET @rejectSequenceNumber = Next VALUE FOR DOT_Online2GGloveSeq  

-- Set batch Type from Enum Master  
SET @batchType = (SELECT EnumValue from EnumMaster where EnumText = @BatchType and EnumType = 'BatchType') 
SET @LineId = CASE LEN(@line) WHEN 2 THEN '0'+RIGHT(CAST(@Line as VARCHAR(5)),1) WHEN 3 THEN  RIGHT(CAST(@Line as VARCHAR(5)),2)  WHEN 4 THEN  RIGHT(CAST(@Line as VARCHAR(5)),3) END  

-- Generate Batch Number  
SELECT @batchNumber = RTRIM(@batchType) + RIGHT('0'+CAST(@LocationId AS VARCHAR(3)),2) +'/' + @shiftStartDate + '/' + @LineId  
   
-- Insert in to Batch table  
INSERT INTO DOT_FloorD365Online2G VALUES(FORMAT (@rejectSequenceNumber, '000000') ,@batchNumber,'P'+CAST(@LocationId AS VARCHAR(3)),@batchCardDate,@UserId,@ShiftId,@line,
@Resource,@GloveCode,@Size,@BatchOrder,@PackingSize,@InnerBox,@TenPcsWeight,@BatchWeight,@workStationNumber)  
          
-- Return record for print  
SELECT FORMAT( @rejectSequenceNumber,'0000000000') AS SerialNumber, @batchNumber AS BatchNumber, @BatchOrder as BatchOrder, @shiftname as ShiftName,
@Resource as Resource, @GloveCode as GloveCode, @GloveCategory as GloveCategory, @BatchWeight as BatchWeight, @TenPcsWeight as TenPcsWeight,
@Size as Size,@BatchCardDate as BatchCardDate,@PackingSize as PackingSize,@InnerBox as InnerBox,@Quantity as Quantity   

 SET NOCOUNT OFF  
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
PRINT N'Creating Procedure [dbo].[USP_DOT_Online2GResources_Get]...';


GO
-- =======================================================================  
-- Name:   USP_DOT_Online2GResources_Get
-- Purpose:   Get all Resources for Online 2nd Grade Glove Screen 
-- =======================================================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   ------------------------------------------------------  
-- 24/11/2020  Azrul Amin    SP created.  
-- 01/07/2021  Azrul Amin	 Cater Plant10 and onwards. 
-- =======================================================================  

CREATE PROCEDURE [dbo].[USP_DOT_Online2GResources_Get]   
(   
 @LocationId Int,   
 @LineId varchar(20),  
 @ItemId varchar(50),   
 @Size varchar(20)
)   
AS  
BEGIN     
 SET NOCOUNT ON;  
  
 -- #1.To list out all Resources Group (lines).  
 IF (@LineId IS NULL) OR (LEN(@LineId) < 0)   
 BEGIN  
  SELECT DISTINCT  
  dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) as LineId  
  ,'' as GloveCode  
  ,'' as Size  
  ,'' as BatchOrder  
  FROM   
   DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
   DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName  
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND bo.ProdStatus = 'StartedUp'   
  AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) in (select linenumber from linemaster where LocationId = @LocationId) 
 END  

-- #2.To list out all Glove Code.  
ELSE IF ((@ItemId IS NULL) OR (LEN(@ItemId) = 0))  
 BEGIN
   SELECT DISTINCT  
   '' as LineId  
   ,bo.ItemId as GloveCode  
   ,'' as Size  
   ,'' as BatchOrder   
   FROM   
    DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
    DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
    LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName  
   WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) = @LineId AND bo.ProdStatus = 'StartedUp'   
   and bo.IsDeleted=0  
 END
  
-- #3.To list out all Size based on selected Line & GloveCode.  
ELSE IF ((@Size IS NULL) OR (LEN(@Size) = 0))  
 BEGIN  
   SELECT DISTINCT  
   '' as LineId  
   ,'' as GloveCode  
   ,bo.Size as Size  
   ,'' as BatchOrder   
  FROM   
   DOT_FloorD365BO (NOLOCK) AS bo LEFT JOIN  
   DOT_FloorD365BOResource (NOLOCK) AS res ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName  
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) = @LineId  
  AND bo.ItemId = @ItemId AND bo.ProdStatus = 'StartedUp'  
  and bo.IsDeleted=0  
 END  
  
-- #4.To list out all Batch Order based on selected Line, GloveCode & Size.  
ELSE  
 BEGIN  
   SELECT DISTINCT  
   res.ResourceGrp as LineId
   ,'' as GloveCode  
   ,'' as Size  
   ,bo.BthOrderId as BatchOrder   
  FROM   
   DOT_FloorD365BO AS bo  WITH (NOLOCK) LEFT JOIN  
   DOT_FloorD365BOResource  AS res  WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName  
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) = @LineId   
  AND bo.ItemId = @ItemId AND bo.ProdStatus = 'StartedUp' and bo.Size = @Size
  and bo.IsDeleted=0  
 END  
 SET NOCOUNT OFF;    
END
GO
PRINT N'Creating Procedure [dbo].[Usp_DOT_Online2GRpt]...';


GO
-- ==================================================================================  
-- Name:   Usp_DOT_Online2GRpt
-- Purpose:  Online 2G Report
-- ==================================================================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   -----------------------------------------------------------------  
-- 10/01/2021  Azrul Amin    SP created.  
-- exec Usp_DOT_Online2GRpt '2021-05-01', '2021-05-20','P7',1
-- ==================================================================================  

CREATE PROCEDURE [dbo].[Usp_DOT_Online2GRpt]  
(  
	@StartDate DATETIME,
	@EndDate DATETIME,
	@Plant VARCHAR(5),
	@Shift INT = 1
--declare @StartDate DATETIME = '2021-05-01'
--declare @EndDate DATETIME = '2021-05-20'
--declare @Plant VARCHAR(5) = 'P7'
--declare @Shift INT = 1
)   
AS  
BEGIN  

	DECLARE @Location INT 
	SELECT @Location = LocationId from LocationMaster where LocationName = @Plant
 
	--Total batches
	select LineId,batchweight,TotalPCs, GloveType as GloveCode
	into hbc
	from batch with (nolock)
	where LocationId=@Location and BatchType = 'T'
 
	--Total batches
	select LineId, GloveCode, sum(batchweight) as HBCKgs, sum(TotalPCs) as HBCPcs 
	into hbc2
	from hbc with (nolock)
	group by LineId, GloveCode

	--Total 2G
	select GloveCode,LineId,sum(batchweight) as '2GKgs', sum(PackingSize*InnerBox) as '2GPcs'
	into secondGrade
	from DOT_FloorD365Online2G with (nolock)
	where Plant=@Plant
	and CurrentDateandTime BETWEEN   
	   IIF( @Shift = 0,CAST(FORMAT(@StartDate,'yyyy-MM-dd') + ' '+ '00:00:00' AS datetime),CAST(FORMAT(@StartDate,'yyyy-MM-dd') + ' '+ '07:00:00' AS datetime))  
	   AND  
	   IIF( @Shift = 0,CAST(FORMAT(@EndDate,'yyyy-MM-dd') + ' '+ '23:59:59' AS datetime),CAST(FORMAT(dateadd(day,1,@EndDate),'yyyy-MM-dd')  + ' ' + '06:59:59' AS datetime))  
	group by GloveCode,LineId

	select a.*,b.[2GKgs],b.[2GPcs], (CAST(b.[2GPcs] as decimal)/CAST(a.HBCPcs as decimal))*100 as Perc from hbc2 a inner join secondGrade b 
	on a.LineId = b.LineId and a.GloveCode = b.GloveCode
	order by b.GloveCode,b.LineId

	drop table hbc
	drop table hbc2
	drop table secondGrade
  
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_PickingListSummaryBatchCardReport]...';


GO

-- ==================================================================================================================================================        
-- Name: USP_DOT_PickingListSummaryBatchCardReport      
-- Purpose: Summary Batch Card Report for Change Batch Card Inner    
-- ==================================================================================================================================================        
-- Change History        
-- Date   Author   Comments        
-- -----  ------   ------------------------------------------------------------      
-- 2021/10/26   Azrul    SP created.        
-- ==================================================================================================================================================       
-- exec USP_DOT_PickingListSummaryBatchCardReport '2211289819,2211290110,2210559670', '', '', '2021-01-01','2021-10-01','','',''    
-- exec USP_DOT_PickingListSummaryBatchCardReport '2211289819,2211290110,2210559670', '', '', '','','','',''    
-- exec USP_DOT_PickingListSummaryBatchCardReport '', '', 'ANSL 301/20', '','','','',''    
-- exec USP_DOT_PickingListSummaryBatchCardReport '', '', 'MEDU 806/21', '','','','',''    
-- exec USP_DOT_PickingListSummaryBatchCardReport '', '', '', '2021-07-01','2021-07-31','','',''    
-- exec USP_DOT_PickingListSummaryBatchCardReport '2201141299' , '' , '' , '', '', '', '', ''
-- exec USP_DOT_PickingListSummaryBatchCardReport '2210616934' , '' , '' , '', '', '', '', ''
-- ==================================================================================================================================================    
    
CREATE   PROCEDURE [dbo].[USP_DOT_PickingListSummaryBatchCardReport]      
(
@SerialNumber NVARCHAR(4000),    
@D365BatchNumber NVARCHAR(4000),    
@CustReferenceNumber NVARCHAR(4000),    
@PostingDateTimeFrom NVARCHAR(100),    
@PostingDateTimeTo NVARCHAR(100),    
@PlantNo NVARCHAR(4000),    
@Warehouse NVARCHAR(4000),    
@FunctionIdentifier NVARCHAR(4000)    
)    
    
AS    
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;    
     
IF OBJECT_ID('tempdb..#SerialNumbers') IS NOT NULL      
DROP TABLE #SerialNumbers     
IF OBJECT_ID('tempdb..#D365BatchNumbers') IS NOT NULL      
DROP TABLE #D365BatchNumbers     
IF OBJECT_ID('tempdb..#CustReferenceNumbers') IS NOT NULL      
DROP TABLE #CustReferenceNumbers    
IF OBJECT_ID('tempdb..#TmpUnion') IS NOT NULL      
DROP TABLE #TmpUnion    
    
CREATE TABLE #SerialNumbers (SerialNumber NVARCHAR(4000) COLLATE DATABASE_DEFAULT)    
CREATE TABLE #D365BatchNumbers (D365BatchNumber NVARCHAR(4000) COLLATE DATABASE_DEFAULT)    
CREATE TABLE #CustReferenceNumbers (CustReferenceNumber NVARCHAR(4000))    
CREATE TABLE #tmpUnion (PostingDateTime DATETIME2,     
      SerialNumber NVARCHAR(50),     
      D365BatchNumber NVARCHAR(50),     
      BatchOrderNumber NVARCHAR(50),     
      MovementJournalNumber NVARCHAR(50),     
      TransferJournalNumber NVARCHAR(50),     
      PickingListJournalNumber NVARCHAR(50),     
      RouteCardJournalNumber NVARCHAR(50),     
      RAFJournalNumber NVARCHAR(50),     
      ItemNumber NVARCHAR(100),     
      Size NVARCHAR(10),     
      Warehouse NVARCHAR(10),     
      Location NVARCHAR(10),     
      TransitionQuantity DECIMAL (18, 4) DEFAULT 0.000,     
      ReservedQuantity DECIMAL (18, 4),     
      BaseQuantity DECIMAL DEFAULT 0,     
      Sequence INT,     
      FunctionIdentifier NVARCHAR(50),    
      PlantNo NVARCHAR(10),    
      ReferenceBatchNumber1 NVARCHAR(50) DEFAULT NULL,     
      ReferenceBatchNumber2 NVARCHAR(50) DEFAULT NULL,     
      ReferenceBatchNumber3 NVARCHAR(50) DEFAULT NULL,     
      ReferenceBatchNumber4 NVARCHAR(50) DEFAULT NULL,     
      ReferenceBatchNumber5 NVARCHAR(50) DEFAULT NULL,    
      ReferenceBatchSequence1 INT DEFAULT 0,     
      ReferenceBatchSequence2 INT DEFAULT 0,     
      ReferenceBatchSequence3 INT DEFAULT 0,     
      ReferenceBatchSequence4 INT DEFAULT 0,     
      ReferenceBatchSequence5 INT DEFAULT 0,    
      RefNumberOfPieces1 DECIMAL DEFAULT 0,     
      RefNumberOfPieces2 DECIMAL DEFAULT 0,     
      RefNumberOfPieces3 DECIMAL DEFAULT 0,     
      RefNumberOfPieces4 DECIMAL DEFAULT 0,     
      RefNumberOfPieces5 DECIMAL DEFAULT 0,    
      RefItemNumber1 NVARCHAR(50) DEFAULT NULL,     
      RefItemNumber2 NVARCHAR(50) DEFAULT NULL,     
      RefItemNumber3 NVARCHAR(50) DEFAULT NULL,     
      RefItemNumber4 NVARCHAR(50) DEFAULT NULL,     
      RefItemNumber5 NVARCHAR(50) DEFAULT NULL,
	  CreationTime DATETIME2)    
    
IF (LEN(@SerialNumber) > 0)     
BEGIN    
 INSERT INTO #SerialNumbers     
 select * from dbo.SplitString(NULLIF(@SerialNumber, ','), ',')    
END    
ELSE IF (LEN(@D365BatchNumber) > 0)     
BEGIN    
 INSERT INTO #D365BatchNumbers     
 select * from dbo.SplitString(NULLIF(@D365BatchNumber, ''), ',')    
    
 INSERT INTO #SerialNumbers    
 select Distinct  
 case when isnull(a.BatchNumber,'')='' then isnull(b.BatchNumber,'')  
    else isnull(a.BatchNumber,'') end   
    --*  
 from DOT_FloorAxIntParentTable a with(nolock)   
 left join DOT_FGJournalTable b with(nolock) on a.Id=b.ParentRefRecId and b.IsDeleted=0  
 where   
  D365BatchNumber in (select D365BatchNumber from #D365BatchNumbers where D365BatchNumber <> '') or   
  a.BatchNumber in (select D365BatchNumber from #D365BatchNumbers where D365BatchNumber <> '')  
  
 and a.IsDeleted = 0  --and IsMigratedFromAX6 = 0              
END    
Else IF (LEN(@CustReferenceNumber) > 0)     
BEGIN    
 INSERT INTO #CustReferenceNumbers     
 select * from dbo.SplitString(NULLIF(@CustReferenceNumber, ','), ',')    
    
 INSERT INTO #SerialNumbers    
 select Distinct BatchNumber from dbo.UFN_DOT_GetSNFromCustRefForSummaryBatchCardReport(@CustReferenceNumber)    
END    
    
DECLARE @TempPN TABLE (PlantNo VARCHAR(10))    
    
IF (LEN(@PlantNo) > 0)     
BEGIN    
 INSERT INTO @TempPN VALUES (@PlantNo)    
END    
    
ELSE    
BEGIN    
 INSERT INTO @TempPN select LocationName from LocationMaster    
END    
    
DECLARE @StartPostingDateTime DateTime = '2019-03-01'    
DECLARE @EndPostingDateTime DateTime = DATEADD(DAY,1,GetDate())    
DECLARE @SerialNoCount int    
SELECT @SerialNoCount = Count(1) from #SerialNumbers    
    
IF (@SerialNoCount > 0)    
BEGIN    
 --details: picking list    
 INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber,     
 TransferJournalNumber, PickingListJournalNumber, RouteCardJournalNumber, RAFJournalNumber,    
 ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier, PlantNo,    
 ReferenceBatchNumber1, ReferenceBatchNumber2, ReferenceBatchNumber3, ReferenceBatchSequence1, ReferenceBatchSequence2, ReferenceBatchSequence3,    
 RefNumberOfPieces1, RefNumberOfPieces2, RefNumberOfPieces3,CreationTime)    
    
 select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, b.PSIReworkOrderNo as BatchOrderNumber,     
 '' as MovementJournalNumber, '' as TransferJournalNumber, ISNULL(NULLIF(b.PickListJournalId,''), (select PickListJournalId from DOT_PickingSumTable with(nolock) where D365BatchNumber = a.D365BatchNumber and     
 FunctionIdentifier = a.FunctionIdentifier)) as PickListJournalNumber,      
 '' as RouteCardJournalNumber, '' as RAFJournalNumber, b.ItemNumber, b.Configuration as Size,    
 b.Warehouse, 'RWK' as Location,     
 b.OldBatchQty as TransitionQuantity,     
 NULL as ReservedQuantity, a.Sequence, a.FunctionIdentifier, a.PlantNo,    
 a.ReferenceBatchNumber1, a.ReferenceBatchNumber2, a.ReferenceBatchNumber3,    
 a.ReferenceBatchSequence1, a.ReferenceBatchSequence2, a.ReferenceBatchSequence3,    
 b.RefNumberOfPieces1, b.RefNumberOfPieces2, b.RefNumberOfPieces3,a.CreationTime    
     
 from DOT_FloorAxIntParentTable a with(nolock) join DOT_PickingList b with(nolock) on a.id = b.ParentRefRecId    
 where a.FunctionIdentifier in (case when LEN(@FunctionIdentifier) > 0 then @FunctionIdentifier else 'CBCI' end)    
 and a.BatchNumber IN (Select Distinct SerialNumber from #SerialNumbers where SerialNumber <> '')    
 and a.IsDeleted = 0 -- and a.IsMigratedFromAX6 = 0     
 and b.PostingDateTime >= case when LEN(@PostingDateTimeFrom) > 0 then @PostingDateTimeFrom else @StartPostingDateTime end    
 and b.PostingDateTime <= case when LEN(@PostingDateTimeTo) > 0 then @PostingDateTimeTo  else @EndPostingDateTime end     
 and a.PlantNo in (SELECT * FROM @TempPN)   
 
 --details: picking list change glove 1(negative qty)    
 INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber,     
 TransferJournalNumber, PickingListJournalNumber, RouteCardJournalNumber, RAFJournalNumber,    
 ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier, PlantNo,    
 ReferenceBatchNumber1, ReferenceBatchNumber2, ReferenceBatchNumber3, ReferenceBatchSequence1, ReferenceBatchSequence2, ReferenceBatchSequence3,    
 RefNumberOfPieces1, RefNumberOfPieces2, RefNumberOfPieces3,CreationTime)    
    
 select b.PostingDateTime, a.ReferenceBatchNumber1, a.D365BatchNumber, b.PSIReworkOrderNo as BatchOrderNumber,     
 '' as MovementJournalNumber, '' as TransferJournalNumber, ISNULL(NULLIF(b.PickListJournalId,''), (select PickListJournalId from DOT_PickingSumTable with(nolock) where D365BatchNumber = a.D365BatchNumber and     
 FunctionIdentifier = a.FunctionIdentifier)) as PickListJournalNumber,      
 '' as RouteCardJournalNumber, '' as RAFJournalNumber, b.ItemNumber, b.Configuration as Size,    
 dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(ReferenceBatchNumber1,ReferenceBatchSequence1) ,    
 dbo.UFN_DOT_GetLocationForSummaryBatchCard(ReferenceBatchNumber1,ReferenceBatchSequence1) ,     
 b.RefNumberOfPieces1 * -1 as TransitionQuantity,     
 NULL as ReservedQuantity, a.Sequence + 1, a.FunctionIdentifier, a.PlantNo,    
 a.ReferenceBatchNumber1, a.ReferenceBatchNumber2, a.ReferenceBatchNumber3,    
 a.ReferenceBatchSequence1, a.ReferenceBatchSequence2, a.ReferenceBatchSequence3,    
 b.RefNumberOfPieces1, b.RefNumberOfPieces2, b.RefNumberOfPieces3,a.CreationTime    
     
 from DOT_FloorAxIntParentTable a with(nolock) 
 join DOT_PickingList b with(nolock) on a.id = b.ParentRefRecId 
 join #SerialNumbers sn on a.ReferenceBatchNumber1 = sn.SerialNumber
 where a.FunctionIdentifier in (case when LEN(@FunctionIdentifier) > 0 then @FunctionIdentifier else 'CBCI' end)    
 and a.IsDeleted = 0 --and a.IsMigratedFromAX6 = 0     
 and b.PostingDateTime >= case when LEN(@PostingDateTimeFrom) > 0 then @PostingDateTimeFrom else @StartPostingDateTime end    
 and b.PostingDateTime <= case when LEN(@PostingDateTimeTo) > 0 then @PostingDateTimeTo  else @EndPostingDateTime end     
 and a.PlantNo in (SELECT * FROM @TempPN)  
 and a.ReferenceBatchNumber1 is not null
 and a.ReferenceBatchNumber1 <>'RESAMPLE'


  --details: picking list change glove 2(negative qty)    
 INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber,     
 TransferJournalNumber, PickingListJournalNumber, RouteCardJournalNumber, RAFJournalNumber,    
 ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier, PlantNo,    
 ReferenceBatchNumber1, ReferenceBatchNumber2, ReferenceBatchNumber3, ReferenceBatchSequence1, ReferenceBatchSequence2, ReferenceBatchSequence3,    
 RefNumberOfPieces1, RefNumberOfPieces2, RefNumberOfPieces3,CreationTime)    
    
 select b.PostingDateTime, a.ReferenceBatchNumber2, a.D365BatchNumber, b.PSIReworkOrderNo as BatchOrderNumber,     
 '' as MovementJournalNumber, '' as TransferJournalNumber, ISNULL(NULLIF(b.PickListJournalId,''), (select PickListJournalId from DOT_PickingSumTable with(nolock) where D365BatchNumber = a.D365BatchNumber and     
 FunctionIdentifier = a.FunctionIdentifier)) as PickListJournalNumber,      
 '' as RouteCardJournalNumber, '' as RAFJournalNumber, b.ItemNumber, b.Configuration as Size,    
 dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(ReferenceBatchNumber2,ReferenceBatchSequence2) ,    
 dbo.UFN_DOT_GetLocationForSummaryBatchCard(ReferenceBatchNumber2,ReferenceBatchSequence2) ,     
 b.RefNumberOfPieces2 * -1 as TransitionQuantity,     
 NULL as ReservedQuantity, a.Sequence + 1, a.FunctionIdentifier, a.PlantNo,    
 a.ReferenceBatchNumber1, a.ReferenceBatchNumber2, a.ReferenceBatchNumber3,    
 a.ReferenceBatchSequence1, a.ReferenceBatchSequence2, a.ReferenceBatchSequence3,    
 b.RefNumberOfPieces1, b.RefNumberOfPieces2, b.RefNumberOfPieces3,a.CreationTime    
     
 from DOT_FloorAxIntParentTable a with(nolock) 
 join DOT_PickingList b with(nolock) on a.id = b.ParentRefRecId 
 join #SerialNumbers sn on a.ReferenceBatchNumber2 = sn.SerialNumber
 where a.FunctionIdentifier in (case when LEN(@FunctionIdentifier) > 0 then @FunctionIdentifier else 'CBCI' end)    
 and a.IsDeleted = 0 --and a.IsMigratedFromAX6 = 0     
 and b.PostingDateTime >= case when LEN(@PostingDateTimeFrom) > 0 then @PostingDateTimeFrom else @StartPostingDateTime end    
 and b.PostingDateTime <= case when LEN(@PostingDateTimeTo) > 0 then @PostingDateTimeTo  else @EndPostingDateTime end     
 and a.PlantNo in (SELECT * FROM @TempPN)  
 and a.ReferenceBatchNumber2 is not null 

  --details: picking list change glove 3(negative qty)    
 INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber,     
 TransferJournalNumber, PickingListJournalNumber, RouteCardJournalNumber, RAFJournalNumber,    
 ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier, PlantNo,    
 ReferenceBatchNumber1, ReferenceBatchNumber2, ReferenceBatchNumber3, ReferenceBatchSequence1, ReferenceBatchSequence2, ReferenceBatchSequence3,    
 RefNumberOfPieces1, RefNumberOfPieces2, RefNumberOfPieces3,CreationTime)    
    
 select b.PostingDateTime, a.ReferenceBatchNumber3, a.D365BatchNumber, b.PSIReworkOrderNo as BatchOrderNumber,     
 '' as MovementJournalNumber, '' as TransferJournalNumber, ISNULL(NULLIF(b.PickListJournalId,''), (select PickListJournalId from DOT_PickingSumTable with(nolock) where D365BatchNumber = a.D365BatchNumber and     
 FunctionIdentifier = a.FunctionIdentifier)) as PickListJournalNumber,      
 '' as RouteCardJournalNumber, '' as RAFJournalNumber, b.ItemNumber, b.Configuration as Size,    
 dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(ReferenceBatchNumber3,ReferenceBatchSequence3) ,    
 dbo.UFN_DOT_GetLocationForSummaryBatchCard(ReferenceBatchNumber3,ReferenceBatchSequence3) ,     
 b.RefNumberOfPieces3 * -1 as TransitionQuantity,     
 NULL as ReservedQuantity, a.Sequence + 1, a.FunctionIdentifier, a.PlantNo,    
 a.ReferenceBatchNumber1, a.ReferenceBatchNumber2, a.ReferenceBatchNumber3,    
 a.ReferenceBatchSequence1, a.ReferenceBatchSequence2, a.ReferenceBatchSequence3,    
 b.RefNumberOfPieces1, b.RefNumberOfPieces2, b.RefNumberOfPieces3,a.CreationTime    
     
 from DOT_FloorAxIntParentTable a with(nolock) 
 join DOT_PickingList b with(nolock) on a.id = b.ParentRefRecId 
 join #SerialNumbers sn on a.ReferenceBatchNumber3 = sn.SerialNumber
 where a.FunctionIdentifier in (case when LEN(@FunctionIdentifier) > 0 then @FunctionIdentifier else 'CBCI' end)    
 and a.IsDeleted = 0 --and a.IsMigratedFromAX6 = 0     
 and b.PostingDateTime >= case when LEN(@PostingDateTimeFrom) > 0 then @PostingDateTimeFrom else @StartPostingDateTime end    
 and b.PostingDateTime <= case when LEN(@PostingDateTimeTo) > 0 then @PostingDateTimeTo  else @EndPostingDateTime end     
 and a.PlantNo in (SELECT * FROM @TempPN) 
 and a.ReferenceBatchNumber3 is not null  

END    
ELSE    
BEGIN    
 --details: picking list    
 INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber,     
 TransferJournalNumber, PickingListJournalNumber, RouteCardJournalNumber, RAFJournalNumber,    
 ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier, PlantNo,    
 ReferenceBatchNumber1, ReferenceBatchNumber2, ReferenceBatchNumber3, ReferenceBatchSequence1, ReferenceBatchSequence2, ReferenceBatchSequence3,    
 RefNumberOfPieces1, RefNumberOfPieces2, RefNumberOfPieces3,CreationTime)    
    
 select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, b.PSIReworkOrderNo as BatchOrderNumber,     
 '' as MovementJournalNumber, '' as TransferJournalNumber,     
 ISNULL(NULLIF(b.PickListJournalId,''), (select PickListJournalId from DOT_PickingSumTable with(nolock) where D365BatchNumber = a.D365BatchNumber and     
 FunctionIdentifier = a.FunctionIdentifier)) as PickListJournalNumber,     
 '' as RouteCardJournalNumber, '' as RAFJournalNumber, b.ItemNumber, b.Configuration as Size,    
 b.Warehouse, 'RWK' as Location,     
 b.OldBatchQty as TransitionQuantity,     
 NULL as ReservedQuantity, a.Sequence, a.FunctionIdentifier, a.PlantNo,    
 a.ReferenceBatchNumber1, a.ReferenceBatchNumber2, a.ReferenceBatchNumber3,    
 a.ReferenceBatchSequence1, a.ReferenceBatchSequence2, a.ReferenceBatchSequence3,    
 b.RefNumberOfPieces1, b.RefNumberOfPieces2, b.RefNumberOfPieces3,a.CreationTime    
     
 from DOT_FloorAxIntParentTable a with(nolock) join DOT_PickingList b with(nolock) on a.id = b.ParentRefRecId    
 where a.FunctionIdentifier in (case when LEN(@FunctionIdentifier) > 0 then @FunctionIdentifier else 'CBCI' end)    
 and a.IsDeleted = 0 --and a.IsMigratedFromAX6 = 0     
 and b.PostingDateTime >= case when LEN(@PostingDateTimeFrom) > 0 then @PostingDateTimeFrom else @StartPostingDateTime end    
 and b.PostingDateTime <= case when LEN(@PostingDateTimeTo) > 0 then @PostingDateTimeTo  else @EndPostingDateTime end     
 and a.PlantNo in (SELECT * FROM @TempPN)   
     
--details: picking list change SN 1    
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber,     
TransferJournalNumber, PickingListJournalNumber, RouteCardJournalNumber, RAFJournalNumber,    
ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier, PlantNo,CreationTime)    
    
select PostingDateTime, ReferenceBatchNumber1 as SerialNumber,     
dbo.UFN_DOT_GetD365BatchNoForSummaryBatchCard(ReferenceBatchNumber1) as D365BatchNumber,     
dbo.UFN_DOT_GetBOForSummaryBatchCard(ReferenceBatchNumber1) as BatchOrderNumber,     
MovementJournalNumber, TransferJournalNumber, PickingListJournalNumber, RouteCardJournalNumber, RAFJournalNumber, ItemNumber, Size,    
dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(ReferenceBatchNumber1,ReferenceBatchSequence1) as Warehouse,    
dbo.UFN_DOT_GetLocationForSummaryBatchCard(ReferenceBatchNumber1,ReferenceBatchSequence1) as Location,     
RefNumberOfPieces1 * -1 as TransitionQuantity, ReservedQuantity, Sequence + 1 as Sequence, FunctionIdentifier, PlantNo,CreationTime    
from #tmpUnion where ReferenceBatchNumber1 is not null    
    
--details: picking list change SN 2    
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber,     
TransferJournalNumber, PickingListJournalNumber, RouteCardJournalNumber, RAFJournalNumber,    
ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier,PlantNo,CreationTime)    
    
select PostingDateTime, ReferenceBatchNumber2 as SerialNumber,     
dbo.UFN_DOT_GetD365BatchNoForSummaryBatchCard(ReferenceBatchNumber2) as D365BatchNumber,     
dbo.UFN_DOT_GetBOForSummaryBatchCard(ReferenceBatchNumber2) as BatchOrderNumber,     
MovementJournalNumber, TransferJournalNumber, PickingListJournalNumber, RouteCardJournalNumber, RAFJournalNumber, ItemNumber, Size,    
dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(ReferenceBatchNumber2,ReferenceBatchSequence2) as Warehouse,    
dbo.UFN_DOT_GetLocationForSummaryBatchCard(ReferenceBatchNumber2,ReferenceBatchSequence2) as Location,     
RefNumberOfPieces2 * -1 as TransitionQuantity, ReservedQuantity, Sequence + 1 as Sequence, FunctionIdentifier,PlantNo,CreationTime    
from #tmpUnion where ReferenceBatchNumber2 is not null    
    
--details: picking list change SN 3    
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber,     
TransferJournalNumber, PickingListJournalNumber, RouteCardJournalNumber, RAFJournalNumber,    
ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier,PlantNo,CreationTime)    
    
select PostingDateTime, ReferenceBatchNumber3 as SerialNumber,     
dbo.UFN_DOT_GetD365BatchNoForSummaryBatchCard(ReferenceBatchNumber3) as D365BatchNumber,     
dbo.UFN_DOT_GetBOForSummaryBatchCard(ReferenceBatchNumber3) as BatchOrderNumber,     
MovementJournalNumber, TransferJournalNumber, PickingListJournalNumber, RouteCardJournalNumber, RAFJournalNumber, ItemNumber, Size,    
dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(ReferenceBatchNumber3,ReferenceBatchSequence3) as Warehouse,    
dbo.UFN_DOT_GetLocationForSummaryBatchCard(ReferenceBatchNumber3,ReferenceBatchSequence3) as Location,     
RefNumberOfPieces3 * -1 as TransitionQuantity, ReservedQuantity, Sequence + 1 as Sequence, FunctionIdentifier, PlantNo,CreationTime    
from #tmpUnion where ReferenceBatchNumber3 is not null   

END    
 
--add opening balance
 INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber,     
 TransferJournalNumber, PickingListJournalNumber, RouteCardJournalNumber, RAFJournalNumber,    
 ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier, PlantNo,    
 ReferenceBatchNumber1, ReferenceBatchNumber2, ReferenceBatchNumber3, ReferenceBatchSequence1, ReferenceBatchSequence2, ReferenceBatchSequence3,    
 RefNumberOfPieces1, RefNumberOfPieces2, RefNumberOfPieces3,CreationTime)    
SELECT 
	case when b.LastModificationTime is null then b.CreationTime else b.LastModificationTime end ,b.BatchCardNum   as SerialNumber, a.D365BatchNumber ,'',a.JournalId,
	'','','','',
	a.ItemNumber,'',a.Warehouse ,a.Location ,b.Quantity as TransitionQuantity,0 ,0, 'OpeningUpload' as FunctionIdentifier, '',
    '','', '', 0,0, 0,   
     0,0, 0 , null
FROM DOT_InventAdjustmentSumTable a right join DOT_InventAdjustmentSumTableDetails b on a.InventTransId = b.InventTransId
where 
	(a.D365BatchNumber in (select D365BatchNumber from #D365BatchNumbers) or 
	b.BatchCardNum in (select SerialNumber from #SerialNumbers) )
	and a.BatchNumber = 'OpeningUpload'

Select a.PlantNo, a.FunctionIdentifier, a.PostingDateTime, a.SerialNumber, a.D365BatchNumber, a.BatchOrderNumber, a.MovementJournalNumber,     
a.TransferJournalNumber, a.PickingListJournalNumber as PickingListJournalNumber, a.RouteCardJournalNumber, a.RAFJournalNumber, a.ItemNumber, a.Size, a.Warehouse,a.Location,    
 -- hide qty if not complete posting without journal id come back from d365
case when FunctionIdentifier = 'OpeningUpload' then a.TransitionQuantity when ISNULL(a.PickingListJournalNumber,'')='' then 0.0 else CONVERT(DECIMAL (18, 4),a.TransitionQuantity) end as TransitionQuantity, 
CONVERT(DECIMAL (10, 4),a.ReservedQuantity) as ReservedQuantity,CreationTime    
from #tmpUnion a    
WHERE Warehouse LIKE (case when LEN(@Warehouse) > 0 then @Warehouse  else '%' end)    
    
IF OBJECT_ID('tempdb..#SerialNumbers') IS NOT NULL      
DROP TABLE #SerialNumbers     
IF OBJECT_ID('tempdb..#D365BatchNumbers') IS NOT NULL      
DROP TABLE #D365BatchNumbers     
IF OBJECT_ID('tempdb..#CustReferenceNumbers') IS NOT NULL      
DROP TABLE #CustReferenceNumbers    
IF OBJECT_ID('tempdb..#TmpUnion') IS NOT NULL      
DROP TABLE #TmpUnion    
    
END
GO
PRINT N'Creating Procedure [dbo].[Usp_DOT_PNBC_Print_Save]...';


GO
-- ==================================================================================    
-- Name:   Usp_DOT_PNBC_Print_Save  
-- Purpose:  Save & Print Normal Batch Card  
-- ==================================================================================    
-- Change History    
-- Date    Author   Comments    
-- -----   ------   -----------------------------------------------------------------    
-- 31/12/2021  Azrul Amin    SP created.    
-- ==================================================================================    
  
CREATE PROCEDURE [dbo].[Usp_DOT_PNBC_Print_Save]    
(    
@UserId VARCHAR(10),  
@ShiftId VARCHAR(10),  
@Line VARCHAR(10),  
@BatchCardDate DATETIME,  
@ModuleId INT,  
@SubModuleID INT,  
@SiteNumber INT,  
@WorkStationNumber INT,  
@Resource VARCHAR(50),  
@BatchOrder VARCHAR(50),  
@GloveCode VARCHAR(50),  
@Size VARCHAR(50),  
@BatchWeight DECIMAL(18,3),  
@Quantity INT,
@authorizedBy NVARCHAR(25),      
@authorizedFor INT      
)     
AS    
BEGIN    
    
DECLARE @LocationId INT    
DECLARE @SerialNumber Numeric  
DECLARE @BatchNumber VARCHAR(50)  
DECLARE @GloveCategory VARCHAR(50)  
  
SELECT @LocationId = locationid from workstationmaster with (nolock) where isdeleted=0 and workstationid= @WorkStationNumber    
SELECT @SerialNumber = dbo.Ufn_SerailNumberPart(@SiteNumber,@BatchCardDate) + dbo.Ufn_IntToChar((NEXT VALUE FOR DBO.SerialNumberSeq),7)  
SELECT @BatchNumber = dbo.Ufn_BatchNumber(@BatchCardDate,@Line,@Size)  
SELECT @GloveCategory = ISNULL(dbo.Ufn_DOT_GetGloveCategory(@GloveCode),'')  
  
BEGIN TRANSACTION;    
    
BEGIN TRY    
SET NOCOUNT ON  
  
--insert into batch table  
INSERT INTO dbo.Batch(SerialNumber,BatchNumber,ShiftId,LineId,GloveType,Size,TierSide,BatchWeight,BatchCardDate,ReWorkCount,IsOnline,TotalPCs,ModuleId,SubModuleID,LocationId,BatchType,     
LastModifiedOn,WorkstationId,batchcardcurrentlocation,AuthorizedBy,AuthorizedFor)       
SELECT @SerialNumber,@BatchNumber,@ShiftId,@Line,@GloveCode,@Size,substring(@Resource,6,2) AS TierSide,@BatchWeight,@BatchCardDate,0,0,@Quantity,@ModuleId,@SubModuleID,@LocationId,'T',  
GETDATE(),@WorkStationNumber,'PN',@authorizedBy,@authorizedFor  
    
--insert batch details into staging table  
INSERT INTO dbo.DOT_FloorD365HRGLOVERPT(SeqNo,BatchCardNumber,BthOrder,CreationTime,CreatorUserId,CurrentDateandTime,DeleterUserId,DeletionTime,  
GloveCategory,GloveCode,IsDeleted,LastModificationTime,LastModifierUserId,LineId,OutTime,Plant,Resource,SerialNo,ShiftId,Size,UserID,PackingSz,InBox)  
SELECT 1,@BatchNumber,@BatchOrder,GETDATE(),1,GETDATE(),0,NULL,@GloveCategory,@GloveCode,  
0,GETDATE(),NULL,@Line,@BatchCardDate,dbo.Ufn_DOT_GetLocationName(@LocationId),@Resource,@SerialNumber,@ShiftId,@Size,1,@Quantity,1  
  
--select result for print batch card  
SELECT @SerialNumber as SerialNumber,@BatchNumber as BatchNumber,@GloveCode as GloveCode, @GloveCategory as GloveCategory,@Size as Size,@BatchCardDate as BatchCardDate,  
@Resource as Resource,@BatchWeight as BatchWeight,@Quantity as Quantity  
  
 SET NOCOUNT OFF    
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
PRINT N'Creating Procedure [dbo].[USP_DOT_PrintBatchDetails_Get]...';


GO
-- ==================================================================================  
-- Name:   USP_DOT_PrintBatchDetails_Get
-- Purpose:   Get Batch card details from Batch table for the selected Line and Hour  
-- ==================================================================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   -----------------------------------------------------------------  
-- 27/03/2018  Azrul Amin    SP created.  
-- ==================================================================================  
--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_DOT_PrintBatchDetails_Get]') AND type in (N'P', N'PC'))
--DROP PROCEDURE [dbo].[USP_DOT_PrintBatchDetails_Get]
--GO
CREATE PROCEDURE [dbo].[USP_DOT_PrintBatchDetails_Get]  
(  
@LineId NVARCHAR(5),  
@hour INT,  
@TierSide NVARCHAR(20),  
@BatchCardDate DATETIME,  
@ModuleId INT,  
@SubModuleID INT,  
@SIteNumber INT,  
@WorkStationNumber INT  
)  
AS  
BEGIN  
 DECLARE @ReprintDate DATETIME  
 DECLARE @ReprintBatchcardDate DATETIME  
 DECLARE @IsDoubleFormer bit  
 SET @ReprintDate=@BatchCardDate  
  
 DECLARE @TVBatch TABLE  
 (  
  [SerialNumber] [numeric](15, 0),[BatchNumber] [nvarchar](20),[TierSide] [nvarchar](3),[GloveType] [nvarchar](50),[Size] [nvarchar](8), [BatchCardDate]  [DATETIME]  
 )  
  
 SELECT @IsDoubleFormer= IsDoubleFormer FROM ProductionLine where LineId=@LineId  
  
 IF DATEPART(HOUR,GETDATE()) < @hour  
 BEGIN  
 SET @ReprintDate= @BatchCardDate-1  
 END  
  
 SET @ReprintBatchcardDate =CAST(CONVERT(DATE, @ReprintDate) as datetime) + CAST(CAST((CAST(@hour AS varchar)+':00') as time) AS datetime)  
 SET NOCOUNT ON  
   
  ;with CT As  
  (     
   SELECT SerialNumber,BatchNumber,TierSide,ct.String,GloveType,Size,len(TierSide) as TierSideLen,b.BatchCardDate FROM BATCH b  
   JOIN dbo.ufn_CSVToTable(@TierSide) ct ON LEFT(b.TierSide,1)= LEFT(ct.String,1)   
   WHERE   
   CONVERT(DATE, b.BatchCardDate)= CONVERT(DATE, @ReprintDate) AND     
   DATEPART(HOUR,b.BatchCardDate) = @hour AND b.LineId=@LineId   
  )  
  INSERT INTO @TVBatch(SerialNumber,BatchNumber,TierSide,GloveType,Size,BatchCardDate)  
  SELECT DISTINCT  SerialNumber,BatchNumber,TierSide,GloveType,Size,BatchCardDate FROM CT WHERE right(CT.TierSide,1)= CASE  CT.TierSideLen WHEN 1 THEN left(CT.String,1) ELSE right(CT.String,1) END  
  
  IF NOT EXISTS(SELECT * FROM @TVBatch)  
   BEGIN    
    EXEC Usp_DOT_HBC_Print_Save  @ReprintBatchcardDate,@ModuleId,@SubModuleID,@LineId,@TierSide,@SIteNumber,@WorkStationNumber  
   END  
  ELSE  
   BEGIN  
    SELECT * FROM @TVBatch  
   END  
  END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_ReprintMTS_FGBO_Selection]...';


GO
-- =============================================    
-- Author:  Muhammad Khalid  
-- Create date: 4 June 2018    
-- Description: Get Active First Grade PO without SO list      
-- =====================================================================      
CREATE PROCEDURE [dbo].[USP_DOT_ReprintMTS_FGBO_Selection]    
AS    
BEGIN    
  
 SET NOCOUNT ON;    
    -- Insert statements for procedure here    
  SELECT a.BthOrderId, a.ProdStatus,c.ItemType, a.BatchId as CustomerRef,a.QtySched,fp.CasesPacked    
  from DOT_FloorD365BO a   
  -- left join DOT_FloorSales b on a.BatchId = b.CustomerRef   
  left join DOT_FSItemMaster c on a.ItemId = c.ItemId  
   left join  (select sum(fp.CasesPacked) as CasesPacked,bo2.BthOrderId from finalpacking FP WITH (NOLOCK)             
     join purchaseorderitem POIN WITH (NOLOCK) on POIN.ponumber = FP.ponumber and POIN.Itemnumber = FP.itemnumber and FP.Size = POIN.ItemSize            
     join DOT_FloorD365BO bo2 WITH (NOLOCK) on bo2.BthOrderId = fp.FGBatchOrderNo and FP.Size = bo2.Size and bo2.IsDeleted=0            
     WHERE bo2.ProdStatus IN ('ReportedFinished','StartedUp','Released') group by bo2.BthOrderId) as fp          
    on fp.BthOrderId = a.BthOrderId     
  WHERE -- b.CustomerRef is null   
  a.prodPoolId = 'FG' and a.WarehouseId = 'MTS-FG' -- warehouse = “MTS-FG”, Production Pool = “FG” are Made to stock batch order, update on 22/11/2021,Max He  
  and a.ProdStatus = 'StartedUp' and  c.ItemType = 5 --=>'FG'  
  and a.ReworkBatch = 'No' and a.IsDeleted=0  and fp.CasesPacked is not null
  GROUP By a.BthOrderId,a.ProdStatus, a.BatchId, c.ItemType,       
   a.QtySched,        
   fp.CasesPacked       
  having a.QtySched - ISNULL(fp.CasesPacked,0) > 0 --filter out full packed FGBO       
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_ReprintSRBCResource_Get]...';


GO
-- =========================================================  
-- Name:   USP_DOT_ReprintSRBCResource_Get
-- Purpose:   Get Resource for Reprint SRBC
-- =========================================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   ----------------------------------------  
-- 11/05/2018  Azrul Amin    SP created.  
--exec USP_ReprintSRBCResource_Get '23-Oct-20 2:17:45 PM', 10047, '', '',''
--exec USP_ReprintSRBCResource_Get '23-Oct-20 12:25:03 PM', 10047, 'L79', '',''
--exec USP_ReprintSRBCResource_Get '23-Oct-20 12:25:03 PM', 10047, 'L79', 'P7L79B1',''
--exec USP_ReprintSRBCResource_Get '23-Oct-20 12:25:03 PM', 10047, 'L79', 'P7L79B1','HNBON000312958'
-- =========================================================  

CREATE   PROCEDURE [dbo].[USP_DOT_ReprintSRBCResource_Get]  
(
 @outputTime datetime,
 @workstationId int,
 @line varchar(20),
 @resource varchar(20),
 @bo varchar(20)
) 
AS  
BEGIN   
 SET NOCOUNT ON;  
	DECLARE @LocationId int
	DECLARE @plantno varchar(10)
	SELECT @LocationId = locationid from workstationmaster with (nolock) where isdeleted=0 and workstationid= @workstationId
	SELECT @plantno = dbo.Ufn_DOT_GetLocationName(@LocationId)

 IF (@line IS NULL) OR (LEN(@line) = 0) 
 BEGIN
	SELECT distinct LineId as ddlVal from DOT_FloorD365HRGLOVERPT  WITH (NOLOCK) where (DATEDIFF(d, CreationTime, @outputTime) = 0) and Plant = @plantno
 END
 ELSE IF (@resource IS NULL) OR (LEN(@resource) = 0) 
 BEGIN
	SELECT distinct Resource as ddlVal from DOT_FloorD365HRGLOVERPT  WITH (NOLOCK) where (DATEDIFF(d, CreationTime, @outputTime) = 0) and Plant = @plantno
	and LineId = @line
 END
 ELSE IF (@bo IS NULL) OR (LEN(@bo) = 0) 
 BEGIN
	SELECT distinct BthOrder as ddlVal from DOT_FloorD365HRGLOVERPT  WITH (NOLOCK) where (DATEDIFF(d, CreationTime, @outputTime) = 0) and Plant = @plantno
	and LineId = @line and @resource = @resource
 END
 ELSE
 BEGIN
	SELECT SerialNo as ddlVal from DOT_FloorD365HRGLOVERPT  WITH (NOLOCK) where (DATEDIFF(d, CreationTime, @outputTime) = 0) and Plant = @plantno
	and LineId = @line and Resource = @resource and BthOrder = @bo
 END

 SET NOCOUNT OFF;  
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_ResourceNBODetails_Get]...';


GO
-- =======================================================================  
-- Name:   USP_DOT_ResourceNBODetails_Get
-- Purpose:   Get Resource and Batch Order details for Glove Output Screen 
-- =======================================================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   ------------------------------------------------------  
-- 13/03/2018  Azrul Amin    SP created.  
-- 01/07/2021  Azrul Amin	 Cater Plant10 and onwards. 
-- 06/12/2021  Azrul Amin	 HTLG_HSB_002: Special Glove - PTPF, CLRP & SCON not allowed to print HBC/SRBC. 
-- =======================================================================  
--exec [dbo].[USP_DOT_ResourceNBODetails_Get] 6,'L47','P6L47LT','NULL',null,null,null,null
 
CREATE PROCEDURE [dbo].[USP_DOT_ResourceNBODetails_Get]   
(   
 @LocationId Int,   
 @LineId varchar(20),  
 @Resource varchar(20),   
 @BO varchar(20),  
 @ResFilter1 varchar(20),  
 @ResFilter2 varchar(20),  
 @ResFilter3 varchar(20),  
 @ResFilter4 varchar(20)  
)   
AS  
BEGIN     
 SET NOCOUNT ON;  
  
 -- #1.To list out all Resources Group (lines).  
 IF (@LineId IS NULL) OR (LEN(@LineId) < 0)   
 BEGIN  
  SELECT DISTINCT  
  '' as Resource  
  ,'' as ResourceId  
  ,'' as LocationId  
  ,'' as Plant  
  ,dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) as LineId  
  ,res.ResourceGrp as Line  
  ,'' as TierSide  
  ,'' as BatchOrder  
  ,'' as GloveCode  
  ,'' as Size  
  FROM   
   DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
   DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName JOIN
   DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND bo.ProdStatus = 'StartedUp'   
  AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) in (select linenumber from linemaster where LocationId = @LocationId) --#AZRUL-BUG 1179: Remove Invalid Lines In Glove Output Reporting.  
  AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon','PTPF') --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
 END  
  
-- #2.To list out all Resources (TierSide).  
ELSE IF ((@Resource IS NULL) OR (LEN(@Resource) = 0))  
 BEGIN  
  IF ((@BO IS NULL) OR (LEN(@BO) = 0))  
  BEGIN  
   SELECT DISTINCT  
   res.Resource  
   ,'' as ResourceId  
   ,'' as LocationId  
   ,'' as Plant  
   ,dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) as LineId  
   ,res.ResourceGrp as Line  
   ,dbo.UDF_DOT_GetTierSidefromResource(res.Resource) as TierSide  
   ,'' as BatchOrder  
   ,'' as GloveCode  
   ,'' as Size  
   FROM   
    DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
    DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
    LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName JOIN
    DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
   WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) = @LineId AND bo.ProdStatus = 'StartedUp'   
   AND res.Resource NOT IN (ISNULL(@ResFilter1,''),ISNULL(@ResFilter2,''),ISNULL(@ResFilter3,''),ISNULL(@ResFilter4,''))  
   and bo.IsDeleted=0 AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon','PTPF')  --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  END  
  ELSE  
  BEGIN  
   SELECT DISTINCT  
   res.Resource  
   ,'' as ResourceId  
   ,'' as LocationId  
   ,'' as Plant  
   ,dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) as LineId  
   ,res.ResourceGrp as Line  
   ,dbo.UDF_DOT_GetTierSidefromResource(res.Resource) as TierSide  
   ,'' as BatchOrder  
   ,'' as GloveCode  
   ,'' as Size  
   FROM   
    DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
    DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
    LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName JOIN
    DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
   WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) = @LineId AND bo.ProdStatus = 'StartedUp'   
   AND res.Resource NOT IN (ISNULL(@ResFilter1,''),ISNULL(@ResFilter2,''),ISNULL(@ResFilter3,''),ISNULL(@ResFilter4,''))  
   and bo.IsDeleted=0 and bo.BthOrderId = @BO AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon','PTPF')   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  END  
 END  
  
-- #3.To list out all Batch Order based on selected Resource.  
ELSE IF ((@BO IS NULL) OR (LEN(@BO) = 0))  
 BEGIN  
  SELECT   
  res.Resource  
  ,res.Id as ResourceId  
  ,loc.LocationId  
  ,dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) as Plant  
  ,dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) as LineId  
  ,res.ResourceGrp as Line  
  ,dbo.UDF_DOT_GetTierSidefromResource(res.Resource) as TierSide  
  ,bo.BthOrderId as BatchOrder  
  ,bo.ItemId as GloveCode  
  ,bo.Size  
  FROM   
   DOT_FloorD365BO (NOLOCK) AS bo LEFT JOIN  
   DOT_FloorD365BOResource (NOLOCK) AS res ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName JOIN
   DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) = @LineId  
  AND res.Resource = @Resource AND bo.ProdStatus = 'StartedUp'  
  and bo.IsDeleted=0 AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon','PTPF')   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
 END  
  
-- #4.To list out all Batch Order Details based on selected Resource and Batch Order.  
ELSE  
 BEGIN  
  SELECT   
  res.Resource  
  ,res.Id as ResourceId  
  ,loc.LocationId  
  ,dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) as Plant  
  ,dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) as LineId  
  ,res.ResourceGrp as Line  
  ,dbo.UDF_DOT_GetTierSidefromResource(res.Resource) as TierSide  
  ,bo.BthOrderId as BatchOrder  
  ,bo.ItemId as GloveCode  
  ,bo.Size  
  FROM   
   DOT_FloorD365BO AS bo  WITH (NOLOCK) LEFT JOIN  
   DOT_FloorD365BOResource  AS res  WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName JOIN
   DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) = @LineId   
  AND res.Resource = @Resource AND bo.ProdStatus = 'StartedUp' AND bo.BthOrderId = @BO  
  and bo.IsDeleted=0 AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon','PTPF')  --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC. 
 END  
 SET NOCOUNT OFF;    
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_SEL_ChangeGloveType]...';


GO
-- =============================================  
-- Name:   [dbo].[USP_DOT_SEL_ChangeGloveType]
-- Purpose:   <Get the ToChangeGloveType based on FromGloveType>  
-- =============================================  
-- Change History  
-- Date   Author    Comments  
-- -----  ------    ------------  
-- 18/11/2021  Azrul  SP created  
-- =============================================  
CREATE PROCEDURE [dbo].[USP_DOT_SEL_ChangeGloveType]  
@gloveType NVARCHAR(50)  
AS  
BEGIN  
 SET NOCOUNT ON;     
	SELECT ToGloveCode FROM DOT_ChangeGloveCode with (nolock) WHERE FROMGLOVECODE = @gloveType AND STOPPED <> 1  
 SET NOCOUNT OFF;   
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_SEL_ChangeGloveTypeBatchOrder]...';


GO
-- =============================================  
-- Name:   [dbo].[USP_DOT_SEL_ChangeGloveTypeBatchOrder]
-- Purpose:   <Get Batch Order Number from Glove Code>  
-- =============================================  
-- Change History  
-- Date   Author    Comments  
-- -----  ------    ------------  
-- 18/11/2021  Azrul  SP created  
-- exec USP_DOT_SEL_ChangeGloveTypeBatchOrder 'P6','NB-AB-CRP-70-EC-LBLU-KCL' 
-- =============================================  
CREATE PROCEDURE [dbo].[USP_DOT_SEL_ChangeGloveTypeBatchOrder]  
@location NVARCHAR(10),
@gloveType NVARCHAR(50),
@size NVARCHAR(10)
AS  
BEGIN  
 SET NOCOUNT ON;     
	SELECT BthOrderId as BatchOrder FROM DOT_FloorD365BO with (nolock) WHERE ItemId = @gloveType AND Size = @size
	AND IsDeleted = 0 and ProdStatus = 'StartedUp'
 SET NOCOUNT OFF;   
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_SOBCSummaryBatchCardReport]...';


GO
-- ==================================================================================================================================================    
-- Name: USP_DOT_HBCSummaryBatchCardReport  
-- Purpose: Open Batch Card Summarization Second Grade Batch (SOBC) 
-- ==================================================================================================================================================    
-- Change History    
-- Date			Author   Comments    
-- -----		------   ------------------------------------------------------------  
-- 2021/10/26   Azrul    SP created.    
-- ==================================================================================================================================================   
-- exec USP_DOT_SOBCSummaryBatchCardReport '2211289819,2211290110,2210559670', '', '', '2021-01-01','2021-10-01'
-- exec USP_DOT_SOBCSummaryBatchCardReport '2211289819,2211290110,2210559670', '', '', '',''
-- exec USP_DOT_SOBCSummaryBatchCardReport '', '', 'ANSL 301/20', '',''
-- exec USP_DOT_SOBCSummaryBatchCardReport '', '', 'MEDU 806/21', '',''
-- exec USP_DOT_SOBCSummaryBatchCardReport '', '', '', '2021-08-01','2021-08-31'
-- ==================================================================================================================================================

Create   PROCEDURE USP_DOT_SOBCSummaryBatchCardReport  
(
@SerialNumber NVARCHAR(4000),
@D365BatchNumber NVARCHAR(4000),
@CustReferenceNumber NVARCHAR(4000),
@PostingDateTimeFrom NVARCHAR(100),
@PostingDateTimeTo NVARCHAR(100)
)

AS
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;
 
IF OBJECT_ID('tempdb..#SerialNumbers') IS NOT NULL  
DROP TABLE #SerialNumbers 
IF OBJECT_ID('tempdb..#D365BatchNumbers') IS NOT NULL  
DROP TABLE #D365BatchNumbers 
IF OBJECT_ID('tempdb..#CustReferenceNumbers') IS NOT NULL  
DROP TABLE #CustReferenceNumbers
IF OBJECT_ID('tempdb..#TmpUnion') IS NOT NULL  
DROP TABLE #TmpUnion

CREATE TABLE #SerialNumbers (SerialNumber NVARCHAR(4000))
CREATE TABLE #D365BatchNumbers (D365BatchNumber NVARCHAR(4000))
CREATE TABLE #CustReferenceNumbers (CustReferenceNumber NVARCHAR(4000))
CREATE TABLE #tmpUnion (PostingDateTime DATETIME2, 
						SerialNumber NVARCHAR(50), 
						D365BatchNumber NVARCHAR(50), 
						BatchOrderNumber NVARCHAR(50), 
						MovementJournalNumber NVARCHAR(50), 
						TransferJournalNumber NVARCHAR(50), 
						ItemNumber NVARCHAR(100), 
						Size NVARCHAR(10), 
						Warehouse NVARCHAR(10), 
						Location NVARCHAR(10), 
						TransitionQuantity DECIMAL DEFAULT 0, 
						ReservedQuantity DECIMAL, 
						BaseQuantity DECIMAL DEFAULT 0, 
						Sequence INT, 
						FunctionIdentifier NVARCHAR(50),
						ReferenceBatchNumber1 NVARCHAR(50) DEFAULT NULL, 
						ReferenceBatchNumber2 NVARCHAR(50) DEFAULT NULL, 
						ReferenceBatchNumber3 NVARCHAR(50) DEFAULT NULL, 
						ReferenceBatchNumber4 NVARCHAR(50) DEFAULT NULL, 
						ReferenceBatchNumber5 NVARCHAR(50) DEFAULT NULL,
						ReferenceBatchSequence1 INT DEFAULT 0, 
						ReferenceBatchSequence2 INT DEFAULT 0, 
						ReferenceBatchSequence3 INT DEFAULT 0, 
						ReferenceBatchSequence4 INT DEFAULT 0, 
						ReferenceBatchSequence5 INT DEFAULT 0,
						RefNumberOfPieces1 DECIMAL DEFAULT 0, 
						RefNumberOfPieces2 DECIMAL DEFAULT 0, 
						RefNumberOfPieces3 DECIMAL DEFAULT 0, 
						RefNumberOfPieces4 DECIMAL DEFAULT 0, 
						RefNumberOfPieces5 DECIMAL DEFAULT 0,
						RefItemNumber1 NVARCHAR(50) DEFAULT NULL, 
						RefItemNumber2 NVARCHAR(50) DEFAULT NULL, 
						RefItemNumber3 NVARCHAR(50) DEFAULT NULL, 
						RefItemNumber4 NVARCHAR(50) DEFAULT NULL, 
						RefItemNumber5 NVARCHAR(50) DEFAULT NULL,)

IF (LEN(@SerialNumber) > 0) 
BEGIN
	INSERT INTO #SerialNumbers 
	select * from dbo.SplitString(NULLIF(@SerialNumber, ','), ',')
END
ELSE IF (LEN(@D365BatchNumber) > 0) 
BEGIN
	INSERT INTO #D365BatchNumbers 
	select * from dbo.SplitString(NULLIF(@D365BatchNumber, ''), ',')

	INSERT INTO #SerialNumbers
	select BatchNumber from DOT_FloorAxIntParentTable with(nolock) 
	where D365BatchNumber in (select D365BatchNumber from #D365BatchNumbers where D365BatchNumber <> '')
	and IsDeleted = 0  and IsMigratedFromAX6 = 0   
END
Else IF (LEN(@CustReferenceNumber) > 0) 
BEGIN
	INSERT INTO #CustReferenceNumbers 
	select * from dbo.SplitString(NULLIF(@CustReferenceNumber, ','), ',')

	INSERT INTO #SerialNumbers
	select BatchNumber from dbo.UFN_DOT_GetSNFromCustRefForSummaryBatchCardReport(@CustReferenceNumber)
END

DECLARE @StartPostingDateTime DateTime = '2019-03-01'
DECLARE @EndPostingDateTime DateTime = DATEADD(DAY,1,GetDate())
DECLARE @SerialNoCount int
SELECT @SerialNoCount = Count(1) from #SerialNumbers

IF (@SerialNoCount > 0)
BEGIN
	--details: RAF SOBC
	INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
	TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

	select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
	ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
	b.Warehouse, b.Location, b.RAFGoodQty as TransitionQuantity, NULL as ReservedQuantity, 
	a.Sequence, a.FunctionIdentifier
	from DOT_FloorAxIntParentTable a with(nolock) join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
	where a.FunctionIdentifier = 'SOBC'
	and a.BatchNumber IN (Select Distinct SerialNumber from #SerialNumbers where SerialNumber <> '')
	and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 

	--details: Rejected SOBC
	INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
	TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

	select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
	ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
	b.Warehouse, b.Location, b.RejectedQuantity * -1 as TransitionQuantity, NULL as ReservedQuantity, 
	a.Sequence, a.FunctionIdentifier + '-Rejected' as FunctionIdentifier
	from DOT_FloorAxIntParentTable a with(nolock) join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
	where a.FunctionIdentifier in ('SOBC') and b.RejectedQuantity > 0
	and a.BatchNumber IN (Select Distinct SerialNumber from #SerialNumbers where SerialNumber <> '')
	and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 

	--details: Rejected Sample SOBC
	INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
	TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

	select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
	ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
	b.Warehouse, b.Location, b.RejectedSampleQuantity * -1 as TransitionQuantity, NULL as ReservedQuantity, 
	a.Sequence, a.FunctionIdentifier + '-RejectedSample' as FunctionIdentifier
	from DOT_FloorAxIntParentTable a with(nolock) join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
	where a.FunctionIdentifier in ('SOBC') and b.RejectedSampleQuantity > 0
	and a.BatchNumber IN (Select Distinct SerialNumber from #SerialNumbers where SerialNumber <> '')
	and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 

	--details: 2nd grade SOBC
	INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
	TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

	select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
	ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
	b.Warehouse, b.Location, b.SecondGradeQuantity * -1 as TransitionQuantity, NULL as ReservedQuantity, 
	a.Sequence, a.FunctionIdentifier + '-SecondGrade' as FunctionIdentifier
	from DOT_FloorAxIntParentTable a with(nolock) join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
	where a.FunctionIdentifier in ('SOBC') and b.SecondGradeQuantity > 0
	and a.BatchNumber IN (Select Distinct SerialNumber from #SerialNumbers where SerialNumber <> '')
	and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 
END
ELSE
BEGIN
	--details: RAF SOBC
	INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
	TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

	select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
	ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
	b.Warehouse, b.Location, b.RAFGoodQty as TransitionQuantity, NULL as ReservedQuantity, 
	a.Sequence, a.FunctionIdentifier
	from DOT_FloorAxIntParentTable a with(nolock) join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
	where a.FunctionIdentifier = 'SOBC'
	and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0
	and b.PostingDateTime >= case when LEN(@PostingDateTimeFrom) > 0 then @PostingDateTimeFrom else @StartPostingDateTime end
	and b.PostingDateTime <= case when LEN(@PostingDateTimeTo) > 0 then @PostingDateTimeTo  else @EndPostingDateTime end 

	--details: Rejected SOBC
	INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
	TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

	select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
	ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
	b.Warehouse, b.Location, b.RejectedQuantity * -1 as TransitionQuantity, NULL as ReservedQuantity, 
	a.Sequence, a.FunctionIdentifier + '-Rejected' as FunctionIdentifier
	from DOT_FloorAxIntParentTable a with(nolock) join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
	where a.FunctionIdentifier in ('SOBC') and b.RejectedQuantity > 0
	and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0
	and b.PostingDateTime >= case when LEN(@PostingDateTimeFrom) > 0 then @PostingDateTimeFrom else @StartPostingDateTime end
	and b.PostingDateTime <= case when LEN(@PostingDateTimeTo) > 0 then @PostingDateTimeTo  else @EndPostingDateTime end 

	--details: Rejected Sample SOBC
	INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
	TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

	select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
	ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
	b.Warehouse, b.Location, b.RejectedSampleQuantity * -1 as TransitionQuantity, NULL as ReservedQuantity, 
	a.Sequence, a.FunctionIdentifier + '-RejectedSample' as FunctionIdentifier
	from DOT_FloorAxIntParentTable a with(nolock) join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
	where a.FunctionIdentifier in ('SOBC') and b.RejectedSampleQuantity > 0
	and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0
	and b.PostingDateTime >= case when LEN(@PostingDateTimeFrom) > 0 then @PostingDateTimeFrom else @StartPostingDateTime end
	and b.PostingDateTime <= case when LEN(@PostingDateTimeTo) > 0 then @PostingDateTimeTo  else @EndPostingDateTime end 

	--details: 2nd grade SOBC
	INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
	TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

	select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
	ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
	b.Warehouse, b.Location, b.SecondGradeQuantity * -1 as TransitionQuantity, NULL as ReservedQuantity, 
	a.Sequence, a.FunctionIdentifier + '-SecondGrade' as FunctionIdentifier
	from DOT_FloorAxIntParentTable a with(nolock) join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
	where a.FunctionIdentifier in ('SOBC') and b.SecondGradeQuantity > 0
	and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 
	and b.PostingDateTime >= case when LEN(@PostingDateTimeFrom) > 0 then @PostingDateTimeFrom else @StartPostingDateTime end
	and b.PostingDateTime <= case when LEN(@PostingDateTimeTo) > 0 then @PostingDateTimeTo  else @EndPostingDateTime end
END

Select * from #tmpUnion

IF OBJECT_ID('tempdb..#SerialNumbers') IS NOT NULL  
DROP TABLE #SerialNumbers 
IF OBJECT_ID('tempdb..#D365BatchNumbers') IS NOT NULL  
DROP TABLE #D365BatchNumbers 
IF OBJECT_ID('tempdb..#CustReferenceNumbers') IS NOT NULL  
DROP TABLE #CustReferenceNumbers
IF OBJECT_ID('tempdb..#TmpUnion') IS NOT NULL  
DROP TABLE #TmpUnion

END
GO
PRINT N'Creating Procedure [dbo].[Usp_DOT_SRBC_Print_Save]...';


GO
-- ==================================================================================    
-- Name:   Usp_DOT_SRBC_Print_Save  
-- Purpose:  Save & Print Surgical Batch Card  
-- ==================================================================================    
-- Change History    
-- Date    Author   Comments    
-- -----   ------   -----------------------------------------------------------------    
-- 19/10/2020  Azrul Amin    SP created.    
-- 23/12/2021  Azrul Amin    AuthorizedBy/For column updated.    
-- ==================================================================================    
  
CREATE PROCEDURE [dbo].[Usp_DOT_SRBC_Print_Save]    
(    
@UserId VARCHAR(10),  
@ShiftId VARCHAR(10),  
@Line VARCHAR(10),  
@BatchCardDate DATETIME,  
@ModuleId INT,  
@SubModuleID INT,  
@SiteNumber INT,  
@WorkStationNumber INT,  
@Resource VARCHAR(50),  
@BatchOrder VARCHAR(50),  
@GloveCode VARCHAR(50),  
@Size VARCHAR(50),  
@BatchWeight DECIMAL(18,3),  
@Quantity INT,
@authorizedBy NVARCHAR(25),      
@authorizedFor INT      
)     
AS    
BEGIN    
    
DECLARE @LocationId INT    
DECLARE @SerialNumber Numeric  
DECLARE @BatchNumber VARCHAR(50)  
DECLARE @GloveCategory VARCHAR(50)  
  
SELECT @LocationId = locationid from workstationmaster with (nolock) where isdeleted=0 and workstationid= @WorkStationNumber    
SELECT @SerialNumber = dbo.Ufn_SerailNumberPart(@SiteNumber,@BatchCardDate) + dbo.Ufn_IntToChar((NEXT VALUE FOR DBO.SerialNumberSeq),7)  
SELECT @BatchNumber = dbo.Ufn_BatchNumber(@BatchCardDate,@Line,@Size)  
SELECT @GloveCategory = ISNULL(dbo.Ufn_DOT_GetGloveCategory(@GloveCode),'')  
  
BEGIN TRANSACTION;    
    
BEGIN TRY    
SET NOCOUNT ON  
  
--insert into batch table  
INSERT INTO dbo.Batch(SerialNumber,BatchNumber,ShiftId,LineId,GloveType,Size,TierSide,BatchWeight,BatchCardDate,ReWorkCount,IsOnline,TotalPCs,ModuleId,SubModuleID,LocationId,BatchType,     
LastModifiedOn,WorkstationId,batchcardcurrentlocation,AuthorizedBy,AuthorizedFor)       
SELECT @SerialNumber,@BatchNumber,@ShiftId,@Line,@GloveCode,@Size,substring(@Resource,6,2) AS TierSide,@BatchWeight,@BatchCardDate,0,1,@Quantity,@ModuleId,@SubModuleID,@LocationId,'T',  
GETDATE(),@WorkStationNumber,'PN',@authorizedBy,@authorizedFor  
    
--insert batch details into staging table  
INSERT INTO dbo.DOT_FloorD365HRGLOVERPT(SeqNo,BatchCardNumber,BthOrder,CreationTime,CreatorUserId,CurrentDateandTime,DeleterUserId,DeletionTime,  
GloveCategory,GloveCode,IsDeleted,LastModificationTime,LastModifierUserId,LineId,OutTime,Plant,Resource,SerialNo,ShiftId,Size,UserID,PackingSz,InBox)  
SELECT 1,@BatchNumber,@BatchOrder,GETDATE(),1,GETDATE(),0,NULL,@GloveCategory,@GloveCode,  
0,GETDATE(),NULL,@Line,@BatchCardDate,dbo.Ufn_DOT_GetLocationName(@LocationId),@Resource,@SerialNumber,@ShiftId,@Size,1,@Quantity,1  
  
--select result for print batch card  
SELECT @SerialNumber as SerialNumber,@BatchNumber as BatchNumber,@GloveCode as GloveCode, @GloveCategory as GloveCategory,@Size as Size,@BatchCardDate as BatchCardDate,  
@Resource as Resource,@BatchWeight as BatchWeight,@Quantity as Quantity  
  
 SET NOCOUNT OFF    
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
    
  --test  
  --exec Usp_DOT_SRBC_Print_Save 1, 2, 'L79', '21-Oct-20 4:55:53 PM',19,125,2,10047,'P7L79B1','HNBON000312958','NR-HS-OLPD-105-EC-NATL-CFDA','5 ½ L',12.332,1111  
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_SRBC_ReprintBatchCard_Save]...';


GO

-- =============================================
-- Name:			USP_DOT_SRBC_ReprintBatchCard_Save
-- Purpose: 		SRBC ReprintBatchCard Details
-- =============================================
-- Change History
-- Date    Author   Comments
-- -----   ------   -----------------------------
-- 23/10/2020 	Azrul			   SP created.
-- =============================================

CREATE   PROCEDURE [dbo].[USP_DOT_SRBC_ReprintBatchCard_Save]
(
@SerialNumber		NVARCHAR(3000),
@ReprintDateTime	DATETIME,
@OperatorId			NVARCHAR(20),
@ReasonId			INT,
@WorkStationName    NVARCHAR(150)
)
AS
BEGIN	
DECLARE  @LocationId INT
DECLARE  @ProcessArea NVARCHAR(20)
DECLARE  @BatchType NVARCHAR(20)
BEGIN TRANSACTION;
BEGIN TRY
	SELECT @LocationId=LocationId FROM WorkStationMaster c WHERE WorkStationName=@WorkStationName
	SELECT  @ProcessArea=Area FROM LocationMaster c WHERE LocationId=@LocationId

	INSERT INTO ReprintBatchCard(SerialNumber,OperatorId,ReprintDateTime,ReasonId,ProcessArea,PrintDatetime,LocationId,ReprintHour)
	SELECT SerialNumber,@OperatorId,@ReprintDateTime,@ReasonId,@ProcessArea,BatchCardDate,@LocationId,convert(TIME,@ReprintDateTime) FROM BATCH WITH (NOLOCK)
	JOIN dbo.ufn_CSVToTable(@SerialNumber) ct ON SerialNumber= ct.String	
	
	SELECT a.SerialNumber,a.BatchNumber,a.GloveType, ISNULL(dbo.Ufn_DOT_GetGloveCategory(a.GloveType),'') as GloveCategory, a.Size,a.BatchCardDate,
	b.Resource, a.BatchWeight,a.TotalPCs as TotalQty
	FROM batch a WITH (NOLOCK) JOIN DOT_FloorD365HRGLOVERPT b WITH (NOLOCK) ON a.serialnumber = b.serialNo
	WHERE a.LocationId = @LocationId and a.SerialNumber = @SerialNumber 

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
PRINT N'Creating Procedure [dbo].[USP_DOT_SummaryBatchCardReport]...';


GO
-- ==================================================================================================================================================    
-- Name: USP_DOT_SummaryBatchCardReport  
-- Purpose: Open Batch Card Summarization Report   
-- ==================================================================================================================================================    
-- Change History    
-- Date			Author   Comments    
-- -----		------   ------------------------------------------------------------  
-- 2021/09/27   Azrul    SP created.    
-- Any serial number appeared in web admin before cut off, it will classified as open batch card. 
-- PWTBCA, PVTBCA, SGBC, ON2G and OREJ will not update in Batch Card report.
-- Batch Card report will not sum up any quantity field.
-- Batch Card report will exclude RAF quantity for sample quantity.
-- RWKCR Location get from previous FunctionIdentifier.
-- List down SOBC if got Rejected, RejectedSample and 2nd grade quantity.
-- List down glove quantity details for FinalPack.
-- List down all related change SerialNumber for Change Batch Card Inner (CBCI).
-- Set offline batch RWKCR to QAI.
-- ==================================================================================================================================================   
-- exec USP_DOT_SummaryBatchCardReport '2210605283,2210605145,2210605001,2210605292,2210605426', '', '', '2021-01-01','2021-10-01'
-- exec USP_DOT_SummaryBatchCardReport '', '', 'MEDU 806/21', '2021-01-01','2021-10-21'
-- exec USP_DOT_SummaryBatchCardReport '', '', 'MEDU 806/21', '',''
-- exec USP_DOT_SummaryBatchCardReport '2210605283', '', '', '2021-08-01','2021-08-31'
-- exec USP_DOT_SummaryBatchCardReport '2210605283', '', '', '',''
-- exec USP_DOT_SummaryBatchCardReport '', '', '', '2021-08-01','2021-08-31'
-- ==================================================================================================================================================

Create   PROCEDURE USP_DOT_SummaryBatchCardReport  
(
@SerialNumber NVARCHAR(4000),
@D365BatchNumber NVARCHAR(4000),
@CustReferenceNumber NVARCHAR(4000),
@PostingDateTimeFrom NVARCHAR(100),
@PostingDateTimeTo NVARCHAR(100)
)

AS
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;
 
IF OBJECT_ID('tempdb..#SerialNumbers') IS NOT NULL  
DROP TABLE #SerialNumbers 
IF OBJECT_ID('tempdb..#D365BatchNumbers') IS NOT NULL  
DROP TABLE #D365BatchNumbers 
IF OBJECT_ID('tempdb..#CustReferenceNumbers') IS NOT NULL  
DROP TABLE #CustReferenceNumbers
IF OBJECT_ID('tempdb..#tmpParent') IS NOT NULL  
DROP TABLE #tmpParent  
IF OBJECT_ID('tempdb..#tmpUnion') IS NOT NULL  
DROP TABLE #tmpUnion 
IF OBJECT_ID('tempdb..#tmpParentSMBP1') IS NOT NULL  
DROP TABLE #tmpParentSMBP1  
IF OBJECT_ID('tempdb..#tmpParentSMBP2') IS NOT NULL  
DROP TABLE #tmpParentSMBP2  
IF OBJECT_ID('tempdb..#tmpParentSMBP3') IS NOT NULL  
DROP TABLE #tmpParentSMBP3  
IF OBJECT_ID('tempdb..#tmpParentSMBP4') IS NOT NULL  
DROP TABLE #tmpParentSMBP4  
IF OBJECT_ID('tempdb..#tmpParentSMBP5') IS NOT NULL  
DROP TABLE #tmpParentSMBP5  

CREATE TABLE #SerialNumbers (SerialNumber NVARCHAR(4000))
CREATE TABLE #D365BatchNumbers (D365BatchNumber NVARCHAR(4000))
CREATE TABLE #CustReferenceNumbers (CustReferenceNumber NVARCHAR(4000))
CREATE TABLE #tmpUnion (PostingDateTime DATETIME2, 
						SerialNumber NVARCHAR(50), 
						D365BatchNumber NVARCHAR(50), 
						BatchOrderNumber NVARCHAR(50), 
						MovementJournalNumber NVARCHAR(50), 
						TransferJournalNumber NVARCHAR(50), 
						ItemNumber NVARCHAR(100), 
						Size NVARCHAR(10), 
						Warehouse NVARCHAR(10), 
						Location NVARCHAR(10), 
						TransitionQuantity DECIMAL DEFAULT 0, 
						ReservedQuantity DECIMAL, 
						BaseQuantity DECIMAL DEFAULT 0, 
						Sequence INT, 
						FunctionIdentifier NVARCHAR(50),
						ReferenceBatchNumber1 NVARCHAR(50) DEFAULT NULL, 
						ReferenceBatchNumber2 NVARCHAR(50) DEFAULT NULL, 
						ReferenceBatchNumber3 NVARCHAR(50) DEFAULT NULL, 
						ReferenceBatchNumber4 NVARCHAR(50) DEFAULT NULL, 
						ReferenceBatchNumber5 NVARCHAR(50) DEFAULT NULL,
						ReferenceBatchSequence1 INT DEFAULT 0, 
						ReferenceBatchSequence2 INT DEFAULT 0, 
						ReferenceBatchSequence3 INT DEFAULT 0, 
						ReferenceBatchSequence4 INT DEFAULT 0, 
						ReferenceBatchSequence5 INT DEFAULT 0,
						RefNumberOfPieces1 DECIMAL DEFAULT 0, 
						RefNumberOfPieces2 DECIMAL DEFAULT 0, 
						RefNumberOfPieces3 DECIMAL DEFAULT 0, 
						RefNumberOfPieces4 DECIMAL DEFAULT 0, 
						RefNumberOfPieces5 DECIMAL DEFAULT 0,
						RefItemNumber1 NVARCHAR(50) DEFAULT NULL, 
						RefItemNumber2 NVARCHAR(50) DEFAULT NULL, 
						RefItemNumber3 NVARCHAR(50) DEFAULT NULL, 
						RefItemNumber4 NVARCHAR(50) DEFAULT NULL, 
						RefItemNumber5 NVARCHAR(50) DEFAULT NULL,)

IF (LEN(@SerialNumber) > 0) 
BEGIN
	INSERT INTO #SerialNumbers 
	select * from dbo.SplitString(NULLIF(@SerialNumber, ','), ',')
END
ELSE IF (LEN(@D365BatchNumber) > 0) 
BEGIN
	INSERT INTO #D365BatchNumbers 
	select * from dbo.SplitString(NULLIF(@D365BatchNumber, ''), ',')

	INSERT INTO #SerialNumbers
	select BatchNumber from DOT_FloorAxIntParentTable with(nolock) 
	where D365BatchNumber in (select D365BatchNumber from #D365BatchNumbers where D365BatchNumber <> '')
	and IsDeleted = 0  and IsMigratedFromAX6 = 0   
END
Else IF (LEN(@CustReferenceNumber) > 0) 
BEGIN
	INSERT INTO #CustReferenceNumbers 
	select * from dbo.SplitString(NULLIF(@CustReferenceNumber, ','), ',')

	INSERT INTO #SerialNumbers
	select a.BatchNumber from DOT_FloorAxIntParentTable a with(nolock)
	join DOT_FGJournalTable b with(nolock) on a.Id = b.ParentRefRecId
	where b.CustomerReference in (select CustReferenceNumber from #CustReferenceNumbers where CustReferenceNumber <> '')
END
ELSE --filter by posting date time
BEGIN
	INSERT INTO #SerialNumbers
	select distinct a.BatchNumber from DOT_FloorAxIntParentTable a with(nolock)
	left join DOT_RafStgTable b with(nolock) on a.Id = b.ParentRefRecId
	left join DOT_RwkBatchOrderCreationChildTable c with(nolock) on a.Id = c.ParentRefRecId
	left join DOT_TransferJournal d with(nolock) on a.Id = d.ParentRefRecId
	left join DOT_PickingList e with(nolock) on a.Id = e.ParentRefRecId
	left join DOT_FGJournalTable f with(nolock) on a.Id = f.ParentRefRecId
	WHERE a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 
	  and b.PostingDateTime > = @PostingDateTimeFrom and b.PostingDateTime < = @PostingDateTimeTo  
	  or (c.PostingDateandTime > = @PostingDateTimeFrom and c.PostingDateandTime < = @PostingDateTimeTo)  
	  or (d.ScanInDateTime > = @PostingDateTimeFrom and d.ScanInDateTime < = @PostingDateTimeTo)  
	  or (e.PostingDateTime > = @PostingDateTimeFrom and e.PostingDateTime < = @PostingDateTimeTo)  
	  or (f.PostingDateTime > = @PostingDateTimeFrom and f.PostingDateTime < = @PostingDateTimeTo) 
END

--parent table
select * INTO #tmpParent from DOT_FloorAxIntParentTable with(nolock) 
where BatchNumber IN (select SerialNumber from #SerialNumbers where SerialNumber <> '') 
and IsDeleted = 0 and IsMigratedFromAX6 = 0 

--parent SMBP
--select * INTO #tmpParentSMBP1 from DOT_FloorAxIntParentTable with(nolock) 
--where ReferenceBatchNumber1 IN (select SerialNumber from #SerialNumbers where SerialNumber <> '')
--select * INTO #tmpParentSMBP2 from DOT_FloorAxIntParentTable with(nolock) 
--where ReferenceBatchNumber2 IN (select SerialNumber from #SerialNumbers where SerialNumber <> '')
--select * INTO #tmpParentSMBP3 from DOT_FloorAxIntParentTable with(nolock) 
--where ReferenceBatchNumber3 IN (select SerialNumber from #SerialNumbers where SerialNumber <> '')
--select * INTO #tmpParentSMBP4 from DOT_FloorAxIntParentTable with(nolock) 
--where ReferenceBatchNumber4 IN (select SerialNumber from #SerialNumbers where SerialNumber <> '')
--select * INTO #tmpParentSMBP5 from DOT_FloorAxIntParentTable with(nolock) 
--where ReferenceBatchNumber5 IN (select SerialNumber from #SerialNumbers where SerialNumber <> '')

--details: online/surgical batch only, online 2G and offline batch excluded
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
ISNULL(NULLIF(b.Warehouse,''),a.PlantNo+'-PROD') as Warehouse, b.Location, b.RAFGoodQty as TransitionQuantity, NULL as ReservedQuantity, 
a.Sequence, a.FunctionIdentifier
from #tmpParent a join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
where a.FunctionIdentifier in ('HBC','SRBC')

--details: RAF SOBC
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
b.Warehouse, b.Location, b.RAFGoodQty as TransitionQuantity, NULL as ReservedQuantity, 
a.Sequence, a.FunctionIdentifier
from #tmpParent a join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
where a.FunctionIdentifier in ('SOBC')

--details: Rejected SOBC
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
b.Warehouse, b.Location, b.RejectedQuantity * -1 as TransitionQuantity, NULL as ReservedQuantity, 
a.Sequence, a.FunctionIdentifier + '-Rejected' as FunctionIdentifier
from #tmpParent a join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
where a.FunctionIdentifier in ('SOBC') and b.RejectedQuantity > 0

--details: Rejected Sample SOBC
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
b.Warehouse, b.Location, b.RejectedSampleQuantity * -1 as TransitionQuantity, NULL as ReservedQuantity, 
a.Sequence, a.FunctionIdentifier + '-RejectedSample' as FunctionIdentifier
from #tmpParent a join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
where a.FunctionIdentifier in ('SOBC') and b.RejectedSampleQuantity > 0

--details: 2nd grade SOBC
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.BatchOrderNumber,'') as BatchOrderNumber, 
ISNULL(b.MovementJournalId,'') as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
b.Warehouse, b.Location, b.SecondGradeQuantity * -1 as TransitionQuantity, NULL as ReservedQuantity, 
a.Sequence, a.FunctionIdentifier + '-SecondGrade' as FunctionIdentifier
from #tmpParent a join DOT_RafStgTable b with(nolock) on a.id = b.ParentRefRecId
where a.FunctionIdentifier in ('SOBC') and b.SecondGradeQuantity > 0
 
--details: rework Reserved
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

select b.PostingDateandTime as PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, ISNULL(b.ReworkOrder,'') as BatchOrderNumber, 
'' as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
b.Warehouse, ISNULL(dbo.UFN_DOT_GetLocationForSummaryBatchCard(a.BatchNumber,a.Sequence),Replace(b.RouteCategory,'O','')) as Location, 
NULL as TransitionQuantity, b.Quantity * -1 as ReservedQuantity, 
a.Sequence, a.FunctionIdentifier
from #tmpParent a 
join DOT_RwkBatchOrderCreationChildTable b with(nolock) on a.id = b.ParentRefRecId
where a.FunctionIdentifier in ('RWKCR','RWKDEL')

--details: rework Transition
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

select PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, 
MovementJournalNumber, TransferJournalNumber, ItemNumber, Size,
Warehouse, Location, ReservedQuantity as TransitionQuantity, NULL as ReservedQuantity, 
Sequence + 1 as Sequence, FunctionIdentifier + '-Trans' as FunctionIdentifier
from #tmpUnion
where FunctionIdentifier in ('RWKCR','RWKDEL')

--details: transfer
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

select b.ScanInDateTime as PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, '' as BatchOrderNumber, 
'' as MovementJournalNumber, b.TransferJournalId as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
b.Warehouse, b.Location, 
Case when b.Location = 'PN' Then b.Quantity * -1 else b.Quantity end as TransitionQuantity, 
NULL as ReservedQuantity, a.Sequence, a.FunctionIdentifier
from #tmpParent a 
join DOT_TransferJournal b with(nolock) on a.id = b.ParentRefRecId
where a.FunctionIdentifier in ('SPBC','STPI','STPO')
 
--details: picking list
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier,
ReferenceBatchNumber1, ReferenceBatchNumber2, ReferenceBatchNumber3, ReferenceBatchSequence1, ReferenceBatchSequence2, ReferenceBatchSequence3,
RefNumberOfPieces1, RefNumberOfPieces2, RefNumberOfPieces3)

select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, b.PSIReworkOrderNo as BatchOrderNumber, 
'' as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
b.Warehouse, 'RWK' as Location, 
b.OldBatchQty * -1 as TransitionQuantity, 
NULL as ReservedQuantity, a.Sequence, a.FunctionIdentifier, 
a.ReferenceBatchNumber1, a.ReferenceBatchNumber2, a.ReferenceBatchNumber3,
a.ReferenceBatchSequence1, a.ReferenceBatchSequence2, a.ReferenceBatchSequence3,
b.RefNumberOfPieces1, b.RefNumberOfPieces2, b.RefNumberOfPieces3
from #tmpParent a 
join DOT_PickingList b with(nolock) on a.id = b.ParentRefRecId
where a.FunctionIdentifier in ('CBCI')

--details: picking list change SN 1
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

select PostingDateTime, ReferenceBatchNumber1 as SerialNumber, 
dbo.UFN_DOT_GetD365BatchNoForSummaryBatchCard(ReferenceBatchNumber1) as D365BatchNumber, 
dbo.UFN_DOT_GetBOForSummaryBatchCard(ReferenceBatchNumber1) as BatchOrderNumber, 
MovementJournalNumber, TransferJournalNumber, ItemNumber, Size,
dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(ReferenceBatchNumber1,ReferenceBatchSequence1) as Warehouse,
dbo.UFN_DOT_GetLocationForSummaryBatchCard(ReferenceBatchNumber1,ReferenceBatchSequence1) as Location, 
RefNumberOfPieces1 as TransitionQuantity, ReservedQuantity, Sequence + 1 as Sequence, FunctionIdentifier + '-SN1' as FunctionIdentifier
from #tmpUnion
where FunctionIdentifier = 'CBCI' and ReferenceBatchNumber1 is not null

--details: picking list change SN 2
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

select PostingDateTime, ReferenceBatchNumber1 as SerialNumber, 
dbo.UFN_DOT_GetD365BatchNoForSummaryBatchCard(ReferenceBatchNumber2) as D365BatchNumber, 
dbo.UFN_DOT_GetBOForSummaryBatchCard(ReferenceBatchNumber2) as BatchOrderNumber, 
MovementJournalNumber, TransferJournalNumber, ItemNumber, Size,
dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(ReferenceBatchNumber2,ReferenceBatchSequence2) as Warehouse,
dbo.UFN_DOT_GetLocationForSummaryBatchCard(ReferenceBatchNumber2,ReferenceBatchSequence2) as Location, 
RefNumberOfPieces2 as TransitionQuantity, ReservedQuantity, Sequence + 1 as Sequence, FunctionIdentifier + '-SN2' as FunctionIdentifier
from #tmpUnion
where FunctionIdentifier = 'CBCI' and ReferenceBatchNumber2 is not null

--details: picking list change SN 3
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

select PostingDateTime, ReferenceBatchNumber1 as SerialNumber, 
dbo.UFN_DOT_GetD365BatchNoForSummaryBatchCard(ReferenceBatchNumber3) as D365BatchNumber, 
dbo.UFN_DOT_GetBOForSummaryBatchCard(ReferenceBatchNumber3) as BatchOrderNumber, 
MovementJournalNumber, TransferJournalNumber, ItemNumber, Size,
dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(ReferenceBatchNumber3,ReferenceBatchSequence3) as Warehouse,
dbo.UFN_DOT_GetLocationForSummaryBatchCard(ReferenceBatchNumber3,ReferenceBatchSequence3) as Location, 
RefNumberOfPieces3 as TransitionQuantity, ReservedQuantity, Sequence + 1 as Sequence, FunctionIdentifier + '-SN3' as FunctionIdentifier
from #tmpUnion
where FunctionIdentifier = 'CBCI' and ReferenceBatchNumber3 is not null

--details: final pack
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, BaseQuantity, Sequence, FunctionIdentifier)

select b.PostingDateTime, a.BatchNumber as SerialNumber, a.D365BatchNumber, b.BatchOrderNumber,
'' as MovementJournalNumber, '' as TransferJournalNumber, 
case when a.FunctionIdentifier = 'SGBC' then b.ReferenceItemNumber else b.ItemNumber end as ItemNumber,
b.Configuration as Size,
'FG' as Warehouse, '' as Location, b.Quantity as TransitionQuantity, NULL as ReservedQuantity, d.BaseQuantity,
a.Sequence, a.FunctionIdentifier
from #tmpParent a join DOT_FGJournalTable b with(nolock) on a.id = b.ParentRefRecId
join DOT_FloorSalesLine d with(nolock) on d.SalesId = b.SalesOrderNumber
where a.FunctionIdentifier in ('SBC','SGBC','SPPBC')
AND d.ItemId = case when a.FunctionIdentifier = 'SGBC' then b.ReferenceItemNumber else b.ItemNumber end 
AND b.CONFIGURATION = case when a.FunctionIdentifier = 'SPPBC' then d.CustomerSize else d.HartalegaCommonSize end

--details: final pack-glove qty
INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

select PostingDateTime, SerialNumber, 
dbo.UFN_DOT_GetD365BatchNoForSummaryBatchCard(SerialNumber) as D365BatchNumber, 
dbo.UFN_DOT_GetBOForSummaryBatchCard(SerialNumber) as BatchOrderNumber, 
MovementJournalNumber, TransferJournalNumber,
dbo.UFN_DOT_GetItemNumberForSummaryBatchCard(SerialNumber) as ItemNumber, Size,
dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(SerialNumber,Sequence) as Warehouse,
dbo.UFN_DOT_GetLocationForSummaryBatchCard(SerialNumber,Sequence) as Location, 
CAST(BaseQuantity as DECIMAL) * TransitionQuantity * -1 as TransitionQuantity, ReservedQuantity, Sequence + 1 as Sequence, 
FunctionIdentifier + '-Detail' as FunctionIdentifier
from #tmpUnion
where FunctionIdentifier = 'SBC'

----details: final pack multiple batch 1
--INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
--TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier, RefNumberOfPieces1, RefItemNumber1)

--select b.PostingDateTime, a.ReferenceBatchNumber1 as SerialNumber, a.D365BatchNumber, b.BatchOrderNumber,
--'' as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
--'FG' as Warehouse, '' as Location, b.Quantity as TransitionQuantity, NULL as ReservedQuantity, 
--a.ReferenceBatchSequence1 as Sequence, a.FunctionIdentifier, b.RefNumberOfPieces1, b.RefItemNumber1
--from #tmpParentSMBP1 a join DOT_FGJournalTable b with(nolock) on a.id = b.ParentRefRecId
--where a.Id is not null

----details: final pack multiple batch 1 - glove qty
--INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
--TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

--select PostingDateTime, SerialNumber, 
--dbo.UFN_DOT_GetD365BatchNoForSummaryBatchCard(SerialNumber) as D365BatchNumber, 
--dbo.UFN_DOT_GetBOForSummaryBatchCard(SerialNumber) as BatchOrderNumber, 
--MovementJournalNumber, TransferJournalNumber, RefItemNumber1 as ItemNumber, Size,
--dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(SerialNumber,Sequence) as Warehouse,
--dbo.UFN_DOT_GetLocationForSummaryBatchCard(SerialNumber,Sequence) as Location, 
--RefNumberOfPieces1 * -1 as TransitionQuantity, NULL as ReservedQuantity,
--Sequence + 1 as Sequence, FunctionIdentifier + 'Detail1' as FunctionIdentifier
--from #tmpUnion
--where FunctionIdentifier = 'SMBP' and RefItemNumber1 is not null and RefNumberOfPieces1 > 0

----details: final pack multiple batch 2
--INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
--TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier, RefNumberOfPieces2, RefItemNumber2)

--select b.PostingDateTime, a.ReferenceBatchNumber2 as SerialNumber, a.D365BatchNumber, b.BatchOrderNumber,
--'' as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
--'FG' as Warehouse, '' as Location, b.Quantity as TransitionQuantity, NULL as ReservedQuantity, 
--a.ReferenceBatchSequence2 as Sequence, a.FunctionIdentifier, b.RefNumberOfPieces2, b.RefItemNumber2
--from #tmpParentSMBP2 a join DOT_FGJournalTable b with(nolock) on a.id = b.ParentRefRecId
--where a.Id is not null

----details: final pack multiple batch 2 - glove qty
--INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
--TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

--select PostingDateTime, SerialNumber, 
--dbo.UFN_DOT_GetD365BatchNoForSummaryBatchCard(SerialNumber) as D365BatchNumber, 
--dbo.UFN_DOT_GetBOForSummaryBatchCard(SerialNumber) as BatchOrderNumber, 
--MovementJournalNumber, TransferJournalNumber, RefItemNumber2 as ItemNumber, Size,
--dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(SerialNumber,Sequence) as Warehouse,
--dbo.UFN_DOT_GetLocationForSummaryBatchCard(SerialNumber,Sequence) as Location, 
--RefNumberOfPieces2* -1 as TransitionQuantity, NULL as ReservedQuantity,
--Sequence + 1 as Sequence, FunctionIdentifier + 'Detail2' as FunctionIdentifier
--from #tmpUnion
--where FunctionIdentifier = 'SMBP' and RefItemNumber2 is not null and RefNumberOfPieces2 > 0

----details: final pack multiple batch 3
--INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
--TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier, RefNumberOfPieces3, RefItemNumber3)

--select b.PostingDateTime, a.ReferenceBatchNumber3 as SerialNumber, a.D365BatchNumber, b.BatchOrderNumber,
--'' as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
--'FG' as Warehouse, '' as Location, b.Quantity as TransitionQuantity, NULL as ReservedQuantity, 
--a.ReferenceBatchSequence3 as Sequence, a.FunctionIdentifier, b.RefNumberOfPieces3, b.RefItemNumber3
--from #tmpParentSMBP3 a join DOT_FGJournalTable b with(nolock) on a.id = b.ParentRefRecId
--where a.Id is not null

----details: final pack multiple batch 3 - glove qty
--INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
--TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

--select PostingDateTime, SerialNumber, 
--dbo.UFN_DOT_GetD365BatchNoForSummaryBatchCard(SerialNumber) as D365BatchNumber, 
--dbo.UFN_DOT_GetBOForSummaryBatchCard(SerialNumber) as BatchOrderNumber, 
--MovementJournalNumber, TransferJournalNumber, RefItemNumber3 as ItemNumber, Size,
--dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(SerialNumber,Sequence) as Warehouse,
--dbo.UFN_DOT_GetLocationForSummaryBatchCard(SerialNumber,Sequence) as Location, 
--RefNumberOfPieces3* -1 as TransitionQuantity, NULL as ReservedQuantity,
--Sequence + 1 as Sequence, FunctionIdentifier + 'Detail3' as FunctionIdentifier
--from #tmpUnion
--where FunctionIdentifier = 'SMBP' and RefItemNumber3 is not null and RefNumberOfPieces3 > 0

----details: final pack multiple batch 4
--INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
--TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier, RefNumberOfPieces4, RefItemNumber4)

--select b.PostingDateTime, a.ReferenceBatchNumber4 as SerialNumber, a.D365BatchNumber, b.BatchOrderNumber,
--'' as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
--'FG' as Warehouse, '' as Location, b.Quantity as TransitionQuantity, NULL as ReservedQuantity, 
--a.ReferenceBatchSequence4 as Sequence, a.FunctionIdentifier, b.RefNumberOfPieces4, b.RefItemNumber4
--from #tmpParentSMBP4 a join DOT_FGJournalTable b with(nolock) on a.id = b.ParentRefRecId
--where a.Id is not null

----details: final pack multiple batch 4 - glove qty
--INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
--TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

--select PostingDateTime, SerialNumber, 
--dbo.UFN_DOT_GetD365BatchNoForSummaryBatchCard(SerialNumber) as D365BatchNumber, 
--dbo.UFN_DOT_GetBOForSummaryBatchCard(SerialNumber) as BatchOrderNumber, 
--MovementJournalNumber, TransferJournalNumber, RefItemNumber4 as ItemNumber, Size,
--dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(SerialNumber,Sequence) as Warehouse,
--dbo.UFN_DOT_GetLocationForSummaryBatchCard(SerialNumber,Sequence) as Location, 
--RefNumberOfPieces4* -1 as TransitionQuantity, NULL as ReservedQuantity,
--Sequence + 1 as Sequence, FunctionIdentifier + 'Detail4' as FunctionIdentifier
--from #tmpUnion
--where FunctionIdentifier = 'SMBP' and RefItemNumber4 is not null and RefNumberOfPieces4 > 0

----details: final pack multiple batch 5
--INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
--TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier, RefNumberOfPieces5, RefItemNumber5)

--select b.PostingDateTime, a.ReferenceBatchNumber5 as SerialNumber, a.D365BatchNumber, b.BatchOrderNumber,
--'' as MovementJournalNumber, '' as TransferJournalNumber, b.ItemNumber, b.Configuration as Size,
--'FG' as Warehouse, '' as Location, b.Quantity as TransitionQuantity, NULL as ReservedQuantity, 
--a.ReferenceBatchSequence5 as Sequence, a.FunctionIdentifier, b.RefNumberOfPieces5, b.RefItemNumber5
--from #tmpParentSMBP5 a join DOT_FGJournalTable b with(nolock) on a.id = b.ParentRefRecId
--where a.Id is not null

----details: final pack multiple batch 5 - glove qty
--INSERT INTO #tmpUnion (PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
--TransferJournalNumber, ItemNumber, Size, Warehouse, Location, TransitionQuantity, ReservedQuantity, Sequence, FunctionIdentifier)

--select PostingDateTime, SerialNumber, 
--dbo.UFN_DOT_GetD365BatchNoForSummaryBatchCard(SerialNumber) as D365BatchNumber, 
--dbo.UFN_DOT_GetBOForSummaryBatchCard(SerialNumber) as BatchOrderNumber, 
--MovementJournalNumber, TransferJournalNumber, RefItemNumber5 as ItemNumber, Size,
--dbo.UFN_DOT_GetWarehouseForSummaryBatchCard(SerialNumber,Sequence) as Warehouse,
--dbo.UFN_DOT_GetLocationForSummaryBatchCard(SerialNumber,Sequence) as Location, 
--RefNumberOfPieces5* -1 as TransitionQuantity, NULL as ReservedQuantity,
--Sequence + 1 as Sequence, FunctionIdentifier + 'Detail5' as FunctionIdentifier
--from #tmpUnion
--where FunctionIdentifier = 'SMBP' and RefItemNumber5 is not null and RefNumberOfPieces5 > 0

--details: surgical packing plan

--details: movement for online rejection is excluded

--Displayed records will sort according to posting date and time column.
--TODO: move sorting to webadmin
IF LEN(@PostingDateTimeFrom) > 0
BEGIN
	Select PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
	TransferJournalNumber, ItemNumber, Size, Warehouse, Location,
	CONVERT(INT,TransitionQuantity) as TransitionQuantity, CONVERT(INT,ReservedQuantity) as ReservedQuantity 
	from #tmpUnion
	Where PostingDateTime > = @PostingDateTimeFrom and PostingDateTime < = @PostingDateTimeTo 
	--order by PostingDateTime, Sequence
END
ELSE
BEGIN 
	Select PostingDateTime, SerialNumber, D365BatchNumber, BatchOrderNumber, MovementJournalNumber, 
	TransferJournalNumber, ItemNumber, Size, Warehouse, Location,
	CONVERT(INT,TransitionQuantity) as TransitionQuantity, CONVERT(INT,ReservedQuantity) as ReservedQuantity 
	from #tmpUnion
	--order by PostingDateTime, Sequence
END

IF OBJECT_ID('tempdb..#SerialNumbers') IS NOT NULL  
DROP TABLE #SerialNumbers 
IF OBJECT_ID('tempdb..#D365BatchNumbers') IS NOT NULL  
DROP TABLE #D365BatchNumbers 
IF OBJECT_ID('tempdb..#CustReferenceNumbers') IS NOT NULL  
DROP TABLE #CustReferenceNumbers
IF OBJECT_ID('tempdb..#tmpParent') IS NOT NULL  
DROP TABLE #tmpParent  
IF OBJECT_ID('tempdb..#tmpUnion') IS NOT NULL  
DROP TABLE #tmpUnion 
IF OBJECT_ID('tempdb..#tmpParentSMBP1') IS NOT NULL  
DROP TABLE #tmpParentSMBP1  
IF OBJECT_ID('tempdb..#tmpParentSMBP2') IS NOT NULL  
DROP TABLE #tmpParentSMBP2  
IF OBJECT_ID('tempdb..#tmpParentSMBP3') IS NOT NULL  
DROP TABLE #tmpParentSMBP3  
IF OBJECT_ID('tempdb..#tmpParentSMBP4') IS NOT NULL  
DROP TABLE #tmpParentSMBP4  
IF OBJECT_ID('tempdb..#tmpParentSMBP5') IS NOT NULL  
DROP TABLE #tmpParentSMBP5  
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_SurgicalResource_Get]...';


GO
-- =============================================================================    
-- Name:   USP_DOT_SurgicalResource_Get
-- Purpose:   Get Resource and Batch Order details for Print Surgical Batch Card 
-- =============================================================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   ------------------------------------------------------------  
-- 19/10/2020  Azrul Amin    SP created.  
-- =============================================================================    

CREATE   PROCEDURE [dbo].[USP_DOT_SurgicalResource_Get]  
(   
 @LocationId Int,   
 @LineId varchar(20),  
 @Resource varchar(20),   
 @BO varchar(20)
)   
AS  
BEGIN     
 SET NOCOUNT ON;  
  
 -- #1.List out all ResourceGroup (line).  
 IF (@LineId IS NULL) OR (LEN(@LineId) < 0)   
 BEGIN  
  SELECT DISTINCT  
  '' as Resource  
  ,'' as ResourceId  
  ,'' as LocationId  
  ,'' as Plant  
  ,substring(res.ResourceGrp,3,3) as LineId  
  ,res.ResourceGrp as Line  
  ,'' as TierSide  
  ,'' as BatchOrder  
  ,'' as GloveCode  
  ,'' as Size  
  FROM   
   DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
   DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON substring(res.ResourceGrp,0,3) = loc.LocationName  
  WHERE bo.ProdPoolId = 'SGR' AND loc.LocationId = @LocationId AND bo.ProdStatus = 'StartedUp'   
  AND substring(res.ResourceGrp,3,3) in (select linenumber from linemaster where LocationId = @LocationId)
 END  
  
-- #2.List out all Resource  
ELSE IF ((@Resource IS NULL) OR (LEN(@Resource) = 0))  
 BEGIN  
  IF ((@BO IS NULL) OR (LEN(@BO) = 0))  
  BEGIN  
   SELECT DISTINCT  
   res.Resource  
   ,'' as ResourceId  
   ,'' as LocationId  
   ,'' as Plant  
   ,substring(res.ResourceGrp,3,3) as LineId  
   ,res.ResourceGrp as Line  
   ,substring(res.Resource,6,2) as TierSide  
   ,'' as BatchOrder  
   ,'' as GloveCode  
   ,'' as Size  
   FROM   
    DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
    DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
    LocationMaster as loc ON substring(res.ResourceGrp,0,3) = loc.LocationName  
   WHERE bo.ProdPoolId = 'SGR' AND loc.LocationId = @LocationId AND substring(res.ResourceGrp,3,3) = @LineId AND bo.ProdStatus = 'StartedUp' 
   and bo.IsDeleted=0  
  END  
  ELSE  
  BEGIN  
   SELECT DISTINCT  
   res.Resource  
   ,'' as ResourceId  
   ,'' as LocationId  
   ,'' as Plant  
   ,substring(res.ResourceGrp,3,3) as LineId  
   ,res.ResourceGrp as Line  
   ,substring(res.Resource,6,2) as TierSide  
   ,'' as BatchOrder  
   ,'' as GloveCode  
   ,'' as Size  
   FROM   
    DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
    DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
    LocationMaster as loc ON substring(res.ResourceGrp,0,3) = loc.LocationName  
   WHERE bo.ProdPoolId = 'SGR' AND loc.LocationId = @LocationId AND substring(res.ResourceGrp,3,3) = @LineId AND bo.ProdStatus = 'StartedUp'   
   and bo.IsDeleted=0 and bo.BthOrderId = @BO  
  END  
 END  
  
-- #3.To list out all Batch Order based on selected Resource.  
ELSE IF ((@BO IS NULL) OR (LEN(@BO) = 0))  
 BEGIN  
  SELECT   
  res.Resource  
  ,res.Id as ResourceId  
  ,loc.LocationId  
  ,substring(res.ResourceGrp,0,3) as Plant  
  ,substring(res.ResourceGrp,3,3) as LineId  
  ,res.ResourceGrp as Line  
  ,substring(res.Resource,6,2) as TierSide  
  ,bo.BthOrderId as BatchOrder  
  ,bo.ItemId as GloveCode  
  ,bo.Size  
  FROM   
   DOT_FloorD365BO (NOLOCK) AS bo LEFT JOIN  
   DOT_FloorD365BOResource (NOLOCK) AS res ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON substring(res.ResourceGrp,0,3) = loc.LocationName  
  WHERE bo.ProdPoolId = 'SGR' AND loc.LocationId = @LocationId AND substring(res.ResourceGrp,3,3) = @LineId  
  AND res.Resource = @Resource AND bo.ProdStatus = 'StartedUp'  
  and bo.IsDeleted=0  
 END  
  
-- #4.To list out all Batch Order Details based on selected Resource and Batch Order.  
ELSE  
 BEGIN  
  SELECT   
  res.Resource  
  ,res.Id as ResourceId  
  ,loc.LocationId  
  ,substring(res.ResourceGrp,0,3) as Plant  
  ,substring(res.ResourceGrp,3,3) as LineId  
  ,res.ResourceGrp as Line  
  ,substring(res.Resource,6,2) as TierSide  
  ,bo.BthOrderId as BatchOrder  
  ,bo.ItemId as GloveCode  
  ,bo.Size  
  FROM   
   DOT_FloorD365BO AS bo  WITH (NOLOCK) LEFT JOIN  
   DOT_FloorD365BOResource  AS res  WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON substring(res.ResourceGrp,0,3) = loc.LocationName  
  WHERE bo.ProdPoolId = 'SGR' AND loc.LocationId = @LocationId AND substring(res.ResourceGrp,3,3) = @LineId   
  AND res.Resource = @Resource AND bo.ProdStatus = 'StartedUp' AND bo.BthOrderId = @BO  
  and bo.IsDeleted=0  
 END  
 SET NOCOUNT OFF;    
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_TOMsScanIn]...';


GO
-- ==================================================================================  
-- Name:	USP_DOT_TOMsScanIn
-- Purpose:  Pump TOMs Scan In data into staging 
-- ==================================================================================  
-- Change History  
-- Date			Author			Comments  
-- -----		------			-----------------------------------------------------------------  
-- 18/07/2018  Muhd Khalid		SP created.  
-- 09/09/2021  Azrul			Open batch flag for NGC1.5.
-- 25/12/2021  Max He			get Isconsolidated by function and fix STPI after SMBP
-- ==================================================================================  
--@BatchSerialNos '218023232:150,218023233:250'
--USP_DOT_TOMsScanIn '','',0,'M','NB-AB-OLPF-035-SE-WHTE-OM6N','PH1-TP','NT100012',0.0,'2021-12-25','2211010100:22000','TP'
CREATE PROCEDURE [dbo].[USP_DOT_TOMsScanIn] 
@FGItemCode nvarchar(40),
@FGProductionOrder nvarchar(40),
@IsOrignalTemppack bit,
@Configuration nvarchar(10),
@GloveCode nvarchar(40),
@Warehouse nvarchar(10),
@PalletId nvarchar(20),
@PalletTotalQty numeric(32, 16),
@ScanInDateTime datetime,
@BatchSerialNos nvarchar(max),
@Location nvarchar(10)
AS
BEGIN
  BEGIN TRANSACTION;
    BEGIN TRY
      --Parent table variable
      DECLARE @BATCHCARDNUMBER nvarchar(50),
              @BATCHNUMBER nvarchar(20),
              @FSIDENTIFIER uniqueidentifier,
              @FUNCTIONIDENTIFIER nvarchar(20),
              @PLANTNO nvarchar(20),
              @REFERENCEBATCHNUMBER1 nvarchar(20),
              @REFERENCEBATCHNUMBER2 nvarchar(20),
              @REFERENCEBATCHNUMBER3 nvarchar(20),
              @REFERENCEBATCHNUMBER4 nvarchar(20),
              @REFERENCEBATCHNUMBER5 nvarchar(20),
              @REFERENCEBATCHSEQUENCE1 int,
              @REFERENCEBATCHSEQUENCE2 int,
              @REFERENCEBATCHSEQUENCE3 int,
              @REFERENCEBATCHSEQUENCE4 int,
              @REFERENCEBATCHSEQUENCE5 int,
              @SEQUENCE int,
              --Transfer Journal Variable
              --@BRAND nvarchar(20),
              @FORMULA nvarchar(20),
              --@ITEMNUMBER nvarchar(20),
              --@LOCATION nvarchar(10),
              @PARENTREFRECID int,
              @QUANTITY numeric(32, 16),
              @ServiceName nvarchar(50),
              @PostingType nvarchar(20),
              @PostedDate datetime,
              @SerialNumber numeric(15, 0),
              @IsPostedToAX bit,
              @IsPostedInAX bit,
              @ExceptionCode nvarchar(1000),
              @TransactionID nvarchar(100),
              @Area nvarchar(10),
              @SPLITDATA nvarchar(50),
			  @ReworkCount int,
			  @SOBCCount int,
			  @MaxQty int,
			  @LastService nvarchar(20),
			  @IsConsolidated bit -- #AZRUL 17/9/2021: Open batch flag for NGC1.5

		--DECLARE @tempTable Table (Seq int, Result BIT) -- #AZRUL 17/9/2021: Open batch flag for NGC1.5

      --SET @FSIDENTIFIER = NEWID();
      SELECT
        ROW_NUMBER() OVER (ORDER BY (SELECT
          1)
        ) AS id,
        Data INTO #tempTable
      FROM SPLIT(@BatchSerialNos, ',');

      DECLARE @COUNT int = (SELECT
        MAX(id)
      FROM #tempTable);
      DECLARE @ROW int = 1;

      WHILE (@ROW <= @COUNT)
      BEGIN
        SET @SPLITDATA = (SELECT
          Data
        FROM #tempTable
        WHERE id = @ROW);
        SET @BATCHNUMBER = SUBSTRING(@SPLITDATA, 0, CHARINDEX(':', @SPLITDATA));
        SET @BATCHCARDNUMBER = (SELECT TOP 1
          BatchcardNumber
        FROM DOT_FloorAxIntParentTable
        WHERE BatchNumber = @BATCHNUMBER AND IsDeleted=0);

		-- Validate not done SOBC can't do STPI
		select @ReworkCount = count(1) from DOT_FloorAxIntParentTable WITH (NOLOCK) where BatchNumber=@BATCHNUMBER and FunctionIdentifier='RWKCR' and IsDeleted=0 and IsMigratedFromAX6 = 0

		select @SOBCCount = count(1) from DOT_FloorAxIntParentTable WITH (NOLOCK) where BatchNumber=@BATCHNUMBER and FunctionIdentifier='SOBC' and IsDeleted=0 and IsMigratedFromAX6 = 0

	  	if @ReworkCount<>@SOBCCount  
		BEGIN  
			-- allow insert TOMS if got extra rework from HBC RESAMPLE 
		    DECLARE @ResampleCount INT  

			select @ResampleCount=COUNT(1) 
			from DOT_FloorAxIntParentTable a with (nolock) 
			join DOT_RafStgTable c with (nolock) on a.id = c.ParentRefRecId
			join DOT_FloorD365HRGLOVERPT b with (nolock) on a.BatchNumber = b.SerialNo and b.Resource = c.Resource
			where a.BatchNumber = @BATCHNUMBER and a.FunctionIdentifier = 'HBC' 
			and a.ReferenceBatchNumber1 = 'RESAMPLE' and b.SeqNo = 1
			and a.IsMigratedFromAX6=0 -- added by Max He on 22/5/2019, to bypass if RESAMPLE is from AX migration
			and a.IsDeleted=0

			SET @ResampleCount = ISNULL(@ResampleCount,0); 
			SET @ReworkCount = @ReworkCount-@ResampleCount

			if @ReworkCount<>@SOBCCount  
				BEGIN
					RAISERROR ('In Order to proceed scan in,please complete QC process!', -- Message text.    
							   16, -- Severity.    
							   1 -- State.    
							   );    
				END
		 END   

		--check last posting, block if STPI
		select Top 1 @LastService = ServiceName from AXPostingLog where SerialNumber=@BATCHNUMBER order by CreationDate desc 

		if @LastService = 'STPI'
		BEGIN
			RAISERROR ('Temp pack not scan out!', -- Message text.    
						16, -- Severity.    
						1 -- State.    
						);    
		END

        SET @FSIDENTIFIER = NEWID(); -- fix Duplicated GUID, Max He, 4/1/2019
        SET @FUNCTIONIDENTIFIER = 'STPI';
        --SET @PLANTNO = SUBSTRING(@Warehouse, 0, 3);
		
        SET @REFERENCEBATCHNUMBER1 = NULL;
        SET @REFERENCEBATCHNUMBER2 = NULL;
        SET @REFERENCEBATCHNUMBER3 = NULL;
        SET @REFERENCEBATCHNUMBER4 = NULL;
        SET @REFERENCEBATCHNUMBER5 = NULL;
        SET @REFERENCEBATCHSEQUENCE1 = 0;
        SET @REFERENCEBATCHSEQUENCE2 = 0;
        SET @REFERENCEBATCHSEQUENCE3 = 0;
        SET @REFERENCEBATCHSEQUENCE4 = 0;
        SET @REFERENCEBATCHSEQUENCE5 = 0;
        --SET @SEQUENCE = (SELECT
        --  COUNT(SerialNumber) + 1 AS 'BatchSequence'
        --FROM dbo.AXPostingLog
        --WHERE SerialNumber = @BATCHNUMBER
        --AND (exceptioncode IS NULL
        --OR exceptioncode = '999'));

		set @SEQUENCE = dbo.Ufn_DOT_GET_BATCHSEQUENCE(@BATCHNUMBER);

		-- Find previous record plant no
		-- Special handling for STPI/STPO due to TOMs didn't send plant no, Max He, 19/07/2021
		select @PLANTNO = PlantNo from DOT_FloorAxIntParentTable WITH (NOLOCK) 
		where IsMigratedFromAX6 = 0 and IsDeleted=0 and 
		((BatchNumber=@BATCHNUMBER and Sequence=@SEQUENCE-1)
			or 
			(ReferenceBatchNumber1=@BATCHNUMBER and  ReferenceBatchSequence1=@SEQUENCE-1 )
			or 
			(ReferenceBatchNumber2=@BATCHNUMBER and  ReferenceBatchSequence2=@SEQUENCE-1 )
			or 																 
			(ReferenceBatchNumber3=@BATCHNUMBER and  ReferenceBatchSequence3=@SEQUENCE-1 )
			or 																 
			(ReferenceBatchNumber4=@BATCHNUMBER and  ReferenceBatchSequence4=@SEQUENCE-1 )
			or 																 
			(ReferenceBatchNumber5=@BATCHNUMBER and  ReferenceBatchSequence5=@SEQUENCE-1 ));
        --SET @BRAND = '';
        SET @FORMULA = '';
        --SET @ITEMNUMBER = @GloveCode;
        --SET @LOCATION = '';
        SET @QUANTITY = SUBSTRING(@SPLITDATA, CHARINDEX(':', @SPLITDATA) + 1, LEN(@SPLITDATA));

		-- #AZRUL 7/8/2019: Validate Qty to proceed STPI START
		SELECT TOP 1 @MaxQty = BalancePcs FROM dbo.ufn_GetBatchSummaryTable(@BATCHNUMBER) ORDER BY ProcessDate DESC  

		if @QUANTITY>@MaxQty  
			BEGIN
				RAISERROR ('Over maximum quantity!', -- Message text.    
							16, -- Severity.    
							1 -- State.    
							);    
			END
		if @QUANTITY=0  
		BEGIN
			RAISERROR ('Quantity must be more than 0!', -- Message text.    
						16, -- Severity.    
						1 -- State.    
						);    
		END
		-- #AZRUL 7/8/2019: END

        SET @ServiceName = @FUNCTIONIDENTIFIER;
        SET @PostingType = 'DOTTOMsScanIn';
        SET @PostedDate = GETDATE();
        SET @SerialNumber = @BATCHNUMBER;
        SET @IsPostedToAX = 1;
        SET @IsPostedInAX = 1;
        SET @ExceptionCode = NULL;
        SET @TransactionID = '-1';
        SET @Area = @LOCATION;

		-- #AZRUL 17/9/2021: Open batch flag for NGC1.5 START
		--INSERT INTO @tempTable
		--	EXEC dbo.USP_GET_BATCHSEQUENCE @SerialNumber, @PLANTNO

		--SET @IsConsolidated = (SELECT Result FROM @tempTable)
		SET @IsConsolidated = dbo.Ufn_DOT_GET_IsConsolidated(@SerialNumber,@PLANTNO);
		--DELETE @tempTable
		-- #AZRUL 17/9/2021: Open batch flag for NGC1.5 END

        --Transaction start
        IF NOT EXISTS (SELECT
            *
          FROM DOT_FloorAxIntParentTable WITH (NOLOCK)
          WHERE [BATCHCARDNUMBER] = @BATCHCARDNUMBER
          AND [BATCHNUMBER] = @BATCHNUMBER
          AND [FUNCTIONIDENTIFIER] = @FUNCTIONIDENTIFIER
          AND [PLANTNO] = @PLANTNO
		  AND [SEQUENCE] = @SEQUENCE
		  AND IsDeleted=0)
        BEGIN
          INSERT INTO [dbo].[DOT_FloorAxIntParentTable] ([BATCHCARDNUMBER]
          , [BATCHNUMBER]
          , [CREATIONTIME]
          , [CREATORUSERID]
          , [DELETERUSERID]
          , [DELETIONTIME]
          , [ERRORMESSAGE]
          , [FSIDENTIFIER]
          , [FUNCTIONIDENTIFIER]
          , [ISDELETED]
          , [LASTMODIFICATIONTIME]
          , [LASTMODIFIERUSERID]
          , [PROCESSINGSTATUS]
          , [PLANTNO]
          , [PRODID]
          , [REFERENCEBATCHNUMBER1]
          , [REFERENCEBATCHNUMBER2]
          , [REFERENCEBATCHNUMBER3]
          , [REFERENCEBATCHNUMBER4]
          , [REFERENCEBATCHNUMBER5]
          , [REFERENCEBATCHSEQUENCE1]
          , [REFERENCEBATCHSEQUENCE2]
          , [REFERENCEBATCHSEQUENCE3]
          , [REFERENCEBATCHSEQUENCE4]
          , [REFERENCEBATCHSEQUENCE5]
          , [SEQUENCE]
          , [PALLETID]
		  , [IsConsolidated]) -- #AZRUL 17/9/2021: Open batch flag for NGC1.5
            VALUES (@BATCHCARDNUMBER, @BATCHNUMBER, GETDATE(), 1, NULL, NULL, '', @FSIDENTIFIER, @FUNCTIONIDENTIFIER, 0, GETDATE(), 1, 1, @PLANTNO, NULL, @REFERENCEBATCHNUMBER1, @REFERENCEBATCHNUMBER2, @REFERENCEBATCHNUMBER3, @REFERENCEBATCHNUMBER4, @REFERENCEBATCHNUMBER5, @REFERENCEBATCHSEQUENCE1, @REFERENCEBATCHSEQUENCE2, @REFERENCEBATCHSEQUENCE3, @REFERENCEBATCHSEQUENCE4, @REFERENCEBATCHSEQUENCE5, @SEQUENCE, @PalletId, @IsConsolidated);

          SET @PARENTREFRECID = (SELECT
            @@IDENTITY);
        END
        ELSE
        BEGIN
          SET @PARENTREFRECID = (SELECT
            Id
          FROM DOT_FloorAxIntParentTable WITH (NOLOCK)
          WHERE [BATCHCARDNUMBER] = @BATCHCARDNUMBER
          AND [BATCHNUMBER] = @BATCHNUMBER
          AND [FUNCTIONIDENTIFIER] = @FUNCTIONIDENTIFIER
          AND [PLANTNO] = @PLANTNO
		  AND [SEQUENCE] = @SEQUENCE
		  AND IsDeleted=0);
        END

        EXEC dbo.USP_DOT_CreateTransferJournal @BATCHCARDNUMBER,
                                               @BATCHNUMBER,
                                               @FGItemCode,--@BRAND,
                                               @Configuration,
                                               @FORMULA,
                                               @GloveCode,
                                               @LOCATION,
                                               @PARENTREFRECID,
                                               @QUANTITY,
                                               @ScanInDateTime,
                                               @ScanInDateTime,
                                               '',
                                               @Warehouse,
											   @IsOrignalTemppack,
											   @FGProductionOrder;

        EXEC dbo.USP_SAVE_AXPOSTINGLOG @ServiceName,
                                       @PostingType,
                                       @PostedDate,
                                       @BATCHCARDNUMBER,
                                       @SerialNumber,
                                       @IsPostedToAX,
                                       @IsPostedInAX,
                                       @Sequence,
                                       @ExceptionCode,
                                       @TransactionID,
                                       @Area,
									   @IsConsolidated

        SET @ROW = @ROW + 1
      END

      DROP TABLE #tempTable;
    END TRY
    BEGIN CATCH
      DECLARE @ErrorMessage nvarchar(4000);
      DECLARE @ErrorSeverity int;
      DECLARE @ErrorState int;
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
PRINT N'Creating Procedure [dbo].[USP_DOT_TOMsScanOut]...';


GO
-- ==================================================================================  
-- Name:   USP_DOT_TOMsScanOut
-- Purpose:  Pump TOMs Scan Out data into staging 
-- ==================================================================================  
-- Change History  
-- Date			Author			Comments  
-- -----		------			-----------------------------------------------------------------  
-- 18/07/2018  Muhd Khalid		SP created.  
-- 09/09/2021  Azrul			Open batch flag for NGC1.5.
-- ==================================================================================  
CREATE PROCEDURE [dbo].[USP_DOT_TOMsScanOut]  
@FGItemCode nvarchar(40),
@IsOrignalTemppack bit,
@Configuration nvarchar(10),
@GloveCode nvarchar(40),
@Warehouse nvarchar(10),
@PalletId nvarchar(20),
@PalletTotalQty numeric(32, 16),
@ScanOutDateTime datetime,
@BatchSerialNos nvarchar(max),
@Location nvarchar(10)
AS
BEGIN
  BEGIN TRANSACTION;
    BEGIN TRY
      --Parent table variable
      DECLARE @BATCHCARDNUMBER nvarchar(50),
              @BATCHNUMBER nvarchar(20),
              @FSIDENTIFIER uniqueidentifier,
              @FUNCTIONIDENTIFIER nvarchar(20),
              @PLANTNO nvarchar(20),
              @REFERENCEBATCHNUMBER1 nvarchar(20),
              @REFERENCEBATCHNUMBER2 nvarchar(20),
              @REFERENCEBATCHNUMBER3 nvarchar(20),
              @REFERENCEBATCHNUMBER4 nvarchar(20),
              @REFERENCEBATCHNUMBER5 nvarchar(20),
              @REFERENCEBATCHSEQUENCE1 int,
              @REFERENCEBATCHSEQUENCE2 int,
              @REFERENCEBATCHSEQUENCE3 int,
              @REFERENCEBATCHSEQUENCE4 int,
              @REFERENCEBATCHSEQUENCE5 int,
              @SEQUENCE int,
              --Transfer Journal Variable
              --@BRAND nvarchar(20),
              @FORMULA nvarchar(20),
              --@ITEMNUMBER nvarchar(20),
              --@LOCATION nvarchar(10),
              @PARENTREFRECID int,
              @QUANTITY numeric(32, 16),
              @ServiceName nvarchar(50),
              @PostingType nvarchar(20),
              @PostedDate datetime,
              @SerialNumber numeric(15, 0),
              @IsPostedToAX bit,
              @IsPostedInAX bit,
              @ExceptionCode nvarchar(1000),
              @TransactionID nvarchar(100),
              @Area nvarchar(10),
              @SPLITDATA nvarchar(50),
			  @LastService nvarchar(20),
			  @IsConsolidated bit -- #AZRUL 17/9/2021: Open batch flag for NGC1.5

		DECLARE @tempTable Table (Seq int, Result BIT) -- #AZRUL 17/9/2021: Open batch flag for NGC1.5

		if @IsOrignalTemppack=0 and isnull(@Warehouse,'')=''  
		 BEGIN  
		   RAISERROR ('Not orignal temp pack warehouse can not empty!', -- Message text.    
					   16, -- Severity.    
					   1 -- State.    
					   );    
		 END   

		if @IsOrignalTemppack=0 and isnull(@Location,'')=''  
		 BEGIN  
		   RAISERROR ('Not orignal temp pack warehouse''s location can not empty!', -- Message text.    
					   16, -- Severity.    
					   1 -- State.    
					   );    
		 END   

		if @IsOrignalTemppack=1 and isnull(@FGItemCode,'')=''  
		 BEGIN  
		   RAISERROR ('Orignal temp pack FG brand info can not empty!', -- Message text.    
					   16, -- Severity.    
					   1 -- State.    
					   );    
		 END   
		 
	  --SET @FSIDENTIFIER = NEWID();
      SELECT
        ROW_NUMBER() OVER (ORDER BY (SELECT
          1)
        ) AS id,
        Data INTO #tempTable
      FROM SPLIT(@BatchSerialNos, ',');

      DECLARE @COUNT int = (SELECT
        MAX(id)
      FROM #tempTable);
      DECLARE @ROW int = 1;

      WHILE (@ROW <= @COUNT)
      BEGIN
        SET @SPLITDATA = (SELECT
          Data
        FROM #tempTable
        WHERE id = @ROW);
        SET @BATCHNUMBER = SUBSTRING(@SPLITDATA, 0, CHARINDEX(':', @SPLITDATA));
        SET @BATCHCARDNUMBER = (SELECT TOP 1
          BatchcardNumber
        FROM DOT_FloorAxIntParentTable
        WHERE BatchNumber = @BATCHNUMBER AND IsDeleted=0);

		--check last posting, block if STPI
		select Top 1 @LastService = ServiceName from AXPostingLog where SerialNumber=@BATCHNUMBER order by CreationDate desc 

		if @LastService = 'STPO'
		BEGIN
			RAISERROR ('Temp pack already scan out!', -- Message text.    
						16, -- Severity.    
						1 -- State.    
						);    
		END
        
		SET @FSIDENTIFIER = NEWID(); -- fix Duplicated GUID, Max He, 4/1/2019
        SET @FUNCTIONIDENTIFIER = 'STPO';
        --SET @PLANTNO = SUBSTRING(@Warehouse, 0, 3);
        SET @REFERENCEBATCHNUMBER1 = NULL;
        SET @REFERENCEBATCHNUMBER2 = NULL;
        SET @REFERENCEBATCHNUMBER3 = NULL;
        SET @REFERENCEBATCHNUMBER4 = NULL;
        SET @REFERENCEBATCHNUMBER5 = NULL;
        SET @REFERENCEBATCHSEQUENCE1 = 0;
        SET @REFERENCEBATCHSEQUENCE2 = 0;
        SET @REFERENCEBATCHSEQUENCE3 = 0;
        SET @REFERENCEBATCHSEQUENCE4 = 0;
        SET @REFERENCEBATCHSEQUENCE5 = 0;
        SET @SEQUENCE = (SELECT
          COUNT(SerialNumber) + 1 AS 'BatchSequence'
        FROM dbo.AXPostingLog
        WHERE SerialNumber = @BATCHNUMBER
        AND (exceptioncode IS NULL
        OR exceptioncode = '999'));
		
		-- Find previous record plant no
		-- Special handling for STPI/STPO due to TOMs didn't send plant no, Max He, 19/07/2021
		select @PLANTNO = PlantNo from DOT_FloorAxIntParentTable WITH (NOLOCK) 
		where BatchNumber=@BATCHNUMBER and Sequence=@SEQUENCE-1 and IsDeleted=0 and IsMigratedFromAX6 = 0 AND IsDeleted=0;
		print @plantno;
        --SET @BRAND = '';
        SET @FORMULA = '';
        --SET @ITEMNUMBER = @GloveCode;
        --SET @LOCATION = '';
        SET @QUANTITY = SUBSTRING(@SPLITDATA, CHARINDEX(':', @SPLITDATA) + 1, LEN(@SPLITDATA));
        SET @ServiceName = @FUNCTIONIDENTIFIER;
        SET @PostingType = 'DOTTOMsScanOut';
        SET @PostedDate = GETDATE();
        SET @SerialNumber = @BATCHNUMBER;
        SET @IsPostedToAX = 1;
        SET @IsPostedInAX = 1;
        SET @ExceptionCode = NULL;
        SET @TransactionID = '-1';
        SET @Area = @LOCATION;

		-- #AZRUL 17/9/2021: Open batch flag for NGC1.5 START
		INSERT INTO @tempTable
			EXEC dbo.USP_GET_BATCHSEQUENCE @SerialNumber, @PLANTNO

		SET @IsConsolidated = (SELECT Result FROM @tempTable)
		DELETE @tempTable
		-- #AZRUL 17/9/2021: Open batch flag for NGC1.5 END

		--print @SEQUENCE
        --Transaction start
        IF NOT EXISTS (SELECT
            *
          FROM DOT_FloorAxIntParentTable WITH (NOLOCK)
          WHERE [BATCHCARDNUMBER] = @BATCHCARDNUMBER
          AND [BATCHNUMBER] = @BATCHNUMBER
          AND [FUNCTIONIDENTIFIER] = @FUNCTIONIDENTIFIER
          AND [PLANTNO] = @PLANTNO
		  AND [SEQUENCE] = @SEQUENCE
		  AND IsDeleted=0)
        BEGIN
          INSERT INTO [dbo].[DOT_FLOORAXINTPARENTTABLE] ([BATCHCARDNUMBER]
          , [BATCHNUMBER]
          , [CREATIONTIME]
          , [CREATORUSERID]
          , [DELETERUSERID]
          , [DELETIONTIME]
          , [ERRORMESSAGE]
          , [FSIDENTIFIER]
          , [FUNCTIONIDENTIFIER]
          , [ISDELETED]
          , [LASTMODIFICATIONTIME]
          , [LASTMODIFIERUSERID]
          , [PROCESSINGSTATUS]
          , [PLANTNO]
          , [PRODID]
          , [REFERENCEBATCHNUMBER1]
          , [REFERENCEBATCHNUMBER2]
          , [REFERENCEBATCHNUMBER3]
          , [REFERENCEBATCHNUMBER4]
          , [REFERENCEBATCHNUMBER5]
          , [REFERENCEBATCHSEQUENCE1]
          , [REFERENCEBATCHSEQUENCE2]
          , [REFERENCEBATCHSEQUENCE3]
          , [REFERENCEBATCHSEQUENCE4]
          , [REFERENCEBATCHSEQUENCE5]
          , [SEQUENCE]
          , [PALLETID]
		  , [IsConsolidated]) -- #AZRUL 17/9/2021: Open batch flag for NGC1.5
            VALUES (@BATCHCARDNUMBER, @BATCHNUMBER, GETDATE(), 1, NULL, NULL, '', @FSIDENTIFIER, @FUNCTIONIDENTIFIER, 0, GETDATE(), 1, 1, @PLANTNO, NULL, @REFERENCEBATCHNUMBER1, @REFERENCEBATCHNUMBER2, @REFERENCEBATCHNUMBER3, @REFERENCEBATCHNUMBER4, @REFERENCEBATCHNUMBER5, @REFERENCEBATCHSEQUENCE1, @REFERENCEBATCHSEQUENCE2, @REFERENCEBATCHSEQUENCE3, @REFERENCEBATCHSEQUENCE4, @REFERENCEBATCHSEQUENCE5, @SEQUENCE, @PalletId, @IsConsolidated);

          SET @PARENTREFRECID = (SELECT
            @@IDENTITY);
        END
        ELSE
        BEGIN
          SET @PARENTREFRECID = (SELECT
            Id
          FROM DOT_FloorAxIntParentTable WITH (NOLOCK)
          WHERE [BATCHCARDNUMBER] = @BATCHCARDNUMBER
          AND [BATCHNUMBER] = @BATCHNUMBER
          AND [FUNCTIONIDENTIFIER] = @FUNCTIONIDENTIFIER
          AND [PLANTNO] = @PLANTNO
		  AND [SEQUENCE] = @SEQUENCE
		  AND IsDeleted=0);
        END

        EXEC dbo.USP_DOT_CreateTransferJournal @BATCHCARDNUMBER,
                                               @BATCHNUMBER,
                                               @FGItemCode, --@BRAND,
                                               @Configuration,
                                               @FORMULA,
                                               @GloveCode,
                                               @LOCATION,
                                               @PARENTREFRECID,
                                               @QUANTITY,
                                               @ScanOutDateTime,
                                               @ScanOutDateTime,
                                               '',
                                               @Warehouse,
											   @IsOrignalTemppack;

        EXEC dbo.USP_SAVE_AXPOSTINGLOG @ServiceName,
                                       @PostingType,
                                       @PostedDate,
                                       @BATCHCARDNUMBER,
                                       @SerialNumber,
                                       @IsPostedToAX,
                                       @IsPostedInAX,
                                       @Sequence,
                                       @ExceptionCode,
                                       @TransactionID,
                                       @Area,
									   @IsConsolidated

        SET @ROW = @ROW + 1
      END

      DROP TABLE #tempTable;
    END TRY
    BEGIN CATCH
      DECLARE @ErrorMessage nvarchar(4000);
      DECLARE @ErrorSeverity int;
      DECLARE @ErrorState int;
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
PRINT N'Creating Procedure [dbo].[USP_DOT_TumblingResource_Get]...';


GO
-- =============================================================================      
-- Name:   USP_DOT_TumblingResource_Get  
-- Purpose:   Get Resource and Batch Order details for Print Normal Batch Card   
-- =============================================================================    
-- Change History    
-- Date    Author   Comments    
-- -----   ------   ------------------------------------------------------------    
-- 24/12/2020  Azrul Amin    SP created.    
-- =============================================================================      
  
CREATE PROCEDURE [dbo].[USP_DOT_TumblingResource_Get]    
(     
 @LocationId Int,     
 @LineId varchar(20),    
 @Resource varchar(20),     
 @BO varchar(20)  
)     
AS    
BEGIN       
 SET NOCOUNT ON;    
    
 -- #1.List out all ResourceGroup (line).    
 IF (@LineId IS NULL) OR (LEN(@LineId) < 0)     
 BEGIN    
  SELECT DISTINCT    
  '' as Resource    
  ,'' as ResourceId    
  ,'' as LocationId    
  ,'' as Plant    
  ,dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) as LineId  
  ,res.ResourceGrp as Line    
  ,'' as TierSide    
  ,'' as BatchOrder    
  ,'' as GloveCode    
  ,'' as Size    
  FROM     
   DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN    
   DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN    
   LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName JOIN
   DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.    
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND bo.ProdStatus = 'StartedUp'     
  AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) in (select linenumber from linemaster where LocationId = @LocationId) --#AZRUL-BUG 1179: Remove Invalid Lines In Glove Output Reporting.  
  AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon') --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
 END    
    
-- #2.List out all Resource    
ELSE IF ((@Resource IS NULL) OR (LEN(@Resource) = 0))    
 BEGIN    
  IF ((@BO IS NULL) OR (LEN(@BO) = 0))    
  BEGIN    
   SELECT DISTINCT    
   res.Resource    
   ,'' as ResourceId    
   ,'' as LocationId    
   ,'' as Plant    
   ,dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) as LineId  
   ,res.ResourceGrp as Line  
   ,dbo.UDF_DOT_GetTierSidefromResource(res.Resource) as TierSide  
   ,'' as BatchOrder    
   ,'' as GloveCode    
   ,'' as Size    
   FROM   
    DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
    DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
    LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName JOIN
    DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
   WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) = @LineId AND bo.ProdStatus = 'StartedUp'   
   and bo.IsDeleted=0 AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon')  --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  END  
  ELSE    
  BEGIN    
   SELECT DISTINCT    
   res.Resource    
   ,'' as ResourceId    
   ,'' as LocationId    
   ,'' as Plant    
   ,dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) as LineId  
   ,res.ResourceGrp as Line  
   ,dbo.UDF_DOT_GetTierSidefromResource(res.Resource) as TierSide  
   ,'' as BatchOrder    
   ,'' as GloveCode    
   ,'' as Size    
   FROM   
    DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN  
    DOT_FloorD365BOResource AS res WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
    LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName JOIN
    DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
   WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) = @LineId AND bo.ProdStatus = 'StartedUp'   
   and bo.IsDeleted=0 and bo.BthOrderId = @BO AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon')   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  END  
 END    
    
-- #3.To list out all Batch Order based on selected Resource.    
ELSE IF ((@BO IS NULL) OR (LEN(@BO) = 0))    
 BEGIN    
  SELECT     
  res.Resource    
  ,res.Id as ResourceId    
  ,loc.LocationId    
  ,dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) as Plant  
  ,dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) as LineId  
  ,res.ResourceGrp as Line  
  ,dbo.UDF_DOT_GetTierSidefromResource(res.Resource) as TierSide  
  ,bo.BthOrderId as BatchOrder    
  ,bo.ItemId as GloveCode    
  ,bo.Size    
  FROM   
   DOT_FloorD365BO (NOLOCK) AS bo LEFT JOIN  
   DOT_FloorD365BOResource (NOLOCK) AS res ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName JOIN
   DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) = @LineId  
  AND res.Resource = @Resource AND bo.ProdStatus = 'StartedUp'  
  and bo.IsDeleted=0 AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon')   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
 END  
    
-- #4.To list out all Batch Order Details based on selected Resource and Batch Order.    
ELSE    
 BEGIN    
  SELECT     
  res.Resource    
  ,res.Id as ResourceId    
  ,loc.LocationId    
  ,dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) as Plant  
  ,dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) as LineId  
  ,res.ResourceGrp as Line  
  ,dbo.UDF_DOT_GetTierSidefromResource(res.Resource) as TierSide  
  ,bo.BthOrderId as BatchOrder    
  ,bo.ItemId as GloveCode    
  ,bo.Size    
  FROM   
   DOT_FloorD365BO AS bo  WITH (NOLOCK) LEFT JOIN  
   DOT_FloorD365BOResource  AS res  WITH (NOLOCK) ON bo.BthOrderId = res.BatchOrderId and res.IsDeleted=0 LEFT JOIN  
   LocationMaster as loc ON dbo.UDF_DOT_GetPlantfromResourceGrp(res.ResourceGrp) = loc.LocationName JOIN
   DOT_FSItemMaster as item WITH (NOLOCK) on item.ItemId = bo.ItemId   --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC.
  WHERE bo.ProdPoolId = 'Glove' AND loc.LocationId = @LocationId AND dbo.UDF_DOT_GetLinefromResourceGrp(res.ResourceGrp) = @LineId   
  AND res.Resource = @Resource AND bo.ProdStatus = 'StartedUp' AND bo.BthOrderId = @BO  
  and bo.IsDeleted=0 AND item.IsDeleted = 0 and item.ItemTypeCode not in ('Clean Room','Semicon')  --HTLG_HSB_002: Special Glove - CLRP & SCON not allowed to print HBC/SRBC. 
 END  
 SET NOCOUNT OFF;      
END
GO
PRINT N'Creating Procedure [dbo].[USP_DOT_ValidateSOBCPosting]...';


GO
-- ==================================================================================================    
-- Name:   [USP_DOT_ValidateSOBCPosting]    
-- Purpose:   Validate SOBC Posting Count    
-- ==================================================================================================    
-- Change History    
-- Date               Author                     Comments    
-- -----   ------   ---------------------------------------------------------------------------------    
--  7, 1,2019    Max He       SP created.    
-- ==================================================================================================    
CREATE PROCEDURE [dbo].[USP_DOT_ValidateSOBCPosting]    
 @serialNo NUMERIC(10,0)    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
 DECLARE @ReworkOrderCount INT    
 DECLARE @SOBCCount INT    
    
 SELECT @ReworkOrderCount=COUNT(1)    
 FROM AXPostingLog WITH (NOLOCK)     
 WHERE ServiceName='RWKCR' AND SerialNumber=@serialNo    
 group by ServiceName;    
    
     
 SELECT @SOBCCount=COUNT(1)    
 FROM AXPostingLog WITH (NOLOCK)     
 WHERE ServiceName='SOBC' AND SerialNumber=@serialNo    
 GROUP BY ServiceName;    
    
 SET @ReworkOrderCount = ISNULL(@ReworkOrderCount,0);    
 SET @SOBCCount   = ISNULL(@SOBCCount,0);    
    
 IF @ReworkOrderCount-@SOBCCount=1 -- allow insert SOBC    
  BEGIN    
   SELECT 1;    
  END    
 ELSE IF @ReworkOrderCount-@SOBCCount>1 -- allow insert SOBC if got extra rework from HBC RESAMPLE   
  BEGIN    
    DECLARE @ResampleCount INT    
  
 select @ResampleCount=COUNT(1)   
 from DOT_FloorAxIntParentTable a with (nolock)   
 join DOT_RafStgTable c with (nolock) on a.id = c.ParentRefRecId  
 join DOT_FloorD365HRGLOVERPT b with (nolock) on a.BatchNumber = b.SerialNo and b.Resource = c.Resource  
 where a.BatchNumber = convert(varchar,@serialNo) and a.FunctionIdentifier = 'HBC'   
 and a.ReferenceBatchNumber1 = 'RESAMPLE' and b.SeqNo = 1  
  
 SET @ResampleCount = ISNULL(@ResampleCount,0);   
  
 IF @ReworkOrderCount-@ResampleCount-@SOBCCount = 1  
  BEGIN  
   SELECT 1;  
  END  
 ELSE  
  BEGIN  
   SELECT 0;  
  END   
  END    
 ELSE IF @ReworkOrderCount-@SOBCCount=0  -- SOBC create before rework order, should pop error    
  BEGIN    
	IF EXISTS (select 1 from DOT_OpenBatchCard where BatchNumber = @serialNo) --handle for open batch always return 1
		BEGIN
			SELECT 1;
		END
	ELSE
		BEGIN
			SELECT -1;
		END
  END    
 ELSE    
  BEGIN    
   Select 0; -- SOBC tally with rework, should not create SOBC any more    
  END    
 SET NOCOUNT OFF;     
    
END
GO
PRINT N'Creating Procedure [dbo].[USP_EWN_CheckPalletStatus]...';


GO
-- =============================================
-- Name:			[USP_EWN_CheckPalletStatus]
-- Purpose: 		to check whether a pallet is available, 
--					 For PalletMaster, the new SP will check if IsAvailable column equals to 1 and IsOccupied column equals to 0
--                   For EWN_CompletedPallet, the new SP will check if DateStockOut column is not null. 
--                  When both checking are true, the new SP will return 1, else return 0
-- =============================================
-- Change History
-- Date			Author				Comments
-- -----		------				-----------------------------
-- 26/11/2019 	Pang Yik Siu		SP created.
-- =============================================

CREATE PROCEDURE [dbo].[USP_EWN_CheckPalletStatus]
	@PalletID			varchar(8)
	,@PONum				varchar(20)
	,@Item				varchar(20)
	,@PalletIsAvailable	bit OUTPUT
AS
BEGIN
	SET @PalletIsAvailable = 0

	DECLARE @PalletMasterValidation			bit = 0
	DECLARE @EWN_CompletedPalletValidation	bit = 0

	SET @PalletMasterValidation = 0;

	IF EXISTS(SELECT 1 FROM PalletMaster(NOLOCK) WHERE PalletId = @PalletID AND IsAvailable = 1 AND Isoccupied = 0)
		SET @PalletMasterValidation = 1

	IF EXISTS(SELECT 1 FROM EWN_CompletedPallet(NOLOCK) WHERE PalletId = @PalletID AND PONumber = @PONum AND Item = @Item AND DateStockOut IS NOT NULL)
		SET @EWN_CompletedPalletValidation = 1

	IF (@PalletMasterValidation = 1 AND @EWN_CompletedPalletValidation = 1)
		SET @PalletIsAvailable = 1

	SELECT @PalletIsAvailable
	RETURN @PalletIsAvailable
END
GO
PRINT N'Creating Procedure [dbo].[USP_EWN_GetPalletData_WebAdmin]...';


GO

-- =============================================    
-- Name:   [USP_EWN_GetPalletData_WebAdmin]    
-- Purpose:   Get record of the pallet in the eWareNavi table    
-- =============================================    
-- Change History    
-- Date   Author    Comments    
-- -----  ------    -----------------------------    
-- 20/04/2019  Max He filter out must IOT scanned only post to ewarenavi  
-- =============================================    
    
CREATE PROCEDURE [dbo].[USP_EWN_GetPalletData_WebAdmin]    
(    
 @PalletID   varchar(8)    
)     
AS    
BEGIN     
     
 Select * from EWN_CompletedPallet (nolock)    
 where PalletId=@PalletID and DateStockOut IS NULL and DateScanned IS NOT NULL
 order by DateCompleted desc    
    
END
GO
PRINT N'Creating Procedure [dbo].[usp_FP_Resource_Get]...';


GO

---- =================================================================  
---- Author:  <Azrul Amin>  
---- Create date: <11-Mar-2019>  
---- Description: Get Resource No for Final Packing
---- =================================================================  
CREATE PROCEDURE [dbo].[usp_FP_Resource_Get]

	 @FGBatchOrderNo nvarchar(20)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT rs.Resource from Dot_FloorD365BO bo WITH (NOLOCK)     
	left join Dot_FloorD365BOResource rs WITH (NOLOCK) ON rs.BatchOrderId = bo.BthOrderId
	WHERE bo.IsDeleted=0 and bo.ProdPoolId <> 'PSI' and bo.ProdStatus = 'StartedUp' 
	and rs.IsDeleted=0  and bo.ProdPoolId = 'FG'
	and bo.BthOrderId = @FGBatchOrderNo

END
GO
PRINT N'Creating Procedure [dbo].[usp_FP_RollBackEWNCaseQty]...';


GO
-- =============================================
-- Author:		Azrul Amin
-- Create date: 01 Mar 2019
-- Description:	Roll back SP for Final Packing Print Inner AND Outer
-- =============================================
CREATE PROCEDURE [dbo].[usp_FP_RollBackEWNCaseQty]
	@PONumber nvarchar(15),
	@palletId nvarchar(15),
	@itemNumber nvarchar(15),
	@size nvarchar(15)
AS
BEGIN
BEGIN TRANSACTION;
	BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

		DECLARE @qty INT
		SELECT @qty = Count(CaseNumber) FROM PurchaseOrderItemCases (NOLOCK)
					  WHERE PONumber = @PONumber AND PalletId = @palletId AND ItemNumber = @itemNumber AND size = @size
		SELECT @qty
		UPDATE EWN_CompletedPallet SET qty = @qty  
			   WHERE PONumber = @PONumber AND PalletId = @palletId AND Item = @itemNumber+'_'+@size AND DateStockOut IS NULL

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
GO
PRINT N'Creating Procedure [dbo].[usp_FP_RollBackEWNForSurgical]...';


GO
-- =============================================  
-- Author:  Azrul Amin  
-- Create date: 17 Mar 2021  
-- Description: Roll back SP for Surgical Print Inner AND Outer  
-- =============================================  
CREATE PROCEDURE [dbo].[usp_FP_RollBackEWNForSurgical]  
 @PONumber nvarchar(15),  
 @palletId nvarchar(15),  
 @itemNumber nvarchar(15),  
 @size nvarchar(15)  
AS  
BEGIN  
BEGIN TRANSACTION;  
 BEGIN TRY  
 -- SET NOCOUNT ON added to prevent extra result sets from  

Delete EWN_CompletedPallet where PONumber = @PONumber and Item = @itemNumber+'_'+@size 
and PalletId = @palletId and DateStockOut IS NULL

Update PalletMaster set IsAvailable = 1, Isoccupied = 0 where PalletId = @palletId
  
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
GO
PRINT N'Creating Procedure [dbo].[USP_GET_AXPostingLogInfo]...';


GO
-- =======================================================
-- Name:             USP_GET_AXPostingLogInfo
-- Purpose:          Check AXPostingLog is it exsist
-- =======================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     -----------------------------
-- 2/09/2018    Max He    SP created.
-- =======================================================
Create PROCEDURE [dbo].[USP_GET_AXPostingLogInfo]
(
	@SerialNumber decimal,
	@ServiceName  varchar(10)
)
AS
BEGIN

	SELECT count(1) FROM AXPostingLog where ServiceName = @ServiceName and SerialNumber = @SerialNumber;

END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_BatchOrderBySerialNo]...';


GO
-- =====================================================================    
-- Name:   USP_GET_BatchOrderBySerialNo  
-- Purpose:   Get Batch Order No from Serial No    
-- =====================================================================    
-- Change History    
-- Date    Author   Comments    
-- -----   ------   ----------------------------------------------------    
-- 24/12/2021  Azrul Amin    SP created.    
-- ===================================================================== 
CREATE PROCEDURE [dbo].[USP_GET_BatchOrderBySerialNo]    
(    
 @serialNo Decimal
)    
AS    
BEGIN    
	SELECT BthOrder FROM DOT_FloorD365HRGLOVERPT WITH (NOLOCK) WHERE SerialNo=@serialNo    
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_CBCILocation]...';


GO
  
  
-- =======================================================  
-- Name:             USP_GET_CBCILocation  
-- Purpose:          Check CBCI Location to update Rework Location 
-- =======================================================  
-- Change History  
-- Date         Author     Comments  
-- -----        ------     -----------------------------  
-- 26/06/2019   Azrul    SP created.  
-- =======================================================  
CREATE PROCEDURE [dbo].[USP_GET_CBCILocation]  
(  
 @SerialNumber nvarchar(100)  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  

 select top 1 PlantNo from DOT_FloorAxIntParentTable with (nolock) where BatchNumber=@SerialNumber 
 and FunctionIdentifier = 'CBCI' order by CreationTime desc

END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_CheckIsPTPF]...';


GO

-- ================================================================================
-- Name:			[dbo].[USP_GET_CheckIsPTPF]
-- Purpose: 		<Checks whether this batch is PTPF glove
-- ================================================================================
-- Change History
-- Date			Author			   Comments
-- ----------	------------	   ------------------------------------------------
-- 23/07/2020 	Pang Yik Siu	   SP created
-- exec USP_GET_CheckIsPTPF '2200635664'

-- ================================================================================
CREATE PROCEDURE [dbo].[USP_GET_CheckIsPTPF]
(
	@SerialNumber numeric
)
AS
BEGIN

	--DECLARE @SerialNumber numeric = 2200635664
	SET NOCOUNT ON;
	DECLARE @isPTPF BIT = 0

	IF EXISTS (SELECT 1 FROM Batch (nolock) WHERE SerialNumber = @SerialNumber)
	BEGIN 
		SET @isPTPF = (SELECT ISNULL(c.PTGLOVE, 0) FROM Batch (nolock) b 
						LEFT JOIN AX_AVAGLOVECODETABLE_EXTENSION (NOLOCK) c ON c.GLOVECODE = b.GloveType
						WHERE b.SerialNumber = @SerialNumber)
	END

	SELECT @isPTPF
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_CheckIsPTPFForFinalPack]...';


GO
  
-- ================================================================================  
-- Name:   [dbo].[USP_GET_CheckIsPTPFForFinalPack]  
-- Purpose:   <Checks whether this batch is PTPF glove for Final Pack validation  
-- ================================================================================  
-- Change History  
-- Date   Author      Comments  
-- ---------- ------------    ------------------------------------------------  
-- 4/1/2022  Azrul    SP created  
-- exec USP_GET_CheckIsPTPFGloveForFinalPack  1220563617
-- ================================================================================  
CREATE PROCEDURE [dbo].[USP_GET_CheckIsPTPFForFinalPack]  
(  
 @SerialNumber numeric  
)  
AS  
BEGIN  
  
 SET NOCOUNT ON;  
 DECLARE @isPTPF BIT = 0  
  
 IF EXISTS (SELECT 1 FROM Batch a with (nolock) join DOT_FSItemMaster b with (nolock) 
	  on a.GloveType = b.ItemId and b.ItemTypeCode = 'PTPF'
	  where SerialNumber = @SerialNumber)  
 BEGIN   
  SET @isPTPF = 1
 END  
 ELSE
 BEGIN
  SET @isPTPF = 0
 END

 SELECT @isPTPF  
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_CompleteBatchDetailsByResource]...';


GO
-- ==========================================================================
-- Name:             USP_GET_CompleteBatchDetailsByResource
-- Purpose:          Get complete batch details by serial number and resource
-- ==========================================================================
-- Change History
-- Date				Author      Comments
-- -----			------      ---------------------------------------------
-- 14/05/0218		Azrul		SP created.
-- ========================================================================== 
CREATE PROCEDURE [dbo].[USP_GET_CompleteBatchDetailsByResource]
(
	@serialNo decimal,
	@resource varchar(50)
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT d.SeqNo,b.SerialNumber,b.BatchNumber,d.BthOrder AS BatchOrder,d.Resource,b.GloveType,b.LineId,s.name AS 'ShiftName',b.Size,b.TenPCsWeight,
	(SELECT ((SUM(PackingSz*InBox))*b.TenPCsWeight)/10000 FROM DOT_FloorD365HRGLOVERPT with (nolock) WHERE SerialNo = @serialNo AND Resource = @resource)
	 as BatchWeight,(d.PackingSz*d.InBox) AS TotalPCs,b.IsOnline,b.QCType,b.ModuleId,b.SubModuleId,b.BatchType,l.locationname,b.BatchCardDate,
	DATEADD(DAY,1,b.BatchCardDate) as DeliveryDate, (SELECT RouteCategory from DOT_FSQCTypeTable with (nolock) where QCType = b.QCType) as RouteCategory,
	(SELECT RouteCategory from DOT_FSQCTypeTable with (nolock) where QCType = b.QCType) as [Pool],
	'PN' as Area FROM Batch b with (nolock) JOIN locationmaster l 
	ON b.LocationId=l.LocationId
	JOIN shiftmaster s with (nolock) ON b.ShiftId= s.ShiftId
	LEFT JOIN DOT_FloorD365HRGLOVERPT d WITH (NOLOCK) ON b.SerialNumber = d.SerialNo --AND b.BatchNumber = d.BatchCardNumber
	WHERE SerialNumber = @serialNo AND Resource = @resource 
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_CurrentDateAndHour]...';


GO

-- ========================================================
-- Name:                   [dbo].[USP_GET_CurrentDateAndHour]
-- Purpose:          <Get Current Date and Hour from Server>
-- ========================================================
-- Change History
-- Date              Author                     Comments
-- -----             ------                     ------------
-- 13/12/2018        Azrul				SP created
-- ========================================================

CREATE PROCEDURE [dbo].[USP_GET_CurrentDateAndHour]
AS
BEGIN
	declare
	  @dt datetime;
	select @dt = cast(cast(getdate() as date) as datetime)+cast(datepart(hour,getdate()) as float)/24
	select @dt
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_GetLatestReworkQty]...';


GO

-- ================================================================================
-- Name:			USP_GET_GetLatestReworkQty
-- Purpose: 		Get latest rework qty to do calculate rejected sample when qcqi
-- ================================================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     --------------------------------------------------------
-- 12/07/2019 	Azrul	   SP created.
-- ================================================================================
CREATE PROCEDURE [dbo].[USP_GET_GetLatestReworkQty]
	@SerialNumber		nvarchar(50)
AS
BEGIN	
BEGIN TRANSACTION;
BEGIN TRY
	SET NOCOUNT ON
	select Quantity from DOT_RwkBatchOrderCreationChildTable with (nolock) where ParentRefRecId in 
	(select Id from DOT_FloorAxIntParentTable with (nolock) 
		where BatchNumber = @SerialNumber and FunctionIdentifier = 'RWKCR')
	 order by id desc
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
		THROW;
END CATCH;

IF @@TRANCOUNT > 0
  COMMIT TRANSACTION;

  END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_GloveType]...';


GO
-- =======================================================

-- Name:			[USP_GET_GloveType]

-- Purpose: 		Gets GloveType by GloveType or BarCode

-- =======================================================

-- Change History

-- Date         Author     Comments

-- -----        ------     -----------------------------

-- 28/05/2018 	Azrul	   SP created.
-- =======================================================

CREATE PROCEDURE [dbo].[USP_GET_GloveType]
(
                @GloveType NVARCHAR(100)
)
AS
BEGIN

                IF ISNUMERIC(@GloveType) = 1
                                SELECT b.ItemId from DOT_FSGloveCode a join DOT_FSItemMaster b on a.ItemRecordId = b.Id where a.Barcode = @GloveType
                ELSE
                                SELECT @GloveType
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_HBCInBoxFromSerialNo]...';


GO
-- =======================================================  
-- Name:   USP_GET_HBCInBoxFromSerialNo
-- Purpose:   Get HBC Inner Box from Serial Number
-- =======================================================  
-- Change History  
-- Date         Author     Comments  
-- -----        ------     -----------------------------  
-- 27 Nov 2018  Azrul      SP created.  
-- =======================================================  
CREATE PROCEDURE [dbo].[USP_GET_HBCInBoxFromSerialNo]  
 @SerialNumber  BIGINT  
AS  
BEGIN   
  
 SET NOCOUNT ON  

		SELECT STUFF((SELECT ', ' + CONVERT(NVARCHAR,a.InBox) FROM dbo.DOT_FloorD365HRGLOVERPT a WITH (NOLOCK)
		WHERE a.SerialNo = @SerialNumber
		FOR XML path('') ), 1, 2, '') AS InnerBox
		FROM dbo.DOT_FloorD365HRGLOVERPT WITH (NOLOCK)
		WHERE SerialNo = @SerialNumber
		GROUP BY SerialNo

END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_HBCPackSizeFromSerialNo]...';


GO
-- =======================================================  
-- Name:   USP_GET_HBCPackSizeFromSerialNo
-- Purpose:   Get HBC Packing Size from Serial Number
-- =======================================================  
-- Change History  
-- Date         Author     Comments  
-- -----        ------     -----------------------------  
-- 27 Nov 2018  Azrul      SP created.  
-- =======================================================  
CREATE PROCEDURE [dbo].[USP_GET_HBCPackSizeFromSerialNo]  
 @SerialNumber  BIGINT  
AS  
BEGIN   
  
 SET NOCOUNT ON  
  
		--Emergency change by Azman 17/02
		--SELECT STUFF((SELECT ', ' + CONVERT(NVARCHAR,a.PackingSz) FROM dbo.DOT_FloorD365HRGLOVERPT a WITH (NOLOCK)
		--WHERE a.SerialNo = @SerialNumber
		--FOR XML path('') ), 1, 2, '') AS PackingSize
		--FROM dbo.DOT_FloorD365HRGLOVERPT WITH (NOLOCK)
		--WHERE SerialNo = @SerialNumber
		--GROUP BY SerialNo

		SELECT STUFF((SELECT ', ' + CONVERT(NVARCHAR,a.PackingSz) FROM dbo.DOT_FloorD365HRGLOVERPT a WITH (NOLOCK)
		WHERE a.SerialNo = CAST(@SerialNumber as nvarchar(20))
		FOR XML path('') ), 1, 2, '') AS PackingSize
		FROM dbo.DOT_FloorD365HRGLOVERPT WITH (NOLOCK)
		WHERE SerialNo = CAST(@SerialNumber as nvarchar(20))
		GROUP BY SerialNo

END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_HBCRePrintTierSide]...';


GO
-- =========================================================  
-- Name:   USP_HBCSerialNo_Get
-- Purpose:   Get HBC Serial Number 
-- =========================================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   ----------------------------------------  
-- 11/05/2018  Azrul Amin    SP created.  
-- =========================================================  
CREATE PROCEDURE [dbo].[USP_GET_HBCRePrintTierSide]  
(
 @outputTime datetime,
 @resourceGrp varchar(10)
 
) 
AS  
BEGIN   
 SET NOCOUNT ON;  
	select distinct substring(resource,6,3) AS TierSide from DOT_FloorD365HRGLOVERPT  WITH (NOLOCK) where OutTime = @outputTime and 
			Plant = substring(@resourceGrp,0,3) and LineId = substring(@resourceGrp,3,3)
 SET NOCOUNT OFF;  
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_IsChangeQCType]...';


GO

-- =======================================================
-- Name:			[USP_GET_IsChangeQCType]
-- Purpose: 		Get Is Create Rework Order
-- =======================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     -----------------------------
-- 18/07/2018 	Azrul	   SP created.
-- =======================================================
CREATE PROCEDURE [dbo].[USP_GET_IsChangeQCType]
	@SerialNumber		varchar(100) 
AS
BEGIN	
SET NOCOUNT ON
	SELECT TOP 1 ISNULL(IsChangeQCType,0) FROM Batch WHERE SerialNumber = @SerialNumber 
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_IsPostWT]...';


GO

-- =======================================================
-- Name:			[USP_GET_IsPostWT]
-- Purpose: 		Get Is Post Water Tight to D365
-- =======================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     -----------------------------
-- 09/07/2018 	Azrul	   SP created.
-- =======================================================
CREATE PROCEDURE [dbo].[USP_GET_IsPostWT]
	@QCType		varchar(100) 
AS
BEGIN	
SET NOCOUNT ON
	SELECT IsPostWT FROM DOT_FSQCTypeTable WHERE QCType = @QCType
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_IsPrevReworkWithoutSOBC]...';


GO
  
-- ========================================================= 
-- Name:   [USP_GET_IsPrevReworkWithoutSOBC]  
-- Purpose: Get Previous Rework is without SOBC (OQC to OQC)
-- =========================================================
-- Change History  
-- Date         Author     Comments  
-- -----        ------     --------------------------------- 
-- 16/07/2019   Azrul	   SP created.  
-- 30/11/2020	Max He	   filter out isRWKDeleted 
-- ========================================================= 
CREATE PROCEDURE [dbo].[USP_GET_IsPrevReworkWithoutSOBC]  
 @SerialNumber nvarchar(100)   
AS  
BEGIN   
SET NOCOUNT ON  

DECLARE @RWKCRCount int
DECLARE @SOBCCount int

SELECT @RWKCRCount = Count(ItemNumber) from DOT_RwkBatchOrderCreationChildTable with (nolock) WHERE isRWKDeleted = 0 and ParentRefRecId IN (  
     SELECT Id FROM DOT_FloorAxIntParentTable with (nolock) WHERE BatchNumber = @SerialNumber AND FunctionIdentifier = 'RWKCR' AND RouteCategory = 'OQC' and IsDeleted=0)
SELECT @SOBCCount = Count(BatchNumber) from DOT_FloorAxIntParentTable with (nolock) where BatchNumber = @SerialNumber and FunctionIdentifier in ('SOBC') and IsDeleted=0

IF (@RWKCRCount > @SOBCCount)
 BEGIN  
  SELECT 1  
 END  
 ELSE  
 BEGIN  
  SELECT 0  
 END  
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_IsPrevSPBCCreated]...';


GO
-- =========================================================   
-- Name:   [USP_GET_IsPrevSPBCCreated]    
-- Purpose: Get Previous SPBC created for 2nd PT Batch Card Scan 
-- =========================================================  
-- Change History    
-- Date         Author     Comments    
-- -----        ------     ---------------------------------   
-- 30/11/2020   Max He    SP created.    
-- =========================================================   
CREATE PROCEDURE [dbo].[USP_GET_IsPrevSPBCCreated]    
 @SerialNumber numeric     
AS    
BEGIN     
SET NOCOUNT ON    
  
DECLARE @MaxRecordId int  
DECLARE @MinRecordId int  
DECLARE @LastServiceName nvarchar(100)  =''
  
SELECT @MaxRecordId = max(id),@MinRecordId=min(id) from AXPostingLog with(nolock) where SerialNumber=@SerialNumber;

SELECT @LastServiceName = ServiceName from AXPostingLog with(nolock) where SerialNumber=@SerialNumber and id<=@MaxRecordId and id>@MinRecordId and ServiceName ='SPBC';
  
IF (@LastServiceName <>'SPBC')  
 BEGIN    
  SELECT 0    
 END    
 ELSE    
 BEGIN    
  SELECT 1    
 END    
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_LastServiceName]...';


GO
    
    
-- =======================================================    
-- Name:             USP_GET_LastServiceName    
-- Purpose:          Check Last Sequence Service Name  
-- =======================================================    
-- Change History    
-- Date         Author     Comments    
-- -----        ------     -----------------------------    
-- 26/06/2019   Azrul    SP created.    
-- =======================================================    
CREATE PROCEDURE [dbo].[USP_GET_LastServiceName]    
(    
 @SerialNumber decimal    
)    
AS    
BEGIN    
 SET NOCOUNT ON;    

 if exists (  select top 1 ServiceName from AXPostingLog with (nolock) where SerialNumber = @SerialNumber)
    select top 1 ServiceName from AXPostingLog with (nolock) where SerialNumber = @SerialNumber order by CreationDate desc
else
    select ''
  
  
END
GO
PRINT N'Creating Procedure [dbo].[USP_Get_Prev_SPBC_PlantNo]...';


GO

-- ==========================================================================
-- Name:			[USP_Get_Prev_SPBC_PlantNo]
-- Purpose: 		Get Previous SPBC PlantNo for compare with Rework PlantNo
-- ==========================================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     --------------------------------------------------
-- 18/06/2019 	Azrul	   SP created.
-- ==========================================================================
CREATE PROCEDURE [dbo].[USP_Get_Prev_SPBC_PlantNo]
	@SerialNumber nvarchar(20)
AS
BEGIN	
SET NOCOUNT ON
	SELECT TOP 1 PlantNo from DOT_FloorAxIntParentTable with (nolock) 
	where BatchNumber = @SerialNumber and FunctionIdentifier = 'SPBC' 
	and IsDeleted = 0
	order by Id desc
END
GO
PRINT N'Creating Procedure [dbo].[USP_Get_PreviousReworkIsOQC]...';


GO

-- =======================================================
-- Name:			[USP_Get_PreviousReworkIsOQC]
-- Purpose: 		Get Previous OQC Rework
-- =======================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     -----------------------------
-- 27/08/2018 	Azrul	   SP created.
-- 30/11/2020	Max He	   filter out isRWKDeleted 
-- =======================================================
CREATE PROCEDURE [dbo].[USP_Get_PreviousReworkIsOQC]
	@SerialNumber nvarchar(100) 
AS
BEGIN	
SET NOCOUNT ON
	IF EXISTS(SELECT * FROM DOT_RwkBatchOrderCreationChildTable WHERE isRWKDeleted = 0 and ParentRefRecId IN (
			  SELECT Id FROM DOT_FloorAxIntParentTable WHERE BatchNumber = @SerialNumber AND FunctionIdentifier = 'RWKCR' AND RouteCategory = 'OQC'))
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_PrevTotalPcsQCPackingYield]...';


GO

-- =======================================================
-- Name:			USP_GET_PrevTotalPcsForQCPackingYield
-- Purpose: 		Get Prev Total Pcs for Scan Out Batch
-- =======================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     -----------------------------
-- 11/01/2019 	Azrul	   SP created.
-- =======================================================
CREATE PROCEDURE [dbo].[USP_GET_PrevTotalPcsQCPackingYield]
	@SerialNumber		BIGINT
AS
BEGIN	
BEGIN TRANSACTION;
BEGIN TRY
	
	SET NOCOUNT ON

	DECLARE @SOBCCount INT = (Select MAX(SOBCCount)from QCYieldAndPacking WITH(NOLOCK) 
							  where serialnumber = @SerialNumber )			
	SELECT TOP 1 PackingSize*InnerBoxCount as TotalPcs from QCYieldAndPacking WITH(NOLOCK)
	WHERE SerialNumber=@SerialNumber and sobccount = @SOBCCount order by ReworkCount desc
	

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
		THROW;
END CATCH;

IF @@TRANCOUNT > 0
  COMMIT TRANSACTION;

  END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_ResourceBySerialNo]...';


GO
-- =============================================================
-- Name:             USP_GET_ResourceBySerialNo
-- Purpose:          Get Resource and SeqNo by serial number
-- =============================================================
-- Change History
-- Date				Author      Comments
-- -----			------      --------------------------------
-- 14/05/0218		Azrul		SP created.
-- =============================================================
--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_GET_ResourceBySerialNo]') AND type in (N'P', N'PC'))
--DROP PROCEDURE [dbo].[USP_GET_ResourceBySerialNo]
--GO
CREATE PROCEDURE [dbo].[USP_GET_ResourceBySerialNo]
(
	@serialNo decimal
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Resource FROM DOT_FloorD365HRGLOVERPT WITH (NOLOCK) 
	WHERE SerialNo=@serialNo
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_ReworkCategory]...';


GO

-- =======================================================
-- Name:			[USP_GET_ReworkCategory]
-- Purpose: 		Get Rework Category By QC Type
-- =======================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     -----------------------------
-- 18/07/2018 	Azrul	   SP created.
-- =======================================================
CREATE PROCEDURE [dbo].[USP_GET_ReworkCategory]
	@QCType		varchar(100) 
AS
BEGIN	
SET NOCOUNT ON
	SELECT RouteCategory FROM DOT_FSQCTypeTable with (nolock) WHERE QCType = @QCType
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_ReworkOrderDetails]...';


GO
-- =======================================================
-- Name:             USP_GET_ReworkOrderDetails
-- Purpose:          Get Rework Order Details
-- =======================================================
-- Change History
-- Date				Author     Comments
-- -----			------     ---------------------------
-- 19/07/0218		Azrul		SP altered.
-- ======================================================= 
CREATE PROCEDURE [dbo].[USP_GET_ReworkOrderDetails]
(
	@serialNo decimal,
	@QCType Nvarchar(100)
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT b.BatchNumber,b.Size,
		CASE WHEN ISNULL(b.QCBatchWeight,0) = 0
		THEN b.TotalPCs - ISNULL(b.PackedPcs,0)
		ELSE ((ISNULL(b.QCBatchWeight,0) / ISNULL(b.QCTenPcsWeight,0)) * 10 * 1000) - ISNULL(b.PackedPcs,0)
		END AS TotalPCs,
	b.GloveType,
	GETDATE() + 1 as DeliveryDate,
	(SELECT RouteCategory from DOT_FSQCTypeTable where QCType = @QCType) as [Pool], 
	(SELECT RouteCategory from DOT_FSQCTypeTable where QCType = @QCType) as RouteCategory,
	'PN' as Area
	FROM Batch b with(nolock) WHERE b.SerialNumber=@serialNo
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_RouteCategory]...';


GO

-- =======================================================
-- Name:			[USP_GET_ReworkCategory]
-- Purpose: 		Get Rework Category By QC Type
-- =======================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     -----------------------------
-- 18/07/2018 	Azrul	   SP created.
-- =======================================================
CREATE PROCEDURE [dbo].[USP_GET_RouteCategory]
	@QCType		varchar(100) 
AS
BEGIN	
SET NOCOUNT ON
	SELECT RouteCategory FROM DOT_FSQCTypeTable WHERE QCType = @QCType
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_RWK_After_SOBC_For_Second_SOBC]...';


GO


-- =======================================================
-- Name:			[USP_GET_RWK_After_SOBC_For_Second_SOBC]
-- Purpose: 		Get Rework After SOBC for 2nd SOBC
-- =======================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     -----------------------------
-- 15/01/2019 	Azrul	   SP created.
-- =======================================================
CREATE PROCEDURE [dbo].[USP_GET_RWK_After_SOBC_For_Second_SOBC]
	@serialnumber	NVARCHAR(100)
AS
BEGIN	
SET NOCOUNT ON
	Declare @SOBCSeq INT = (Select max(sequence) from DOT_FloorAxIntParentTable where BatchNumber = @serialnumber and FunctionIdentifier = 'SOBC')
	Declare @RWKSeq INT = (Select max(sequence) from DOT_FloorAxIntParentTable where BatchNumber = @serialnumber and FunctionIdentifier = 'RWKCR')

	IF (@RWKSeq > @SOBCSeq)
	BEGIN
		Select 1
	END
	ELSE
	BEGIN
		Select 0
	END
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_SavedHBCToValidate]...';


GO

-- =======================================================
-- Name:			[USP_GET_SavedHBCToValidate]
-- Purpose: 		Get saved HBC to validate before print 
-- =======================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     -----------------------------
-- 16/08/2018 	Azrul	   SP created.
-- =======================================================
CREATE PROCEDURE [dbo].[USP_GET_SavedHBCToValidate]
	@Resource		varchar(100),
	@OutputTime		DateTime,
	@BatchOrder		varchar(100)
AS
BEGIN	
SET NOCOUNT ON
	IF EXISTS(select * from DOT_FloorD365HRGLOVERPT with (nolock) where Resource = @Resource AND OutTime = @OutputTime)
		SELECT 0 
	ELSE
		SELECT 1
END
GO
PRINT N'Creating Procedure [dbo].[USP_GET_TotalPcsForQCPackingYield]...';


GO

-- =======================================================
-- Name:			USP_GET_TotalPcsForQCPackingYield
-- Purpose: 		Get Total Pcs for Scan Out Batch
-- =======================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     -----------------------------
-- 07/09/2019 	Azrul	   SP created.
-- =======================================================
CREATE PROCEDURE [dbo].[USP_GET_TotalPcsForQCPackingYield]
	@SerialNumber		BIGINT
AS
BEGIN	
BEGIN TRANSACTION;
BEGIN TRY
	
	SET NOCOUNT ON

--SELECT(
	SELECT CASE WHEN ISNULL(b.QCBatchWeight,0) = 0
	THEN b.TotalPCs-ISNULL(b.PackedPcs,0)
	ELSE ((ISNULL(b.QCBatchWeight,0)*1000)/(ISNULL(b.QCTenPcsWeight,0)/10))-ISNULL(b.PackedPcs,0)
	END AS TotalPcs from Batch b
	Where b.SerialNumber = @SerialNumber--)
	--- ISNULL((SELECT SUM(PackingSize*InnerboxCount) from QCYieldAndPacking where SerialNumber = @SerialNumber and ReworkCount > 0),0)
	

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
		THROW;
END CATCH;

IF @@TRANCOUNT > 0
  COMMIT TRANSACTION;

  END
GO
PRINT N'Creating Procedure [dbo].[USP_GetQAIDetails_SRBC]...';


GO
-- ==========================================================================
-- Name:             USP_GetQAIDetails_SRBC
-- Purpose:          Get SRBC batch details for QAI
-- ==========================================================================
-- Change History
-- Date				Author      Comments
-- -----			------      ---------------------------------------------
-- 25/10/2020		Azrul		SP created.
-- ========================================================================== 
CREATE PROCEDURE [dbo].[USP_GetQAIDetails_SRBC]
(
	@serialNo decimal
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT d.SeqNo,b.SerialNumber,b.BatchNumber,d.BthOrder AS BatchOrder,d.Resource,b.GloveType,b.LineId,s.name AS 'ShiftName',b.Size,b.TenPCsWeight,
	b.BatchWeight,b.TotalPCs as TotalPcs,b.IsOnline,b.QCType,b.ModuleId,b.SubModuleId,b.BatchType,l.locationname,b.BatchCardDate,
	DATEADD(DAY,1,b.BatchCardDate) as DeliveryDate, (SELECT RouteCategory from DOT_FSQCTypeTable with (nolock) where QCType = b.QCType) as RouteCategory,
	(SELECT RouteCategory from DOT_FSQCTypeTable with (nolock) where QCType = b.QCType) as [Pool],
	'PN' as Area FROM Batch b with (nolock) JOIN locationmaster l 
	ON b.LocationId=l.LocationId
	JOIN shiftmaster s with (nolock) ON b.ShiftId= s.ShiftId
	LEFT JOIN DOT_FloorD365HRGLOVERPT d WITH (NOLOCK) ON b.SerialNumber = d.SerialNo AND b.BatchNumber = d.BatchCardNumber
	WHERE SerialNumber = @serialNo 
END
GO
PRINT N'Creating Procedure [dbo].[USP_HBC_ReprintBatchCard_Get]...';


GO
-- =========================================================================
-- Name:   USP_HBC_ReprintBatchCard_Get
-- Purpose:   Get Glove Batch Order print details for Reprint
-- =========================================================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   --------------------------------------------------------  
-- 27/06/2018  Azrul Amin    SP created.  
-- 02/02/2021  Azrul Amin    Include ON2G.  
-- =========================================================================  
CREATE PROCEDURE [dbo].[USP_HBC_ReprintBatchCard_Get]  
( 
 @SerialNumber varchar(150),
 @Resource varchar(150)    
) 
AS  
BEGIN   
 SET NOCOUNT ON;  
	SELECT OutTime,LineId, SUBSTRING(Resource,6,2) AS Tier, SerialNo
	FROM DOT_FloorD365HRGLOVERPT with (nolock) WHERE SerialNo = @SerialNumber and Resource = @Resource
	UNION ALL
	SELECT CurrentDateandTime AS OutTime, LineId, '' As Tiew, FORMAT(SerialNumber,'0000000000') AS SerialNo
	FROM DOT_FloorD365Online2G with (nolock) 
	WHERE SerialNumber = @SerialNumber and Resource = @Resource
 SET NOCOUNT OFF;  
END
GO
PRINT N'Creating Procedure [dbo].[USP_HBCSerialNo_Get]...';


GO
-- =========================================================  
-- Name:   USP_HBCSerialNo_Get
-- Purpose:   Get HBC Serial Number 
-- =========================================================  
-- Change History  
-- Date    Author   Comments  
-- -----   ------   ----------------------------------------  
-- 11/05/2018  Azrul Amin    SP created.  
-- =========================================================  
CREATE PROCEDURE [dbo].[USP_HBCSerialNo_Get]  
( 
 @OutputTime datetime,
 @Resource varchar(10)
 
) 
AS  
BEGIN   
	BEGIN
		SELECT DISTINCT SerialNo FROM DOT_FloorD365HRGLOVERPT WITH (NOLOCK) WHERE OutTime = @OutputTime and Resource = @Resource
	END
 SET NOCOUNT OFF;  
END
GO
PRINT N'Creating Procedure [dbo].[USP_HBCTotalPcs_Get]...';


GO
 --=========================================================  
 --Name:   USP_HBCTotalPcs_Get
 --Purpose:   Get HBC Total Quantity by Serial No 
 --=========================================================  
 --Change History  
 --Date    Author   Comments  
 -------   ------   ----------------------------------------  
 --27/05/2018  Azrul Amin    SP created.  
 --=========================================================  
CREATE PROCEDURE [dbo].[USP_HBCTotalPcs_Get]  
( 
 @SerialNo varchar(150)  
) 
AS  
BEGIN   
 SET NOCOUNT ON; 
	SELECT SUM(PackingSz*InBox) as TotalPcs from DOT_FloorD365HRGLOVERPT where SerialNo = @SerialNo
 SET NOCOUNT OFF;  
END
GO
PRINT N'Creating Procedure [dbo].[USP_Rollback_PWT]...';


GO
-- ========================================================================================
-- Name:			[USP_Rollback_PWT]
-- Purpose: 		<Inactives PWTBCA in staging after PTScan rollback>
-- ========================================================================================
-- Change History
-- Date               Author                     Comments
-- -----   ------   -----------------------------------------------------------------------
-- <3 Dec,2019> 	<Azrul>	         SP created.

CREATE PROCEDURE [dbo].[USP_Rollback_PWT]
(
       @serialNo	nvarchar(20)
)
AS
BEGIN
	 SET NOCOUNT ON;
	 UPDATE DOT_FloorAxIntParentTable set IsDeleted = 1, DeleterUserId = 1, DeletionTime = GETDATE()
	 where FunctionIdentifier like 'PWTBC%' and BatchNumber = @serialNo

	 UPDATE DOT_RafStgTable set IsDeleted = 1, DeleterUserId = 1, DeletionTime = GETDATE() 
	 where ParentRefRecId in (Select Id from DOT_FloorAxIntParentTable where FunctionIdentifier like 'PWTBC%' and BatchNumber = @serialNo)

	 DELETE AXPostingLog where ServiceName like 'PWTBC%' and SerialNumber = @serialNo
	 SET NOCOUNT OFF;
END
GO
PRINT N'Creating Procedure [dbo].[USP_SAV_IncreaseSOBCCount]...';


GO

-- =======================================================
-- Name:			[USP_SAV_IncreaseSOBCCount]
-- Purpose: 		Increase SOBC posting count for handle multiple SOBC.
-- =======================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     -----------------------------
-- 18/07/2018 	Azrul	   SP created.
-- 24/03/2019   Max		   fine tune mulitiple SOBC
-- =======================================================
CREATE PROCEDURE [dbo].[USP_SAV_IncreaseSOBCCount]
	@SerialNumber		varchar(100) 
AS
BEGIN	
SET NOCOUNT ON
	DECLARE @MaxSOBCCount INT = (Select MAX(SOBCCount)
									from QCYieldAndPacking WITH(NOLOCK) 
									where serialnumber = @SerialNumber );	
	-- Update those records not been post to staging	
	-- QCQI not post should be defult to 0, increase 1 after QCQI posted
	UPDATE QCYieldAndPacking 
	SET SOBCCount = @MaxSOBCCount + 1 
	WHERE SerialNumber = @SerialNumber --and SOBCCount = 0 --QCQI not post should be defult to 0
END
GO
PRINT N'Creating Procedure [dbo].[USP_SAV_ScanBatchCardPieces]...';


GO

	-- ========================================================================================
-- Name:			[USP_SAV_ScanBatchCardPieces]
-- Purpose: 		<Save Details in QCYieldAndPacking table>
-- ========================================================================================
-- Change History
-- Date               Author                     Comments
-- -----   ------   -----------------------------------------------------------------------
-- <21 August,2020> 	<Gary Lau>	         SP created.

CREATE PROCEDURE [dbo].[USP_SAV_ScanBatchCardPieces]
(
       @serialNo             NUMERIC(10,0),
	   @tenPcsWeight         DECIMAL(18,3),
	   @batchPieces          INT,
	   @reworkReasonId       INT = NULL,
	   @reworkCount          INT = NULL,	   
	   @batchStatus          NVARCHAR(25),
	   @lastModifiedOn       DATETIME,
	   @workstationNumber    NVARCHAR(25),
	   @module               NVARCHAR(20),
	   @targetTime           DATETIME,
	   @subModule            NVARCHAR(50),
	   @Id                   INT,
	   @qcGroupId            INT,
	   @qcGroupMember        NVARCHAR(200),
	   @memberCount          INT,
	   @shift                INT
)
AS
BEGIN
	BEGIN TRANSACTION;
		 -- Try Block
		BEGIN TRY
				DECLARE @QC_LocationId	INT
				SELECT @QC_LocationId = LocationId FROM QCGroupMaster (nolock) WHERE QCGroupId = @qcGroupId

			    SET NOCOUNT ON;	
				IF @id = 1
				BEGIN
					UPDATE QCYieldAndPacking SET TenPiecesWeight = @tenPcsWeight, PackingSize = @batchPieces, InnerBoxCount = 1, BatchStatus = @batchStatus,
					WorkStationId = @workstationNumber, BatchEndTime = @lastModifiedOn, LastModifiedOn = @lastModifiedOn,
					ReworkReasonId = @reworkReasonId, ModuleId = @module, SubModuleId = @subModule, QCGroupId = @qcGroupId,
					 BatchTargetTime = @targetTime, QcGroupMembers = @qcGroupMember, QCGroupCount =  @memberCount, LocationId = @QC_LocationId
					--WHERE SerialNumber = @serialNo AND ReworkCount = @reworkCount	
					WHERE LastModifiedOn = (select top(1) LastModifiedOn
											 from QCYieldAndPacking
											 where SerialNumber = @serialNo
											 order by LastModifiedOn desc);
					
				END
				ELSE
				BEGIN
					INSERT INTO QCYieldAndPacking (SerialNumber, TenPiecesWeight, PackingSize, InnerBoxCount, 
					BatchStatus, WorkStationId, LastModifiedOn, ModuleId, SubModuleId, ReworkReasonId, ReworkCount,
					 QCGroupId, QcGroupMembers, QCGroupCount, ShiftId, LocationId) VALUES
					(@serialNo, @tenPcsWeight, @batchPieces, 1, @batchStatus, @workstationNumber, @lastModifiedOn,
					 @module, @subModule, @reworkReasonId, @reworkCount, @qcGroupId, @qcGroupMember, @memberCount, @shift, @QC_LocationId)
				END

				UPDATE Batch SET QCTenPcsWeight = @tenPcsWeight WHERE SerialNumber = @serialNo
			    SET NOCOUNT OFF;
		END TRY
		 -- Catch Block 
		BEGIN CATCH
			IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
		END CATCH;
		IF @@TRANCOUNT > 0
		  COMMIT TRANSACTION;
END
GO
PRINT N'Creating Procedure [dbo].[USP_SaveWTSamplingQCQI]...';


GO
  
-- ============================================================================
-- Name:             USP_SaveWTSamplingQCQI  
-- Purpose:          Accumulate WT sample quantity after QCQI more than 1 times 
-- ============================================================================
-- Change History  
-- Date				 Author			 Comments  
-- -----			 ------			 ------------------------------------------ 
-- 13/03/2018		 Azrul           SP Created.  
-- 12/01/2019		 Azrul           Cater multiple SOBC.  
-- ============================================================================
CREATE PROCEDURE [dbo].[USP_SaveWTSamplingQCQI] 
(  
	@SerialNo NVARCHAR(100),
	@WTSamplingSize DECIMAL
)  
AS  
BEGIN  
BEGIN TRANSACTION;  
BEGIN TRY       
	--#Azrul 12/01/2019: QAI records not been post to staging default is 0,will update after posted
	DECLARE @SOBCCount INT = (Select min(SOBCCount)from QCYieldAndPacking WITH(NOLOCK) where serialnumber = @serialNo )									--#Azrul 12/01/2019
    UPDATE TOP (1) QCYieldAndPacking SET WTSamplingSize = WTSamplingSize + @WTSamplingSize WHERE SerialNumber = @SerialNo AND SOBCCount = @SOBCCount	--#Azrul 12/01/2019
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
PRINT N'Creating Procedure [dbo].[USP_SEL_DOT_QAIDetailBySerialNoIgnoreAXPostingSuccess]...';


GO

-- =======================================================
-- Name:             USP_SEL_DOT_QAIDetailBySerialNoIgnoreAXPostingSuccess
-- Purpose:          Get batch  details by serial number cater time out issue
-- =======================================================
-- Change History
-- Date         Author					Comments
-- -----        ----------     -----------------------------
-- 09/02/2018     Max He				SP created.
-- =======================================================
CREATE PROCEDURE [dbo].[USP_SEL_DOT_QAIDetailBySerialNoIgnoreAXPostingSuccess]
(
	@SerialNumber Nvarchar(10)
)
AS
BEGIN
	DECLARE @CurrentStage VARCHAR(10)

	IF EXISTS(SELECT * FROM  QCYieldandPacking 
	          WHERE serialNumber = @SerialNumber)
			 BEGIN		
				SET @CurrentStage = 'QCQI'
			 END	 

	ELSE IF EXISTS(SELECT * FROM PTScanBatchCard
	          WHERE serialNumber = @SerialNumber)
			BEGIN
				SET @CurrentStage =  'PTQI'
			END

       SET NOCOUNT ON;           
	 
              SELECT TOP 1  b.BatchNumber,ISNULL(IsOnline,0) IsOnline,
			  --CASE ISNULL(q.IsAXPostingSuccess,0) WHEN 0 THEN NULL ELSE b.QAIDate END AS QAIDate,--#MH isAXPostingSuccess will check on program
			  b.QAIDate,--#MH 9/2/2018
			  b.GloveType,b.Size,q.VTSamplingSize,q.WTSampliingSize,
			 CASE WHEN @CurrentStage='QCQI' THEN b.QCTenPcsWeight WHEN @CurrentStage='PTQI' THEN b.PTTenPcsWeight ELSE  b.TenPCsWeight END AS 'TenPCsWeight' ,
			 CASE WHEN @CurrentStage='QCQI' THEN b.QCBatchWeight WHEN @CurrentStage='PTQI' THEN b.PTBatchWeight ELSE  b.BatchWeight END AS 'BatchWeight', 
			--CASE ISNULL(q.IsAXPostingSuccess,0) WHEN 0 THEN '' ELSE  q.QCType END  AS QCType,--#MH isAXPostingSuccess will check on program
			q.QCType,--#MH 9/2/2018
			ISNULL(q.IsAXPostingSuccess,0) IsAXPostingSuccess,
			q.QAIInspectorId,q.serialNumber,q.QaiID  
			FROM Batch b  WITH (NOLOCK)
			  LEFT JOIN QAI q   WITH (NOLOCK) ON b.SerialNumber=q.serialNumber
			  WHERE b.SerialNumber=@SerialNumber  ORDER BY q.QaiID DESC
     
       SET NOCOUNT OFF;

END
GO
PRINT N'Creating Procedure [dbo].[USP_SEL_ShiftByTime]...';


GO

-- =======================================================
-- Name:			USP_SEL_ShiftByTime
-- Purpose: 		Gets Current Shift from Time
-- =======================================================
-- Change History
-- Date         Author     Comments
-- -----        ------     -----------------------------
-- 03/01/2019 	Azrul	   SP Created.
-- =======================================================
CREATE PROCEDURE [dbo].[USP_SEL_ShiftByTime]
	-- Add the parameters for the stored procedure here
	@shiftType NVARCHAR(10),	
	@shiftTime DATETIME
AS
BEGIN
	SELECT Name AS 'Name', ShiftId AS 'ID',dbo.Ufn_GetCurrentShiftByTime(@shiftType,@shiftTime) as 'CurrentShift' FROM ShiftMaster  WHERE GroupType = @shiftType AND isDeleted = 0 
END
GO
PRINT N'Creating Permission Permission...';


GO
DENY DELETE
    ON SCHEMA::[dbo] TO [HARTALEGA\ainol.azmi] CASCADE;


GO
PRINT N'Creating Permission Permission...';


GO
DENY DELETE
    ON SCHEMA::[dbo] TO [HARTALEGA\chua.lh] CASCADE;


GO
PRINT N'Creating Permission Permission...';


GO
DENY DELETE
    ON SCHEMA::[dbo] TO [HARTALEGA\deepa.pandian] CASCADE;


GO
PRINT N'Creating Permission Permission...';


GO
DENY DELETE
    ON SCHEMA::[dbo] TO [HARTALEGA\roseifareena.roselee] CASCADE;


GO
PRINT N'Creating Permission Permission...';


GO
DENY DELETE
    ON SCHEMA::[dbo] TO [HARTALEGA\syafiq.razak] CASCADE;


GO
PRINT N'Creating Permission Permission...';


GO
GRANT ALTER
    ON SCHEMA::[dbo] TO [HARTALEGA\admin_ereena];


GO
PRINT N'Creating Permission Permission...';


GO
GRANT ALTER
    ON SCHEMA::[dbo] TO [HARTALEGA\Admin_Maizatul];


GO
PRINT N'Creating Permission Permission...';


GO
GRANT EXECUTE
    ON SCHEMA::[dbo] TO [HARTALEGA\admin_ereena];


GO
PRINT N'Creating Permission Permission...';


GO
GRANT EXECUTE
    ON SCHEMA::[dbo] TO [HARTALEGA\Admin_Maizatul];


GO
PRINT N'Creating Permission Permission...';


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[USP_DOT_TOMsScanOut] TO [mppreader]
    AS [dbo];


GO
PRINT N'Creating Permission Permission...';


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[USP_EWN_CheckPalletStatus] TO [svc_EWN]
    AS [dbo];


GO
PRINT N'Creating Permission Permission...';


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[USP_GET_AXPostingLogInfo] TO [FSDB]
    AS [dbo];


GO
PRINT N'Creating Permission Permission...';


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[USP_SEL_DOT_QAIDetailBySerialNoIgnoreAXPostingSuccess] TO [FSDB]
    AS [dbo];


GO
PRINT N'Creating Permission Permission...';


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[USP_GET_BATCHSEQUENCE] TO [FSDB]
    AS [dbo];


GO
PRINT N'Creating Permission Permission...';


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[USP_SAVE_AXPOSTINGLOG] TO [FSDB]
    AS [dbo];


GO
PRINT N'Refreshing Function [dbo].[ufn_GetBatchSummaryTable_BeforeFP]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[ufn_GetBatchSummaryTable_BeforeFP]';


GO
PRINT N'Refreshing Procedure [dbo].[usp_FP_BatchCapacity]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[usp_FP_BatchCapacity]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_GET_GloveInquiry]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_GET_GloveInquiry]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_GET_SurgicalPackingPlan_GetBatchPcs]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_GET_SurgicalPackingPlan_GetBatchPcs]';


GO
PRINT N'Refreshing Procedure [dbo].[usp_GIS_GetBatchInfo]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[usp_GIS_GetBatchInfo]';


GO
PRINT N'Refreshing Procedure [dbo].[usp_wis_getGISStockLevel]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[usp_wis_getGISStockLevel]';


GO
PRINT N'Refreshing Procedure [dbo].[SCM_GetFGTotalPacked]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[SCM_GetFGTotalPacked]';


GO
PRINT N'Refreshing Procedure [dbo].[SCM_GetFGTotalPackedLineProd]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[SCM_GetFGTotalPackedLineProd]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_FS_AX_WorkOrderSyncBatchJob]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_FS_AX_WorkOrderSyncBatchJob]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_SEL_WorkOrderDetails]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_SEL_WorkOrderDetails]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_SEL_WorkOrderDetails_Test]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_SEL_WorkOrderDetails_Test]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_SEL_WorkOrderDetailsById]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_SEL_WorkOrderDetailsById]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_VAL_CartonNoConfig]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_VAL_CartonNoConfig]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_CreatePurchaseOrderRecords]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_CreatePurchaseOrderRecords]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_EWN_PalletDataModify]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_EWN_PalletDataModify]';


GO
PRINT N'Refreshing Procedure [dbo].[usp_FP_POlastPreshipment_Validate]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[usp_FP_POlastPreshipment_Validate]';


GO
PRINT N'Refreshing Procedure [dbo].[usp_FP_SELECT_ReprintOuterCasePOList]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[usp_FP_SELECT_ReprintOuterCasePOList]';


GO
PRINT N'Refreshing Procedure [dbo].[usp_FP_SELECT_SurgicalPOList]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[usp_FP_SELECT_SurgicalPOList]';


GO
PRINT N'Refreshing Procedure [dbo].[usp_FP_ValidateFGLabel]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[usp_FP_ValidateFGLabel]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_GET_CartonNoConfigList]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_GET_CartonNoConfigList]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_GET_CNCPODropDownList]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_GET_CNCPODropDownList]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_GET_DestinationPurchaseOrderDropDownList]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_GET_DestinationPurchaseOrderDropDownList]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_GET_DestinationPurchaseOrderDropDownList_AddPoTransfer]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_GET_DestinationPurchaseOrderDropDownList_AddPoTransfer]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_GET_ItemNumberDropDownList]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_GET_ItemNumberDropDownList]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_GET_ItemSizeDropDownList]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_GET_ItemSizeDropDownList]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_GET_PurchaseOrderDropDownList_AddPoTransfer]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_GET_PurchaseOrderDropDownList_AddPoTransfer]';


GO
PRINT N'Refreshing Procedure [dbo].[usp_GET_SurgicalPackingPlan_POListAll]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[usp_GET_SurgicalPackingPlan_POListAll]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_GET_SurgicalPackingPlan_Size]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_GET_SurgicalPackingPlan_Size]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_GET_SurgicalPackingPlan_UnplannedCarton]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_GET_SurgicalPackingPlan_UnplannedCarton]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_SAV_CartonNoConfig]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_SAV_CartonNoConfig]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_SAV_SurgicalPackingPlan]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_SAV_SurgicalPackingPlan]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_VAL_PurchaseOrderWithWorkOrderStatus]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_VAL_PurchaseOrderWithWorkOrderStatus]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_VAL_SurgicalPackingPlanConfig]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_VAL_SurgicalPackingPlanConfig]';


GO
PRINT N'Refreshing Procedure [dbo].[USP_CreatePOTransfer]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[USP_CreatePOTransfer]';


GO
PRINT N'Refreshing Procedure [dbo].[Usp_OEEDaily_Line_Calculate]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[Usp_OEEDaily_Line_Calculate]';


GO
PRINT N'Refreshing Procedure [dbo].[usp_OEEPlant_Date_Calculate]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[usp_OEEPlant_Date_Calculate]';


GO
PRINT N'Refreshing Procedure [dbo].[usp_OEEPlant_Month_Calculate]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[usp_OEEPlant_Month_Calculate]';


GO
PRINT N'Refreshing Procedure [dbo].[usp_OEEPlantWide_Month_Calculate]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[usp_OEEPlantWide_Month_Calculate]';


GO
PRINT N'Refreshing Procedure [dbo].[usp_OEE_ProductionLogging_Changes]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[usp_OEE_ProductionLogging_Changes]';


GO
PRINT N'Refreshing Procedure [dbo].[Usp_OEE_StagingTableFill]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[Usp_OEE_StagingTableFill]';


GO
PRINT N'Update complete.';


GO

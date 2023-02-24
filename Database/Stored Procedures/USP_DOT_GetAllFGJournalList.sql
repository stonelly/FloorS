-- =============================================      
-- Author:  <Azrul>      
-- Create date: <19-May-2021>      
-- Description: <Get All FG>      
-- exec USP_DOT_GetAllFGJournalList '2021-12-26 23:59:59:999','HSBBON000000226',1   
-- =============================================      
ALTER PROCEDURE [dbo].[USP_DOT_GetAllFGJournalList]      
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
select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQuantity, a.PlantNo, a.Sequence as SequenceNo, a.BatchNumber as SerialNo,      
isnull(CAST(d.BaseQuantity as DECIMAL),b.BaseQty) as BaseQuantity,  
CAST(b.Quantity as DECIMAL) as Qty,   
isnull(CAST(b.Quantity * d.BaseQuantity as DECIMAL),b.GloveQty) as GloveQuantity,  
b.PostingDateTime,     
case when isnull(b.CustomerReference,'''') = '''' then e.BatchId else b.CustomerReference end as CustRef,   
CAST(a.PreshipmentCases as DECIMAL) as PreshipmentCases, a.LastModificationTime    
from DOT_FloorAxIntParentTable a with(nolock)    
join DOT_FGJournalTable b with(nolock) on a.id = b.ParentRefRecId    
left join DOT_FloorSalesLine d with(nolock) on d.SalesId = b.SalesOrderNumber and d.IsDeleted = 0     
AND d.ItemId = case when a.FunctionIdentifier = ''SGBC'' then b.ReferenceItemNumber else b.ItemNumber end     
AND d.HartalegaCommonSize = b.Configuration    
left join DOT_FloorD365BO e with(nolock) on e.BthOrderId = b.BatchOrderNumber and e.IsDeleted = 0   
where a.FunctionIdentifier in (''SBC'',''SGBC'',''MTS'')    
and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 and a.IsConsolidated = 1      
and a.AutoConsoleMarking=1 and a.ConsoleMarkingTime <=''' + @CutOffTime + '''     
and a.ProcessingStatus in (0,1,4,5) -- SBC/SGBC not console      
and b.BatchOrderNumber = ''' + @BatchOrder + '''     
and a.ConsolidationSequence = ''' + @ConsolidationSequence + '''     
) a'    
    
 EXEC (@sql + ' order by a.LastModificationTime')      
END
-- =============================================      
-- Author:  <Azrul>      
-- Create date: <19-May-2021>      
-- Description: <Get All Batch Orders>      
--exec [dbo].[USP_DOT_GetAllFGBatchOrderList] '2021-12-16 16:35:18.2080000', 1   
-- =============================================      
ALTER PROCEDURE [dbo].[USP_DOT_GetAllFGBatchOrderList]      
  @CutOffTime varchar(100),    
  @IsSp int    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
 DECLARE @sql varchar(max);      
 DECLARE @sqlFilter varchar(max);      
      
 IF (@IsSp = 1)    
 BEGIN    
 SET @sqlFilter = 'a.ConsolidationSequence = ''1'''    
 END    
 ELSE    
 BEGIN    
 SET @sqlFilter = 'a.ConsolidationSequence <> ''1'''    
 END    
    
  SET @sql = '     
  select a.* from(     
  select case when isnull(b.CustomerReference,'''')='''' then e.BatchId else b.CustomerReference end CustomerReference,  
  b.Configuration,b.BatchOrderNumber,a.FunctionIdentifier, a.PlantNo, b.Location, b.SalesOrderNumber,    
  case when a.FunctionIdentifier = ''SGBC'' then b.ReferenceItemNumber else b.ItemNumber end as ItemNumber,    
  count(1) BatchCardCount, sum(a.FGQuantity) as SumFGQty, 0 as SumSampleQty,  isnull(CAST(d.BaseQuantity as DECIMAL),b.BaseQty) as BaseQuantity,    
  min(b.PostingDateTime) as StartPostingDate, max(b.PostingDateTime) as EndPostingDate,a.ConsolidationSequence,     
  min(a.LastModificationTime) as LastModificationTime,  
  b.IsWTS --MTS  
  from DOT_FloorAxIntParentTable a with(nolock)    
  join DOT_FGJournalTable b with(nolock) on a.id=b.ParentRefRecId    
  left join DOT_FloorSalesLine d with(nolock) on d.SalesId = b.SalesOrderNumber and d.IsDeleted = 0     
  AND d.ItemId = case when a.FunctionIdentifier = ''SGBC'' then b.ReferenceItemNumber else b.ItemNumber end     
  AND b.CONFIGURATION = case when a.FunctionIdentifier = ''SPPBC'' then d.CustomerSize else d.HartalegaCommonSize end    
  AND  d.IsDeleted = 0   
  left join DOT_FloorD365BO e with(nolock) on e.BthOrderId = b.BatchOrderNumber and e.IsDeleted = 0   
  where FunctionIdentifier in (''SBC'',''SGBC'',''SMBP'',''SPPBC'',''MTS'',''MMTS'')    
  and a.ProcessingStatus in (0,1,4,5) and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0     
  and a.AutoConsoleMarking=1 and a.ConsoleMarkingTime <=''' + @CutOffTime + '''     
  and ' + @sqlFilter + '    
  group by b.CustomerReference,b.BatchOrderNumber, a.FunctionIdentifier, b.ItemNumber, b.ReferenceItemNumber,b.Configuration, a.PlantNo, b.Location,     
  b.SalesOrderNumber,d.BaseQuantity,a.ConsolidationSequence,b.isWTS,b.BaseQty,e.BatchId  
  ) a'    
    
  EXEC (@sql + ' order by a.LastModificationTime')     
END 
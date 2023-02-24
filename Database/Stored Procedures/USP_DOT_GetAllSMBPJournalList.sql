-- =============================================    
-- Author:  <Azrul>    
-- Create date: <9-June-2021>    
-- Description: <Get All SMBP>    
-- exec [dbo].[USP_DOT_GetAllFGBatchOrderList]   
-- exec USP_DOT_GetAllSMBPJournalList '2021-08-22 23:59:59:999','HNBON000476779',1  
-- =============================================    
CREATE OR ALTER PROCEDURE [dbo].[USP_DOT_GetAllSMBPJournalList]    
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
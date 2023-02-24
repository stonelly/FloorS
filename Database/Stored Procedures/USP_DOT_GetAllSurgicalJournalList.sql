
/****** Object:  StoredProcedure [dbo].[USP_DOT_GetAllSurgicalJournalList]    Script Date: 28/6/2021 4:56:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Author:  <Azrul>  
-- Create date: <19-May-2021>  
-- Description: <Get All FG>  
-- exec USP_DOT_GetAllSurgicalJournalList '2021-08-22 23:59:59:999','HNBON000351504',1
-- =============================================  
ALTER PROCEDURE [dbo].[USP_DOT_GetAllSurgicalJournalList]  
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
b.ReferenceItemNumber as ItemNo,b.InnerLotNumber as LotNo,b.CustomerReference as CustRef, a.PlantNo
from DOT_FloorAxIntParentTable a with(nolock)
join DOT_FGJournalTable b with(nolock) on a.id=b.ParentRefRecId
join DOT_FloorSalesLine d with(nolock) on d.SalesId = b.SalesOrderNumber AND d.ItemId = b.ItemNumber AND d.CustomerSize = b.Configuration and d.IsDeleted=0
where a.FunctionIdentifier in (''SPPBC'')
and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 and a.IsConsolidated = 1  
and a.AutoConsoleMarking=1 and a.ConsoleMarkingTime <=''' + @CutOffTime + ''' 
and a.ProcessingStatus in (0,1,4,5) -- SPPBC not console 
and b.BatchOrderNumber = ''' + @BatchOrder + '''  
and a.ConsolidationSequence = ''' + @ConsolidationSequence + ''' 
) a'

 EXEC (@sql + ' order by a.PostingDateTime')  
END


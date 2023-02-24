
/****** Object:  StoredProcedure [dbo].[USP_DOT_GetAllBatchOrderList]    Script Date: 12/7/2021 3:20:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Azrul>  
-- Create date: <19-May-2021>  
-- Description: <Get All Batch Orders>
--exec [dbo].[USP_DOT_GetAllBatchOrderList] '2021-08-01 12:59:59:999'
-- =============================================  
ALTER PROCEDURE [dbo].[USP_DOT_GetAllBatchOrderList]  
  @CutOffTime varchar(100)
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 DECLARE @sql varchar(max);  
  SET @sql = ' 
 select a.* from(
select b.BatchOrderNumber,count(1) BatchCardCount, MIN(b.PostingDateTime) as StartPostingDate, MAX(b.PostingDateTime) as EndPostingDate, 
sum(b.RAFGoodQty) as SumRAFGoodQty,sum(b.RAFHBSample) as SumRAFHBSample,sum(b.RAFVTSample) as SumRAFVTSample,sum(b.RAFWTSample) as SumRAFWTSample,
sum(b.BatchWeight) as SumBatchWeight, FunctionIdentifier, b.ItemNumber, b.Configuration, a.PlantNo, b.Location
from DOT_FloorAxIntParentTable a with(nolock)
join DOT_RafStgTable b with(nolock) on a.id=b.ParentRefRecId
where a.IsDeleted=0 and b.IsDeleted=0
and a.FunctionIdentifier in (''HBC'',''SRBC'',''PNBC'') 
and a.ProcessingStatus in (0,1,4,5) and a.IsDeleted=0 and a.IsMigratedFromAX6 = 0 
and ISNULL(a.ReferenceBatchNumber1,'''') <> ''RESAMPLE'' 
and a.AutoConsoleMarking = 1 and a.IsConsolidated = 1 and a.ConsoleMarkingTime <=''' + @CutOffTime + ''' 
group by b.BatchOrderNumber, a.FunctionIdentifier, b.ItemNumber, b.Configuration, a.PlantNo, b.Location
) a'

  EXEC (@sql + ' order by a.StartPostingDate') 
END
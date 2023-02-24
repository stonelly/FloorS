USE [FloorSystemUAT]
GO

/****** Object:  StoredProcedure [dbo].[USP_DOT_GetAllReportAsFinishedList]    Script Date: 26/11/2021 2:48:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Author:  <Amir>  
-- Create date: <26-Nov-2021>  
-- Description: <Get Glove Change All Report As Finished>  
-- exec USP_DOT_GetAllReportAsFinishedForCGLVList @CutOffTime=N'2021-08-01 01:00:00.000',@BatchOrder=N'HNBON000372103'
-- =============================================  
CREATE OR ALTER PROCEDURE [dbo].[USP_DOT_GetAllReportAsFinishedForCGLVList]  
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


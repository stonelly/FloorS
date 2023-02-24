
INSERT INTO [dbo].[DOT_FloorD365BO]
           ([BatchId]
           ,[BthOrderId]
           ,[CreationTime]
           ,[CreatorUserId]
           ,[IsDeleted]
           ,[ItemId]
           ,[LastModificationTime]
           ,[ProdPoolId]
           ,[ProdStatus]
           ,[QtySched]
           ,[RecId]
           ,[RecVersion]
           ,[SchedEnd]
           ,[SchedFromTime]
           ,[SchedStart]
           ,[SchedToTime]
           ,[Size]
           ,[WarehouseId]
           ,[ReworkBatch]
           )
select [Batch Number],'FG'+a.[SalesId]+a.[Configuration]+replace(a.[ItemId],'-',''),getdate(),1,0,[ItemId],getdate(),'FG','StartedUp',sum([SalesQty]),ROW_NUMBER() OVER(ORDER BY a.[SalesId],a.[Configuration] ASC),1,'2022-03-29 00:00:00','2022-03-29 02:18:00','2022-03-29 00:00:00','2022-03-29 02:50:00',a.[Configuration],'FG','No'
from [dbo].[OpenSO$] a
--join ( SELECT  [SalesId],[Configuration]
--  FROM [OpenSO$]
--  group by [SalesId],[Configuration]
--  having count(1)>1) b on a.[SalesId]=b.[SalesId] and a.[Configuration]=b.[Configuration]
group by [Batch Number],[SalesId],[Configuration],[ItemId]

GO
insert dot_floorD365BOResource(batchOrderId,creationTime,creatorUserId,isdeleted,recVersion,Resource,ResourceGrp,RecordId,PlantNo)
select 'FG'+a.[SalesId]+a.[Configuration]+a.[ItemId],getdate(),1,0,0,'P6-FP','P6FP',1,'P6'
from [dbo].[OpenSO$] a
--join ( SELECT  [SalesId],[Configuration]
--  FROM [OpenSO$]
--  group by [SalesId],[Configuration]
--  having count(1)=1) b on a.[SalesId]=b.[SalesId] and a.[Configuration]=b.[Configuration]
group by [SalesId],[Configuration],[ItemId]


-- later need to update based on acutal FG BO sync from d365 
select bo.bthOrderId, * 
from FinalPacking fp with(nolock)
join [OpenSO$] o on fp.PONumber=o.[SalesId] and fp.ItemNumber = o.[ItemId] and fp.Size=o.[Configuration]
join DOT_FloorD365BO bo with(nolock) on bo.[BatchId]=o.[Batch Number] and bo.[ItemId] = fp.ItemNumber and bo.[Size]=fp.[Size]


--select * from AX_AVACHANGEGLOVECODE
update AX_AVACHANGEGLOVECODE set stopped = 1
insert into AX_AVACHANGEGLOVECODE
select 
--ISNULL((select distinct itemnumber from DOT_InventBatchSum where Searchname = FROMGLOVECODE),FROMGLOVECODE) , 
--ISNULL((select distinct itemnumber from DOT_InventBatchSum where Searchname = TOGLOVECODE), TOGLOVECODE),
--(select distinct itemnumber from DOT_InventBatchSum where Searchname = FROMGLOVECODE) , 
--(select distinct itemnumber from DOT_InventBatchSum where Searchname = TOGLOVECODE),
DATAAREAID,RECVERSION,PARTITION,RECID,0, '2022-03-29','2022-03-29' from AX_AVACHANGEGLOVECODE
GO

select * from EmployeeMaster where password <> ''














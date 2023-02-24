  
insert into screenmaster
values ('FG Batch Order', (select moduleid from ModuleMaster where Modulename = 'Final Packing'))
go

insert into ModuleScreenPermissionMapping
values ((select ModuleId from ModuleMaster where modulename = 'Final Packing'), (select ScreenId from screenmaster where screenname = 'FG Batch Order'), '>=', 
(select PermissionId from PermissionMaster where PermissionDescription = 'MIS'),1)
go
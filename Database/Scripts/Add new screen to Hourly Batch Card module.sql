
if not exists (select * from ScreenMaster where screenname ='Glove Output Reporting')
begin
	insert into ScreenMaster (screenname,moduleid) 
	values ('Glove Output Reporting', (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'));
	
end
go
if not exists (select * from ScreenMaster where screenname ='Online 2nd Grade Glove')
begin
	insert into ScreenMaster (screenname,moduleid) 
	values ('Online 2nd Grade Glove', (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'))
	
end
go

if not exists (select * from ModuleScreenPermissionMapping where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card')
					and ScreenId = (select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card') and screenname = 'Glove Output Reporting'))
begin
	insert into ModuleScreenPermissionMapping (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card') and screenname = 'Glove Output Reporting'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Admin'),1)
	insert into ModuleScreenPermissionMapping (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card') and screenname = 'Online 2nd Grade Glove'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Admin'),1)
	
end
go

if not exists (select * from [OperatorModulePermissionMapping] where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card')
					and ScreenId = (select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card') and screenname = 'Glove Output Reporting'))
begin
	insert into [OperatorModulePermissionMapping] (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card') and screenname = 'Glove Output Reporting'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Admin'),1)
	insert into [OperatorModulePermissionMapping] (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card') and screenname = 'Online 2nd Grade Glove'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Admin'),1)
	
end
go

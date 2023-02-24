
--moduleId must be 19 in all environment
if not exists (select * from ModuleMaster where ModuleName = 'Surgical Glove System')
begin
	insert into ModuleMaster (ModuleName,Description) 
	values ('Surgical Glove System', 'Surgical Glove System')
end
go

if not exists (select * from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'))
begin
	insert into ScreenMaster (screenname,moduleid) 
	values ('Print Surgical Batch Card', (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'))
	insert into ScreenMaster (screenname,moduleid) 
	values ('Glove Batch Order', (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'))
	insert into ScreenMaster (screenname,moduleid) 
	values ('Reprint SRBC', (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'))
	insert into ScreenMaster (screenname,moduleid) 
	values ('Batch Card reprint Log', (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'))
end
go

if not exists (select * from ModuleScreenPermissionMapping where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'))
begin
	insert into ModuleScreenPermissionMapping (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Print Surgical Batch Card'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Admin'),1)
	insert into ModuleScreenPermissionMapping (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Glove Batch Order'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Admin'),1)
	insert into ModuleScreenPermissionMapping (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Reprint SRBC'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Admin'),1)
	insert into ModuleScreenPermissionMapping (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Batch Card reprint Log'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Admin'),1)
end
go

if not exists (select * from [OperatorModulePermissionMapping] where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'))
begin
	insert into [OperatorModulePermissionMapping] (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Print Surgical Batch Card'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Admin'),1)
	insert into [OperatorModulePermissionMapping] (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Glove Batch Order'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Admin'),1)
	insert into [OperatorModulePermissionMapping] (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Reprint SRBC'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Admin'),1)
	insert into [OperatorModulePermissionMapping] (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Batch Card reprint Log'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Admin'),1)
end
go

if not exists (select * from RoleModuleMapping where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'))
begin
	insert into RoleModuleMapping (RoleId, ModuleId)
	values ((select RoleId from RoleMaster where role = 'MIS_Admin'),(select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'))
end
go

IF NOT EXISTS(SELECT * FROM [SurgicalPackingPlanSequenceNo])
BEGIN 
	INSERT INTO [dbo].[SurgicalPackingPlanSequenceNo] ([SequenceName],[LastSequenceNo],[SequenceLastModified])
	VALUES ('SPP', 4, '2020-11-19 13:10:13.243')
END
GO

IF NOT EXISTS(SELECT * FROM MessageMaster WHERE MessageKey = 'RESOURCE_NOT_SELECT')
BEGIN
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('RESOURCE_NOT_SELECT','Resource Not Selected!',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('BATCHORDER_NOT_SELECT','Batch Order Not Selected!',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('BATCHKG_IS_EMPTY','Batch(Kg) Is Empty!',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('QTYPCS_IS_EMPTY','Quantity(Pcs) Is Empty!',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('BATCHKG_IS_0','Batch(Kg) Must Be More Than 0',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('QTYPCS_IS_0','Quantity(Pcs) Must Be More Than 0',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('SURGICAL_BLOCKED','Surgical Glove Not Allowed',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('NON_SURGICAL_BLOCKED','Only Surgical Glove Is Allowed',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('SPP_INVALID_LOT','Invalid Internal Lot No',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('SPP_LOTNO_ZERO','Internal Lot No still in Planned Stage. Please complete on eFloorSystem before printing.',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('SPP_LOTNO_ONE','Internal Lot No still in Print Pouch Stage. Please complete this process before proceed.',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('SPP_LOTNO_THREE','Internal Lot No already printed. Please use Reprint module',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('SPP_LOTNO_TWO','Internal Lot No already printed. Please use Reprint module',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('INVALID_VALUE_FOR_BATCH_PCS','Please enter a valid value for Batch(Pcs).',NULL,getdate())
	INSERT INTO [dbo].[MessageMaster] ([MessageKey],[MessageText],[LastModifiedBy],[LastModifiedDate])
	VALUES ('QCQI_WITHOUT_PTQI','Batch must complete PTQI before proceed QCQI.',NULL,getdate())
END
GO

--QC packing & Yielding - Scan Batch Card Pieces menu access
--#TODO: Recheck QAIBLL.cs Line commented // #GL 04/11-2020 New screen - HTLG_P7CR_014&015 2nd Grade Surgical Glove Reporting
if not exists(select * from ScreenMaster where screenname = 'QC Scanning - Scan Batch Card Pieces')
BEGIN
	declare @screenid int
	--insert into ScreenMaster values('QC packing & Yielding - Scan Batch Card Pieces',9)
	--select @screenid = ScreenId from ScreenMaster where ScreenName like '%QC packing & Yielding - Scan Batch Card Pieces%'
	insert into ScreenMaster values('QC Scanning - Scan Batch Card Pieces',9)
	select @screenid = ScreenId from ScreenMaster where ScreenName like '%QC Scanning - Scan Batch Card Pieces%'
	insert into ModuleScreenPermissionMapping values(9,@screenid,'>=',2019,1)
	--QC packing & Yielding - Scan Batch Card Pieces
END
GO
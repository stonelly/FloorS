/*
This script was created by Visual Studio on 29-Nov-21 at 2:18 PM.
Run this script on 192.168.12.57.FloorSystemUAT (sa) to make it the same as 192.168.129.142.FloorSystemUAT (sa).
This script performs its actions in the following order:
1. Disable foreign-key constraints.
2. Perform DELETE commands. 
3. Perform UPDATE commands.
4. Perform INSERT commands.
5. Re-enable foreign-key constraints.
Please back up your target database before running this script.
*/
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
/*Pointer used for text / image updates. This might not be needed, but is declared here just in case*/
DECLARE @pv binary(16)
BEGIN TRANSACTION
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'VALIDATELABEL_PODATE', N'Labelset for this product require PO to have Customer Purchase Order Document Date. Please contact Sales Team to rectify this issue.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'VALIDATELABEL_PORDATE', N'Labelset for this product require PO to have Customer Purchase Order Received Date. Please contact Sales Team to rectify this issue.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'SHIFT_NOT_SELECT', N'Please Select Shift!', NULL, '20190329 17:12:43.067', NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'PTQI_QCTYPE_SP', N'STRAIGHT PACK not allowed.', NULL, '20190329 17:12:43.147', NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'NO_RWK_AFTER_SOBC_FOR_SECOND_SOBC', N'Batch must performed Change QC Type to proceed QCQI.', NULL, '20190329 17:12:43.190', NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'PTQI_COMPLETED', N'Batch already completed PTQI process.', NULL, '20190329 17:12:43.193', NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISION_VSAPI_NOT_CONFIGURE', N'Vision API not configured for this workstation.', N'1', '20201201 13:33:15.507', N'1', '20201201 13:33:15.507')
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'QCQI_INCOMPLETE', N'Batch must complete QC-QI, kindly bring batch to Scan QI Test Result.', N'1', '20201201 13:33:15.507', N'1', '20201201 13:33:15.507')
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'RESOURCE_ALREADY_USED_WITH_THE_SELECTED_TIME', N'Resource is already saved with the selected Output Time.', N'1', '20201201 13:33:15.507', N'1', '20201201 13:33:15.507')
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'UNSUFFICIENT_GLOVE', N'Unable to proceed due to remaining Glove to report is 0!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'QAI_DataSaved_AX_Fail_DetectOnFinalPack', N'This batch card has been saved but AX posting not successful, email will be send to MIS.Please continue.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'QAI_DataSaved_AX_Fail', N'This batch card has been saved but AX posting not successful, email will be send to MIS.Please continue scan another batch card!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'RESOURCE_OUTPUT_1_NOT_SELECT', N'Please Select Resource for Output 1!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'BATCHORDER_OUTPUT_1_NOT_SELECT', N'Please Select Batch Order for Output 1!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'BATCHORDER_OUTPUT_2_NOT_SELECT', N'Please Select Batch Order for Output 2!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'BATCHORDER_OUTPUT_3_NOT_SELECT', N'Please Select Batch Order for Output 3!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'BATCHORDER_OUTPUT_4_NOT_SELECT', N'Please Select Batch Order for Output 4!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'PACKSIZE_1_NOT_SELECT', N'Please Select Packing Size for Output 1!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'PACKSIZE_2_NOT_SELECT', N'Please Select Packing Size for Output 2!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'PACKSIZE_3_NOT_SELECT', N'Please Select Packing Size for Output 3!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'PACKSIZE_4_NOT_SELECT', N'Please Select Packing Size for Output 4!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'INBOX_1_NOT_SELECT', N'Please Insert Quantity for Inner Box for Output 1!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'INBOX_2_NOT_SELECT', N'Please Insert Quantity for Inner Box for Output 2!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'INBOX_3_NOT_SELECT', N'Please Insert Quantity for Inner Box for Output 3!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'INBOX_4_NOT_SELECT', N'Please Insert Quantity for Inner Box for Output 4!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'IDENTICAL_OUTPUT1_2', N'Unable to proceed because Output 1 and Output 2 is same!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'IDENTICAL_OUTPUT2_3', N'Unable to proceed because Output 2 and Output 3 is same!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'IDENTICAL_OUTPUT2_4', N'Unable to proceed because Output 2 and Output 4 is same!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'IDENTICAL_OUTPUT1_3', N'Unable to proceed because Output 1 and Output 3 is same!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'IDENTICAL_OUTPUT3_4', N'Unable to proceed because Output 3 and Output 4 is same!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'IDENTICAL_OUTPUT1_4', N'Unable to proceed because Output 1 and Output 4 is same!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'L_TIER_UNIDENTICAL', N'Glove code and glove size must be same for L tier!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'R_TIER_UNIDENTICAL', N'Glove code and glove size must be same for R tier!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'NOPSIREWORKORDERNO', N'Please Setup PSI Rework Order', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'PSIREWORKORDERNOTSTART', N'PSI Rework Order Not Started', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'TEMPPACK_NOT_SCAN_OUT', N'Temp Pack not scan out.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'INNERBOXES_IS_MAX', N'Inner Boxes must be less than ', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'TOTAL_INNERBOXES_MOD_GOT_BALANCE', N'Total Inner Boxes must be multiply by ', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'IS_CREATE_REWORK', N'Do you want to create Rework Order?', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'REWORK_QTY_IS_ZERO', N'Rework Quantity Is 0!', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'TEN_PCS_WEIGHT_IS_ZERO', N'Ten Pcs Weight is 0.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'TOTAL_PCS_IS_ZERO', N'Remaining Total Pieces to Rework is 0.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'REQUIREDFIELD_DRYER_PROGRAM', N'Please select a dryer program.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'OUTPUT1_2_TIER_NOT_SAME', N'Output1 and Output2 must have the same side.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'OUTPUT3_4_TIER_NOT_SAME', N'Output3 and Output4 must have the same side.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'3_TIER_BLOCKED', N'3 Tier print is not allowed.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'TUMBLING_REPRINT_HBC', N'Batch need to use Reprint HBC to proceed.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'PT_NOT_ALLOW', N'PT Not Allowed.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'PT_QC_NOT_COMPLETE', N'In order to proceed, batch must complete PT/QC process.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'LOTVERIFICATION_MAILSUBJECT', N'Inner Box Barcode Verification Fail!{0}', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISIONE_MAILSUBJECT', N'FP Vision Verification Fail!{0}', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISION_VSRECIPE_NOT_CONFIGURE', N'Vision recipe not configured.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISION_VSAPI_DOWN', N'Vision System API down. Please check on Vision PC and software configuration', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISIONVAL_LINECLEAR_CLOSEWINDOW', N'System prompt Line Clearance Verification Form to proceed manual close window', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISIONVAL_LINECLEAR_CLOSEWINDOW_INNERBOX', N'{0}/{1} inner boxes validated. {2} inner pending success vision validation.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISIONVAL_LINECLEAR_CLOSEWINDOW_OUTERBOX', N'{0}/{1} outer boxes validated. {2} outer pending success vision validation.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISIONVAL_LINECLEAR_REQUIRED', N'Please perform line clearance at Floor System machine', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISIONVAL_RESPONSE_VALIDATEFAIL', N'Validate fail on ValidateFail', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISIONVAL_RESPONSE_INTERNALSERVERERROR', N'Validation data is empty. Aborting this process', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISIONVAL_RESPONSE_UPDATESCANNO', N'Received on UpdateScanNo', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISIONVAL_NA_MESSAGE', N'Passed as Not Available from Vision System', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'FPVISIONVAL_DEFAULT_APICALL_ERRORMESSAGE', N'fail. No API call to Vision System to load recipe', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'INCORRECT_TOTAL_PCS_SCAN_By_PCS', N'Batch(Pcs) field is more than actual Batch(Pcs)', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'REPRINT_CHANGEGLOVETYPE', N'Glove already changed, please select Changed Batch Order for selected Serial No.', NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'PTPF_NOT_ALLOW', N'PTPF Glove not allowed.', NULL, NULL, NULL, NULL)
COMMIT TRANSACTION

GO

--HBC
if not exists (select * from ScreenMaster where ScreenName = 'Glove Output Reporting' and moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'))
begin
	insert into ScreenMaster (screenname,moduleid) 
	values ('Glove Output Reporting', (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'))
end
go

if not exists (select * from ModuleScreenPermissionMapping where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card')
				and ScreenId = (select ScreenId from ScreenMaster where ScreenName = 'Glove Output Reporting' and 
				moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card')))
begin
	insert into ModuleScreenPermissionMapping (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card') and screenname = 'Glove Output Reporting'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Operator and Above'),1)
end
go

if not exists (select * from [OperatorModulePermissionMapping] where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card')
				and ScreenId = (select ScreenId from ScreenMaster where ScreenName = 'Glove Output Reporting' and 
				moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card')))
begin
	insert into [OperatorModulePermissionMapping] (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card') and screenname = 'Glove Output Reporting'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Operator and Above'),1)
end
go

--ON2G
if not exists (select * from ScreenMaster where ScreenName = 'Online 2nd Grade Glove' and moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'))
begin
	insert into ScreenMaster (screenname,moduleid) 
	values ('Online 2nd Grade Glove', (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'))
end
go

if not exists (select * from ModuleScreenPermissionMapping where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card')
				and ScreenId = (select ScreenId from ScreenMaster where ScreenName = 'Online 2nd Grade Glove' and 
				moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card')))
begin
	insert into ModuleScreenPermissionMapping (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card') and screenname = 'Online 2nd Grade Glove'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Operator and Above'),1)
end
go

if not exists (select * from [OperatorModulePermissionMapping] where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card')
				and ScreenId = (select ScreenId from ScreenMaster where ScreenName = 'Online 2nd Grade Glove' and 
				moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card')))
begin
	insert into [OperatorModulePermissionMapping] (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Hourly Batch Card') and screenname = 'Online 2nd Grade Glove'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Operator and Above'),1)
end
go

--FGBO
 if not exists (select * from screenmaster where ScreenName = 'FG Batch Order')
 BEGIN

	insert into screenmaster
	values ('FG Batch Order', (select moduleid from ModuleMaster where Modulename = 'Final Packing'))

	insert into ModuleScreenPermissionMapping
	values ((select ModuleId from ModuleMaster where modulename = 'Final Packing'), (select ScreenId from screenmaster where screenname = 'FG Batch Order'), '>=', 
	(select PermissionId from PermissionMaster where PermissionDescription = 'MIS'),1)
END
GO

--if exists (select 1 from DOT_FSBrandHeaders where IsDeleted=1)
--BEGIN
--    delete DOT_FSBrandHeaders where IsDeleted=1
--    delete DOT_FSBrandLines where isdeleted=1
--END
--GO


--SRBC
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
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Operator and Above'),1)
	insert into ModuleScreenPermissionMapping (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Glove Batch Order'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Operator and Above'),1)
	insert into ModuleScreenPermissionMapping (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Reprint SRBC'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Operator and Above'),1)
	insert into ModuleScreenPermissionMapping (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Batch Card reprint Log'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Operator and Above'),1)
end
go

if not exists (select * from [OperatorModulePermissionMapping] where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'))
begin
	insert into [OperatorModulePermissionMapping] (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Print Surgical Batch Card'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Operator and Above'),1)
	insert into [OperatorModulePermissionMapping] (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Glove Batch Order'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Operator and Above'),1)
	insert into [OperatorModulePermissionMapping] (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Reprint SRBC'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Operator and Above'),1)
	insert into [OperatorModulePermissionMapping] (ModuleId,ScreenId,PermissionOperator,PermissionId,WorkStationNumber)
	values ((select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System'),
			(select screenId from ScreenMaster where moduleid = (select moduleid from ModuleMaster where ModuleName = 'Surgical Glove System') and screenname = 'Batch Card reprint Log'),
			'>=',(select PermissionId from PermissionMaster where permissiondescription = 'Operator and Above'),1)
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

update enumMaster set EnumText='10+10',EnumValue='10,10' where EnumType = 'VTSamplingSize_QAIScanInnerTenPcs' and EnumValue='10'
go
update enumMaster set EnumText='50+10',EnumValue='50,10' where EnumType = 'VTSamplingSize_QAIScanInnerTenPcs' and EnumValue='50'
go
update enumMaster set EnumText='80+10',EnumValue='80,10' where EnumType = 'VTSamplingSize_QAIScanInnerTenPcs' and EnumValue='80'
go
update enumMaster set EnumText='0+0',EnumValue='0,0' where EnumType = 'VTSamplingSize_QAIScanInnerTenPcs' and EnumValue='0'
go


if not exists (select * from EnumMaster where enumtype  = 'RouteCategory' and EnumValue = 'PT')
begin
	insert into EnumMaster
	select 257,'RouteCategory',	'PT',	'PT',	0,	NULL,	NULL,	NULL,	NULL
end
if not exists (select * from EnumMaster where enumtype  = 'RouteCategory' and EnumValue = 'OQC')
begin
	insert into EnumMaster
	select 258,'RouteCategory',	'OQC',	'OQC',	0,	NULL,	NULL,	NULL,	NULL
end

update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'PT', IsPostWT = 0 where qctype = '0006020001'
go
update DOT_FSQCTypeTable set Stopped = 1, RouteCategory = 'OQC', IsPostWT = 0 where qctype = '0006020002'
go
update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'OQC', IsPostWT = 1 where qctype = '0006020003'
go
update DOT_FSQCTypeTable set Stopped = 1, RouteCategory = 'OQC', IsPostWT = 1 where qctype = '0006020004'
go
update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'OQC', IsPostWT = 0 where qctype = '0006020005'
go
update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'OQC', IsPostWT = 0 where qctype = '0006020006'
go
update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'OQC', IsPostWT = 1 where qctype = '0006020007'
go
update DOT_FSQCTypeTable set Stopped = 1, RouteCategory = 'OQC', IsPostWT = 1 where qctype = '0006020008'
go
update DOT_FSQCTypeTable set Stopped = 1, RouteCategory = 'OQC', IsPostWT = 1 where qctype = '0006020009'
go
update DOT_FSQCTypeTable set Stopped = 1, RouteCategory = 'OQC', IsPostWT = 1 where qctype = '0006020010'
go
update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'OQC', IsPostWT = 0 where qctype = '0006020011'
go
update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'OQC', IsPostWT = 0 where qctype = '0006020012'
go
update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'OQC', IsPostWT = 0 where qctype = '0006020013'
go
update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'OQC', IsPostWT = 0 where qctype = '0006020014'
go
update DOT_FSQCTypeTable set Stopped = 1, RouteCategory = 'OQC', IsPostWT = 1 where qctype = '0006020015'
go
update DOT_FSQCTypeTable set Stopped = 1, RouteCategory = 'OQC', IsPostWT = 1 where qctype = '0006020016'
go
update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'OQC', IsPostWT = 0 where qctype = '0006020017'
go
update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'OQC', IsPostWT = 0 where qctype = '0006020018'
go
update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'OQC', IsPostWT = 0 where qctype = '0006020019'
go
update DOT_FSQCTypeTable set Stopped = 0, RouteCategory = 'PT', IsPostWT = 1 where qctype = '0006020020'
go

delete DOT_PreshipmentSamplingTable
go
insert into DOT_PreshipmentSamplingTable select getdate(),1,null,null,0,null,null,'Order Less than 50,000',1
go
insert into DOT_PreshipmentSamplingTable select getdate(),1,null,null,0,null,null,'Order less than 100000',2
go
insert into DOT_PreshipmentSamplingTable select getdate(),1,null,null,0,null,null,'Order less than 150000',3
go
insert into DOT_PreshipmentSamplingTable select getdate(),1,null,null,0,null,null,'Order less than 8000',4
go
insert into DOT_PreshipmentSamplingTable select getdate(),1,null,null,0,null,null,'Qty more than 90,000',5
go
insert into DOT_PreshipmentSamplingTable select getdate(),1,null,null,0,null,null,'6',6
go

SET IDENTITY_INSERT [dbo].[RoleModuleMapping] ON 
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2151, 20, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2158, 21, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2164, 22, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2187, 26, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2201, 29, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2206, 30, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2215, 34, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2221, 35, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2240, 36, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2247, 37, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2254, 38, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2262, 39, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2267, 40, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2276, 41, 22)
INSERT [dbo].[RoleModuleMapping] ([RoleModuleMappingId], [RoleId], [ModuleId]) VALUES (2289, 44, 22)
SET IDENTITY_INSERT [dbo].[RoleModuleMapping] OFF
GO

update WSConfigurationMaster set ModuleIds = '10,5,7,9,11,12,8,14,13,15,17,6,4,3,16,1,21,19,20,18,0,2,22' where ConfigurationId = 1205
go
update WSConfigurationMaster set ModuleIds = '1,7,22' where ConfigurationId = 1255
go
update WSConfigurationMaster set ModuleIds = '1,22' where ConfigurationId = 1256
go
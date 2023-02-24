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
INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) VALUES (N'RESOURCE_ALREADY_USED_WITH_THE_SELECTED_TIME', N'Resource and Batch Number is already saved with the selected Output Time.', N'1', '20201201 13:33:15.507', N'1', '20201201 13:33:15.507')
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

--7/12/2021 hsb sit issue
update MessageMaster set MessageText = 'Resource is already saved with the selected Output Time.' 
where Messagekey  = 'RESOURCE_ALREADY_USED_WITH_THE_SELECTED_TIME'

select * from MessageMaster
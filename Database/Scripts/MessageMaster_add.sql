
IF NOT EXISTS (SELECT 1 FROM MessageMaster WITH (NOLOCK) WHERE MessageKey = 'INVALID_NEW_PAS_OR_FAIL_PTQI_QC')
Begin
	INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) 
	VALUES (N'INVALID_NEW_PAS_OR_FAIL_PTQI_QC', N'New PTQI Pass/Fail with QC is not allowed.', NULL, NULL, NULL, NULL)
END
GO


IF NOT EXISTS (SELECT 1 FROM MessageMaster WITH (NOLOCK) WHERE MessageKey = 'INVALID_NEW_SURGICAL_PTPF_NO_PTQI_PASS')
Begin
	INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) 
	VALUES (N'INVALID_NEW_SURGICAL_PTPF_NO_PTQI_PASS', N'Please do PTQI Pass first', NULL, NULL, NULL, NULL)
END
GO


IF NOT EXISTS (SELECT 1 FROM MessageMaster WITH (NOLOCK) WHERE MessageKey = 'INVALID_QI_PASS')
Begin
	INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) 
	VALUES (N'INVALID_QI_PASS', N'Previous QI result is PASS, only allow next QI result to FAIL', NULL, NULL, NULL, NULL)
END
GO


IF NOT EXISTS (SELECT 1 FROM MessageMaster WITH (NOLOCK) WHERE MessageKey = 'INVALID_PTQI_PASS_QC')
Begin
	INSERT INTO [dbo].[MessageMaster] ([MessageKey], [MessageText], [LastModifiedBy], [LastModifiedDate], [CreatedBy], [CreatedDate]) 
	VALUES (N'INVALID_PTQI_PASS_QC', N'PTQI Pass with QC is not allowed.', NULL, NULL, NULL, NULL)
END
GO
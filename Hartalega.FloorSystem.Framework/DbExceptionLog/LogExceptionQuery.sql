/****** Object:  Table [dbo].[ExceptionCategory]    Script Date: 05/21/2013 14:59:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExceptionCategory](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExceptionLog]    Script Date: 05/21/2013 14:59:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExceptionLog](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NULL,
	[Priority] [int] NOT NULL,
	[Severity] [nvarchar](32) NOT NULL,
	[Title] [nvarchar](256) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[MachineName] [nvarchar](32) NOT NULL,
	[AppDomainName] [nvarchar](512) NOT NULL,
	[ProcessID] [nvarchar](256) NOT NULL,
	[ProcessName] [nvarchar](512) NOT NULL,
	[ThreadName] [nvarchar](512) NULL,
	[Win32ThreadId] [nvarchar](128) NULL,
	[Message] [nvarchar](1500) NULL,
	[FormattedMessage] [ntext] NULL,
	[AppName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[usp_WriteLog]    Script Date: 05/21/2013 15:00:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[dbo].[usp_WriteLog]') AND type in (N'P', N'PC'))
--BEGIN
CREATE PROCEDURE [dbo].[usp_WriteLog]
(
	@EventID int, 
	@Priority int, 
	@Severity nvarchar(32), 
	@Title nvarchar(256), 
	@Timestamp datetime,
	@MachineName nvarchar(32), 
	@AppDomainName nvarchar(512),
	@ProcessID nvarchar(256),
	@ProcessName nvarchar(512),
	@ThreadName nvarchar(512),
	@Win32ThreadId nvarchar(128),
	@Message nvarchar(1500),
	@FormattedMessage ntext,
	@extendedProperties nvarchar(max),	
	@appName nvarchar(50),
	@LogId int OUTPUT
)
AS 
BEGIN
	INSERT INTO [ExceptionLog] (
		EventID,
		[Priority],
		Severity,
		Title,
		[Timestamp],
		MachineName,
		AppDomainName,
		ProcessID,
		ProcessName,
		ThreadName,
		Win32ThreadId,
		[Message],
		FormattedMessage,
		AppName
	)
	VALUES (
		@EventID, 
		@Priority, 
		@Severity, 
		@Title, 
		@Timestamp,
		@MachineName, 
		@AppDomainName,
		@ProcessID,
		@ProcessName,
		@ThreadName,
		@Win32ThreadId,
		@Message,
		@FormattedMessage,
		@appName)

	SET @LogID = @@IDENTITY

	/*if @extendedProperties is not null and len(@extendedProperties) > 0 
	BEGIN
		declare @x XML
		set @x = convert(XML, @extendedProperties)
		INSERT INTO [ExceptionLogExtendedProperties]([Key], [Value], [LogID])
		SELECT 
			replace(replace(replace(replace(replace(cast (T.c.query('./Key/text()')   as nvarchar(max)),'&quot;' , '"'), '&lt;', '<'), '&gt;', '>'), '&amp;', '&'), '&apos;', '''') as [Key],
			replace(replace(replace(replace(replace(cast (T.c.query('./Value/text()') as nvarchar(max)),'&quot;' , '"'), '&lt;', '<'), '&gt;', '>'), '&amp;', '&'), '&apos;', '''') as [Value],
			@LogId
		FROM   
			@x.nodes('/ExtendedProperties/ExtendedProperty') T(c) 
	END*/
	
	RETURN @LogID

END
GO
/****** Object:  Table [dbo].[ExceptionCategoryLog]    Script Date: 05/21/2013 15:00:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExceptionCategoryLog](
	[CategoryLogID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[LogID] [int] NOT NULL,
 CONSTRAINT [PK_CategoryLog] PRIMARY KEY CLUSTERED 
(
	[CategoryLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ixCategoryLog] ON [dbo].[ExceptionCategoryLog] 
(
	[LogID] ASC,
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExceptionLogExtendedProperties]    Script Date: 05/21/2013 15:00:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExceptionLogExtendedProperties](
	[ExtendedPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[LogID] [int] NOT NULL,
	[Key] [nvarchar](max) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_ExtendedProperties] PRIMARY KEY CLUSTERED 
(
	[ExtendedPropertyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[usp_Get_ExceptionLog]    Script Date: 05/21/2013 15:00:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Get_ExceptionLog]
(
	@appName		NVARCHAR(50)	=	NULL
)
AS
BEGIN
	SELECT [LogID]
      ,[EventID]
      ,[Priority]
      ,[Severity]
      ,[Title]
      ,[Timestamp]
      ,[MachineName]
      ,[AppDomainName]
      ,[ProcessID]
      ,[ProcessName]
      ,[ThreadName]
      ,[Win32ThreadId]
      ,[Message]
      ,[FormattedMessage]
      ,[AppName]
  FROM [ExceptionLog]
  WHERE [AppName] = @appName
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ClearLogs]    Script Date: 05/21/2013 15:00:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ClearLogs]
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [ExceptionLogExtendedProperties]
	DELETE FROM [ExceptionCategoryLog]
	DELETE FROM [ExceptionLog]
    DELETE FROM [ExceptionCategory]
END
GO
/****** Object:  StoredProcedure [dbo].[usp_Save_ExceptionCategoryLog]    Script Date: 05/21/2013 15:00:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Save_ExceptionCategoryLog]
	@CategoryID INT,
	@LogID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CatLogID INT
	SELECT @CatLogID FROM [ExceptionCategoryLog] WHERE CategoryID=@CategoryID and LogID = @LogID
	IF @CatLogID IS NULL
	BEGIN
		INSERT INTO [ExceptionCategoryLog] (CategoryID, LogID) VALUES(@CategoryID, @LogID)
		RETURN @@IDENTITY
	END
	ELSE RETURN @CatLogID
END
GO
/****** Object:  StoredProcedure [dbo].[usp_Save_ExceptionCategory]    Script Date: 05/21/2013 15:00:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Save_ExceptionCategory]
	-- Add the parameters for the function here
	@CategoryName nvarchar(64),
	@LogID int
AS
BEGIN
	SET NOCOUNT ON;
    DECLARE @CatID INT
	SELECT @CatID = CategoryID FROM [ExceptionCategory] WHERE CategoryName = @CategoryName
	IF @CatID IS NULL
	BEGIN
		INSERT INTO [ExceptionCategory] (CategoryName) VALUES(@CategoryName)
		SELECT @CatID = @@IDENTITY
	END

	EXEC usp_Save_ExceptionCategoryLog @CatID, @LogID 

	RETURN @CatID
END
GO
/****** Object:  ForeignKey [FK_CategoryLog_Category]    Script Date: 05/21/2013 15:00:24 ******/
ALTER TABLE [dbo].[ExceptionCategoryLog]  WITH CHECK ADD  CONSTRAINT [FK_CategoryLog_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[ExceptionCategory] ([CategoryID])
GO
ALTER TABLE [dbo].[ExceptionCategoryLog] CHECK CONSTRAINT [FK_CategoryLog_Category]
GO
/****** Object:  ForeignKey [FK_CategoryLog_Log]    Script Date: 05/21/2013 15:00:24 ******/
ALTER TABLE [dbo].[ExceptionCategoryLog]  WITH CHECK ADD  CONSTRAINT [FK_CategoryLog_Log] FOREIGN KEY([LogID])
REFERENCES [dbo].[ExceptionLog] ([LogID])
GO
ALTER TABLE [dbo].[ExceptionCategoryLog] CHECK CONSTRAINT [FK_CategoryLog_Log]
GO
/****** Object:  ForeignKey [FK_ExtendedProperties_Log]    Script Date: 05/21/2013 15:00:24 ******/
ALTER TABLE [dbo].[ExceptionLogExtendedProperties]  WITH CHECK ADD  CONSTRAINT [FK_ExtendedProperties_Log] FOREIGN KEY([LogID])
REFERENCES [dbo].[ExceptionLog] ([LogID])
GO
ALTER TABLE [dbo].[ExceptionLogExtendedProperties] CHECK CONSTRAINT [FK_ExtendedProperties_Log]
GO

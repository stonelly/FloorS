
/****** Object:  StoredProcedure [dbo].[USP_DOT_CreateFLOORAXINTPARENTTABLE]    Script Date: 12/23/2021 3:34:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==============================================
-- Author:  <Azrul Amin>  
-- Create date: <20-Apr-2018>  
-- Description: <Insert_DOT_FLOORAXINTPARENTTABLE
-- ==============================================
ALTER PROCEDURE [dbo].[USP_DOT_CreateFLOORAXINTPARENTTABLE]  
		@BATCHCARDNUMBER nvarchar(50),  
		@BATCHNUMBER nvarchar(20),  
		@FSIDENTIFIER uniqueidentifier,  
		@FUNCTIONIDENTIFIER nvarchar(20),  
		@PLANTNO nvarchar(20), 
		@REFERENCEBATCHNUMBER1 nvarchar(20),  
		@REFERENCEBATCHNUMBER2 nvarchar(20),  
		@REFERENCEBATCHNUMBER3 nvarchar(20),  
		@REFERENCEBATCHNUMBER4 nvarchar(20),  
		@REFERENCEBATCHNUMBER5 nvarchar(20),  
		@REFERENCEBATCHSEQUENCE1 int,  
		@REFERENCEBATCHSEQUENCE2 int,  
		@REFERENCEBATCHSEQUENCE3 int,  
		@REFERENCEBATCHSEQUENCE4 int,  
		@REFERENCEBATCHSEQUENCE5 int,  
		@SEQUENCE int,
		@PROCESSINGSTATUS int = NULL,
		@FGQUATITY int = 0,					--Surgical Packing Plan
		@PRESHIMPENT int = 0,				--Surgical Packing Plan
		@PRESHIPMENTCASES int = 0,			--Surgical Packing Plan
		@GLOVESAMPLEQUANTITY int = 0,		--Surgical Packing Plan
		@ISCONSOLIDATED int = 0				--#AZRUL 20210909: Open batch flag for NGC1.5
AS  
BEGIN                                                                                                                                                                                                                       
	SET NOCOUNT ON;  
		BEGIN
		    set @PROCESSINGSTATUS = Isnull(@PROCESSINGSTATUS,1);

			INSERT INTO [dbo].[DOT_FLOORAXINTPARENTTABLE]  
					   ([BATCHCARDNUMBER]  
					   ,[BATCHNUMBER]  
					   ,[CREATIONTIME]  
					   ,[CREATORUSERID]  
					   ,[DELETERUSERID]  
					   ,[DELETIONTIME]  
					   ,[ERRORMESSAGE]  
					   ,[FSIDENTIFIER]  
					   ,[FUNCTIONIDENTIFIER]  
					   ,[ISDELETED]  
					   ,[LASTMODIFICATIONTIME]  
					   ,[LASTMODIFIERUSERID]  
					   ,[PROCESSINGSTATUS]  
					   ,[PLANTNO]
					   --,[PRODID]  
					   ,[REFERENCEBATCHNUMBER1]  
					   ,[REFERENCEBATCHNUMBER2]  
					   ,[REFERENCEBATCHNUMBER3]  
					   ,[REFERENCEBATCHNUMBER4]  
					   ,[REFERENCEBATCHNUMBER5]  
					   ,[REFERENCEBATCHSEQUENCE1]  
					   ,[REFERENCEBATCHSEQUENCE2]  
					   ,[REFERENCEBATCHSEQUENCE3]  
					   ,[REFERENCEBATCHSEQUENCE4]  
					   ,[REFERENCEBATCHSEQUENCE5]  
					   ,[SEQUENCE]
					   ,[FGQUANTITY]			--Surgical Packing Plan
					   ,[PRESHIPMENT]			--Surgical Packing Plan
					   ,[PRESHIPMENTCASES]		--Surgical Packing Plan 
					   ,[GLOVESAMPLEQUANTITY]   --Surgical Packing Plan
					   ,[ISCONSOLIDATED])		--#AZRUL 20210909: Open batch flag for NGC1.5
				 VALUES  
					  (@BATCHCARDNUMBER,  
					   @BATCHNUMBER,  
					   GETDATE(),  
					   1,
					   NULL,
					   NULL,
					   '',  
					   @FSIDENTIFIER,  
					   @FUNCTIONIDENTIFIER,  
					   0,  
					   GETDATE(),
					   1,
					   @PROCESSINGSTATUS,  
					   @PLANTNO,
					   --NULL,
					   @REFERENCEBATCHNUMBER1,  
					   @REFERENCEBATCHNUMBER2,  
					   @REFERENCEBATCHNUMBER3,  
					   @REFERENCEBATCHNUMBER4,  
					   @REFERENCEBATCHNUMBER5,  
					   dbo.Ufn_DOT_GET_BATCHSEQUENCE(@REFERENCEBATCHNUMBER1), --@REFERENCEBATCHSEQUENCE1,  -- replace by function,  
					   dbo.Ufn_DOT_GET_BATCHSEQUENCE(@REFERENCEBATCHNUMBER2), --@REFERENCEBATCHSEQUENCE2,  -- replace by function,  
					   dbo.Ufn_DOT_GET_BATCHSEQUENCE(@REFERENCEBATCHNUMBER3), --@REFERENCEBATCHSEQUENCE3,  -- replace by function,  
					   dbo.Ufn_DOT_GET_BATCHSEQUENCE(@REFERENCEBATCHNUMBER4), --@REFERENCEBATCHSEQUENCE4,  -- replace by function,  
					   dbo.Ufn_DOT_GET_BATCHSEQUENCE(@REFERENCEBATCHNUMBER5), --@REFERENCEBATCHSEQUENCE5,  -- replace by function
					   dbo.Ufn_DOT_GET_BATCHSEQUENCE(@BATCHNUMBER), --@SEQUENCE, -- replace by function
					   @FGQUATITY,				--Surgical Packing Plan
					   @PRESHIMPENT,			--Surgical Packing Plan
					   @PRESHIPMENTCASES,		--Surgical Packing Plan
					   @GLOVESAMPLEQUANTITY,    --Surgical Packing Plan
					   dbo.Ufn_DOT_GET_IsConsolidated(@BATCHNUMBER,@PLANTNO));-- -- replace by function @ISCONSOLIDATED);		--#AZRUL 20210909: Open batch flag for NGC1.5
			SELECT @@IDENTITY
		END
END

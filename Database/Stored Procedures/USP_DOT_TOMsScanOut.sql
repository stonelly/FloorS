-- ==================================================================================    
-- Name:   USP_DOT_TOMsScanOut  
-- Purpose:  Pump TOMs Scan Out data into staging   
-- ==================================================================================    
-- Change History    
-- Date   Author   Comments    
-- -----  ------   -----------------------------------------------------------------    
-- 18/07/2018  Muhd Khalid  SP created.    
-- 09/09/2021  Azrul   Open batch flag for NGC1.5.  
-- 10/04/2022  Azrul   Get BatchCardNumber from Batch table not 1st staging.  
-- ==================================================================================    
CREATE PROCEDURE [dbo].[USP_DOT_TOMsScanOut]    
@FGItemCode nvarchar(40),  
@IsOrignalTemppack bit,  
@Configuration nvarchar(10),  
@GloveCode nvarchar(40),  
@Warehouse nvarchar(10),  
@PalletId nvarchar(20),  
@PalletTotalQty numeric(32, 16),  
@ScanOutDateTime datetime,  
@BatchSerialNos nvarchar(max),  
@Location nvarchar(10)  
AS  
BEGIN  
  BEGIN TRANSACTION;  
    BEGIN TRY  
      --Parent table variable  
      DECLARE @BATCHCARDNUMBER nvarchar(50),  
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
              --Transfer Journal Variable  
              --@BRAND nvarchar(20),  
              @FORMULA nvarchar(20),  
              --@ITEMNUMBER nvarchar(20),  
              --@LOCATION nvarchar(10),  
              @PARENTREFRECID int,  
              @QUANTITY numeric(32, 16),  
              @ServiceName nvarchar(50),  
              @PostingType nvarchar(20),  
              @PostedDate datetime,  
              @SerialNumber numeric(15, 0),  
              @IsPostedToAX bit,  
              @IsPostedInAX bit,  
              @ExceptionCode nvarchar(1000),  
              @TransactionID nvarchar(100),  
              @Area nvarchar(10),  
              @SPLITDATA nvarchar(50),  
     @LastService nvarchar(20),  
     @IsConsolidated bit -- #AZRUL 17/9/2021: Open batch flag for NGC1.5  
  
  DECLARE @tempTable Table (Seq int, Result BIT) -- #AZRUL 17/9/2021: Open batch flag for NGC1.5  
  
  if @IsOrignalTemppack=0 and isnull(@Warehouse,'')=''    
   BEGIN    
     RAISERROR ('Not orignal temp pack warehouse can not empty!', -- Message text.      
        16, -- Severity.      
        1 -- State.      
        );      
   END     
  
  if @IsOrignalTemppack=0 and isnull(@Location,'')=''    
   BEGIN    
     RAISERROR ('Not orignal temp pack warehouse''s location can not empty!', -- Message text.      
        16, -- Severity.      
        1 -- State.      
        );      
   END     
  
  if @IsOrignalTemppack=1 and isnull(@FGItemCode,'')=''    
   BEGIN    
     RAISERROR ('Orignal temp pack FG brand info can not empty!', -- Message text.      
        16, -- Severity.      
        1 -- State.      
        );      
   END     
     
   --SET @FSIDENTIFIER = NEWID();  
      SELECT  
        ROW_NUMBER() OVER (ORDER BY (SELECT  
          1)  
        ) AS id,  
        Data INTO #tempTable  
      FROM SPLIT(@BatchSerialNos, ',');  
  
      DECLARE @COUNT int = (SELECT  
        MAX(id)  
      FROM #tempTable);  
      DECLARE @ROW int = 1;  
  
      WHILE (@ROW <= @COUNT)  
      BEGIN  
        SET @SPLITDATA = (SELECT  
          Data  
        FROM #tempTable  
        WHERE id = @ROW);  
        SET @BATCHNUMBER = SUBSTRING(@SPLITDATA, 0, CHARINDEX(':', @SPLITDATA));  
        SET @BATCHCARDNUMBER = (SELECT BatchNumber from Batch with (nolock)  
        WHERE cast(serialnumber as nvarchar(100)) = @BATCHNUMBER);  
  
  --check last posting, block if STPI  
  select Top 1 @LastService = ServiceName from AXPostingLog with (nolock) where SerialNumber=@BATCHNUMBER order by CreationDate desc   
  
  if @LastService = 'STPO'  
  BEGIN  
   RAISERROR ('Temp pack already scan out!', -- Message text.      
      16, -- Severity.      
      1 -- State.      
      );      
  END  
          
  SET @FSIDENTIFIER = NEWID(); -- fix Duplicated GUID, Max He, 4/1/2019  
        SET @FUNCTIONIDENTIFIER = 'STPO';  
        --SET @PLANTNO = SUBSTRING(@Warehouse, 0, 3);  
        SET @REFERENCEBATCHNUMBER1 = NULL;  
        SET @REFERENCEBATCHNUMBER2 = NULL;  
        SET @REFERENCEBATCHNUMBER3 = NULL;  
        SET @REFERENCEBATCHNUMBER4 = NULL;  
        SET @REFERENCEBATCHNUMBER5 = NULL;  
        SET @REFERENCEBATCHSEQUENCE1 = 0;  
        SET @REFERENCEBATCHSEQUENCE2 = 0;  
        SET @REFERENCEBATCHSEQUENCE3 = 0;  
        SET @REFERENCEBATCHSEQUENCE4 = 0;  
        SET @REFERENCEBATCHSEQUENCE5 = 0;  
        SET @SEQUENCE = (SELECT  
          COUNT(SerialNumber) + 1 AS 'BatchSequence'  
        FROM dbo.AXPostingLog  
        WHERE SerialNumber = @BATCHNUMBER  
        AND (exceptioncode IS NULL  
        OR exceptioncode = '999'));  
    
  -- Find previous record plant no  
  -- Special handling for STPI/STPO due to TOMs didn't send plant no, Max He, 19/07/2021  
  select @PLANTNO = PlantNo from DOT_FloorAxIntParentTable WITH (NOLOCK)   
  where BatchNumber=@BATCHNUMBER and Sequence=@SEQUENCE-1 and IsDeleted=0 and IsMigratedFromAX6 = 0 AND IsDeleted=0;  
  print @plantno;  
        --SET @BRAND = '';  
        SET @FORMULA = '';  
        --SET @ITEMNUMBER = @GloveCode;  
        --SET @LOCATION = '';  
        SET @QUANTITY = SUBSTRING(@SPLITDATA, CHARINDEX(':', @SPLITDATA) + 1, LEN(@SPLITDATA));  
        SET @ServiceName = @FUNCTIONIDENTIFIER;  
        SET @PostingType = 'DOTTOMsScanOut';  
        SET @PostedDate = GETDATE();  
        SET @SerialNumber = @BATCHNUMBER;  
        SET @IsPostedToAX = 1;  
        SET @IsPostedInAX = 1;  
        SET @ExceptionCode = NULL;  
        SET @TransactionID = '-1';  
        SET @Area = @LOCATION;  
  
  -- #AZRUL 17/9/2021: Open batch flag for NGC1.5 START  
  INSERT INTO @tempTable  
   EXEC dbo.USP_GET_BATCHSEQUENCE @SerialNumber, @PLANTNO  
  
  SET @IsConsolidated = (SELECT Result FROM @tempTable)  
  DELETE @tempTable  
  -- #AZRUL 17/9/2021: Open batch flag for NGC1.5 END  
  
  --print @SEQUENCE  
        --Transaction start  
        IF NOT EXISTS (SELECT  
            *  
          FROM DOT_FloorAxIntParentTable WITH (NOLOCK)  
          WHERE [BATCHCARDNUMBER] = @BATCHCARDNUMBER  
          AND [BATCHNUMBER] = @BATCHNUMBER  
          AND [FUNCTIONIDENTIFIER] = @FUNCTIONIDENTIFIER  
          AND [PLANTNO] = @PLANTNO  
    AND [SEQUENCE] = @SEQUENCE  
    AND IsDeleted=0)  
        BEGIN  
          INSERT INTO [dbo].[DOT_FLOORAXINTPARENTTABLE] ([BATCHCARDNUMBER]  
          , [BATCHNUMBER]  
          , [CREATIONTIME]  
          , [CREATORUSERID]  
          , [DELETERUSERID]  
          , [DELETIONTIME]  
          , [ERRORMESSAGE]  
          , [FSIDENTIFIER]  
          , [FUNCTIONIDENTIFIER]  
          , [ISDELETED]  
          , [LASTMODIFICATIONTIME]  
          , [LASTMODIFIERUSERID]  
          , [PROCESSINGSTATUS]  
          , [PLANTNO]  
          , [PRODID]  
          , [REFERENCEBATCHNUMBER1]  
          , [REFERENCEBATCHNUMBER2]  
          , [REFERENCEBATCHNUMBER3]  
          , [REFERENCEBATCHNUMBER4]  
          , [REFERENCEBATCHNUMBER5]  
          , [REFERENCEBATCHSEQUENCE1]  
          , [REFERENCEBATCHSEQUENCE2]  
          , [REFERENCEBATCHSEQUENCE3]  
          , [REFERENCEBATCHSEQUENCE4]  
          , [REFERENCEBATCHSEQUENCE5]  
          , [SEQUENCE]  
          , [PALLETID]  
    , [IsConsolidated]) -- #AZRUL 17/9/2021: Open batch flag for NGC1.5  
            VALUES (@BATCHCARDNUMBER, @BATCHNUMBER, GETDATE(), 1, NULL, NULL, '', @FSIDENTIFIER, @FUNCTIONIDENTIFIER, 0, GETDATE(), 1, 1, @PLANTNO, NULL, @REFERENCEBATCHNUMBER1, @REFERENCEBATCHNUMBER2, @REFERENCEBATCHNUMBER3, @REFERENCEBATCHNUMBER4, @REFERENCEBATCHNUMBER5, @REFERENCEBATCHSEQUENCE1, @REFERENCEBATCHSEQUENCE2, @REFERENCEBATCHSEQUENCE3, @REFERENCEBATCHSEQUENCE4, @REFERENCEBATCHSEQUENCE5, @SEQUENCE, @PalletId, @IsConsolidated);  
  
          SET @PARENTREFRECID = (SELECT  
            @@IDENTITY);  
        END  
        ELSE  
        BEGIN  
          SET @PARENTREFRECID = (SELECT  
            Id  
          FROM DOT_FloorAxIntParentTable WITH (NOLOCK)  
          WHERE [BATCHCARDNUMBER] = @BATCHCARDNUMBER  
          AND [BATCHNUMBER] = @BATCHNUMBER  
          AND [FUNCTIONIDENTIFIER] = @FUNCTIONIDENTIFIER  
          AND [PLANTNO] = @PLANTNO  
    AND [SEQUENCE] = @SEQUENCE  
    AND IsDeleted=0);  
        END  
  
        EXEC dbo.USP_DOT_CreateTransferJournal @BATCHCARDNUMBER,  
                                               @BATCHNUMBER,  
                                               @FGItemCode, --@BRAND,  
                                               @Configuration,  
                                               @FORMULA,  
                                               @GloveCode,  
                                               @LOCATION,  
                                               @PARENTREFRECID,  
                                               @QUANTITY,  
                                               @ScanOutDateTime,  
                                               @ScanOutDateTime,  
                                               '',  
                                               @Warehouse,  
              @IsOrignalTemppack;  
  
        EXEC dbo.USP_SAVE_AXPOSTINGLOG @ServiceName,  
                                       @PostingType,  
                                       @PostedDate,  
                                       @BATCHCARDNUMBER,  
                                       @SerialNumber,  
                                       @IsPostedToAX,  
                                       @IsPostedInAX,  
                                       @Sequence,  
                                       @ExceptionCode,  
                                       @TransactionID,  
                                       @Area,  
            @IsConsolidated  
  
        SET @ROW = @ROW + 1  
      END  
  
      DROP TABLE #tempTable;  
    END TRY  
    BEGIN CATCH  
      DECLARE @ErrorMessage nvarchar(4000);  
      DECLARE @ErrorSeverity int;  
      DECLARE @ErrorState int;  
      SELECT  
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
      RAISERROR (@ErrorMessage,  
      @ErrorSeverity,  
      @ErrorState  
      );  
  
      IF @@TRANCOUNT > 0  
        ROLLBACK TRANSACTION;  
    END CATCH;  
  
    IF @@TRANCOUNT > 0  
    COMMIT TRANSACTION;  
  
END  
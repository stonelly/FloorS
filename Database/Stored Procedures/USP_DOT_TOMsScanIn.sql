USE [FloorSystem]
GO
/****** Object:  StoredProcedure [dbo].[USP_DOT_TOMsScanIn]    Script Date: 1/4/2022 6:12:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================================================================================    
-- Name: USP_DOT_TOMsScanIn  
-- Purpose:  Pump TOMs Scan In data into staging   
-- ==================================================================================    
-- Change History    
-- Date   Author   Comments    
-- -----  ------   -----------------------------------------------------------------    
-- 18/07/2018  Muhd Khalid  SP created.    
-- 09/09/2021  Azrul   Open batch flag for NGC1.5.  
-- 25/12/2021  Max He   get Isconsolidated by function and fix STPI after SMBP  
-- ==================================================================================    
--@BatchSerialNos '218023232:150,218023233:250'  
--USP_DOT_TOMsScanIn '','',0,'M','NB-AB-OLPF-035-SE-WHTE-OM6N','PH1-TP','NT100012',0.0,'2021-12-25','2211010100:22000','TP'  
ALTER PROCEDURE [dbo].[USP_DOT_TOMsScanIn]   
@FGItemCode nvarchar(40),  
@FGProductionOrder nvarchar(40),  
@IsOrignalTemppack bit,  
@Configuration nvarchar(10),  
@GloveCode nvarchar(40),  
@Warehouse nvarchar(10),  
@PalletId nvarchar(20),  
@PalletTotalQty numeric(32, 16),  
@ScanInDateTime datetime,  
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
     @ReworkCount int,  
     @SOBCCount int,  
     @MaxQty int,  
     @LastService nvarchar(20),  
     @IsConsolidated bit -- #AZRUL 17/9/2021: Open batch flag for NGC1.5  
  
  --DECLARE @tempTable Table (Seq int, Result BIT) -- #AZRUL 17/9/2021: Open batch flag for NGC1.5  
  
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
        --SET @BATCHCARDNUMBER = (SELECT TOP 1  
        --  BatchcardNumber  
        --FROM DOT_FloorAxIntParentTable  
        --WHERE BatchNumber = @BATCHNUMBER AND IsDeleted=0);
		SET @BATCHCARDNUMBER = (SELECT BatchNumber from Batch with (nolock)  
        WHERE cast(serialnumber as nvarchar(100)) = @BATCHNUMBER); 
  
  -- Validate not done SOBC can't do STPI  
  select @ReworkCount = count(1) from DOT_FloorAxIntParentTable WITH (NOLOCK) where BatchNumber=@BATCHNUMBER and FunctionIdentifier='RWKCR' and IsDeleted=0 and IsMigratedFromAX6 = 0  
  
  select @SOBCCount = count(1) from DOT_FloorAxIntParentTable WITH (NOLOCK) where BatchNumber=@BATCHNUMBER and FunctionIdentifier='SOBC' and IsDeleted=0 and IsMigratedFromAX6 = 0  
  
    if ((@ReworkCount<>@SOBCCount) OR (@ReworkCount = 0 AND @SOBCCount = 0)) --Fatin 20220331: HSB not require QCQI checking during TOMS.
  BEGIN    
   -- allow insert TOMS if got extra rework from HBC RESAMPLE   
      DECLARE @ResampleCount INT    
  
   select @ResampleCount=COUNT(1)   
   from DOT_FloorAxIntParentTable a with (nolock)   
   join DOT_RafStgTable c with (nolock) on a.id = c.ParentRefRecId  
   join DOT_FloorD365HRGLOVERPT b with (nolock) on a.BatchNumber = b.SerialNo and b.Resource = c.Resource  
   where a.BatchNumber = @BATCHNUMBER and a.FunctionIdentifier = 'HBC'   
   and a.ReferenceBatchNumber1 = 'RESAMPLE' and b.SeqNo = 1  
   and a.IsMigratedFromAX6=0 -- added by Max He on 22/5/2019, to bypass if RESAMPLE is from AX migration  
   and a.IsDeleted=0  
  
   SET @ResampleCount = ISNULL(@ResampleCount,0);   
   SET @ReworkCount = @ReworkCount-@ResampleCount  
  
   if @ReworkCount<>@SOBCCount    
    BEGIN  
     RAISERROR ('In Order to proceed scan in,please complete QC process!', -- Message text.      
          16, -- Severity.      
          1 -- State.      
          );      
    END  
   END     
  
  --check last posting, block if STPI  
  select Top 1 @LastService = ServiceName from AXPostingLog where SerialNumber=@BATCHNUMBER order by CreationDate desc   
  
  if @LastService = 'STPI'  
  BEGIN  
   RAISERROR ('Temp pack not scan out!', -- Message text.      
      16, -- Severity.      
      1 -- State.      
      );      
  END  
  
        SET @FSIDENTIFIER = NEWID(); -- fix Duplicated GUID, Max He, 4/1/2019  
        SET @FUNCTIONIDENTIFIER = 'STPI';  
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
        --SET @SEQUENCE = (SELECT  
        --  COUNT(SerialNumber) + 1 AS 'BatchSequence'  
        --FROM dbo.AXPostingLog  
        --WHERE SerialNumber = @BATCHNUMBER  
        --AND (exceptioncode IS NULL  
        --OR exceptioncode = '999'));  
  
  set @SEQUENCE = dbo.Ufn_DOT_GET_BATCHSEQUENCE(@BATCHNUMBER);  
  
  -- Find previous record plant no  
  -- Special handling for STPI/STPO due to TOMs didn't send plant no, Max He, 19/07/2021  
  select @PLANTNO = PlantNo from DOT_FloorAxIntParentTable WITH (NOLOCK)   
  where IsMigratedFromAX6 = 0 and IsDeleted=0 and   
  ((BatchNumber=@BATCHNUMBER and Sequence=@SEQUENCE-1)  
   or   
   (ReferenceBatchNumber1=@BATCHNUMBER and  ReferenceBatchSequence1=@SEQUENCE-1 )  
   or   
   (ReferenceBatchNumber2=@BATCHNUMBER and  ReferenceBatchSequence2=@SEQUENCE-1 )  
   or                    
   (ReferenceBatchNumber3=@BATCHNUMBER and  ReferenceBatchSequence3=@SEQUENCE-1 )  
   or                    
   (ReferenceBatchNumber4=@BATCHNUMBER and  ReferenceBatchSequence4=@SEQUENCE-1 )  
   or                    
   (ReferenceBatchNumber5=@BATCHNUMBER and  ReferenceBatchSequence5=@SEQUENCE-1 ));  
        --SET @BRAND = '';  
        SET @FORMULA = '';  
        --SET @ITEMNUMBER = @GloveCode;  
        --SET @LOCATION = '';  
        SET @QUANTITY = SUBSTRING(@SPLITDATA, CHARINDEX(':', @SPLITDATA) + 1, LEN(@SPLITDATA));  
  
  -- #AZRUL 7/8/2019: Validate Qty to proceed STPI START  
  SELECT TOP 1 @MaxQty = BalancePcs FROM dbo.ufn_GetBatchSummaryTable(@BATCHNUMBER) ORDER BY ProcessDate DESC    
  
  if @QUANTITY>@MaxQty    
   BEGIN  
    RAISERROR ('Over maximum quantity!', -- Message text.      
       16, -- Severity.      
       1 -- State.      
       );      
   END  
  if @QUANTITY=0    
  BEGIN  
   RAISERROR ('Quantity must be more than 0!', -- Message text.      
      16, -- Severity.      
      1 -- State.      
      );      
  END  
  -- #AZRUL 7/8/2019: END  
  
        SET @ServiceName = @FUNCTIONIDENTIFIER;  
        SET @PostingType = 'DOTTOMsScanIn';  
        SET @PostedDate = GETDATE();  
        SET @SerialNumber = @BATCHNUMBER;  
        SET @IsPostedToAX = 1;  
        SET @IsPostedInAX = 1;  
        SET @ExceptionCode = NULL;  
        SET @TransactionID = '-1';  
        SET @Area = @LOCATION;  
  
  -- #AZRUL 17/9/2021: Open batch flag for NGC1.5 START  
  --INSERT INTO @tempTable  
  -- EXEC dbo.USP_GET_BATCHSEQUENCE @SerialNumber, @PLANTNO  
  
  --SET @IsConsolidated = (SELECT Result FROM @tempTable)  
  SET @IsConsolidated = dbo.Ufn_DOT_GET_IsConsolidated(@SerialNumber,@PLANTNO);  
  --DELETE @tempTable  
  -- #AZRUL 17/9/2021: Open batch flag for NGC1.5 END  
  
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
          INSERT INTO [dbo].[DOT_FloorAxIntParentTable] ([BATCHCARDNUMBER]  
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
                                               @FGItemCode,--@BRAND,  
                                               @Configuration,  
                                               @FORMULA,  
                                               @GloveCode,  
                                               @LOCATION,  
                                               @PARENTREFRECID,  
                                               @QUANTITY,  
                                               @ScanInDateTime,  
                                               @ScanInDateTime,  
                                               '',  
                                               @Warehouse,  
              @IsOrignalTemppack,  
              @FGProductionOrder;  
  
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
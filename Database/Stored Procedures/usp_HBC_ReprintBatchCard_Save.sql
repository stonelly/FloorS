  
-- =============================================    
-- Name:   usp_HBC_ReprintBatchCard_Save    
-- Purpose:   Save ReprintBatchCard Details    
-- =============================================    
-- Change History    
-- Date    Author   Comments    
-- -----   ------   -----------------------------    
-- 27/06/2014  Narendranath    SP created.    
-- 27/06/2018  Azrul      SP altered.    
-- 15/12/2021  Azrul      HSB SIT Issue: Add PrintType.    
-- 31/03/2022  Azrul      Handling for HSB Open Batch.      
-- 31/03/2022  Azrul      Handling for Reprint PNBC.     
-- =============================================    
    
CREATE PROCEDURE [dbo].[usp_HBC_ReprintBatchCard_Save]    
(    
	@SerialNumber  NVARCHAR(3000),
	@ReprintDateTime DATETIME,
	@OperatorId   NVARCHAR(20),
	@ReasonId   INT, 
	@WorkStationName    NVARCHAR(150)
)    
AS    
BEGIN     
DECLARE  @LocationId INT    
DECLARE  @ProcessArea NVARCHAR(20)    
DECLARE  @BatchType NVARCHAR(20)    
BEGIN TRANSACTION;    
BEGIN TRY    
 SELECT @LocationId=LocationId FROM WorkStationMaster c WHERE WorkStationName=@WorkStationName    
  SELECT  @ProcessArea=Area FROM LocationMaster c WHERE LocationId=@LocationId    
 INSERT INTO ReprintBatchCard(SerialNumber,OperatorId,ReprintDateTime,ReasonId,ProcessArea,PrintDatetime,LocationId,ReprintHour,PrintType)    
    
  SELECT SerialNumber,@OperatorId,@ReprintDateTime,@ReasonId,@ProcessArea,BatchCardDate,@LocationId,convert(TIME,@ReprintDateTime),'REPRINT' FROM BATCH b    
  JOIN dbo.ufn_CSVToTable(@SerialNumber) ct ON b.SerialNumber= ct.String     
     
 SELECT @BatchType = RTRIM(BatchType) FROM Batch WITH (NOLOCK) WHERE SerialNumber = @SerialNumber    
 IF (@BatchType = 'T')    
 BEGIN    
  IF ((SELECT COUNT (DISTINCT PackingSz) FROM DOT_FloorD365HRGLOVERPT WITH (NOLOCK) WHERE SerialNo = @SerialNumber) = 1)    
  BEGIN         
   --select result for print batch card if same PackingSize    
   SELECT b.OutTime,a.SerialNumber,a.BatchNumber,a.GloveType, a.Size,a.BatchCardDate,    
   STUFF((SELECT ', ' + r.Resource FROM DOT_FloorD365HRGLOVERPT r WITH (NOLOCK) WHERE r.SerialNo = a.SerialNumber FOR XML path('') ), 1, 2, '') AS Resource,    
   b.PackingSz AS PackingSize,    
   SUM(b.InBox) AS InnerBox,    
   REPLACE(CONVERT(VARCHAR,CONVERT(Money, SUM(b.PackingSz * b.InBox)),1),'.00','') as TotalGloveQty ,a.BatchType,a.BatchWeight,a.TenPCsWeight      
   from batch a WITH (NOLOCK) left join DOT_FloorD365HRGLOVERPT as b  WITH (NOLOCK) on a.SerialNumber = b.SerialNo      
   where a.LocationId = @LocationId and a.SerialNumber = @SerialNumber       
   group by b.PackingSz,b.OutTime,a.SerialNumber,a.BatchNumber,a.GloveType,a.Size,a.BatchCardDate,a.BatchType,a.BatchWeight,a.TenPCsWeight        
  END    
  ELSE    
  BEGIN    
  IF NOT EXISTS (select 1 from DOT_InventBatchSum where BatchNumber = @SerialNumber and IsMigratedFromAX6 = 1) --handling for HSB Open Batch
  BEGIN
   --select result for print batch card if diff PackingSize    
   SELECT b.OutTime,a.SerialNumber,a.BatchNumber,a.GloveType, a.Size,a.BatchCardDate,    
   STUFF((SELECT ', ' + r.Resource FROM DOT_FloorD365HRGLOVERPT r WITH (NOLOCK) WHERE r.SerialNo = a.SerialNumber FOR XML path('') ), 1, 2, '') AS Resource,    
   STUFF((SELECT ',' + CONVERT(NVARCHAR,p.PackingSz) FROM DOT_FloorD365HRGLOVERPT p WITH (NOLOCK) WHERE p.SerialNo = a.SerialNumber FOR XML path('') ), 1, 1, '') AS PackingSize,    
   STUFF((SELECT ',' + CONVERT(NVARCHAR,i.InBox) FROM DOT_FloorD365HRGLOVERPT i WITH (NOLOCK) WHERE i.SerialNo = a.SerialNumber FOR XML path('') ), 1, 1, '') AS InnerBox,    
   REPLACE(CONVERT(VARCHAR,CONVERT(Money, SUM(b.PackingSz * b.InBox)),1),'.00','') as TotalGloveQty     
   from batch a WITH (NOLOCK) left join DOT_FloorD365HRGLOVERPT as b  WITH (NOLOCK) on a.SerialNumber = b.SerialNo    
   where a.LocationId = @LocationId and a.SerialNumber = @SerialNumber     
   group by b.OutTime,a.SerialNumber,a.BatchNumber,a.GloveType,a.Size,a.BatchCardDate   
  END
  ELSE
  BEGIN
   --handling for HSB Open Batch, get all info from batch table   
   SELECT a.BatchCardDate as OutTime,a.SerialNumber,a.BatchNumber,a.GloveType, a.Size,a.BatchCardDate,    
   '' AS Resource,'' AS PackingSize,'' AS InnerBox,a.TotalPCs as TotalGloveQty,a.BatchType,a.BatchWeight,a.TenPCsWeight
   from batch a with (nolock)   
   where a.LocationId = @LocationId and a.SerialNumber = @SerialNumber     
  END 
  END    
 END    
 ELSE    
 BEGIN    
  -- IF Online 2G Glove    
  IF EXISTS (SELECT * FROM DOT_FloorD365Online2G WHERE SerialNumber = @SerialNumber)    
  BEGIN    
   SELECT FORMAT(SerialNumber,'0000000000') AS SerialNumber, BatchNumber, GloveCode as GloveType,Size, CurrentDateandTime as BatchCardDate,    
   Resource,CurrentDateandTime as OutTime,  PackingSize, InnerBox, PackingSize*InnerBox as TotalGloveQty    
   FROM DOT_FloorD365Online2G WITH (NOLOCK) WHERE Plant = 'P'+ CAST(@LocationId AS VARCHAR(2)) and SerialNumber = @SerialNumber     
  END    
  ELSE    
  BEGIN    
   --select result for print batch card other than HBC    
   SELECT SerialNumber,BatchNumber,GloveType,Size,BatchCardDate,BatchCardDate,BatchWeight,TenPcsWeight,LTRIM(BatchType) as BatchType    
   FROM batch WITH (NOLOCK) WHERE LocationId = @LocationId and SerialNumber = @SerialNumber     
  END    
 END    
    
END TRY    
BEGIN CATCH    
 DECLARE @ErrorMessage NVARCHAR(4000);    
 DECLARE @ErrorSeverity INT;    
 DECLARE @ErrorState INT;    
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

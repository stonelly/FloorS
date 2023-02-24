-- =============================================  
-- Name:   [dbo].[usp_DryerBatch_SerialNo_Get]  
-- Purpose:   <If serial number entered already exists in the DryerScanBatchCard table   
--     then SerialNumber, StartTime,StopTime, Dryer, DryerProgram and   
--     ReworkCount will be returned from DryerScanBatchCard table>  
-- =============================================  
-- Change History  
-- Date   Author    Comments  
-- -----  ------    -----------------------------  
-- 07/07/2014  Kishan    SP created.  
-- 16/07/2014  Kishan     Changes after review by Narendra  
-- =============================================  
-- 05/04/2018 Azman Kasim  Change Dryer Program  
-- 08/01/2019  Azrul    Get the Dryer program name instead of dryer program id  
CREATE PROCEDURE [dbo].[usp_DryerBatch_SerialNo_Get]  
(  
@serialNo DECIMAL  
)  
AS  
BEGIN  
SET NOCOUNT ON;  
  
 --SELECT TOP 1 SerialNumber,DryerId,DryerProgram,StartDateTime as StartTime,StopDateTime as StopTime,ReworkReason AS ReworkType,  
 --ReworkCount,LastModifiedOn FROM DryerScanBatchCard  
 --WHERE SerialNumber = @serialNo ORDER BY ReworkCount DESC  
  
 --SELECT TOP 100 SerialNumber,DryerId, B.CYCLONEPROCESS as DryerProgram,StartDateTime as StartTime,StopDateTime as StopTime,ReworkReason AS ReworkType,  
 --ReworkCount,LastModifiedOn FROM DryerScanBatchCard A INNER JOIN AX_AVACYCLONEPROCESSTABLE B ON A.DryerProgram = B.AVACYCLONEPROCESSTABLE_ID  
 --WHERE SerialNumber = @serialNo ORDER BY ReworkCount DESC  
  
 -- SELECT TOP 100 SerialNumber,DryerId, B.CYCLONEPROCESS as DryerProgram,StartDateTime as StartTime,StopDateTime as StopTime,ReworkReason AS ReworkType,  
 --ReworkCount,LastModifiedOn FROM DryerScanBatchCard A INNER JOIN DOT_FSCycloneProgram B ON A.DryerProgram = B.Id  
 --WHERE SerialNumber = @serialNo ORDER BY ReworkCount DESC  

  SELECT TOP 1 dsb.SerialNumber,dsb.DryerId,dp.Id as DryerProgramId,dp.CycloneProcess as DryerProgram,dsb.StartDateTime as StartTime,dsb.StopDateTime as StopTime,  
 dsb.ReworkReason AS ReworkType,dsb.ReworkCount,dsb.LastModifiedOn   
 FROM DryerScanBatchCard dsb  
 join DOT_FSCycloneProgram dp on dsb.DryerProgram=dp.Id  
 WHERE SerialNumber = @serialNo ORDER BY ReworkCount DESC  
END  
  
  
  
  
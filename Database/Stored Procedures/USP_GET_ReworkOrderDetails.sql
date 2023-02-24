-- =======================================================  
-- Name:             USP_GET_ReworkOrderDetails  
-- Purpose:          Get Rework Order Details  
-- =======================================================  
-- Change History  
-- Date    Author     Comments  
-- -----   ------     ---------------------------  
-- 19/07/2018  Azrul  SP altered.  
-- 13/04/2022  Azrul  Add BatchType.  
-- =======================================================   
CREATE OR ALTEr PROCEDURE [dbo].[USP_GET_ReworkOrderDetails]  
(  
 @serialNo decimal,  
 @QCType Nvarchar(100)  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
 SELECT b.BatchNumber,b.Size,  
  CASE WHEN ISNULL(b.QCBatchWeight,0) = 0  
  THEN b.TotalPCs - ISNULL(b.PackedPcs,0)  
  ELSE ((ISNULL(b.QCBatchWeight,0) / ISNULL(b.QCTenPcsWeight,0)) * 10 * 1000) - ISNULL(b.PackedPcs,0)  
  END AS TotalPCs,  
 b.GloveType,  
 GETDATE() + 1 as DeliveryDate,  
 (SELECT RouteCategory from DOT_FSQCTypeTable where QCType = @QCType) as [Pool],   
 (SELECT RouteCategory from DOT_FSQCTypeTable where QCType = @QCType) as RouteCategory,  
 'PN' as Area, b.BatchType  
 FROM Batch b with(nolock) WHERE b.SerialNumber=@serialNo  
END  
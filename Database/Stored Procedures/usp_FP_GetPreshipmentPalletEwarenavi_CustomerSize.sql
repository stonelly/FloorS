
-- =============================================
-- Author:		Fusionex
-- Create date: 
-- Description:	created

-- Modififed by : Pang YS to fix join condition between salestable and POIC 
-- MOdiified date: 2019-04-26

-- Modififed by : Pang YS to fix add itemsize for 9Dots new field (EWN_CompletedPallet.FGCodeAndSize) 
-- MOdiified date: 2022-04-14
--==============================================================================
ALTER PROCEDURE [dbo].[usp_FP_GetPreshipmentPalletEwarenavi_CustomerSize]              
  @PalletId nvarchar(8),
  @PONumber nvarchar(20),
  @ItemName nvarchar(40)
AS
BEGIN
                
 SET NOCOUNT ON;
 -- Insert statements for procedure here
 select  poic.PONumber, poic.ItemNumber, sl.HartalegaCommonSize AS ItemSize, sl.CustomerSize AS size,poic.PalletId,count(poic.casenumber) as CaseCount  
 from PurchaseOrderItemCases poic (NOLOCK)
 inner join SalesLineWorkOrder sl (NOLOCK)
 --on poic.InventTransId = sl.InventTransId
 on poic.PONumber = sl.SalesId
 and poic.ItemNumber = sl.ItemId
 and poic.CustomerSize = sl.CustomerSize
 where poic.PalletId = @PalletId and poic.PONumber = @PONumber and poic.ItemNumber = @ItemName
 group by poic.PONumber, poic.ItemNumber,sl.HartalegaCommonSize,sl.CustomerSize,poic.PalletId

END
GO

USE [FloorSystemUAT]
GO
/****** Object:  StoredProcedure [dbo].[OEE_SP_GetGloveQuantity]    Script Date: 21/01/2022 17:19:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Azman Kasim>
-- Create date: <09/11/2021>
-- Description:	<OEE Hotbox>
-- =============================================
ALTER PROCEDURE [dbo].[OEE_SP_GetGloveQuantity]
	-- Add the parameters for the stored procedure here
	@OperationStartDate datetime,
	@OperationEndDate datetime,
	@LineId nchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT a.SerialNumber, a.QCType, a.BatchWeight, Cast(((a.BatchWeight*10000)/(Cast(a.TenPCsWeight as decimal(9,2)))) As Decimal(9,0)) As [Pcs],
	b.QCTYPEBeforeHotbox, b.HotboxVerifiedDate, b.IsHotbox12Month
	FROM [dbo].[Batch] a with (nolock)
	LEFT JOIN dbo.OEE_HotboxGlove b (nolock) ON a.SerialNumber = b.SerialNumber
	--WHERE (a.BatchCardDate BETWEEN '" + dataDTO.OperationStartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + dataDTO.OperationEndDate.ToString("yyyy-MM-dd HH:mm:ss") + "') 
	WHERE ((a.BatchCardDate >= @OperationStartDate) AND (a.BatchCardDate <= @OperationEndDate))
	AND a.BatchType IN ('T','RJ','2G') AND a.BatchWeight != 0 AND a.LineId = @LineId AND a.QCType IS NOT NULL 
	--AND ModuleId IN (1,2,19) AND SubModuleId IN (1,10,11,117,125)	
	AND ModuleId IN (1,2,19,22) 
	AND SubModuleId IN (1,10,11,117,125,139,141)
	--AND ((b.IsHotbox12Month = 0) OR (b.IsHotbox12Month IS NULL))

END

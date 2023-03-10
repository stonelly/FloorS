USE [FloorSystemUAT]
GO
/****** Object:  StoredProcedure [dbo].[OEE_SP_GetGloveQuantity_Hotbox12Month]    Script Date: 21/01/2022 17:19:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Azman Kasim>
-- Create date: <09/11/2021>
-- Description:	<OEE Hotbox>
-- =============================================
ALTER PROCEDURE [dbo].[OEE_SP_GetGloveQuantity_Hotbox12Month]
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
	b.QCTYPEBeforeHotbox
	FROM [dbo].[Batch] a with (nolock)
	LEFT JOIN dbo.OEE_HotboxGlove b (nolock) ON a.SerialNumber = b.SerialNumber
	WHERE ((b.HotboxVerifiedDate >= @OperationStartDate) AND (b.HotboxVerifiedDate <= @OperationEndDate))
	AND b.IsHotbox12Month = 1
	AND a.BatchType IN ('T','RJ','2G') 
	AND a.BatchWeight != 0 
	AND a.LineId = @LineId 
	AND a.QCType IS NOT NULL 
	--AND ModuleId IN (1,2,19) 
	--AND SubModuleId IN (1,10,11,117,125)
	AND ModuleId IN (1,2,19,22) 
	AND SubModuleId IN (1,10,11,117,125,139,141)
END

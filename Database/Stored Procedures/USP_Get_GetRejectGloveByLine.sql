
-- =============================================
-- Name:			USP_Get_GetRejectGloveByLine
-- Purpose: 		<To Get Glove types for regect Gloves based on line id>
-- =============================================
-- Change History
-- Date    Author   Comments
-- -----   ------   -----------------------------
-- 16/03/2016 	NarendraNath	   SP created.
-- 15/04/2022	Pang YS			   get active glovecode under batch table
-- =============================================

ALTER PROCEDURE [dbo].[USP_Get_GetRejectGloveByLine]
(
	@LineId Nvarchar(15)
)	
AS
BEGIN	
		IF @LineId='Used Glove'
			BEGIN
				--SELECT  DISTINCT GloveType FROM Batch WHERE   LEN(GloveType)>0 -- 329
				SELECT DISTINCT b.GloveType FROM VW_GloveCode (NOLOCK) a JOIN Batch b (NOLOCK) ON b.GloveType = a.GLOVECODE
				WHERE a.[STOPPED] = 0

			END
		ELSE
			BEGIN
				SELECT DISTINCT GloveType FROM VW_ProductionLine (NOLOCK) WHERE LineId=@LineId and  LEN(GloveType)>0
			END
END  
GO
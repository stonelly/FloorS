-- =======================================================  
  
-- Name:   USP_SEL_SizeByGloveTypeTumbling  
  
-- Purpose:   Gets Size by GloveType  
  
-- =======================================================  
  
-- Change History  
  
-- Date         Author     Comments  
  
-- -----        ------     -----------------------------  
  
-- 13/03/2015  Ruhi    SP created.  
-- 24/11/2017 MYAdamas SP altered get size by glovetypeid gloverrecid no longer in use  
  
-- Author: pang yik siu  
-- Created date: <19 Jan 2021>  
-- Description: eFS STD - Glove Setup - Glove Code use D365 FS tables: DOT_FSItemMaster, DOT_FSGloveCode  
--    from old tables: AX_AVAGLOVECODETABLE  
-- 11/04/2022 	Azrul	   Filter out inactive Glove Code.
-- =======================================================  
  
CREATE OR ALTER PROCEDURE [dbo].[USP_SEL_SizeByGloveTypeTumbling]  
  
 @gloveType   NVARCHAR(50)  
  
AS  
  
BEGIN  
IF ISNUMERIC(@GloveType) = 1  
 select a.CommonSize AS 'SizeName' from DOT_GLOVERELCOMSIZE a (NOLOCK)  
 join DOT_FSItemMaster b (NOLOCK) on a.ItemId = b.ItemId  
 join DOT_FSGloveCode c (NOLOCK) on c.ItemRecordId = b.id  
 where c.Barcode = @gloveType and c.IsDeleted=0
ELSE  
 select a.CommonSize AS 'SizeName' from DOT_GLOVERELCOMSIZE a (NOLOCK)  
 --join DOT_FSItemMaster b on a.recid = b.id  
 --join DOT_FSGloveCode c on c.ItemRecordId = b.id  
 where a.ItemId = @gloveType and a.IsDeleted=0  
  
/** original sp  
  SELECT COMMONSIZE AS 'SizeName' FROM AX_AVAGLOVERELCOMSIZE A   
  JOIN AX_AVAGLOVECODETABLE B ON A.GLOVEREFRECID = B.RECID   
  WHERE glovecode = @gloveType  
  **/  
END
GO


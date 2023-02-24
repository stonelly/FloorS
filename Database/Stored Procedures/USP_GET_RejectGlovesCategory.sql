-- =============================================  
-- Author: pang yik siu  
-- Created date: <19 Jan 2021>  
-- Description: eFS STD - Glove Setup - Glove Code use D365 FS tables: DOT_FSItemMaster, DOT_FSGloveCode  
--    from old tables: AX_AVAGLOVECODETABLE  
-- 31/12/2021  Azrul    SP merged from NGC.  
-- 11/04/2022  Azrul	Filter out inactive Glove Code.
-- =============================================  
CREATE OR ALTER PROCEDURE [dbo].[USP_GET_RejectGlovesCategory]  
(  
                @GloveType NVARCHAR(100)  
)  
AS  
BEGIN  
  
                IF ISNUMERIC(@GloveType) = 1  
						SELECT a.GloveCategory from DOT_FSGloveCode a (NOLOCK) join DOT_FSItemMaster b (NOLOCK) on a.ItemRecordId = b.Id where a.Barcode = @GloveType and a.IsDeleted=0
                ELSE  
						SELECT a.GloveCategory from DOT_FSGloveCode a (NOLOCK) join DOT_FSItemMaster b (NOLOCK) on a.ItemRecordId = b.Id where b.ItemId = @GloveType and a.IsDeleted=0  
  
/** orginal script  
                IF ISNUMERIC(@GloveType) = 1  
                                SELECT GloveCode FROM [dbo].[AX_AVAGLOVECODETABLE] WHERE Barcode = @GloveType   
                ELSE  
                                SELECT Glovecategory FROM [dbo].[AX_AVAGLOVECODETABLE] WHERE GloveCode = @GloveType   
        **/  
END  
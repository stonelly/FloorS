-- =============================================  
-- 04/04/2022  Max    New SP for HSB reject glove printing  
-- 11/04/2022  Azrul  Filter out inactive Glove Code.
-- =============================================  
CREATE OR ALTER PROCEDURE [dbo].[USP_GET_RejectGloveType]  
(  
                @GloveType NVARCHAR(100)  
)  
AS  
BEGIN  
  
    IF ISNUMERIC(@GloveType) = 1  
				SELECT b.ItemId from DOT_FSGloveCode a (NOLOCK) join DOT_FSItemMaster b (NOLOCK) on a.ItemRecordId = b.Id where a.Barcode = @GloveType and a.IsDeleted=0  
    ELSE  
                SELECT b.ItemId from DOT_FSGloveCode a (NOLOCK) join DOT_FSItemMaster b (NOLOCK) on a.ItemRecordId = b.Id where b.ItemId = @GloveType and a.IsDeleted=0  
  

END  
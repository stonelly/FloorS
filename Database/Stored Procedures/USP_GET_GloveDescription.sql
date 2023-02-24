-- =======================================================

-- Name:			[USP_GET_GloveDescription]

-- Purpose: 		Gets Desc by GloveType

-- =======================================================

-- Change History

-- Date         Author     Comments

-- -----        ------     -----------------------------

-- 28/05/2018 	Azrul	   SP altered.
-- 11/04/2022 	Azrul	   Filter out inactive Glove Code.
-- =======================================================

CREATE OR ALTER PROCEDURE [dbo].[USP_GET_GloveDescription]
(
                @GloveType NVARCHAR(100)
)
AS
BEGIN
        IF ISNUMERIC(@GloveType) = 1
            SELECT b.Name from DOT_FSGloveCode a (NOLOCK) join DOT_FSItemMaster b (NOLOCK) on a.ItemRecordId = b.Id where a.Barcode = @GloveType and a.IsDeleted=0
        ELSE
            SELECT a.GloveCategory from DOT_FSGloveCode a (NOLOCK) join DOT_FSItemMaster b (NOLOCK) on a.ItemRecordId = b.Id where b.ItemId = @GloveType and a.IsDeleted=0
END

GO
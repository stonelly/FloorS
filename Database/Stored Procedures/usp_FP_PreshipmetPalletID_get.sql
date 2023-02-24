  
-- =============================================  
-- Author:  Srikanth Balda  
-- Create date: 26 Aug 2014  
-- Description: List of availabel preshipment Palletid  
-- =============================================  
CREATE PROCEDURE [dbo].[usp_FP_PreshipmetPalletID_get]   
(  
 @locationid int  
)   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
-- Insert statements for procedure here  
select PalletId, IsPreshipment  from [dbo].[PalletMaster] where IsAvailable = 1 and  IsPreshipment = 1 and IsOccupied = 0   
and IsDeleted = 0 -- HSB SIT Issue# 90: Final packing screen > Preshipment pallet id allows to choose soft deleted pallets in database.
and   
(  
Zone is null  
or   
--LocationId=@locationid  
  
 Zone=(Select ZoneName from LocationMaster where locationid=@locationid)  
)  
  
END  
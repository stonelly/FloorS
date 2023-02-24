/****** Object:  StoredProcedure [dbo].[usp_GET_SurgicalPackingPlan_POList]    Script Date: 10/12/2020 12:58:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Azman Kasim>
-- Create date: <07/08/2020>
-- Description:	<Get PO List for eFS surgical packing plan>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GET_SurgicalPackingPlan_POList]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select DISTINCT CustomerRef + ' | ' + SalesId as [Value], SalesId as [Key] from VW_AXSOline (nolock)
	WHERE ITEMTYPE = '8' AND WorkOrderStatus = 2 -- for HSB version approve enum value is 2 

END
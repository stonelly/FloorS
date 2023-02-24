 --=========================================================    
 --Name:   USP_BalanceGloveQty_Get  
 --Purpose:   Get Balance Glove Quantity by Batch Order No   
 --=========================================================    
 --Change History    
 --Date    Author   Comments    
 -------   ------   ----------------------------------------    
 --21/05/2018  Azrul Amin    SP created.    
 -- exec USP_BalanceGloveQty_Get 'HSBBON000000028'
 --=========================================================    
alter PROCEDURE [dbo].[USP_BalanceGloveQty_Get]    
(   
 @BatchOrder varchar(150)    
)   
AS     
BEGIN   
 SET NOCOUNT ON;  

 SELECT a.*
 into #tempBO FROM
 ( SELECT bo.QtySched,
		ISNULL(hr.GloveReportedQty,0) + ISNULL(on2g.GloveReportedQty,0) + ISNULL(cg.GloveReportedQty,0) as GloveReportedQty
		FROM DOT_FloorD365BO AS bo WITH (NOLOCK) LEFT JOIN 

		--GloveReportedQty
		(SELECT b.BthOrder, ISNULL(SUM(b.PackingSz * b.InBox),0) as GloveReportedQty
		FROM DOT_FloorD365HRGLOVERPT b WITH (NOLOCK)
		GROUP BY b.BthOrder) as hr on hr.BthOrder = bo.BthOrderId LEFT JOIN
		
		--Online2GReportedQty
		(SELECT b.BatchOrder, ISNULL(SUM(b.PackingSize * b.InnerBox),0) as GloveReportedQty
		FROM DOT_FloorD365Online2G b WITH (NOLOCK)
		GROUP BY b.BatchOrder) as on2g on on2g.BatchOrder = bo.BthOrderId LEFT JOIN
		
		--ChengeGloveReportedQty
		(SELECT a.OldBatchOrder, ISNULL(SUM(b.PackingSz * b.InBox),0) as GloveReportedQty
		FROM ChangeGloveHistory a with (nolock) JOIN DOT_FloorD365HRGLOVERPT b with (nolock)
		on a.SerialNumber = b.SerialNo
		GROUP BY a.OldBatchOrder) as cg on cg.OldBatchOrder = bo.BthOrderId

		WHERE bo.ReworkBatch = 'No' and bo.IsDeleted = 0
		and bo.BthOrderId = @BatchOrder
	) a

	select CASE when (a.QtySched - a.GloveReportedQty) < 0 Then 0 Else a.QtySched - a.GloveReportedQty end AS RemainingQty from #tempBO a 
	drop table #tempBO
END
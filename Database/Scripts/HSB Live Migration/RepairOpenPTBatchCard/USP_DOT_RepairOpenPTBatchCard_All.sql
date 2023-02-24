
CREATE PROCEDURE [dbo].[USP_DOT_RepairOpenPTBatchCard_All]     
AS    
BEGIN
	DECLARE @openPTBatch AS TABLE 
			(
				ID INT IDENTITY(1,1), 
				SerialNumber VARCHAR(40)
			)
	INSERT INTO @openPTBatch (SerialNumber)
	SELECT SerialNumber FROM Batch WITH (NOLOCK)  
	WHERE SerialNumber IN  
	(  
	SELECT SerialNumber FROM QAI WITH (NOLOCK)  
	WHERE cast(SerialNumber as nvarchar(100)) in (  
	select BatchNumber from DOT_InventBatchSum with(nolock) where IsMigratedFromAX6=1 -- filter only open batch card  
	)  
	and SerialNumber IN  
	(  
	SELECT SerialNumber from QAI WITH (NOLOCK)  
	GROUP BY SerialNumber  
	HAVING count(SerialNumber) = '2'  
	)  
	AND QITestResult = 'FAIL'  
	)  
	AND QCBatchWeight IS NULL  
	AND QCTenPcsWeight IS NULL  
	AND PTBatchWeight IS NOT NULL  
	AND PTTenPcsWeight IS NOT NULL  
	ORDER BY SerialNumber DESC  

	DECLARE @ID INT = 1, @SerialNumber VARCHAR(40)

	WHILE @ID IS NOT NULL
	BEGIN
		SELECT @SerialNumber = SerialNumber
		FROM @openPTBatch 
		WHERE ID = @ID
   
		INSERT INTO @openPTBatch (SerialNumber)
		   EXEC USP_DOT_RepairOpenPTBatchCard @SerialNumber
   
		SELECT @ID = MIN(ID) 
		FROM @openPTBatch 
		WHERE ID > @ID; 
	END
END
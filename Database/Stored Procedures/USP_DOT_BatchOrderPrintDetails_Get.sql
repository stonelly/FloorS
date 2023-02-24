-- =========================================================================    
-- Name:   USP_DOT_BatchOrderPrintDetails_Get    
-- Purpose:   Get Glove Batch Order print details from master grid selection    
-- =========================================================================      
-- Change History      
-- Date    Author   Comments      
-- -----   ------   --------------------------------------------------------      
-- 26/06/2018  Azrul Amin    SP created.  
-- 22/10/2020  Azrul Amin    Include Surgical logic.  
-- 02/02/2021  Azrul Amin    Include ON2G.  
-- 27/12/2021  Azrul Amin    HTLG_HSB_002:Special Glove (Clean Room Product) 
-- =========================================================================      
CREATE PROCEDURE [dbo].[USP_DOT_BatchOrderPrintDetails_Get]      
(     
 @BatchOrderNo varchar(150),    
 @PlantNo varchar(10)  
)     
AS      
BEGIN       
 SET NOCOUNT ON;   

 SELECT SerialNo AS SerialNumber, BthOrder AS BatchNumber, Resource, PackingSz AS PackingSize, InBox AS InnerBox,   
 PackingSz*InBox AS ReportedQty, PackingSz*InBox AS TotalQty, OutTime AS OutputTime, 'Reprint' AS ReprintHBC   
 FROM DOT_FloorD365HRGLOVERPT with (nolock)   
 WHERE BthOrder = @BatchOrderNo AND Plant = @PlantNo   

 UNION ALL  

 SELECT FORMAT(SerialNumber,'0000000000') AS SerialNumber, BatchOrder as BatchNumber, Resource, PackingSize, InnerBox,   
 PackingSize*InnerBox AS ReportedQty, PackingSize*InnerBox AS TotalQty, CurrentDateandTime AS OutputTime, 'Reprint' AS ReprintHBC   
 FROM DOT_FloorD365Online2G with (nolock)   
 WHERE BatchOrder = @BatchOrderNo AND Plant = @PlantNo   

 UNION ALL

 select a.SerialNumber, a.OldBatchOrder AS BatchNumber, b.Resource,b. PackingSz AS PackingSize, b.InBox AS InnerBox,   
 b.PackingSz*b.InBox AS ReportedQty, b.PackingSz*b.InBox AS TotalQty, b.OutTime AS OutputTime, 'Changed Glove' AS ReprintHBC   
 from ChangeGloveHistory a with (nolock) JOIN DOT_FloorD365HRGLOVERPT b with (nolock)
 on a.SerialNumber = b.SerialNo
 WHERE a.OldBatchOrder = @BatchOrderNo AND Plant = @PlantNo     

 ORDER BY OutputTime DESC    

 SET NOCOUNT OFF;      
END  
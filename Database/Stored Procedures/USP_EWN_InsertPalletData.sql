-- =============================================  
-- Name:   [USP_EWN_InsertPalletData]  
-- Purpose:   Insert data to the eWareNavi table  
-- =============================================  
-- Change History  
-- Date   Author    Comments  
-- -----  ------    -----------------------------  
-- 24/01/2017  Cheah Teik Chuan SP created.  
-- 23/09/2020 Chong Kah HOe  Changed SP to match ewarenavi special character limitation  
-- 10/02/2022 Azrul  New column FGCodeAndSize (HartalegaCommonSize) for IOT scan, 
--					 original Item (CustomerSize) field for EWN and lifter scan.
-- =============================================  
  
alter PROCEDURE [dbo].[USP_EWN_InsertPalletData]  
(   
 @QAPassed   int,  
 @Item    varchar(20),  
 @PONumber   varchar(20),  
 @Qty    int,  
 @PalletID   varchar(8),  
 @DateComplete  datetime,  
 @DateStockOut  datetime,  
 @FGCodeAndSize   varchar(20)  
)   
AS  
BEGIN   
 SET @Item = Replace(@Item, ' 1/2', '½')   
  
 Insert into EWN_CompletedPallet(QAPassed,Item,PONumber,Qty,PalletId,DateCompleted,DateStockOut,FGCodeAndSize)  
 values (@QAPassed, @Item,@PONumber,@Qty,@PalletID,@DateComplete,@DateStockOut,@FGCodeAndSize)  
  
END  
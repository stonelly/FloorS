-- =======================================================  
-- Change History  
-- Date         Author   Comments  
-- -----        ------   -----------------------------  
-- 04/09/2014   NarendraNath    SP created.  
-- 09/09/2021   Azrul   Open batch flag for NGC1.5.  
-- =======================================================  
CREATE PROCEDURE [dbo].[USP_SAVE_AXPOSTINGLOG]  
(  
 @ServiceName nvarchar(50) ,  
 @PostingType nvarchar(20) ,  
 @PostedDate datetime ,  
 @BatchNumber nvarchar(30) ,  
 @SerialNumber numeric(15, 0) ,  
 @IsPostedToAX bit ,  
 @IsPostedInAX bit ,  
 @Sequence int ,  
 @ExceptionCode nvarchar(1000),  
 @TransactionID nvarchar(100),       
 @Area NVARCHAR(10),  
 @IsConsolidated bit, -- Open batch flag for NGC1.5  
 @PlantNo nvarchar(20) = '' -- Open batch flag for NGC1.5  
)  
AS  
BEGIN  
INSERT INTO dbo.AXPostingLog  
           (ServiceName,PostingType,PostedDate,BatchNumber,SerialNumber,IsPostedToAX,IsPostedInAX,Sequence,ExceptionCode,TransactionId,Area,CreationDate,IsConsolidated)  
     VALUES(@ServiceName,@PostingType,@PostedDate,@BatchNumber,@SerialNumber,@IsPostedToAX,@IsPostedInAX,@Sequence,@ExceptionCode,@TransactionID,@Area,getdate(),
			dbo.Ufn_DOT_GET_IsConsolidated(@SerialNumber,@PlantNo));-- -- replace by function @ISCONSOLIDATED)
END 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[USP_SAVE_AXPOSTINGLOG] TO [FSDB]
    AS [dbo];

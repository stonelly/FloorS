-- =============================================    
-- Author:  Max He  
-- Create date: <28-July-2021>    
-- Description: Get All FG Staging with Detail based on Parent Record ids   
-- exec USP_DOT_GetFGStagingWithDetail '8018001,8018036','SBC'   --split SBC SP: HBC > SBC (dev) (PN-PN)  
-- exec USP_DOT_GetFGStagingWithDetail '9302727,9302728,9302729', 'SBC' --split SBC QC:  HBC > RWKCR > SOBC > SBC > SBC (dev) (PN-QC-QC)  
--                  --split SBC SP & QC: HBC > SBC > RWKCR > SOBC > SBC  
--                  --split SBC SP & Temp pack: HBC > SBC > STPI > STPO > SBC  
--                  --split SBC QC & Temp pack: HBC > RWKCR > SOBC > SBC > STPI > STPO > SBC   
-- exec USP_DOT_GetFGStagingWithDetail '7910161,7910162','SBC'  --(uat2)  
-- exec USP_DOT_GetFGStagingWithDetail '7910006','SPPBC' --(uat2)  
-- exec USP_DOT_GetFGStagingWithDetail '7910044','SPPBC' --(uat2)  
-- exec USP_DOT_GetFGStagingWithDetail '7910167','SBC' --(uat2) split consolidate (QC)  
-- exec USP_DOT_GetFGStagingWithDetail '7910168','SBC' --(uat2) split consolidate (QC)  
-- exec USP_DOT_GetFGStagingWithDetail '8018834,8021101,8024995,8077607,8083607','SMBP' --online batch (dev)  
-- exec USP_DOT_GetFGStagingWithDetail '8060641,8064617,8068527,8068730,8070057,8076762,8080534','SMBP' --online batch (dev)  
-- exec USP_DOT_GetFGStagingWithDetail '8346930,8347282','SMBP' --offline batch (dev)  
-- =============================================    
alter PROCEDURE [dbo].[USP_DOT_GetFGStagingWithDetail]    
@ParentIds varchar(max),  
@FunctionIdentifier varchar(20)  
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
   
DECLARE @sql varchar(max);    
  
IF (@FunctionIdentifier = 'SBC' or @FunctionIdentifier = 'MTS')  
BEGIN  
 SET @sql = '  
 select a.* into #tempfg  from (    
 select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQty, a.PlantNo, a.Sequence as SequenceNo, a.BatchNumber as SerialNo,      
 CAST(d.BaseQuantity as DECIMAL) as BaseQuantity,CAST(b.Quantity as DECIMAL) as Qty, CAST(b.Quantity * d.BaseQuantity as DECIMAL) as GloveQty,b.PostingDateTime,    
 b.ReferenceItemNumber as ItemNo, b.InnerLotNumber as LotNo, b.CustomerReference as CustRef, CAST(a.PreshipmentCases as DECIMAL) as PreshipmentCases,   
 a.FunctionIdentifier, d.Id as FGSumId, a.D365BatchNumber as ParentD365BatchNumber, e.D365BatchNumber as DetailsD365BatchNumber  
 from DOT_FloorAxIntParentTable a with(nolock)    
 join DOT_FGJournalTable b with(nolock) on a.id = b.ParentRefRecId    
 join DOT_FGSumTable d with(nolock) on d.SalesOrderNumber = b.SalesOrderNumber   
 join DOT_FGSumTableDetails e on d.id = e.ParentRefRecId  
 AND d.ItemNumber = b.ItemNumber   
 AND d.ItemSize = b.Configuration    
 and d.D365BatchNumber = a.D365BatchNumber  
 and d.FunctionIdentifier = a.FunctionIdentifier  
 where a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 and a.Id in (' + @ParentIds + ')  
    and d.DetailStagingJSON = ''' + @ParentIds + ''' -- SBC D365 BatchNumber may not unique enough   
 ) a  
  
 select aa.*, ISNULL(Max(b.Location), Max(c.Location)) as Location, aa.DetailsD365BatchNumber as D365BatchNumber   
 from #tempfg aa with (NOLOCK)  
 join DOT_FloorAxIntParentTable a with (NOLOCK) on a.BatchNumber = aa.SerialNo  
 left join DOT_RafStgTable b with (nolock) on b.ParentRefRecId = a.id  
 left join DOT_TransferJournal c with (nolock) on c.ParentRefRecId = a.id  
 where a.BatchNumber = aa.SerialNo and a.Sequence < aa.SequenceNo  
 and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0  
 and aa.DetailsD365BatchNumber = a.D365BatchNumber  
 and b.Location is not null or c.Location is not null  
 group by aa.ParentId, aa.BatchOrderNumber, aa.Config, aa.SampleQty, aa.PlantNo, aa.SequenceNo, aa.SerialNo,   
 aa.BaseQuantity, aa.Qty, aa.GloveQty, aa.PostingDateTime, aa.ItemNo, aa.LotNo, aa.CustRef, aa.PreshipmentCases,   
 aa.FunctionIdentifier, aa.FGSumId, a.D365BatchNumber, aa.DetailsD365BatchNumber, aa.ParentD365BatchNumber   
 '   
END  
  
IF (@FunctionIdentifier = 'SPPBC')  
BEGIN  
 SET @sql = '  
 select a.id as FGSumId, e.D365BatchNumber, d.BatchNumber as SerialNo, d.GloveSize as Config, ''QC'' as Location,   
 d.PickingListQuantity as GloveQty, d.GloveSampleQuantity as SampleQty  
 from DOT_FloorAxIntParentTable c join DOT_FGJournalTable d on c.id = d.ParentRefRecId  
 join DOT_FloorAxIntParentTable e on e.BatchNumber = d.BatchNumber  
 join DOT_FGSumTable a on a.DetailStagingJSON = ''' + @ParentIds + '''  
 where c.id in (' + @ParentIds + ') and e.FunctionIdentifier = ''SRBC'' and e.Sequence=1  
 and a.FunctionIdentifier = ''SPPBC''  
 '  
END  
  
IF (@FunctionIdentifier = 'SMBP')  
BEGIN  
 SET @sql = '    
 select a.* into #tempfg  from (   
 Select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQty, a.PlantNo, b.PostingDateTime, a.FunctionIdentifier,   
 b.InnerLotNumber as LotNo, b.CustomerReference as CustRef, a.PreshipmentCases, b.SalesOrderNumber, b.Quantity as Qty,  
 a.ReferencebatchNumber1 as SerialNo, a.ReferenceBatchSequence1 as SequenceNo, b.RefNumberOfPieces1 as GloveQty, b.RefItemNumber1 as ItemNo  
 from DOT_FloorAxIntParentTable a join DOT_FGJournalTable b  
 join DOT_FGSumTable d with(nolock) on d.SalesOrderNumber = b.SalesOrderNumber  
 on a.id = b.ParentRefRecId  
 where a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 and a.id in (' + @ParentIds + ')  
  
 UNION ALL  
  
 Select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQty, a.PlantNo, b.PostingDateTime, a.FunctionIdentifier,   
 b.InnerLotNumber as LotNo, b.CustomerReference as CustRef, a.PreshipmentCases, b.SalesOrderNumber, b.Quantity as Qty,  
 a.ReferencebatchNumber2 as SerialNo, a.ReferenceBatchSequence2 as SequenceNo, b.RefNumberOfPieces2 as GloveQty, b.RefItemNumber2 as ItemNo  
 from DOT_FloorAxIntParentTable a join DOT_FGJournalTable b  
 on a.id = b.ParentRefRecId  
 where a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 and a.id in (' + @ParentIds + ')  
 and a.ReferenceBatchNumber2 is not null  
  
 UNION ALL  
  
 Select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQty, a.PlantNo, b.PostingDateTime, a.FunctionIdentifier,   
 b.InnerLotNumber as LotNo, b.CustomerReference as CustRef, a.PreshipmentCases, b.SalesOrderNumber, b.Quantity as Qty,   
 a.ReferencebatchNumber3 as SerialNo, a.ReferenceBatchSequence3 as SequenceNo, b.RefNumberOfPieces3 as GloveQty, b.RefItemNumber3 as ItemNo  
 from DOT_FloorAxIntParentTable a join DOT_FGJournalTable b  
 on a.id = b.ParentRefRecId  
 where a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 and a.id in (' + @ParentIds + ')  
 and a.ReferenceBatchNumber3 is not null  
  
 UNION ALL  
  
 Select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQty, a.PlantNo, b.PostingDateTime, a.FunctionIdentifier,   
 b.InnerLotNumber as LotNo, b.CustomerReference as CustRef, a.PreshipmentCases, b.SalesOrderNumber, b.Quantity as Qty,   
 a.ReferencebatchNumber4 as SerialNo, a.ReferenceBatchSequence4 as SequenceNo, b.RefNumberOfPieces4 as GloveQty, b.RefItemNumber4 as ItemNo  
 from DOT_FloorAxIntParentTable a join DOT_FGJournalTable b  
 on a.id = b.ParentRefRecId  
 where a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 and a.id in (' + @ParentIds + ')  
 and a.ReferenceBatchNumber4 is not null  
  
 UNION ALL  
  
 Select a.Id as ParentId,b.BatchOrderNumber, b.Configuration as Config, 0.00 as SampleQty, a.PlantNo, b.PostingDateTime, a.FunctionIdentifier,   
 b.InnerLotNumber as LotNo, b.CustomerReference as CustRef, a.PreshipmentCases, b.SalesOrderNumber, b.Quantity as Qty,   
 a.ReferencebatchNumber5 as SerialNo, a.ReferenceBatchSequence5 as SequenceNo, b.RefNumberOfPieces5 as GloveQty, b.RefItemNumber5 as ItemNo  
 from DOT_FloorAxIntParentTable a join DOT_FGJournalTable b  
 on a.id = b.ParentRefRecId  
 where a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0 and a.id in (' + @ParentIds + ')  
 and a.ReferenceBatchNumber5 is not null  
 ) a  
  
 select a.ParentId, a.BatchOrderNumber, a.SampleQty, a.PlantNo, a.SequenceNo, a.SerialNo, a.Config,    
 CAST(a.Qty as DECIMAL) as Qty, CAST(a.GloveQty as DECIMAL) as GloveQty,a.PostingDateTime,    
 a.ItemNo, a.LotNo, a.CustRef, CAST(a.PreshipmentCases as DECIMAL) as PreshipmentCases,   
 a.FunctionIdentifier, d.Id as FGSumId ,CAST(d.BaseQuantity as DECIMAL) as BaseQuantity   
 into #tempSMBP from #tempfg a  
 join DOT_FGSumTable d with(nolock) on d.SalesOrderNumber = a.SalesOrderNumber   
 where d.DetailStagingJSON = ''' + @ParentIds + '''  
  
 select aa.*, ISNULL(Max(b.Location), Max(c.Location)) as Location, a.D365BatchNumber   
 from #tempSMBP aa with (NOLOCK)  
 join DOT_FloorAxIntParentTable a with (NOLOCK) on a.BatchNumber = aa.SerialNo  
 left join DOT_RafStgTable b with (nolock) on b.ParentRefRecId = a.id  
 left join DOT_TransferJournal c with (nolock) on c.ParentRefRecId = a.id  
 where a.BatchNumber = aa.SerialNo and a.Sequence < aa.SequenceNo  
 and a.IsDeleted = 0 and a.IsMigratedFromAX6 = 0  
 and b.Location is not null or c.Location is not null  
 group by aa.ParentId, aa.BatchOrderNumber, aa.Config, aa.SampleQty, aa.PlantNo, aa.SequenceNo, aa.SerialNo,   
 aa.BaseQuantity, aa.Qty, aa.GloveQty, aa.PostingDateTime, aa.ItemNo, aa.LotNo, aa.CustRef, aa.PreshipmentCases,   
 aa.FunctionIdentifier, aa.FGSumId, a.D365BatchNumber   
 '   
END  
 EXEC (@sql)    
END  
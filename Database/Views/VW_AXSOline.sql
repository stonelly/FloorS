-- =================================================================          
-- Author:  <Azrul Amin>          
-- Modify date: <27-Feb-2018>          
-- Description: <View for Sales Order Integrate to Floor System>         
        
-- Author:  <Pang Yik Siu>          
-- Modify date: <14-Mar-2020>          
-- Description: <Add Customer Size>          
         
-- =================================================================          
alter VIEW [dbo].[VW_AXSOline]    
AS         
SELECT         
SL.INVENTTRANSID,        
im.Name as ItemName,        
im.ITEMTYPE,        
SL.Salesid ,ST.PurchOrderFormNum,ST.CustomerRef ,ST.ShippingDateConfirmed,        
im.ItemId,        
SL.Name,         
SL.configuration as HartalegaCommonSize, --bl.HartalegaCommonSize,     
SL.CustomerSize as CustomerSize,        
isnull(SL.NetWeight,0) as NetWeight,         
isnull(SL.GrossWeight,0) as GrossWeight,        
SL.GlovesInnerboxNo as NumberGlovesInnerbox,        
SL.InnerboxinCaseNo as NumberInnerBoxInOuter,        
SL.InnerLabelSet,         
SL.OuterLabelSetNo as OuterLabelSet,        
SL.PreshipmentPlan as PreshipmentSamplingPlan,         
SL.PalletCapacity,        
SL.GLOVECODE,        
SL.AlternateGloveCode1,         
SL.AlternateGloveCode2,         
SL.AlternateGloveCode3,        
SL.LINENUM,        
SL.CONFIGURATION,        
SL.CONFIGURATIONNAME,         
SL.ManufacturingDateOn as ManufacturingDateBasis,        
SL.LotVerification as InnerVerification,        
SL.GCLabel  as OuterVerification,        
SL.DOTCUSTOMERLOTID as CUSTOMERLOTID,        
SL.InnerProductCode,        
SL.InnerDateFormat,        
SL.OuterProductCode,        
SL.OuterDateFormat,        
SL.Expiry,        
SL.SpecialInnerCode,        
SL.SpecialInnerCharacter as SpecialInnerCodeCharacter,        
SL.Reference1,         
SL.Reference2,        
SL.ReceiptDateRequested,        
--ST.DOTManufacturingETD as ShippingDateRequested,-- standard        
ST.DOTCustMfgDate as ShippingDateRequested,-- standard        
SL.SalesQty,-- standard        
ST.SalesStatus,-- standard        
ST.SalesName,-- standard        
ST.DocumentStatus, -- standard        
ST.ShippingDateConfirmed as STShippingDateConfirmed,-- standard        
SL.DOTBaseQty as AvaBaseQty,        
ST.DOTCustMfgDate, -- standard        
ST.DOTCustExpDate -- standard        
-- Max he, can not join batch order table because duplicate record show on PO list        
--,bo.BthOrderId as BatchOrder        
,'StartedUp' as ProdStatus -- facked just for fix UI        
,SL.PrintingSize -- for outer label print, 26 Nov 2018 Max He         
,ST.DOTPODate as HSB_CustPODocumentDate --CR-010: PO date and PO received date in SO header        
,ST.DOTPOReceivedDate as HSB_CustPORecvDate --CR-010: PO date and PO received date in SO header        
,ST.WorkflowStatus as WorkOrderStatus -- P7 SurgicalPackingPlan SP used field      
,SL.InnerDateFormat as INNERLABELSETDATEFORMAT -- P7 SurgicalPackingPlan SP used field      
,SL.OuterDateFormat as OUTERLABELSETDATEFORMAT -- P7 SurgicalPackingPlan SP used field      
,ST.DOTManufacturingETD as ManufacturingDateETD -- P7 SurgicalPackingPlan SP used field      
,SL.BARCODE-- P7 SurgicalPackingPlan SP used field      
,SL.BARCODEOUTERBOX -- P7 SurgicalPackingPlan SP used field      
from DOT_FloorSalesLine SL        
Join  DOT_FloorSales ST on SL.SALESID =  ST.SALESID         
join DOT_FSItemMaster im on im.ItemId = SL.ItemId         
--join DOT_FSBrandHeaders bh on bh.ItemId = SL.ItemId        
--join VW_AX_AVABRANDLINE_SALESLINE bl on bl.ItemId = bh.ItemId and bl.CUSTOMERSIZE = SL.CustomerSize       
where exists        
 (select 1 from DOT_FloorD365BO bo with(nolock)        
  where bo.BatchId = st.CustomerRef and bo.Size = case when im.ItemType = 8 then sl.CustomerSize else SL.CONFIGURATION end         
  and bo.ReworkBatch<>'Yes' and bo.ProdStatus='StartedUp')        
 -- Max He, only show sales order that have StartedUp batch order and not rework order        
and  im.ITEMTYPE in (5,6,8,9) -- <> 'GLove'        
and  ST.SALESSTATUS = 1 -- Open Order'Backorder'        
and  ST.DocumentStatus >= 3        
and  ST.WorkflowStatus = 2 -- for HSB version approve enum value is 2    
and  sl.IsDeleted=0    
/****** start original view ******/    
--select FSL.INVENTTRANSID,    
--FSL.ITEMNAME,    
--FSL.ITEMTYPE,    
--SL.Salesid ,SL.PurchOrderFormNum,ST.CustomerRef ,SL.ShippingDateConfirmed,    
--FSL.ItemId,SL.Name, FSL.HartalegaCommonSize,     
--(isnull(FSL.NETWEIGHT,0)/1000) as NETWEIGHT,     
--(isnull(FSL.GROSSWEIGHT,0)/1000) as GROSSWEIGHT,    
--FSL.NUMBERGLOVESINNERBOX,    
--FSL.NUMBERINNERBOXINOUTER,    
--FSL.INNERLABELSET, FSL.OUTERLABELSET,    
--FSL.PRESHIPMENTSAMPLINGPLAN, FSL.PALLETCAPACITY,    
--FSL.GLOVECODE, FSL.ALTERNATEGLOVECODE1, FSL.ALTERNATEGLOVECODE2, FSL.ALTERNATEGLOVECODE3,FSL.LINENUM,    
--FSL.CONFIGURATION , FSL.CONFIGURATIONNAME, FSL.MANUFACTURINGDATEBASIS, FSL.INNERVERIFICATION,FSL.OUTERVERIFICATION,    
--FSL.CUSTOMERLOTID,FSL.INNERPRODUCTCODE, FSL.OUTERPRODUCTCODE,FSL.EXPIRY,FSL.SPECIALINNERCODE,    
--FSL.SPECIALINNERCODECHARACTER,FSL.REFERENCE1, FSL.REFERENCE2,    
--SL.RECEIPTDATEREQUESTED,ST.AVAManufacturingETD as SHIPPINGDATEREQUESTED,SL.SalesQty,    
--ST.SALESSTATUS, ST.SalesName,ST.DocumentStatus , ST.ShippingDateConfirmed as STShippingDateConfirmed,    
--SL.AvaBaseQty    
--from MicrosoftDynamicsAX.dbo.SalesLine SL    
--Join MicrosoftDynamicsAX.dbo.AVAFLOORSALESLINE FSL on FSL.INVENTTRANSID = SL.INVENTTRANSID    
--Join  MicrosoftDynamicsAX.dbo.SalesTable ST on SL.SALESID =  ST.SALESID    
--where FSL.ITEMTYPE in (5,6,8,9)    
--and ST.SALESSTATUS = 1     
--and ST.DocumentStatus >= 3  and ST.DataAreaID ='hngc'  and SL.DataAreaID='hngc' and FSL.DataAreaID='hngc'    
/****** end original view ******/  
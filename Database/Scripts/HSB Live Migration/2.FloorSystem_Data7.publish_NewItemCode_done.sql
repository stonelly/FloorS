
update ep set DateScanned='2022-04-01', FGCodeAndSize=item
from EWN_CompletedPallet ep
go


--PurchaseOrderItemCases
update po set po.ItemNumber = fgm.newFGCode
from PurchaseOrderItemCases po
join FGCodeMapping$ fgm on fgm.OldFGCode = po.ItemNumber
where PONumber in (
 select SalesId from OpenSO$
)
and fgm.newFGCode <> po.ItemNumber
go

--FinalPacking
update fp set fp.ItemNumber = fgm.newFGCode
from finalpacking fp
join FGCodeMapping$ fgm on fgm.OldFGCode = fp.ItemNumber
where PONumber in (
 select SalesId from OpenSO$
)
and fgm.newFGCode <> fp.ItemNumber
go

--SurgicalPackingPlan
update spp set spp.ItemNumber = fgm.newFGCode
from [SurgicalPackingPlan] spp
join FGCodeMapping$ fgm on fgm.OldFGCode = spp.ItemNumber
where spp.PONumber in (
 select SalesId from OpenSO$)
 go

 --EWN_CompletedPallet
 update ep set item=replace(item,m.oldFGCode,m.NewFGCode)
from EWN_CompletedPallet ep with(nolock)
join [FGCodeMapping$] m with(nolock) on item like '%'+m.oldFGCode+'%' and m.newFGCode<>m.oldFGCode
where ep.PONumber in (
select SalesId from OpenSO$
)
go

--select * from PurchaseOrderItem
update a set a.Itemnumber = b.NewFGCode
from PurchaseOrderItem a join FGCodeMapping$ b on a.ItemNumber = b.OldFGCode
where a.PONumber in (
select SalesId from OpenSO$
)
and a.Itemnumber <> b.NewFGCode
go

--PurchaseOrderItem
SET ANSI_WARNINGS OFF
update a set a.ItemName = b.Name
from PurchaseOrderItem a join DOT_FSItemMaster b on a.ItemNumber = b.ItemId 
where a.PONumber in (
select SalesId from OpenSO$
)
and a.ItemName <> b.Name
SET ANSI_WARNINGS ON
go


--Batch
update a set a.GloveType = b.ItemNumber
from batch a with (nolock) join dot_inventbatchsum b on b.batchnumber = cast(a.serialnumber as nvarchar(20)) and IsMigratedFromAX6 = 1
go

--DryerMaster
update a set a.GloveType = b.ItemNumber 
from DryerMaster a with (nolock) join dot_inventbatchsum b on b.Searchname = a.GloveType and a.GloveSize = b.ItemSize
go


--WasherMaster
update a set a.GloveType = b.ItemNumber 
from WasherMaster a with (nolock) join dot_inventbatchsum b on b.Searchname = a.GloveType and a.GloveSize = b.ItemSize
go


--FP_SerialNo_Exemption
update a set a.GloveDesc = b.ItemNumber
from FP_SerialNo_Exemption a with (nolock) join dot_inventbatchsum b on b.batchnumber = cast(a.serialnumber as nvarchar(20)) and IsMigratedFromAX6 = 1
go

--FP_SerialNo_ExemptionForProductionDate
update a set a.GloveDesc = b.ItemNumber
from FP_SerialNo_ExemptionForProductionDate a with (nolock) join dot_inventbatchsum b on b.batchnumber = cast(a.serialnumber as nvarchar(20)) and IsMigratedFromAX6 = 1
go

--SurgicalPackingPlan
update spp set spp.GloveCode = dd.ItemNumber
from SurgicalPackingPlan spp
join dot_inventbatchsum dd on spp.GloveCode = dd.Searchname
where spp.PONumber in (select SalesId from OpenSO$)
and spp.GloveCode <> dd.ItemNumber
go


--SurgicalPackingPlanDetails
update sppd set sppd.GloveCode = dd.ItemNumber
from SurgicalPackingPlan spp
join SurgicalPackingPlanDetails sppd on spp.SurgicalPackingPlanId=sppd.SurgicalPackingPlanId
join dot_inventbatchsum dd on cast(sppd.serialnumber as nvarchar(20)) = dd.batchnumber
where spp.PONumber in (select SalesId from OpenSO$)
go


--PurchaseOrderItem
update a set a.AlternateGloveCode1 = b.itemnumber
from PurchaseOrderItem a join dot_inventbatchsum b on b.Searchname = a.AlternateGloveCode1 where PONumber in (
select SalesId from OpenSO$)
and isnull(a.AlternateGloveCode1,'') <> ''
go
update a set a.AlternateGloveCode2 = b.itemnumber
from PurchaseOrderItem a join dot_inventbatchsum b on b.Searchname = a.AlternateGloveCode2 where PONumber in (
select SalesId from OpenSO$)
and isnull(a.AlternateGloveCode2,'') <> ''
go
update a set a.AlternateGloveCode3 = b.itemnumber
from PurchaseOrderItem a join dot_inventbatchsum b on b.Searchname = a.AlternateGloveCode3 where PONumber in (
select SalesId from OpenSO$)
and isnull(a.AlternateGloveCode3,'') <> ''
go
update a set a.GloveCode = b.itemnumber
from PurchaseOrderItem a join dot_inventbatchsum b on b.Searchname = a.AlternateGloveCode3 where PONumber in (
select SalesId from OpenSO$)
and a.GloveCode <> b.itemnumber
go


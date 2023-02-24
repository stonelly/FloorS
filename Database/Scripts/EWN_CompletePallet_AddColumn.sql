
Alter table EWN_CompletedPallet
add FGCodeAndSize varchar(20)
go

--patch not scan pallet
--update EWN_CompletedPallet set FGCodeAndSize = Item where DateScanned is null


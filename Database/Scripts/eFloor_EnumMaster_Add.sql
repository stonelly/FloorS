
if not exists (select * from EnumMaster where enumtype  = 'RouteCategory' and EnumValue = 'PT')
begin
	insert into EnumMaster
	select 251,'RouteCategory',	'PT',	'PT',	0,	NULL,	NULL,	NULL,	NULL
end
if not exists (select * from EnumMaster where enumtype  = 'RouteCategory' and EnumValue = 'OQC')
begin
	insert into EnumMaster
	select 252,'RouteCategory',	'OQC',	'OQC',	0,	NULL,	NULL,	NULL,	NULL
end
use [FLChat]
go

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Usr].[User]')
	and [name] = 'ForeignId')
alter table [Usr].[User]
add [ForeignId] nvarchar(100) NULL
go

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Usr].[User]')
	and [name] = 'ForeignOwnerId')
alter table [Usr].[User]
add [ForeignOwnerId] nvarchar(100) NULL
go

if not exists(select 1 from sysindexes where [name] = 'UNQ__UsrUser__ForeignId')
create unique index [UNQ__UsrUser__ForeignId]
	on [Usr].[User] ([ForeignId])
	where ([ForeignId] is not null)
go
use [FLChat]
go

alter table [Cfg].[TransportType]
add [InnerTransport] bit NOT NULL default 0
go

update [Cfg].[TransportType] set [InnerTransport] = 1 where [Id] in (0, 100)
go
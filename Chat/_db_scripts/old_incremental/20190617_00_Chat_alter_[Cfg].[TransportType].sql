use [FLChat]
go

alter table [Cfg].[TransportType]
add [VisibleForUser] bit default 1 NOT NULL
go

alter table [Cfg].[TransportType]
add [CanSelectAsDefault] bit default 1 NOT NULL
go

alter table [Cfg].[TransportType]
add [Prior] tinyint default 10 NOT NULL
go

update [Cfg].[TransportType] set [Prior] = 255 where [Id] = 0;
go

insert into [Cfg].[TransportType] 
([Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior])
values
(100, 'WebChat', 1, 0, 1, 0)
go

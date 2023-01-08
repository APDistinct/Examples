use [FLChat]
go

alter table [Cfg].[MessageType]
add [ShowInHistory] bit NOT NULL default 1
go

insert into [Cfg].[MessageType] 
([Id], [Name], [ShowInHistory])
values
(4, 'Mailing', 0)
go

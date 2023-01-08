use [FLChat]
go

insert into [Cfg].[TransportType] 
([Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior])
values
(151, 'Email', 1, 0, 0, 0)
go

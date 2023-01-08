use [FLChat]
go

alter table [Cfg].[TransportType]
add [DeepLink] nvarchar(255) NULL
go

update [Cfg].[TransportType] 
set [DeepLink] = 'https://telegram.me/TeleFLBot?start=%code%'
where [Id] = /**Telegram**/1
go
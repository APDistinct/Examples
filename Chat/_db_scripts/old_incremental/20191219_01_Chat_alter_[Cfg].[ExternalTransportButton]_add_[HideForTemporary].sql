use [FLChat]
go

alter table [Cfg].[ExternalTransportButton]
add [HideForTemporary] bit NOT NULL default 0
go

update [Cfg].[ExternalTransportButton]
set [HideForTemporary] = 1
where [Command] in (N'cmd:score', N'cmd:profile', N'cmd:select_addressee')
go
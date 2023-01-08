use [ViChat]
go

insert into [Cfg].[ExternalTransportButton] ([Row], [Col], [HideForTemporary], [Caption], [Command])
values
 (0, 0, 1, N'Мои баллы', N'cmd:score')
,(0, 1, 1, N'Мой профиль', N'cmd:profile')

select * from [Cfg].[ExternalTransportButton]

GO
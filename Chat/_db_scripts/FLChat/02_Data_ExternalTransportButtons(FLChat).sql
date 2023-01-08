use [FLChat]
go

insert into [Cfg].[ExternalTransportButton] ([Row], [Col], [HideForTemporary], [Caption], [Command])
values
 (0, 0, 1, N'Выбрать адресата', N'cmd:select_addressee')
,(1, 0, 1, N'Мои баллы', N'cmd:score')
,(1, 1, 1, N'Мой профиль', N'cmd:profile')
,(2, 0, 0, N'Акции', N'url:https://new.faberlic.com/ru/c/1?q=%3Arelevance%3AperiodShields%3Apromo')
,(2, 1, 0, N'Новый каталог', N'url:https://new.faberlic.com/ru/c/1?q=%3Arelevance%3AperiodShields%3Anew')
,(3, 0, 0, N'Как заказать', N'url:https://new.faberlic.com/ru/howtoaddproducttocart')
,(3, 1, 0, N'Где получить', N'url:https://new.faberlic.com/ru/delivery')

select * from [Cfg].[ExternalTransportButton]

GO
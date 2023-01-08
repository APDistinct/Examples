use [FLChat]
go

update [Cfg].[Settings]  set [Value]= 'http://chat.vicado.ai/demo/%code%'
where [Name] = N'LITE_LINK_DEEP_URL'

go

update [Cfg].[Settings]  set [Value]= 'http://chat.vicado.ai/demo/%code%'
where [Name] = N'INVITE_LINK'

go

update [Cfg].[Settings]  set [Value]= 'http://chat.vicado.ai/demo/%code%'
where [Name] = N'WEB_CHAT_DEEP_URL'

go
use [FLChat]
go

update [Cfg].[Settings]  set [Value]= '%sender_name% invites you to Vicado: %url%'
where [Name] = N'WEB_CHAT_SMS'

go

update [Cfg].[Settings]  set [Value]= 'http://welcome.chat.vicado.ai/%code%'
where [Name] = N'LITE_LINK_DEEP_URL'

go

update [Cfg].[Settings]  set [Value]= 'http://welcome.chat.vicado.ai/%code%'
where [Name] = N'INVITE_LINK'

go

update [Cfg].[Settings]  set [Value]= 'https://chat.vicado.ai/external/%code%'
where [Name] = N'WEB_CHAT_DEEP_URL'

go
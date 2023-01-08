use [FLChat]
go

INSERT INTO [Cfg].[Settings] ([Name], [Value], [Descr])
values 
(N'COMMAND_GETAVATAR',N'users/%id%/avatar', N'Команда получения аватара'),
(N'AVATAR_DEFAULT',N'https://chat.faberlic.com/user_m.png', N'Ссылка получения аватара для неимеющих')
go
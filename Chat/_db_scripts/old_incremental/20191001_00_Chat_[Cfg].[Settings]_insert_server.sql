use [FLChat]
go

INSERT INTO [Cfg].[Settings] ([Name], [Value], [Descr])
values 
(N'MAINSERVER_NAME',N'http://5.188.115.71:33892/FLChat/', N'Адрес сервера'),
(N'COMMAND_GETFILE',N'file/%id%', N'Команда получения файла')
go
use [FLChat]
go

UPDATE [Cfg].[Settings] set [Value] = 'http://5.188.115.71:8082/FLChat/'
WHERE [Name] = N'MAINSERVER_NAME'
go
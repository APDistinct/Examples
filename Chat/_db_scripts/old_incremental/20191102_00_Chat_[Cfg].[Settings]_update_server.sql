use [FLChat]
go

update [Cfg].[Settings]  set [Value]= 'https://rvprj.ru:8443/FLChat/'
where [Name] = N'MAINSERVER_NAME'

go
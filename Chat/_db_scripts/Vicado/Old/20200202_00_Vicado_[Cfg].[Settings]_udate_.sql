use [FLChat]
go

update [Cfg].[Settings]  set [Value]= '%sendername% from Vicado sent you a new message'
where [Name] = N'EMAIL_SUBJECT_TEMPLATE'

go

update [Cfg].[Settings]  set [Value]= 'https://flchat-test.rvprj.ru/FLChat'
where [Name] = N'MAINSERVER_NAME'

go


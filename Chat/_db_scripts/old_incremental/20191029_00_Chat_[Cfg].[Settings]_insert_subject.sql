use [FLChat]
go

INSERT INTO [Cfg].[Settings] ([Name], [Value], [Descr])
values 
(N'EMAIL_SUBJECT_TEMPLATE',N'Сообщение от личного консультанта Faberlic %sendername%', N'Шаблон темы письма')

go
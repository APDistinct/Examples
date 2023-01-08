use [FLChat]
go

INSERT INTO [Cfg].[Settings] ([Name], [Value], [Descr])
values 
(N'VK_FILELINK_TEMPLATE',N'https://vk.com/away.php?to=#FileLink&cc_key=#FileText', N'Шаблон ссылки файла')
go
use [FLChat]
go

insert into [Cfg].[Settings] ([Name], [Value], [Descr])
values
('WEB_CHAT_DEEP_URL', 'fl.im/mid%code%', 'Шаблон ссылки на веб-чат'),
('WEB_CHAT_SMS', 'Ваш наставник %sender_name% отправил вам сообщение. %url%', 'Текст смс сообщения для веб-чата')
go
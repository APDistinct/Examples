use [FLChat]
go

delete from [Cfg].[Settings] where [Name] in (
	N'TEXT_LITELINK_GREETING_MSG',
	N'TEXT_LITELINK_ROUTED',
	N'TEXT_LITELINK_UNROUTED',
	N'TEXT_DEEPLINK_GREETING_MSG_REJECTED',
	N'TEXT_LITELINK_GREETING_MSG_VIBER');

insert into [Cfg].[Settings]([Name], [Value], [Descr])
values
(N'TEXT_LITELINK_GREETING_MSG', N'Добрый день, #ФИО.', N'Приветствие для известного пользователя, перешедшего по лёгкой ссылке.'),
(N'TEXT_LITELINK_ROUTED', N'Ваш личный консультант %addressee%. Вы можете задать ему вопрос прямо сейчас.', N'Вторая часть приветствия для известного пользователя, перешедшего по лёгкой ссылке.'),
(N'TEXT_LITELINK_UNROUTED', N'Оставайтесь на связи и получайте актуальную информацию о своих баллах и акциях.', 'Приветствие для известного пользователя, перешедшего по лёгкой ссылке, для которого нет наставгика с флчатом'),
(N'TEXT_DEEPLINK_GREETING_MSG_REJECTED', N'Добрый день!
Оставайтесь на связи и получайте актуальную информацию об акциях и новинках.', 'Приветствие для неизвестного пользователя, перешедшего по ссылке'),
(N'TEXT_LITELINK_GREETING_MSG_VIBER', N'Добрый день, #ФИО. Общайтесь с личным консультантом FABERLIC и получайте мгновенно ответы на все вопросы.', 'Первая часть приветсвия для пользователя перешедшего по лёшкой ссылке в вайбер');


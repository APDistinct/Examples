use [ViChat]
go

declare @dt as table (
 [Name] [nvarchar](100) NOT NULL PRIMARY KEY,
  [Value] [nvarchar](max) NULL,
  [Descr] [nvarchar](500) NULL
)

INSERT INTO @dt ([Name], [Value], [Descr])
VALUES 


('AVATAR_DEFAULT', N'https://chat.vicado.ai/user_m.png', 'Ссылка получения аватара для неимеющих'),
('COMMAND_GETAVATAR', N'users/%id%/avatar', N'Команда получения аватара'),
('COMMAND_GETFILE', N'file/%id%', N'Команда получения файла'),
('EMAIL_SUBJECT_TEMPLATE', N'%sendername% from Vicado sent you a new message', 'Шаблон темы письма'),
('INVITE_LINK', N'http://chat.vicado.ai/external/%code%', N'Ссылка на страницу сайта для приглашения пользователя в прямое подчинение'),
('LITE_LINK_DEEP_URL', N'http://chat.vicado.ai/external/%code%', N'Шаблон ссылки на страницу, где отображаются лёгкие ссылки'),
('MAINSERVER_NAME', N'https://wineday-srv.vicado.ai/ViChat/', N'Адрес сервера'),
(N'MESSAGE_DELAY_TIMEOUT', N'1000', N'Таймаут в миллисекундах для проверки статуса/времени отложеных сообщений'),
('ONLINE_PERIOD_SEC', N'300', N'Период в секундах с последнего запроса событий, в течении которого пользователь считается онлайн'),
('TEXT_CHANGE_MESSAGE_ADDRESSEE', N'Сообщения будут отправлены консультанту: %FullName%', N'Сообщение при смене'),
('TEXT_DEEPLINK_GREETING_MSG_REJECTED', N'Добрый день! Оставайтесь на связи и получайте актуальную информацию об акциях и новинках.', N'Приветствие для неизвестного пользователя, перешедшего по ссылке'),
('TEXT_GREETING_MSG', N'Добро пожаловать в Vicado Chat. Общайтесь с наставниками из Vicado, получайте советы и помощь экспертов.', N'Приветственное сообщение при подключении нового канала'),
('TEXT_LITELINK_GREETING_MSG', N'Добрый день, #ФИО.', N'Приветствие для известного пользователя, перешедшего по лёгкой ссылке.'),
('TEXT_LITELINK_GREETING_MSG_VIBER', N'Добрый день, #ФИО. Общайтесь с личным консультантом Vicado и получайте мгновенно ответы на все вопросы.', N'Первая часть приветсвия для пользователя перешедшего по лёшкой ссылке в вайбер'),
('TEXT_LITELINK_ROUTED', N'Ваш личный консультант %addressee%. Вы можете задать ему вопрос прямо сейчас.', N'Вторая часть приветствия для известного пользователя, перешедшего по лёгкой ссылке.'),
('TEXT_LITELINK_UNROUTED', N'Оставайтесь на связи и получайте актуальную информацию о своих баллах и акциях.', N'Приветствие для известного пользователя, перешедшего по лёгкой ссылке, для которого нет наставника с флчатом'),
('TEXT_TG_START_MSG', N'Официальный канал Vicado. Здесь вы можете общаться со своими наставниками', N'Сообщение на команду /start введённую старым пользователем'),
('TEXT_TG_SWITCH_TO_SENDER', N'Написать ответ', N'Сообщение показывается под входящим сообщением от пользователя, который не является текущим адресатом'),
('VIBER_WELCOME_MESSAGE', N'Добро пожаловать в Vicado Chat. Общайтесь с наставниками из Vicado, получайте советы и помощь экспертов.', N'Приветственное сообщение при подключении при открытии диалога в Viber (событие ConversationStarted)'),
('VK_FILELINK_TEMPLATE', N'https://vk.com/away.php?to=#FileLink&cc_key=#FileText', N'Шаблон ссылки файла'),
('WEB_CHAT_DEEP_URL', N'http://chat.vicado.ai/demo/%code%', N'Шаблон ссылки на веб-чат'),
('WEB_CHAT_SMS', N'%sender_name% invites you to Vicado: %url%', N'Текст смс сообщения для веб-чата'),
('WEB_CHAT_VIBER', N'#sendername, ваш личный консультант Vicado, отправил вам сообщение:', N'Шаблон сообщения СМС для Viber')
,('INVITELINK_WORK', N'1', N'Признак включения обработчика для технологии Invite Link')
,('COMMONLINK_WORK', N'0', N'Признак включения обработчика для технологии Common Link')
,('TMP_CACHE', N'Simple', N'Признак включения')
,('TEXT_REJECT_MESSAGE', N'Вам не назначен личный консультант, поэтому ваше сообщение пока не будет прочитано', N'Сообщение при отсутствии адрессата для сообщения')


update t
set t.[Value] = d.[Value], t.[Descr] = d.[Descr]
from [Cfg].[Settings] t
inner join @dt d on t.[Name] = d.[Name]
where t.[Descr] <> d.[Descr] 
   or t.[Value] <> d.[Value]

insert into [Cfg].[Settings] ([Name], [Value], [Descr])
select [Name], [Value], [Descr] 
from @dt where [Name] not in (select [Name] from [Cfg].[Settings]);

select * from [Cfg].[Settings]
go
use [FLChat]
go

create table [Cfg].[ExternalTransportButton] (
  [Id] int NOT NULL IDENTITY (1,1),
  [Caption] nvarchar(100) NOT NULL,
  [Command] nvarchar(255) NOT NULL,
  [Row] int NOT NULL,
  [Col] int NOT NULL,
  constraint [PK__CfgExternalTransportButton] primary key([Id])
)
go

insert into [Cfg].[ExternalTransportButton] ([Caption], [Command], [Row], [Col])
values
(N'Выбрать адресата', N'cmd:select_addressee', 0, 0),
(N'Мои баллы', N'cmd:score', 1, 0),
(N'Мой профиль', N'cmd:profile', 1, 1),
(N'Акции', N'url:www.ya.ru', 2, 0),
(N'Новый каталог', N'url:www.ya.ru', 2, 1),
(N'Как заказать', N'url:www.ya.ru', 3, 0),
(N'Где получить', N'url:www.ya.ru', 3, 1)
go
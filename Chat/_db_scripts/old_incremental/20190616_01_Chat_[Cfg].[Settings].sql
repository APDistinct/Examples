use [FLChat]
go

create table [Cfg].[Settings] (
  [Name] nvarchar(100) NOT NULL PRIMARY KEY,
  [Value] nvarchar(max),
  [Descr] nvarchar(500)
)
go

insert into [Cfg].[Settings] ([Name], [Value], [Descr])
values 
('ONLINE_PERIOD_SEC', '300', 'Период в секундах с последнего запроса событий, в течении которого пользователь считается онлайн')
go
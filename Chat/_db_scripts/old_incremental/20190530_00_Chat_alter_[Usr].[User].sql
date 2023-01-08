use [FLChat]
go

alter table [Usr].[User]
add LoBonusScores decimal(12,2) NULL
go

alter table [Usr].[User]
add OlgBonusScores decimal(12,2) NULL
go

alter table [Usr].[User]
add GoBonusScores decimal(12,2) NULL
go

create schema [Dir]
go

create table [Dir].[Rank] (
  [Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [Name] nvarchar(250) NOT NULL UNIQUE
)
go

insert into [Dir].[Rank] ([Name])
values 
('Рубиновый директор'),
('Серебрянный директор'),
('Золотой директор'),
('Элитный Директор'),
('Бриллиантовый Директор'),
('Национальный директор')
go

alter table [Usr].[User]
add [RankId] int NULL
go

alter table [Usr].[User]
add constraint [FK__UsrUser__DirRank]
  foreign key ([RankId])
  references [Dir].[Rank] ([Id])
go
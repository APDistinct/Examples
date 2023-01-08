use [FLChat]
go

alter table [Usr].[User]
add [FLUserNumber] int NULL;
go

CREATE UNIQUE NONCLUSTERED INDEX  [UNQ__UsrUser__FLUserNumber]
ON [Usr].[User] ([FLUserNumber])
  where [FLUserNumber] is not null
go

alter table [Usr].[User]
add [ParentFLUserNumber] int NULL;
go

alter table [Usr].[User]
add [Birthday] date NULL;
go

alter table [Usr].[User]
add [ZipCode] nvarchar(20) NULL;
go

create table [Dir].[Country] (
  [Id] int IDENTITY(1,1) NOT NULL,
  [Name] nvarchar(250) NOT NULL,
  constraint [PK__DirCountry] primary key ([Id]),
  constraint [UNQ__DirCountry__Name] unique ([Name])
)
go

create table [Dir].[Region] (
  [Id] int IDENTITY(1,1) NOT NULL,
  [CountryId] int NOT NULL,
  [Name] nvarchar(250) NOT NULL,
  constraint [PK__DirRegion] primary key ([Id]),
  constraint [FK__DirRegion__DirCountry] 
    foreign key ([CountryId])
	references [Dir].[Country]([Id])
	on delete cascade,
  constraint [UNQ__DirRegion__CountryName] unique ([CountryId], [Name])
)
go

create table [Dir].[City] (
  [Id] int IDENTITY(1,1) NOT NULL,
  [RegionId] int NOT NULL,
  [Name] nvarchar(250),
  constraint [PK__DirCity] primary key ([Id]),
  constraint [FK__DirCity__DirRegion]
    foreign key ([RegionId])
	references [Dir].[Region]([Id])
	on delete cascade,
  constraint [UNQ__DirCity__RegionName] unique ([RegionId], [Name])
)
go

alter table [Usr].[User]
add [CityId] int NULL
go

alter table [Usr].[User]
add constraint [FK__UsrUser__DirCity]
  foreign key ([CityId])
  references [Dir].[City] ([Id])
  on delete cascade
go

alter table [Usr].[User]
add [EmailPermission] bit NOT NULL default 1
go

alter table [Usr].[User]
add [SmsPermission] bit NOT NULL default 1
go

alter table [Usr].[User]
add [IsDirector] bit NOT NULL default 0
go

alter table [Usr].[User]
add [LastOrderDate] date NULL
go

alter table [Usr].[User]
add [PeriodsWolo] int NULL
go

alter table [Usr].[User]
add [CashBackBalance] decimal(12,2) NULL
go

alter table [Usr].[User]
add [FLClubPoints] decimal(12,2) NULL
go

alter table [Usr].[User]
add [FLClubPointsBurn] decimal(12,2) NULL
go

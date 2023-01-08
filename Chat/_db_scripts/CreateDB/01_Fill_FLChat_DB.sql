USE [FLChat]
GO

--*****  Create schemas *****

if SCHEMA_ID('Arch') is null
exec ('CREATE SCHEMA [Arch]')
GO

if SCHEMA_ID('Usr') is null
exec ('CREATE SCHEMA [Usr]')
GO

if SCHEMA_ID('Msg') is null
exec ('CREATE SCHEMA [Msg]')
GO

if SCHEMA_ID('Auth') is null
exec ('create schema [Auth]')
GO

if SCHEMA_ID('Test') is null
exec ('create schema [Test]')
GO

if SCHEMA_ID('Ui') is null
exec ('CREATE SCHEMA [Ui]')
GO

if SCHEMA_ID('Dir') is null
exec ('CREATE SCHEMA [Dir]')
GO

if SCHEMA_ID('File') is null
exec ('CREATE SCHEMA [File]')
GO

if SCHEMA_ID('Reg') is null
exec ('create schema [Reg]')
go

if SCHEMA_ID('Cfg') is null
exec ('create schema [Cfg]')
go

if SCHEMA_ID('Cache') is null
exec ('create schema [Cache]')
go

if SCHEMA_ID('System') is null
exec('CREATE SCHEMA [System]')
GO

if SCHEMA_ID('Imp') is null
exec('CREATE SCHEMA [Imp]')
GO


/*****************************************************************************
	Types
*****************************************************************************/
if TYPE_ID('[dbo].[GuidBigintTable]') is null
create type [dbo].[GuidBigintTable] as table (
  [Guid] uniqueidentifier NOT NULL,
  [BInt] bigint NOT NULL
)
go

if TYPE_ID('[dbo].[GuidList]') is null
create type [dbo].[GuidList] as table (
  [Guid] uniqueidentifier NOT NULL
)
go

if TYPE_ID('[Usr].[UserIdDeep]') is null
CREATE TYPE [Usr].[UserIdDeep] AS TABLE(
	[UserId] uniqueidentifier NOT NULL UNIQUE,
	[Deep] integer NULL,
	UNIQUE NONCLUSTERED ([UserId] ASC)WITH (IGNORE_DUP_KEY = OFF)

)
GO

if TYPE_ID('[dbo].[IntList]') is null
CREATE TYPE [dbo].[IntList] AS TABLE (
    [Value] int NOT NULL	    
)
GO

if TYPE_ID('[dbo].[StringList]') is null
begin
 create type [dbo].[StringList] as table (
  [String] nvarchar(max) NOT NULL
)
 PRINT 'create type [dbo].[StringList]'
end
go


/*****************************************************************************
	FULLTEXT CATALOGs
*****************************************************************************/
IF NOT EXISTS(SELECT 1 FROM sys.fulltext_catalogs WHERE name = 'UserFTCat') 
  begin
   CREATE FULLTEXT CATALOG UserFTCat  WITH ACCENT_SENSITIVITY = ON AS DEFAULT;
   print 'CREATE FULLTEXT CATALOG UserFTCat'
  end;


  
/*****************************************************************************
	Tables
*****************************************************************************/
/****** Table [System].[Guard] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Guard' and [uid] = SCHEMA_ID('System')) begin
PRINT '[System].[Guard]: create table'
create table [System].[Guard] (
  [Id] int NOT NULL,
  [Url] nvarchar(500) NOT NULL,
  [AppKey] nvarchar(250) NOT NULL,
  [RefreshDate] datetime NULL,
  [Token] nvarchar(1000) NULL,
  [Hash] nvarchar(1000) NULL,
  [Ver] int NULL,
  constraint [PK__SystemGuard] primary key ([Id]),
  constraint [CHK__SystemGuard__SingleRow] check ([Id] = 1),
  constraint [CHK__SystemGuard__HasData] 
    check (([RefreshDate] is null and [Token] is null and [Hash] is null and [Ver] is null)
	    or ([RefreshDate] is not null and [Token] is not null and [Hash] is not null and [Ver] is not null))
)
end
go


/***********************************************************
	[Cfg] tables
***********************************************************/

/****** Table [Cfg].[TransportType] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='TransportType' and [uid] = SCHEMA_ID('Cfg'))
CREATE TABLE [Cfg].[TransportType] (
  [Id] integer NOT NULL PRIMARY KEY,
  [Name] varchar(50) NOT NULL UNIQUE,
  [Enabled] bit DEFAULT 1 NOT NULL,
  [VisibleForUser] bit default 1 NOT NULL,
  [CanSelectAsDefault] bit default 1 NOT NULL,
  [Prior] tinyint default 10 NOT NULL,
  [DeepLink] nvarchar(255) NULL,
  [InnerTransport] bit NOT NULL default 0,
  [SendGreetingMessage] bit not null default 0
)
GO

declare @dt as table (
  [Id] integer NOT NULL PRIMARY KEY,
  [Name] varchar(50) NOT NULL UNIQUE,
  [Enabled] bit DEFAULT 1 NOT NULL,
  [VisibleForUser] bit default 1 NOT NULL,
  [CanSelectAsDefault] bit default 1 NOT NULL,
  [Prior] tinyint default 10 NOT NULL,
  [InnerTransport] bit NOT NULL default 0,
  [SendGreetingMessage] bit not null default 0
);

INSERT INTO @dt 
([Id], [Name], [VisibleForUser], [CanSelectAsDefault], [Prior], [InnerTransport], [SendGreetingMessage])
VALUES 
(-1, 'Test', 		1, 1, 10, 0, 0),
(0, 'FLChat',		1, 1, 255, 1, 0),
(1, 'Telegram',		1, 1, 10,  0, 0),
(3, 'Viber',		1, 1, 10,  0, 0),
(4, 'VK',		1, 1, 10,  0, 0),
(100, 'WebChat',	0, 1, 0,   1, 0),
(150, 'Sms',		0, 0, 0,   0, 0),
(151, 'Email',		0, 0, 0,   0, 0);

update t
set t.[Name] = d.[Name]
from [Cfg].[TransportType] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name]
  and t.[Enabled] <> d.[Enabled]
  and t.[VisibleForUser] <> d.[VisibleForUser]
  and t.[CanSelectAsDefault] <> d.[CanSelectAsDefault]
  and t.[Prior] <> d.[Prior]
  and t.[InnerTransport] <> d.[InnerTransport]
  and t.[SendGreetingMessage] <> d.[SendGreetingMessage]

insert into [Cfg].[TransportType]
([Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior], [InnerTransport], [SendGreetingMessage])
select [Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior], [InnerTransport], [SendGreetingMessage]
from @dt where [Id] not in (select [Id] from [Cfg].[TransportType]);
GO

/****** 05.04.2021 ******/
if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Cfg].[TransportType]') 
	and [name] = 'ProcessingWay') 
begin
 alter table [Cfg].[TransportType]  add [ProcessingWay] integer NULL
 PRINT '[Cfg].[TransportType]: add column [ProcessingWay]'
end
go

update  [Cfg].[TransportType] set [ProcessingWay] = 1 where [Id] = 2



/****** Table [Cfg].[MessageType] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MessageType' and [uid] = SCHEMA_ID('Cfg'))
CREATE TABLE [Cfg].[MessageType] (
  [Id] integer NOT NULL PRIMARY KEY,
  [Name] nvarchar(50) NOT NULL UNIQUE,
  [ShowInHistory] bit NOT NULL default 1,
  [LimitForDay] int NULL,
  [LimitForOnce] int NULL
)
GO

declare @dt as table (
  [Id] integer NOT NULL PRIMARY KEY,
  [Name] nvarchar(50) NOT NULL UNIQUE,
  [ShowInHistory] bit NOT NULL
);

INSERT INTO @dt ([Id], [Name], [ShowInHistory])
VALUES
(0, 'Personal', 1)
--,(1, 'Group', 1)
,(2, 'Broadcast', 1)
--,(3, 'Segment', 1)
,(4, 'Mailing', 0)


update t
set t.[Name] = d.[Name]
from [Cfg].[MessageType] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name] or t.[ShowInHistory] <> d.[ShowInHistory]

insert into [Cfg].[MessageType]([Id], [Name], [ShowInHistory])
select [Id], [Name], [ShowInHistory] 
from @dt where [Id] not in (select [Id] from [Cfg].[MessageType]);
GO

/****** Table [Cfg].[EventType] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='EventType' and [uid] = SCHEMA_ID('Cfg'))
CREATE TABLE [Cfg].[EventType] (
  [Id] integer NOT NULL primary key,
  [Name] varchar(50) NOT NULL UNIQUE
)

declare @dt as table (
  [Id] integer NOT NULL primary key,
  [Name] varchar(50) NOT NULL UNIQUE
)

INSERT INTO @dt ([Id], [Name])
values 
(0, 'Test event'),
(1, 'Message sent'),
(2, 'Message delivered'),
(3, 'Message read'),
(4, 'Message deleted'),
(5, 'Message failed'),
(10, 'Incoming message'),
(100, 'User avatar changed'),
(101, 'Deep link accepted')

update t
set t.[Name] = d.[Name]
from [Cfg].[EventType] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name];

insert into [Cfg].[EventType]([Id], [Name])
select [Id], [Name] from @dt where [Id] not in (select [Id] from [Cfg].[EventType]);
GO

/****** Table [Cfg].[MediaTypeGroup] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MediaTypeGroup' and [uid] = SCHEMA_ID('Cfg'))
CREATE TABLE [Cfg].[MediaTypeGroup](
	[Id] int NOT NULL PRIMARY KEY,
	[Name] [nvarchar](50) NOT NULL,
	[MaxLength] int NULL
)

declare @dt as table(
	[Id] int NOT NULL PRIMARY KEY,
	[Name] [nvarchar](50) NOT NULL	
)

INSERT INTO @dt ([Id], [Name])
values 
(1, 'Image'),
(2, 'Document'),
(3, 'Audio'),
(4, 'Video')

update t
set t.[Name] = d.[Name]
from [Cfg].[MediaTypeGroup] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name];

insert into [Cfg].[MediaTypeGroup]([Id], [Name])
select [Id], [Name] from @dt where [Id] not in (select [Id] from [Cfg].[MediaTypeGroup]);

GO

/****** Table [Cfg].[MediaType] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MediaType' and [uid] = SCHEMA_ID('Cfg'))
CREATE TABLE [Cfg].[MediaType] (
  [Id] integer NOT NULL IDENTITY(1,1),
  [Name] varchar(500) NOT NULL,
  [CanBeAvatar] bit DEFAULT 0 NOT NULL,
  [MediaTypeGroupId] int NOT NULL,
  [Enabled] bit NOT NULL DEFAULT 1,
  CONSTRAINT [PK__MediaType] PRIMARY KEY CLUSTERED ([Id]),
  CONSTRAINT [FK__MediaTypeId__MediaTypeGroupId] 
	foreign key ([MediaTypeGroupId])
	references [Cfg].[MediaTypeGroup] ([Id]),
  constraint [UNQ__CfgMediaType__Name] UNIQUE NONCLUSTERED ([Name] ASC)
)
GO

/****** Table [Cfg].[Settings] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Settings' and [uid] = SCHEMA_ID('Cfg'))
create table [Cfg].[Settings] (
  [Name] nvarchar(100) NOT NULL PRIMARY KEY,
  [Value] nvarchar(max),
  [Descr] nvarchar(500)
)
go

/****** Table [Cfg].[ExternalTransportButton] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='ExternalTransportButton' and [uid] = SCHEMA_ID('Cfg'))
create table [Cfg].[ExternalTransportButton] (
  [Id] int NOT NULL IDENTITY (1,1),
  [Caption] nvarchar(100) NOT NULL,
  [Command] nvarchar(255) NOT NULL,
  [Row] int NOT NULL,
  [Col] int NOT NULL,
  [HideForTemporary] bit NOT NULL default 0,
  constraint [PK__CfgExternalTransportButton] primary key([Id])
)
go

/****** Table [Cfg].[Scenario] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Scenario' and [uid] = SCHEMA_ID('Cfg'))
begin
CREATE TABLE [Cfg].[Scenario] (
  [Id] integer NOT NULL IDENTITY(1,1),
  [Name] nvarchar(50) NOT NULL ,
  [Comment] nvarchar(200) NULL,
  CONSTRAINT [PK__Scenario__Id] PRIMARY KEY NONCLUSTERED ( [Id] ),
  CONSTRAINT [UNQ__Scenario__Name] UNIQUE CLUSTERED ( [Name] )
)
  PRINT 'CREATE TABLE [Cfg].[Scenario]'
end

GO

declare @dt as table (
  [Name] nvarchar(50) NOT NULL UNIQUE,
  [Comment] nvarchar(200) NULL 
);

INSERT INTO @dt ([Name], [Comment])
VALUES
( 'Main', 'By default')
,( 'Invite', 'With Invite from ')
,( 'Common', 'Without any code')


update t
set t.[Comment] = d.[Comment] 
from [Cfg].[Scenario] t
inner join @dt d on t.[Name] = d.[Name]
where t.[Comment] <> d.[Comment]

insert into [Cfg].[Scenario]([Name], [Comment] )
select [Name], [Comment]  
from @dt where [Name] not in (select [Name] from [Cfg].[Scenario]);
GO



/****** Table [Cfg].[ScenarioStep] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='ScenarioStep' and [uid] = SCHEMA_ID('Cfg'))
begin
CREATE TABLE [Cfg].[ScenarioStep] (
  [Id] integer NOT NULL IDENTITY(1,1),
  [ScenarioId] integer NOT NULL,
  [Step] integer NOT NULL,
  [Description] nvarchar(200) NULL,
  CONSTRAINT [PK__CfgScenarioStep] PRIMARY KEY ([Id]),
  CONSTRAINT [UNQ__Scenario__ScenarioId_Step] UNIQUE CLUSTERED ( [ScenarioId], [Step] ),
  CONSTRAINT [FK__CfgScenarioStep__CfgScenario] foreign key ([ScenarioId])
    references [Cfg].[Scenario] ([Id])  
)
  PRINT 'CREATE TABLE [Cfg].[ScenarioStep]'
end

GO

/****** 05.04.2021 ******/
/****** Table [Cfg].[Provider] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Provider' and [uid] = SCHEMA_ID('Cfg'))
begin
CREATE TABLE [Cfg].[Provider] (
  [Id] uniqueidentifier NOT NULL,
  [Name] nvarchar(50) NOT NULL,
  [Description] nvarchar(500) NULL
  constraint [PK__CfgProvider] PRIMARY KEY ([Id])
)
PRINT 'CREATE TABLE [Cfg].[Provider]'
end
GO

/****** Table [Cfg].[TransportProvider] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='TransportProvider' and [uid] = SCHEMA_ID('Cfg'))
CREATE TABLE [Cfg].[TransportProvider] (
  [Id] uniqueidentifier NOT NULL,
  [Name] nvarchar(50) NOT NULL,
  [TransportTypeId] integer NOT NULL,
  [Description] nvarchar(500) NULL,
  [Enabled] bit DEFAULT 1 NOT NULL,
  [ProviderId] uniqueidentifier NOT NULL,
  constraint [PK__CfgTransportProvider] PRIMARY KEY ([Id]),
  constraint [UNQ__CfgTransportProvider__Name_TransportTypeId] UNIQUE NONCLUSTERED ([Name] ASC, [TransportTypeId])
)
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__CfgTransportProvider__CfgTransportType' and [uid]=SCHEMA_ID('Cfg'))
begin
alter table [Cfg].[TransportProvider] add constraint [FK__CfgTransportProvider__CfgTransportType]
  foreign key ([TransportTypeId])  references [Cfg].[TransportType]([Id])
PRINT '[Cfg].[TransportProvider] add constraint [FK__CfgTransportProvider__CfgTransportType]'
end
GO

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Cfg].[TransportProvider]') 
	and [name] = 'ProviderId') 
begin
 alter table [Cfg].[TransportProvider]  add [ProviderId] uniqueidentifier NOT NULL
 PRINT '[Cfg].[TransportProvider]: add column [ProviderId]'
end
go
-- Для новой таблицы, без значений, заменить сразу
-- alter table [Cfg].[TransportProvider]  ALTER COLUMN  [ProviderId] uniqueidentifier NOT NULL


if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__CfgTransportProvider__CfgProvider' and [uid]=SCHEMA_ID('Cfg'))
begin
alter table [Cfg].[TransportProvider] add constraint [FK__CfgTransportProvider__CfgProvider]
  foreign key ([ProviderId])  references [Cfg].[Provider]([Id])
PRINT '[Cfg].[TransportProvider] add constraint [FK__CfgTransportProvider__CfgProvider]'
end
GO



/*********************************************************************
		[Dir] tables
*********************************************************************/

/****** Table [Dir].[Rank]******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Rank' and [uid] = SCHEMA_ID('Dir'))
create table [Dir].[Rank] (
  [Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [Name] nvarchar(250) NOT NULL UNIQUE
)
go

/****** Table [Dir].[Country] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Country' and [uid] = SCHEMA_ID('Dir'))
create table [Dir].[Country] (
  [Id] int IDENTITY(1,1) NOT NULL,
  [Name] nvarchar(250) NOT NULL,
  constraint [PK__DirCountry] primary key ([Id]),
  constraint [UNQ__DirCountry__Name] unique ([Name])
)
go

/****** Table [Dir].[Region] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Region' and [uid] = SCHEMA_ID('Dir'))
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

/****** Table [Dir].[City] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='City' and [uid] = SCHEMA_ID('Dir'))
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

/*********************************************************************
		[Usr] tables
*********************************************************************/

/****** Table [Usr].[User] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='User' and [uid] = SCHEMA_ID('Usr'))
CREATE TABLE [Usr].[User] (
  [Id] uniqueidentifier NOT NULL DEFAULT NEWSEQUENTIALID(),
  [Enabled] bit NOT NULL default 1,
  [FullName] nvarchar(255) NULL,
  [IsConsultant] bit NOT NULL default 0,
  [RegistrationDate] date NULL,
  [InsertDate] datetime NOT NULL DEFAULT GETUTCDATE(),
  [SignUpDate] datetime NULL,
  [Phone] varchar(20) NULL,
  [Email] nvarchar(255) NULL,
  [Login] varchar(255) NULL,
  [PswHash] varchar(255) NULL,
  [OwnerUserId] uniqueidentifier null,
  [DefaultTransportTypeId] integer null,
  [AvatarUploadDate] datetime null,
  [IsTemporary] bit NOT NULL default 0,
  [IsBot] bit NOT NULL default 0,
  [LoBonusScores] decimal(12,2) NULL,
  [OlgBonusScores] decimal(12,2) NULL,
  [GoBonusScores] decimal(12,2) NULL,
  [RankId] int NULL,
  [LastGetEvents] datetime NULL,
  [FLUserNumber] int NULL,
  [ParentFLUserNumber] int NULL,
  [Birthday] date NULL,
  [ZipCode] nvarchar(20) NULL,
  [CityId] int NULL,
  [EmailPermission] bit NOT NULL default 1,
  [SmsPermission] bit NOT NULL default 1,
  [IsDirector] bit NOT NULL default 0,
  [LastOrderDate] date NULL,
  [PeriodsWolo] int NULL,
  [CashBackBalance] decimal(12,2) NULL,
  [FLClubPoints] decimal(12,2) NULL,
  [FLClubPointsBurn] decimal(12,2) NULL,
  [DontDisableOnUpdateStructure] bit NOT NULL default 0,
  [IsUseDeepChilds] bit NOT NULL default 0,
  [LastImportDate] datetime null,
  [ForeignId] nvarchar(100) NULL,
  [ForeignOwnerId] nvarchar(100) NULL,
  constraint [PK__User__ID] PRIMARY KEY ([Id]),
  constraint [FK__UsrUser__UsrUserOwned] foreign key ([OwnerUserId])
    references [Usr].[User] ([Id]),
  constraint [FK__UsrUser__DirRank]
	foreign key ([RankId])
	references [Dir].[Rank] ([Id]),
  constraint [FK__UsrUser__DirCity]
	foreign key ([CityId])
	references [Dir].[City] ([Id])
	on delete set null
)
GO

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Usr].[User]') 
	and [name] = 'ForeignId') begin
PRINT '[Usr].[User]: add column [ForeignId]'
alter table [Usr].[User]
add [ForeignId] nvarchar(100) NULL
end
go

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Usr].[User]') 
	and [name] = 'ForeignOwnerId') begin
PRINT '[Usr].[User]: add column [ForeignOwnerId]'
alter table [Usr].[User]
add [ForeignOwnerId] nvarchar(100) NULL
end
go

if not exists(select 1 from sysindexes where [name] = 'UNQ__UsrUser__ForeignId') begin
PRINT '[Usr].[User]: add index [UNQ__UsrUser__ForeignId]'
create unique index [UNQ__UsrUser__ForeignId]
	on [Usr].[User] ([ForeignId])
	where ([ForeignId] is not null)
end
go

if not exists(select 1 from sysindexes where [name] = 'UNQ__UsrUser_Phone')
CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrUser_Phone] ON [Usr].[User]
	([Phone] ASC)
WHERE ([Phone] IS NOT NULL AND [Enabled] = (1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

if not exists(select 1 from sysindexes where [name] = 'UNQ__UsrUser_Email')
CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrUser_Email] ON [Usr].[User]
	([Email] ASC)
WHERE ([Email] IS NOT NULL AND [Enabled] = (1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

if not exists(select 1 from sysindexes where [name] = 'UNQ__UsrUser_Login')
CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrUser_Login]
ON [Usr].[User]([Login])
WHERE [Login] IS NOT NULL;
GO

if not exists(select 1 from sysindexes where [name] = 'UNQ__UsrUser__FLUserNumber')
CREATE UNIQUE NONCLUSTERED INDEX  [UNQ__UsrUser__FLUserNumber]
ON [Usr].[User] ([FLUserNumber])
  where [FLUserNumber] is not null
go

if not exists(select 1 from sysindexes where [name] = 'IDX__UsrUser__OwnerUserId')
create index [IDX__UsrUser__OwnerUserId]
  on [Usr].[User] ([OwnerUserId])
  include ([Id], [Enabled])
go

if not exists(select 1 from sysindexes where [name] = 'IDX__UsrUser__IsUseDeepChilds') begin
PRINT '[Usr].[User]: add index [IDX__UsrUser__IsUseDeepChilds]'
create index [IDX__UsrUser__IsUseDeepChilds]
	on [Usr].[User] ([IsUseDeepChilds])	
	where ([IsUseDeepChilds] = (1))
end
go

/****** 05.04.2021 ******/
--DROP FULLTEXT INDEX ON [Usr].[User];
 IF NOT EXISTS(SELECT 1 FROM sys.fulltext_indexes  where object_id = object_id('[Usr].[User]'))
  begin
   CREATE FULLTEXT INDEX ON [Usr].[User]
	(   
	  FullName,
	  Email,
	  Phone
	)
     KEY INDEX [PK__User__ID]
    WITH STOPLIST = SYSTEM, CHANGE_TRACKING OFF, NO POPULATION;
   ALTER FULLTEXT INDEX ON [Usr].[User] START FULL POPULATION;

   print 'CREATE FULLTEXT INDEX ON [Usr].[User]'
  end;
GO


/****** Table [Usr].[Transport] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Transport' and [uid] = SCHEMA_ID('Usr'))
CREATE TABLE [Usr].[Transport] (
  [UserId] uniqueidentifier NOT NULL,
  [TransportTypeId] integer NOT NULL,
  [TransportOuterId] nvarchar(255) NULL,
  [Enabled] bit NOT NULL default 1,
  constraint [PK__UsrTransport] primary key ([UserId], [TransportTypeId]),
  constraint [FK__UsrTransport__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id])
	on delete cascade,
  constraint [FK_UsrTransport__CfgTransportType] foreign key ([TransportTypeId])  
    references [Cfg].[TransportType] ([Id]),
  CONSTRAINT [CHK_UsrTransport_OuterId] 
	CHECK  ([TransportTypeId] IN     (0, 100, 150, 151) AND [TransportOuterId] IS NULL 
	  OR    [TransportTypeId] NOT IN (0, 100, 150, 151) AND [TransportOuterId] IS NOT NULL)

)
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__UsrUser__UsrTransport' and [uid]=SCHEMA_ID('Usr'))
alter table [Usr].[User] add constraint [FK__UsrUser__UsrTransport]
  foreign key ([Id], [DefaultTransportTypeId])  
  references [Usr].[Transport]([UserId], [TransportTypeId])
GO

if exists (select 1 from sysindexes where [name] = 'UNQ__UsrTransport')
alter table [Usr].[Transport]
drop [UNQ__UsrTransport]
GO

if not exists (select 1 from sysindexes where [name] = 'UNQ__UsrTransport_OuterId')
-- Проверка на '' отсекает также и null
CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrTransport_OuterId] ON [Usr].[Transport]
(
	[TransportTypeId] ASC,
	[TransportOuterId] ASC
)
WHERE ([Enabled]=(1) AND [TransportOuterId] <> '')
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

if not exists(select 1 from sysobjects where xtype='C' 
	and [name]='CHK_UsrTransport_OuterId' and [uid]=SCHEMA_ID('Usr'))
alter table [Usr].[Transport] 
add constraint [CHK_UsrTransport_OuterId]
  check (([TransportTypeId] = 0  AND [TransportOuterId] IS NULL)
      or ([TransportTypeId] <> 0 AND [TransportOuterId] IS NOT NULL))
GO

/****** Table [Usr].[Contact] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Contact' and [uid] = SCHEMA_ID('Usr'))
CREATE TABLE [Usr].[Contact] (
  [UserId] uniqueidentifier NOT NULL,
  [ContactId] uniqueidentifier NOT NULL,
  constraint [PK__UsrContact] primary key ([UserId], [ContactId]),
  constraint [FK__UsrContact__UsrUser1] foreign key ([UserId])
    references [Usr].[User] ([Id]),
  constraint [FK__UsrContact__UsrUser2] foreign key ([ContactId])
    references [Usr].[User] ([Id])
)
GO

/****** Table [Usr].[UserAvatar] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='UserAvatar' and [uid] = SCHEMA_ID('Usr'))
create table [Usr].[UserAvatar] (
  [UserId] uniqueidentifier NOT NULL PRIMARY KEY,
  [Data] varbinary(max) NOT NULL,
  [MediaTypeId] integer NOT NULL,
  [Width] int NOT NULL default 0,
  [Height] int NOT NULL default 0,
  constraint [FK__UsrUserAvatar__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id]),
  CONSTRAINT [FK__UserAvatar__MediaType_Id] foreign key ([MediaTypeId])
    references [Cfg].[MediaType] ([Id])
)
GO

create or alter trigger [Usr].[UserAvatar_OnInsertUpdate_UpdDateInUser]
on [Usr].[UserAvatar]
after insert, update
as
  update [Usr].[User] 
  set [AvatarUploadDate] = GETUTCDATE() 
  where [Id] in (select [UserId] from inserted)
go

create or alter trigger [Usr].[UserAvatar_OnDelete_ClearDateInUser]
on [Usr].[UserAvatar]
after delete
as
  update [Usr].[User] 
  set [AvatarUploadDate] = null
  where [Id] in (select [UserId] from deleted)
go

/****** Table [Usr].[Group] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Group' and [uid] = SCHEMA_ID('Usr'))
CREATE TABLE [Usr].[Group] (
  [Id] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
  [IsDeleted] bit NOT NULL default 0,
  [Name] nvarchar(255) NULL,
  [CreatedByUserId] uniqueidentifier NOT NULL,
  [CreatedDate] datetime NOT NULL DEFAULT GETUTCDATE(),
  [IsEqual] bit NOT NULL,
  [DeletedDate] datetime NULL,
  constraint [FK__UsrGroup__UsrUserOwned] foreign key ([CreatedByUserId])
    references [Usr].[User] ([Id])  
)
GO

/****** Table [Usr].[GroupMember] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='GroupMember' and [uid] = SCHEMA_ID('Usr'))
CREATE TABLE [Usr].[GroupMember] (
  [GroupId] uniqueidentifier NOT NULL,
  [UserId] uniqueidentifier NOT NULL,
  [IsAdmin] bit NOT NULL default 0,
  constraint [PK__UsrGroupList] primary key ([UserId], [GroupId]),
  constraint [FK__UsrGroupList__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id]),
  constraint [FK__UsrGroupList__UsrGroup] foreign key ([GroupId])
    references [Usr].[Group] ([Id])
)
GO

/****** Table [Usr].[Segment] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Segment' and [uid] = SCHEMA_ID('Usr'))
create table [Usr].[Segment] (
  [Id] uniqueidentifier NOT NULL default newsequentialid(),
  [IsDeleted] bit NOT NULL default 0,
  [Name] nvarchar(255) NOT NULL,
  [Descr] nvarchar(255) NOT NULL default '',
  [ShowInShortProfile] bit NOT NULL default 0,
  [Tag] nvarchar(250) NULL,
  [PartnerName] nvarchar(100) NULL,
  constraint [PK__UsrSegment] primary key ([Id]),
)
go

if not exists (select 1 from sysindexes where [name] = 'UNQ__UsrSegment__Name')
create unique index [UNQ__UsrSegment__Name]
  on [Usr].[Segment] ([Name])
  where ([IsDeleted]=(0))
go

if not exists (select 1 from sysindexes where [name] = 'UNQ__UsrSegment__PartnerName')
create unique index [UNQ__UsrSegment__PartnerName]
  on [Usr].[Segment] ([PartnerName])
  where [PartnerName] is not null
go

/****** Table [Usr].[SegmentMember] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='SegmentMember' and [uid] = SCHEMA_ID('Usr'))
create table [Usr].[SegmentMember] (
  [SegmentId] uniqueidentifier NOT NULL,
  [UserId] uniqueidentifier NOT NULL,
  constraint [PK__UsrSegmentMember] primary key ([SegmentId], [UserId]),
  constraint [FK__UsrSegmentMember__UsrSegment] foreign key ([SegmentId])
    references [Usr].[Segment]([Id]),
  constraint [FK__UsrSegmentMember__UsrUser] foreign key ([UserId])
    references [Usr].[User]([Id])
)
go

/********* 2021.04.26 ***********/
/********* Ускорение импорта ***********/
if not exists (select 1 from sysindexes where [name] = 'IDX__UsrSegmentMember__UserId') 
begin
PRINT '[Usr].[SegmentMember]: create index IDX__UsrSegmentMember__UserId'
create index [IDX__UsrSegmentMember__UserId]
  on [Usr].[SegmentMember] ([UserId])  
end
go


--  Тормоза на кэшировании, временно отключены до выяснения обстоятельств

--if not exists (select 1 from sysindexes where [name] = 'IDX__UsrSegmentMember__SegmentId') begin
--PRINT '[Usr].[SegmentMember]: create index IDX__UsrSegmentMember__SegmentId'
--create index [IDX__UsrSegmentMember__SegmentId]
--  on [Usr].[SegmentMember] ([SegmentId])  
--end
--go


/****** Table [Usr].[UserSentry] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='UserSentry' and [uid] = SCHEMA_ID('Usr'))
create table [Usr].[UserSentry] (
  [UserId] uniqueidentifier NOT NULL PRIMARY KEY,
  constraint [FK__UsrUserSentry__UsrUser] 
    foreign key  ([UserId])
	references [Usr].[User] ([Id])
	on delete cascade
)
go

/****** Table [Usr].[Comment] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Comment' and [uid] = SCHEMA_ID('Usr'))
create table [Usr].[Comment] (
  [UserId] uniqueidentifier NOT NULL,
  [UponUserId] uniqueidentifier NOT NULL,
  [Text] nvarchar(4000) NOT NULL,
  CONSTRAINT [PK__UsrComment] PRIMARY KEY ([UserId], [UponUserId]),
  CONSTRAINT [FK__UsrComment__UsrUser_1] 
    FOREIGN KEY ([UserId])
	references [Usr].[User] ([Id]),
  CONSTRAINT [FK__UsrComment__UsrUser_2] 
    FOREIGN KEY ([UponUserId])
	references [Usr].[User] ([Id]),
  CONSTRAINT [CHK_UsrComment__UserId_and_UponUserId] 
    check ([UserId] <> [UponUserId])
)
go

/****** Table [Usr].[MsgAddressee] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MsgAddressee' and [uid] = SCHEMA_ID('Usr'))
create table [Usr].[MsgAddressee] (
  [UserId] uniqueidentifier NOT NULL,
  [TransportTypeId] int NOT NULL,
  [AddrUserId] uniqueidentifier NOT NULL,
  constraint [PK__UsrMsgAddressee] primary key ([UserId], [TransportTypeId]),
  constraint [FK__UsrMsgAddressee__UsrTransport] 
    foreign key ([UserId], [TransportTypeId])
	references [Usr].[Transport] ([UserId], [TransportTypeId])
	on delete cascade,
  constraint [FK_UsrMsgAddressee__AddrUserId]
    foreign key ([AddrUserId])
	references [Usr].[User] ([Id])	
)
go

/****** Table [Usr].[BroadcastProhibition] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='BroadcastProhibition' and [uid] = SCHEMA_ID('Usr'))
create table [Usr].[BroadcastProhibition] (
  [UserId] uniqueidentifier NOT NULL,
  [ProhibitionUserId] uniqueidentifier NOT NULL,
  constraint [PK__UsrBroadcastProhibition] primary key ([UserId], [ProhibitionUserId]),
  constraint [FK__UsrBroadcastProhibition__UserId]
    foreign key ([UserId])
	references [Usr].[User] ([Id])
	on delete cascade,
  constraint [FK__UsrBroadcastProhibition__ProhibitionUserId]
    foreign key ([ProhibitionUserId])
	references [Usr].[User] ([Id])
	--on delete cascade
)
go

/****** Table [Usr].[UserDeepChilds] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='UserDeepChilds' and [uid] = SCHEMA_ID('Usr'))
create table [Usr].[UserDeepChilds] (
  [UserId] uniqueidentifier NOT NULL,
  [ChildUserId] uniqueidentifier NOT NULL,
  constraint [PK__Usr_UserDeepChilds] primary key ([UserId], [ChildUserId]),
  constraint [FK__Usr_UserDeepChilds__UserId]
    foreign key ([UserId])
	references [Usr].[User] ([Id]),
  constraint [FK__Usr_UserDeepChilds_ChildUserId]
    foreign key ([ChildUserId])
	references [Usr].[User] ([Id])
	on delete cascade,
)
go

-- index on [Usr].[UserDeepChilds]: used in procedure [Usr].[UserDeepChilds]
if not exists (select 1 from sysindexes where [name] = 'IDX__UsrUserDeepChilds__UserId') begin
PRINT '[Usr].[UserDeepChilds]: create index IDX__UsrUserDeepChilds__UserId'
create  index [IDX__UsrUserDeepChilds__UserId]
  on [Usr].[UserDeepChilds] ([UserId])  
end
go

/****** Table [Usr].[ScenarioProcess] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='ScenarioProcess' and [uid] = SCHEMA_ID('Usr'))
begin
CREATE TABLE [Usr].[ScenarioProcess] (
  [UserId] uniqueidentifier NOT NULL,
  [TransportTypeId] int NOT NULL,
--  [ScenarioId] integer NOT NULL,
  [ScenarioStepId] integer NOT NULL,
  CONSTRAINT [PK__UsrScenarioProcess] PRIMARY KEY ([UserId], [TransportTypeId]),
  CONSTRAINT [FK__UsrScenarioProcess__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id]),
  CONSTRAINT [FK_UsrScenarioProcess__CfgTransportType] foreign key ([TransportTypeId])  
    references [Cfg].[TransportType] ([Id]),
  CONSTRAINT [FK__UsrScenarioProcess__CfgScenarioStep] foreign key ([ScenarioStepId])
    references [Cfg].[ScenarioStep] ([Id])  
)
  PRINT 'CREATE TABLE [Usr].[ScenarioProcess]'
end
GO

/****** Table [Usr].[UserDataKey] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='UserDataKey' and [uid] = SCHEMA_ID('Usr'))
begin
CREATE TABLE [Usr].[UserDataKey] (
  [Id] bigint NOT NULL IDENTITY(1,1),
  [Key] nvarchar(255) NOT NULL,
  constraint [PK__UsrUserDataKey] primary key ([Id])
)
 PRINT 'CREATE TABLE [Usr].[UserDataKey]'
end

GO

if not exists(select 1 from sysindexes where [name] = 'UNQ__UserDataKey__Key')
CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UserDataKey__Key]
ON [Usr].[UserDataKey]([Key]);

GO

/****** Table [Usr].[UserData] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='UserData' and [uid] = SCHEMA_ID('Usr'))
begin
CREATE TABLE [Usr].[UserData] (
  [UserId] uniqueidentifier NOT NULL,
  [KeyId] bigint NOT NULL,
  [Data] nvarchar(max) NOT NULL,
  constraint [PK__UsrUserData] primary key ([UserId], [KeyId]),
  constraint [FK__UsrUserData__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id]),
  constraint [FK__UsrUserData__UsrUserDataKey] foreign key ([KeyId])
    references [Usr].[UserDataKey] ([Id])
)
 PRINT 'CREATE TABLE [Usr].[UserData]'
end
GO

/****** Table [Usr].[MatchedPhones] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MatchedPhones' and [uid] = SCHEMA_ID('Usr'))
begin
 create table [Usr].[MatchedPhones] (
  [UserId] uniqueidentifier NOT NULL,
  [AddrUserId] uniqueidentifier NOT NULL,
  constraint [PK__UsrMatchedPhones] primary key ([UserId], [AddrUserId]),
  constraint [FK__UsrMatchedPhones__UsrUser] foreign key ([UserId]) references [Usr].[User] ([Id])
	on delete cascade,
  constraint [FK__UsrMatchedPhones__AddrUserId] foreign key ([AddrUserId]) references [Usr].[User] ([Id])	
)
 PRINT 'create table [Usr].[MatchedPhones]'
end
go

/****** Table [Usr].[PersonalProhibition] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='PersonalProhibition' and [uid] = SCHEMA_ID('Usr'))
begin
 create table [Usr].[PersonalProhibition] (
  [UserId] uniqueidentifier NOT NULL,
  [ProhibitionUserId] uniqueidentifier NOT NULL,
  constraint [PK__UsrPersonalProhibition] primary key ([UserId], [ProhibitionUserId]),
  constraint [FK__UsrPersonalProhibition__UserId]
    foreign key ([UserId])
	references [Usr].[User] ([Id])
	on delete cascade,
  constraint [FK__UsrPersonalProhibition__ProhibitionUserId]
    foreign key ([ProhibitionUserId])
	references [Usr].[User] ([Id])
	--on delete cascade
 )
 PRINT 'create table [Usr].[PersonalProhibition]'
end
go

/********* 2021.02.06 ***********/
/********* Построение отчётов ***********/
/****** Table [Usr].[ReportKind] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='ReportKind' and [uid] = SCHEMA_ID('Usr'))
begin
CREATE TABLE [Usr].[ReportKind](
	[Id] [int] NOT NULL IDENTITY (1,1),
	[Code] [nvarchar](50) NOT NULL,
	[TemplateName] [nvarchar](500) NOT NULL,
	[SqlScripteName] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](250) NULL,
  constraint [PK__UsrReportKind] primary key([Id])
)
PRINT 'CREATE TABLE [Usr].[ReportKind]'
end

GO

if not exists(select 1 from sysindexes where [name] = 'UNQ__ReportKind__Code') 
begin
ALTER TABLE [Usr].[ReportKind] add constraint [UNQ__ReportKind__Code] unique ([Code])
--create unique index [UNQ__ReportKind__Code]	on [Usr].[ReportState] ([Code])
PRINT '[Usr].[ReportKind]: add index [UNQ__ReportKind__Code]';
end;
go

/****** Table [Usr].[ReportKindParam] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='ReportKindParam' and [uid] = SCHEMA_ID('Usr'))
begin
CREATE TABLE [Usr].[ReportKindParam](
	[Id] [int] NOT NULL IDENTITY (1,1),
	[ReportKindId] [int] NOT NULL,
	[ParamName] [nvarchar](500) NOT NULL,
	[Checkout] bit NOT NULL,
  constraint [PK__UsrReportKindParam] primary key([Id])
)
PRINT 'CREATE TABLE [Usr].[ReportKindParam]'
end
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__UsrReportKindParam__UsrReportKind' and [uid]=SCHEMA_ID('Usr'))
begin
alter table [Usr].[ReportKindParam] add constraint [FK__UsrReportKindParam__UsrReportKind]
  foreign key ([ReportKindId])  
  references [Usr].[ReportKind]([Id])
PRINT 'add constraint [FK__UsrReportKindParam__UsrReportKind]'
end
GO

/****** Table [Usr].[ReportState] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='ReportState' and [uid] = SCHEMA_ID('Usr'))
begin
CREATE TABLE [Usr].[ReportState](
	[Id] [int] NOT NULL IDENTITY (1,1),
	[Code] [int] NOT NULL,
	[Description] [nvarchar](250) NULL,
  constraint [PK__UsrReportState] primary key([Id])
)
PRINT 'CREATE TABLE [Usr].[ReportState]'
end

GO

if not exists(select 1 from sysindexes where [name] = 'UNQ__ReportState__Code') 
begin
ALTER TABLE [Usr].[ReportState] add constraint [UNQ__ReportState__Code] unique ([Code])
--create unique index [UNQ__ReportState__Code]	on [Usr].[ReportState] ([Code])
PRINT '[Usr].[ReportState]: add index [UNQ__ReportState__Code]';
end;
go

declare @dt as table (
--	[Id] [int] NOT NULL,
	[Code] [int] NOT NULL UNIQUE,
	[Description] [nvarchar](250) NULL
)

INSERT INTO @dt ([Code], [Description])
values 
(1, 'Затребован'),
(10, 'В работе'),
(20, 'Сформирован')

update t
set t.[Description] = d.[Description]
from [Usr].[ReportState] t
inner join @dt d on t.[Code] = d.[Code]
where t.[Description] <> d.[Description];

insert into [Usr].[ReportState]([Code], [Description])
select [Code], [Description] from @dt where [Code] not in (select [Code] from [Usr].[ReportState]);


/****** Table [Usr].[UserReport] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='UserReport' and [uid] = SCHEMA_ID('Usr'))
begin
CREATE TABLE [Usr].[UserReport](
	[Id] [int] NOT NULL IDENTITY (1,1),
	[ReportKindId] [int] NOT NULL,
	[UserId] uniqueidentifier NOT NULL,
	[ReportStateId] [int] NOT NULL,
	[RequestDate] datetime NOT NULL DEFAULT GETUTCDATE(),
	[StartDate] datetime NULL,
	[FinishDate] datetime NULL,
	[Error] nvarchar(MAX) NULL,
  constraint [PK__UsrReport] primary key([Id])
)
PRINT 'CREATE TABLE [Usr].[UserReport]'
end
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__UsrUserReport__UsrReportKind' and [uid]=SCHEMA_ID('Usr'))
begin
alter table [Usr].[UserReport] add constraint [FK__UsrUserReport__UsrReportKind]
  foreign key ([ReportKindId])  
  references [Usr].[ReportKind]([Id])
PRINT 'add constraint [FK__UsrUserReport__UsrReportKind]'
end
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__UsrUserReport__UsrUser' and [uid]=SCHEMA_ID('Usr'))
begin
alter table [Usr].[UserReport] add constraint [FK__UsrUserReport__UsrUser]
  foreign key ([UserId])  
  references [Usr].[User]([Id])
PRINT 'add constraint [FK__UsrUserReport__UsrUser]'
end
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__UsrUserReport__UsrReportState' and [uid]=SCHEMA_ID('Usr'))
begin
alter table [Usr].[UserReport] add constraint [FK__UsrUserReport__UsrReportState]
  foreign key ([ReportStateId])  
  references [Usr].[ReportState]([Id])
PRINT 'add constraint [FK__UsrUserReport__UsrReportState]'
end
GO

/****** Table [Usr].[UserReportParam] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='UserReportParam' and [uid] = SCHEMA_ID('Usr'))
begin
CREATE TABLE [Usr].[UserReportParam](
	[Id] [int] NOT NULL IDENTITY (1,1),
	[UserReportId] [int] NOT NULL,
	[ReportKindParamId] [int] NOT NULL,
	[ParamValue] [nvarchar](500) NOT NULL,
  constraint [PK__UsrUserReportParam] primary key([Id])
)
PRINT 'CREATE TABLE [Usr].[UserReportParam]'
end
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__UsrUserReportParam__UsrUserReport' and [uid]=SCHEMA_ID('Usr'))
begin
alter table [Usr].[UserReportParam] add constraint [FK__UsrUserReportParam__UsrUserReport]
  foreign key ([UserReportId])  
  references [Usr].[UserReport]([Id])
PRINT 'add constraint [FK__UsrUserReportParam__UsrUserReport]'
end
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__UsrUserReportParam__UsrReportKindParam' and [uid]=SCHEMA_ID('Usr'))
begin
alter table [Usr].[UserReportParam] add constraint [FK__UsrUserReportParam__UsrReportKindParam]
  foreign key ([ReportKindParamId])  
  references [Usr].[ReportKindParam]([Id])
PRINT 'add constraint [FK__UsrUserReportParam__UsrReportKindParam]'
end
GO

/****** Table [Usr].[ReportKindCfg] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='ReportKindCfg' and [uid] = SCHEMA_ID('Usr'))
begin
CREATE TABLE [Usr].[ReportKindCfg](
	[Id] [int] NOT NULL IDENTITY (1,1),
	[ReportKindId] [int] NOT NULL,
	[ParamName] [nvarchar](100) NOT NULL,
	[ParamValue] [nvarchar](max) NOT NULL,
  constraint [PK__UsrReportKindCfg] primary key([Id]),
  constraint [UNQ__UsrReportKindCfg__ReportKindId_ParamName] UNIQUE NONCLUSTERED ([ReportKindId], [ParamName])
)
PRINT 'CREATE TABLE [Usr].[ReportKindCfg]'
end

GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__ReportKindCfg__UsrReportKind' and [uid]=SCHEMA_ID('Usr'))
begin
alter table [Usr].[ReportKindCfg] add constraint [FK__ReportKindCfg__UsrReportKind]
  foreign key ([ReportKindId])  
  references [Usr].[ReportKind]([Id])
PRINT 'add constraint [FK__ReportKindCfg__UsrReportKind]'
end
GO


/*****************************************************************************
	[File] tables
*****************************************************************************/

/****** Table [File].[FileInfo] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='FileInfo' and [uid] = SCHEMA_ID('File'))
CREATE TABLE [File].[FileInfo](
	[Id] uniqueidentifier NOT NULL DEFAULT NEWSEQUENTIALID(),
	[Idx] bigint NOT NULL IDENTITY(1,1),
	[FileOwnerId] [uniqueidentifier] NOT NULL,
	[LoadDate] [datetime] NOT NULL DEFAULT GETUTCDATE(),
	[FileName] [nvarchar](4000) NULL,
	[MediaTypeId] [int] NOT NULL,
	[FileLength] [int] NOT NULL,
	[Width] int NULL,
	[Height] int NULL,
	CONSTRAINT [PK__FileFile__Id] PRIMARY KEY NONCLUSTERED ( [Id] ),
	CONSTRAINT [UNQ__FileFile__Idx] UNIQUE CLUSTERED ( [Idx] ),
	CONSTRAINT [FK__FileFile__UsrUser] 
	  FOREIGN KEY([FileOwnerId]) 
	  REFERENCES  [Usr].[User] ([Id]) 
	  ON DELETE CASCADE,
	CONSTRAINT [FK__FileFile__CfgMediaType] 
	  FOREIGN KEY([MediaTypeId]) 
	  REFERENCES  [Cfg].[MediaType] ([Id])
)
GO

/*****************************************************************************
		[Msg] tables
*****************************************************************************/

/****** Table [Msg].[Message] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Message' and [uid] = SCHEMA_ID('Msg'))
CREATE TABLE [Msg].[Message](
	[Id] uniqueidentifier NOT NULL DEFAULT newsequentialid(),
	[Idx] bigint NOT NULL IDENTITY(1,1),
	[PostTm] datetime NOT NULL DEFAULT getutcdate(),
	[MessageTypeId] int NOT NULL,
	[FromUserId] uniqueidentifier NOT NULL,
	[FromTransportTypeId] int NOT NULL,
	[AnswerToId] uniqueidentifier NULL,
	[Text] nvarchar(4000) NULL,
	[IsDeleted] bit NOT NULL DEFAULT 0,
	[Specific] nvarchar(500) NULL,
	[ForwardMsgId] uniqueidentifier NULL,
	[FileId] uniqueidentifier NULL,
	[NeedToChangeText] bit default 0 NOT NULL,
	[DelayedStart] datetime NULL,
	[DalayedCancelled] datetime NULL,
	[ScenarioStepId] integer NULL,
	[ResendMsgId] [uniqueidentifier] NULL,
	[ResendTime] [datetime] NULL,
    CONSTRAINT [PK__MsgMessage__Id] PRIMARY KEY NONCLUSTERED ([Id]),
	CONSTRAINT [UNQ__MsgMessage_Idx] UNIQUE CLUSTERED ([Idx]),
	CONSTRAINT [FK__MsgMessage__AnswerMessage] 
	  FOREIGN KEY([AnswerToId])
      REFERENCES [Msg].[Message] ([Id]),
    CONSTRAINT [FK__MsgMessage__CfgMessageType] 
	  FOREIGN KEY([MessageTypeId])
      REFERENCES [Cfg].[MessageType] ([Id]),
	CONSTRAINT [FK__MsgMessage__ForwardMsgId] 
	  FOREIGN KEY([ForwardMsgId])
      REFERENCES [Msg].[Message] ([Id]),
	CONSTRAINT [FK__MsgMessage__FromTransport] 
	  FOREIGN KEY([FromUserId], [FromTransportTypeId])
	  REFERENCES [Usr].[Transport] ([UserId], [TransportTypeId]),
	constraint [FK__MsgMesage__FileInfoFile]
		foreign key ([FileId])
		references [File].[FileInfo] ([Id])
		on delete set null,
	constraint [CHK_MsgMessage_DelayedCancelled] 
		check (([DalayedCancelled] is not null and [DelayedStart] is not null)
		     or [DalayedCancelled] is null)
) ON [PRIMARY]
GO

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[Message]')
	and [name] = 'DelayedStart')
alter table [Msg].[Message]
add [DelayedStart] datetime NULL
go

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[Message]')
	and [name] = 'DalayedCancelled')
alter table [Msg].[Message]
add [DalayedCancelled] datetime NULL
go

if not exists(select 1 from sysobjects where xtype = 'C'
	and [name] = 'CHK_MsgMessage_DelayedCancelled' and [uid]=SCHEMA_ID('Msg'))
alter table [Msg].[Message]
add constraint [CHK_MsgMessage_DelayedCancelled] 
	check (([DalayedCancelled] is not null and [DelayedStart] is not null)
		 or [DalayedCancelled] is null)
go

if not exists(select 1 from sysindexes where [name] = 'IDX__MsgMessage__Stats') begin
PRINT '[Msg].[Message]: create index [IDX__MsgMessage__Stats]'

CREATE NONCLUSTERED INDEX [IDX__MsgMessage__Stats]
ON [Msg].[Message] ([FromUserId],[FromTransportTypeId],[MessageTypeId])
INCLUDE ([Id],[Idx])
END
GO

/****** Indexes for Table [Msg].[Message] ******/
if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[Message]')
	and [name] = 'ScenarioStepId')
begin
  alter table [Msg].[Message]  
  add [ScenarioStepId] integer NULL
  PRINT '[Msg].[Message] : add [ScenarioStepId] integer NULL'
end
go

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__MsgMessage__CfgScenarioStep' and [uid]=SCHEMA_ID('Msg'))
begin
  alter table [Msg].[Message] add constraint [FK__MsgMessage__CfgScenarioStep]
  foreign key ([ScenarioStepId])  
  references [Cfg].[ScenarioStep]([Id])
  PRINT '[Msg].[Message] : add constraint [FK__MsgMessage__CfgScenarioStep]'
end
GO

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[Message]')
	and [name] = 'ResendMsgId')
begin
  alter table [Msg].[Message]  
  add [ResendMsgId] [uniqueidentifier] NULL
  PRINT '[Msg].[Message] : add [ResendMsgId] [uniqueidentifier] NULL'
end
go

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[Message]')
	and [name] = 'ResendTime')
begin
  alter table [Msg].[Message]  
  add [ResendTime] [datetime] NULL
  PRINT '[Msg].[Message] : add [ResendTime] [datetime] NULL'
end
go

if not exists(select 1 from sysindexes where [name] = 'IDX__MsgMessage__IsDeleted_DalayedCancelled_PostTm') begin
CREATE NONCLUSTERED INDEX [IDX__MsgMessage__IsDeleted_DalayedCancelled_PostTm]
ON [Msg].[Message] ([MessageTypeId] ASC, [ResendMsgId] ASC, [ResendTime] ASC)
INCLUDE ([Id],[MessageTypeId],[DelayedStart])
PRINT '[Msg].[Message]: create index [IDX__MsgMessage__IsDeleted_DalayedCancelled_PostTm]'
END
GO

if not exists(select 1 from sysindexes where [name] = 'IDX__MsgMessage__MessageTypeId_ResendMsgId_ResendTime') begin
CREATE NONCLUSTERED INDEX [IDX__MsgMessage__MessageTypeId_ResendMsgId_ResendTime]
ON [Msg].[Message] ([IsDeleted] ASC, [DalayedCancelled] ASC, [PostTm])
INCLUDE ([Id],[FromUserId])
PRINT '[Msg].[Message]: create index [IDX__MsgMessage__MessageTypeId_ResendMsgId_ResendTime]'
END
GO

if not exists(select 1 from sysindexes where [name] = 'IDX__MsgMessage__ResendMsgId') begin
CREATE NONCLUSTERED INDEX [IDX__MsgMessage__ResendMsgId]
ON [Msg].[Message] ([ResendMsgId])
INCLUDE ([Id],[FromUserId])
PRINT '[Msg].[Message]: create index [IDX__MsgMessage__ResendMsgId]'
END
GO


/********* 2021.07.10 ***********/
/*** Индекс для Функции выбора неотправленных сообщений ***/

if not exists(select 1 from sysindexes where [name] = 'IDX__MsgMessage__PostTm') 
begin
PRINT '[Msg].[Message]: create index [IDX__MsgMessage__PostTm]'
CREATE NONCLUSTERED INDEX [IDX__MsgMessage__PostTm]
ON [Msg].[Message] ([PostTm] DESC)
END
GO

/****** Table [Msg].[MessageToSelection] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MessageToSelection' and [uid] = SCHEMA_ID('Msg'))
create table [Msg].[MessageToSelection] (
  [Idx] bigint NOT NULL IDENTITY(1,1),
  [MsgId] uniqueidentifier NOT NULL,
  [UserId] uniqueidentifier NOT NULL,
  [WithStructure] bit NOT NULL,
  [Include] bit NOT NULL,
  [StructureDeep] int,
  constraint [PK__MsgMessageToSelection_Idx] primary key clustered ([Idx]),
  constraint [FK__MsgMessageToSelection__MsgMessage] 
    foreign key ([MsgId])
	references [Msg].[Message] ([Id])
	on delete cascade,
  constraint [FK__MsgMessageToSelection__UsrUser]
    foreign key ([UserId])
	references [Usr].[User]([Id])
	on delete cascade
)
go

/****** Table [Msg].[MessageToSegment] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MessageToSegment' and [uid] = SCHEMA_ID('Msg'))
CREATE TABLE [Msg].[MessageToSegment](
    [Idx] bigint NOT NULL IDENTITY(1,1),
	[MsgId] uniqueidentifier NOT NULL,
	[SegmentId] uniqueidentifier NOT NULL,
	[MaxDeep] int NULL,
    CONSTRAINT [PK__MsgMessageToSegment] PRIMARY KEY NONCLUSTERED (
	    [MsgId], [SegmentId]),
	CONSTRAINT [UNQ__MsgMessageToSegment__Idx] UNIQUE CLUSTERED ([Idx]),
	CONSTRAINT [FK__MsgMessageToSegment__MsgMessage] 
		FOREIGN KEY([MsgId])
		REFERENCES [Msg].[Message] ([Id])
		ON DELETE CASCADE,
	CONSTRAINT [FK__MsgMessageToSegment__UsrSegment] 
		FOREIGN KEY([SegmentId])
		REFERENCES [Usr].[Segment] ([Id])
) ON [PRIMARY]
GO

/****** Table [Msg].[MessageToUser] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MessageToUser' and [uid] = SCHEMA_ID('Msg'))
CREATE TABLE [Msg].[MessageToUser](   
	[MsgId] uniqueidentifier NOT NULL,
	[ToUserId] uniqueidentifier NOT NULL,
	[ToTransportTypeId] int NOT NULL,
	[Idx] bigint NOT NULL IDENTITY(1,1),
	[IsFailed] bit NOT NULL DEFAULT 0,
	[IsSent] bit NOT NULL DEFAULT 0,
	[IsDelivered] bit NOT NULL DEFAULT 0,
	[IsRead] bit NOT NULL DEFAULT 0,
	[IsWebChatGreeting] bit NOT NULL default 0,
	[PushMark] [bit] NOT NULL DEFAULT 0,
	[TransportProviderId] [uniqueidentifier] NULL,
	CONSTRAINT [PK__MsgMessageToUser] PRIMARY KEY NONCLUSTERED (
		[MsgId], [ToUserId], [ToTransportTypeId]),
	CONSTRAINT [UNQ_MsgMessageToUser_Idx] UNIQUE CLUSTERED ([Idx]),
	CONSTRAINT [FK__MsgMessageToUser__MsgMessage] 
		FOREIGN KEY([MsgId])
		REFERENCES [Msg].[Message] ([Id])
		ON DELETE CASCADE,
	CONSTRAINT [FK__MsgMessageToUser__UsrTransport] 
		FOREIGN KEY([ToUserId], [ToTransportTypeId])
		REFERENCES [Usr].[Transport] ([UserId], [TransportTypeId])
		ON DELETE CASCADE,
	CONSTRAINT [CHK_MessageToUser_FLChat] 
		CHECK (([ToTransportTypeId]=(0) AND [IsFailed]=(0) AND [IsSent]=(1) OR [ToTransportTypeId]<>(0)))
) ON [PRIMARY]
GO

if not exists(select * from sysindexes where [name] = 'Idx__MsgMessageToUser__ToUserId') BEGIN
PRINT '[Msg].[MessageToUser]: add index [Idx__MsgMessageToUser__ToUserId]'
CREATE NONCLUSTERED INDEX [Idx__MsgMessageToUser__ToUserId]
ON [Msg].[MessageToUser] ([ToUserId])
END
GO

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[MessageToUser]')
	and [name] = 'PushMark')
begin
  PRINT '[Msg].[MessageToUser] : add [PushMark]  bit NOT NULL DEFAULT 0';
  alter table [Msg].[MessageToUser] add [PushMark]  bit NOT NULL DEFAULT 0;
end
go

--update [Msg].[MessageToUser] set [PushMark] = 1;
go

IF NOT EXISTS (SELECT name FROM sys.indexes  
    WHERE name = N'Idx__MsgMessageToUser__PushMark'  
    AND object_id = OBJECT_ID (N'[Msg].[MessageToUser]'))  
BEGIN
  PRINT '[Msg].[MessageToUser]: add index [Idx__MsgMessageToUser__PushMark]';
  CREATE NONCLUSTERED INDEX [Idx__MsgMessageToUser__PushMark]
    ON [Msg].[MessageToUser] ([PushMark])
    WHERE ([PushMark]=(0)) ;  
END
GO 

/****** 05.04.2021 ******/
if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[MessageToUser]') 
	and [name] = 'TransportProviderId') 
begin
 alter table [Msg].[MessageToUser]  add [TransportProviderId] uniqueidentifier NULL
 PRINT '[Msg].[MessageToUser]: add column [TransportProviderId]'
end
go

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__MsgMessageToUser__CfgTransportProvider' and [uid]=SCHEMA_ID('Msg'))
begin
alter table [Msg].[MessageToUser] add constraint [FK__MsgMessageToUser__CfgTransportProvider]
  foreign key ([TransportProviderId])  references [Cfg].[TransportProvider]([Id])
PRINT '[Msg].[MessageToUser] add constraint [FK__MsgMessageToUser__CfgTransportProvider]'
end
GO

/****** 20.07.2021 ******/
if not exists(select 1 from sysindexes where [name] = 'Idx_MessageToUser_TT_Failed_Send') 
begin
CREATE NONCLUSTERED INDEX [Idx_MessageToUser_TT_Failed_Send]
ON [Msg].[MessageToUser] ([ToTransportTypeId],[IsFailed],[IsSent])
INCLUDE ([MsgId],[ToUserId],[IsDelivered],[IsRead],[IsWebChatGreeting],[PushMark],[TransportProviderId])
PRINT '[Msg].[MessageToUser]: create index [Idx_MessageToUser_TT_Failed_Send]'
END
GO
 

/****** Table [Msg].[WebChatDeepLink] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='WebChatDeepLink' and [uid] = SCHEMA_ID('Msg'))
create table [Msg].[WebChatDeepLink] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [MsgId] uniqueidentifier NOT NULL,
  [ToUserId] uniqueidentifier NOT NULL,
  [ToTransportTypeId] int NOT NULL DEFAULT 100,
  [Link] nvarchar(100) NOT NULL,
  [ExpireDate] datetime NOT NULL,
  [SentTo] [int] NULL,
  [ViberStatus] [int] NULL,
  [SmsStatus] [int] NULL,
  [UpdatedTime] [datetime] NULL,
  [WebFormRequested] [bit] NOT NULL DEFAULT 0,
  [IsFinished] [bit] NOT NULL DEFAULT 0,
  CONSTRAINT [FK__MsgWebChatDeepLink__MsgMessageToUser] 
	FOREIGN KEY([MsgId], [ToUserId], [ToTransportTypeId])
	REFERENCES [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId])
	ON DELETE CASCADE
	ON UPDATE CASCADE,
  constraint [UNQ__MsgWebChatDeepLink] unique ([Link]),
  constraint [CHK_WebChatDeepLink_ToTransportTypeId] check ([ToTransportTypeId] = 100),
  CONSTRAINT [FK__MsgWebChatDeepLink__CfgTransportType] FOREIGN KEY([SentTo])
	REFERENCES [Cfg].[TransportType] ([Id])
)
go

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[WebChatDeepLink]')
	and [name] = 'SentTo')
begin
  PRINT '[Msg].[WebChatDeepLink] : add [SentTo] int NULL'
  alter table [Msg].[WebChatDeepLink] add [SentTo] int NULL
end
go


if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__MsgWebChatDeepLink__CfgTransportType' and [uid]=SCHEMA_ID('Msg'))
begin
  PRINT '[Msg].[WebChatDeepLink] : add constraint [FK__MsgWebChatDeepLink__CfgTransportType]'
  alter table [Msg].[WebChatDeepLink] add constraint [FK__MsgWebChatDeepLink__CfgTransportType]
  foreign key ([SentTo])  
  references [Cfg].[TransportType]([Id])
end
GO

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[WebChatDeepLink]')
	and [name] = 'ViberStatus')
begin
  PRINT '[Msg].[WebChatDeepLink] : add [ViberStatus] int NULL'
  alter table [Msg].[WebChatDeepLink] add [ViberStatus] int NULL
end
go

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[WebChatDeepLink]')
	and [name] = 'SmsStatus')
begin
  PRINT '[Msg].[WebChatDeepLink] : add [SmsStatus] int NULL'
  alter table [Msg].[WebChatDeepLink] add [SmsStatus] int NULL
end
go

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[WebChatDeepLink]')
	and [name] = 'UpdatedTime')
begin
  PRINT '[Msg].[WebChatDeepLink] : add [UpdatedTime] datetime NULL'
  alter table [Msg].[WebChatDeepLink] add [UpdatedTime] datetime NULL
end
go

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[WebChatDeepLink]')
	and [name] = 'WebFormRequested')
begin
  PRINT '[Msg].[WebChatDeepLink] : add [WebFormRequested] bit default 0 NOT NULL'
  alter table [Msg].[WebChatDeepLink] add [WebFormRequested] bit default 0 NOT NULL
end
go

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[WebChatDeepLink]')
	and [name] = 'IsFinished')
begin
  PRINT '[Msg].[WebChatDeepLink] : add [IsFinished] bit default 0 NOT NULL'
  alter table [Msg].[WebChatDeepLink] add [IsFinished] bit default 0 NOT NULL
  update [Msg].[WebChatDeepLink] set [IsFinished] = 1
end
go

if not exists(select 1 from sysindexes where [name] = 'IDX__MsgWebChatDeepLink__MsgId_ToUserId') 
begin
CREATE NONCLUSTERED INDEX [IDX__MsgWebChatDeepLink__MsgId_ToUserId]
ON [Msg].[WebChatDeepLink] ([MsgId] ASC, [ToUserId] ASC)
INCLUDE ([ViberStatus])
PRINT '[Msg].[WebChatDeepLink]: create index [IDX__MsgWebChatDeepLink__MsgId_ToUserId]'
END
GO


/****** Table [Msg].[WebChatAccepted] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='WebChatAccepted' and [uid] = SCHEMA_ID('Msg'))
create table [Msg].[WebChatAccepted] (
  [WebChatId] bigint NOT NULL,
  [TransportTypeId] int NOT NULL,
  constraint [PK__MsgWebChatAccepted] primary key ([WebChatId], [TransportTypeId]),
  constraint [FK__MsgWebChatAccepted__MsgWebChatDeepLink]
    foreign key ([WebChatId])
	references [Msg].[WebChatDeepLink] ([Id])
	on delete cascade,
  constraint [FK__MsgWebChatAccepted__CfgTransportTypeId]
    foreign key ([TransportTypeId])
	references [Cfg].[TransportType]([Id])
)
go

/****** Table [Msg].[Event] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Event' and [uid] = SCHEMA_ID('Msg'))
CREATE TABLE [Msg].[Event] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [Tm] datetime NOT NULL DEFAULT GETUTCDATE(),
  [EventTypeId] integer NOT NULL,  
  [CausedByUserId] uniqueidentifier NOT NULL,
  [CausedByUserTransportTypeId] integer null,
  [MsgId] uniqueidentifier NULL,
  constraint [FK__MsgEvent__CfgEventType] foreign key ([EventTypeId])
    references [Cfg].[EventType] ([Id]),
  constraint [FK__MsgEvent__UsrUserCaused] foreign key ([CausedByUserId])
    references [Usr].[User] ([Id]),
  constraint [FK__MsgEvent__MsgMessage] foreign key([MsgId])
	references [Msg].[Message] ([Id])
	on delete cascade,
  constraint [FK__MsgEvent__UsrTransport] 
    foreign key ([CausedByUserId], [CausedByUserTransportTypeId])
    references [Usr].[Transport] ([UserId], [TransportTypeId]),
  constraint [CHK__MsgEvent__MsgIdIsNotNull] 
	check ( ([MsgId] is not null and [EventTypeId] >= 1 and [EventTypeId] < 20)
         or ([MsgId] is null     and [EventTypeId] >= 20)
	     or ([EventTypeId] = 0) )
)
GO

/****** Table [Msg].[EventAddressee] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='EventAddressee' and [uid] = SCHEMA_ID('Msg'))
CREATE TABLE [Msg].[EventAddressee] (
  [Id] bigint NOT NULL,
  [UserId] uniqueidentifier NOT NULL,
  constraint [PK__MsgEventAddressee] primary key ([Id], [UserId]),
  constraint [FK__MsgEventAddressee__MsgEvent] foreign key([Id])
	references [Msg].[Event] ([Id])
	on delete cascade,
  constraint [FK__MsgEventAddressee__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id])
)
GO

if not exists(select 1 from sysindexes where [name] = 'IX_EventAddressee_User_Id') 
begin
--ALTER TABLE [Msg].[EventAddressee] add constraint [IX_EventAddressee_User_Id] NONCLUSTERED INDEX ([UserId],[Id])
CREATE NONCLUSTERED INDEX [IX_EventAddressee_User_Id] ON [Msg].[EventAddressee] ([UserId],[Id])
PRINT '[Msg].[EventAddressee]: add index [IX_EventAddressee_User_Id]';
end;
go


/****** Table [Msg].[EventDelivered] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='EventDelivered' and [uid] = SCHEMA_ID('Msg'))
CREATE TABLE [Msg].[EventDelivered] (
  [UserId] uniqueidentifier NOT NULL PRIMARY KEY,
  [LastEventId] bigint NOT NULL,
  constraint [FK__MsgEventDelivered__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id]),
  CONSTRAINT [FK__MsgEventDelivered__MsgEvent] FOREIGN KEY([LastEventId])
	REFERENCES [Msg].[Event] ([Id])
	ON DELETE CASCADE
)
GO

/****** Table [Msg].[MessageTransportId] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MessageTransportId' and [uid] = SCHEMA_ID('Msg'))
create table [Msg].[MessageTransportId] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [InsertedDate] datetime NOT NULL default GETUTCDATE(),
  [TransportId] nvarchar(255) NOT NULL,
  [MsgId] uniqueidentifier NOT NULL,
  [ToUserId] uniqueidentifier NULL,
  [TransportTypeId] int NOT NULL,
  [Index] tinyint NOT NULL default 0,
  [Count] tinyint NOT NULL default 1,
  [TransportProviderId] [uniqueidentifier] NULL,
  constraint [FK__MsgMessageTransportId__MsgMessage] 
    foreign key ([MsgId])
    references [Msg].[Message] ([Id]),
  constraint [FK__MsgMessageTransportId__MsgMessageToUser] 
    foreign key ([MsgId], [ToUserId], [TransportTypeId])
	references [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId]),
  constraint [FK_MsgMessageTransportId__CfgTransportType]
    foreign key ([TransportTypeId])
	references [Cfg].[TransportType]([Id]),
  constraint [UNQ__MsgMessageTransportId__Message] 
    unique (
		[MsgId], 
		[ToUserId], 
		[TransportTypeId],
		[Index]),
  constraint [UNQ__MsgMessageTransportId__Id]
    unique ([TransportId], [TransportTypeId]),
  constraint [CHK__MsgMessageTransportId__TransportTypeId] 
	check ([TransportTypeId] <> 0)
)
go

/****** 05.04.2021 ******/
if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[MessageTransportId]') 
	and [name] = 'TransportProviderId') 
begin
 alter table [Msg].[MessageTransportId]  add [TransportProviderId] uniqueidentifier NULL
 PRINT '[Msg].[MessageTransportId]: add column [TransportProviderId]'
end
go

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK__MsgMessageTransportId__CfgTransportProvider' and [uid]=SCHEMA_ID('Msg'))
begin
alter table [Msg].[MessageTransportId] add constraint [FK__MsgMessageTransportId__CfgTransportProvider]
  foreign key ([TransportProviderId])  references [Cfg].[TransportProvider]([Id])
PRINT '[Msg].[MessageTransportId] add constraint [FK__MsgMessageTransportId__CfgTransportProvider]'
end
GO



/****** Table [Msg].[MessageError] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MessageError' and [uid] = SCHEMA_ID('Msg'))
create table [Msg].[MessageError] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [InsertedDate] datetime NOT NULL default GETUTCDATE(),
  [MsgId] uniqueidentifier NULL,
  [ToUserId] uniqueidentifier NULL,
  [ToTransportTypeId] int NULL,
  [Type] nvarchar(255),
  [Descr] nvarchar(4000),
  [Trace] nvarchar(max),
  [Code] [nvarchar](100) NULL,
  CONSTRAINT [FK__MsgMessageError__MsgMessage] 
	FOREIGN KEY([MsgId])
	REFERENCES [Msg].[Message] ([Id])
	ON DELETE CASCADE,
  constraint [FK__MsgMessageError__MsgMessageToUser] 
    foreign key ([MsgId], [ToUserId], [ToTransportTypeId])
	references [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId])
)
go

/****** 05.04.2021 ******/
if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[MessageError]') 
	and [name] = 'Code') 
begin
 alter table [Msg].[MessageError]  add [Code] nvarchar(100) NULL
 PRINT '[Msg].[MessageError]: add column [Code]'
end
go


/****** 2020.11.16 ******/
/****** Доп. данные к сообщениям ******/
/****** Table [Usr].[UserDataKey] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MessageDataKey' and [uid] = SCHEMA_ID('Msg'))
begin
CREATE TABLE [Msg].[MessageDataKey] (
  [Id] bigint NOT NULL IDENTITY(1,1),
  [Key] nvarchar(255) NOT NULL,
  constraint [PK__MsgMessageDataKey] primary key ([Id])
)
 PRINT 'CREATE TABLE [Msg].[MessageDataKey]'
end

GO

if not exists(select 1 from sysindexes where [name] = 'UNQ__MessageDataKey__Key')
CREATE UNIQUE NONCLUSTERED INDEX [UNQ__MessageDataKey__Key]
ON [Msg].[MessageDataKey]([Key]);

GO

/****** Table [Msg].[MessageData] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MessageData' and [uid] = SCHEMA_ID('Msg'))
begin
CREATE TABLE [Msg].[MessageData] (
  [MessageId] uniqueidentifier NOT NULL,
  [KeyId] bigint NOT NULL,
  [Data] nvarchar(max) NOT NULL,
  constraint [PK__MsgMessageData] primary key ([MessageId], [KeyId]),
  constraint [FK__MsgMessageData__MsgMessage] foreign key ([MessageId])
    references [Msg].[Message] ([Id]),
  constraint [FK__MsgMessageData__MsgMessageDataKey] foreign key ([KeyId])
    references [Msg].[MessageDataKey] ([Id])
)
 PRINT 'CREATE TABLE [Msg].[MessageData]'
end
GO

/********* 2021.02.06 ***********/
/********* Построение отчётов ***********/
/****** Table [Msg].[MessageStatus] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MessageStatus' and [uid] = SCHEMA_ID('Msg'))
begin
CREATE TABLE [Msg].[MessageStatus](
	[Id] [int] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Alias] [nvarchar](50) NOT NULL,
	[Severity] [int] NOT NULL,
	[Description] [nvarchar](250) NULL
)

 PRINT 'CREATE TABLE [Msg].[MessageStatus]'
end
GO

declare @dt as table (
	[Id] [int] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Alias] [nvarchar](50) NOT NULL,
	[Severity] [int] NOT NULL,
	[Description] [nvarchar](250) NULL
)

Insert into @dt
([Id], [Alias], [Code], [Severity], [Description]) values
(0,	'TransportNotFound','transport_not_found',	10, NULL)
,(1,	'IsFailed',	'failed',	100, N'ошибка')
,(2,	'IsDeleted',	'deleted',	200, NULL)
,(3,	'IsQueued',	'queued',	50, NULL)
,(4,	'IsSent',	'sent',	300, N'отправлено')
,(5,	'IsDelivered',	'delivered',	500, N'доставлено')
,(6,	'IsNotDelivered','not_delivered',500, N'не доставлено')
,(7,	'IsCancelled',	'cancelled',	200, NULL)
,(8,	'IsRead',	'read',	1000, N'прочитано')
,(9,	'IsNoDataFound','info_not_found',200, N'данные отсутствуют')


update t
set t.[Id] = d.[Id], t.[Alias] = d.[Alias], t.[Severity] = d.[Severity], t.[Description] = d.[Description]
from [Msg].[MessageStatus] t
inner join @dt d on t.[Code] = d.[Code]
where t.[Id] <> d.[Id] 
   or ISNULL (t.[Alias], '') <> d.[Alias]
   or t.[Severity] <> d.[Severity]
   or ISNULL ( t.[Description], '') <> d.[Description]


insert into [Msg].[MessageStatus] ([Id], [Alias], [Code], [Severity], [Description])
select [Id], [Alias], [Code], [Severity], [Description] 
from @dt where [Code] not in (select [Code] from [Msg].[MessageStatus]);

--select * from [Msg].[MessageStatus]
GO


/*******************************************************************
	[Auth] tables
*******************************************************************/

/****** Table [Auth].[SmsCode] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='SmsCode' and [uid] = SCHEMA_ID('Auth'))
create table [Auth].[SmsCode] (
  [UserId] uniqueidentifier NOT NULL PRIMARY KEY,
  [Code] integer NOT NULL,
  [IssueDate] datetime NOT NULL DEFAULT getdate(),
  [ExpireBySec] integer NOT NULL,
  constraint [FK__AuthSmsCode__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id])
)
GO

/****** Table [Auth].[SmsCode] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='AuthToken' and [uid] = SCHEMA_ID('Auth'))
create table [Auth].[AuthToken] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [UserId] uniqueidentifier NOT NULL,
  [Token] varchar(255) NOT NULL UNIQUE,
  [IssueDate] datetime NOT NULL DEFAULT getdate(),
  [ExpireBy] integer NOT NULL,
  constraint [FK__AuthToken__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id])
)
GO

/****** Table [Auth].[PushNotificationToken] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='PushNotificationToken' and [uid] = SCHEMA_ID('Auth'))
create table [Auth].[PushNotificationToken] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [UserId] uniqueidentifier NOT NULL,
  [Token] varchar(255) NOT NULL UNIQUE,
  [IssueDate] datetime NOT NULL DEFAULT GETUTCDATE(),
  [ExpireBy] integer NOT NULL,
  constraint [FK__PushNotificationToken__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id])
)
GO

/****************************************************************
	[Ui] tables
****************************************************************/

/****** Table [Ui].[StructureNode] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='StructureNode' and [uid] = SCHEMA_ID('Ui'))
create table [Ui].[StructureNode] 
(
  [Id] uniqueidentifier NOT NULL default newsequentialid(),
  [Name] nvarchar(255) NOT NULL,
  [ParentNodeId] uniqueidentifier NULL default ('00000000-0000-0000-0000-000000000000'),
  [IsShowSegments] bit NOT NULL default 0,
  [IsShowParentUsers] bit NOT NULL default 0,
  [Order] smallint NOT NULL default 0,
  constraint [PK__UiStructureNode] primary key ([Id]),
  constraint [FK__UiStructureNode__UiStructureNode] foreign key ([ParentNodeId])
    references [Ui].[StructureNode]([Id]),
  constraint [CHK__Ui_StructureNode__ParentNodeId] check 
	([ParentNodeId] is not null or [Id] = '00000000-0000-0000-0000-000000000000')
)
go

/****** Table [Ui].[StructureNodeSegment] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='StructureNodeSegment' and [uid] = SCHEMA_ID('Ui'))
create table [Ui].[StructureNodeSegment]
(
  [NodeId] uniqueidentifier NOT NULL,
  [SegmentId] uniqueidentifier NOT NULL,
  [Order] smallint NOT NULL default 0,
  [IsBelongToNode] bit NOT NULL default 1,
  constraint [PK__UiStructureNodeSegment] primary key ([NodeId], [SegmentId]),
  constraint [FK__UiStructureNodeSegment__UsrSegment] foreign key ([SegmentId])
    references [Usr].[Segment]([Id])
	on delete cascade,
  constraint [FK__UiStructureNodeSegment__UiStructureNode] foreign key ([NodeId])
    references [Ui].[StructureNode] ([Id])
	on delete cascade
)
go

/****** Table [Ui].[StructureNodeParents] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='StructureNodeParents' and [uid] = SCHEMA_ID('Ui'))
create table [Ui].[StructureNodeParents] (
  [ParentNodeId] uniqueidentifier NOT NULL,
  [Name] nvarchar(255) NOT NULL,
  [Order] smallint NOT NULL,
  constraint [PK__StructureNodeParents] primary key ([ParentNodeId]),
  constraint [FK__StructureNodeParents] foreign key ([ParentNodeId])
    references [Ui].[StructureNode] ([Id])
	on delete cascade
)
go

/********************************************************************
	[Reg] tables
********************************************************************/

/****** Table [Reg].[LastUsedTransport] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='LastUsedTransport' and [uid] = SCHEMA_ID('Reg'))
create table [Reg].[LastUsedTransport] (
  [UserId] uniqueidentifier NOT NULL,
  [TransportTypeId] int NOT NULL,
  constraint [PK__RegLastTransport] primary key([UserId]),
  constraint [FK__RegLastTransport__UsrUser] 
    foreign key ([UserId])
	references [Usr].[User] ([Id])
	on delete cascade,
  constraint [FK__RegLastTransport__CfgTransportType] 
    foreign key ([TransportTypeId])
	references [Cfg].[TransportType] ([Id])
)
go


/********************************************************************
	[Cache] tables
********************************************************************/

/****** Table [Cache].[StructureNodeCount] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='StructureNodeCount' and [uid] = SCHEMA_ID('Cache'))
create table [Cache].[StructureNodeCount] (
  [NodeId] uniqueidentifier NOT NULL,
  [UserId] uniqueidentifier NOT NULL,
  [Count] int NOT NULL,
  constraint [PK__Cache_StructureNodeCount] primary key ([NodeId], [UserId]),
  constraint [FK__Cache_StructureNodeCount_NodeId] foreign key ([NodeId])
    references [Ui].[StructureNode] ([Id])
	on delete cascade,
  constraint [FK__Cache_StructureNodeCount_UserId] foreign key ([UserId])
    references [Usr].[User] ([Id])
	on delete cascade
)
go

-- index on [Cache].[StructureNodeCount]: used in function [Cache].[StructureNodeCount]
if not exists (select 1 from sysindexes where [name] = 'IDX__CacheStructureNodeCount__UserId_NodeId') begin
PRINT '[Cache].[StructureNodeCount]: create index IDX__CacheStructureNodeCount__UserId_NodeId'
create index [IDX__CacheStructureNodeCount__UserId_NodeId]
  on [Cache].[StructureNodeCount] ([UserId], [NodeId])  
end
go

/********************************************************************
		Views
********************************************************************/

/******* [dbo].[vwGetNewID] *******/
create or alter view [dbo].[vwGetNewID]
as
select newid() as new_id
go

/******* [Usr].[UserDefaultTransportView] *******/
CREATE OR ALTER view [Usr].[UserDefaultTransportView]
as
with [LUT] as (
  select lut.[UserId], lut.[TransportTypeId]
  from [Reg].[LastUsedTransport] lut
  inner join [Usr].[Transport] t 
    on lut.[UserId] = t.[UserId]
	and lut.[TransportTypeId] = t.TransportTypeId
	and t.[Enabled] = 1
),
[TT] as (
  select tt.[Id], tt.[Prior]
  from [Cfg].[TransportType] tt 
  where tt.[Enabled] = 1 and tt.[CanSelectAsDefault] = 1
),
[TOther] as (
  select t.[UserId], min(t.[TransportTypeId]) as [TransportTypeId]
  from [Usr].[Transport] t
  where t.[TransportTypeId] <> 0 and t.[TransportTypeId] < 100  
    and t.[Enabled] = 1
  group by t.[UserId]
)
select 
  u.[Id] as [UserId], 
  coalesce(
    tdef.[TransportTypeId], 
    tfl.[TransportTypeId], 
	lut.[TransportTypeId], 
	t.[TransportTypeId], 
	100) as [DefaultTransportTypeId]
from [Usr].[User] u
left join [Usr].[Transport] tdef 
  on u.[DefaultTransportTypeId] is not null
  and u.[Id] = tdef.[UserId] and tdef.[Enabled] = 1
  and u.[DefaultTransportTypeId] = tdef.[TransportTypeId]  
  --and tdef.TransportTypeId in (select [Id] from [TT])
left join [Usr].[Transport] tfl 
  on u.[Id] = tfl.[UserId] and tfl.[Enabled] = 1
  and tfl.[TransportTypeId] = /**FLChat**/0
  --and tfl.TransportTypeId in (select [Id] from [TT])
left join [LUT] lut 
  on u.[Id] = lut.[UserId]  
  and lut.TransportTypeId in (select [Id] from [TT])
left join [TOther] t on u.[Id] = t.[UserId]
where u.[Enabled] = 1
GO

/******** [Usr].[UserMailingTransportView] *****/
CREATE OR ALTER VIEW [Usr].[UserMailingTransportView]
AS
SELECT DISTINCT u.Id AS UserId, t.TransportTypeId AS DefaultTransportTypeId
FROM            Usr.[User] AS u 
		INNER JOIN Usr.Transport AS t ON u.Id = t.UserId AND t.Enabled = 1 
		INNER JOIN Cfg.TransportType AS tt ON t.TransportTypeId = tt.Id AND tt.Enabled = 1
WHERE        (u.Enabled = 1) AND (t.TransportTypeId IN (151))
GO


/****** [Msg].[LastMessageView] ***********/
-- Edit date: 2020.02.26
create or alter view [Msg].[LastMessageView]
as
with [FromRN] as (
	select 
	  ROW_NUMBER() OVER (PARTITION BY m.[FromUserId], mtu.[ToUserId] ORDER BY m.[Idx] DESC) as [RN],
	  m.[FromUserId] as [UserId], 
	  mtu.[ToUserId] as [UserOppId], 
	  m.[Id] as [MsgId],
	  m.[Idx] as [MsgIdx],
	  mtu.[ToTransportTypeId],
	  mtu.[Idx] as [MsgToUserIdx]
	from [Msg].[Message] m
	inner join [Cfg].[TransportType] fromtt on fromtt.[Id] = m.[FromTransportTypeId]
	inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
	inner join [Cfg].[MessageType] mt on m.[MessageTypeId] = mt.[Id]
	where fromtt.[InnerTransport] = 1
	  and m.[IsDeleted] = 0
	  and mt.[ShowInHistory] = 1
	  and mtu.[IsWebChatGreeting] = 0
),
[From] as (
	select [UserId], [UserOppId], [MsgId], [MsgIdx], [ToTransportTypeId], [MsgToUserIdx]
	from [FromRN]
	where [RN] = 1
),
[ToRn] as (
	select
	  ROW_NUMBER() OVER (PARTITION BY m.[FromUserId], mtu.[ToUserId] ORDER BY m.[Idx] DESC) as [RN],
	  mtu.[ToUserId] as [UserId], 
	  m.[FromUserId] as [UserOppId], 
	  m.[Id] as [MsgId],
	  m.[Idx] as [MsgIdx],
	  mtu.[ToTransportTypeId],
	  mtu.[Idx] as [MsgToUserIdx],
	  mtu.[IsRead]
	from [Msg].[Message] m
	inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
	inner join [Cfg].[TransportType] tott on tott.[Id] = mtu.[ToTransportTypeId]
	inner join [Cfg].[MessageType] mt on m.[MessageTypeId] = mt.[Id]
	where tott.[InnerTransport] = 1
	  and m.[IsDeleted] = 0
	  and mt.[ShowInHistory] = 1
	  and mtu.[IsWebChatGreeting] = 0
),
[UnreadCnt] as (
	select [UserId], [UserOppId], count(*) as UnreadCnt
	from [ToRn]
	where [IsRead] = 0
	group by [UserId], [UserOppId]
),
[To] as (
	select [UserId], [UserOppId], [MsgId], [MsgIdx], [ToTransportTypeId], [MsgToUserIdx]
	from [ToRN]
	where [RN] = 1
),
[Union] as (
  select cast(0 as bit) as [Income], * from [From]
  union
  select cast(1 as bit) as [Income], * from [To]
),
[UnionRN] as (
  select 
    ROW_NUMBER() OVER (PARTITION BY [UserId], [UserOppId] ORDER BY [MsgIdx] DESC) as [RN],
	[Income],
	[UserId], 
	[UserOppId], 
	[MsgId],
	[MsgIdx],
	[ToTransportTypeId],
	[MsgToUserIdx]
  from [Union]
),
[LastMsg] as (
select
	t.[UserId], 
	t.[UserOppId], 
	t.[MsgId],
	t.[MsgIdx],
	t.[ToTransportTypeId],
	t.[Income],
	t.[MsgToUserIdx]
from [UnionRN] t
inner join [Usr].[User] u on t.[UserOppId] = u.[Id] and u.[Enabled] = 1
where t.[RN] = 1
)
select
	t.[UserId], 
	t.[UserOppId], 
	t.[MsgId],
	t.[MsgIdx],
	t.[ToTransportTypeId],
	t.[Income],
	t.[MsgToUserIdx],
	coalesce(u.[UnreadCnt], 0) as [UnreadCnt]
from [LastMsg] t
left join [UnreadCnt] u on t.[UserId] = u.[UserId] and t.[UserOppId] = u.[UserOppId]
GO


/******  [Msg].[MessageCountOverToday] **********/
create or alter view [Msg].[MessageCountOverToday]
as
select 
	m.[MessageTypeId], 
	m.[FromUserId], 
	--CONVERT(date, DATEADD(minute, DATEPART(TZoffset, SYSDATETIMEOFFSET()), m.[PostTm])),
	count(mtu.[Idx]) as [Count]
from [Msg].[Message] m
inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
where 
  CONVERT(date, DATEADD(minute, DATEPART(TZoffset, SYSDATETIMEOFFSET()), m.[PostTm])) 
  = CONVERT(date, GETDATE())
  and mtu.[IsWebChatGreeting] = 0
group by 
	m.[MessageTypeId], 
	m.[FromUserId]
GO

/******* [Msg].[MessageStatsRowsView] ******/
create or alter view [Msg].[MessageStatsRowsView]
as
with 
--select webchat code record
[WebChat] as (
  select 
    wcdl.[MsgId], 
	wcdl.[ToUserId], 
	count(wca.[TransportTypeId]) as [Accepted]
  from [Msg].[WebChatDeepLink] wcdl
  left join [Msg].[WebChatAccepted] wca on wcdl.[Id] = wca.[WebChatId]
  group by wcdl.[MsgId], wcdl.[ToUserId]
)
select 
  m.[Id] as [MsgId], --message id
  m.[Idx] as [MsgIdx],
  m.[MessageTypeId], -- message type
  m.[FromUserId], --from user id
  mtu.[ToUserId] as [ToUserId],
  mtu.[ToTransportTypeId] as [ToTransportTypeId],
  --m.[Idx], --message idx
  --m.[PostTm], --message posted time
  
  --m.[Text],	--message text
  --wcmsg.[Text],
  --m.[FileId], -- message file
  case when mtu.[ToTransportTypeId] = 100 then 1 else 0 end as [IsWebChat]
  --sum(case when mtu.[ToTransportTypeId] = 100 then 1 else 0 end),
  --count(distinct mtu.[ToUserId]) as [RecipientCount] --total count of recipients
  --, wcmsg.*
  , case when mtu.[IsFailed] = 1 then 1 else 0 end as [IsFailed]
  , case when mtu.[IsSent] = 1 then 1 else 0 end as [IsSent]
  , case when mtu.[Idx] is not null
          and mtu.[IsFailed] = 0 
          and mtu.[IsSent] = 0 
	 then 1 else 0 end as [IsQuequed]
  , case when mtu.[ToTransportTypeId] = 100 and mtu.[IsFailed] = 1 then 1 else 0 end as [CantSendToWebChat]
  , case when wc.[Accepted] > 0 then 1 else 0 end as [IsWebChatAccepted]
  , case when mtu.[ToTransportTypeId] = /**WebChat**/100 and mtu.[IsRead] = 1 then 1 else 0 end as [IsSmsUrlOpened]
from [Msg].[Message] m
left join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
left join [WebChat] wc on wc.[MsgId] = mtu.[MsgId]
                      and wc.[ToUserId] = mtu.[ToUserId]
					  and mtu.[ToTransportTypeId] = /**WebChat**/100
where 
  --m.[FromUserId] = 'DB9CD15F-4412-EA11-A2C3-DCF6A6FC5B19'--'41c8775b-f380-e911-a2c0-9f888bb5fde6'
  m.[FromTransportTypeId] = /**FLChat**/0 --only messages from flchat
  --and m.[MessageTypeId] in (2, 4)
  and mtu.[IsWebChatGreeting] = 0
/*group by 
  m.[Id], --message id
  m.[Idx], --message idx
  m.[PostTm], --message posted time
  m.[MessageTypeId], -- message type
  m.[Text],	--message text
  m.[FileId] -- message file*/
--order by m.[Idx] desc
GO

/******* [Msg].[MessageStatsGroupedView] ********/
create or alter view [Msg].[MessageStatsGroupedView]
as
select 
  [MsgId], --message id
  [MsgIdx],
  [MessageTypeId], --message type id
  [FromUserId], --message sender
  count (DISTINCT [ToUserId]) as [RecipientCount] --total count of recipients
  , sum([IsWebChat]) as [WebChatCount] --total messages to WebChat
  , sum([IsFailed]) as [FailedCount] --total failed messages
  , sum([IsSent]) as [SentCount] --total sent messages
  , sum([IsQuequed]) as [QuequedCount] --total quequed messages
  , sum([CantSendToWebChat]) as [CantSendToWebChatCount] --total messages can't be sent to webchat
  , sum([IsWebChatAccepted]) as [WebChatAcceptedCount] --total accepted web chat messages
  , sum([IsSmsUrlOpened]) as [SmsUrlOpenedCount] --total recipients who was opened sms url
from [Msg].[MessageStatsRowsView]
group by [MsgId], [MsgIdx], [MessageTypeId], [FromUserId]
GO

/****** [Msg].[TransportIdStatsNotFinishedView] ******/
CREATE OR ALTER VIEW [Msg].[TransportIdStatsNotFinishedView]
AS
SELECT [TransportId]
FROM  [Msg].[MessageTransportId] mt
		INNER JOIN [Msg].[WebChatDeepLink] wcdl on(wcdl.MsgId = mt.MsgId and wcdl.ToUserId = mt.ToUserId)		
WHERE  wcdl.IsFinished = 0
GO


/******************************************************************************
	Functions
******************************************************************************/

/****** [dbo].[RandomString] *******/
create or alter function [dbo].[RandomString](@Length int)
returns nvarchar(100)
as
begin
  declare @chars varchar(2048)
  declare @charsLength int;
  declare @result nvarchar(100);

  set @chars = 'abcdefghijkmnopqrstuvwxyz' +  --lower letters
               'ABCDEFGHIJKLMNPQRSTUVWXYZ' +      --upper letters
               '0123456789';       --number characters	
  set @charsLength = LEN(@chars);

  set @result = 
    (select top (@Length)
      substring(@chars, 1 + (number % @charsLength), 1) as [text()]       
     from master..spt_values
     where type = 'P'
     order by (select new_id from vwGetNewID) --instead of using newid()
     for xml path(''));

  return @result;
end
go 

/****** [Usr].[User_GetChilds] *******/
create or ALTER function [Usr].[User_GetChilds](
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE,
  [Deep] integer NOT NULL,
  [OwnerUserId] uniqueidentifier NOT NULL
)
as
begin
  --declare @tmp [dbo].[GuidList];
  declare @deep integer;
  set @deep = 1;

  insert into @ids ([UserId], [Deep], [OwnerUserId])
  --output inserted.[UserId] into @tmp
  select [Id], @deep, [OwnerUserId] 
  from [Usr].[User]   
  where ([Enabled] = 1 or @includeDeleted = 1) and [OwnerUserId] = @userId;

  while @deep < @maxDeep or @maxDeep is null
  begin
    insert into @ids ([UserId], [Deep], [OwnerUserId])
	select [Id], @deep + 1, [OwnerUserId] 
	from [Usr].[User] 
	where ([Enabled] = 1 or @includeDeleted = 1)
	  and [OwnerUserId] in (select [UserId] from @ids where [Deep] = @deep)
	  --and [Id] not in (select [UserId] from @ids)
	  ;

	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  RETURN
end;
GO

/******* [Usr].[User_GetChilds4] ********/
CREATE       function [Usr].[User_GetChilds4](
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS TABLE
as
RETURN
(
	WITH Rec AS (
		-- ancor member expression
		SELECT Id UserId, 1 deep, OwnerUserId FROM [Usr].[User]
		WHERE ([Enabled] = 1 or @includeDeleted = 1) AND [OwnerUserId] = @userId
		UNION ALL
		-- recursive member expression
		SELECT t.Id UserId, Rec.deep + 1 deep, t.OwnerUserId
		FROM [Usr].[User] t JOIN Rec ON Rec.UserId = t.OwnerUserId
		WHERE (t.[Enabled] = 1 or @includeDeleted = 1) AND t.[OwnerUserId] != @userId AND ABS(Rec.deep) < IsNull(@maxDeep,32767)
	)
	SELECT CAST(UserId as uniqueidentifier) as UserId
		, CAST(deep as integer) as deep
		, CAST(OwnerUserId as uniqueidentifier) as OwnerUserId
	FROM Rec
	--ORDER BY deep
	/* The ORDER BY clause is invalid in views, inline functions, derived tables, subqueries, and common table expressions, unless TOP, OFFSET or FOR XML is also specified. */
)
GO


/******* 2020.12.29 ********/
/******* [Usr].[User_GetChildsSimple] ********/
CREATE OR ALTER     function [Usr].[User_GetChildsSimple1](
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE
)
as
begin
  if (select [IsUseDeepChilds] from [Usr].[User] where [Id] = @userId) = 1 
    and @maxDeep is null
	and @includeDeleted = 0
  begin
    insert into @ids 
	select [ChildUserId] from [Usr].[UserDeepChilds] where [UserId] = @userId;
  end
  else 
  begin
    insert into @ids
	select [UserId] from [Usr].[User_GetChilds](@userId, @maxDeep, @includeDeleted);
  end

  RETURN
end;
GO

/******* [Usr].[User_GetChildsSimple2] ********/
CREATE OR ALTER function [Usr].[User_GetChildsSimple2](
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS TABLE
AS
RETURN
(
WITH Rec AS (
	-- ancor member expression
	SELECT Id UserId, 1 deep, OwnerUserId FROM [Usr].[User]
	WHERE Enabled = 1 AND [OwnerUserId] = @userId
	UNION ALL
	-- recursive member expression
	SELECT t.Id UserId, Rec.deep + 1 deep, t.OwnerUserId
	FROM [Usr].[User] t JOIN Rec ON Rec.UserId = t.OwnerUserId
	WHERE (t.[Enabled] = 1 or 0 = 1) AND t.[OwnerUserId] != @userId
)
-- start main query
select ChildUserId UserId from [Usr].[UserDeepChilds] where [UserId] = @userId
	and (select [IsUseDeepChilds] from [Usr].[User] where [Id] = @userId) = 1
    and @maxDeep is null and @includeDeleted = 0
UNION ALL
SELECT UserId FROM Rec
WHERE ABS(deep) <= IsNull(@maxDeep,32767)
	AND (select [IsUseDeepChilds] from [Usr].[User] where [Id] = @userId) = 0
--OPTION (MAXRECURSION 32767) -- no OPTION here!
)
GO

DROP FUNCTION IF EXISTS [Usr].[User_GetChildsSimple]
/******* [Usr].[User_GetChildsSimple] ********/
go

CREATE OR ALTER     function [Usr].[User_GetChildsSimple](
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS TABLE
AS
RETURN
(
--  Первый вариант
--  SELECT distinct UserId FROM [Usr].[User_GetChildsSimple1](@userId, @maxDeep, @includeDeleted) where UserId is not null
--  Второй вариант, ускоренный
  SELECT UserId FROM [Usr].[User_GetChildsSimple2](@userId, @maxDeep, @includeDeleted) --OPTION (MAXRECURSION 32767)

)
GO


/******* [Usr].[User_GetChildsMulti] ********/
create or alter function [Usr].[User_GetChildsMulti](
   @usersId [dbo].[GuidList] readonly,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL,
  [Deep] integer NOT NULL,
  [OwnerUserId] uniqueidentifier NOT NULL  
)
as
begin
  declare @deep integer;
  set @deep = 1;

  insert into @ids ([UserId], [Deep], [OwnerUserId])
  select DISTINCT u.[Id], @deep, o.[Guid]
  from [Usr].[User] u 
  inner join @usersId o on u.[OwnerUserId] = o.[Guid]
  where (u.[Enabled] = 1 or @includeDeleted = 1);

  while @deep < @maxDeep or @maxDeep is null
  begin
    insert into @ids ([UserId], [Deep], [OwnerUserId])
	select DISTINCT [Id], @deep + 1, [OwnerUserId]
	from [Usr].[User]
	where ([Enabled] = 1 or @includeDeleted = 1)
	  and [OwnerUserId] in (select [UserId] from @ids where [Deep] = @deep);

	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  RETURN
end;
GO

/****** [Usr].[User_GetChildsWoBP] *******/
-- ============================================
-- Get all childs without broadcast prohibition in @userId structure
-- Edited: 2020.02.10
-- ============================================
create or alter  function [Usr].[User_GetChildsWoBP](
   @userId uniqueidentifier,
   @currentUserId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE,
  [Deep] integer NOT NULL,
  [OwnerUserId] uniqueidentifier NOT NULL,
  [BP] bit NOT NULL
)
as
begin
	--declare @tmp [dbo].[GuidList];
	declare @deep integer;
	set @deep = 1;

	insert into @ids ([UserId], [Deep], [OwnerUserId], [BP])
	--output inserted.[UserId] into @tmp
	select u.[Id], @deep, u.[OwnerUserId], case when bp.[UserId] is not null then 1 else 0 end
	from [Usr].[User] u
	left join [Usr].[BroadcastProhibition] bp 
		on u.[Id] = bp.[ProhibitionUserId] and bp.[UserId] = @currentUserId
	where (u.[Enabled] = 1 or @includeDeleted = 1) and u.[OwnerUserId] = @userId;

	while @deep < @maxDeep or @maxDeep is null
	begin
		insert into @ids ([UserId], [Deep], [OwnerUserId], [BP])
		select u.[Id], @deep + 1, u.[OwnerUserId], case when bp.[UserId] is not null then 1 else 0 end
		from [Usr].[User] u
		left join [Usr].[BroadcastProhibition] bp 
			on u.[Id] = bp.[ProhibitionUserId] and bp.[UserId] = @currentUserId
		where (u.[Enabled] = 1 or @includeDeleted = 1)
			and u.[OwnerUserId] in 
				(select [UserId] from @ids where [Deep] = @deep
					and [BP] = 0)
	  --and [Id] not in (select [UserId] from @ids)
	  ;

	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  RETURN
end;
GO

/******** [Usr].[User_GetChildsExt] ***********/
CREATE OR ALTER function [Usr].[User_GetChildsExt](
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE,
  [Deep] integer NOT NULL,
  [OwnerUserId] uniqueidentifier NOT NULL
)
as
begin
  declare @cur [dbo].[GuidList];
  declare @prev [dbo].[GuidList];
  declare @deep integer;
  set @deep = 1;

  insert into @ids ([UserId], [Deep], [OwnerUserId])
  output inserted.[UserId] into @prev
  select [Id], @deep, [OwnerUserId] 
  from [Usr].[User]   
  where ([Enabled] = 1 or @includeDeleted = 1) and [OwnerUserId] = @userId;

  while @deep < @maxDeep or @maxDeep is null
  begin
    delete from @cur;

    insert into @ids ([UserId], [Deep], [OwnerUserId])
	output inserted.[UserId] into @cur
	select [Id], @deep + 1, [OwnerUserId] 
	from [Usr].[User] 
	where ([Enabled] = 1 or @includeDeleted = 1)
	  and [OwnerUserId] in (select [Guid] from @prev)
	  --(select [UserId] from @ids where [Deep] = @deep)	  
	  ;

	delete from @prev;
	insert into @prev ([Guid]) select [Guid] from @cur;

	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  RETURN
end;
GO


/******** [Usr].[User_GetParents] ***********/
create or alter function [Usr].[User_GetParents](
   @userId uniqueidentifier,
   @maxDeep integer = null)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE,
  [Deep] integer NOT NULL
)
as
begin
  --declare @ids [Usr].[UserIdDeep];
  declare @deep integer;
  set @deep = 1;
  declare @parent uniqueidentifier;

  set @parent = (select [OwnerUserId] from [Usr].[User] where [Id] = @userId);  

  while (@deep <= @maxDeep or @maxDeep is null) 
    and @parent is not null
	and NOT EXISTS(select 1 from @ids where [UserId] = @parent)
  begin
    insert into @ids ([UserId], [Deep]) values (@parent, -1 * @deep);
	
	set @deep = @deep + 1;
	set @parent = (select [OwnerUserId] from [Usr].[User] where [Id] = @parent);  
  end

  --return select * from @ids;

  RETURN
end;
go

/******** [Usr].[User_GetParents2] ***********/
CREATE function [Usr].[User_GetParents2](
   @userId uniqueidentifier,
   @maxDeep integer = null)
RETURNS TABLE 
AS
RETURN
(
	WITH Rec AS (
		-- ancor member expression
		SELECT OwnerUserId, 1 deep FROM [Usr].[User]
		WHERE [Id] = @userId
		UNION ALL
		-- recursive member expression
		SELECT t.OwnerUserId, Rec.deep + 1 deep
		FROM [Usr].[User] t JOIN Rec ON Rec.OwnerUserId = t.Id
		WHERE t.[Id] != @userId AND ABS(Rec.deep) < IsNull(@maxDeep,32767)
	)
	SELECT CAST(OwnerUserId as uniqueidentifier) as UserId
		, CAST(-1*deep as integer) as deep
	FROM Rec
	WHERE OwnerUserId IS NOT NULL
)
GO


/******** [Usr].[Segment_GetMembers] **********/
create or alter function [Usr].[Segment_GetMembers](
   @segmentId uniqueidentifier,
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
--RETURNS @ids TABLE (
--  [UserId] uniqueidentifier NOT NULL UNIQUE,
--  [Deep] integer NOT NULL
--)
RETURNS TABLE
as
return (  
  select 
    ids.[UserId]   
  from [Usr].[User_GetChildsSimple] (@userId, @maxDeep, @includeDeleted) ids
  inner join [Usr].[SegmentMember] sm on ids.[UserId] = sm.[UserId]
  where sm.[SegmentId] = @segmentId
);
GO


/******** [Usr].[Segments_GetMembers] **********/
create or alter function [Usr].[Segments_GetMembers](
   @userId uniqueidentifier,
   @segments [dbo].[GuidList] readonly,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
--RETURNS @ids TABLE (
--  [UserId] uniqueidentifier NOT NULL UNIQUE,
--  [Deep] integer NOT NULL
--)
RETURNS TABLE
as
return (  
  select 
    ids.[UserId]   
  from [Usr].[User_GetChildsSimple] (@userId, @maxDeep, @includeDeleted) ids
  inner join [Usr].[SegmentMember] sm on ids.[UserId] = sm.[UserId]
  inner join @segments s on sm.[SegmentId] = s.[Guid]
);
GO

/****** [Usr].[User_GetAddresseesForExternalTrans] *******/
create or alter function [Usr].[User_GetAddresseesForExternalTrans] (
  @userId uniqueidentifier)
returns table
as 
  return (
  with 
  [Owner] as (
    --select [OwnerUserId] as [UserId] 
	--from [Usr].[User] 
	--where [Id] = @userId and [OwnerUserId] is not null
    select top 1 p.[UserId] 
    from [Usr].[User_GetParents](@userId, default) p
    inner join [Usr].[Transport] t on p.[UserId] = t.[UserId]
                                  and t.[Enabled] = 1
		                          and t.[TransportTypeId] = /**FLChat**/0
    order by p.[Deep] desc
  ),
  [Msgs] as (
    select m.[FromUserId] as [UserId], max(m.[Idx]) as [Idx]
    from [Msg].[MessageToUser] mtu
    inner join [Msg].[Message] m on mtu.[MsgId] = m.[Id]
    where mtu.[ToUserId] = @userId
      and m.[FromUserId] not in ('00000000-0000-0000-0000-000000000000', @userId)
	  and m.[FromUserId] not in (select [UserId] from [Owner])
    group by m.[FromUserId]),
  [Data] as (
    select [UserId], 0 as [Order], 0 as [SubOrder] from [Owner]
    union 
    select [UserId] as [UserId], 1 as [Order], [Idx] as [SubOrder] from [Msgs] )
  select d.*
  from [Data] d
  inner join [Usr].[Transport] t on d.[UserId] = t.[UserId] 
                                and t.[Enabled] = 1 
								and [TransportTypeId] = /**FLChat**/0
  --order by [Order] asc, [SubOrder] desc
)
GO


/****** [Usr].[BroadcastProhibition_Structure]  ********/
create or alter function [Usr].[BroadcastProhibition_Structure] 
(
  @userId uniqueidentifier
)
returns @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE
) as
begin

  declare @bh [dbo].[GuidList];
  insert into @bh
  select [ProhibitionUserId] 
  from [Usr].[BroadcastProhibition]
  where [UserId] = @userId;

  insert into @ids ([UserId])
  select DISTINCT [UserId] 
  from [Usr].[User_GetChildsMulti](@bh, default, default);

  return
end
go

/********* [Usr].[BroadcastProhibition_StructureUpward1]  **********/
create or alter function [Usr].[BroadcastProhibition_StructureUpward1] 
(
  @userId uniqueidentifier,
  @users [dbo].[GuidList] readonly
)
returns @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE
) as
begin

  declare @curUserId uniqueidentifier;

  --broadcast prohibition structure results
  declare @bps table (
    [UserId] uniqueidentifier NOT NULL PRIMARY KEY,
	[BPS] bit NULL
  );

  declare @curList [dbo].[GuidList];
  declare @tmp uniqueidentifier;
  declare @val bit;

  --list of broadcast prohibition users
  declare @bp table ([UserId] uniqueidentifier NOT NULL PRIMARY KEY);
  
  --select all broadcast prohibitions users
  insert into @bp
  select [ProhibitionUserId] from [Usr].[BroadcastProhibition]
  where [UserId] = @userId;

  if @@ROWCOUNT = 0 return;

  ---enumerate every input user
  DECLARE ic CURSOR FORWARD_ONLY FAST_FORWARD
  FOR
    select DISTINCT [Guid] from @users;
  open ic;
  fetch next from ic into @curUserId;
  while @@FETCH_STATUS = 0
  begin
     
	set @val = (select [BPS] from @bps where [UserId] = @curUserId) 
	if @val is null
	begin
	  
  		insert into @bps ([UserId], [BPS]) values (@curUserId, null);
		--get user's owner
		set @tmp = (select [OwnerUserId] from [Usr].[User] where [Id] = @curUserId);
		--flag is broadcast prohibition structure or not
		set @val = null;
		--extract parents
		while @tmp is not null and @tmp <> @curUserId and @val is null
		begin
			--if parent has broadcast prohibition
			if (select 1 from @bp where [UserId] = @tmp) is not null begin
				--set flag
				set @val = 1;
			end	
			else 
			begin
				--if parent is still unknown
				set @val = (select [BPS] from @bps where [UserId] = @tmp);
				if (@val is null)
				begin
					--if unknown, then insert
					insert into @bps ([UserId], [BPS]) values (@tmp, null);
					--extract next parent
					set @tmp = (select [OwnerUserId] from [Usr].[User] where [Id] = @tmp);
				end
			end
		end

		update @bps set [BPS] = coalesce(@val, 0) where [BPS] is null;
	end
	if @val = 1
	  insert into @ids values (@curUserId);

    fetch next from ic into @curUserId;
  end;

  close ic;
  deallocate ic;

  return
end
GO

/********* [Usr].[BroadcastProhibition_StructureUpward2]  **********/
CREATE function [Usr].[BroadcastProhibition_StructureUpward2]
(
  @userId uniqueidentifier,
  @users [dbo].[GuidList] readonly
)
RETURNS TABLE 
AS
RETURN
(
	SELECT CAST(u.Id as uniqueidentifier) as UserId
	FROM [Usr].[User] u
	CROSS APPLY [Usr].[User_GetParents2](u.Id, default) parents
	INNER JOIN [Usr].[BroadcastProhibition] bp ON bp.ProhibitionUserId = parents.UserId
	WHERE bp.[UserId] = @userId
		AND u.Id IN (SELECT DISTINCT [Guid] FROM @users)
)
GO

/****** function [Usr].[BroadcastProhibition_StructureUpward] ******/
CREATE       function [Usr].[BroadcastProhibition_StructureUpward](
 @userId uniqueidentifier,
  @users [dbo].[GuidList] readonly)
RETURNS TABLE
AS
RETURN
(
--  Первый вариант
--  select [UserId] from [Usr].[BroadcastProhibition_StructureUpward1](@usersPhone, @userId) 
--  Второй вариант, ускоренный
  select [UserId] from [Usr].[BroadcastProhibition_StructureUpward2](@userId,  @users)  --OPTION (MAXRECURSION 32767)
)
GO


/******** [Ui].[StructureNode_GetNestedNodes] *********/
create or alter function [Ui].[StructureNode_GetNestedNodes](
   @nodeId uniqueidentifier)
RETURNS @ids TABLE (
  [NodeId] uniqueidentifier NOT NULL UNIQUE,
  [Deep] int NOT NULL
)
as
begin
  declare @deep integer;
  set @deep = 1;

  insert into @ids ([NodeId], [Deep])
  select @nodeId, 0;

  insert into @ids ([NodeId], [Deep])
  select [Id], @deep 
  from [Ui].[StructureNode]   
  where [ParentNodeId] = @nodeId;

  while 1 = 1
  begin
    insert into @ids ([NodeId], [Deep])
	select n.[Id], @deep + 1 
	from [Ui].[StructureNode] n
	inner join @ids i on n.[ParentNodeId] = i.[NodeId] and i.[Deep] = @deep;	
	      	  
	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  RETURN
end;
GO

/********* [Usr].[User_UiNodesCount] ***********/
CREATE OR ALTER FUNCTION [Usr].[User_UiNodesCount] (
	@userId uniqueidentifier
)
RETURNS @result TABLE (
  [NodeId] uniqueidentifier NOT NULL UNIQUE,
  [Count] integer NOT NULL
)
AS
BEGIN
    --root node
	declare @nodeId uniqueidentifier;
	set @nodeId = '00000000-0000-0000-0000-000000000000';
	
	--all nodes hierarchy
	declare @nodesDeep table (
		[NodeId] uniqueidentifier NOT NULL,
		[InheritFromNodeId] uniqueidentifier NOT NULL);

	DECLARE icc CURSOR FORWARD_ONLY FAST_FORWARD
	FOR
		select [Id] from [Ui].[StructureNode]
	open icc;
	fetch next from icc into @nodeId;
	while @@FETCH_STATUS = 0
	begin
	  insert into @nodesDeep
	  select [NodeId], @nodeId from [Ui].[StructureNode_GetNestedNodes](@nodeId);

	  fetch next from icc into @nodeId;
	end;
	
	with [Nodes] as (
		select [NodeId], [InheritFromNodeId] from @nodesDeep
	),
	[LowSegments] as (
		select 
			n.[InheritFromNodeId] as [NodeId],
			ns.[SegmentId]	
		from [Nodes] n
		left join [Ui].[StructureNodeSegment] ns on n.[NodeId] = ns.[NodeId]
	)
	insert into @result ([NodeId], [Count])
	select 
		ls.[NodeId],
		coalesce(count(DISTINCT childs.[UserId]), 0) as [Count]
	from [LowSegments] ls
	left join [Usr].[SegmentMember] sm on sm.[SegmentId] = ls.[SegmentId]
	left join [Usr].[User_GetChildsSimple](@userId, default, default) childs on sm.[UserId] = childs.[UserId]
	group by ls.[NodeId];

	RETURN;
END
GO

/******** [Msg].[WebChat_GetSmsText] **********/
create or alter function [Msg].[WebChat_GetSmsText](
  @sender_name nvarchar(500),
  @code nvarchar(100))
returns nvarchar(max)
as
begin

  declare @text_t nvarchar(max);
  declare @url_t nvarchar(100);
  set @text_t = (select [Value] from [Cfg].[Settings] where [Name] = 'WEB_CHAT_SMS');
  set @url_t = (select [Value] from [Cfg].[Settings] where [Name] = 'WEB_CHAT_DEEP_URL');

  return REPLACE( 
    REPLACE(@text_t, '%sender_name%', @sender_name),
	'%url%', REPLACE(@url_t, '%code%', @code));
end
go

/******** [Ui].[StructureNode_GetChilds] **********/
create or alter function [Ui].[StructureNode_GetChilds](
   @nodeId uniqueidentifier)
RETURNS @ids TABLE (
  [NodeId] uniqueidentifier NOT NULL UNIQUE,
  [InheritFromNodeId] uniqueidentifier,
  [Deep] int NOT NULL
)
as
begin
  --declare @ids [Usr].[UserIdDeep];
  declare @deep integer;
  set @deep = 1;

  insert into @ids ([NodeId], [InheritFromNodeId], [Deep])
  select [Id], [Id], @deep 
  from [Ui].[StructureNode]   
  where [ParentNodeId] = @nodeId;

  while 1 = 1
  begin
    insert into @ids ([NodeId], [InheritFromNodeId], [Deep])
	select n.[Id], i.[InheritFromNodeId], @deep + 1 
	from [Ui].[StructureNode] n
	inner join @ids i on n.[ParentNodeId] = i.[NodeId] and i.[Deep] = @deep;	
	      	  
	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  RETURN
end;
go

/****** [Usr].[GetUserData] *******/
CREATE OR ALTER FUNCTION [Usr].[GetUserData]
(
	@UserId uniqueidentifier,
	@Key nvarchar(255)
)
returns nvarchar(max)
AS
BEGIN   
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

--	DECLARE @KeyId bigint;
--	exec [Usr].[GetUserDataKey] @Key, @KeyId = @KeyId OUTPUT;

DECLARE @Data nvarchar(max);
	SET @Data = 
(SELECT TOP 1 [Data] FROM [Usr].[UserData] d 
inner join  [Usr].[UserDataKey] k on(d.[KeyId] = k.[Id])
WHERE d.[UserId] = @UserId and k.[Key] = @Key) ;
--);
return @Data;
END
GO

/****** 2020.12.29 ******/
/****** function [Usr].[User_GetChildsWithPhone] ******/
CREATE OR ALTER     function [Usr].[User_GetChildsWithPhone1](
   @usersPhone [dbo].[StringList] readonly,
   @userId uniqueidentifier)
RETURNS @ids TABLE (  [UserId] uniqueidentifier NOT NULL )
as
begin

 insert into @ids ([UserId])

 select [UserId] from [Usr].[User_GetChildsSimple](@userId, default, default) ugc
  inner join [Usr].[User] u on (ugc.[UserId] = u.[Id])
  inner join @usersPhone p on (p.[String] = u.[Phone])

  RETURN
end;
GO

CREATE OR ALTER   function [Usr].[User_GetChildsWithPhone2](
   @usersPhone [dbo].[StringList] readonly,
   @userId uniqueidentifier)
RETURNS TABLE
AS
RETURN
(
select [UserId] from [Usr].[User_GetChildsSimple2](@userId, default, default) ugc
  inner join [Usr].[User] u on (ugc.[UserId] = u.[Id])
  inner join @usersPhone p on (p.[String] = u.[Phone])
)
GO

DROP FUNCTION IF EXISTS  [Usr].[User_GetChildsWithPhone]
GO

/****** function [Usr].[User_GetChildsWithPhone] ******/
CREATE OR ALTER     function [Usr].[User_GetChildsWithPhone](
   @usersPhone [dbo].[StringList] readonly,
   @userId uniqueidentifier)
RETURNS TABLE
AS
RETURN
(
--  Первый вариант
--  select [UserId] from [Usr].[User_GetChildsWithPhone1](@usersPhone, @userId)  where UserId is not null
--  Второй вариант, ускоренный
  select [UserId] from [Usr].[User_GetChildsWithPhone2](@usersPhone, @userId)  --OPTION (MAXRECURSION 32767)
)
GO

/****** 2020.11.16 ******/
/****** Доп. данные к сообщениям ******/
/****** function [Msg].[GetMessageData] ******/
CREATE OR ALTER FUNCTION [Msg].[GetMessageData]
(
	@MessageId uniqueidentifier,
	@Key nvarchar(255)
)
returns nvarchar(max)
AS
BEGIN   
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

--	DECLARE @KeyId bigint;
--	exec [Msg].[GetMessageDataKey] @Key, @KeyId = @KeyId OUTPUT;

DECLARE @Data nvarchar(max);
	SET @Data = 
(SELECT TOP 1 [Data] FROM [Msg].[MessageData] d 
inner join  [Msg].[MessageDataKey] k on(d.[KeyId] = k.[Id])
WHERE d.[MessageId] = @MessageId and k.[Key] = @Key) ;
--);
return @Data;
END
GO

/****** 2020.12.29 ******/
/****** Оптимизация добывания данных ******/
/****** 2021.03.05 ******/
/****** Не включать в выборку отложенные ******/

create or ALTER  function [Msg].[LastMessage2](@userId uniqueidentifier)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL,
  [UserOppId] uniqueidentifier NOT NULL, 
  [MsgId] uniqueidentifier NOT NULL,
  [MsgIdx] bigint NOT NULL,
  [ToTransportTypeId] integer NOT NULL,
  [Income] bit,
  [MsgToUserIdx] bigint NOT NULL,
  [UnreadCnt] integer
)
as
begin

with [FromRN] as (
	select 
	  ROW_NUMBER() OVER (PARTITION BY m.[FromUserId], mtu.[ToUserId] ORDER BY m.[Idx] DESC) as [RN],
	  m.[FromUserId] as [UserId], 
	  mtu.[ToUserId] as [UserOppId], 
	  m.[Id] as [MsgId],
	  m.[Idx] as [MsgIdx],
	  mtu.[ToTransportTypeId],
	  mtu.[Idx] as [MsgToUserIdx]
	from [Msg].[MessageToUser] mtu
	inner join [Msg].[Message] m  on m.[Id] = mtu.[MsgId]
	inner join [Cfg].[TransportType] fromtt on fromtt.[Id] = m.[FromTransportTypeId]
	inner join [Cfg].[MessageType] mt on m.[MessageTypeId] = mt.[Id]
	where fromtt.[InnerTransport] = 1
	  and m.[IsDeleted] = 0
	  and (m.DalayedCancelled is not null or m.DelayedStart is null or (m.DelayedStart is not null and m.DelayedStart < GETUTCDATE())) 
	  and mt.[ShowInHistory] = 1
	  and mtu.[IsWebChatGreeting] = 0
	  and m.[FromUserId] = @userId
),
[From] as (
	select [UserId], [UserOppId], [MsgId], [MsgIdx], [ToTransportTypeId], [MsgToUserIdx]
	from [FromRN]
	where [RN] = 1
),
[ToRn] as (
	select
	  ROW_NUMBER() OVER (PARTITION BY m.[FromUserId], mtu.[ToUserId] ORDER BY m.[Idx] DESC) as [RN],
	  mtu.[ToUserId] as [UserId], 
	  m.[FromUserId] as [UserOppId], 
	  m.[Id] as [MsgId],
	  m.[Idx] as [MsgIdx],
	  mtu.[ToTransportTypeId],
	  mtu.[Idx] as [MsgToUserIdx],
	  mtu.[IsRead]
	from [Msg].[MessageToUser] mtu
	inner join [Msg].[Message] m on m.[Id] = mtu.[MsgId]
	inner join [Cfg].[TransportType] tott on tott.[Id] = mtu.[ToTransportTypeId]
	inner join [Cfg].[MessageType] mt on m.[MessageTypeId] = mt.[Id]
	where tott.[InnerTransport] = 1
	  and m.[IsDeleted] = 0
	  and (m.DalayedCancelled is not null or m.DelayedStart is null or (m.DelayedStart is not null and m.DelayedStart < GETUTCDATE())) 
	  and mt.[ShowInHistory] = 1
	  and mtu.[IsWebChatGreeting] = 0
	  and mtu.[ToUserId] = @userId
),
[To] as (
	select [UserId], [UserOppId], [MsgId], [MsgIdx], [ToTransportTypeId], [MsgToUserIdx]
	from [ToRN]
	where [RN] = 1
),
[UnionRN] as (
	select ROW_NUMBER() OVER (PARTITION BY [UserId], [UserOppId] ORDER BY [MsgIdx] DESC) as [RN], v.*
	from (
	  select cast(0 as bit) as [Income], * from [From]
	  union
	  select cast(1 as bit) as [Income], * from [To]
	) v
)
insert into @ids
  select
	t.[UserId], 
	t.[UserOppId], 
	t.[MsgId],
	t.[MsgIdx],
	t.[ToTransportTypeId],
	t.[Income],
	t.[MsgToUserIdx],
	(select coalesce(count(*), 0) from [ToRn] where [IsRead] = 0 and [UserId] = t.[UserId] and [UserOppId] = t.[UserOppId]) as [UnreadCnt]
  from [UnionRN] t
  inner join [Usr].[User] u on t.[UserOppId] = u.[Id] and u.[Enabled] = 1
   where t.[RN] = 1 --and t.[UserId] = '87614589-411b-ea11-a2c3-dcf6a6fc5b19'
 ;
return
end;
go


/****** [Msg].[LastMessage3] ******/
CREATE OR ALTER FUNCTION [Msg].[LastMessage3](@userId uniqueidentifier)
RETURNS TABLE
AS
RETURN
(
with [FromRN] as (
	select 
	  ROW_NUMBER() OVER (PARTITION BY m.[FromUserId], mtu.[ToUserId] ORDER BY m.[Idx] DESC) as [RN],
	  m.[FromUserId] as [UserId],
	  mtu.[ToUserId] as [UserOppId],
	  m.[Id] as [MsgId],
	  m.[Idx] as [MsgIdx],
	  mtu.[ToTransportTypeId],
	  mtu.[Idx] as [MsgToUserIdx]
	from [Msg].[MessageToUser] mtu
	inner join [Msg].[Message] m  on m.[Id] = mtu.[MsgId]
	inner join [Cfg].[TransportType] fromtt on fromtt.[Id] = m.[FromTransportTypeId]
	inner join [Cfg].[MessageType] mt on m.[MessageTypeId] = mt.[Id]
	where fromtt.[InnerTransport] = 1
	  and m.[IsDeleted] = 0
	  and mt.[ShowInHistory] = 1
	  and mtu.[IsWebChatGreeting] = 0
	  and m.[FromUserId] = @userId
),
[ToRn] as (
	select
	  ROW_NUMBER() OVER (PARTITION BY m.[FromUserId], mtu.[ToUserId] ORDER BY m.[Idx] DESC) as [RN],
	  mtu.[ToUserId] as [UserId], 
	  m.[FromUserId] as [UserOppId], 
	  m.[Id] as [MsgId],
	  m.[Idx] as [MsgIdx],
	  mtu.[ToTransportTypeId],
	  mtu.[Idx] as [MsgToUserIdx],
	  mtu.[IsRead]
	from [Msg].[MessageToUser] mtu
	inner join [Msg].[Message] m on m.[Id] = mtu.[MsgId]
	inner join [Cfg].[TransportType] tott on tott.[Id] = mtu.[ToTransportTypeId]
	inner join [Cfg].[MessageType] mt on m.[MessageTypeId] = mt.[Id]
	where tott.[InnerTransport] = 1
	  and m.[IsDeleted] = 0
	  and mt.[ShowInHistory] = 1
	  and mtu.[IsWebChatGreeting] = 0
	  and mtu.[ToUserId] = @userId
),
[UnionRN] as (
	select ROW_NUMBER() OVER (PARTITION BY [UserId], [UserOppId] ORDER BY [MsgIdx] DESC) as [RN], v.*
	from (
	  select cast(0 as bit) as [Income], [UserId], [UserOppId], [MsgId], [MsgIdx], [ToTransportTypeId], [MsgToUserIdx] from [FromRN] where [RN] = 1
	  union
	  select cast(1 as bit) as [Income], [UserId], [UserOppId], [MsgId], [MsgIdx], [ToTransportTypeId], [MsgToUserIdx] from [ToRN] where [RN] = 1
	) v
)
select
	t.[UserId], 
	t.[UserOppId], 
	t.[MsgId],
	t.[MsgIdx],
	t.[ToTransportTypeId],
	t.[Income],
	t.[MsgToUserIdx],
	(select coalesce(count(*), 0) from [ToRn] where [IsRead] = 0 and [UserId] = t.[UserId] and [UserOppId] = t.[UserOppId]) as [UnreadCnt]
from [UnionRN] t
	inner join [Usr].[User] u on t.[UserOppId] = u.[Id] and u.[Enabled] = 1
where t.[RN] = 1
)
go

/******* [Usr].[User_GetChildsSimple] ********/
CREATE OR ALTER     function [Usr].[User_GetChildsSimple1](
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE
)
as
begin
  if (select [IsUseDeepChilds] from [Usr].[User] where [Id] = @userId) = 1 
    and @maxDeep is null
	and @includeDeleted = 0
  begin
    insert into @ids 
	select [ChildUserId] from [Usr].[UserDeepChilds] where [UserId] = @userId;
  end
  else 
  begin
    insert into @ids
	select [UserId] from [Usr].[User_GetChilds](@userId, @maxDeep, @includeDeleted);
  end

  RETURN
end;
GO

/******* [Usr].[User_GetChildsSimple2] ********/
CREATE OR ALTER function [Usr].[User_GetChildsSimple2](
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS TABLE
AS
RETURN
(
WITH Rec AS (
	-- ancor member expression
	SELECT Id UserId, 1 deep, OwnerUserId FROM [Usr].[User]
	WHERE Enabled = 1 AND [OwnerUserId] = @userId
	UNION ALL
	-- recursive member expression
	SELECT t.Id UserId, Rec.deep + 1 deep, t.OwnerUserId
	FROM [Usr].[User] t JOIN Rec ON Rec.UserId = t.OwnerUserId
	WHERE (t.[Enabled] = 1 or 0 = 1) AND t.[OwnerUserId] != @userId
)
-- start main query
select ChildUserId UserId from [Usr].[UserDeepChilds] where [UserId] = @userId
	and (select [IsUseDeepChilds] from [Usr].[User] where [Id] = @userId) = 1
    and @maxDeep is null and @includeDeleted = 0
UNION ALL
SELECT UserId FROM Rec
WHERE ABS(deep) <= IsNull(@maxDeep,32767)
	AND (select [IsUseDeepChilds] from [Usr].[User] where [Id] = @userId) = 0
--OPTION (MAXRECURSION 32767) -- no OPTION here!
)
GO

DROP FUNCTION IF EXISTS [Usr].[User_GetChildsSimple]
/******* [Usr].[User_GetChildsSimple] ********/
go

CREATE OR ALTER     function [Usr].[User_GetChildsSimple](
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS TABLE
AS
RETURN
(
--  Первый вариант
--  SELECT distinct UserId FROM [Usr].[User_GetChildsSimple1](@userId, @maxDeep, @includeDeleted) where UserId is not null
--  Второй вариант, ускоренный
  SELECT UserId FROM [Usr].[User_GetChildsSimple2](@userId, @maxDeep, @includeDeleted) --OPTION (MAXRECURSION 32767)

)
GO

/******** [Usr].[Segment_GetMembers] **********/
CREATE OR ALTER     function [Usr].[Segment_GetMembers1](
   @segmentId uniqueidentifier,
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
--RETURNS @ids TABLE (
--  [UserId] uniqueidentifier NOT NULL UNIQUE,
--  [Deep] integer NOT NULL
--)
RETURNS TABLE
as
return (  
  select 
    ids.[UserId]   
  from [Usr].[User_GetChildsSimple] (@userId, @maxDeep, @includeDeleted) ids
  inner join [Usr].[SegmentMember] sm on ids.[UserId] = sm.[UserId]
  where sm.[SegmentId] = @segmentId
);
GO

/******** [Usr].[Segment_GetMembers] **********/
CREATE OR ALTER     function [Usr].[Segment_GetMembers2](
   @segmentId uniqueidentifier,
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
--RETURNS @ids TABLE (
--  [UserId] uniqueidentifier NOT NULL UNIQUE,
--  [Deep] integer NOT NULL
--)
RETURNS TABLE
as
return (  
  select 
    CAST(ids.[UserId] as uniqueidentifier) [UserId]
  from [Usr].[User_GetChildsSimple2] (@userId, @maxDeep, @includeDeleted) ids
  inner join [Usr].[SegmentMember] sm on ids.[UserId] = sm.[UserId]
  where sm.[SegmentId] = @segmentId
);
GO

/******** [Usr].[Segment_GetMembers] **********/
CREATE OR ALTER     function [Usr].[Segment_GetMembers](
   @segmentId uniqueidentifier,
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
--RETURNS @ids TABLE (
--  [UserId] uniqueidentifier NOT NULL UNIQUE,
--  [Deep] integer NOT NULL
--)
RETURNS TABLE
as
return (  
--  Первый вариант
--  SELECT UserId FROM [Usr].[Segment_GetMembers1](@segmentId, @userId, @maxDeep, @includeDeleted)
--  Второй вариант, ускоренный
  SELECT UserId FROM [Usr].[Segment_GetMembers2](@segmentId, @userId, @maxDeep, @includeDeleted) --OPTION (MAXRECURSION 32767)
);
GO

/******* [Usr].[User_GetChildsWOBP2] ********/
CREATE OR ALTER function [Usr].[User_GetChildsWOBP2](
   @userId uniqueidentifier,
   @currentUserId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS TABLE
as
RETURN
(
	WITH Rec AS (
		-- ancor member expression
		SELECT Id UserId, 1 deep, OwnerUserId FROM [Usr].[User]
		WHERE ([Enabled] = 1 or @includeDeleted = 1) AND [OwnerUserId] = @userId
			AND NOT EXISTS (SELECT 0 FROM [Usr].[BroadcastProhibition] bp WHERE [Id] = bp.[ProhibitionUserId] and bp.[UserId] = @currentUserId)
		UNION ALL
		-- recursive member expression
		SELECT t.Id UserId, Rec.deep + 1 deep, t.OwnerUserId
		FROM [Usr].[User] t JOIN Rec ON Rec.UserId = t.OwnerUserId
		WHERE (t.[Enabled] = 1 or @includeDeleted = 1) AND t.[OwnerUserId] != @userId AND ABS(Rec.deep) < IsNull(@maxDeep,32767)
			AND NOT EXISTS (SELECT 0 FROM [Usr].[BroadcastProhibition] bp WHERE t.[Id] = bp.[ProhibitionUserId] and bp.[UserId] = @currentUserId)
	)
	SELECT CAST(UserId as uniqueidentifier) as UserId
		, CAST(deep as integer) as deep
		, CAST(OwnerUserId as uniqueidentifier) as OwnerUserId
	FROM Rec
)
GO

/****** [Usr].[User_GetSelection]  ********/
-- Edited: 2020.02.10
CREATE OR ALTER FUNCTION [Usr].[User_GetSelection1] 
(	
	-- Add the parameters for the function here
	@userId uniqueidentifier,
	@includeUsersWithStructure [Usr].[UserIdDeep] readonly,
	@excludeUsersWithStructure [dbo].[GuidList] readonly,
	@includeUsers [dbo].[GuidList] readonly,
	@excludeUsers [dbo].[GuidList] readonly,
	@segments [dbo].[GuidList] readonly,
    @includeDeleted bit = 0
)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE
) 
as
begin

  --excluded users
  declare @exclude [dbo].[GuidList];
  --included users
  declare @include [dbo].[GuidList];
  declare @guid uniqueidentifier;
  declare @deep int;

  insert into @exclude 
  select * from @excludeUsersWithStructure;

  insert into @exclude
  select * from @excludeUsers;  

  --get structure for exluded users
  DECLARE ic CURSOR FORWARD_ONLY FAST_FORWARD
  FOR  
    select [Guid] from @excludeUsersWithStructure
	where [Guid] not in (
		select [UserId] from [Usr].[BroadcastProhibition_StructureUpward](
			@userId, @excludeUsersWithStructure));
  open ic;

  fetch next from ic into @guid;
  while @@FETCH_STATUS = 0
  begin
    insert into @exclude
	select [UserId] 
	from [Usr].[User_GetChildsWoBP](@guid, @userId, default, @includeDeleted);

    fetch next from ic into @guid;
  end;
  	
  close ic;
  deallocate ic;  

  --get structure for included users
  DECLARE ic CURSOR FORWARD_ONLY FAST_FORWARD
  FOR  
    select [UserId], [Deep] 
	from @includeUsersWithStructure
	--where [UserId] in (
	--  select [UserId] from [Usr].[User_GetChildsSimple](@userId, default, default) 
	--  union
	--  select @userId)
  open ic;

  fetch next from ic into @guid, @deep;
  while @@FETCH_STATUS = 0
  begin
    insert into @include
	select [UserId] 
	from [Usr].[User_GetChildsWoBP](@guid, @userId, @deep, @includeDeleted);

	if @guid <> @userId
		insert into @include values (@guid);

    fetch next from ic into @guid, @deep;
  end;
  	
  close ic;
  deallocate ic;

  --add segments
  insert into @include
  select sm.[UserId]
  from [Usr].[SegmentMember] sm 
  inner join [Usr].[User_GetChildsWoBP](@userId, @userId, default, @includeDeleted) c 
	on sm.[UserId] = c.[UserId]
  where sm.[SegmentId] in (select [Guid] from @segments);

  delete from @include where [Guid] in (select [Guid] from @exclude);
  delete from @include where [Guid] in (select [ProhibitionUserId] from [Usr].[PersonalProhibition] where [UserId] = @userId);
  insert into @include select [Guid] from @includeUsers where [Guid] <> @userId;

  --get selection
  /*with 
  dt as (		--all users in selection
    select DISTINCT [Guid]
	from @include
	--where [Guid] not in (select [Guid] from @exclude)
	--union
	--select DISTINCT [Guid] from @includeUsers where [Guid] <> @userId
  ),
  bp as ( --broadcast prohibition
    --select [UserId]
	--from [Usr].[User_GetChildsMulti](@broadcastProhibition, default, @includeDeleted)
	select [UserId]
	from [Usr].[BroadcastProhibition_StructureUpward](@userId, @include)
  )
  insert into @ids ([UserId])
  (
    select [Guid] 
	from dt
	where dt.[Guid] not in (select [UserId] from bp)
  );*/
  insert into @ids
  select [Id] from [Usr].[User] where [Id] in (select [Guid] from @include);

  --only for test
	/*select * from @ids where [UserId] in
		(select * from [Usr].[BroadcastProhibition_Structure](@userId))

	select * from @ids where [UserId] not in
		(select [UserId] from [Usr].[User_GetChilds](@userId, default, default))

	select * from @ids where [UserId] not in (
		select [UserId] from [Usr].[SegmentMember] where [SegmentId] in (select * from @segments))

	select * from [Usr].[SegmentMember] sm
	inner join [Usr].[User_GetChilds](@userId, default, default) c
		on sm.[UserId] = c.[UserId]
	left join [Usr].[BroadcastProhibition_Structure](@userId) bp on sm.[UserId] = bp.[UserId]
	where sm.[SegmentId] in (select * from @segments)
		and bp.[UserId] is null*/

  RETURN
end
GO

/****** [Usr].[User_GetSelection2]  ********/
--  Модифицированный вариант от 15.12.2020
CREATE OR ALTER FUNCTION [Usr].[User_GetSelection2] 
(	
	-- Add the parameters for the function here
	@userId uniqueidentifier,
	@includeUsersWithStructure [Usr].[UserIdDeep] readonly,
	@excludeUsersWithStructure [dbo].[GuidList] readonly,
	@includeUsers [dbo].[GuidList] readonly,
	@excludeUsers [dbo].[GuidList] readonly,
	@segments [dbo].[GuidList] readonly,
    @includeDeleted bit = 0
)
RETURNS TABLE
AS
RETURN
(
--  Модифицированный вариант от 15.12.2020
	/* базовая идея - получить два множества:
	1. приход = список включаемых в выборку конечных пользователей
		1.1 подчиненные из @includeUsersWithStructure
		1.2 родители подчиненных из @includeUsersWithStructure, отличные от текущего пользователя
		1.3 @includeUsers
		1.4 @segments
	2. расход = список исключаемых из выборки конечных пользователей
		2.1 подчиненные из @excludeUsersWithStructure
		2.2 родители подчиненных из @excludeUsersWithStructure
		2.3 пропускаем, т.е. непосредственно подчиненные из BroadcastProhibition исключены на этапе подсчета "прихода"
		2.4 родители подчиненных из BroadcastProhibition
		2.5 PersonalProhibition
		2.6 @excludeUsers
	вычесть из первого списка все, что попало во второй
	*/
	--1.1 получить пользователей вместе с их структурой подчинения includeUsersWithStructure
	SELECT CAST(include_ws_list.UserId AS uniqueidentifier) AS UserId FROM @includeUsersWithStructure iws
	CROSS APPLY [Usr].[User_GetChildsWOBP2](iws.UserId, @userId, iws.Deep, @includeDeleted) include_ws_list
	WHERE --iws.UserId NOT IN (SELECT Guid FROM @excludeUsers) AND 
		iws.UserId NOT IN (SELECT Guid FROM @excludeUsersWithStructure)
	UNION ALL
	--1.2 получить родителей из списка включений includeUsersWithStructure, которые НЕ совпадают с текущим пользователем
	SELECT CAST(UserId AS uniqueidentifier) AS UserId FROM @includeUsersWithStructure WHERE UserId != @userId
	UNION ALL
	--1.3 получить пользователей из поименного списка
	SELECT CAST(Id AS uniqueidentifier) AS UserId FROM [Usr].[User] WHERE Id IN (SELECT Guid FROM @includeUsers)
	UNION ALL
	--1.4 получить пользователей из указанных сегментов
	SELECT CAST(sm.[UserId] AS uniqueidentifier) AS UserId
	FROM [Usr].[SegmentMember] sm
		INNER JOIN [Usr].[User_GetChildsWOBP2](@userId, @userId, (SELECT IsNull(MAX(Deep),32767) FROM @includeUsersWithStructure), @includeDeleted) c ON sm.[UserId] = c.[UserId]
	WHERE sm.[SegmentId] in (SELECT [Guid] FROM @segments)
	EXCEPT
	--2.1 исключить подчиненных из списка исключений со структурой подчинения:
	-- a) если передан список включений, то на максимальную глубину из этого списка;
	-- b) на всю допустимую глубину по умолчанию.
	SELECT CAST(exclude_ws_list.UserId AS uniqueidentifier) AS UserId FROM @excludeUsersWithStructure ews
	CROSS APPLY [Usr].[User_GetChilds4](ews.Guid, (SELECT IsNull(MAX(Deep),32767) FROM @includeUsersWithStructure), @includeDeleted) exclude_ws_list
	WHERE ews.Guid NOT IN (SELECT Guid FROM @excludeUsers)
	EXCEPT
	--2.2 исключить родителей из списка исключений со структурой подчинения
	SELECT CAST(Guid AS uniqueidentifier) AS UserId FROM @excludeUsersWithStructure
	EXCEPT
	--2.4 исключить установленные запреты на рассылку родителям списков со структурой
	SELECT CAST([ProhibitionUserId] AS uniqueidentifier) AS UserId FROM [Usr].[BroadcastProhibition] WHERE [UserId] = @userId
	EXCEPT
	--2.5 исключить установленные персональные запреты
	SELECT CAST([ProhibitionUserId] AS uniqueidentifier) AS UserId FROM [Usr].[PersonalProhibition] WHERE [UserId] = @userId
	EXCEPT
	--2.6 исключить пользователей из поименного списка
	SELECT CAST(Guid AS uniqueidentifier) AS UserId FROM @excludeUsers
)
GO

DROP FUNCTION IF EXISTS  [Usr].[User_GetSelection] 
GO

/****** [Usr].[User_GetSelection]  ********/
CREATE OR ALTER FUNCTION [Usr].[User_GetSelection] 
(	
	-- Add the parameters for the function here
	@userId uniqueidentifier,
	@includeUsersWithStructure [Usr].[UserIdDeep] readonly,
	@excludeUsersWithStructure [dbo].[GuidList] readonly,
	@includeUsers [dbo].[GuidList] readonly,
	@excludeUsers [dbo].[GuidList] readonly,
	@segments [dbo].[GuidList] readonly,
    @includeDeleted bit = 0
)
RETURNS TABLE
AS
RETURN
(

   select [UserId] FROM 
	-- Вариант 1 - исходный. 
--	[Usr].[User_GetSelection1](@userId, @includeUsersWithStructure, @excludeUsersWithStructure, @includeUsers, @excludeUsers, @segments, @includeDeleted) 
	-- Вариант 2 - модифицированный.  
	-- Для использования необходимо в самом верхнем вызове добавить OPTION (MAXRECURSION 32767)
	[Usr].[User_GetSelection2](@userId, @includeUsersWithStructure, @excludeUsersWithStructure, @includeUsers, @excludeUsers, @segments, @includeDeleted) 
)
GO

/********* [Usr].[User_UiNodesCount] ***********/
CREATE OR ALTER FUNCTION [Usr].[User_UiNodesCount3] (
	@userId uniqueidentifier
)
RETURNS TABLE 
AS
RETURN
(
	With struct AS (
		-- ancor member expression
		SELECT sn.Id FROM [Ui].[StructureNode] sn WHERE ParentNodeId is null
		UNION ALL
		-- recursive member expression
		SELECT sn.Id FROM [Ui].[StructureNode] sn JOIN struct ON struct.Id = sn.ParentNodeId
	)
	select
		CAST(struct.[Id] as uniqueidentifier) as [NodeId]
		, CAST(coalesce(count(DISTINCT childs.[UserId]), 0) as integer) as [Count]
	FROM struct
		CROSS APPLY [Ui].[StructureNode_GetNestedNodes](struct.[Id]) nn
		INNER JOIN  [Ui].[StructureNodeSegment] sns ON sns.[NodeId] = nn.[NodeId]
		OUTER APPLY [Usr].[Segment_GetMembers](sns.SegmentId, @userId, default, default) childs
	group by struct.[Id]
)
GO


/********* 2020.12.29 ***********/
/********* Построение отчётов ***********/

--  Правка от 13.01.2021
--  Правка от 23.02.2021
--  Правка от 19.07.2021
create or ALTER   function [Msg].[Message_GetItems](
   @msg uniqueidentifier)
RETURNS TABLE
as
RETURN
(
select mtu.ToUserId
	, mtu.ToTransportTypeId
	, (case when mtu.IsFailed = 0 then (case when wcdl.ViberStatus is null then mtu.IsSent else 
	(case when wcdl.ViberStatus not in (6,8,0,-100,7) then 1 else 0 end) end ) else 0 end) [IsSent]
	, (case when mtu.IsFailed = 0 then (case when wcdl.ViberStatus is null then 
	(case when mtu.ToTransportTypeId in (0,3) then mtu.IsDelivered else mtu.IsSent end) else 
	(case when ViberStatus in (2,4,5) then 1 else 0 end)  end )  else 0 end) [IsDelivered]
	, (case when mtu.IsFailed = 0 then (case when wcdl.ViberStatus is null then 
	(case when mtu.ToTransportTypeId in (0,3) then mtu.IsRead else mtu.IsSent end) else 
	(case when ViberStatus in (4,5) then 1 else 0 end) end )  else 0 end) [IsRead]
	, (case when mtu.ToTransportTypeId in (0,3) and mtu.IsDelivered = 0 then 1 else
	(case when ViberStatus in (3) then 1 else 0 end) end) [IsNotDelivered]
	, (case when mtu.IsFailed = 1 then mtu.IsFailed else (case when ViberStatus in (6,8) then 1 else 0 end) end) [IsFailed]
	, (case when ViberStatus in (0,-100,7) then 1 else 0 end) [IsNoDataFound]
	, (case when wca.[WebChatId] is not null then 1 else 0 end) [IsWebChatAccepted]
	, (case when mtu.[ToTransportTypeId] = 100 and mtu.[IsFailed] = 1 then 1 else 0 end) as [CantSendToWebChat]
	, (case when mtu.[ToTransportTypeId] = /**WebChat**/100 and mtu.[IsRead] = 1 then 1 else 0 end) as [IsSmsUrlOpened]
	, (case when mtu.[ToTransportTypeId] = 100 then 1 else 0 end) as [IsWebChat]
	, (case when mtu.[Idx] is not null and mtu.[IsFailed] = 0 and mtu.[IsSent] = 0 then 1 else 0 end) as [IsQueued]	
	, COALESCE(m.[IsDeleted], 0) as [IsDeleted] 
	, (case when m.[DalayedCancelled] is not null then 1 else 0 end) as [IsCancelled]
	, 0 as [TransportNotFound]
from [Msg].[MessageToUser] mtu 
inner join  [Msg].[Message] m on m.id = mtu.MsgId 
left join [Msg].[WebChatDeepLink] wcdl on wcdl.ToUserId = mtu.ToUserId and wcdl.MsgId = mtu.MsgId
left join [Msg].[WebChatAccepted] wca on wcdl.[Id] = wca.[WebChatId]
where mtu.MsgId = @msg
--order by ToTransportTypeId desc, mtu.ToUserId
)
go

create or alter function [Msg].[Message_GetStats](
   @msg uniqueidentifier)
RETURNS TABLE
as
RETURN
(
select	cast(count(*) as int) [RecipientCount]
	, cast(sum([IsSent]) as int) [SentCount]
	, sum([IsFailed]) [FailedCount]
	, sum([IsQueued]) as [QueuedCount]
	, sum([IsDelivered]) [DeliveredCount]
	, sum([IsRead]) [ReadCount]
	, sum([IsNotDelivered]) [NotDeliveredCount]
	, sum([IsNoDataFound]) [NoDataFoundCount]
	, sum([IsWebChat]) as [WebChatCount]
	, sum([IsWebChatAccepted]) [WebChatAcceptedCount]
	, sum([CantSendToWebChat]) as [CantSendToWebChatCount]
	, sum([IsSmsUrlOpened]) as [SmsUrlOpenedCount]

from
[Msg].[Message_GetItems](@msg) 
)
go

/********* 2021.02.06 ***********/
/********* Построение отчётов ***********/

/********* [Msg].[MessageToUser_GetStats] ***********/
create or ALTER function [Msg].[MessageToUser_GetStats](
   @msg uniqueidentifier)
RETURNS TABLE
as
RETURN
(
SELECT TOP 1 WITH TIES ToUserId, ToTransportTypeId, [Code] --, StateKey as State
FROM [Msg].[Message_GetItems](@msg) p
UNPIVOT ([State] FOR StateKey 
  IN   	([IsSent], [IsDelivered], [IsRead], [IsNotDelivered], [IsFailed], [IsNoDataFound], [IsQueued], [TransportNotFound], [IsDeleted], [IsCancelled])
) AS unpvt
join [Msg].[MessageStatus] ms on ms.[Alias] = unpvt.StateKey
where [State] > 0 
--group by ToUserId, StateKey
ORDER BY ROW_NUMBER() OVER(PARTITION BY ToUserId ORDER BY ms.[Severity] DESC)
)
GO

/********* [Msg].[MessageToUser_GetFiltredStats] ***********/
create or ALTER   function [Msg].[MessageToUser_GetFiltredStats](
   @msg uniqueidentifier)
RETURNS TABLE
as
RETURN
(
SELECT /**TOP 1 WITH TIES **/ ToUserId, ToTransportTypeId, [Code] --, StateKey as State
FROM [Msg].[Message_GetItems](@msg) p
UNPIVOT ([State] FOR StateKey 
  IN   	([IsSent], [IsDelivered], [IsRead], [IsNotDelivered], [IsFailed], [IsNoDataFound], [IsQueued], [TransportNotFound], [IsDeleted], [IsCancelled])
) AS unpvt
join [Msg].[MessageStatus] ms on ms.[Alias] = unpvt.StateKey
where [State] > 0 
--group by ToUserId, StateKey
--ORDER BY ROW_NUMBER() OVER(PARTITION BY ToUserId ORDER BY ms.[Severity] DESC)
)
GO

/****** 05.04.2021 ******/
/****** [Usr].[FindStringInStructure] *******/
create or ALTER function [Usr].[FindStringInStructure](
   @userId uniqueidentifier,
   @searchString nvarchar(500),
   @includeDeleted bit = 0)
RETURNS TABLE
as
RETURN
(
SELECT u.* from [Usr].[User_GetChildsSimple](@userId,default,default)  dc 
inner join [Usr].[User] u on dc.UserId = u.Id
WHERE (Enabled = 1  OR @includeDeleted = 1) 
--  AND ( FullName like TRIM('*"' FROM @searchString) + '%' or [Phone] like TRIM('*"' FROM @searchString) + '%' or [Email] like TRIM('*"' FROM @searchString) + '%')
AND ( CONTAINS(([FullName], [Phone], [Email]), @searchString) )
--OPTION (MAXRECURSION 32767)
)
GO

/****** [Usr].[FindStringInStructurePart] *******/
create or ALTER function [Usr].[FindStringInStructurePart](
   @userId uniqueidentifier,
   @searchString nvarchar(500),
   @offset int,
   @rows int,
   @includeDeleted bit = 0)
RETURNS TABLE
as
RETURN
(
  SELECT * FROM [Usr].[FindStringInStructure](   @userId ,   @searchString ,   @includeDeleted )
ORDER BY Id 
    OFFSET ISNULL(@offset, 0)  ROWS
    FETCH NEXT ISNULL(@rows, 100) ROWS ONLY
)
GO

/****** [Usr].[FindStringInAll] *******/
create or ALTER function [Usr].[FindStringInAll](
   @searchString nvarchar(500),
   @includeDeleted bit = 0,
   @includeNoNumber bit = 1)
RETURNS TABLE
as
RETURN
(
SELECT u.* from [Usr].[User] u 
WHERE (Enabled = 1  OR @includeDeleted = 1) AND((FLUserNumber is not null) or @includeNoNumber = 1)
--  AND ( FullName like TRIM('*"' FROM @searchString) + '%' or [Phone] like TRIM('*"' FROM @searchString) + '%' or [Email] like TRIM('*"' FROM @searchString) + '%')
AND ( CONTAINS(([FullName], [Phone], [Email]), @searchString) )
)
GO


/****** [Usr].[FindStringInAllPart] *******/
create or ALTER function [Usr].[FindStringInAllPart](
   @searchString nvarchar(500),
   @offset int,
   @rows int,
   @includeDeleted bit = 0,
   @includeNoNumber bit = 1)
RETURNS TABLE
as
RETURN
(
  SELECT * FROM [Usr].[FindStringInAll](@searchString, @includeDeleted, @includeNoNumber)
ORDER BY Id 
    OFFSET ISNULL(@offset, 0)  ROWS
    FETCH NEXT ISNULL(@rows, 100) ROWS ONLY
)
GO

/****** [Msg].[Message_GetForResend] *******/
  CREATE  or ALTER  function [Msg].[Message_GetForResend](
   @MaxCount integer = 5,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS @ret TABLE (
  [Id] uniqueidentifier NOT NULL UNIQUE
) 
as
begin
declare @pri int;		--  Вариант переотправки (кому)
 declare @str nvarchar(50);
 select @str = [Value] from [Cfg].[Settings] where [Name] = 'RESEND_UNDELIVERED_BY_EMAIL';

if(@str is null)
  return;
set @pri = isnull(cast( @str as int), 0);
if(@pri = 0)
  return;


--declare @MaxCount int = 3;	--  количество итераций переотправки
--declare @maxDeep integer = null; 
--declare @includeDeleted bit = 0;
--declare  @ret [dbo].[GuidList];  -- ответ

declare @tmp as table (
  [MsgId] uniqueidentifier NOT NULL,
  [FromUserId] uniqueidentifier NOT NULL
);

with [RESEND] as
(
select [ResendMsgId], [FromUserId] from [Msg].[Message] m 
inner join [Msg].[MessageToUser] mtu on m.Id = mtu.MsgId -- and mtu.IsFailed = 0
where [ResendMsgId] is not null 
group by [ResendMsgId], [FromUserId]
having count([ResendMsgId]) < @MaxCount and 
count([ResendMsgId]) = sum(cast([IsFailed] as int))

union
select Id , [FromUserId] from [Msg].[Message] m 
 CROSS APPLY [Msg].[Message_GetItems](Id) mtu 
where [ResendMsgId] is null and [ResendTime] is null and [MessageTypeId] = 0 -- Потом сделать настройку
and ([IsFailed] = 1 /*or [IsNotDelivered] = 1*/ or IsNoDataFound = 1 )
group by [Id], [FromUserId]
)
--, [ALL] as
--(
--select * from [Msg].[MessageToUser] mtu 
--inner join (select distinct [ResendMsgId], [FromUserId] from RESEND) r on r.[ResendMsgId] = mtu.MsgId
--where mtu.IsFailed = 1 
--)
insert into @tmp select [ResendMsgId],  [FromUserId] from [RESEND]

if(@pri = 1) -- Для всех
 insert into @ret 
 select distinct [MsgId] from @tmp
else
if(@pri = 2) -- Для структуры
insert into @ret 
select /*distinct */ t.[MsgId] from @tmp t
INNER JOIN [Msg].[MessageToUser] mu ON t.MsgId = mu.MsgId
where exists (select 0 from [Usr].[User_GetChilds4]([FromUserId], default, default) ugc where ugc.UserId = mu.ToUserId)
 OPTION (MAXRECURSION 32765)
 --select distinct [MsgId] from @tmp
 --CROSS APPLY [Usr].[User_GetChilds4]([FromUserId], @maxDeep, @includeDeleted) ugc
else
if(@pri = 3)  -- Для не из структуры
insert into @ret 
 select /*distinct */ t.[MsgId] from @tmp t
INNER JOIN [Msg].[MessageToUser] mu ON t.MsgId = mu.MsgId
where not exists (select 0 from [Usr].[User_GetChilds4]([FromUserId], default, default) ugc where ugc.UserId = mu.ToUserId )
 OPTION (MAXRECURSION 32765)
return
end
GO

/********* 2021.07.10 ***********/
/*** Функция выбора неотправленных сообщений ***/

create or alter function [Msg].[MessageLoader](
   @toTransportTypeId int,
   @count int )
RETURNS TABLE
as
RETURN
(
SELECT TOP (isnull(@count, 10000)) mtu.*
FROM [Msg].[MessageToUser] AS mtu
INNER JOIN [Msg].[Message] AS m  WITH (INDEX(IDX__MsgMessage__PostTm)) ON mtu.[MsgId] = m.[Id] --AND m.[PostTm] > DATEADD(DAY,-@days,SysUtcDateTime())
AND m.[PostTm] > DATEADD(DAY,-isnull((select cast(value as int) from [Cfg].[Settings] where name = 'MESSAGE_PERFORM_DAYS'),62),SysUtcDateTime())
WHERE (mtu.[ToTransportTypeId] = @toTransportTypeId) AND (0 = mtu.[IsSent]) AND (0 = mtu.[IsFailed]) AND (0 = m.[IsDeleted]) 
  AND (m.[DalayedCancelled] IS NULL) AND ((m.[DelayedStart] IS NULL) OR ((m.[DelayedStart] IS NOT NULL) AND (m.[DelayedStart] <= SysUtcDateTime())))
  --AND m.[PostTm] > DATEADD(DAY,-@days,SysUtcDateTime())
ORDER BY m.[MessageTypeId] ASC, m.[Idx]
)
go


/*********************************************************************
	Triggers
*********************************************************************/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

drop trigger if exists [Usr].[Transport__OnInsertFLChat]
go

/******* [Usr].[Transport__InsteadOfInsert] *******/
drop trigger if exists [Usr].[Transport__InsteadOfInsert]
GO

drop trigger if exists [MessageToSegment_ProduceMessageToUsers]
go

/******* [Usr].[User_OnInsert_AddWebChatTransport] *******/
CREATE OR ALTER TRIGGER [Usr].[User_OnInsert_AddWebChatTransport]
   ON  [Usr].[User]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	insert into [Usr].[Transport] ([UserId], [TransportTypeId])
	select i.[Id], /**WebChat**/100
	from inserted i;
END
GO

/******* [Usr].[User_OnDisabled] ********/
create or alter trigger [Usr].[User_OnDisabled]
on [Usr].[User]
after update
as
  --delete all auth tokens if user become disabled
  delete from [Auth].[AuthToken] where [UserId] in (
  select i.[Id]
  from inserted i
  inner join deleted d on i.[Id] = d.[Id]
  where i.[Enabled] = 0
    and i.[Enabled] <> d.[Enabled]
)
GO

/******* [Usr].[User_ManageSmsTransport] ******/
CREATE OR ALTER TRIGGER [Usr].[User_ManageSmsTransport]
   ON  [Usr].[User]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- insert sms transport for users without sms transport
	insert into [Usr].[Transport] ([UserId], [TransportTypeId])
	select i.[Id], /**Sms**/150
	from inserted i
	left join deleted d on i.[Id] = d.[Id]
	left join [Usr].[Transport] t on i.[Id] = t.[UserId] and t.[TransportTypeId] = /**Sms**/150
	where i.[Phone] is not null 
	  and d.[Phone] is null
	  and t.[UserId] is null;

	-- enabled sms transport, if user already has disabled transport
    update [Usr].[Transport] set [Enabled] = 1
	where [UserId] in (
	  select i.[Id]
	  from inserted i
	  inner join [Usr].[Transport] t on i.[Id] = t.[UserId] and t.[TransportTypeId] = /**Sms**/150
	  left join deleted d on i.[Id] = d.[Id]
	where i.[Phone] is not null 
	  and d.[Phone] is null
	  and t.[Enabled] = 0
	) and [TransportTypeId] = /**Sms**/150;

	--disable sms transport if user set phone = null
	update [Usr].[Transport] set [Enabled] = 0
	where [UserId] in (
	  select i.[Id]
	  from inserted i
	  inner join deleted d on i.[Id] = d.[Id]
	  inner join [Usr].[Transport] t on i.[Id] = t.[UserId] and t.[TransportTypeId] = /**Sms**/150
	where i.[Phone] is null 
	  and d.[Phone] is not null
	  and t.[Enabled] = 1
	) and [TransportTypeId] = /**Sms**/150;
END
GO

/****** [Usr].[User_ManageEmailTransport] ******/
CREATE OR ALTER TRIGGER [Usr].[User_ManageEmailTransport]
   ON  [Usr].[User]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- insert Email transport for users without Email transport
	insert into [Usr].[Transport] ([UserId], [TransportTypeId])
	select i.[Id], /**Email**/151
	from inserted i
	left join deleted d on i.[Id] = d.[Id]
	left join [Usr].[Transport] t on i.[Id] = t.[UserId] and t.[TransportTypeId] = /**Email**/151
	where i.[Email] is not null 
	  and d.[Email] is null
	  and t.[UserId] is null;

	-- enabled Email transport, if user already has disabled transport
    update [Usr].[Transport] set [Enabled] = 1
	where [UserId] in (
	  select i.[Id]
	  from inserted i
	  inner join [Usr].[Transport] t on i.[Id] = t.[UserId] and t.[TransportTypeId] = /**Email**/151
	  left join deleted d on i.[Id] = d.[Id]
	where i.[Email] is not null 
	  and d.[Email] is null
	  and t.[Enabled] = 0
	) and [TransportTypeId] = /**Email**/151;

	--disable Email transport if user set Email = null
	update [Usr].[Transport] set [Enabled] = 0
	where [UserId] in (
	  select i.[Id]
	  from inserted i
	  inner join deleted d on i.[Id] = d.[Id]
	  inner join [Usr].[Transport] t on i.[Id] = t.[UserId] and t.[TransportTypeId] = /**Email**/151
	where i.[Email] is null 
	  and d.[Email] is not null
	  and t.[Enabled] = 1
	) and [TransportTypeId] = /**Email**/151;
END
GO

/******* [Msg].[MessageToUser_OnInsert_ProduceEvents] ******/
CREATE OR ALTER trigger [Msg].[MessageToUser_OnInsert_ProduceEvents]
on [Msg].[MessageToUser]
after insert 
as
  -- trigger produce notification event 'sent' for every FLChat message's addressee

  declare @link [dbo].[GuidBigintTable];

  -- insert event header and collect inserted event's id into @link
  insert into [Msg].[Event] ([EventTypeId], [CausedByUserId], [CausedByUserTransportTypeId], [MsgId])
  output 
    inserted.[MsgId], 
	inserted.[Id] 
  into @link
  select distinct     
	/**Incomming message event**/10,
	msg.[FromUserId],
	msg.[FromTransportTypeId],
	msg.[Id]
  from inserted i
  inner join [Msg].[Message] msg on i.[MsgId] = msg.[Id]
  inner join [Cfg].[TransportType] tt on i.ToTransportTypeId = tt.Id
  where tt.[InnerTransport] = 1 -- only inner transports (like FLChat and WebChat)
    --i.[ToTransportTypeId] = /**FLChat**/0 -- only for FLChat messages
	and msg.[IsDeleted] = 0 -- skip deleted message. Is there exist someone 
	                        -- who can send already deleted messages?
    and i.[IsWebChatGreeting] = 0 --skip webchat resent messages as greeting message
  ;

  -- insert event addressee
  insert into [Msg].[EventAddressee] ([Id], [UserId])
  select 
    l.[BInt], i.[ToUserId]
  from inserted i
  inner join [Cfg].[TransportType] tt on i.ToTransportTypeId = tt.Id
  inner join @link l on l.[Guid] = i.[MsgId]
  inner join [Usr].[Transport] t on t.[UserId] = i.[ToUserId] 
                                and t.TransportTypeId = i.[ToTransportTypeId] 
								and t.[Enabled] = 1
  where tt.[InnerTransport] = 1 -- only inner transports (like FLChat and WebChat)
    --i.[ToTransportTypeId] = /**FLChat**/0
	and i.[IsWebChatGreeting] = 0 --skip webchat resent messages as greeting message
  ;
GO

/****** [Msg].[MessageToUser_OnUpdate_ProduceEvents] *********/
create or alter trigger [Msg].[MessageToUser_OnUpdate_ProduceEvents]
on [Msg].[MessageToUser]
after update
as
  declare @link [dbo].[GuidBigintTable];
  
  insert into [Msg].[Event] ([EventTypeId], [CausedByUserId], [CausedByUserTransportTypeId], [MsgId])
  output 
    inserted.[MsgId],
	inserted.[Id]
    into @link 
  select distinct [EventType], [ToUserId], [ToTransportTypeId], [MsgId] 
  from (
    select 
      case when i.[IsFailed] <> d.[IsFailed]                                then /**failed**/5
	       when i.[IsRead] <> d.[IsRead] and d.[IsFailed] = 0               then /**read**/3
		   when i.[IsDelivered] <> d.[IsDelivered] 
		    and d.[IsFailed] = 0 and d.[IsRead] = 0                         then /**delivered**/2
		   when i.[IsSent] <> d.[IsSent] 
		    and d.[IsFailed] = 0 and d.[IsRead] = 0 and d.[IsDelivered] = 0 then /**Sent**/1
           else /**Skip**/null
	  end as [EventType],
      i.[ToUserId], 
	  i.[ToTransportTypeId],
	  i.[MsgId]
    from inserted i
    inner join deleted d on i.[MsgId] = d.[MsgId]
                        and i.[ToUserId] = d.[ToUserId]
                        and i.[ToTransportTypeId] = d.[ToTransportTypeId]
    inner join [Msg].[Message] msg on msg.[Id] = i.[MsgId]
    inner join [Cfg].[TransportType] tt on msg.[FromTransportTypeId] = tt.[Id]

    where msg.IsDeleted = 0
	  --and msg.[FromTransportTypeId] = /**FLChat**/0
	  and tt.[InnerTransport] = 1 -- only inner transports (like FLChat and WebChat)
	  and i.[IsWebChatGreeting] = 0 --skip webchat resent messages as greeting message
	 ) t
  where [EventType] is not null
  ;

  insert into [Msg].[EventAddressee] ([Id], [UserId])   
  select l.[BInt], msg.[FromUserId]
  from @link l
  inner join [Msg].[Message] msg on l.[Guid] = msg.[Id]
  ;
GO

/***** [Msg].[MessageToUser_OnUpdate_PreventRollbackFlags] **********/
create or alter trigger [Msg].[MessageToUser_OnUpdate_PreventRollbackFlags]
on [Msg].[MessageToUser]
after update
as
  if exists(
    select 
      i.[MsgId]
    from inserted i
    inner join deleted d on i.[MsgId] = d.[MsgId]
                        and i.[ToUserId] = d.[ToUserId]
                        and i.[ToTransportTypeId] = d.[ToTransportTypeId]
    where (i.[IsFailed] <> d.[IsFailed] and d.[IsFailed] = 1)
       or (i.[IsRead] <> d.[IsRead] and d.[IsRead] = 1)
	   or (i.[IsDelivered] <> d.[IsDelivered]  and d.[IsDelivered] = 1)
	   or (i.[IsSent] <> d.[IsSent] and d.[IsSent] = 1))
  begin
    throw 51001, 'Can not rollback message flags', 1;
  end
GO

/******** [Msg].[MessageToUser_OnInsert_WebChat] ********/
drop trigger if exists [Msg].[MessageToUser_OnInsert_WebChat];
GO

-- 2020.02.12
-- Trigger was deleted, because too slow. Need to investigate new version of it
/* 
CREATE OR ALTER TRIGGER [Msg].[MessageToUser_OnInsert_WebChat]
   ON  [Msg].[MessageToUser]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @msgId uniqueidentifier;
    declare @toId uniqueidentifier;	
	declare @link nvarchar(20);

	declare @text_t nvarchar(max);
    declare @url_t nvarchar(100);
    set @text_t = (select [Value] from [Cfg].[Settings] where [Name] = 'WEB_CHAT_SMS');
    set @url_t = (select [Value] from [Cfg].[Settings] where [Name] = 'WEB_CHAT_DEEP_URL');

	DECLARE icc CURSOR FORWARD_ONLY FAST_FORWARD
    FOR
      select 
	    i.[MsgId], i.[ToUserId]
	  from inserted i	
	  inner join [Msg].[Message] m on m.[Id] = i.[MsgId]

	  where i.[ToTransportTypeId] = /**WebChat**/100        
    open icc;
    fetch next from icc into @msgId, @toId;

	while @@FETCH_STATUS = 0
	begin
	  
	  set @link = (select [dbo].[RandomString](20));

	  insert into [Msg].[WebChatDeepLink] 
	    ([MsgId], [ToUserId], [ToTransportTypeId], [Link], [ExpireDate])
	  values
	    (@msgId, @toId, /**WebChat**/100 , @link, DATEADD(month, 1, GETUTCDATE()));  


	  fetch next from icc into @msgId, @toId;
	end
	
	close icc;
	deallocate icc;
END
GO*/

/****** Trigger [Msg].[Message_OnInsert_LastUsedTransport] ********/
CREATE OR ALTER TRIGGER [Msg].[Message_OnInsert_LastUsedTransport]
   ON  [Msg].[Message]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @ids [dbo].[GuidList];

    -- Insert statements for trigger here
    update [Usr].[User]
	set [DefaultTransportTypeId] = t.[TransportTypeId] 
	from 
	(
	  select DISTINCT
	    i.[FromUserId],
		LAST_VALUE(i.[FromTransportTypeId]) OVER (order by [Idx]) as [TransportTypeId] 
	  from inserted i
	inner join [Cfg].[MessageType] mt on i.[MessageTypeId] = mt.[Id]
	where mt.[ShowInHistory] = 1
	) t
	where [Id] = t.[FromUserId]	  

    update [Reg].[LastUsedTransport]
	set [TransportTypeId] = t.[TransportTypeId] 
	output inserted.[UserId] into @ids ([Guid])
	from 
	(
	  select DISTINCT
	    i.[FromUserId],
		LAST_VALUE(i.[FromTransportTypeId]) OVER (order by [Idx]) as [TransportTypeId] 
	  from inserted i
	inner join [Cfg].[MessageType] mt on i.[MessageTypeId] = mt.[Id]
	where mt.[ShowInHistory] = 1
	) t
	where [UserId] = t.[FromUserId]	  

	insert [Reg].[LastUsedTransport] ([UserId], [TransportTypeId])
	select DISTINCT
	    i.[FromUserId],
		LAST_VALUE(i.[FromTransportTypeId]) OVER (order by [Idx]) as [TransportTypeId] 
	from inserted i
	inner join [Cfg].[MessageType] mt on i.[MessageTypeId] = mt.[Id]
	where i.[FromUserId] not in (select [Guid] from @ids)
	  and mt.[ShowInHistory] = 1;
	
END
GO

/****** [Auth].[AuthToken_OnInsertNewbe] *******/
CREATE OR ALTER trigger [Auth].[AuthToken_OnInsertNewbe]
on [Auth].[AuthToken]
after insert
as
  --set up newbe user

  -- update user [SignUpDate] if empty
  update [Usr].[User]
  set [SignUpDate] = GETUTCDATE()
  where [SignUpDate] is null
    and [Id] in (select [UserId] from inserted);

  -- insert FLChat transport for new user
  insert into [Usr].[Transport] ([UserId], [TransportTypeId])
  select i.[UserId], /**FLChat**/0
  from inserted i
  left join [Usr].[Transport] t on i.[UserId] = t.[UserId] 
                               and t.[TransportTypeId] = /**FlChat**/0
  where t.[UserId] is null; 
  
  -- enable FLChat transport
  update [Usr].[Transport] 
  set [Enabled] = 1 
  where [Enabled] = 0 
    and [TransportTypeId] = /**FlChat**/0
    and [UserId] in (select [UserId] from inserted);
GO

/******* [Auth].[AuthToken_InsertVerification] ******/
create or alter trigger [Auth].[AuthToken_InsertVerification]
on [Auth].[AuthToken]
for insert
as
  if exists(
    select 1 
	from inserted i
	inner join [Usr].[User] u on i.[UserId] = u.[Id]
	where u.[Enabled] = 0)
  begin
    THROW 51000, 'Can not insert auth token for disabled user', 1;
    --RAISERROR('Can not insert auth token for disabled user', 10, 10)	

	--ROLLBACK TRANSACTION
	--RETURN
  end
GO

/******************************************************************************
	Procedures
******************************************************************************/

/***** [Usr].[Segment_UpdateMembers]  *******/
create or alter procedure [Usr].[Segment_UpdateMembers] 
	@segmentId uniqueidentifier,
	@newMembersIds [dbo].[GuidList] readonly
as
  delete from [Usr].[SegmentMember]
  where [SegmentId] = @segmentId
    and [UserId] not in (select [Guid] from @newMembersIds);

  insert into [Usr].[SegmentMember] ([SegmentId], [UserId])
  select @segmentId, n.[Guid]
  from @newMembersIds n
  where n.[Guid] not in (select [UserId] from [Usr].[SegmentMember] where [SegmentId] = @segmentId);
go

/***** [Usr].[Segment_UpdateMembers_FLUserNumber] ********/
CREATE OR ALTER PROCEDURE [Usr].[Segment_UpdateMembers_FLUserNumber]
	-- Add the parameters for the stored procedure here
	@segmentId uniqueidentifier, 
	@FLUserNumberList [dbo].[IntList] readonly
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    declare @guidList [dbo].[GuidList];
	insert into @guidList
	select u.[Id]
	from [Usr].[User] u
	inner join @FLUserNumberList l on u.[FLUserNumber] = l.[Value];
	
	exec [Usr].[Segment_UpdateMembers] @segmentId = @segmentId, @newMembersIds = @guidList;	
END
GO

/******** [Usr].[MergeUsers] **********/
create or alter procedure [Usr].[MergeUsers] 
		@master uniqueidentifier,
		@donor uniqueidentifier
as
begin
  --master's transport types 
  declare @mt table ([TransportTypeId] int NOT NULL);
  insert into @mt
  select [TransportTypeId] from [Usr].[Transport] where [UserId] = @master;

  --donors transport types
  declare @dt table (
    [TransportTypeId] int NOT NULL,
	[TransportOuterId] nvarchar(255) NULL);
  insert into @dt
  select [TransportTypeId], [TransportOuterId] 
  from [Usr].[Transport] 
  where [UserId] = @donor and [Enabled] = 1;

  --disable donor's transports 
  update [Usr].[Transport] set [TransportOuterId] = '' , [Enabled] = 0 
  where [UserId] = @donor and [TransportOuterId] is not null;
  update [Usr].[Transport] set [Enabled] = 0 
  where [UserId] = @donor and [TransportOuterId] is null;

  --update master's transports which type exists in master before merge
  update mt
  set [TransportOuterId] = dt.[TransportOuterId]
    , [Enabled] = 1
  from [Usr].[Transport] mt 
  inner join @dt dt on dt.[TransportTypeId] = mt.[TransportTypeId] 
  where mt.[UserId] = @master;

  --move new type of donor's transport into master
  insert into [Usr].[Transport] ([UserId], [TransportTypeId], [TransportOuterId])
  select @master, [TransportTypeId], [TransportOuterId]
  from @dt  
  where [TransportTypeId] not in (select * from @mt);	

  declare @donorMsg [dbo].[GuidList];

  --change sender in donor's messages

  --05.05.2020
  --update [Msg].[Message]
  --set [FromUserId] = @master
  --output inserted.[Id] into @donorMsg
  --where [FromUserId] = @donor;

  --update [Msg].[MessageToUser]
  --set [ToUserId] = @master
  --where [ToUserId] = @donor;

  --disable donor
  update [Usr].[User] set [Enabled] = 0 where [Id] = @donor;

  select * from @donorMsg;
end
GO

/****** [Msg].[MessagesSetDelivered] ******/
create or ALTER procedure [Msg].[MessagesSetDelivered]
	@userId uniqueidentifier,
	@ids [dbo].[GuidList] readonly,
	@transportTypeId integer
as
begin
  update [Msg].[MessageToUser] 
  set [IsDelivered] = 1 
  where [ToUserId] = @userId 
    and [ToTransportTypeId] = @transportTypeId
    and [MsgId] in (select * from @ids);
end
GO

/******* [Msg].[MessagesSetRead] ******/
create or ALTER procedure [Msg].[MessagesSetRead]
	@userId uniqueidentifier,
	@ids [dbo].[GuidList] readonly,
	@transportTypeId integer
as
begin
  update [Msg].[MessageToUser] 
  set [IsRead] = 1 
  where [ToUserId] = @userId 
    and [ToTransportTypeId] = @transportTypeId
    and [MsgId] in (select * from @ids);
end
go

/******* [Msg].[SetLastDeliveredEvent] ******/
create or alter procedure [Msg].[SetLastDeliveredEvent]
	@userId uniqueidentifier,
	@eventId bigint
as
begin
	update [Msg].[EventDelivered] 
	set [LastEventId] = @eventId 
	where [UserId] = @userId;

	if @@ROWCOUNT = 0
	begin
		insert into [Msg].[EventDelivered] ([UserId], [LastEventId])
		values (@userId, @eventId);
	end
end
GO

/******* [Msg].[Message_ProduceToUsers]  ********/
--20201209  Доработка - опция OPTION (MAXRECURSION 32767) при использовании  User_GetSelection
--20210306  Доработка - вставка с принудительным типом транспорта и ID провайдера
create or alter procedure [Msg].[Message_ProduceToUsers] 
	@msgId uniqueidentifier,
	@transportId int = null,
	@providerId uniqueidentifier = null
as 
begin
  declare @msgTypeId int;
  declare @from uniqueidentifier;
  
  --find message and get message type and sender
  select 
    @msgTypeId = [MessageTypeId], 
	@from = [FromUserId] 
  from [Msg].[Message] 
  where [Id] = @msgId;

  --if message has not found then error
  if @@ROWCOUNT = 0 THROW 50001, 'Message has not found', 1;

  -- table for addressees
  declare @toUsers table (
    [UserId] uniqueidentifier NOT NULL,
	[TransportTypeId] int NOT NULL
  );

  --arguments for selection function
  declare @include_ws [Usr].[UserIdDeep];
  declare @exclude_ws [dbo].[GuidList];
  declare @include [dbo].[GuidList];
  declare @exclude [dbo].[GuidList];
  declare @segments [dbo].[GuidList];

  /**Include with structure**/
  insert into @include_ws ([UserId], [Deep])
  select [UserId], [StructureDeep]
  from [Msg].[MessageToSelection] 
  where [MsgId] = @msgId and [WithStructure] = 1 and [Include] = 1;

  /**Exclude with structure**/
  insert into @exclude_ws 
  select [UserId] 
  from [Msg].[MessageToSelection] 
  where [MsgId] = @msgId and [WithStructure] = 1 and [Include] = 0;

  /**Include**/
  insert into @include 
  select [UserId] 
  from [Msg].[MessageToSelection] 
  where [MsgId] = @msgId and [WithStructure] = 0 and [Include] = 1;

  /**Exclude**/
  insert into @exclude 
  select [UserId] 
  from [Msg].[MessageToSelection] 
  where [MsgId] = @msgId and [WithStructure] = 0 and [Include] = 0;

  --segments
  insert into @segments
  select [SegmentId] from [Msg].[MessageToSegment] where [MsgId] = @msgId;

  --get all users from selection with transport
if @transportId is null
  insert into @toUsers
  select t.[UserId], t.[DefaultTransportTypeId]          
  from [Usr].[User_GetSelection](@from, @include_ws, @exclude_ws, @include, @exclude, @segments, default) s
  inner join (
	select * from [Usr].[UserMailingTransportView] where @msgTypeId = /**Mailing**/4 
	union all
	select * from [Usr].[UserDefaultTransportView] where @msgTypeId <> /**Mailing**/4 
	) t on s.[UserId] = t.[UserId]
	OPTION (MAXRECURSION 32767);
else
  insert into @toUsers
  select s.[UserId], @transportId
  from [Usr].[User_GetSelection](@from, @include_ws, @exclude_ws, @include, @exclude, @segments, default) s
	OPTION (MAXRECURSION 32767);

  --calculate count of users
  declare @cnt int;
  set @cnt = (select count(*) from @toUsers);

  --get limits
  declare @dayLimit int;
  declare @onceLimit int;
  select @dayLimit = [LimitForDay], @onceLimit = [LimitForOnce] 
  from [Cfg].[MessageType]
  where [Id] = @msgTypeId;

  declare @exceedOnce bit;
  declare @exceedDay bit;
  declare @sentOverToday int;
  set @exceedOnce = 0;
  set @exceedDay = 0;

  --check potential exceed
  if @onceLimit is not null and @cnt > @onceLimit
    set @exceedOnce = 1;
  else if @dayLimit is not null 
  begin 
    set @sentOverToday = (select count(*) 
			from [Msg].[MessageCountOverToday] 
			where [FromUserId] = @from and [MessageTypeId] = @msgTypeId);
    if @cnt + @sentOverToday > @dayLimit
		set @exceedDay = 1;
  end

  --return first result set: information about exceed
  select 
	  @cnt as [SelectionCount],
	  @exceedDay as [ExceedDay],
	  @dayLimit as [DayLimit],
	  @sentOverToday as [SentOverToday],
	  @exceedOnce as [ExceedOnce],
	  @onceLimit as [OnceLimit];
  --quit if has exceed
  if @exceedDay = 1 or @exceedOnce = 1
	return;

  declare @result table (
    [MsgId] uniqueidentifier NOT NULL,
	[ToUserId] uniqueidentifier NOT NULL,
	[ToTransportTypeId] int NOT NULL,
	[Idx] bigint NOT NULL,
	[IsFailed] bit NOT NULL,
	[IsSent] bit NOT NULL,
	[IsDelivered] bit NOT NULL,
	[IsRead] bit NOT NULL,
	[IsWebChatGreeting] bit NOT NULL,
	[PushMark] bit NOT NULL,
	[TransportProviderId] uniqueidentifier NULL
	);

  --insert addressees and second result set
  insert into [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId], [IsSent], [TransportProviderId])
  output 
    inserted.[MsgId],
	inserted.[ToUserId], 
	inserted.[ToTransportTypeId], 
	inserted.[Idx],
	inserted.[IsFailed],
	inserted.[IsSent],
	inserted.[IsDelivered],
	inserted.[IsRead],
	inserted.[IsWebChatGreeting],
 	inserted.[PushMark],
 	inserted.[TransportProviderId]  
	into @result
  select 
    @msgId, 
	u.[UserId], 
	u.[TransportTypeId],
	case when u.[TransportTypeId] = /**FLChat**/0 then 1 else 0 end,
	ISNULL(@providerId,tp.[Id])
  from @toUsers u
  left join [Msg].[MessageToUser] mtu 
	on mtu.[MsgId] = @msgId
	and mtu.[ToUserId] = u.[UserId]
	and mtu.[ToTransportTypeId] = u.[TransportTypeId]
  left join [Cfg].[TransportProvider] tp
	on tp.[TransportTypeId] = u.[TransportTypeId] and tp.[Enabled] = 1
  where mtu.[Idx] is null;

  --returns values
  select * from @result;
end
GO

/****** [Usr].[User_UpdateDeepChilds] *******/
CREATE OR ALTER PROCEDURE [Usr].[User_UpdateDeepChilds]	
	@userId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    declare @childs [dbo].[GuidList];
	
	insert into @childs 
	select [UserId] from [Usr].[User_GetChilds](@userId, default, default);

	insert into [Usr].[UserDeepChilds] ([UserId], [ChildUserId])
	select @userId, [Guid]
	from @childs c 
	where c.[Guid] not in (
	  select [ChildUserId] from [Usr].[UserDeepChilds] where [UserId] = @userId);

	delete from [Usr].[UserDeepChilds]
	where [UserId] = @userId 
	  and [ChildUserId] not in (select [Guid] from @childs)
END
GO

/****** [Msg].[DeepLinkStatsType] *******/
if TYPE_ID('[Msg].[DeepLinkStatsType]') is null begin
create type [Msg].[DeepLinkStatsType] as table (
	[TransportId] [nvarchar](255) NOT NULL,
	[SentTo] [int] NULL,
	[ViberStatus] [int] NULL,
	[SmsStatus] [int] NULL,
	[UpdatedTime] [datetime] NULL,
	[IsFinished] [bit] NOT NULL
)
PRINT 'create type [Msg].[DeepLinkStatsType]'
end
go

/****** [Msg].[UpdateWebChatDeepLinkStats] *******/
CREATE OR ALTER PROCEDURE [Msg].[UpdateWebChatDeepLinkStats]
	-- Add the parameters for the stored procedure here
	@table [Msg].[DeepLinkStatsType] readonly
AS
BEGIN

  declare @minval int;
  set @minval = -10000000;

  update wcdl
  set [SentTo] = t.[SentTo],
      [ViberStatus] = IIF(COALESCE(wcdl.[ViberStatus],@minval) < t.[ViberStatus], t.[ViberStatus], wcdl.[ViberStatus]),
      [SmsStatus] = IIF(COALESCE(wcdl.[SmsStatus],@minval) < t.[SmsStatus], t.[SmsStatus], wcdl.[SmsStatus]),
      [IsFinished] = t.[IsFinished],
      [UpdatedTime] = t.[UpdatedTime]

  from [Msg].[WebChatDeepLink] wcdl
  INNER JOIN [Msg].[MessageTransportId] mt on (wcdl.MsgId = mt.MsgId and wcdl.ToUserId = mt.ToUserId)
  INNER JOIN @table t on (mt.TransportId = t.TransportId)
  WHERE wcdl.[IsFinished] = 0 and
    (COALESCE(wcdl.[ViberStatus],@minval) < t.[ViberStatus]
   or COALESCE(wcdl.[SmsStatus],@minval) < t.[SmsStatus]
   or t.[IsFinished] = 1 )
END
GO
/****** [Usr].[GetUserDataKey] *******/
CREATE OR ALTER PROCEDURE [Usr].[GetUserDataKey]
	-- Add the parameters for the stored procedure here
	@Key nvarchar(255),
	@ID bigint out
AS
BEGIN
    --DECLARE @ID bigint
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SET @ID = (SELECT TOP 1 [ID] FROM [Usr].[UserDataKey] WHERE [Key] = @Key);
	IF @ID IS NULL BEGIN
	  INSERT INTO [Usr].[UserDataKey] ([Key]) VALUES (@Key);
	  SET @ID = @@IDENTITY;
	END
END
GO

/****** [Usr].[SetUserData] *******/
CREATE OR ALTER PROCEDURE [Usr].[SetUserData]
	-- Add the parameters for the stored procedure here
	@UserId uniqueidentifier,
	@Key nvarchar(255),
	@Data nvarchar(max)
AS
BEGIN
   
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @KeyId bigint;
	IF @Data is null 
	begin
	  DELETE d FROM [Usr].[UserData] d 
	  inner join  [Usr].[UserDataKey] k on(d.[KeyId] = k.[Id])
	  WHERE d.[UserId] = @UserId and k.[Key] = @Key
	end
	else
	begin
	  exec [Usr].[GetUserDataKey] @Key, @Id = @KeyId OUTPUT;
	  UPDATE [Usr].[UserData] set [Data] = @Data where [UserId] = @UserId and [KeyId] = @KeyId;
 	  IF @@ROWCOUNT = 0
	  begin
 	    INSERT INTO [Usr].[UserData] ([UserId], [KeyId], [Data]) VALUES (@UserId, @KeyId, @Data);
	  end
	end
END
GO

/****** [Usr].[DelUserData] *******/
CREATE OR ALTER PROCEDURE [Usr].[DelUserData]
	-- Add the parameters for the stored procedure here
	@UserId uniqueidentifier,
	@Key nvarchar(255)
AS
BEGIN   
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE d FROM [Usr].[UserData] d 
	  inner join  [Usr].[UserDataKey] k on(d.[KeyId] = k.[Id])
	  WHERE d.[UserId] = @UserId and k.[Key] = @Key
END
GO

/****** 2020.11.16 ******/
/****** Доп. данные к сообщениям ******/
/****** PROCEDURE [Msg].[GetMessageDataKey] *******/
CREATE OR ALTER PROCEDURE [Msg].[GetMessageDataKey]
	-- Add the parameters for the stored procedure here
	@Key nvarchar(255),
	@ID bigint out
AS
BEGIN
    --DECLARE @ID bigint
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SET @ID = (SELECT TOP 1 [ID] FROM [Msg].[MessageDataKey] WHERE [Key] = @Key);
	IF @ID IS NULL BEGIN
	  INSERT INTO [Msg].[MessageDataKey] ([Key]) VALUES (@Key);
	  SET @ID = @@IDENTITY;
	END
END
GO

/****** PROCEDURE [Msg].[SetMessageData] *******/
CREATE OR ALTER PROCEDURE [Msg].[SetMessageData]
	-- Add the parameters for the stored procedure here
	@MessageId uniqueidentifier,
	@Key nvarchar(255),
	@Data nvarchar(max)
AS
BEGIN
   
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @KeyId bigint;
	IF @Data is null 
	begin
	  DELETE d FROM [Msg].[MessageData] d 
	  inner join  [Msg].[MessageDataKey] k on(d.[KeyId] = k.[Id])
	  WHERE d.[MessageId] = @MessageId and k.[Key] = @Key
	end
	else
	begin
	  exec [Msg].[GetMessageDataKey] @Key, @Id = @KeyId OUTPUT;
	  UPDATE [Msg].[MessageData] set [Data] = @Data where [MessageId] = @MessageId and [KeyId] = @KeyId;
 	  IF @@ROWCOUNT = 0
	  begin
 	    INSERT INTO [Msg].[MessageData] ([MessageId], [KeyId], [Data]) VALUES (@MessageId, @KeyId, @Data);
	  end
	end
END
GO

/****** PROCEDURE [Msg].[DelMessageData] *******/
CREATE OR ALTER PROCEDURE [Msg].[DelMessageData]
	-- Add the parameters for the stored procedure here
	@MessageId uniqueidentifier,
	@Key nvarchar(255)
AS
BEGIN   
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE d FROM [Msg].[MessageData] d 
	  inner join  [Msg].[MessageDataKey] k on(d.[KeyId] = k.[Id])
	  WHERE d.[MessageId] = @MessageId and k.[Key] = @Key
END
GO

/****** 2020.12.29 ******/
/****** Включение/отключение индексов для построения кэшей ******/
/***** [System].[RebuildIndexes] ********/
CREATE OR ALTER PROCEDURE [System].[RebuildIndexes]
	-- Add the parameters for the stored procedure here
	@fillfactor INT 
AS
BEGIN
 DECLARE @TableName VARCHAR(255)
 DECLARE @sql NVARCHAR(500)
 DECLARE TableCursor CURSOR FOR
 SELECT QUOTENAME(OBJECT_SCHEMA_NAME([object_id]))+'.' + QUOTENAME(name) AS TableName FROM sys.tables

 OPEN TableCursor
 FETCH NEXT FROM TableCursor INTO @TableName
 WHILE @@FETCH_STATUS = 0
 BEGIN
  SET @sql = 'ALTER INDEX ALL ON ' + @TableName + ' REBUILD WITH (FILLFACTOR = ' + CONVERT(VARCHAR(3),@fillfactor) + ')'
  EXEC (@sql)
  FETCH NEXT FROM TableCursor INTO @TableName
 END
 CLOSE TableCursor
 DEALLOCATE TableCursor
END
GO

/***** [Cache].[WorkBefore] ********/
CREATE OR ALTER PROCEDURE [Cache].[WorkBefore]
AS
BEGIN
 ALTER INDEX [IDX__UsrUserDeepChilds__UserId]  on [Usr].[UserDeepChilds] DISABLE;
 ALTER INDEX [IDX__CacheStructureNodeCount__UserId_NodeId]
  on [Cache].[StructureNodeCount] DISABLE;
END
GO

/***** [Cache].[WorkAfter] ********/
CREATE OR ALTER PROCEDURE [Cache].[WorkAfter]
AS
BEGIN
-- ALTER INDEX [IDX__UsrUserDeepChilds__UserId]  on [Usr].[UserDeepChilds] REBUILD;
-- ALTER INDEX [IDX__CacheStructureNodeCount__UserId_NodeId]
--  on [Cache].[StructureNodeCount] REBUILD;
 exec [System].[RebuildIndexes] 80;
END
GO


/****** [Usr].[User_UpdateDeepChilds] *******/
/****** Обновлённая процедура пересчёта кэшей *******/
CREATE OR ALTER   PROCEDURE [Usr].[User_UpdateDeepChilds2]
--	@userId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY 
	BEGIN TRANSACTION 
	TRUNCATE TABLE [Usr].[UserDeepChilds];
	insert into [Usr].[UserDeepChilds] ([UserId], [ChildUserId])
	 select u.Id, ugc.[UserId]
	  from [Usr].[User] u
	  CROSS APPLY [Usr].[User_GetChilds4](u.[Id], default, default) ugc
	   where IsUseDeepChilds = 1
	  OPTION (MAXRECURSION 32767); 
	COMMIT TRANSACTION;
	END TRY 
	BEGIN CATCH 
	  ROLLBACK TRANSACTION;
	  --if we are here then error
	  THROW 50001, 'Error updating', 1;
	END CATCH 

END


/*******************************************************************************
	Structure node procedures
*******************************************************************************/
if TYPE_ID('[Ui].[StructureNodeInfo]') is null
create type [Ui].[StructureNodeInfo] as table (
  [NodeId] nvarchar(255) NOT NULL,
  [Name] nvarchar(255) NOT NULL,
  [Count] int  NOT NULL,
  [Final] [bit] NOT NULL
);
go

/******* [Ui].[StructureNode_GetInfo_Node] ********/
CREATE OR ALTER     procedure [Ui].[StructureNode_GetInfo_Node1]
		@nodeId uniqueidentifier,
		@userId uniqueidentifier
as
begin

if not exists(select 1 from [Ui].[StructureNode] where [Id] = @nodeId)
  return -1;

declare @result table (
  [Type] integer NOT NULL,
  [Id] uniqueidentifier NOT NULL,
  [Name] nvarchar(255) NOT NULL, 
  [Count] integer NOT NULL,
  [Order] smallint NOT NULL);

if (select [IsUseDeepChilds] from [Usr].[User] where [Id] = @userId) = 1 
begin

	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 0, sn.[Id], sn.[Name], coalesce(snc.[Count], 0), sn.[Order]
	from [Ui].[StructureNode] sn
	left join [Cache].[StructureNodeCount] snc on sn.[Id] = snc.[NodeId] and snc.[UserId] = @userId
	where sn.[ParentNodeId] = @nodeId;

	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 0, sn.[Id], sn.[Name], coalesce(snc.[Count], 0), sn.[Order]
	from [Ui].[StructureNode] sn
	left join [Cache].[StructureNodeCount] snc on sn.[Id] = snc.[NodeId] and snc.[UserId] = @userId
	where sn.[Id] = @nodeId;

  	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 
	  1,
	  s.[Id],
	  s.[Descr], 
	  coalesce(count(childs.[UserId]), 0) as [Count],
	  ns.[Order]  
	from [Ui].[StructureNodeSegment] ns
	inner join [Usr].[Segment] s on ns.[SegmentId] = s.[Id] and s.[IsDeleted] = 0
	left join [Usr].[SegmentMember] sm on sm.[SegmentId] = s.[Id]
	left join [Usr].[User_GetChildsSimple](@userId, default, default) childs on sm.[UserId] = childs.[UserId]
	where ns.[NodeId] = @nodeId
	group by s.[Id], s.[Descr], ns.[Order];

end
else
begin
	declare @nodesDeep table (
	  [NodeId] uniqueidentifier NOT NULL,
	  [InheritFromNodeId] uniqueidentifier NOT NULL);

	insert into @nodesDeep
	select [NodeId], [InheritFromNodeId] 
	from [Ui].[StructureNode_GetChilds](@nodeId);

	if @nodeId <> '00000000-0000-0000-0000-000000000000'
	begin
	  insert into @nodesDeep
	  select [NodeId], @nodeId as [InheritFromNodeId]
	  from @nodesDeep;

	  insert into @nodesDeep
	  values (@nodeId, @nodeId)
	end;

	with [NodesL1] as (
	  select n.[Id], n.[Name], n.[Order]
	  from [Ui].[StructureNode] n where n.[ParentNodeId] = @nodeId
	  union 
	  select n.[Id], n.[Name], n.[Order]
	  from [Ui].[StructureNode] n where n.[Id] = @nodeId),
	[LowSegments] as (
	  select 
		0 as [Type],
		n.[Id] as [Id],
		n.[Name],     
		n.[Order],
		ns.[SegmentId]	
	  from [NodesL1] n
	  left join @nodesDeep nd on n.[Id] = nd.[InheritFromNodeId] 
	  left join [Ui].[StructureNodeSegment] ns on nd.[NodeId] = ns.[NodeId]
	  union
	  select     
		1 as [Type],
		s.[Id] as [Id],
		s.[Descr] as [Name],     
		ns.[Order],
		ns.[SegmentId]
	  from [Ui].[StructureNodeSegment] ns
	  inner join [Usr].[Segment] s on ns.[SegmentId] = s.[Id] and s.[IsDeleted] = 0
	  where ns.[NodeId] = @nodeId)
	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 
	  ls.[Type],
	  ls.[Id],
	  ls.[Name], 
	  coalesce(count(DISTINCT childs.[UserId]), 0) as [Count],
	  ls.[Order]  
	from [LowSegments] ls
	left join [Usr].[SegmentMember] sm on sm.[SegmentId] = ls.[SegmentId]
	left join [Usr].[User_GetChildsSimple](@userId, default, default) childs on sm.[UserId] = childs.[UserId]
	group by ls.[Type], ls.[Id], ls.[Name], ls.[Order]
	order by ls.[Order];

	--get information about parents' node
	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 
	  2,
	  p.[ParentNodeId],
	  p.[Name],
	  (select count([UserId]) from [Usr].[User_GetParents](@userId, default)),
	  p.[Order]
	from [Ui].[StructureNodeParents] p
	where p.[ParentNodeId] = @nodeId;

end

-- result set #1: information about current node
select 
  'nod-' + cast(r.[Id] as nvarchar(255)) as [NodeId],
  r.[Name],  
  r.[Count] as [Count]
from @result r
where r.[Id] = @nodeId and r.[Type] = 0

--result set #2: information about child nodes
select 
  case when r.[Type] = 0 then 'nod-' + cast(r.[Id] as nvarchar(250))
       when r.[Type] = 1 then 'seg-' + cast(r.[Id] as nvarchar(250))
	   when r.[Type] = 2 then 'parents'
  end as [NodeId], 
  r.[Name], 
  r.[Count],
  cast(case when r.[Type] = 0 then 0
       else 1
  end as bit) as [Final]
from @result r
where r.[Id] <> @nodeId or (r.[Id] = @nodeId and r.[Type] <> 0)
order by r.[Order];

--return set #3: information about users is empty result set 
select top 0 * from [Usr].[User];

--return total count of users
select 0 as [TotalCount];

end;
GO

/******* [Ui].[StructureNode_GetInfo_Node] ********/
CREATE OR ALTER     procedure [Ui].[StructureNode_GetInfo_Node2]
		@nodeId uniqueidentifier,
		@userId uniqueidentifier
as

if not exists(select 1 from [Ui].[StructureNode] where [Id] = @nodeId)
  return -1;

declare @result table (
  [Type] integer NOT NULL,
  [Id] uniqueidentifier NOT NULL,
  [Name] nvarchar(255) NOT NULL, 
  [Count] integer NOT NULL,
  [Order] smallint NOT NULL);

if (select [IsUseDeepChilds] from [Usr].[User] where [Id] = @userId) = 1 
begin

	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 0, sn.[Id], sn.[Name], coalesce(snc.[Count], 0), sn.[Order]
	from [Ui].[StructureNode] sn
	left join [Cache].[StructureNodeCount] snc on sn.[Id] = snc.[NodeId] and snc.[UserId] = @userId
	where sn.[ParentNodeId] = @nodeId;

	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 0, sn.[Id], sn.[Name], coalesce(snc.[Count], 0), sn.[Order]
	from [Ui].[StructureNode] sn
	left join [Cache].[StructureNodeCount] snc on sn.[Id] = snc.[NodeId] and snc.[UserId] = @userId
	where sn.[Id] = @nodeId;

  	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 
	  1,
	  s.[Id],
	  s.[Descr], 
	  coalesce(count(childs.[UserId]), 0) as [Count],
	  ns.[Order]  
	from [Ui].[StructureNodeSegment] ns
	inner join [Usr].[Segment] s on ns.[SegmentId] = s.[Id] and s.[IsDeleted] = 0
	left join [Usr].[SegmentMember] sm on sm.[SegmentId] = s.[Id]
	left join [Usr].[User_GetChildsSimple2](@userId, default, default) childs on sm.[UserId] = childs.[UserId]
	where ns.[NodeId] = @nodeId
	group by s.[Id], s.[Descr], ns.[Order] OPTION (MAXRECURSION 32767);

end
else
begin
	declare @nodesDeep table (
	  [NodeId] uniqueidentifier NOT NULL,
	  [InheritFromNodeId] uniqueidentifier NOT NULL);

	insert into @nodesDeep
	select [NodeId], [InheritFromNodeId] 
	from [Ui].[StructureNode_GetChilds](@nodeId);

	if @nodeId <> '00000000-0000-0000-0000-000000000000'
	begin
	  insert into @nodesDeep
	  select [NodeId], @nodeId as [InheritFromNodeId]
	  from @nodesDeep;

	  insert into @nodesDeep
	  values (@nodeId, @nodeId)
	end;

	with [NodesL1] as (
	  select n.[Id], n.[Name], n.[Order]
	  from [Ui].[StructureNode] n where n.[ParentNodeId] = @nodeId
	  union 
	  select n.[Id], n.[Name], n.[Order]
	  from [Ui].[StructureNode] n where n.[Id] = @nodeId),
	[LowSegments] as (
	  select 
		0 as [Type],
		n.[Id] as [Id],
		n.[Name],     
		n.[Order],
		ns.[SegmentId]	
	  from [NodesL1] n
	  left join @nodesDeep nd on n.[Id] = nd.[InheritFromNodeId] 
	  left join [Ui].[StructureNodeSegment] ns on nd.[NodeId] = ns.[NodeId]
	  union
	  select     
		1 as [Type],
		s.[Id] as [Id],
		s.[Descr] as [Name],     
		ns.[Order],
		ns.[SegmentId]
	  from [Ui].[StructureNodeSegment] ns
	  inner join [Usr].[Segment] s on ns.[SegmentId] = s.[Id] and s.[IsDeleted] = 0
	  where ns.[NodeId] = @nodeId)
	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 
	  ls.[Type],
	  ls.[Id],
	  ls.[Name], 
	  coalesce(count(DISTINCT childs.[UserId]), 0) as [Count],
	  ls.[Order]  
	from [LowSegments] ls
	left join [Usr].[SegmentMember] sm on sm.[SegmentId] = ls.[SegmentId]
	left join [Usr].[User_GetChildsSimple2](@userId, default, default) childs on sm.[UserId] = childs.[UserId]
	group by ls.[Type], ls.[Id], ls.[Name], ls.[Order]
	order by ls.[Order] OPTION (MAXRECURSION 32767);

	--get information about parents' node
	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 
	  2,
	  p.[ParentNodeId],
	  p.[Name],
	  (select count([UserId]) from [Usr].[User_GetParents](@userId, default)),
	  p.[Order]
	from [Ui].[StructureNodeParents] p
	where p.[ParentNodeId] = @nodeId;

end

-- result set #1: information about current node
select 
  'nod-' + cast(r.[Id] as nvarchar(255)) as [NodeId],
  r.[Name],  
  r.[Count] as [Count]
from @result r
where r.[Id] = @nodeId and r.[Type] = 0

--result set #2: information about child nodes
select 
  case when r.[Type] = 0 then 'nod-' + cast(r.[Id] as nvarchar(250))
       when r.[Type] = 1 then 'seg-' + cast(r.[Id] as nvarchar(250))
	   when r.[Type] = 2 then 'parents'
  end as [NodeId], 
  r.[Name], 
  r.[Count],
  cast(case when r.[Type] = 0 then 0
       else 1
  end as bit) as [Final]
from @result r
where r.[Id] <> @nodeId or (r.[Id] = @nodeId and r.[Type] <> 0)
order by r.[Order];

--return set #3: information about users is empty result set 
select top 0 * from [Usr].[User];

--return total count of users
select 0 as [TotalCount];

GO

/******* [Ui].[StructureNode_GetInfo_Node3] ********/
CREATE procedure [Ui].[StructureNode_GetInfo_Node3]
		@nodeId uniqueidentifier,
		@userId uniqueidentifier
as

if not exists(select 1 from [Ui].[StructureNode] where [Id] = @nodeId)
  return -1;

declare @result table (
  [Type] integer NOT NULL,
  [Id] uniqueidentifier NOT NULL,
  [Name] nvarchar(255) NOT NULL, 
  [Count] integer NOT NULL,
  [Order] smallint NOT NULL);

if (select [IsUseDeepChilds] from [Usr].[User] where [Id] = @userId) = 1
	begin

		insert into @result ([Type], [Id], [Name], [Count], [Order])
		select 0, sn.[Id], sn.[Name], coalesce(snc.[Count], 0), sn.[Order]
		from [Ui].[StructureNode] sn
		left join [Cache].[StructureNodeCount] snc on sn.[Id] = snc.[NodeId] and snc.[UserId] = @userId
		where sn.[ParentNodeId] = @nodeId;

		insert into @result ([Type], [Id], [Name], [Count], [Order])
		select 0, sn.[Id], sn.[Name], coalesce(snc.[Count], 0), sn.[Order]
		from [Ui].[StructureNode] sn
		left join [Cache].[StructureNodeCount] snc on sn.[Id] = snc.[NodeId] and snc.[UserId] = @userId
		where sn.[Id] = @nodeId;

  		insert into @result ([Type], [Id], [Name], [Count], [Order])
		select 
		  1,
		  s.[Id],
		  s.[Descr], 
		  coalesce(count(childs.[UserId]), 0) as [Count],
		  ns.[Order]  
		from [Ui].[StructureNodeSegment] ns
		inner join [Usr].[Segment] s on ns.[SegmentId] = s.[Id] and s.[IsDeleted] = 0
		OUTER APPLY [Usr].[Segment_GetMembers2](s.[Id], @userId, default, default) childs
		where ns.[NodeId] = @nodeId
		group by s.[Id], s.[Descr], ns.[Order] OPTION (MAXRECURSION 32767);

	end
else
	begin
--if @nodeId <> '00000000-0000-0000-0000-000000000000'
		with [NodesDeep] as (
			select [NodeId], [InheritFromNodeId] from [Ui].[StructureNode_GetChilds](@nodeId)
			union all
			select [NodeId], @nodeId as [InheritFromNodeId] from [Ui].[StructureNode_GetChilds](@nodeId)
			where @nodeId <> '00000000-0000-0000-0000-000000000000'
			union all
			select @nodeId, @nodeId where @nodeId <> '00000000-0000-0000-0000-000000000000'
		),
		[NodesL1] as (
		  select n.[Id], n.[Name], n.[Order]
		  from [Ui].[StructureNode] n where n.[ParentNodeId] = @nodeId
		  union 
		  select n.[Id], n.[Name], n.[Order]
		  from [Ui].[StructureNode] n where n.[Id] = @nodeId),
		[LowSegments] as (
		  select 
			0 as [Type],
			n.[Id] as [Id],
			n.[Name],     
			n.[Order],
			ns.[SegmentId]	
		  from [NodesL1] n
		  left join NodesDeep nd on n.[Id] = nd.[InheritFromNodeId] 
		  left join [Ui].[StructureNodeSegment] ns on nd.[NodeId] = ns.[NodeId]
		  union
		  select     
			1 as [Type],
			s.[Id] as [Id],
			s.[Descr] as [Name],     
			ns.[Order],
			ns.[SegmentId]
		  from [Ui].[StructureNodeSegment] ns
		  inner join [Usr].[Segment] s on ns.[SegmentId] = s.[Id] and s.[IsDeleted] = 0
		  where ns.[NodeId] = @nodeId)
		insert into @result ([Type], [Id], [Name], [Count], [Order])
		select 
		  ls.[Type],
		  ls.[Id],
		  ls.[Name], 
		  coalesce(count(DISTINCT childs.[UserId]), 0) as [Count],
		  ls.[Order]  
		from [LowSegments] ls
		OUTER APPLY [Usr].[Segment_GetMembers2](ls.[SegmentId], @userId, default, default) childs
		group by ls.[Type], ls.[Id], ls.[Name], ls.[Order]
		order by ls.[Order] OPTION (MAXRECURSION 32767);

		--get information about parents' node
		insert into @result ([Type], [Id], [Name], [Count], [Order])
		select 
		  2,
		  p.[ParentNodeId],
		  p.[Name],
		  (select count([UserId]) from [Usr].[User_GetParents2](@userId, default)),
		  p.[Order]
		from [Ui].[StructureNodeParents] p
		where p.[ParentNodeId] = @nodeId;

	end

-- result set #1: information about current node
select 
  'nod-' + cast(r.[Id] as nvarchar(255)) as [NodeId],
  r.[Name],  
  r.[Count] as [Count]
from @result r
where r.[Id] = @nodeId and r.[Type] = 0

--result set #2: information about child nodes
select 
  case when r.[Type] = 0 then 'nod-' + cast(r.[Id] as nvarchar(250))
       when r.[Type] = 1 then 'seg-' + cast(r.[Id] as nvarchar(250))
	   when r.[Type] = 2 then 'parents'
  end as [NodeId], 
  r.[Name], 
  r.[Count],
  cast(case when r.[Type] = 0 then 0
       else 1
  end as bit) as [Final]
from @result r
where r.[Id] <> @nodeId or (r.[Id] = @nodeId and r.[Type] <> 0)
order by r.[Order];

--return set #3: information about users is empty result set 
select top 0 * from [Usr].[User];

--return total count of users
select 0 as [TotalCount];
GO


/******* [Ui].[StructureNode_GetInfo_Node] ********/
CREATE OR ALTER     procedure [Ui].[StructureNode_GetInfo_Node]
		@nodeId uniqueidentifier,
		@userId uniqueidentifier
as
BEGIN
--  Первый вариант
--  EXEC [Ui].[StructureNode_GetInfo_Node1] @nodeId, @userId;
--  Второй тестовый вариант
--  Третий ускоренный вариант
  EXEC [Ui].[StructureNode_GetInfo_Node3] @nodeId, @userId ;

END;
GO



/******** [Ui].[StructureNode_GetInfo_Segment] *********/
CREATE OR ALTER procedure [Ui].[StructureNode_GetInfo_Segment]
		@segmentId uniqueidentifier,
		@userId uniqueidentifier,
		@offset int = null,
		@count int = null
as

declare @users [dbo].[GuidList];
declare @cnt int;

--get all childs users from user's structure
insert into @users
--  20201207  Добавление OPTION (MAXRECURSION 32767)
select [UserId] from [Usr].[Segment_GetMembers](@segmentId, @userId, default, default) OPTION (MAXRECURSION 32767);

set @cnt = @@ROWCOUNT;

--result set#1: information about current segment, include count of users
select 
  'seg-' + cast(s.[Id] as nvarchar(255)) as [NodeId],
  s.[Descr] as [Name],  
  @cnt as [Count]
from [Usr].[Segment] s
where s.[Id] = @segmentId;

-- if not found then break
if @@ROWCOUNT = 0
  return -1;

--data type for nodes result set
declare @nodes as [Ui].[StructureNodeInfo];

--result set #2: information about child nodes is empty result set 
select [NodeId], [Name], [Count], [Final] from @nodes;

--return all users from segment
if @offset is null and @count is null
	select * from [Usr].[User] where [Id] in (select * from @users) 
	order by 
		case when [FullName] is not null then 1
			 else 0 end desc, 
		case when UNICODE([FullName]) >= 1024 and UNICODE([FullName]) <= 1279 then 1
			 else 0 end desc,
		[FullName];
else
	select * from [Usr].[User] where [Id] in (select * from @users)
	order by 
		case when [FullName] is not null then 1
			 else 0 end desc, 
		case when UNICODE([FullName]) >= 1024 and UNICODE([FullName]) <= 1279 then 1
			 else 0 end desc,
		[FullName]
    OFFSET @offset ROWS FETCH NEXT @count ROWS ONLY;

--return total count of users
select @cnt as [TotalCount];

GO


/***********  [Ui].[StructureNode_GetInfo_Parents] *********/
CREATE OR ALTER procedure [Ui].[StructureNode_GetInfo_Parents]	
		@userId uniqueidentifier
as

declare @users table ([UserId] uniqueidentifier, [Deep] int);

--get all childs users from user's structure
insert into @users
select [UserId], [Deep] from [Usr].[User_GetParents](@userId, default);

--result set#1: information about current segment, include count of users
select 
  'parents',
  (select top 1 [Name] from [Ui].[StructureNodeParents]) as [Name],  
  (select count(*) as [Count] from @users) as [Count];

--data type for nodes result set
declare @nodes as [Ui].[StructureNodeInfo];

--result set #2: information about child nodes is empty result set 
select [NodeId], [Name], [Count], [Final] from @nodes;

--return all users from segment
select u.* from @users p
inner join [Usr].[User] u on p.[UserId] = u.[Id]
order by p.[Deep] desc

--return total count of users
select count(DISTINCT [UserId]) as [TotalCount] from @users;

GO

/***********  [Ui].[StructureNode_GetInfo] *********/
create or ALTER procedure [Ui].[StructureNode_GetInfo]
		@sid nvarchar(255),
		@userId uniqueidentifier,
		@offset int = null,
		@count int = null
as

declare @prefix nvarchar(4);
declare @id uniqueidentifier;

if @sid = 'parents' 
  exec [Ui].[StructureNode_GetInfo_Parents] @userId = @userId;
else
begin
	if @sid is not null begin
	  set @prefix = SUBSTRING (@sid, 1, 4);
	  set @id = cast(SUBSTRING (@sid, 5, 500) as uniqueidentifier);
	end else begin
	  set @prefix = 'nod-';
	  set @id = '00000000-0000-0000-0000-000000000000';
	end

	if @prefix = 'nod-'
	  exec [Ui].[StructureNode_GetInfo_Node] @nodeId = @id, @userId = @userId;
	else if @prefix = 'seg-'
	  exec [Ui].[StructureNode_GetInfo_Segment] 
		@segmentId = @id, 
		@userId = @userId, 
		@offset = @offset,
		@count = @count;
end
GO

/******* [Cache].[Update_StructureNodeCount] **********/
CREATE OR ALTER PROCEDURE [Cache].[Update_StructureNodeCount]
	-- Add the parameters for the stored procedure here
	@userId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @result TABLE (
		[NodeId] uniqueidentifier NOT NULL UNIQUE,
		[Count] integer NOT NULL
	);

--  20201207  Добавление OPTION (MAXRECURSION 32767) для [Usr].[User_UiNodesCount], использующую User_GetChildsSimple
	insert into @result
	select [NodeId], [Count] from [Usr].[User_UiNodesCount](@userId)
	OPTION (MAXRECURSION 32767); 

	delete from [Cache].[StructureNodeCount] where [UserId] = @userId;
	insert into [Cache].[StructureNodeCount]
	select [NodeId], @userId, [Count] from @result;
END
GO

/****** 2020.12.29 ******/
/****** Обновлённая процедура пересчёта кэшей ******/

CREATE OR ALTER   PROCEDURE [Cache].[Update_StructureNodeCount2]
	-- Add the parameters for the stored procedure here
--	@userId uniqueidentifier
AS
BEGIN
	BEGIN TRY 
	BEGIN TRANSACTION 
	TRUNCATE TABLE [Cache].[StructureNodeCount];

	insert into [Cache].[StructureNodeCount]
	 select nc.[NodeId], u.Id [UserId], nc.[Count]
	  from [Usr].[User] u
	  CROSS APPLY [Usr].[User_UiNodesCount3](u.[Id]) nc
	   where IsUseDeepChilds = 1
	  OPTION (MAXRECURSION 32767); 
	COMMIT TRANSACTION;
	END TRY 
	BEGIN CATCH 
	  ROLLBACK TRANSACTION;
	  --if we are here then error
	  THROW 50001, 'Error updating', 1;
	END CATCH 
END
GO

/*******************************************************************
	Procedures for tests
********************************************************************/

/******* [Test].[GetTwoUsersWithTransport] ******/
create or alter procedure [Test].[GetTwoUsersWithTransport]
	@TransportType1 integer,
	@TransportType2 integer,
	@UserId1 uniqueidentifier out,
	@UserId2 uniqueidentifier out
as
begin
--seek user with FLChat for sending message
set @UserId1 = (select top 1 u.[Id]
  from [Usr].[Transport] t
  inner join [Usr].[User] u on t.[UserId] = u.[Id]
  where t.[TransportTypeId] = @TransportType1
    and t.[Enabled] = 1
    and u.[Enabled] = 1);

--if not exists, then create one
if @UserId1 is null
begin
  set @UserId1 = NEWID();
  insert into [Usr].[User] ([Id], [FullName]) values (@UserId1, 'Created by MessageToUser_OnUpdate_ProduceEvents test');
  insert into [Usr].[Transport] ([UserId], [TransportTypeId]) values (@UserId1, @TransportType1);
end

-- seek second user with another transport for addressee
set @UserId2 = (select top 1 u.[Id]
  from [Usr].[Transport] t
  inner join [Usr].[User] u on t.[UserId] = u.[Id]
  where t.[TransportTypeId] = @TransportType2
    and t.[Enabled] = 1
    and u.[Enabled] = 1
	and u.[Id] <> @UserId1 );

--if not exists, then create one
if @UserId2 is null
begin
  set @UserId2 = NEWID();
  insert into [Usr].[User] ([Id], [FullName]) values (@UserId2, 'Created by MessageToUser_OnUpdate_ProduceEvents test');
  insert into [Usr].[Transport] ([UserId], [TransportTypeId], [TransportOuterId]) 
    values (@UserId2, @TransportType2, cast(@UserId2 as nvarchar(255)));
end
end
go

/******** [Test].[GetUsers] ***********/
create or alter procedure [Test].[GetUsers]
	@count integer
as
begin

  SET NOCOUNT ON;

  declare @users [dbo].[GuidList];
  
  insert into @users 
  select Top (@count) [Id] from [Usr].[User] where [Enabled] = 1;

  if @@ROWCOUNT < @count
  begin
    declare @realcnt integer;
    set @realcnt = (select count(*) from @users);
    while @realcnt < @count
	begin
	  insert into @users
	  select * from (
	    insert into [Usr].[User] ([FullName]) 
	    output inserted.[Id]
	    values ('Created by [Test].[GetUsers]')
	  ) as t;	  
	end
  end

  select * from @users;
end;
GO

/******** [Test].[SendMessage] *********/
create or alter procedure [Test].[SendMessage]
   @from uniqueidentifier,
   @fromTrans integer,
   @to uniqueidentifier,
   @toTrans integer,
   @text nvarchar = 'Test message'

as
begin
  SET NOCOUNT ON;

  declare @msg uniqueidentifier;
  set @msg = NEWID();

  insert into [Msg].[Message] ([Id], [MessageTypeId], [FromUserId], [FromTransportTypeId], [Text])
  values (@msg, 0, @from, @fromTrans, @text);

  insert into [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId], [IsSent])
  values (@msg, @to, @toTrans, case when @toTrans = 0 then 1 else 0 end);

  select m.[Id], m.[Idx], mtu.[Idx]
  from [Msg].[Message] m
  inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
  where m.[Id] = @msg;

end
go

/**************************************************************
	Common data
***************************************************************/

/**** Bot user ****/
if exists(select * from [Usr].[User] 
	where [Id] = '00000000-0000-0000-0000-000000000000')
begin
  update [Usr].[User] 
  set 
	[Enabled] = 1,
	[IsConsultant] = 1,
	[OwnerUserId] = null,
	[Phone] = null,
	[Email] = null,
	[SignUpDate] = null,
	[IsBot] = 1
  where
    [Id] = '00000000-0000-0000-0000-000000000000'
	and [Enabled] <> 1
	and [IsBot] <> 1;
end
else
begin
  insert into [Usr].[User] ([Id], [FullName], [IsConsultant], [Enabled], [IsBot])
  values ('00000000-0000-0000-0000-000000000000', 'Faberlic Chat', 1, 1, 1);
end

if not exists(select * from [Usr].[Transport] where [UserId] = '00000000-0000-0000-0000-000000000000')
begin
  insert into [Usr].[Transport] ([UserId], [TransportTypeId], [Enabled])
  values ('00000000-0000-0000-0000-000000000000', /**FLChat**/0, 1);
end
GO

/**** Sms Bot user ****/
if exists(select * from [Usr].[User] 
	where [Id] = '00000000-0000-0000-0000-000000000001')
begin
  update [Usr].[User] 
  set 
	[Enabled] = 1,
	[IsConsultant] = 1,
	[OwnerUserId] = null,
	[Phone] = null,
	[Email] = null,
	[SignUpDate] = null,
	[FullName] = 'Sms Bot',
	[IsBot] = 1
  where
    [Id] = '00000000-0000-0000-0000-000000000001';
--	and [Enabled] <> 1
--	and [IsBot] <> 1;
 PRINT 'SMS Bot updated'
end
else
begin
  insert into [Usr].[User] ([Id], [FullName], [IsConsultant], [Enabled], [IsBot])
  values ('00000000-0000-0000-0000-000000000001', 'Sms Bot', 1, 1, 1);
 PRINT 'SMS Bot inserted'
end
GO

if not exists(select * from [Usr].[Transport] where [UserId] = '00000000-0000-0000-0000-000000000001' and [TransportTypeId] = 150)
begin
  insert into [Usr].[Transport] ([UserId], [TransportTypeId], [Enabled])
  values ('00000000-0000-0000-0000-000000000001', /**Sms**/150, 1);
 PRINT 'SMS Bot Transport inserted'
end
GO


declare @dt as table (
	[UserId] uniqueidentifier,
	[Token] nvarchar(255),
	[ExpireBy] int);

insert into @dt ([UserId], [Token], [ExpireBy])
values ('00000000-0000-0000-0000-000000000000', 
  'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjAwMDAwMDAwLTAwMDAtMDAwMC0wMDAwLTAwMDAwMDAwMDAwMCIsImlzcyI6IjIwMTktMDYtMjJUMTE6NDE6NTYuNTUzMzcyMyswODowMCIsImV4cCI6NjMwNzIwMDAwfQ.kxR4cp4DoEDTL4POsmBxxBeFGiDXHSVMcjKvpRVxoSc',
  630720000);

delete from [Auth].[AuthToken]
where [Id] in (
	select [Id] from [Auth].[AuthToken] t
	inner join @dt d on t.[Token] = d.[Token]
	where t.[UserId] <> d.[UserId]);

update t
set [ExpireBy] = d.[ExpireBy]
from [Auth].[AuthToken] t
inner join @dt d on t.[Token] = d.[Token] and t.[UserId] = d.[UserId]
where t.[ExpireBy] <> d.[ExpireBy];

insert into [Auth].[AuthToken] ([UserId], [Token], [ExpireBy], [IssueDate])
select d.[UserId], d.[Token], d.[ExpireBy], GETUTCDATE()
from @dt d
left join [Auth].[AuthToken] t on t.[Token] = d.[Token] and t.[UserId] = d.[UserId]
where t.[Id] is null

GO

/******** Root node ***********/
if exists(select * from [Ui].[StructureNode] where [Id] = '00000000-0000-0000-0000-000000000000')
begin
  update [Ui].[StructureNode]
  set 
    [Name] = 'Root node',
	[ParentNodeId] = null,
	[IsShowSegments] = 1,
	[IsShowParentUsers] = 0,
	[Order] = 0
  where
    [Id] = '00000000-0000-0000-0000-000000000000';
end
else
begin
  insert into [Ui].[StructureNode] ([Id], [Name], [ParentNodeId], [IsShowSegments], [IsShowParentUsers])
  values ('00000000-0000-0000-0000-000000000000', 'Root node', null, 1, 0);
end

USE [FLChat]
GO

CREATE SCHEMA [Cfg]
GO

CREATE SCHEMA [Usr]
GO

CREATE SCHEMA [Msg]
GO

create schema [Auth]
GO

CREATE TABLE [Usr].[User] (
  [Id] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
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
  constraint [FK__UsrUser__UsrUserOwned] foreign key ([OwnerUserId])
    references [Usr].[User] ([Id])  
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrUser_Phone]
ON [Usr].[User]([Phone])
WHERE [Phone] IS NOT NULL;
GO

CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrUser_Email]
ON [Usr].[User]([Email])
WHERE [Email] IS NOT NULL;
GO

CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrUser_Login]
ON [Usr].[User]([Login])
WHERE [Login] IS NOT NULL;
GO

CREATE TABLE [Cfg].[TransportType] (
  [Id] integer NOT NULL PRIMARY KEY,
  [Name] varchar(50) NOT NULL UNIQUE,
  [Enabled] bit DEFAULT 1 NOT NULL
)
GO

INSERT INTO [Cfg].[TransportType] ([Id], [Name])
VALUES 
(0, 'FLChat'),
(1, 'Telegram'),
(2, 'WhatsApp'),
(3, 'Viber'),
(4, 'VK'),
(5, 'OK');
GO

CREATE TABLE [Usr].[Transport] (
  [UserId] uniqueidentifier NOT NULL,
  [TransportTypeId] integer NOT NULL,
  [TransportOuterId] nvarchar(255) NOT NULL,
  [Enabled] bit NOT NULL default 1,
  constraint [PK__UsrTransport] primary key ([UserId], [TransportTypeId]),
  constraint [FK__UsrTransport__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id]),
  constraint [FK_UsrTransport__CfgTransportType] foreign key ([TransportTypeId])  
    references [Cfg].[TransportType] ([Id]),
  constraint [UNQ__UsrTransport] unique ([TransportTypeId], [TransportOuterId])
)
GO

alter table [Usr].[User] add constraint [FK__UsrUser__UsrTransport]
  foreign key ([Id], [DefaultTransportTypeId])  
  references [Usr].[Transport]([UserId], [TransportTypeId])
GO

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

CREATE TABLE [Cfg].[MessageType] (
  [Id] integer NOT NULL PRIMARY KEY,
  [Name] nvarchar(50) NOT NULL UNIQUE
)
GO

INSERT INTO [Cfg].[MessageType] ([Id], [Name])
VALUES
(0, 'Personal'),
(1, 'Group'),
(2, 'Broadcast')
GO

CREATE TABLE [Msg].[Message] (
  [Id] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
  [PostTm] datetime NOT NULL DEFAULT GETUTCDATE(),
  [MessageTypeId] integer NOT NULL,
  [FromUserId] uniqueidentifier NOT NULL,
  [FromTransportTypeId] integer NOT NULL,
  [AnswerToId] uniqueidentifier NULL,
  [Text] nvarchar(4000) NULL, 
  [IsDeleted] bit NOT NULL DEFAULT 0,
  constraint [FK__MsgMessage__FromTransport] foreign key ([FromUserId], [FromTransportTypeId])
    references [Usr].[Transport] ([UserId], [TransportTypeId]),
  constraint [FK__MsgMessage__CfgMessageType] foreign key ([MessageTypeId])
    references [Cfg].[MessageType] ([Id]),
  constraint [FK__MsgMessage__AnswerMessage] foreign key ([AnswerToId])
    references [Msg].[Message] ([Id])
)
GO

CREATE TABLE [Msg].[MessageToUser] (
  [MsgId] uniqueidentifier NOT NULL,
  [ToUserId] uniqueidentifier NOT NULL,
  [ToTransportTypeId] integer NOT NULL,
  [IsFailed] bit NOT NULL default 0,
  [IsSent] bit NOT NULL default 0,
  [IsDelivered] bit NOT NULL default 0,
  [IsRead] bit NOT NULL default 0,
  constraint [PK__MsgMessageToUser] primary key ([MsgId], [ToUserId], [ToTransportTypeId]),
  constraint [FK__MsgMessageToUser__MsgMessage] 
    foreign key ([MsgId])
	references [Msg].[Message] ([Id]),
  constraint [FK__MsgMessageToUser__UsrTransport] 
    foreign key ([ToUserId], [ToTransportTypeId])
    references [Usr].[Transport] ([UserId], [TransportTypeId])
)
GO

CREATE TABLE [Cfg].[EventType] (
  [Id] integer NOT NULL primary key,
  [Name] varchar(50) NOT NULL UNIQUE
)

INSERT INTO [Cfg].[EventType] ([Id], [Name])
values 
(1, 'Message sent'),
(2, 'Message delivered'),
(3, 'Message read'),
(4, 'Message deleted'),
(5, 'Message failed'),
(100, 'User avatar changed')
GO

CREATE TABLE [Msg].[Event] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [Tm] datetime NOT NULL DEFAULT GETUTCDATE(),
  [EventTypeId] integer NOT NULL,  
  [CausedByUserId] uniqueidentifier NOT NULL,
  [MsgId] uniqueidentifier NOT NULL,
  constraint [FK__MsgEvent__CfgEventType] foreign key ([EventTypeId])
    references [Cfg].[EventType] ([Id]),
  constraint [FK__MsgEvent__UsrUserCaused] foreign key ([CausedByUserId])
    references [Usr].[User] ([Id]),
  constraint [FK__MsgEvent__MsgMessage] foreign key ([MsgId])
    references [Msg].[Message] ([Id])
)
GO

CREATE TABLE [Msg].[EventAddressee] (
  [Id] bigint NOT NULL,
  [UserId] uniqueidentifier NOT NULL,
  constraint [PK__MsgEventAddressee] primary key ([Id], [UserId]),
  constraint [FK__MsgEventAddressee__MsgEvent] foreign key ([Id])
    references [Msg].[Event] ([Id]),
  constraint [FK__MsgEventAddressee__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id])

)
GO

CREATE TABLE [Msg].[EventDelivered] (
  [UserId] uniqueidentifier NOT NULL PRIMARY KEY,
  [LastEventId] bigint NOT NULL,
  constraint [FK__MsgEventDelivered__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id]),
  constraint [FK__MsgEventDelivered__MsgEvent] foreign key ([LastEventId])
    references [Msg].[Event] ([Id])
)
GO

create table [Auth].[SmsCode] (
  [UserId] uniqueidentifier NOT NULL PRIMARY KEY,
  [Code] integer NOT NULL,
  [IssueDate] datetime NOT NULL DEFAULT getdate(),
  [ExpireBySec] integer NOT NULL,
  constraint [FK__AuthSmsCode__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id])
)
GO

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

create table [Usr].[UserAvatar] (
  [UserId] uniqueidentifier NOT NULL PRIMARY KEY,
  [Data] varbinary(max),
  constraint [FK__UsrUserAvatar__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id])
)
GO

create trigger [Usr].[Transport__OnInsertFLChat]
on [Usr].[Transport]
after insert
as
  -- set OuterTransportId for inner transport
  update  [Usr].[Transport] 
  set [TransportOuterId] = cast(NEWID() as nvarchar(255))
  where [UserId] in (select [UserId] from inserted where [TransportTypeId] = 0)  
GO
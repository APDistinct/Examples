USE [FLChat]
GO

CREATE SCHEMA [Cfg]
GO

CREATE SCHEMA [Usr]
GO

CREATE SCHEMA [Msg]
GO

CREATE TABLE [Usr].[User] (
  [Id] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
  [Enabled] bit NOT NULL default 1,
  [PartnerId] uniqueidentifier NULL,
  [FullName] nvarchar(255) NULL,
  [IsConsultant] bit NOT NULL default 0,
  [RegistrationDate] date NULL,
  [InsertDate] datetime NOT NULL DEFAULT GETUTCDATE(),
  [SignUpDate] datetime NULL,
  [Phone] varchar(20) NULL,
  [Email] nvarchar(255) NULL,
  [Login] varchar(255) NULL,
  [PswHash] varchar(255) NULL,
  [OwnerUserId] uniqueidentifier null
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrUser_PartnerId]
ON [Usr].[User]([PartnerId])
WHERE [PartnerId] IS NOT NULL;
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

CREATE TABLE [Cfg].[TokenType] (
  [Id] integer NOT NULL PRIMARY KEY,
  [Name] nvarchar(50) NOT NULL UNIQUE
)
GO

INSERT INTO [Cfg].[TokenType] ([Id], [Name])
VALUES
(0, 'Auth'),
(1, 'SMS Log in'),
(101, 'Telegram deep link')
GO

CREATE TABLE [Usr].[AuthToken] (
  [TokenTypeId] integer NOT NULL,
  [Token] varchar(255) NOT NULL,
  [UserId] uniqueidentifier NOT NULL,
  [GenTime] datetime NOT NULL DEFAULT GETDATE(),
  [ExpTime] datetime NULL,
  constraint [PK__UsrAuthToken] primary key ([TokenTypeId], [Token]),
  constraint [FK__UsrAuthToken__CfgTokenType] foreign key ([TokenTypeId])
    references [Cfg].[TokenType] ([Id]),
  constraint [FK__UsrAuthToken__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id])
)
GO

CREATE TABLE [Cfg].[TransportType] (
  [Id] integer NOT NULL PRIMARY KEY,
  [Name] nvarchar(50) NOT NULL UNIQUE
)
GO

INSERT INTO [Cfg].[TransportType] ([Id], [Name])
VALUES 
--(0, 'FLChat'),
(1, 'Telegram'),
(2, 'WhatsApp'),
(3, 'Viber'),
(4, 'VK'),
(5, 'OK');
GO

CREATE TABLE [Usr].[Transport] (
  [Id] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
  [UserId] uniqueidentifier NOT NULL,
  [TransportTypeId] integer NOT NULL,
  [TransportOuterId] nvarchar(255) NOT NULL,
  [Enabled] bit NOT NULL default 1,
  [Default] bit NOT NULL default 0,
  constraint [FK__UsrTransport__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id]),
  constraint [FK_UsrTransport__CfgTransportType] foreign key ([TransportTypeId])  
    references [Cfg].[TransportType] ([Id]),
  constraint [UNQ__UsrTransport] unique ([TransportTypeId], [TransportOuterId])
)
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
(2, 'Broadcast'),
(100, 'Initial chat')
GO

CREATE TABLE [Cfg].[MsgStatus] (
  [Id] integer NOT NULL PRIMARY KEY,
  [Name] nvarchar(50) NOT NULL UNIQUE
)
GO

INSERT INTO [Cfg].[MsgStatus] ([Id], [Name])
VALUES 
(1, 'sent'),
(2, 'delivered'),
(3, 'read'),
(-1, 'failed'),
(0, 'deleted');
GO

CREATE TABLE [Msg].[Message] (
  [Id] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
  [FromUserId] uniqueidentifier NOT NULL,
  [MessageTypeId] integer NOT NULL,
  [ToUserId] uniqueidentifier NULL,
  [TransportId] uniqueidentifier NULL,
  [TransportMsgId] nvarchar(100) NULL,
  [Income] bit NULL,
  [AnswerToId] uniqueidentifier NULL,
  [StatusId] integer NOT NULL,
  [Text] nvarchar(4000) NULL, 
  [PostTm] datetime NOT NULL DEFAULT GETUTCDATE(),
  constraint [FK__MsgMessage__FromUser] foreign key ([FromUserId])
    references [Usr].[User] ([Id]),
  constraint [FK__MsgMessage__ToUser] foreign key ([ToUserId])
    references [Usr].[User] ([Id]),
  constraint [FK__MsgMessage__CfgMessageType] foreign key ([MessageTypeId])
    references [Cfg].[MessageType] ([Id]),
  constraint [FK__MsgMessage__UsrTransport] foreign key ([TransportId])
    references [Usr].[Transport] ([Id]),
  constraint [FK__MsgMessage__AnswerMessage] foreign key ([AnswerToId])
    references [Msg].[Message] ([Id]),
  constraint [FK__MsgMessage__CfgMsgStatus] foreign key ([StatusId])
    references [Cfg].[MsgStatus] ([Id])
)
GO

CREATE TABLE [Msg].[Event] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [Tm] datetime NOT NULL DEFAULT GETUTCDATE(),
  [UserId] uniqueidentifier NOT NULL,
  [MsgId] uniqueidentifier NOT NULL,
  [StatusId] integer NOT NULL,
  constraint [FK__MsgEvent__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id]),
  constraint [FK__MsgEvent__MsgMessage] foreign key ([MsgId])
    references [Msg].[Message] ([Id]),
  constraint [FK__MsgEvent__CfgMsgStatus] foreign key ([StatusId])
    references [Cfg].[MsgStatus] ([Id])
)
GO

CREATE TABLE [Msg].[EventDelivered] (
  [UserId] uniqueidentifier NOT NULL PRIMARY KEY,
  [EventId] bigint NOT NULL UNIQUE,
  constraint [FK__MsgEventDelivered__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id]),
  constraint [FK__MsgEventDelivered__MsgEvent] foreign key ([EventId])
    references [Msg].[Event] ([Id]),  
)
GO
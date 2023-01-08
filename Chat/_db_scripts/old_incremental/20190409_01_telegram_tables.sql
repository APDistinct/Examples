use [FLChat]
go

create schema [Prot]
go

create table [Prot].[TransportLog] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [InsertedDate] datetime NOT NULL default GETUTCDATE(),
  [TransportTypeId] int NOT NULL,
  [Outcome] bit NOT NULL,
  [Method] nvarchar(255) NULL,
  [Request] nvarchar(max) NOT NULL,
  [Response] nvarchar(max) NULL,
  constraint [FK__MsgMessageLog__CfgTransportType] foreign key ([TransportTypeId])
    references [Cfg].[TransportType] ([Id])
)
go

create table [Msg].[TelegramMsg] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [InsertedDate] datetime NOT NULL default GETUTCDATE(),
  [TelegramId] int NOT NULL UNIQUE,
  [MsgId] uniqueidentifier NOT NULL,
  [ToUserId] uniqueidentifier NULL,
  [ToTransportTypeId] int NULL,
  constraint [Chk__MsgTelegramMsg__ToTransportTypeId] 
    check ([ToTransportTypeId] is null or [ToTransportTypeId] = /**Telegram**/1),
  constraint [FK__MsgTelegramMsg__MsgMessage] 
    foreign key ([MsgId])
    references [Msg].[Message] ([Id]),
  constraint [FK__MsgTelegramMsg__MsgMessageToUser] 
    foreign key ([MsgId], [ToUserId], [ToTransportTypeId])
	references [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId]),
  constraint [UNQ__MsgTelegramMsg__Message] 
    unique ([MsgId], [ToUserId], [ToTransportTypeId])
)
go

create table [Msg].[MessageError] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [InsertedDate] datetime NOT NULL default GETUTCDATE(),
  [MsgId] uniqueidentifier NULL,
  [ToUserId] uniqueidentifier NULL,
  [ToTransportTypeId] int NULL,
  [Type] nvarchar(255),
  [Descr] nvarchar(4000),
  [Trace] nvarchar(max),
  constraint [FK__MsgMessageError__MsgMessage] 
    foreign key ([MsgId])
    references [Msg].[Message] ([Id]),
  constraint [FK__MsgMessageError__MsgMessageToUser] 
    foreign key ([MsgId], [ToUserId], [ToTransportTypeId])
	references [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId])
)
go
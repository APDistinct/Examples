use [FLChat]
go

drop table [Msg].[TelegramMsg]
go

create table [Msg].[MessageTransportId] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [InsertedDate] datetime NOT NULL default GETUTCDATE(),
  [TransportId] nvarchar(255) NOT NULL,
  [MsgId] uniqueidentifier NOT NULL,
  [ToUserId] uniqueidentifier NULL,
  [TransportTypeId] int NOT NULL,
  --constraint [Chk__MsgTelegramMsg__ToTransportTypeId] 
  --  check ([ToTransportTypeId] is null or [ToTransportTypeId] = /**Telegram**/1),
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
    unique ([MsgId], [ToUserId], [TransportTypeId]),
  constraint [UNQ__MsgMessageTransportId__Id]
    unique ([TransportId], [TransportTypeId])
)
go

alter table [Msg].[MessageTransportId]
add constraint [CHK__MsgMessageTransportId__TransportTypeId] 
  check ([TransportTypeId] <> 0)
go
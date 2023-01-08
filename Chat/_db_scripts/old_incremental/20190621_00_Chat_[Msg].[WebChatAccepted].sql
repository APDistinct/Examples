use [FLChat]
go

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
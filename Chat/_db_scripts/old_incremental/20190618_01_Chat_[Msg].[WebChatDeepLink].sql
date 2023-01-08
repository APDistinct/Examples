use [FLChat]
go

create table [Msg].[WebChatDeepLink] (
  [Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [MsgId] uniqueidentifier NOT NULL,
  [ToUserId] uniqueidentifier NOT NULL,
  [ToTransportTypeId] int NOT NULL DEFAULT 100,
  [Link] nvarchar(100) NOT NULL,
  [ExpireDate] datetime NOT NULL,
  constraint [FK__MsgWebChatDeepLink__MsgMessageToUser]
    foreign key ([MsgId], [ToUserId], [ToTransportTypeId])
	references [Msg].[MessageToUser]([MsgId], [ToUserId], [ToTransportTypeId])
	on delete cascade,
  constraint [UNQ__MsgWebChatDeepLink] unique ([Link]),
  constraint [CHK_WebChatDeepLink_ToTransportTypeId] check ([ToTransportTypeId] = 100)
)
go
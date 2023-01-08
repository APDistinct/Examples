use [FLChat]
go

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
use [FLChat]
go

alter table [Msg].[Message]
add [ForwardMsgId] uniqueidentifier NULL
go

alter table [Msg].[Message]
add constraint [FK__MsgMessage__ForwardMsgId]
  foreign key ([ForwardMsgId])
  references [Msg].[Message]([Id])  
go
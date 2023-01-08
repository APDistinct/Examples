use [FLChat]
go

alter table [Msg].[Event]
alter column [MsgId] uniqueidentifier NULL
go

alter table [Msg].[Event]
add [CausedByUserTransportTypeId] integer null
go

alter table [Msg].[Event]
add constraint [FK__MsgEvent__UsrTransport] 
  foreign key ([CausedByUserId], [CausedByUserTransportTypeId])
  references [Usr].[Transport] ([UserId], [TransportTypeId])
go
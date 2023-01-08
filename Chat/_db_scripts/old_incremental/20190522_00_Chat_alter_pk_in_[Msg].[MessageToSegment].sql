use [FLChat]
go

alter table [Msg].[MessageToSegment]
drop [PK__MsgMessageToSegment]
go

alter table [Msg].[MessageToSegment]
add constraint [PK__MsgMessageToSegment] primary key ([MsgId], [SegmentId])
go
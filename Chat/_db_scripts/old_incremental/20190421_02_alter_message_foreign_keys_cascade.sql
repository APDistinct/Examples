use [FLChat]
go

alter table [Msg].[EventAddressee]
drop constraint [FK__MsgEventAddressee__MsgEvent]
go

alter table [Msg].[EventAddressee]
add constraint [FK__MsgEventAddressee__MsgEvent] foreign key([Id])
references [Msg].[Event] ([Id])
on delete cascade
GO

alter table [Msg].[Event]
drop constraint [FK__MsgEvent__MsgMessage]
go

alter table [Msg].[Event]
add constraint [FK__MsgEvent__MsgMessage] foreign key([MsgId])
references [Msg].[Message] ([Id])
on delete cascade
GO

alter table [Msg].[MessageToUser]
drop constraint [FK__MsgMessageToUser__MsgMessage]
go

alter table [Msg].[MessageToUser]
add constraint [FK__MsgMessageToUser__MsgMessage] foreign key([MsgId])
references [Msg].[Message] ([Id])
on delete cascade
GO

alter table [Msg].[MessageToSegment]
drop constraint [FK__MsgMessageToSegment__MsgMessage]
go

alter table [Msg].[MessageToSegment]
add constraint  [FK__MsgMessageToSegment__MsgMessage] foreign key([MsgId])
references [Msg].[Message] ([Id])
on delete cascade
GO
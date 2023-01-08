use [FLChat]
go

alter table [Msg].[MessageTransportId]
add [Index] tinyint NOT NULL default 0
go

alter table [Msg].[MessageTransportId]
add [Count] tinyint NOT NULL default 1
go

/****** Object:  Index [UNQ__MsgMessageTransportId__Message]    Script Date: 02.11.2019 16:08:24 ******/
ALTER TABLE [Msg].[MessageTransportId] DROP CONSTRAINT [UNQ__MsgMessageTransportId__Message]
GO

/****** Object:  Index [UNQ__MsgMessageTransportId__Message]    Script Date: 02.11.2019 16:08:24 ******/
ALTER TABLE [Msg].[MessageTransportId] 
ADD  CONSTRAINT [UNQ__MsgMessageTransportId__Message] UNIQUE NONCLUSTERED 
(
	[MsgId] ASC,
	[ToUserId] ASC,
	[TransportTypeId] ASC,
	[Index] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
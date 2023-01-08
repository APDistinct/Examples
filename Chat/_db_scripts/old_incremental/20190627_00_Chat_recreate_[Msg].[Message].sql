use [FLChat]
go

select count(*) from [Msg].[Message]

drop table if exists #msgtmp

select *
into #msgtmp
from  [Msg].[Message]

--drop keys
ALTER TABLE [Msg].[Event] DROP CONSTRAINT [FK__MsgEvent__MsgMessage]
ALTER TABLE [Msg].[MessageError] DROP CONSTRAINT [FK__MsgMessageError__MsgMessage]
ALTER TABLE [Msg].[MessageToSegment] DROP CONSTRAINT [FK__MsgMessageToSegment__MsgMessage]
ALTER TABLE [Msg].[MessageToUser] DROP CONSTRAINT [FK__MsgMessageToUser__MsgMessage]
ALTER TABLE [Msg].[MessageTransportId] DROP CONSTRAINT [FK__MsgMessageTransportId__MsgMessage]

--recreate table
--ALTER TABLE [Msg].[Message] DROP CONSTRAINT [FK__MsgMessage__FromTransport]
--ALTER TABLE [Msg].[Message] DROP CONSTRAINT [FK__MsgMessage__ForwardMsgId]
--ALTER TABLE [Msg].[Message] DROP CONSTRAINT [FK__MsgMessage__CfgMessageType]
--ALTER TABLE [Msg].[Message] DROP CONSTRAINT [FK__MsgMessage__AnswerMessage]
--ALTER TABLE [Msg].[Message] DROP CONSTRAINT [DF__Message__IsDelet__3D5E1FD2]
--ALTER TABLE [Msg].[Message] DROP CONSTRAINT [DF__Message__PostTm__3C69FB99]
--ALTER TABLE [Msg].[Message] DROP CONSTRAINT [DF__Message__Id__3B75D760]

DROP TABLE [Msg].[Message]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Msg].[Message](
	[Id] uniqueidentifier NOT NULL DEFAULT newsequentialid(),
	[Idx] bigint NOT NULL IDENTITY(1,1),
	[PostTm] datetime NOT NULL DEFAULT getutcdate(),
	[MessageTypeId] int NOT NULL,
	[FromUserId] uniqueidentifier NOT NULL,
	[FromTransportTypeId] int NOT NULL,
	[AnswerToId] uniqueidentifier NULL,
	[Text] nvarchar(4000) NULL,
	[IsDeleted] bit NOT NULL DEFAULT 0,
	[Specific] nvarchar(500) NULL,
	[ForwardMsgId] uniqueidentifier NULL,
	[FileId] uniqueidentifier NULL,
    CONSTRAINT [PK__MsgMessage__Id] PRIMARY KEY NONCLUSTERED ([Id]),
	CONSTRAINT [UNQ__MsgMessage_Idx] UNIQUE CLUSTERED ([Idx]),
	CONSTRAINT [FK__MsgMessage__AnswerMessage] 
	  FOREIGN KEY([AnswerToId])
      REFERENCES [Msg].[Message] ([Id]),
    CONSTRAINT [FK__MsgMessage__CfgMessageType] 
	  FOREIGN KEY([MessageTypeId])
      REFERENCES [Cfg].[MessageType] ([Id]),
	CONSTRAINT [FK__MsgMessage__ForwardMsgId] 
	  FOREIGN KEY([ForwardMsgId])
      REFERENCES [Msg].[Message] ([Id]),
	CONSTRAINT [FK__MsgMessage__FromTransport] 
	  FOREIGN KEY([FromUserId], [FromTransportTypeId])
	  REFERENCES [Usr].[Transport] ([UserId], [TransportTypeId])
) ON [PRIMARY]

--insert rows
insert into [Msg].[Message] 
([Id], [PostTm], [MessageTypeId], [FromUserId], [FromTransportTypeId], 
[AnswerToId], [Text], [IsDeleted], [Specific], [ForwardMsgId], [FileId])
select 
[Id], [PostTm], [MessageTypeId], [FromUserId], [FromTransportTypeId], 
[AnswerToId], [Text], [IsDeleted], [Specific], [ForwardMsgId], [FileId] 
from #msgtmp
order by [PostTm] asc

--recreate keys
ALTER TABLE [Msg].[Event]  WITH CHECK ADD  CONSTRAINT [FK__MsgEvent__MsgMessage] FOREIGN KEY([MsgId])
REFERENCES [Msg].[Message] ([Id])
ON DELETE CASCADE

ALTER TABLE [Msg].[Event] CHECK CONSTRAINT [FK__MsgEvent__MsgMessage]

ALTER TABLE [Msg].[MessageError]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessageError__MsgMessage] FOREIGN KEY([MsgId])
REFERENCES [Msg].[Message] ([Id])
ON DELETE CASCADE

ALTER TABLE [Msg].[MessageError] CHECK CONSTRAINT [FK__MsgMessageError__MsgMessage]

ALTER TABLE [Msg].[MessageToSegment]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessageToSegment__MsgMessage] FOREIGN KEY([MsgId])
REFERENCES [Msg].[Message] ([Id])
ON DELETE CASCADE

ALTER TABLE [Msg].[MessageToSegment] CHECK CONSTRAINT [FK__MsgMessageToSegment__MsgMessage]

ALTER TABLE [Msg].[MessageToUser]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessageToUser__MsgMessage] FOREIGN KEY([MsgId])
REFERENCES [Msg].[Message] ([Id])
ON DELETE CASCADE

ALTER TABLE [Msg].[MessageToUser] CHECK CONSTRAINT [FK__MsgMessageToUser__MsgMessage]

ALTER TABLE [Msg].[MessageTransportId]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessageTransportId__MsgMessage] FOREIGN KEY([MsgId])
REFERENCES [Msg].[Message] ([Id])

ALTER TABLE [Msg].[MessageTransportId] CHECK CONSTRAINT [FK__MsgMessageTransportId__MsgMessage]

select count(*) from [Msg].[Message]

drop table if exists #msgtmp
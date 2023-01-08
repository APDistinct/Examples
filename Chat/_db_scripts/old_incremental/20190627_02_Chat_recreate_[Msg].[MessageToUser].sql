use [FLChat]
go

select count(*) from [Msg].[MessageToUser]

drop table if exists #mtutmp

select *
into #mtutmp
from  [Msg].[MessageToUser]

--drop keys
ALTER TABLE [Msg].[MessageTransportId] DROP CONSTRAINT [FK__MsgMessageTransportId__MsgMessageToUser]
ALTER TABLE [Msg].[WebChatDeepLink] DROP CONSTRAINT [FK__MsgWebChatDeepLink__MsgMessageToUser]
ALTER TABLE [Msg].[MessageError] DROP CONSTRAINT [FK__MsgMessageError__MsgMessageToUser]

drop table [Msg].[MessageToUser]

--create table
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Msg].[MessageToUser](   
	[MsgId] uniqueidentifier NOT NULL,
	[ToUserId] uniqueidentifier NOT NULL,
	[ToTransportTypeId] int NOT NULL,
	[Idx] bigint NOT NULL IDENTITY(1,1),
	[IsFailed] bit NOT NULL DEFAULT 0,
	[IsSent] bit NOT NULL DEFAULT 0,
	[IsDelivered] bit NOT NULL DEFAULT 0,
	[IsRead] bit NOT NULL DEFAULT 0,
	CONSTRAINT [PK__MsgMessageToUser] PRIMARY KEY NONCLUSTERED (
		[MsgId], [ToUserId], [ToTransportTypeId]),
	CONSTRAINT [UNQ_MsgMessageToUser_Idx] UNIQUE CLUSTERED ([Idx]),
	CONSTRAINT [FK__MsgMessageToUser__MsgMessage] 
		FOREIGN KEY([MsgId])
		REFERENCES [Msg].[Message] ([Id])
		ON DELETE CASCADE,
	CONSTRAINT [FK__MsgMessageToUser__UsrTransport] 
		FOREIGN KEY([ToUserId], [ToTransportTypeId])
		REFERENCES [Usr].[Transport] ([UserId], [TransportTypeId])
		ON DELETE CASCADE,
	CONSTRAINT [CHK_MessageToUser_FLChat] 
		CHECK (([ToTransportTypeId]=(0) AND [IsFailed]=(0) AND [IsSent]=(1) OR [ToTransportTypeId]<>(0)))
) ON [PRIMARY]
GO

--insert rows
insert into [Msg].[MessageToUser] 
([MsgId], [ToUserId], [ToTransportTypeId], [IsFailed], [IsSent], [IsDelivered], [IsRead])
select 
t.[MsgId], t.[ToUserId], t.[ToTransportTypeId], t.[IsFailed], t.[IsSent], t.[IsDelivered], t.[IsRead]
from #mtutmp t
inner join [Msg].[Message] m on t.[MsgId] = m.[Id]
order by m.[PostTm]

--recreate keys
ALTER TABLE [Msg].[MessageTransportId]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessageTransportId__MsgMessageToUser] FOREIGN KEY([MsgId], [ToUserId], [TransportTypeId])
REFERENCES [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId])

ALTER TABLE [Msg].[MessageTransportId] CHECK CONSTRAINT [FK__MsgMessageTransportId__MsgMessageToUser]

ALTER TABLE [Msg].[WebChatDeepLink]  WITH CHECK ADD  CONSTRAINT [FK__MsgWebChatDeepLink__MsgMessageToUser] FOREIGN KEY([MsgId], [ToUserId], [ToTransportTypeId])
REFERENCES [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId])
ON DELETE CASCADE

ALTER TABLE [Msg].[WebChatDeepLink] CHECK CONSTRAINT [FK__MsgWebChatDeepLink__MsgMessageToUser]

ALTER TABLE [Msg].[MessageError]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessageError__MsgMessageToUser] FOREIGN KEY([MsgId], [ToUserId], [ToTransportTypeId])
REFERENCES [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId])

ALTER TABLE [Msg].[MessageError] CHECK CONSTRAINT [FK__MsgMessageError__MsgMessageToUser]

select count(*) from [Msg].[MessageToUser]

drop table if exists #mtutmp
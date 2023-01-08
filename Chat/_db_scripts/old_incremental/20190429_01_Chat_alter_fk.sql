USE [FLChat]
GO

--event delivered
ALTER TABLE [Msg].[EventDelivered] DROP CONSTRAINT [FK__MsgEventDelivered__MsgEvent]
GO

ALTER TABLE [Msg].[EventDelivered]  WITH CHECK ADD  CONSTRAINT [FK__MsgEventDelivered__MsgEvent] FOREIGN KEY([LastEventId])
REFERENCES [Msg].[Event] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [Msg].[EventDelivered] CHECK CONSTRAINT [FK__MsgEventDelivered__MsgEvent]
GO

-- message to user
ALTER TABLE [Msg].[MessageToUser] DROP CONSTRAINT [FK__MsgMessageToUser__UsrTransport]
GO

ALTER TABLE [Msg].[MessageToUser]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessageToUser__UsrTransport] FOREIGN KEY([ToUserId], [ToTransportTypeId])
REFERENCES [Usr].[Transport] ([UserId], [TransportTypeId])
ON DELETE CASCADE
GO

ALTER TABLE [Msg].[MessageToUser] CHECK CONSTRAINT [FK__MsgMessageToUser__UsrTransport]
GO

--message error
ALTER TABLE [Msg].[MessageError] DROP CONSTRAINT [FK__MsgMessageError__MsgMessage]
GO

ALTER TABLE [Msg].[MessageError]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessageError__MsgMessage] FOREIGN KEY([MsgId])
REFERENCES [Msg].[Message] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [Msg].[MessageError] CHECK CONSTRAINT [FK__MsgMessageError__MsgMessage]
GO
USE [FLChat]
GO

ALTER TABLE [Msg].[WebChatDeepLink]
drop constraint [FK__MsgWebChatDeepLink__MsgMessageToUser]
go

ALTER TABLE [Msg].[WebChatDeepLink]  WITH CHECK ADD  CONSTRAINT [FK__MsgWebChatDeepLink__MsgMessageToUser] FOREIGN KEY([MsgId], [ToUserId], [ToTransportTypeId])
REFERENCES [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId])
ON DELETE CASCADE
ON UPDATE CASCADE
GO

ALTER TABLE [Msg].[WebChatDeepLink] CHECK CONSTRAINT [FK__MsgWebChatDeepLink__MsgMessageToUser]
GO



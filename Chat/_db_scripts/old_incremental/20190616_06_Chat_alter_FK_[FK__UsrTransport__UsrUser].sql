USE [FLChat]
GO

ALTER TABLE [Usr].[Transport] DROP CONSTRAINT [FK__UsrTransport__UsrUser]
GO

ALTER TABLE [Usr].[Transport]  WITH CHECK ADD  CONSTRAINT [FK__UsrTransport__UsrUser] 
  FOREIGN KEY([UserId])
  REFERENCES [Usr].[User] ([Id])
  on delete cascade
GO

ALTER TABLE [Usr].[Transport] CHECK CONSTRAINT [FK__UsrTransport__UsrUser]
GO



USE [FLChat]
GO

ALTER TABLE [Usr].[User]
ADD CONSTRAINT [FK__UsrUser__UsrUser_OwnerUserId] foreign key ([OwnerUserId])
  references [Usr].[User] ([Id])
GO
USE [FLChat]
GO

ALTER TABLE [Usr].[UserAvatar]
add [Width] int NOT NULL default 0
GO

ALTER TABLE [Usr].[UserAvatar]
add [Height] int NOT NULL default 0
GO


use [FLChat]
GO

alter table [Usr].[User]
add [EnabledInnerTransport] bit NOT NULL default 0;
GO
use [FLChat]
go

alter table [Usr].[User]
add [IsTemporary] bit NOT NULL default 0
go
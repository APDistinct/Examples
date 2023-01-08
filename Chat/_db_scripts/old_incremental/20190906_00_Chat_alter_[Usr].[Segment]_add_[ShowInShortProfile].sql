use [FLChat]
go

alter table [Usr].[Segment]
add [ShowInShortProfile] bit NOT NULL default 0
go

alter table [Usr].[Segment]
add [Tag] nvarchar(250) NULL
go
use [FLChat]
go

alter table [Usr].[User]
add [DontDisableOnUpdateStructure] bit NOT NULL default 0
go

update [Usr].[User] set [DontDisableOnUpdateStructure] = 1
where [Id] in (select [UserId] from [Usr].[SegmentMember] 
	where [SegmentId] in ((select [Id] from [Usr].[Segment] where [Name] = N'Testers')))
go
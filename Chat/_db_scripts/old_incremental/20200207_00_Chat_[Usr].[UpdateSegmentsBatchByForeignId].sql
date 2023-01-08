USE [FLChat]
GO

/****** Object:  StoredProcedure [Usr].[UpdateSegmentsBatch]    Script Date: 07.02.2020 19:39:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TYPE [Usr].[ForeignIdList] as table (
	[ForeignId] nvarchar(100) NOT NULL UNIQUE
)
GO

CREATE TYPE [Usr].[ForeignIdGuidList] as table (
	[ForeignId] nvarchar(100) NOT NULL,
	[Guid] uniqueidentifier NOT NULL
)
GO

/******* [Usr].[UpdateSegmentsBatch] *********/
CREATE OR ALTER  PROCEDURE [Usr].[UpdateSegmentsBatchByForeignId]
	@users [Usr].[ForeignIdList] readonly,
	@segments [Usr].[ForeignIdGuidList] readonly
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @deleted int;
	declare @inserted int;

	delete sm 
	from [Usr].[SegmentMember] sm
	inner join [Usr].[User] u on sm.[UserId] = u.[Id]
	inner join [Usr].[Segment] s on s.[Id] = sm.[SegmentId]
	left join @segments ins on ins.[Guid] = sm.[SegmentId] and ins.[ForeignId] = u.[ForeignId]
	where s.[PartnerName] is not null
	  and u.[ForeignId] in (select * from @users)
	  and ins.[Guid] is null;
	  
	set @deleted = @@ROWCOUNT;

	insert into [Usr].[SegmentMember] ([UserId], [SegmentId])
	select u.[Id], ins.[Guid]
	from @segments ins
	inner join [Usr].[User] u on ins.[ForeignId] = u.[ForeignId]
	left join [Usr].[SegmentMember] sm on sm.[UserId] = u.[Id] and sm.[SegmentId] = ins.[Guid]
	where sm.[UserId] is null;

	set @inserted = @@ROWCOUNT;

	select @inserted as [Inserted], @deleted as [Deleted];
END
GO



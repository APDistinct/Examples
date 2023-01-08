use [FLChat]
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create type [dbo].[IntGuidList] as table (
  [Int] integer NOT NULL,
  [Guid] uniqueidentifier NOT NULL,
  primary key ([Int], [Guid])
)
go

drop procedure if exists [Usr].[UpdateSegmentsBatch]
go

CREATE PROCEDURE [Usr].[UpdateSegmentsBatch]
	@users [dbo].[IntList] readonly,
	@segments [dbo].[IntGuidList] readonly
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
	left join @segments ins on ins.[Guid] = sm.[SegmentId] and ins.[Int] = u.[FLUserNumber]
	where s.[PartnerName] is not null
	  and u.[FLUserNumber] in (select * from @users)
	  and ins.[Guid] is null;
	  
	set @deleted = @@ROWCOUNT;

	insert into [Usr].[SegmentMember] ([UserId], [SegmentId])
	select u.[Id], ins.[Guid]
	from @segments ins
	inner join [Usr].[User] u on ins.[Int] = u.[FLUserNumber]
	left join [Usr].[SegmentMember] sm on sm.[UserId] = u.[Id] and sm.[SegmentId] = ins.[Guid]
	where sm.[UserId] is null;

	set @inserted = @@ROWCOUNT;

	select @inserted as [Inserted], @deleted as [Deleted];
END
GO

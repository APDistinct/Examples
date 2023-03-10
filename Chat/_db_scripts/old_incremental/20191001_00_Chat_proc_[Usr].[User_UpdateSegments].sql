USE [FLChat]
GO
/****** Object:  StoredProcedure [Usr].[User_UpdateSegments]    Script Date: 01.10.2019 21:55:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [Usr].[User_UpdateSegments] 
	-- Add the parameters for the stored procedure here
	@userId uniqueidentifier,
	@segments [dbo].[GuidList] readonly,
	@userFLNumber int = NULL,
	@removeFromPartnerOnly bit = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if @userId is NULL and @userFLNumber is not null 
	begin
	  set @userId = (select [Id] from [Usr].[User] where [FLUserNumber] = @userFLNumber);
	end

	if @userId is NULL
	  THROW 50001, 'User must be present', 1;
    
	declare @deleted int;
	declare @inserted int;

	delete from [Usr].[SegmentMember] 
	where [UserId] = @userId
	  and [SegmentId] not in (select * from @segments)
	  and (@removeFromPartnerOnly = 0 
	   or [SegmentId] in (select [Id] from [Usr].[Segment] where [PartnerName] is not null));

	set @deleted = @@ROWCOUNT;

	insert into [Usr].[SegmentMember] ([SegmentId], [UserId])
	select [Guid], @userId 
	from @segments
	where [Guid] not in (
	  select [SegmentId] from [Usr].[SegmentMember] where [UserId] = @userId);

	set @inserted = @@ROWCOUNT;

	select @deleted as [Deleted], @inserted as [Inserted];
END

use [FLChat]
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

drop procedure if exists [Usr].[User_UpdateDeepChilds]
go

CREATE PROCEDURE [Usr].[User_UpdateDeepChilds]	
	@userId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    declare @childs [dbo].[GuidList];
	
	insert into @childs 
	select [UserId] from [Usr].[User_GetChilds](@userId, default, default);

	insert into [Usr].[UserDeepChilds] ([UserId], [ChildUserId])
	select @userId, [Guid]
	from @childs c 
	where c.[Guid] not in (
	  select [ChildUserId] from [Usr].[UserDeepChilds] where [UserId] = @userId);

	delete from [Usr].[UserDeepChilds]
	where [UserId] = @userId 
	  and [ChildUserId] not in (select [Guid] from @childs)
END
GO

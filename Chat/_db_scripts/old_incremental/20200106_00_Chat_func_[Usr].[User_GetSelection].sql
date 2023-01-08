USE [FLChat]
GO

/****** Object:  UserDefinedFunction [Usr].[User_GetSelection]    Script Date: 06.01.2020 6:24:49 ******/
DROP FUNCTION [Usr].[User_GetSelection]
GO

/****** Object:  UserDefinedFunction [Usr].[User_GetSelection]    Script Date: 06.01.2020 6:24:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE FUNCTION [Usr].[User_GetSelection] 
(	
	-- Add the parameters for the function here
	@userId uniqueidentifier,
	@includeUsersWithStructure [Usr].[UserIdDeep] readonly,
	@excludeUsersWithStructure [dbo].[GuidList] readonly,
	@includeUsers [dbo].[GuidList] readonly,
	@excludeUsers [dbo].[GuidList] readonly,
	@segments [dbo].[GuidList] readonly,
    @includeDeleted bit = 0
)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE
) 
as
begin

  --excluded users
  declare @exclude [dbo].[GuidList];
  --included users
  declare @include [dbo].[GuidList];
  declare @guid uniqueidentifier;
  declare @deep int;

  insert into @exclude 
  select * from @excludeUsersWithStructure;

  insert into @exclude
  select * from @excludeUsers;

  --get structure for exluded users
  DECLARE ic CURSOR FORWARD_ONLY FAST_FORWARD
  FOR  
    select [Guid] from @excludeUsersWithStructure;        
  open ic;

  fetch next from ic into @guid;
  while @@FETCH_STATUS = 0
  begin
    insert into @exclude
	select [UserId] 
	from [Usr].[User_GetChildsSimple](@guid, default, @includeDeleted);

    fetch next from ic into @guid;
  end;
  	
  close ic;
  deallocate ic;  

  --get structure for included users
  DECLARE ic CURSOR FORWARD_ONLY FAST_FORWARD
  FOR  
    select [UserId], [Deep] 
	from @includeUsersWithStructure
	where [UserId] in (
	  select [UserId] from [Usr].[User_GetChildsSimple](@userId, default, default) 
	  union
	  select @userId)
  open ic;

  fetch next from ic into @guid, @deep;
  while @@FETCH_STATUS = 0
  begin
    insert into @include
	select [UserId] 
	from [Usr].[User_GetChildsSimple](@guid, @deep, @includeDeleted);

	if @guid <> @userId
		insert into @include values (@guid);

    fetch next from ic into @guid, @deep;
  end;
  	
  close ic;
  deallocate ic;

  --add segments
  insert into @include
  select [UserId]
  from [Usr].[Segments_GetMembers](@userId, @segments, null, @includeDeleted);

  --prepare broadcast prohibition user's structure
  --declare @broadcastProhibition [dbo].[GuidList];
  --insert into @broadcastProhibition
  --select [ProhibitionUserId] 
  --from [Usr].[BroadcastProhibition]
  --where [UserId] = @userId;

  delete from @include where [Guid] in (select [Guid] from @exclude);
  insert into @include select [Guid] from @includeUsers where [Guid] <> @userId;

  --get selection
  with 
  dt as (		--all users in selection
    select DISTINCT [Guid]
	from @include
	--where [Guid] not in (select [Guid] from @exclude)
	--union
	--select DISTINCT [Guid] from @includeUsers where [Guid] <> @userId
  ),
  bp as ( --broadcast prohibition
    --select [UserId]
	--from [Usr].[User_GetChildsMulti](@broadcastProhibition, default, @includeDeleted)
	select [UserId]
	from [Usr].[BroadcastProhibition_StructureUpward](@userId, @include)
  )
  insert into @ids ([UserId])
  (
    select [Guid] 
	from dt
	where dt.[Guid] not in (select [UserId] from bp)
  );

  RETURN
end
GO



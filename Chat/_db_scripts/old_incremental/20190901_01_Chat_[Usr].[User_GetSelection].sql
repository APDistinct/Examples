use [FLChat]
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

drop function if exists [Usr].[User_GetSelection]
go

CREATE FUNCTION [Usr].[User_GetSelection] 
(	
	-- Add the parameters for the function here
	@userId uniqueidentifier,
	@includeUsersWithStructure [Usr].[UserIdDeep] readonly,
	@excludeUsersWithStructure [dbo].[GuidList] readonly,
	@includeUsers [dbo].[GuidList] readonly,
	@excludeUsers [dbo].[GuidList] readonly,    
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
	from [Usr].[User_GetChilds](@guid, default, @includeDeleted);

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
	  select [UserId] from [Usr].[User_GetChilds](@userId, default, default) 
	  union
	  select @userId)
  open ic;

  fetch next from ic into @guid, @deep;
  while @@FETCH_STATUS = 0
  begin
    insert into @include
	select [UserId] 
	from [Usr].[User_GetChilds](@guid, @deep, @includeDeleted);

	if @guid <> @userId
		insert into @include values (@guid);

    fetch next from ic into @guid, @deep;
  end;
  	
  close ic;
  deallocate ic;

  --get selection
  insert into @ids ([UserId])
  (
  select DISTINCT [Guid]
  from @include
  where [Guid] not in (select [Guid] from @exclude)
  union
  select DISTINCT [Guid] from @includeUsers where [Guid] <> @userId
  );

  RETURN
end
GO

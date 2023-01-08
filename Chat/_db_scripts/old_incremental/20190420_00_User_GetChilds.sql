use [FLChat]
go

CREATE TYPE [Usr].[UserIdDeep] AS TABLE(
	[UserId] uniqueidentifier NOT NULL UNIQUE,
	[Deep] integer NOT NULL
)
GO

IF OBJECT_ID (N'Usr.User_GetChilds', N'TF') IS NOT NULL  
    DROP FUNCTION [Usr].[User_GetChilds];  
GO  

create function [Usr].[User_GetChilds](
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE,
  [Deep] integer NOT NULL
)
as
begin
  --declare @ids [Usr].[UserIdDeep];
  declare @deep integer;
  set @deep = 1;

  insert into @ids ([UserId], [Deep])
  select [Id], @deep 
  from [Usr].[User]   
  where ([Enabled] = 1 or @includeDeleted = 1) and [OwnerUserId] = @userId;

  while @deep < @maxDeep or @maxDeep is null
  begin
    insert into @ids ([UserId], [Deep])
	select [Id], @deep + 1 from [Usr].[User] 
	where ([Enabled] = 1 or @includeDeleted = 1)
	  and [OwnerUserId] in (select [UserId] from @ids where [Deep] = @deep)
	  and [Id] not in (select [UserId] from @ids);

	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  --return select * from @ids;

  RETURN
end;
go
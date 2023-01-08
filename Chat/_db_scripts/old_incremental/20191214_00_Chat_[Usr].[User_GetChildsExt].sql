USE [FLChat]
GO

/****** Object:  UserDefinedFunction [Usr].[User_GetChilds]    Script Date: 13.12.2019 23:34:40 ******/
DROP FUNCTION IF EXISTS [Usr].[User_GetChildsExt]
GO

/****** Object:  UserDefinedFunction [Usr].[User_GetChilds]    Script Date: 13.12.2019 23:34:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE function [Usr].[User_GetChildsExt](
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE,
  [Deep] integer NOT NULL,
  [OwnerUserId] uniqueidentifier NOT NULL
)
as
begin
  declare @cur [dbo].[GuidList];
  declare @prev [dbo].[GuidList];
  declare @deep integer;
  set @deep = 1;

  insert into @ids ([UserId], [Deep], [OwnerUserId])
  output inserted.[UserId] into @prev
  select [Id], @deep, [OwnerUserId] 
  from [Usr].[User]   
  where ([Enabled] = 1 or @includeDeleted = 1) and [OwnerUserId] = @userId;

  while @deep < @maxDeep or @maxDeep is null
  begin
    delete from @cur;

    insert into @ids ([UserId], [Deep], [OwnerUserId])
	output inserted.[UserId] into @cur
	select [Id], @deep + 1, [OwnerUserId] 
	from [Usr].[User] 
	where ([Enabled] = 1 or @includeDeleted = 1)
	  and [OwnerUserId] in (select [Guid] from @prev)
	  --(select [UserId] from @ids where [Deep] = @deep)	  
	  ;

	delete from @prev;
	insert into @prev ([Guid]) select [Guid] from @cur;

	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  RETURN
end;
GO



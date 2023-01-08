USE [FLChat]
GO

/****** Object:  UserDefinedFunction [Usr].[User_GetChilds]    Script Date: 29.07.2019 15:04:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

drop function if exists [Usr].[User_GetChilds]
go

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
  --declare @tmp [dbo].[GuidList];
  declare @deep integer;
  set @deep = 1;

  insert into @ids ([UserId], [Deep])
  --output inserted.[UserId] into @tmp
  select [Id], @deep 
  from [Usr].[User]   
  where ([Enabled] = 1 or @includeDeleted = 1) and [OwnerUserId] = @userId;

  while @deep < @maxDeep or @maxDeep is null
  begin
    insert into @ids ([UserId], [Deep])
	select [Id], @deep + 1 from [Usr].[User] 
	where ([Enabled] = 1 or @includeDeleted = 1)
	  and [OwnerUserId] in (select [UserId] from @ids where [Deep] = @deep)
	  --and [Id] not in (select [UserId] from @ids)
	  ;

	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  RETURN
end;
GO



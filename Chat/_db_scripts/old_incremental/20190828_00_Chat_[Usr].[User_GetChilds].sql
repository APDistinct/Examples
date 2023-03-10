USE [FLChat]
GO
/****** Object:  UserDefinedFunction [Usr].[User_GetChilds]    Script Date: 28.08.2019 18:32:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER function [Usr].[User_GetChilds](
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
  --declare @tmp [dbo].[GuidList];
  declare @deep integer;
  set @deep = 1;

  insert into @ids ([UserId], [Deep], [OwnerUserId])
  --output inserted.[UserId] into @tmp
  select [Id], @deep, [OwnerUserId] 
  from [Usr].[User]   
  where ([Enabled] = 1 or @includeDeleted = 1) and [OwnerUserId] = @userId;

  while @deep < @maxDeep or @maxDeep is null
  begin
    insert into @ids ([UserId], [Deep], [OwnerUserId])
	select [Id], @deep + 1, [OwnerUserId] 
	from [Usr].[User] 
	where ([Enabled] = 1 or @includeDeleted = 1)
	  and [OwnerUserId] in (select [UserId] from @ids where [Deep] = @deep)
	  --and [Id] not in (select [UserId] from @ids)
	  ;

	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  RETURN
end;

USE [FLChat]
GO
/****** Object:  UserDefinedFunction [Usr].[User_GetChildsMulti]    Script Date: 18.09.2019 14:04:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

drop function if exists [Usr].[User_GetChildsMulti]
go

create function [Usr].[User_GetChildsMulti](
   @usersId [dbo].[GuidList] readonly,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL,
  [Deep] integer NOT NULL,
  [OwnerUserId] uniqueidentifier NOT NULL  
)
as
begin
  declare @deep integer;
  set @deep = 1;

  insert into @ids ([UserId], [Deep], [OwnerUserId])
  select DISTINCT u.[Id], @deep, o.[Guid]
  from [Usr].[User] u 
  inner join @usersId o on u.[OwnerUserId] = o.[Guid]
  where (u.[Enabled] = 1 or @includeDeleted = 1);

  while @deep < @maxDeep or @maxDeep is null
  begin
    insert into @ids ([UserId], [Deep], [OwnerUserId])
	select DISTINCT [Id], @deep + 1, [OwnerUserId]
	from [Usr].[User]
	where ([Enabled] = 1 or @includeDeleted = 1)
	  and [OwnerUserId] in (select [UserId] from @ids where [Deep] = @deep);

	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  RETURN
end;

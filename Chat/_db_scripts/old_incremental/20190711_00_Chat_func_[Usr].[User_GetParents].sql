USE [FLChat]
GO
/****** Object:  UserDefinedFunction [Usr].[User_GetChilds]    Script Date: 11.07.2019 10:31:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID (N'Usr.User_GetParents', N'TF') IS NOT NULL  
    DROP FUNCTION [Usr].[User_GetParents];  
GO  

create function [Usr].[User_GetParents](
   @userId uniqueidentifier,
   @maxDeep integer = null)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE,
  [Deep] integer NOT NULL
)
as
begin
  --declare @ids [Usr].[UserIdDeep];
  declare @deep integer;
  set @deep = 1;
  declare @parent uniqueidentifier;

  set @parent = (select [OwnerUserId] from [Usr].[User] where [Id] = @userId);  

  while (@deep <= @maxDeep or @maxDeep is null) 
    and @parent is not null
	and NOT EXISTS(select 1 from @ids where [UserId] = @parent)
  begin
    insert into @ids ([UserId], [Deep]) values (@parent, -1 * @deep);
	
	set @deep = @deep + 1;
	set @parent = (select [OwnerUserId] from [Usr].[User] where [Id] = @parent);  
  end

  --return select * from @ids;

  RETURN
end;
go

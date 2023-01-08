USE [FLChat]
GO

/****** Object:  UserDefinedFunction [Usr].[User_GetChilds]    Script Date: 17.12.2019 20:29:53 ******/
DROP FUNCTION IF EXISTS [Usr].[User_GetChildsSimple]
GO

/****** Object:  UserDefinedFunction [Usr].[User_GetChilds]    Script Date: 17.12.2019 20:29:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE function [Usr].[User_GetChildsSimple](
   @userId uniqueidentifier,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
RETURNS @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE
)
as
begin
  if (select [IsUseDeepChilds] from [Usr].[User] where [Id] = @userId) = 1 
    and @maxDeep is null
	and @includeDeleted = 0
  begin
    insert into @ids 
	select [ChildUserId] from [Usr].[UserDeepChilds] where [UserId] = @userId;
  end
  else 
  begin
    insert into @ids
	select [UserId] from [Usr].[User_GetChilds](@userId, @maxDeep, @includeDeleted);
  end

  RETURN
end;
GO



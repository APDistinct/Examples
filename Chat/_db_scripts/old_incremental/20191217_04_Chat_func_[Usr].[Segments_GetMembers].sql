USE [FLChat]
GO

/****** Object:  UserDefinedFunction [Usr].[Segments_GetMembers]    Script Date: 17.12.2019 21:32:35 ******/
DROP FUNCTION IF EXISTS [Usr].[Segments_GetMembers]
GO

/****** Object:  UserDefinedFunction [Usr].[Segments_GetMembers]    Script Date: 17.12.2019 21:32:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



create function [Usr].[Segments_GetMembers](
   @userId uniqueidentifier,
   @segments [dbo].[GuidList] readonly,
   @maxDeep integer = null,
   @includeDeleted bit = 0)
--RETURNS @ids TABLE (
--  [UserId] uniqueidentifier NOT NULL UNIQUE,
--  [Deep] integer NOT NULL
--)
RETURNS TABLE
as
return (  
  select 
    ids.[UserId]   
  from [Usr].[User_GetChildsSimple] (@userId, @maxDeep, @includeDeleted) ids
  inner join [Usr].[SegmentMember] sm on ids.[UserId] = sm.[UserId]
  inner join @segments s on sm.[SegmentId] = s.[Guid]
);
GO



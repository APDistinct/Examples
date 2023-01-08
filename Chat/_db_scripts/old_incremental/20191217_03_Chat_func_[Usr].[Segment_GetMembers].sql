USE [FLChat]
GO

/****** Object:  UserDefinedFunction [Usr].[Segment_GetMembers]    Script Date: 17.12.2019 21:22:12 ******/
DROP FUNCTION IF EXISTS [Usr].[Segment_GetMembers]
GO

/****** Object:  UserDefinedFunction [Usr].[Segment_GetMembers]    Script Date: 17.12.2019 21:22:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create function [Usr].[Segment_GetMembers](
   @segmentId uniqueidentifier,
   @userId uniqueidentifier,
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
  where sm.[SegmentId] = @segmentId
);
GO



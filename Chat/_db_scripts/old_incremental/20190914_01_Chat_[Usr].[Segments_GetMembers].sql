USE [FLChat]
GO

drop function if exists [Usr].[Segments_GetMembers]
go

/****** Object:  UserDefinedFunction [Usr].[Segment_GetMembers]    Script Date: 14.09.2019 19:11:44 ******/
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
    ids.[UserId], ids.[Deep]   
  from [Usr].[User_GetChilds] (@userId, @maxDeep, @includeDeleted) ids
  inner join [Usr].[SegmentMember] sm on ids.[UserId] = sm.[UserId]
  inner join @segments s on sm.[SegmentId] = s.[Guid]
);
GO



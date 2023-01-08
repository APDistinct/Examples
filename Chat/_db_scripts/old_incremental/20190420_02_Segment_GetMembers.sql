use [FLChat]
go

IF OBJECT_ID (N'Usr.Segment_GetMembers', N'TF') IS NOT NULL  
    DROP FUNCTION [Usr].[Segment_GetMembers];  
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
    ids.[UserId], ids.[Deep]   
  from [Usr].[User_GetChilds] (@userId, @maxDeep, @includeDeleted) ids
  inner join [Usr].[SegmentMember] sm on ids.[UserId] = sm.[UserId]
  where sm.[SegmentId] = @segmentId
);
go
use [FLChat]
go

IF OBJECT_ID (N'Usr.Segment_UpdateMembers', N'P') IS NOT NULL  
    DROP PROCEDURE [Usr].[Segment_UpdateMembers];  
GO  

create procedure [Usr].[Segment_UpdateMembers] 
	@segmentId uniqueidentifier,
	@newMembersIds [dbo].[GuidList] readonly
as
  delete from [Usr].[SegmentMember]
  where [SegmentId] = @segmentId
    and [UserId] not in (select [Guid] from @newMembersIds);

  insert into [Usr].[SegmentMember] ([SegmentId], [UserId])
  select @segmentId, n.[Guid]
  from @newMembersIds n
  where n.[Guid] not in (select [UserId] from [Usr].[SegmentMember] where [SegmentId] = @segmentId);
go
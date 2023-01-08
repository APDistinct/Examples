USE [FLChat]
GO
/****** Object:  Trigger [Msg].[MessageToSegment_ProduceMessageToUsers]    Script Date: 22.05.2019 10:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER trigger [Msg].[MessageToSegment_ProduceMessageToUsers] 
on [Msg].[MessageToSegment]
after insert
as
  --trigger using cursor, because our function [Usr].[Segment_GetMembers] working
  -- for scalar input's values and therefore can'be used in select statement for many rows.
  -- Write new version of [Usr].[Segment_GetMembers] is not reasonable
  declare @msgId uniqueidentifier;
  declare @fromId uniqueidentifier;
  
  DECLARE ic CURSOR FORWARD_ONLY FAST_FORWARD
  FOR
  
    select i.[MsgId], m.[FromUserId]
	from inserted i	
	inner join [Msg].[Message] m on i.[MsgId] = m.[Id]
    group by i.[MsgId], m.[FromUserId];
        
  open ic;

  fetch next from ic into @msgId, @fromId;
  while @@FETCH_STATUS = 0
  begin

    with segments as (
      select DISTINCT sm.[UserId]
      from [Usr].[SegmentMember] sm  
      where sm.segmentId in (select [SegmentId] from inserted)
    ),
    structure as (
	  select [UserId] from [Usr].[User_GetChilds] (@fromId, default, default)
    )
    insert into [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId], [IsSent])    
    select 
      @msgId,
	  sm.[UserId],
	  dt.[DefaultTransportTypeId],
	  case when dt.[DefaultTransportTypeId] = /**FLChat**/0 then 1
	       else 0
      end
    from segments sm
    inner join [Usr].[UserDefaultTransportView] dt on sm.[UserId] = dt.[UserId]
	inner join structure st on sm.[UserId] = st.[UserId]
	left join [Msg].[MessageToUser] mtu on 
		mtu.[MsgId] = @msgId and 
		mtu.[ToUserId] = sm.[UserId] and
		mtu.[ToTransportTypeId] = dt.[DefaultTransportTypeId]
	where sm.[UserId] <> @fromId
	  and mtu.[ToUserId] is null	
    
	fetch next from ic into @msgId, @fromId;
  end;
  	
  close ic;
  deallocate ic;

use [FLChat]
go

create table [Msg].[MessageToSegment] (
  [MsgId] uniqueidentifier NOT NULL,
  [SegmentId] uniqueidentifier NOT NULL,
  [MaxDeep] integer NULL,
  constraint [PK__MsgMessageToSegment] primary key ([MsgId]),
  constraint [FK__MsgMessageToSegment__MsgMessage] foreign key ([MsgId])
    references [Msg].[Message] ([Id]),
  constraint [FK__MsgMessageToSegment__UsrSegment] foreign key ([SegmentId])
    references [Usr].[Segment] ([Id])
)
go

insert into [Cfg].[MessageType] ([Id], [Name])
values (3, 'Segment')
go

drop trigger if exists [Msg].[MessageToSegment_ProduceMessageToUsers];
go

create trigger [Msg].[MessageToSegment_ProduceMessageToUsers] 
on [Msg].[MessageToSegment]
after insert
as
  --trigger using cursor, because our function [Usr].[Segment_GetMembers] working
  -- for scalar input's values and therefore can'be used in select statement for many rows.
  -- Write new version of [Usr].[Segment_GetMembers] is not reasonable
  declare @msgId uniqueidentifier;
  declare @segmentId uniqueidentifier;
  declare @deep integer;
  declare @fromId uniqueidentifier;

  DECLARE ic CURSOR FORWARD_ONLY FAST_FORWARD
  FOR
    select i.[MsgId], i.[SegmentId], i.[MaxDeep], m.[FromUserId]
	from inserted i
	inner join [Msg].[Message] m on i.[MsgId] = m.[Id];
  open ic;
  fetch next from ic into @msgId, @segmentId, @deep, @fromId;
  while @@FETCH_STATUS = 0
  begin

    insert into [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId], [IsSent])
    select 
      @msgId,
	  sm.[UserId],
	  dt.[DefaultTransportTypeId],
	  case when dt.[DefaultTransportTypeId] = /**FLChat**/0 then 1
	       else 0
      end
    from [Usr].[Segment_GetMembers](@segmentId, @fromId, @deep, default) sm 	
    inner join [Usr].[UserDefaultTransportView] dt on sm.[UserId] = dt.[UserId]
	where sm.[UserId] <> @fromId;

	fetch next from ic into @msgId, @segmentId, @deep, @fromId;

  end;

  close ic;
  deallocate ic;
go
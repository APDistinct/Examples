use [FLChat]
go

create trigger [Msg].[MessageToUser_OnUpdate_ProduceEvents]
on [Msg].[MessageToUser]
after update
as
  declare @link [dbo].[GuidBigintTable];
  
  insert into [Msg].[Event] ([EventTypeId], [CausedByUserId], [CausedByUserTransportTypeId], [MsgId])
  output 
    inserted.[MsgId],
	inserted.[Id]
    into @link 
  select distinct [EventType], [ToUserId], [ToTransportTypeId], [MsgId] 
  from (
    select 
      case when i.[IsFailed] <> d.[IsFailed]                                then /**failed**/5
	       when i.[IsRead] <> d.[IsRead] and d.[IsFailed] = 0               then /**read**/3
		   when i.[IsDelivered] <> d.[IsDelivered] 
		    and d.[IsFailed] = 0 and d.[IsRead] = 0                         then /**delivered**/2
		   when i.[IsSent] <> d.[IsSent] 
		    and d.[IsFailed] = 0 and d.[IsRead] = 0 and d.[IsDelivered] = 0 then /**Sent**/1
           else /**Skip**/null
	  end as [EventType],
      i.[ToUserId], 
	  i.[ToTransportTypeId],
	  i.[MsgId]
    from inserted i
    inner join deleted d on i.[MsgId] = d.[MsgId]
                        and i.[ToUserId] = d.[ToUserId]
                        and i.[ToTransportTypeId] = d.[ToTransportTypeId]
    inner join [Msg].[Message] msg on msg.[Id] = i.[MsgId]
    where msg.IsDeleted = 0
	  and msg.[FromTransportTypeId] = /**FLChat**/0
	 ) t
  where [EventType] is not null
  ;

  insert into [Msg].[EventAddressee] ([Id], [UserId])   
  select l.[BInt], msg.[FromUserId]
  from @link l
  inner join [Msg].[Message] msg on l.[Guid] = msg.[Id];
go

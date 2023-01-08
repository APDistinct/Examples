USE [FLChat]
GO

/****** Object:  Trigger [MessageToUser_OnUpdate_ProduceEvents]    Script Date: 27.06.2019 20:03:55 ******/
DROP TRIGGER IF EXISTS [Msg].[MessageToUser_OnUpdate_ProduceEvents]
GO

/****** Object:  Trigger [Msg].[MessageToUser_OnUpdate_ProduceEvents]    Script Date: 27.06.2019 20:03:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



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
    inner join [Cfg].[TransportType] tt on msg.[FromTransportTypeId] = tt.[Id]

    where msg.IsDeleted = 0
	  --and msg.[FromTransportTypeId] = /**FLChat**/0
	  and tt.[InnerTransport] = 1 -- only inner transports (like FLChat and WebChat)
	 ) t
  where [EventType] is not null
  ;

  insert into [Msg].[EventAddressee] ([Id], [UserId])   
  select l.[BInt], msg.[FromUserId]
  from @link l
  inner join [Msg].[Message] msg on l.[Guid] = msg.[Id];
GO

ALTER TABLE [Msg].[MessageToUser] ENABLE TRIGGER [MessageToUser_OnUpdate_ProduceEvents]
GO



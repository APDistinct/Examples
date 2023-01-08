use [FLChat]
GO

create type [dbo].[GuidBigintTable] as table (
  [Guid] uniqueidentifier NOT NULL,
  [BInt] bigint NOT NULL
)
go

create trigger [Msg].[MessageToUser_OnInsert_ProduceEvents]
on [Msg].[MessageToUser]
after insert 
as
  -- trigger produce notification event 'sent' for every FLChat message's addressee

  declare @link [dbo].[GuidBigintTable];

  -- insert event header and collect inserted event's id into @link
  insert into [Msg].[Event] ([EventTypeId], [CausedByUserId], [MsgId])
  output inserted.[MsgId], inserted.[Id] into @link
  select distinct     
	/**Message sent event**/1,
	msg.[FromUserId],
	msg.[Id]
  from inserted i
  inner join [Msg].[Message] msg on i.[MsgId] = msg.[Id]
  where i.[ToTransportTypeId] = /**FLChat**/0 -- only for FLChat messages
	and msg.[IsDeleted] = 0 -- skip deleted message. Is there exist someone 
	                        -- who can send already deleted messages?
  ;

  -- insert event addressee
  insert into [Msg].[EventAddressee] ([Id], [UserId])
  select 
    l.[BInt], i.[ToUserId]
  from inserted i
  inner join @link l on l.[Guid] = i.[MsgId]
  inner join [Usr].[Transport] t on t.[UserId] = i.[ToUserId] 
                                and t.TransportTypeId = i.[ToTransportTypeId] 
								and t.[Enabled] = 1
  where i.[ToTransportTypeId] = /**FLChat**/0
  ;
GO

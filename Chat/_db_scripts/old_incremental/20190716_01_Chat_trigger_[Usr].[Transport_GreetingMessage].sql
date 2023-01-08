use [FLChat]
go

drop trigger if exists [Usr].[Transport_GreetingMessage]
go

create trigger [Usr].[Transport_GreetingMessage]
  on [Usr].[Transport]
  after INSERT, UPDATE
as
begin
  SET NOCOUNT ON;

  declare @tmp table ([UserId] uniqueidentifier, [TTId] int);

  insert into @tmp
  select i.[UserId], i.[TransportTypeId]
  from inserted i
  inner join [Cfg].[TransportType] tt on i.[TransportTypeId] = tt.[Id]
  inner join [Usr].[User] u on u.[Id] = i.[UserId]
  left join deleted d on i.[UserId] = d.[UserId] 
                     and i.[TransportTypeId] = d.[TransportTypeId]
  where i.[Enabled] = 1 
    and coalesce(d.[Enabled], 0) = 0
	and tt.[SendGreetingMessage] = 1
	and u.[IsTemporary] = 0;

  if @@ROWCOUNT <> 0 
  begin
    declare @text nvarchar(max);
	set @text = (select [Value] from [Cfg].[Settings] where [Name] = N'TEXT_GREETING_MSG');

	if @text is not null 
	begin
      declare @msgid uniqueidentifier;
	  set @msgid = NEWID();

	  insert into [Msg].[Message] ([Id], [MessageTypeId], [FromUserId], [FromTransportTypeId], [Text])
	  values (@msgid, /**Broadcast**/2, '00000000-0000-0000-0000-000000000000', /**FLChat**/0, @text);

	  insert into [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId])
	  select @msgid, t.[UserId], t.[TTId]
	  from @tmp t
	end
  end
end
go
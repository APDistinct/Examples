use [FLChat]
go 

drop procedure if exists [Msg].[Message_ProduceToUsers]
go

create procedure [Msg].[Message_ProduceToUsers] 
	@msgId uniqueidentifier
as 
begin
  declare @msgTypeId int;
  declare @from uniqueidentifier;
  
  --find message and get message type and sender
  select 
    @msgTypeId = [MessageTypeId], 
	@from = [FromUserId] 
  from [Msg].[Message] 
  where [Id] = @msgId;

  --if message has not found then error
  if @@ROWCOUNT = 0 THROW 50001, 'Message has not found', 1;

  -- table for addressees
  declare @toUsers table (
    [UserId] uniqueidentifier NOT NULL,
	[TransportTypeId] int NOT NULL
  );

  --arguments for selection function
  declare @include_ws [Usr].[UserIdDeep];
  declare @exclude_ws [dbo].[GuidList];
  declare @include [dbo].[GuidList];
  declare @exclude [dbo].[GuidList];
  declare @segments [dbo].[GuidList];

  /**Include with structure**/
  insert into @include_ws ([UserId], [Deep])
  select [UserId], [StructureDeep]
  from [Msg].[MessageToSelection] 
  where [MsgId] = @msgId and [WithStructure] = 1 and [Include] = 1;

  /**Exclude with structure**/
  insert into @exclude_ws 
  select [UserId] 
  from [Msg].[MessageToSelection] 
  where [MsgId] = @msgId and [WithStructure] = 1 and [Include] = 0;

  /**Include**/
  insert into @include 
  select [UserId] 
  from [Msg].[MessageToSelection] 
  where [MsgId] = @msgId and [WithStructure] = 0 and [Include] = 1;

  /**Exclude**/
  insert into @exclude 
  select [UserId] 
  from [Msg].[MessageToSelection] 
  where [MsgId] = @msgId and [WithStructure] = 0 and [Include] = 0;

  --segments
  insert into @segments
  select [SegmentId] from [Msg].[MessageToSegment] where [MsgId] = @msgId;

  --get all users from selection with transport
  insert into @toUsers
  select t.[UserId], t.[DefaultTransportTypeId]          
  from [Usr].[User_GetSelection](@from, @include_ws, @exclude_ws, @include, @exclude, @segments, default) s
  inner join (
	select * from [Usr].[UserMailingTransportView] where @msgTypeId = /**Mailing**/4 
	union all
	select * from [Usr].[UserDefaultTransportView] where @msgTypeId <> /**Mailing**/4 
	) t on s.[UserId] = t.[UserId];

  --calculate count of users
  declare @cnt int;
  set @cnt = (select count(*) from @toUsers);

  --get limits
  declare @dayLimit int;
  declare @onceLimit int;
  select @dayLimit = [LimitForDay], @onceLimit = [LimitForOnce] 
  from [Cfg].[MessageType]
  where [Id] = @msgTypeId;

  declare @exceedOnce bit;
  declare @exceedDay bit;
  declare @sentOverToday int;
  set @exceedOnce = 0;
  set @exceedDay = 0;

  --check potential exceed
  if @onceLimit is not null and @cnt > @onceLimit
    set @exceedOnce = 1;
  else if @dayLimit is not null 
  begin 
    set @sentOverToday = (select count(*) 
			from [Msg].[MessageCountOverToday] 
			where [FromUserId] = @from and [MessageTypeId] = @msgTypeId);
    if @cnt + @sentOverToday > @dayLimit
		set @exceedDay = 1;
  end

  --return first result set: information about exceed
  select 
	  @cnt as [SelectionCount],
	  @exceedDay as [ExceedDay],
	  @dayLimit as [DayLimit],
	  @sentOverToday as [SentOverToday],
	  @exceedOnce as [ExceedOnce],
	  @onceLimit as [OnceLimit];
  --quit if has exceed
  if @exceedDay = 1 or @exceedOnce = 1
	return;

  declare @result table (
    [MsgId] uniqueidentifier NOT NULL,
	[ToUserId] uniqueidentifier NOT NULL,
	[ToTransportTypeId] int NOT NULL,
	[Idx] bigint NOT NULL,
	[IsFailed] bit NOT NULL,
	[IsSent] bit NOT NULL,
	[IsDelivered] bit NOT NULL,
	[IsRead] bit NOT NULL);

  --insert addressees and second result set
  insert into [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId], [IsSent])
  output 
    inserted.[MsgId],
	inserted.[ToUserId], 
	inserted.[ToTransportTypeId], 
	inserted.[Idx],
	inserted.[IsFailed],
	inserted.[IsSent],
	inserted.[IsDelivered],
	inserted.[IsRead] 
	into @result
  select 
    @msgId, 
	u.[UserId], 
	u.[TransportTypeId],
	case when u.[TransportTypeId] = /**FLChat**/0 then 1 else 0 end
  from @toUsers u
  left join [Msg].[MessageToUser] mtu 
	on mtu.[MsgId] = @msgId
	and mtu.[ToUserId] = u.[UserId]
	and mtu.[ToTransportTypeId] = u.[TransportTypeId]
  where mtu.[Idx] is null;

  --returns values
  select * from @result;
end
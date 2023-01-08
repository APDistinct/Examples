use [FLChat]
go

create procedure [Usr].[MergeUsers] 
		@master uniqueidentifier,
		@donor uniqueidentifier
as
begin
  --master's transport types 
  declare @mt table ([TransportTypeId] int NOT NULL);
  insert into @mt
  select [TransportTypeId] from [Usr].[Transport] where [UserId] = @master;

  --donors transport types
  declare @dt table ([TransportTypeId] int NOT NULL);
  insert into @dt
  select [TransportTypeId] from [Usr].[Transport] where [UserId] = @donor and [Enabled] = 1;

  --disable donor's transports 
  update [Usr].[Transport] set [Enabled] = 0 where [UserId] = @donor;

  --move new type of donor's transport into master
  insert into [Usr].[Transport] ([UserId], [TransportTypeId], [TransportOuterId])
  select @master, [TransportTypeId], [TransportOuterId]
  from [Usr].[Transport]  
  where [UserId] = @donor 
	and [TransportTypeId] not in (select * from @mt)
	and [TransportTypeId]     in (select * from @dt);

  --update master's transports which type exists in master before merge
  with dt as (
    select * from [Usr].[Transport] 
	where [UserId] = @donor	  
	  and [TransportTypeId] in (select * from @mt)
	  and [TransportTypeId] in (select * from @dt))
  update [Usr].[Transport]
  set [TransportOuterId] = dt.[TransportOuterId]
    , [Enabled] = 1
  from [Usr].[Transport] mt 
  inner join dt on dt.[TransportTypeId] = mt.[TransportTypeId] 
  where mt.[UserId] = @master;	

  declare @donorMsg [dbo].[GuidList];

  --change sender in donor's messages
  update [Msg].[Message]
  set [FromUserId] = @master
  output inserted.[Id] into @donorMsg
  where [FromUserId] = @donor;

  update [Msg].[MessageToUser]
  set [ToUserId] = @master
  where [ToUserId] = @donor;

  --disable donor
  update [Usr].[User] set [Enabled] = 0 where [Id] = @donor;

  select * from @donorMsg;
end
go
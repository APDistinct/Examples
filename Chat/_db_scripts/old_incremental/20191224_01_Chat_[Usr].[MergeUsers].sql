USE [FLChat]
GO

/****** Object:  StoredProcedure [Usr].[MergeUsers]    Script Date: 24.12.2019 18:20:48 ******/
DROP PROCEDURE IF EXISTS [Usr].[MergeUsers]
GO

/****** Object:  StoredProcedure [Usr].[MergeUsers]    Script Date: 24.12.2019 18:20:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


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
  declare @dt table (
    [TransportTypeId] int NOT NULL,
	[TransportOuterId] nvarchar(255) NULL);
  insert into @dt
  select [TransportTypeId], [TransportOuterId] 
  from [Usr].[Transport] 
  where [UserId] = @donor and [Enabled] = 1;

  --disable donor's transports 
  update [Usr].[Transport] set [TransportOuterId] = '' 
  where [UserId] = @donor and [TransportOuterId] is not null;
  update [Usr].[Transport] set [Enabled] = 0 
  where [UserId] = @donor and [TransportOuterId] is null;

  --update master's transports which type exists in master before merge
  update mt
  set [TransportOuterId] = dt.[TransportOuterId]
    , [Enabled] = 1
  from [Usr].[Transport] mt 
  inner join @dt dt on dt.[TransportTypeId] = mt.[TransportTypeId] 
  where mt.[UserId] = @master;

  --move new type of donor's transport into master
  insert into [Usr].[Transport] ([UserId], [TransportTypeId], [TransportOuterId])
  select @master, [TransportTypeId], [TransportOuterId]
  from @dt  
  where [TransportTypeId] not in (select * from @mt);	

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
GO



USE [FLChat]
GO

/****** Object:  UserDefinedFunction [Usr].[BroadcastProhibition_StructureUpward]    Script Date: 06.01.2020 7:13:13 ******/
DROP FUNCTION [Usr].[BroadcastProhibition_StructureUpward]
GO

/****** Object:  UserDefinedFunction [Usr].[BroadcastProhibition_StructureUpward]    Script Date: 06.01.2020 7:13:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



create function [Usr].[BroadcastProhibition_StructureUpward] 
(
  @userId uniqueidentifier,
  @users [dbo].[GuidList] readonly
)
returns @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE
) as
begin

  declare @curUserId uniqueidentifier;

  --broadcast prohibition structure results
  declare @bps table (
    [UserId] uniqueidentifier NOT NULL PRIMARY KEY,
	[BPS] bit NULL
  );

  declare @curList [dbo].[GuidList];
  declare @tmp uniqueidentifier;
  declare @val bit;

  --list of broadcast prohibition users
  declare @bp table ([UserId] uniqueidentifier NOT NULL PRIMARY KEY);
  
  --select all broadcast prohibitions users
  insert into @bp
  select [ProhibitionUserId] from [Usr].[BroadcastProhibition]
  where [UserId] = @userId;

  if @@ROWCOUNT = 0 return;

  ---enumerate every input user
  DECLARE ic CURSOR FORWARD_ONLY FAST_FORWARD
  FOR
    select DISTINCT [Guid] from @users;
  open ic;
  fetch next from ic into @curUserId;
  while @@FETCH_STATUS = 0
  begin
     
	set @val = (select [BPS] from @bps where [UserId] = @curUserId) 
	if @val is null
	begin
	  
  		insert into @bps ([UserId], [BPS]) values (@curUserId, null);
		--get user's owner
		set @tmp = (select [OwnerUserId] from [Usr].[User] where [Id] = @curUserId);
		--flag is broadcast prohibition structure or not
		set @val = null;
		--extract parents
		while @tmp is not null and @tmp <> @curUserId and @val is null
		begin
			--if parent has broadcast prohibition
			if (select 1 from @bp where [UserId] = @tmp) is not null begin
				--set flag
				set @val = 1;
			end	
			else 
			begin
				--if parent is still unknown
				set @val = (select [BPS] from @bps where [UserId] = @tmp);
				if (@val is null)
				begin
					--if unknown, then insert
					insert into @bps ([UserId], [BPS]) values (@tmp, null);
					--extract next parent
					set @tmp = (select [OwnerUserId] from [Usr].[User] where [Id] = @tmp);
				end
			end
		end

		update @bps set [BPS] = coalesce(@val, 0) where [BPS] is null;
	end
	if @val = 1
	  insert into @ids values (@curUserId);

    fetch next from ic into @curUserId;
  end;

  close ic;
  deallocate ic;

  return
end
GO



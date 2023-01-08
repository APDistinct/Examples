use [FLChat]
go

create schema [Test]
go

create procedure [Test].[GetTwoUsersWithTransport]
	@TransportType1 integer,
	@TransportType2 integer,
	@UserId1 uniqueidentifier out,
	@UserId2 uniqueidentifier out
as
begin
--seek user with FLChat for sending message
set @UserId1 = (select top 1 u.[Id]
  from [Usr].[Transport] t
  inner join [Usr].[User] u on t.[UserId] = u.[Id]
  where t.[TransportTypeId] = @TransportType1
    and t.[Enabled] = 1
    and u.[Enabled] = 1);

--if not exists, then create one
if @UserId1 is null
begin
  set @UserId1 = NEWID();
  insert into [Usr].[User] ([Id], [FullName]) values (@UserId1, 'Created by MessageToUser_OnUpdate_ProduceEvents test');
  insert into [Usr].[Transport] ([UserId], [TransportTypeId]) values (@UserId1, @TransportType1);
end

-- seek second user with another transport for addressee
set @UserId2 = (select top 1 u.[Id]
  from [Usr].[Transport] t
  inner join [Usr].[User] u on t.[UserId] = u.[Id]
  where t.[TransportTypeId] = @TransportType2
    and t.[Enabled] = 1
    and u.[Enabled] = 1
	and u.[Id] <> @UserId1 );

--if not exists, then create one
if @UserId2 is null
begin
  set @UserId2 = NEWID();
  insert into [Usr].[User] ([Id], [FullName]) values (@UserId2, 'Created by MessageToUser_OnUpdate_ProduceEvents test');
  insert into [Usr].[Transport] ([UserId], [TransportTypeId], [TransportOuterId]) 
    values (@UserId2, @TransportType2, cast(@UserId2 as nvarchar(255)));
end
end
go
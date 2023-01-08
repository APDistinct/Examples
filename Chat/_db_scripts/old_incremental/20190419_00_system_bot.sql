use [FLChat]
go

if exists(select * from [Usr].[User] where [Id] = '00000000-0000-0000-0000-000000000000')
begin
  update [Usr].[User] 
  set 
    [FullName] = 'FLBot',
	[Enabled] = 0,
	[IsConsultant] = 1,
	[OwnerUserId] = null,
	[Phone] = null,
	[Email] = null,
	[SignUpDate] = null
  where
    [Id] = '00000000-0000-0000-0000-000000000000';
end
else
begin
  insert into [Usr].[User] ([Id], [FullName], [IsConsultant])
  values ('00000000-0000-0000-0000-000000000000', 'FLBot', 1);
end

if not exists(select * from [Usr].[Transport] where [UserId] = '00000000-0000-0000-0000-000000000000')
begin
  insert into [Usr].[Transport] ([UserId], [TransportTypeId], [Enabled])
  values ('00000000-0000-0000-0000-000000000000', /**FLChat**/0, 1);
end

delete from [Auth].[AuthToken] where [UserId] = '00000000-0000-0000-0000-000000000000';

go

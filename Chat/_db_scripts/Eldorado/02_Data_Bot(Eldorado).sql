use [FLChat]
go

/**** Bot user ****/
if exists(select * from [Usr].[User] 
	where [Id] = '00000000-0000-0000-0000-000000000000')
begin
  update [Usr].[User] 
  set 
	[Enabled] = 1,
	[IsConsultant] = 1,
	[OwnerUserId] = null,
	[Phone] = null,
	[Email] = null,
	[SignUpDate] = null,
	[FullName] = 'Eldorado',
	[IsBot] = 1
  where
    [Id] = '00000000-0000-0000-0000-000000000000';
--	and [Enabled] <> 1
--	and [IsBot] <> 1;
end
else
begin
  insert into [Usr].[User] ([Id], [FullName], [IsConsultant], [Enabled], [IsBot])
  values ('00000000-0000-0000-0000-000000000000', 'Eldorado', 1, 1, 1);
end

USE [FLChat]
GO

create trigger [Auth].[AuthToken_OnInsertNewbe]
on [Auth].[AuthToken]
after insert
as
  --set up newbe user

  -- update user [SignUpDate] if empty
  update [Usr].[User]
  set [SignUpDate] = GETUTCDATE()
  where [SignUpDate] is null
    and [Id] in (select [UserId] from inserted);

  -- insert FLChat transport for new user
  insert into [Usr].[Transport] ([UserId], [TransportTypeId])
  select i.[UserId], /**FLChat**/0
  from inserted i
  left join [Usr].[Transport] t on i.[UserId] = t.[UserId] 
                               and t.[TransportTypeId] = /**FlChat**/0
  where t.[UserId] is null;  
GO
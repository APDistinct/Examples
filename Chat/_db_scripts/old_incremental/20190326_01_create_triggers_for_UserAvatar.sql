use [FLChat]
go

create trigger [Usr].[UserAvatar_OnInsertUpdate_UpdDateInUser]
on [Usr].[UserAvatar]
after insert, update
as
  update [Usr].[User] 
  set [AvatarUploadDate] = GETUTCDATE() 
  where [Id] in (select [UserId] from inserted)
go

create trigger [Usr].[UserAvatar_OnDelete_ClearDateInUser]
on [Usr].[UserAvatar]
after delete
as
  update [Usr].[User] 
  set [AvatarUploadDate] = null
  where [Id] in (select [UserId] from deleted)
go
  
USE [FLChat]
GO

create trigger [Usr].[User_OnDisabled]
on [Usr].[User]
after update
as
  --delete all auth tokens if user become disabled
  delete from [Auth].[AuthToken] where [UserId] in (
  select i.[Id]
  from inserted i
  inner join deleted d on i.[Id] = d.[Id]
  where i.[Enabled] = 0
    and i.[Enabled] <> d.[Enabled]
)
GO
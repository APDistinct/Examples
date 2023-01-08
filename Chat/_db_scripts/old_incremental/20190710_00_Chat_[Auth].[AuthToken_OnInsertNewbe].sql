USE [FLChat]
GO

/****** Object:  Trigger [AuthToken_OnInsertNewbe]    Script Date: 10.07.2019 15:46:09 ******/
DROP TRIGGER [Auth].[AuthToken_OnInsertNewbe]
GO

/****** Object:  Trigger [Auth].[AuthToken_OnInsertNewbe]    Script Date: 10.07.2019 15:46:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE trigger [Auth].[AuthToken_OnInsertNewbe]
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
  
  -- enable FLChat transport
  update [Usr].[Transport] 
  set [Enabled] = 1 
  where [Enabled] = 0 
    and [TransportTypeId] = /**FlChat**/0
    and [UserId] in (select [UserId] from inserted);
GO

ALTER TABLE [Auth].[AuthToken] ENABLE TRIGGER [AuthToken_OnInsertNewbe]
GO



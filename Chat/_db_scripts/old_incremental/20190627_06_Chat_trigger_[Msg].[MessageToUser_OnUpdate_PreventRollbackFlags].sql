USE [FLChat]
GO

/****** Object:  Trigger [MessageToUser_OnUpdate_PreventRollbackFlags]    Script Date: 27.06.2019 15:37:00 ******/
DROP TRIGGER IF EXISTS [Msg].[MessageToUser_OnUpdate_PreventRollbackFlags]
GO

/****** Object:  Trigger [Msg].[MessageToUser_OnUpdate_PreventRollbackFlags]    Script Date: 27.06.2019 15:37:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create trigger [Msg].[MessageToUser_OnUpdate_PreventRollbackFlags]
on [Msg].[MessageToUser]
after update
as
  if exists(
    select 
      i.[MsgId]
    from inserted i
    inner join deleted d on i.[MsgId] = d.[MsgId]
                        and i.[ToUserId] = d.[ToUserId]
                        and i.[ToTransportTypeId] = d.[ToTransportTypeId]
    where (i.[IsFailed] <> d.[IsFailed] and d.[IsFailed] = 1)
       or (i.[IsRead] <> d.[IsRead] and d.[IsRead] = 1)
	   or (i.[IsDelivered] <> d.[IsDelivered]  and d.[IsDelivered] = 1)
	   or (i.[IsSent] <> d.[IsSent] and d.[IsSent] = 1))
  begin
    throw 51001, 'Can not rollback message flags', 1;
  end
GO

ALTER TABLE [Msg].[MessageToUser] ENABLE TRIGGER [MessageToUser_OnUpdate_PreventRollbackFlags]
GO



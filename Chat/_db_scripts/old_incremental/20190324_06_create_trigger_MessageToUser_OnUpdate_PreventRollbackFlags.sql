use [FLChat]
go

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
go

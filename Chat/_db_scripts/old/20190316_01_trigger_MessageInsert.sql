USE [FLChat]
Go

create trigger [Msg].[MessageInsert]
on [Msg].[Message]
after insert
as
  --добавление событий для пользователей о внутренних персональных сообщениях
  insert into [Msg].[Event] ([UserId], [MsgId], [StatusId])
  select i.[ToUserId], i.[Id], i.[StatusId] 
  from inserted i
  where i.[MessageTypeId] = /**Personal**/0
    and i.[ToUserId] is not null 
	and i.[TransportId] is null /**только внутренние**/;
GO
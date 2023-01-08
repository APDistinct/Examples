USE [FLChat]
Go

create trigger [Msg].[MessageInsert]
on [Msg].[Message]
after insert
as
  --���������� ������� ��� ������������� � ���������� ������������ ����������
  insert into [Msg].[Event] ([UserId], [MsgId], [StatusId])
  select i.[ToUserId], i.[Id], i.[StatusId] 
  from inserted i
  where i.[MessageTypeId] = /**Personal**/0
    and i.[ToUserId] is not null 
	and i.[TransportId] is null /**������ ����������**/;
GO
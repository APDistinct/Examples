use [FLChat]
go

declare @id uniqueidentifier;
set @id = NEWID();

insert into [Msg].[Message] 
([Id], [MessageTypeId], [FromUserId], [FromTransportTypeId], [Text], [Specific])
values 
(@id, 2, '00000000-0000-0000-0000-000000000000', 0, N'Ручное обновление кнопок', N'UPD_MENU');

insert into [Msg].[MessageToUser]  ([MsgId], [ToUserId], [ToTransportTypeId])
select @id, t.[UserId], t.[TransportTypeId]
from [Usr].[Transport] t
inner join [Usr].[User] u on t.[UserId] = u.[Id]
where u.[Enabled] = 1 
  and t.[Enabled] = 1 
  and t.[TransportTypeId] = /**Telegram**/1
go
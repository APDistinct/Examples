use [FLChat]
go

declare @uid uniqueidentifier
declare @toid uniqueidentifier
set @uid = '765E9DE8-C25A-E911-A2BF-CE1FDA20D24A'
--set @toid = ''

select top 200 
  u.[FullName],
  mtu.[ToUserId],
  * 
from [Msg].[Message] m
inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
left join [Msg].[MessageError] me 
	on mtu.[MsgId] = me.[MsgId] 
	and mtu.[ToUserId] = me.[ToUserId]
	and mtu.[ToTransportTypeId] = me.[ToTransportTypeId]
inner join [Usr].[User] u on mtu.[ToUserId] = u.[Id]
where 
 -- [Text] =N'Тест телеги'
  m.[FromUserId] = @uid or mtu.[ToUserId] = @uid
--  m.[FromUserId] = @uid and mtu.[ToUserId] = @toud  --отправитель и получатель
--  m.[FromUserId] = @uid and u.[FullName] like N'%Савельева Наталья%'
order by m.[Idx] desc
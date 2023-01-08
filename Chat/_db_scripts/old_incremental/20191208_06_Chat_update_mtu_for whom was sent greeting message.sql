use [FLChat]
go

with wc as(
  select mtu.[MsgId], mtu.[ToUserId] 
  from [Msg].[MessageToUser] mtu
  where mtu.[ToTransportTypeId] = 100
),
forupd as (
select mtu.[MsgId], mtu.[ToUserId], mtu.[ToTransportTypeId], mtu.[Idx] 
from [Msg].[MessageToUser] mtu
inner join wc on mtu.[MsgId] = wc.[MsgId] and mtu.[ToUserId] = wc.[ToUserId]
where mtu.[ToTransportTypeId] <> 100
  and mtu.[IsWebChatGreeting] = 0
)
update [Msg].[MessageToUser] 
set [IsWebChatGreeting] = 1
where [Idx] in (select [Idx] from forupd)
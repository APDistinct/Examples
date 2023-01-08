use [FLChat]
go

drop VIEW if exists [Msg].[MessageStatsRowsView]
GO

create view [Msg].[MessageStatsRowsView]
as
with 
--select webchat resend messages
wcmsg as (
  select 
    m.[ForwardMsgId],
    mtu.[MsgId],
	mtu.[ToUserId],
	mtu.[ToTransportTypeId],
	mtu.[Idx],
	mtu.[IsFailed],
	mtu.[IsSent],
	mtu.[IsDelivered],
	mtu.[IsRead],
	mtu.[IsWebChatGreeting]
  from [Msg].[Message] m
  inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
  where m.[ForwardMsgId] is not null
    and m.[Specific] like N'WEBCHAT'
),
--select webchat code record
[WebChat] as (
  select 
    wcdl.[MsgId], 
	wcdl.[ToUserId], 
	count(wca.[TransportTypeId]) as [Accepted]
  from [Msg].[WebChatDeepLink] wcdl
  left join [Msg].[WebChatAccepted] wca on wcdl.[Id] = wca.[WebChatId]
  group by wcdl.[MsgId], wcdl.[ToUserId]
)
select 
  m.[Id] as [MsgId], --message id
  m.[Idx] as [MsgIdx],
  m.[MessageTypeId], -- message type
  m.[FromUserId], --from user id
  mtu.[ToUserId] as [ToUserId],
  mtu.[ToTransportTypeId] as [ToTransportTypeId],
  --m.[Idx], --message idx
  --m.[PostTm], --message posted time
  
  --m.[Text],	--message text
  --wcmsg.[Text],
  --m.[FileId], -- message file
  case when mtu.[ToTransportTypeId] = 100 then 1 else 0 end as [IsWebChat]
  --sum(case when mtu.[ToTransportTypeId] = 100 then 1 else 0 end),
  --count(distinct mtu.[ToUserId]) as [RecipientCount] --total count of recipients
  --, wcmsg.*
  , case when mtu.[IsFailed] = 1 or coalesce(wcmsg.[IsFailed], 0) = 1 then 1 else 0 end as [IsFailed]
  , case when mtu.[IsSent] = 1 or coalesce(wcmsg.[IsSent], 0) = 1 then 1 else 0 end as [IsSent]
  , case when mtu.[Idx] is not null
          and mtu.[IsFailed] = 0 and coalesce(wcmsg.[IsFailed], 0) = 0
          and mtu.[IsSent] = 0 and coalesce(wcmsg.[IsSent], 0) = 0 
		  and wcmsg.[MsgId] is not null then 1 else 0 end as [IsQuequed]
  , case when mtu.[ToTransportTypeId] = 100 and wcmsg.[MsgId] is null then 1 else 0 end as [CantSendToWebChat]
  , case when wc.[Accepted] > 0 then 1 else 0 end as [IsWebChatAccepted]
  , case when mtu.[ToTransportTypeId] = /**WebChat**/100 and mtu.[IsRead] = 1 then 1 else 0 end as [IsSmsUrlOpened]
from [Msg].[Message] m
left join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
left join wcmsg wcmsg on mtu.[MsgId] = wcmsg.[ForwardMsgId] 
                  and mtu.[ToUserId] = wcmsg.[ToUserId]
				  and mtu.[ToTransportTypeId] = /**WebChat**/100
left join [WebChat] wc on wc.[MsgId] = mtu.[MsgId]
                      and wc.[ToUserId] = mtu.[ToUserId]
					  and mtu.[ToTransportTypeId] = /**WebChat**/100
where 
  --m.[FromUserId] = 'DB9CD15F-4412-EA11-A2C3-DCF6A6FC5B19'--'41c8775b-f380-e911-a2c0-9f888bb5fde6'
  m.[FromTransportTypeId] = /**FLChat**/0 --only messages from flchat
  --and m.[MessageTypeId] in (2, 4)
  and mtu.[IsWebChatGreeting] = 0
/*group by 
  m.[Id], --message id
  m.[Idx], --message idx
  m.[PostTm], --message posted time
  m.[MessageTypeId], -- message type
  m.[Text],	--message text
  m.[FileId] -- message file*/
--order by m.[Idx] desc
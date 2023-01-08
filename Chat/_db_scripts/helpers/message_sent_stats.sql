declare @msgId uniqueidentifier;
set @msgid = 
  --'17611ACE-D315-EA11-A2C3-DCF6A6FC5B19'
  '8772EEE8-6916-EA11-A2C3-DCF6A6FC5B19'
  --'648192A7-6916-EA11-A2C3-DCF6A6FC5B19'
  --'EC1E638B-9216-EA11-A2C3-DCF6A6FC5B19'
  ;

select u.[FullName], m.*--, mtu.*
from [Msg].[Message] m
inner join [Usr].[User] u on m.[FromUserId] = u.[Id]
--inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
where m.[Id] = @msgId;

with [Frwd] as (
select m.[Id], m.[ForwardMsgId], mtu.[ToUserId], mtu.[ToTransportTypeId]
from [Msg].[Message] m
inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
where m.[ForwardMsgId] is not null
)
select 
  u.[FullName],
  u.[Phone],
  sv.*,
  --frwd.[Id] as [ForwardMsgId],
  tid.[TransportTypeId],
  tid.[TransportId]
from [Msg].[MessageStatsRowsView] sv
inner join [Usr].[User] u on sv.[ToUserId] = u.[Id]
left join [Frwd] frwd on sv.[MsgId] = frwd.[ForwardMsgId]
					 and sv.[ToUserId] = frwd.[ToUserId]
left join [Msg].[MessageTransportId] tid 
	on tid.[MsgId] = coalesce(frwd.[Id], sv.[MsgId])
	and tid.[ToUserId] = sv.[ToUserId]
 
where sv.[MsgId] = @msgid

/*select m.[Id], m.[ForwardMsgId], mtu.[ToUserId], mtu.[ToTransportTypeId],
  tid.*
from [Msg].[Message] m
inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
left join [Msg].[MessageTransportId] tid 
	on tid.[MsgId] = m.[Id]
	and tid.[ToUserId] = mtu.[ToUserId]
where m.[ForwardMsgId] is not null
  and m.[ForwardMsgId] = @msgId*/

/*select * from [Msg].[MessageError] where [MsgId] in (
select m.[Id]
from [Msg].[Message] m
inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
left join [Msg].[MessageTransportId] tid 
	on tid.[MsgId] = m.[Id]
	and tid.[ToUserId] = mtu.[ToUserId]
where m.[ForwardMsgId] is not null
  and m.[ForwardMsgId] = @msgId)*/
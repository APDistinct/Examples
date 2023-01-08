select * from [Msg].[Message] m
inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
left join [Msg].[MessageError] me on 
	me.[MsgId] = mtu.[MsgId]
	and me.[ToUserId] = mtu.[ToUserId]
	and me.[ToTransportTypeId] = mtu.[ToTransportTypeId]
where m.[MessageTypeId] = 4
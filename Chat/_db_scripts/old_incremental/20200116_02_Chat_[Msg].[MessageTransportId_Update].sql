USE [FLChat]
GO

  update mt 
  set 
	mt.MsgId = m.ForwardMsgId, 
	mt.TransportTypeId = 100
  from [Msg].[MessageTransportId] mt 
  inner join [Msg].[MessageToUser] mtu on mt.[MsgId] = mtu.[MsgId] 
                                      and mt.[ToUserId] = mtu.[ToUserId]
									  and mt.[TransportTypeId] = mtu.[ToTransportTypeId]
  inner join [Msg].[Message] m on m.Id = mtu.MsgId
  left join [Msg].[MessageTransportId] mt2 on mt2.[MsgId] = m.[ForwardMsgId]
                                          and mt2.[ToUserId] = mtu.[ToUserId]
										  and mt2.[TransportTypeId] = 100
  where mt.[TransportTypeId] = 150 
    and m.[Specific] like '%WEBCHAT%'
	and mt2.[Id] is null

GO



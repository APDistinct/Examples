USE [FLChat]
GO

disable trigger [MessageToUser_OnUpdate_ProduceEvents]
	on [Msg].[MessageToUser]
go

--disable trigger [MessageToUser_OnUpdate_PreventRollbackFlags]
--	on [Msg].[MessageToUser]
--go

update main 
set 
	  main.[IsSent] = smsmtu.IsSent
	, main.IsFailed = smsmtu.IsFailed 
FROM [Msg].[MessageToUser] main
inner join [Msg].[Message] smsmsg on main.[MsgId] = smsmsg.[ForwardMsgId]
inner join [Msg].[MessageToUser] smsmtu 
	on  smsmsg.[Id] = smsmtu.[MsgId]
	and smsmtu.[ToUserId] = main.[ToUserId]
	and smsmtu.[ToTransportTypeId] = 150
where  
		main.[ToTransportTypeId] = 100 
	and (
		(main.IsFailed <> smsmtu.[IsFailed] and smsmtu.[IsFailed] = 1)
	 or (main.IsSent   <> smsmtu.[IsSent]   and smsmtu.[IsSent] = 1)
	  )
GO

enable trigger [MessageToUser_OnUpdate_ProduceEvents]
	on [Msg].[MessageToUser]
go

--enable trigger [MessageToUser_OnUpdate_PreventRollbackFlags]
--	on [Msg].[MessageToUser]
--go
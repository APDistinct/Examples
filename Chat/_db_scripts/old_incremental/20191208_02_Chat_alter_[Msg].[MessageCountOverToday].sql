USE [FLChat]
GO

/****** Object:  View [Msg].[MessageCountOverToday]    Script Date: 09.12.2019 9:52:55 ******/
DROP VIEW [Msg].[MessageCountOverToday]
GO

/****** Object:  View [Msg].[MessageCountOverToday]    Script Date: 09.12.2019 9:52:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create   view [Msg].[MessageCountOverToday]
as
select 
	m.[MessageTypeId], 
	m.[FromUserId], 
	--CONVERT(date, DATEADD(minute, DATEPART(TZoffset, SYSDATETIMEOFFSET()), m.[PostTm])),
	count(mtu.[Idx]) as [Count]
from [Msg].[Message] m
inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
where 
  CONVERT(date, DATEADD(minute, DATEPART(TZoffset, SYSDATETIMEOFFSET()), m.[PostTm])) 
  = CONVERT(date, GETDATE())
  and mtu.[IsWebChatGreeting] = 0
group by 
	m.[MessageTypeId], 
	m.[FromUserId]
GO



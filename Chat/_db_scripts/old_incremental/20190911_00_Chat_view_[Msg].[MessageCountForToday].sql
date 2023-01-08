USE [FLChat]
GO

/****** Object:  View [dbo].[vwGetNewID]    Script Date: 11.09.2019 17:33:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create or alter view [Msg].[MessageCountOverToday]
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
group by 
	m.[MessageTypeId], 
	m.[FromUserId]
GO



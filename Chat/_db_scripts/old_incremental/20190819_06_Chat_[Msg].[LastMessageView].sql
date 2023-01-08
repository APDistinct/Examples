USE [FLChat]
GO

/****** Object:  View [Msg].[LastMessageView]    Script Date: 17.08.2019 12:25:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create or alter view [Msg].[LastMessageView]
as
with [FromRN] as (
	select 
	  ROW_NUMBER() OVER (PARTITION BY m.[FromUserId], mtu.[ToUserId] ORDER BY m.[Idx] DESC) as [RN],
	  m.[FromUserId] as [UserId], 
	  mtu.[ToUserId] as [UserOppId], 
	  m.[Id] as [MsgId],
	  m.[Idx] as [MsgIdx],
	  mtu.[ToTransportTypeId],
	  mtu.[Idx] as [MsgToUserIdx]
	from [Msg].[Message] m
	inner join [Cfg].[TransportType] fromtt on fromtt.[Id] = m.[FromTransportTypeId]
	inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
	inner join [Cfg].[MessageType] mt on m.[MessageTypeId] = mt.[Id]
	where fromtt.[InnerTransport] = 1
	  and m.[IsDeleted] = 0
	  and mt.[ShowInHistory] = 1
),
[From] as (
	select *
	from [FromRN]
	where [RN] = 1
),
[ToRn] as (
	select
	  ROW_NUMBER() OVER (PARTITION BY m.[FromUserId], mtu.[ToUserId] ORDER BY m.[Idx] DESC) as [RN],
	  mtu.[ToUserId] as [UserId], 
	  m.[FromUserId] as [UserOppId], 
	  m.[Id] as [MsgId],
	  m.[Idx] as [MsgIdx],
	  mtu.[ToTransportTypeId],
	  mtu.[Idx] as [MsgToUserIdx]
	from [Msg].[Message] m
	inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
	inner join [Cfg].[TransportType] tott on tott.[Id] = mtu.[ToTransportTypeId]
	inner join [Cfg].[MessageType] mt on m.[MessageTypeId] = mt.[Id]
	where tott.[InnerTransport] = 1
	  and m.[IsDeleted] = 0
	  and mt.[ShowInHistory] = 1
),
[To] as (
	select *
	from [ToRN]
	where [RN] = 1
),
[Union] as (
  select cast(0 as bit) as [Income], * from [From]
  union
  select cast(1 as bit) as [Income], * from [To]
),
[UnionRN] as (
  select 
    ROW_NUMBER() OVER (PARTITION BY [UserId], [UserOppId] ORDER BY [MsgIdx] DESC) as [RN],
	[Income],
	[UserId], 
	[UserOppId], 
	[MsgId],
	[MsgIdx],
	[ToTransportTypeId],
	[MsgToUserIdx]
  from [Union]
)
select
	[UserId], 
	[UserOppId], 
	[MsgId],
	[MsgIdx],
	[ToTransportTypeId],
	[Income],
	[MsgToUserIdx]
from [UnionRN]	
where [RN] = 1
GO

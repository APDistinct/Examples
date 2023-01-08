USE [FLChat]
GO

/****** Object:  View [Usr].[UserDefaultTransportView]    Script Date: 15.12.2019 11:12:00 ******/
DROP VIEW IF EXISTS [Usr].[UserDefaultTransportView]
GO

/****** Object:  View [Usr].[UserDefaultTransportView]    Script Date: 15.12.2019 11:12:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [Usr].[UserDefaultTransportView]
as
with [LUT] as (
  select lut.[UserId], lut.[TransportTypeId]
  from [Reg].[LastUsedTransport] lut
  inner join [Usr].[Transport] t 
    on lut.[UserId] = t.[UserId]
	and lut.[TransportTypeId] = t.TransportTypeId
	and t.[Enabled] = 1
),
[TT] as (
  select tt.[Id], tt.[Prior]
  from [Cfg].[TransportType] tt 
  where tt.[Enabled] = 1 and tt.[CanSelectAsDefault] = 1
),
[TOther] as (
  select t.[UserId], min(t.[TransportTypeId]) as [TransportTypeId]
  from [Usr].[Transport] t
  where t.[TransportTypeId] <> 0 and t.[TransportTypeId] < 100  
    and t.[Enabled] = 1
  group by t.[UserId]
)
select 
  u.[Id] as [UserId], 
  coalesce(
    tdef.[TransportTypeId], 
    tfl.[TransportTypeId], 
	lut.[TransportTypeId], 
	t.[TransportTypeId], 
	100) as [DefaultTransportTypeId]
from [Usr].[User] u
left join [Usr].[Transport] tdef 
  on u.[DefaultTransportTypeId] is not null
  and u.[Id] = tdef.[UserId] and tdef.[Enabled] = 1
  and u.[DefaultTransportTypeId] = tdef.[TransportTypeId]  
  --and tdef.TransportTypeId in (select [Id] from [TT])
left join [Usr].[Transport] tfl 
  on u.[Id] = tfl.[UserId] and tfl.[Enabled] = 1
  and tfl.[TransportTypeId] = /**FLChat**/0
  --and tfl.TransportTypeId in (select [Id] from [TT])
left join [LUT] lut 
  on u.[Id] = lut.[UserId]  
  and lut.TransportTypeId in (select [Id] from [TT])
left join [TOther] t on u.[Id] = t.[UserId]
where u.[Enabled] = 1
GO



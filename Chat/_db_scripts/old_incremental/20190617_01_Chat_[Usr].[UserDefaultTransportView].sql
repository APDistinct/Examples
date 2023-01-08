USE [FLChat]
GO

/****** Object:  View [Usr].[UserDefaultTransportView]    Script Date: 17.06.2019 16:47:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER view [Usr].[UserDefaultTransportView]
as
select 
  DISTINCT u.[Id] as [UserId], 
  FIRST_VALUE(t.[TransportTypeId]) 
    over (partition by u.[Id] order by 
		case when u.[DefaultTransportTypeId] = t.[TransportTypeId] then 0 else 1 end, 
		tt.[Prior] desc
	) as [DefaultTransportTypeId]
from [Usr].[User] u
inner join [Usr].[Transport] t on u.[Id] = t.[UserId] and t.[Enabled] = 1
inner join [Cfg].[TransportType] tt 
  on t.[TransportTypeId] = tt.[Id] 
  and tt.[Enabled] = 1
  and tt.[CanSelectAsDefault] = 1
where u.[Enabled] = 1  
GO



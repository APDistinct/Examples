USE [FLChat]
GO

drop function if exists [Usr].[User_GetAddresseesForExternalTrans];
go

/****** Object:  UserDefinedFunction [Usr].[User_GetAddresseesForExternalTrans]    Script Date: 18.12.2019 22:58:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create function [Usr].[User_GetAddresseesForExternalTrans] (
  @userId uniqueidentifier)
returns table
as 
  return (
  with 
  [Owner] as (
    --select [OwnerUserId] as [UserId] 
	--from [Usr].[User] 
	--where [Id] = @userId and [OwnerUserId] is not null
    select top 1 p.[UserId] 
    from [Usr].[User_GetParents](@userId, default) p
    inner join [Usr].[Transport] t on p.[UserId] = t.[UserId]
                                  and t.[Enabled] = 1
		                          and t.[TransportTypeId] = /**FLChat**/0
    order by p.[Deep] desc
  ),
  [Msgs] as (
    select m.[FromUserId] as [UserId], max(m.[Idx]) as [Idx]
    from [Msg].[MessageToUser] mtu
    inner join [Msg].[Message] m on mtu.[MsgId] = m.[Id]
    where mtu.[ToUserId] = @userId
      and m.[FromUserId] not in ('00000000-0000-0000-0000-000000000000', @userId)
	  and m.[FromUserId] not in (select [UserId] from [Owner])
    group by m.[FromUserId]),
  [Data] as (
    select [UserId], 0 as [Order], 0 as [SubOrder] from [Owner]
    union 
    select [UserId] as [UserId], 1 as [Order], [Idx] as [SubOrder] from [Msgs] )
  select d.*
  from [Data] d
  inner join [Usr].[Transport] t on d.[UserId] = t.[UserId] 
                                and t.[Enabled] = 1 
								and [TransportTypeId] = /**FLChat**/0
  --order by [Order] asc, [SubOrder] desc
)
GO



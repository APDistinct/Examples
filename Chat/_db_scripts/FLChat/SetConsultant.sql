use [FLChat]
go
update [Usr].[User] set [IsConsultant] = 0
where [Id] not in 
(select distinct 
		u.[Id]    
	FROM [Usr].[User] u
	inner join [Msg].[Message] m on(u.Id = m.FromUserId)
	where 
--	u.[Enabled] = 1 
--	and 
	m.FromTransportTypeId = 0) 

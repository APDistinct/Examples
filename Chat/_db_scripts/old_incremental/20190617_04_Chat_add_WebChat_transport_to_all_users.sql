use [FLChat]
go

--add FLChat transport to all users
insert into [Usr].[Transport] ([UserId], [TransportTypeId])
select u.[Id], /**WebChat**/100
from [Usr].[User] u
left join [Usr].[Transport] t on t.[UserId] = u.[Id] 
                             and t.[TransportTypeId] = /**WebChat**/100
where t.UserId is null
go
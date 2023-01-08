use [FLChat]
go

insert into [Usr].[Transport] ([UserId], [TransportTypeId])
select [Id], /**SMS**/150
from [Usr].[User] u
left join [Usr].[Transport] t on u.[Id] = t.[UserId] and t.[TransportTypeId] = /**SMS**/150
where u.[Enabled] = 1 
  and [Phone] is not null
  and t.[UserId] is null
go
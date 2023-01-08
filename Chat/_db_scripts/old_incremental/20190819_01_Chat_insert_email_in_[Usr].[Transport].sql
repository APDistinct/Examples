use [FLChat]
go

insert into [Usr].[Transport] ([UserId], [TransportTypeId])
select [Id], /**Email**/151
from [Usr].[User] u
left join [Usr].[Transport] t on u.[Id] = t.[UserId] and t.[TransportTypeId] = /**Email**/151
where u.[Enabled] = 1 
  and [Email] is not null
  and t.[UserId] is null
go
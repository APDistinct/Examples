use [FLChat]
go

select [TransportOuterId], [TransportTypeId], count(*) from [Usr].[Transport]
where --[TransportTypeId] = 3 and
  [TransportOuterId] <> ''
group by [TransportOuterId], [TransportTypeId]
having count(*) > 1

select * 
from [Usr].[Transport] t 
inner join [Usr].[User] u on t.[UserId] = u.[Id]
where t.[TransportTypeId] = 4 and t.[TransportOuterId] = '556302265'

update [Usr].[Transport]
set [TransportOuterId] = ''
where [TransportOuterId] = '556302265' and [TransportTypeId] = 4 and [Enabled] = 0

/*update [Usr].[Transport]
set [Enabled] = 0, [TransportOuterId] = ''
where [UserId] = '35850471-0B23-EA11-A2C3-DCF6A6FC5B19' and [TransportTypeId] = 3
go

update [Usr].[Transport]
set [Enabled] = 1
where [UserId] = '14CA3F57-C41B-EA11-A2C3-DCF6A6FC5B19' and [TransportTypeId] = 3
go*/
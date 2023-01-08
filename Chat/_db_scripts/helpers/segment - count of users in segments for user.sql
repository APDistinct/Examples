select s.[Name], s.[Id], count(*)
from [Usr].[User_GetChilds]('0811ce20-07c1-e911-a2c0-9f888bb5fde6', default, 1) c
inner join [Usr].[User] u on u.[Id] = c.[UserId]
inner join [Usr].[SegmentMember] sm on sm.[UserId] = u.[Id]
inner join [Usr].[Segment] s on sm.[SegmentId] = s.[Id]
where 
  [LastImportDate] is not null
  and [FLUserNumber] is not null
group by s.[Name], s.[Id]
order by s.[Name]
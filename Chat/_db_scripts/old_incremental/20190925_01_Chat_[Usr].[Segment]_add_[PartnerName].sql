use [FLChat]
go

alter table [Usr].[Segment]
add [PartnerName] nvarchar(100) NULL
go

create unique index [UNQ__UsrSegment__PartnerName]
  on [Usr].[Segment] ([PartnerName])
  where [PartnerName] is not null
go
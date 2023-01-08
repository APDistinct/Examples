use [FLChat]
go

create index [IDX__UsrUser__OwnerUserId]
  on [Usr].[User] ([OwnerUserId])
  include ([Id], [Enabled])
go
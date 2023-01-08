use [FLChat]
go

create table [Usr].[BroadcastProhibition] (
  [UserId] uniqueidentifier NOT NULL,
  [ProhibitionUserId] uniqueidentifier NOT NULL,
  constraint [PK__UsrBroadcastProhibition] primary key ([UserId], [ProhibitionUserId]),
  constraint [FK__UsrBroadcastProhibition__UserId]
    foreign key ([UserId])
	references [Usr].[User] ([Id])
	on delete cascade,
  constraint [FK__UsrBroadcastProhibition__ProhibitionUserId]
    foreign key ([ProhibitionUserId])
	references [Usr].[User] ([Id])
	--on delete cascade
)
go
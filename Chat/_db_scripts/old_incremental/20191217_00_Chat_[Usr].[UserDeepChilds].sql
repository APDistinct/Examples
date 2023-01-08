use [FLChat]
go

create table [Usr].[UserDeepChilds] (
  [UserId] uniqueidentifier NOT NULL,
  [ChildUserId] uniqueidentifier NOT NULL,
  constraint [PK__Usr_UserDeepChilds] primary key ([UserId], [ChildUserId]),
  constraint [FK__Usr_UserDeepChilds__UserId]
    foreign key ([UserId])
	references [Usr].[User] ([Id]),
  constraint [FK__Usr_UserDeepChilds_ChildUserId]
    foreign key ([ChildUserId])
	references [Usr].[User] ([Id])
	on delete cascade,
)
go

alter table [Usr].[User]
add [IsUseDeepChilds] bit NOT NULL default 0
go
use [FLChat]
go

create table [Usr].[UserSentry] (
  [UserId] uniqueidentifier NOT NULL PRIMARY KEY,
  constraint [FK__UsrUserSentry__UsrUser] 
    foreign key  ([UserId])
	references [Usr].[User] ([Id])
	on delete cascade
)
go
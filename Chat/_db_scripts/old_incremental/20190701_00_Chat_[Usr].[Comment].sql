use [FLChat]
go

create table [Usr].[Comment] (
  [UserId] uniqueidentifier NOT NULL,
  [UponUserId] uniqueidentifier NOT NULL,
  [Text] nvarchar(4000) NOT NULL,
  CONSTRAINT [PK__UsrComment] PRIMARY KEY ([UserId], [UponUserId]),
  CONSTRAINT [FK__UsrComment__UsrUser_1] 
    FOREIGN KEY ([UserId])
	references [Usr].[User] ([Id]),
  CONSTRAINT [FK__UsrComment__UsrUser_2] 
    FOREIGN KEY ([UponUserId])
	references [Usr].[User] ([Id]),
  CONSTRAINT [CHK_UsrComment__UserId_and_UponUserId] 
    check ([UserId] <> [UponUserId])
)
go
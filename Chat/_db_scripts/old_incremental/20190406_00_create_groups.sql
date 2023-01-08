USE [FLChat]
GO

CREATE TABLE [Usr].[Group] (
  [Id] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
  [IsDeleted] bit NOT NULL default 0,
  [Name] nvarchar(255) NULL,
  [CreatedByUserId] uniqueidentifier NOT NULL,
  [CreatedDate] datetime NOT NULL DEFAULT GETUTCDATE(),
  [IsEqual] bit NOT NULL,
  constraint [FK__UsrGroup__UsrUserOwned] foreign key ([CreatedByUserId])
    references [Usr].[User] ([Id])  
)
GO

CREATE TABLE [Usr].[GroupMember] (
  [GroupId] uniqueidentifier NOT NULL,
  [UserId] uniqueidentifier NOT NULL,
  [IsAdmin] bit NOT NULL default 0,
  constraint [PK__UsrGroupList] primary key ([UserId], [GroupId]),
  constraint [FK__UsrGroupList__UsrUser] foreign key ([UserId])
    references [Usr].[User] ([Id]),
  constraint [FK__UsrGroupList__UsrGroup] foreign key ([GroupId])
    references [Usr].[Group] ([Id])
)
GO
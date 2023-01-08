USE [FLChat]
GO

CREATE TABLE [Cfg].[MediaType] (
  [Id] integer NOT NULL PRIMARY KEY,
  [Name] varchar(50) NOT NULL UNIQUE,
  [CanBeAvatar] bit DEFAULT 0 NOT NULL
)
GO

INSERT INTO [Cfg].[MediaType] ([Id], [Name], [CanBeAvatar])
VALUES 

(1, 'image/png', 1),
(2, 'image/jpeg', 1),
(3, 'image/bmp', 1),
(4, 'image/gif', 0),
(5, 'image/tiff', 0);
GO

delete from [Usr].[UserAvatar]
GO

ALTER TABLE [Usr].[UserAvatar]
add [MediaTypeId] integer NOT NULL
GO

ALTER TABLE [Usr].[UserAvatar]
ADD CONSTRAINT [FK__UserAvatar__MediaType_Id] foreign key ([MediaTypeId])
  references [Cfg].[MediaType] ([Id])
GO
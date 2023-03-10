USE [FLChat]
GO


CREATE TABLE [Cfg].[MediaTypeGroup](
	[Id] int NOT NULL PRIMARY KEY,
	[Name] [nvarchar](50) NOT NULL	
)
GO

INSERT INTO [Cfg].[MediaTypeGroup] ([Id], [Name])
values 
(1, 'Image'),
(2, 'Office'),
(3, 'Audio'),
(4, 'Video')

GO


ALTER TABLE [Cfg].[MediaType]
add [MediaTypeGroupId] int NULL
GO

UPDATE [Cfg].[MediaType] set [MediaTypeGroupId] = 1 where [MediaTypeGroupId] IS NULL
go

ALTER TABLE [Cfg].[MediaType] 
alter column [MediaTypeGroupId] int NOT NULL
go

ALTER TABLE [Cfg].[MediaType]
ADD CONSTRAINT [FK__MediaTypeId__MediaTypeGroupId] foreign key ([MediaTypeGroupId])
  references [Cfg].[MediaTypeGroup] ([Id])

GO



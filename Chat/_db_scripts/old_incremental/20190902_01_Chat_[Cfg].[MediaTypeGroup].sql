USE [FLChat]
GO

UPDATE  [Cfg].[MediaTypeGroup] set [Name] = 'Document' where [Id] = 2
GO

UPDATE  [Cfg].[MediaTypeGroup] set [MaxLength] = 5242880
GO

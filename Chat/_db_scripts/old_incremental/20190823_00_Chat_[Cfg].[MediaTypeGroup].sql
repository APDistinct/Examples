USE [FLChat]
GO

ALTER TABLE [Cfg].[MediaTypeGroup]
add [MaxLength] int NULL
GO


ALTER TABLE [Cfg].[MediaType] 
add [Enabled] bit NOT NULL DEFAULT 1
go

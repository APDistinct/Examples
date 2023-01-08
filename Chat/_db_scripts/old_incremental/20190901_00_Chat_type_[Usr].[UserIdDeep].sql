USE [FLChat]
GO

/****** Object:  UserDefinedTableType [Usr].[UserIdDeep]    Script Date: 01.09.2019 20:15:32 ******/
DROP TYPE [Usr].[UserIdDeep]
GO

/****** Object:  UserDefinedTableType [Usr].[UserIdDeep]    Script Date: 01.09.2019 20:15:32 ******/
CREATE TYPE [Usr].[UserIdDeep] AS TABLE(
	[UserId] [uniqueidentifier] NOT NULL,
	[Deep] [int],
	UNIQUE NONCLUSTERED 
(
	[UserId] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO



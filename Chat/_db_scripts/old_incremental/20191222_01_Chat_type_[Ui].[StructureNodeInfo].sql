USE [FLChat]
GO

/****** Object:  UserDefinedTableType [Ui].[StructureNodeInfo]    Script Date: 22.12.2019 20:23:38 ******/
DROP TYPE [Ui].[StructureNodeInfo]
GO

/****** Object:  UserDefinedTableType [Ui].[StructureNodeInfo]    Script Date: 22.12.2019 20:23:38 ******/
CREATE TYPE [Ui].[StructureNodeInfo] AS TABLE(
	[NodeId] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Count] [int] NOT NULL,
	[Final] [bit] NOT NULL
)
GO



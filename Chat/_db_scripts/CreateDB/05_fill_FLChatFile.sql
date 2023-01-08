USE [FLChatFile]
GO

CREATE TABLE [FileData](
	[Id] uniqueidentifier NOT NULL,	
	[Idx] bigint NOT NULL IDENTITY(1,1),
	[MediaTypeId] [int] NOT NULL,
	[FileData] [varbinary](max) NOT NULL,
	CONSTRAINT [PK__File] PRIMARY KEY NONCLUSTERED ([Id]),
	CONSTRAINT [UNQ__File__Idx] UNIQUE CLUSTERED ( [Idx] )
)
GO

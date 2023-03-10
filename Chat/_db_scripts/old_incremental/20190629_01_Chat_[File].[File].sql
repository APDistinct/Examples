USE [FLChat]
GO

CREATE SCHEMA [File]
GO

CREATE TABLE [File].[FileInfo](
	[Id] uniqueidentifier NOT NULL DEFAULT NEWSEQUENTIALID(),
	[Idx] bigint NOT NULL IDENTITY(1,1),
	[FileOwnerId] [uniqueidentifier] NOT NULL,
	[LoadDate] [datetime] NOT NULL DEFAULT GETUTCDATE(),
	[FileName] [nvarchar](50) NULL,
	[MediaTypeId] [int] NOT NULL,
	[FileLength] [int] NOT NULL,
	CONSTRAINT [PK__FileFile__Id] PRIMARY KEY NONCLUSTERED ( [Id] ),
	CONSTRAINT [UNQ__FileFile__Idx] UNIQUE CLUSTERED ( [Idx] ),
	CONSTRAINT [FK__FileFile__UsrUser] 
	  FOREIGN KEY([FileOwnerId]) 
	  REFERENCES  [Usr].[User] ([Id]) 
	  ON DELETE CASCADE,
	CONSTRAINT [FK__FileFile__CfgMediaType] 
	  FOREIGN KEY([MediaTypeId]) 
	  REFERENCES  [Cfg].[MediaType] ([Id])
)
GO


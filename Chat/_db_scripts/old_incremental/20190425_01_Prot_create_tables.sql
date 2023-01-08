USE [FLChatProt]
GO

/****** Object:  Table [Prot].[TransportLog]    Script Date: 25.04.2019 11:57:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TransportLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[InsertedDate] datetime NOT NULL default getutcdate(),
	[TransportTypeId] int NOT NULL,
	[Outcome] bit NOT NULL,
	[Url] nvarchar(255) NULL,
	[Method] nvarchar(50) NULL,
	[Request] nvarchar(max) NULL,
	[StatusCode] int NULL,
	[Response] nvarchar(max) NULL,
	[Exception] nvarchar(max) NULL,
	[TaskId] int NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



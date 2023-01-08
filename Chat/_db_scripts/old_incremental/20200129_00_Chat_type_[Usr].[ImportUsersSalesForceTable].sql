USE [FLChat]
GO

/****** Object:  UserDefinedTableType [Usr].[UserFLImportTable]    Script Date: 29.01.2020 12:56:20 ******/
if TYPE_ID('[Usr].[ImportUsersSalesForceTable]') is null
CREATE TYPE [Usr].[ImportUsersSalesForceTable] AS TABLE(
	[SURNAME] [nvarchar](250) NULL,
	[NAME] [nvarchar](250) NULL,
	[PATRONYMIC] [nvarchar](250) NULL,
	[BIRTHDAY] [datetime2] NULL,
	[MOBILE] [nvarchar](20) NULL,
	[EMAIL] [nvarchar](250) NULL,
	[TITLE] [nvarchar](250) NULL,
	[COUNTRY] [nvarchar](250) NULL,
	[REGION] [nvarchar](250) NULL,
	[CITY] [nvarchar](250) NULL,
	[REGISTRATIONDATE] [datetime2] NULL,
	[EMAILPERMISSION] [bit] NULL,
	[SMSPERMISSION] [bit] NULL,
	[LASTORDERDATE] [datetime2] NULL,
	[FLCLUBPOINTS] [decimal](12, 2) NULL,
	[ROWNUMBER] int NULL,
	[ForeignID] [nvarchar](100) NOT NULL,
	[ForeignOwnerID] [nvarchar](100) NULL,
	[Enabled] [bit] NOT NULL default 1
)
GO



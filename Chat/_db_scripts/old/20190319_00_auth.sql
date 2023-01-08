USE [FLChat]
GO

create schema [Auth]
GO

create table [Auth].[SmsCode] (
   [UserId] uniqueidentifier NOT NULL PRIMARY KEY,
   [Code] integer NOT NULL,
   [IssueDate] datetime NOT NULL DEFAULT getdate(),
   [ExpireBySec] integer NOT NULL,
   constraint [FK__AuthSmsCode__UsrUser] foreign key ([UserId])
     references [Usr].[User] ([Id])
)
GO

drop table [Usr].[AuthToken]
GO

drop table [Cfg].[TokenType]
GO

create table [Auth].[AuthToken] (
	[Id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[UserId] uniqueidentifier NOT NULL,
	[Token] varchar(255) NOT NULL UNIQUE,
	[IssueDate] datetime NOT NULL DEFAULT getdate(),
	[ExpireBy] integer NOT NULL,
	constraint [FK__AuthToken__UsrUser] foreign key ([UserId])
      references [Usr].[User] ([Id])
)
GO
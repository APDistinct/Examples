USE [FLChatVam]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 02.09.2020 16:37:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Status' and [uid] = SCHEMA_ID('dbo')) 
begin
PRINT '[dbo].[Status]: create table'

CREATE TABLE [dbo].[Status](
	[Id] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
	constraint [PK__Status] primary key ([Id])
)
end
GO

/****** Object:  Table [dbo].[Transaction]    Script Date: 02.09.2020 16:37:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='Transaction' and [uid] = SCHEMA_ID('dbo')) 
begin
PRINT '[dbo].[Transaction]: create table'
CREATE TABLE [dbo].[Transaction](
	[Id] [bigint] NOT NULL IDENTITY(1,1),
	[Key] [nvarchar](150) NOT NULL,
	[UserId] [bigint] NOT NULL,
--	[Sum] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	constraint [PK__Transaction] primary key ([Id]),
	constraint [UNQ__Transaction__Key] unique ([Key])
)
end
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK_Transaction_Status' and [uid]=SCHEMA_ID('dbo'))
begin

ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Status]
PRINT '[FK_Transaction_Status]: create '
end
GO

--  цикличная связь  --
--if not exists(select * from sysobjects where xtype='F' 
--	and [name]='FK_Transaction_User' and [uid]=SCHEMA_ID('dbo'))
--begin
--ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_User] FOREIGN KEY([UserId])
--REFERENCES [dbo].[User] ([Id])
--ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_User]
--end
--GO

/****** Object:  Table [dbo].[TransactionDetail]    Script Date: 02.09.2020 16:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='TransactionDetail' and [uid] = SCHEMA_ID('dbo')) 
begin
PRINT '[dbo].[TransactionDetail]: create table'
CREATE TABLE [dbo].[TransactionDetail](
	[Id] [bigint] NOT NULL IDENTITY(1,1),
	[Key] [nvarchar](150) NOT NULL,
	[Action] [nvarchar](150) NOT NULL,
	[TransactionId] [bigint] NOT NULL,
	[Sum] [int] NOT NULL,
	[ExternalTaskId] [nvarchar](150) NULL,
	[TrDate] [datetime] NOT NULL,
	[StatusId] [int] NOT NULL,
	constraint [PK__TransactionDetail] primary key ([Id]),
	constraint [UNQ__TransactionDetail__Key_Action] unique ([Key], [Action])
) 
end
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK_TransactionDetail_Status' and [uid]=SCHEMA_ID('dbo'))
begin
ALTER TABLE [dbo].[TransactionDetail]  WITH CHECK ADD  CONSTRAINT [FK_TransactionDetail_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
ALTER TABLE [dbo].[TransactionDetail] CHECK CONSTRAINT [FK_TransactionDetail_Status]
PRINT '[FK_TransactionDetail_Status]: create '
end
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK_TransactionDetail_Transaction' and [uid]=SCHEMA_ID('dbo'))
begin
ALTER TABLE [dbo].[TransactionDetail]  WITH CHECK ADD  CONSTRAINT [FK_TransactionDetail_Transaction] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transaction] ([Id])
ALTER TABLE [dbo].[TransactionDetail] CHECK CONSTRAINT [FK_TransactionDetail_Transaction]
PRINT '[FK_TransactionDetail_Transaction]: create '
end
GO


/****** Object:  Table [dbo].[TransactionDetailHistory]    Script Date: 02.09.2020 16:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='TransactionDetailHistory' and [uid] = SCHEMA_ID('dbo')) 
begin
PRINT '[dbo].[TransactionDetailHistory]: create table'
CREATE TABLE [dbo].[TransactionDetailHistory](
	[Id] [bigint] NOT NULL IDENTITY(1,1),
	[StatusId] [int] NOT NULL,
	[TransactionDetailId] [bigint] NOT NULL,
	[TaskDate] [datetime] NOT NULL,
	constraint [PK__TransactionDetailHistory] primary key ([Id])
) 
end
GO


if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK_TransactionDetailHistory_TransactionDetail' and [uid]=SCHEMA_ID('dbo'))
begin
ALTER TABLE [dbo].[TransactionDetailHistory]  WITH CHECK ADD  CONSTRAINT [FK_TransactionDetailHistory_TransactionDetail] FOREIGN KEY([TransactionDetailId])
REFERENCES [dbo].[TransactionDetail] ([Id])
ALTER TABLE [dbo].[TransactionDetailHistory] CHECK CONSTRAINT [FK_TransactionDetailHistory_TransactionDetail]
PRINT '[FK_TransactionDetailHistory_TransactionDetail]: create '
end
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK_TransactionDetailHistory_Status' and [uid]=SCHEMA_ID('dbo'))
begin
ALTER TABLE [dbo].[TransactionDetailHistory]  WITH CHECK ADD  CONSTRAINT [FK_TransactionDetailHistory_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
ALTER TABLE [dbo].[TransactionDetailHistory] CHECK CONSTRAINT [FK_TransactionDetailHistory_Status]
PRINT '[FK_TransactionDetailHistory_Status]: create '
end
GO


/****** Object:  Table [dbo].[User]    Script Date: 02.09.2020 16:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='User' and [uid] = SCHEMA_ID('dbo')) 
begin
PRINT '[dbo].[User]: create table'
CREATE TABLE [dbo].[User](
	[Id] [bigint] NOT NULL IDENTITY(1,1),
	[Key] [nvarchar](50) NOT NULL,
	[TransactionId] [bigint] NULL,
	constraint [PK__User] primary key ([Id]),
	constraint [UNQ__User__Key] unique ([Key]))
end
GO

if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK_User_Transaction' and [uid]=SCHEMA_ID('dbo'))
begin
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Transaction] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transaction] ([Id])
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Transaction]
PRINT '[FK_User_Transaction]: create '
end
GO

--  цикличная связь  --
if not exists(select * from sysobjects where xtype='F' 
	and [name]='FK_Transaction_User' and [uid]=SCHEMA_ID('dbo'))
begin
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_User]
PRINT '[FK_Transaction_User]: create '
end
GO


if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='TransactionLog' and [uid] = SCHEMA_ID('dbo')) 
begin
PRINT '[dbo].[TransactionLog]: create table'
CREATE TABLE [dbo].[TransactionLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertedDate] datetime NOT NULL default getutcdate(),
	[Outcome] bit NOT NULL,
	[Url] nvarchar(255) NULL,
	[Method] nvarchar(50) NULL,
	[Request] nvarchar(max) NULL,
	[StatusCode] int NULL,
	[Response] nvarchar(max) NULL,
	[Exception] nvarchar(max) NULL,
	constraint [PK__TransactionLog] primary key ([Id])
)
end
GO


if TYPE_ID('[dbo].[IntNvarcharTable]') is null
begin
 create type [dbo].[IntNvarcharTable] as table (
  [Id] [int] NOT NULL,
  [Description] [nvarchar](500) NULL  
 PRINT '[dbo].[IntNvarcharTable]: create type '
end
)
go

/***** [dbo].[Status_UpdateValues]  *******/
create or alter procedure [dbo].[Status_UpdateValues] 
	@dt [dbo].[IntNvarcharTable] readonly
as
BEGIN
  update t
  set t.[Description] = d.[Description]
  from [dbo].[Status] t
  inner join @dt d on t.[Id] = d.[Id]
  where t.[Description] <> d.[Description]

  insert into [dbo].[Status]([Id], [Description])
  select [Id], [Description] 
  from @dt where [Id] not in (select [Id] from [dbo].[Status]);
END
GO


declare @dt [dbo].[IntNvarcharTable];

INSERT INTO @dt ([Id], [Description])
VALUES
 (101,N'Поставлена в обработку на холдирование'),
 (102,N'Холдирование завершено успешно'),
 (103,N'Холдирование завершено с ошибкой'),
 (104,N'Поставлена в обработку на редактирование суммы списания'),
 (105,N'Отмена холдирования завершена успешно'),
 (106,N'Отмена холдирования завершена с ошибкой'),
 (107,N'Холдирование новой суммы завершено успешно'),
 (108,N'Холдирование новой суммы завершено с ошибкой'),
 (109,N'Списание новой суммы завершено успешно'),
 (110,N'Списание новой суммы завершено с ошибкой'),        
 (201,N'Поставлена в обработку'),
 (202,N'Находится в обработке'),
 (203,N'Обработка завершена без ошибки'),
 (204,N'Обработка завершена с ошибкой'),
 (1,N'Завершилось успешно done'),
 (2,N'Находится в обработке work'),
 (3,N'Завершилось с ошибкой fail')

exec [dbo].[Status_UpdateValues] @dt;

GO


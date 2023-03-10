USE [master]
GO
/****** Object:  Database [FLChat]    Script Date: 20.03.2019 8:03:04 ******/
CREATE DATABASE [FLChat]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FLChat', FILENAME = N'D:\Projects\_db\MSSQL\MSSQL13.MSSQLSERVER\MSSQL\DATA\FLChat.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FLChat_log', FILENAME = N'D:\Projects\_db\MSSQL\MSSQL13.MSSQLSERVER\MSSQL\DATA\FLChat_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [FLChat] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FLChat].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FLChat] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FLChat] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FLChat] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FLChat] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FLChat] SET ARITHABORT OFF 
GO
ALTER DATABASE [FLChat] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FLChat] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FLChat] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FLChat] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FLChat] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FLChat] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FLChat] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FLChat] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FLChat] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FLChat] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FLChat] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FLChat] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FLChat] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FLChat] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FLChat] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FLChat] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FLChat] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FLChat] SET RECOVERY FULL 
GO
ALTER DATABASE [FLChat] SET  MULTI_USER 
GO
ALTER DATABASE [FLChat] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FLChat] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FLChat] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FLChat] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FLChat] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'FLChat', N'ON'
GO
ALTER DATABASE [FLChat] SET QUERY_STORE = OFF
GO
USE [FLChat]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [FLChat]
GO
/****** Object:  Schema [Auth]    Script Date: 20.03.2019 8:03:04 ******/
CREATE SCHEMA [Auth]
GO
/****** Object:  Schema [Cfg]    Script Date: 20.03.2019 8:03:04 ******/
CREATE SCHEMA [Cfg]
GO
/****** Object:  Schema [Msg]    Script Date: 20.03.2019 8:03:04 ******/
CREATE SCHEMA [Msg]
GO
/****** Object:  Schema [Usr]    Script Date: 20.03.2019 8:03:04 ******/
CREATE SCHEMA [Usr]
GO
/****** Object:  Table [Auth].[AuthToken]    Script Date: 20.03.2019 8:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Auth].[AuthToken](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Token] [varchar](255) NOT NULL,
	[IssueDate] [datetime] NOT NULL,
	[ExpireBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Auth].[SmsCode]    Script Date: 20.03.2019 8:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Auth].[SmsCode](
	[UserId] [uniqueidentifier] NOT NULL,
	[Code] [int] NOT NULL,
	[IssueDate] [datetime] NOT NULL,
	[ExpireBySec] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Cfg].[MessageType]    Script Date: 20.03.2019 8:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Cfg].[MessageType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Cfg].[MsgStatus]    Script Date: 20.03.2019 8:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Cfg].[MsgStatus](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Cfg].[TransportType]    Script Date: 20.03.2019 8:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Cfg].[TransportType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Msg].[Event]    Script Date: 20.03.2019 8:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Msg].[Event](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Tm] [datetime] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[MsgId] [uniqueidentifier] NOT NULL,
	[StatusId] [int] NOT NULL,
	[FromUserId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Msg].[EventDelivered]    Script Date: 20.03.2019 8:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Msg].[EventDelivered](
	[UserId] [uniqueidentifier] NOT NULL,
	[EventId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Msg].[Message]    Script Date: 20.03.2019 8:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Msg].[Message](
	[Id] [uniqueidentifier] NOT NULL,
	[FromUserId] [uniqueidentifier] NOT NULL,
	[MessageTypeId] [int] NOT NULL,
	[ToUserId] [uniqueidentifier] NULL,
	[TransportId] [uniqueidentifier] NULL,
	[TransportMsgId] [nvarchar](100) NULL,
	[Income] [bit] NULL,
	[AnswerToId] [uniqueidentifier] NULL,
	[StatusId] [int] NOT NULL,
	[Text] [nvarchar](4000) NULL,
	[PostTm] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Usr].[Contact]    Script Date: 20.03.2019 8:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Usr].[Contact](
	[UserId] [uniqueidentifier] NOT NULL,
	[ContactId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK__UsrContact] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Usr].[Transport]    Script Date: 20.03.2019 8:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Usr].[Transport](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[TransportTypeId] [int] NOT NULL,
	[TransportOuterId] [nvarchar](255) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Default] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Usr].[User]    Script Date: 20.03.2019 8:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Usr].[User](
	[Id] [uniqueidentifier] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[PartnerId] [uniqueidentifier] NULL,
	[FullName] [nvarchar](255) NULL,
	[IsConsultant] [bit] NOT NULL,
	[RegistrationDate] [date] NULL,
	[InsertDate] [datetime] NOT NULL,
	[SignUpDate] [datetime] NULL,
	[Phone] [varchar](20) NULL,
	[Email] [nvarchar](255) NULL,
	[Login] [varchar](255) NULL,
	[PswHash] [varchar](255) NULL,
	[OwnerUserId] [uniqueidentifier] NULL,
	[EnabledInnerTransport] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [Auth].[AuthToken] ON 

INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (1, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:23:24', CAST(N'2019-03-19T19:23:24.910' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (2, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:26:43', CAST(N'2019-03-19T19:26:43.323' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (3, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:27:42', CAST(N'2019-03-19T19:27:42.957' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (4, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:28:46', CAST(N'2019-03-19T19:28:46.807' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (5, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:29:23', CAST(N'2019-03-19T19:29:23.837' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (6, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:33:54', CAST(N'2019-03-19T19:33:54.577' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (7, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:34:18', CAST(N'2019-03-19T19:34:18.127' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (8, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:35:14', CAST(N'2019-03-19T19:35:14.303' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (9, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:36:15', CAST(N'2019-03-19T19:36:15.440' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (10, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:39:19', CAST(N'2019-03-19T19:39:19.350' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (11, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:39:24', CAST(N'2019-03-19T19:39:24.410' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (12, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:39:52', CAST(N'2019-03-19T19:39:52.597' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (13, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:44:32', CAST(N'2019-03-19T19:44:32.240' AS DateTime), 7776000)
INSERT [Auth].[AuthToken] ([Id], [UserId], [Token], [IssueDate], [ExpireBy]) VALUES (14, N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', N'01e28d80-fe49-e911-82e7-1c1b0dafbcae19.03.2019 19:50:05', CAST(N'2019-03-19T19:50:05.413' AS DateTime), 7776000)
SET IDENTITY_INSERT [Auth].[AuthToken] OFF
INSERT [Auth].[SmsCode] ([UserId], [Code], [IssueDate], [ExpireBySec]) VALUES (N'cf4034ff-fe46-e911-82e7-1c1b0dafbcae', 452105, CAST(N'2019-03-19T14:06:43.787' AS DateTime), 300)
INSERT [Cfg].[MessageType] ([Id], [Name]) VALUES (2, N'Broadcast')
INSERT [Cfg].[MessageType] ([Id], [Name]) VALUES (1, N'Group')
INSERT [Cfg].[MessageType] ([Id], [Name]) VALUES (100, N'Initial chat')
INSERT [Cfg].[MessageType] ([Id], [Name]) VALUES (0, N'Personal')
INSERT [Cfg].[MsgStatus] ([Id], [Name]) VALUES (0, N'deleted')
INSERT [Cfg].[MsgStatus] ([Id], [Name]) VALUES (2, N'delivered')
INSERT [Cfg].[MsgStatus] ([Id], [Name]) VALUES (-1, N'failed')
INSERT [Cfg].[MsgStatus] ([Id], [Name]) VALUES (3, N'read')
INSERT [Cfg].[MsgStatus] ([Id], [Name]) VALUES (1, N'sent')
INSERT [Cfg].[TransportType] ([Id], [Name]) VALUES (5, N'OK')
INSERT [Cfg].[TransportType] ([Id], [Name]) VALUES (1, N'Telegram')
INSERT [Cfg].[TransportType] ([Id], [Name]) VALUES (3, N'Viber')
INSERT [Cfg].[TransportType] ([Id], [Name]) VALUES (4, N'VK')
INSERT [Cfg].[TransportType] ([Id], [Name]) VALUES (2, N'WhatsApp')
INSERT [Msg].[Message] ([Id], [FromUserId], [MessageTypeId], [ToUserId], [TransportId], [TransportMsgId], [Income], [AnswerToId], [StatusId], [Text], [PostTm]) VALUES (N'9ab3facf-c147-e911-82e7-1c1b0dafbcae', N'e40532bb-c147-e911-82e7-1c1b0dafbcae', 0, N'99b3facf-c147-e911-82e7-1c1b0dafbcae', NULL, NULL, NULL, NULL, 1, N'Text message', CAST(N'2019-03-16T08:02:21.590' AS DateTime))
INSERT [Msg].[Message] ([Id], [FromUserId], [MessageTypeId], [ToUserId], [TransportId], [TransportMsgId], [Income], [AnswerToId], [StatusId], [Text], [PostTm]) VALUES (N'5e513419-c347-e911-82e7-1c1b0dafbcae', N'e40532bb-c147-e911-82e7-1c1b0dafbcae', 0, N'99b3facf-c147-e911-82e7-1c1b0dafbcae', NULL, NULL, NULL, NULL, 1, N'Text message', CAST(N'2019-03-16T08:11:28.853' AS DateTime))
INSERT [Msg].[Message] ([Id], [FromUserId], [MessageTypeId], [ToUserId], [TransportId], [TransportMsgId], [Income], [AnswerToId], [StatusId], [Text], [PostTm]) VALUES (N'5f513419-c347-e911-82e7-1c1b0dafbcae', N'e40532bb-c147-e911-82e7-1c1b0dafbcae', 0, N'99b3facf-c147-e911-82e7-1c1b0dafbcae', NULL, NULL, NULL, NULL, 1, N'Text message', CAST(N'2019-03-16T08:11:34.247' AS DateTime))
INSERT [Msg].[Message] ([Id], [FromUserId], [MessageTypeId], [ToUserId], [TransportId], [TransportMsgId], [Income], [AnswerToId], [StatusId], [Text], [PostTm]) VALUES (N'b7a9e005-c447-e911-82e7-1c1b0dafbcae', N'e40532bb-c147-e911-82e7-1c1b0dafbcae', 0, N'99b3facf-c147-e911-82e7-1c1b0dafbcae', NULL, NULL, NULL, NULL, 1, N'Text message', CAST(N'2019-03-16T08:18:05.927' AS DateTime))
INSERT [Msg].[Message] ([Id], [FromUserId], [MessageTypeId], [ToUserId], [TransportId], [TransportMsgId], [Income], [AnswerToId], [StatusId], [Text], [PostTm]) VALUES (N'a9079b50-eb47-e911-82e7-1c1b0dafbcae', N'e40532bb-c147-e911-82e7-1c1b0dafbcae', 0, N'99b3facf-c147-e911-82e7-1c1b0dafbcae', NULL, NULL, NULL, NULL, 1, N'Text message', CAST(N'2019-03-16T12:59:21.670' AS DateTime))
INSERT [Msg].[Message] ([Id], [FromUserId], [MessageTypeId], [ToUserId], [TransportId], [TransportMsgId], [Income], [AnswerToId], [StatusId], [Text], [PostTm]) VALUES (N'014f3a9c-ee47-e911-82e7-1c1b0dafbcae', N'e40532bb-c147-e911-82e7-1c1b0dafbcae', 0, N'99b3facf-c147-e911-82e7-1c1b0dafbcae', NULL, NULL, NULL, NULL, 1, N'Text message', CAST(N'2019-03-16T13:22:57.033' AS DateTime))
INSERT [Msg].[Message] ([Id], [FromUserId], [MessageTypeId], [ToUserId], [TransportId], [TransportMsgId], [Income], [AnswerToId], [StatusId], [Text], [PostTm]) VALUES (N'94a1400f-5148-e911-82e7-1c1b0dafbcae', N'e40532bb-c147-e911-82e7-1c1b0dafbcae', 0, N'99b3facf-c147-e911-82e7-1c1b0dafbcae', NULL, NULL, NULL, NULL, 1, N'Text message', CAST(N'2019-03-17T01:07:40.693' AS DateTime))
INSERT [Usr].[User] ([Id], [Enabled], [PartnerId], [FullName], [IsConsultant], [RegistrationDate], [InsertDate], [SignUpDate], [Phone], [Email], [Login], [PswHash], [OwnerUserId], [EnabledInnerTransport]) VALUES (N'cf4034ff-fe46-e911-82e7-1c1b0dafbcae', 1, NULL, N'Test1', 1, NULL, CAST(N'2019-03-15T08:47:43.873' AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [Usr].[User] ([Id], [Enabled], [PartnerId], [FullName], [IsConsultant], [RegistrationDate], [InsertDate], [SignUpDate], [Phone], [Email], [Login], [PswHash], [OwnerUserId], [EnabledInnerTransport]) VALUES (N'd04034ff-fe46-e911-82e7-1c1b0dafbcae', 1, NULL, N'Test2', 0, NULL, CAST(N'2019-03-15T08:47:46.687' AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0)
INSERT [Usr].[User] ([Id], [Enabled], [PartnerId], [FullName], [IsConsultant], [RegistrationDate], [InsertDate], [SignUpDate], [Phone], [Email], [Login], [PswHash], [OwnerUserId], [EnabledInnerTransport]) VALUES (N'e40532bb-c147-e911-82e7-1c1b0dafbcae', 1, NULL, N'created by test', 0, NULL, CAST(N'2019-03-16T08:01:41.637' AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 1)
INSERT [Usr].[User] ([Id], [Enabled], [PartnerId], [FullName], [IsConsultant], [RegistrationDate], [InsertDate], [SignUpDate], [Phone], [Email], [Login], [PswHash], [OwnerUserId], [EnabledInnerTransport]) VALUES (N'99b3facf-c147-e911-82e7-1c1b0dafbcae', 1, NULL, N'created by test', 0, NULL, CAST(N'2019-03-16T08:02:16.507' AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 1)
INSERT [Usr].[User] ([Id], [Enabled], [PartnerId], [FullName], [IsConsultant], [RegistrationDate], [InsertDate], [SignUpDate], [Phone], [Email], [Login], [PswHash], [OwnerUserId], [EnabledInnerTransport]) VALUES (N'1bdf6a60-fc49-e911-82e7-1c1b0dafbcae', 1, NULL, N'created by test', 0, NULL, CAST(N'2019-03-19T04:06:31.970' AS DateTime), NULL, N'858360756', NULL, NULL, NULL, NULL, 0)
INSERT [Usr].[User] ([Id], [Enabled], [PartnerId], [FullName], [IsConsultant], [RegistrationDate], [InsertDate], [SignUpDate], [Phone], [Email], [Login], [PswHash], [OwnerUserId], [EnabledInnerTransport]) VALUES (N'01e28d80-fe49-e911-82e7-1c1b0dafbcae', 1, NULL, N'created by test', 1, NULL, CAST(N'2019-03-19T04:21:44.880' AS DateTime), NULL, N'668564447', NULL, NULL, NULL, NULL, 0)
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__AuthToke__1EB4F817C68997F1]    Script Date: 20.03.2019 8:03:04 ******/
ALTER TABLE [Auth].[AuthToken] ADD UNIQUE NONCLUSTERED 
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__MessageT__737584F6110B4502]    Script Date: 20.03.2019 8:03:04 ******/
ALTER TABLE [Cfg].[MessageType] ADD UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__MsgStatu__737584F634A48D1D]    Script Date: 20.03.2019 8:03:04 ******/
ALTER TABLE [Cfg].[MsgStatus] ADD UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Transpor__737584F66FCB5E89]    Script Date: 20.03.2019 8:03:04 ******/
ALTER TABLE [Cfg].[TransportType] ADD UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UQ__EventDel__7944C8116004342E]    Script Date: 20.03.2019 8:03:04 ******/
ALTER TABLE [Msg].[EventDelivered] ADD UNIQUE NONCLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UNQ__UsrTransport]    Script Date: 20.03.2019 8:03:04 ******/
ALTER TABLE [Usr].[Transport] ADD  CONSTRAINT [UNQ__UsrTransport] UNIQUE NONCLUSTERED 
(
	[TransportTypeId] ASC,
	[TransportOuterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UNQ__UsrUser_Email]    Script Date: 20.03.2019 8:03:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrUser_Email] ON [Usr].[User]
(
	[Email] ASC
)
WHERE ([Email] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UNQ__UsrUser_Login]    Script Date: 20.03.2019 8:03:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrUser_Login] ON [Usr].[User]
(
	[Login] ASC
)
WHERE ([Login] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UNQ__UsrUser_PartnerId]    Script Date: 20.03.2019 8:03:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrUser_PartnerId] ON [Usr].[User]
(
	[PartnerId] ASC
)
WHERE ([PartnerId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UNQ__UsrUser_Phone]    Script Date: 20.03.2019 8:03:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrUser_Phone] ON [Usr].[User]
(
	[Phone] ASC
)
WHERE ([Phone] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [Auth].[AuthToken] ADD  DEFAULT (getdate()) FOR [IssueDate]
GO
ALTER TABLE [Auth].[SmsCode] ADD  DEFAULT (getdate()) FOR [IssueDate]
GO
ALTER TABLE [Msg].[Event] ADD  DEFAULT (getutcdate()) FOR [Tm]
GO
ALTER TABLE [Msg].[Message] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [Msg].[Message] ADD  DEFAULT (getutcdate()) FOR [PostTm]
GO
ALTER TABLE [Usr].[Transport] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [Usr].[Transport] ADD  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [Usr].[Transport] ADD  DEFAULT ((0)) FOR [Default]
GO
ALTER TABLE [Usr].[User] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [Usr].[User] ADD  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [Usr].[User] ADD  DEFAULT ((0)) FOR [IsConsultant]
GO
ALTER TABLE [Usr].[User] ADD  DEFAULT (getutcdate()) FOR [InsertDate]
GO
ALTER TABLE [Usr].[User] ADD  DEFAULT ((0)) FOR [EnabledInnerTransport]
GO
ALTER TABLE [Auth].[AuthToken]  WITH CHECK ADD  CONSTRAINT [FK__AuthToken__UsrUser] FOREIGN KEY([UserId])
REFERENCES [Usr].[User] ([Id])
GO
ALTER TABLE [Auth].[AuthToken] CHECK CONSTRAINT [FK__AuthToken__UsrUser]
GO
ALTER TABLE [Auth].[SmsCode]  WITH CHECK ADD  CONSTRAINT [FK__AuthSmsCode__UsrUser] FOREIGN KEY([UserId])
REFERENCES [Usr].[User] ([Id])
GO
ALTER TABLE [Auth].[SmsCode] CHECK CONSTRAINT [FK__AuthSmsCode__UsrUser]
GO
ALTER TABLE [Msg].[Event]  WITH CHECK ADD  CONSTRAINT [FK__MsgEvent__CfgMsgStatus] FOREIGN KEY([StatusId])
REFERENCES [Cfg].[MsgStatus] ([Id])
GO
ALTER TABLE [Msg].[Event] CHECK CONSTRAINT [FK__MsgEvent__CfgMsgStatus]
GO
ALTER TABLE [Msg].[Event]  WITH CHECK ADD  CONSTRAINT [FK__MsgEvent__MsgMessage] FOREIGN KEY([MsgId])
REFERENCES [Msg].[Message] ([Id])
GO
ALTER TABLE [Msg].[Event] CHECK CONSTRAINT [FK__MsgEvent__MsgMessage]
GO
ALTER TABLE [Msg].[Event]  WITH CHECK ADD  CONSTRAINT [FK__MsgEvent__UsrUser] FOREIGN KEY([UserId])
REFERENCES [Usr].[User] ([Id])
GO
ALTER TABLE [Msg].[Event] CHECK CONSTRAINT [FK__MsgEvent__UsrUser]
GO
ALTER TABLE [Msg].[Event]  WITH CHECK ADD  CONSTRAINT [FK__MsgEvent__UsrUser__From] FOREIGN KEY([FromUserId])
REFERENCES [Usr].[User] ([Id])
GO
ALTER TABLE [Msg].[Event] CHECK CONSTRAINT [FK__MsgEvent__UsrUser__From]
GO
ALTER TABLE [Msg].[EventDelivered]  WITH CHECK ADD  CONSTRAINT [FK__MsgEventDelivered__MsgEvent] FOREIGN KEY([EventId])
REFERENCES [Msg].[Event] ([Id])
GO
ALTER TABLE [Msg].[EventDelivered] CHECK CONSTRAINT [FK__MsgEventDelivered__MsgEvent]
GO
ALTER TABLE [Msg].[EventDelivered]  WITH CHECK ADD  CONSTRAINT [FK__MsgEventDelivered__UsrUser] FOREIGN KEY([UserId])
REFERENCES [Usr].[User] ([Id])
GO
ALTER TABLE [Msg].[EventDelivered] CHECK CONSTRAINT [FK__MsgEventDelivered__UsrUser]
GO
ALTER TABLE [Msg].[Message]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessage__AnswerMessage] FOREIGN KEY([AnswerToId])
REFERENCES [Msg].[Message] ([Id])
GO
ALTER TABLE [Msg].[Message] CHECK CONSTRAINT [FK__MsgMessage__AnswerMessage]
GO
ALTER TABLE [Msg].[Message]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessage__CfgMessageType] FOREIGN KEY([MessageTypeId])
REFERENCES [Cfg].[MessageType] ([Id])
GO
ALTER TABLE [Msg].[Message] CHECK CONSTRAINT [FK__MsgMessage__CfgMessageType]
GO
ALTER TABLE [Msg].[Message]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessage__CfgMsgStatus] FOREIGN KEY([StatusId])
REFERENCES [Cfg].[MsgStatus] ([Id])
GO
ALTER TABLE [Msg].[Message] CHECK CONSTRAINT [FK__MsgMessage__CfgMsgStatus]
GO
ALTER TABLE [Msg].[Message]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessage__FromUser] FOREIGN KEY([FromUserId])
REFERENCES [Usr].[User] ([Id])
GO
ALTER TABLE [Msg].[Message] CHECK CONSTRAINT [FK__MsgMessage__FromUser]
GO
ALTER TABLE [Msg].[Message]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessage__ToUser] FOREIGN KEY([ToUserId])
REFERENCES [Usr].[User] ([Id])
GO
ALTER TABLE [Msg].[Message] CHECK CONSTRAINT [FK__MsgMessage__ToUser]
GO
ALTER TABLE [Msg].[Message]  WITH CHECK ADD  CONSTRAINT [FK__MsgMessage__UsrTransport] FOREIGN KEY([TransportId])
REFERENCES [Usr].[Transport] ([Id])
GO
ALTER TABLE [Msg].[Message] CHECK CONSTRAINT [FK__MsgMessage__UsrTransport]
GO
ALTER TABLE [Usr].[Contact]  WITH CHECK ADD  CONSTRAINT [FK__UsrContact__UsrUser1] FOREIGN KEY([UserId])
REFERENCES [Usr].[User] ([Id])
GO
ALTER TABLE [Usr].[Contact] CHECK CONSTRAINT [FK__UsrContact__UsrUser1]
GO
ALTER TABLE [Usr].[Contact]  WITH CHECK ADD  CONSTRAINT [FK__UsrContact__UsrUser2] FOREIGN KEY([ContactId])
REFERENCES [Usr].[User] ([Id])
GO
ALTER TABLE [Usr].[Contact] CHECK CONSTRAINT [FK__UsrContact__UsrUser2]
GO
ALTER TABLE [Usr].[Transport]  WITH CHECK ADD  CONSTRAINT [FK__UsrTransport__UsrUser] FOREIGN KEY([UserId])
REFERENCES [Usr].[User] ([Id])
GO
ALTER TABLE [Usr].[Transport] CHECK CONSTRAINT [FK__UsrTransport__UsrUser]
GO
ALTER TABLE [Usr].[Transport]  WITH CHECK ADD  CONSTRAINT [FK_UsrTransport__CfgTransportType] FOREIGN KEY([TransportTypeId])
REFERENCES [Cfg].[TransportType] ([Id])
GO
ALTER TABLE [Usr].[Transport] CHECK CONSTRAINT [FK_UsrTransport__CfgTransportType]
GO
ALTER TABLE [Usr].[User]  WITH CHECK ADD  CONSTRAINT [FK__UsrUser__UsrUser_OwnerUserId] FOREIGN KEY([OwnerUserId])
REFERENCES [Usr].[User] ([Id])
GO
ALTER TABLE [Usr].[User] CHECK CONSTRAINT [FK__UsrUser__UsrUser_OwnerUserId]
GO
USE [master]
GO
ALTER DATABASE [FLChat] SET  READ_WRITE 
GO

USE [FLChat]
GO

/****** Object:  Index [PK__MediaTyp__3214EC072F3BA685]    Script Date: 02.09.2019 8:14:45 ******/
ALTER TABLE [File].[FileInfo] drop constraint [FK__FileFile__CfgMediaType]
GO
ALTER TABLE [Usr].[UserAvatar] drop constraint [FK__UserAvatar__MediaType_Id]
GO

declare @name nvarchar(100)
select @name = [name] 
from sys.objects 
where type = 'PK' 
  and parent_object_id = object_id('[Cfg].[MediaType]')
  and [name] like '%PK__MediaTyp%'
if (@name is not null)
  begin
     exec ('alter table [Cfg].[MediaType] drop constraint [' + @name +']')
  end

DELETE FROM [Cfg].[MediaType] 
GO

ALTER TABLE [Cfg].[MediaType] DROP column [Id]
GO
ALTER TABLE [Cfg].[MediaType] 
add [Id] [int] NOT NULL IDENTITY(1,1)

ALTER TABLE [Cfg].[MediaType]  ADD CONSTRAINT [PK__MediaType] PRIMARY KEY CLUSTERED ([Id])
GO

--ALTER TABLE [Cfg].[MediaType] CHECK CONSTRAINT [PK__MediaType]
GO
SET IDENTITY_INSERT [Cfg].[MediaType] ON

INSERT [Cfg].[MediaType] ([Id], [Name], [CanBeAvatar], [MediaTypeGroupId], [Enabled]) VALUES (1, N'image/png', 1, 1, 1)
INSERT [Cfg].[MediaType] ([Id], [Name], [CanBeAvatar], [MediaTypeGroupId], [Enabled]) VALUES (2, N'image/jpeg', 1, 1, 1)
INSERT [Cfg].[MediaType] ([Id], [Name], [CanBeAvatar], [MediaTypeGroupId], [Enabled]) VALUES (3, N'image/bmp', 1, 1, 1)
INSERT [Cfg].[MediaType] ([Id], [Name], [CanBeAvatar], [MediaTypeGroupId], [Enabled]) VALUES (4, N'image/gif', 0, 1, 1)
INSERT [Cfg].[MediaType] ([Id], [Name], [CanBeAvatar], [MediaTypeGroupId], [Enabled]) VALUES (5, N'image/tiff', 0, 1, 1)
SET IDENTITY_INSERT [Cfg].[MediaType] OFF
SET ANSI_PADDING ON
GO

ALTER TABLE [File].[FileInfo]  WITH CHECK ADD  CONSTRAINT [FK__FileFile__CfgMediaType] FOREIGN KEY([MediaTypeId])
REFERENCES [Cfg].[MediaType] ([Id])
GO

ALTER TABLE [File].[FileInfo] CHECK CONSTRAINT [FK__FileFile__CfgMediaType]
GO
ALTER TABLE [Usr].[UserAvatar]  WITH CHECK ADD  CONSTRAINT [FK__UserAvatar__MediaType_Id] FOREIGN KEY([MediaTypeId])
REFERENCES [Cfg].[MediaType] ([Id])
GO

ALTER TABLE [Usr].[UserAvatar] CHECK CONSTRAINT [FK__UserAvatar__MediaType_Id]
GO

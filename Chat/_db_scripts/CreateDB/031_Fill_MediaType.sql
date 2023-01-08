use [FLChat]
go

declare @dt as table (
  [Id] integer NOT NULL  PRIMARY KEY,
  [Name] varchar(500) NOT NULL UNIQUE,
  [CanBeAvatar] bit DEFAULT 0 NOT NULL,
  [MediaTypeGroupId] int NOT NULL,
  [Enabled] bit DEFAULT 0 NOT NULL
)

INSERT INTO @dt ([Id], [Name], [CanBeAvatar], [MediaTypeGroupId], [Enabled])
VALUES 
(1, 'image/png', 1, 1, 1),
(2, 'image/jpeg', 1, 1, 1),
(3, 'image/bmp', 1, 1, 1),
(4, 'image/gif', 0, 1, 1),
(5, 'image/tiff', 0, 1, 1),
(6, 'application/msword',  0, 2, 1),
(7, 'application/vnd.ms-powerpoint', 0, 2, 1),
(8,'application/vnd.visio', 0, 2, 1) ,
(9, 'application/vnd.ms-excel', 0, 2, 1),
(10,'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 0, 2, 1 ),
(11, 'application/vnd.openxmlformats-officedocument.wordprocessingml.template', 0, 2, 1 ),
(12, 'application/vnd.ms-word.document.macroEnabled.10, 2, 1', 0, 2, 1 ),
(13, 'application/vnd.ms-word.template.macroEnabled.10, 2, 1', 0, 2, 1 ),
(14, 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 0, 2, 1 ),
(15,'application/vnd.openxmlformats-officedocument.spreadsheetml.template', 0, 2, 1 ),
(16, 'application/vnd.ms-excel.sheet.macroEnabled.10, 2, 1', 0, 2, 1 ),
(17, 'application/vnd.ms-excel.template.macroEnabled.10, 2, 1', 0, 2, 1 ),
(18, 'application/vnd.ms-excel.addin.macroEnabled.10, 2, 1', 0, 2, 1 ),
(19, 'application/vnd.ms-excel.sheet.binary.macroEnabled.10, 2, 1', 0, 2, 1 ),
(20, 'application/vnd.openxmlformats-officedocument.presentationml.presentation', 0, 2, 1 ),
(21, 'application/vnd.openxmlformats-officedocument.presentationml.template', 0, 2, 1 ),
(22, 'application/vnd.openxmlformats-officedocument.presentationml.slideshow', 0, 2, 1 ),
(23, 'application/vnd.ms-powerpoint.addin.macroEnabled.10, 2, 1', 0, 2, 1 ),
(24, 'application/vnd.ms-powerpoint.presentation.macroEnabled.10, 2, 1', 0, 2, 1 ),
(25, 'application/vnd.ms-powerpoint.template.macroEnabled.10, 2, 1', 0, 2, 1 ),
(26, 'application/vnd.ms-powerpoint.slideshow.macroEnabled.10, 2, 1', 0, 2, 1 ),
(27, 'application/pdf', 0, 2, 1 );

update t
set t.[CanBeAvatar] = d.[CanBeAvatar], t.[MediaTypeGroupId] = d.[MediaTypeGroupId], t.[Enabled] = d.[Enabled]
from [Cfg].[MediaType] t
inner join @dt d on t.[Name] = d.[Name]
where t.[CanBeAvatar] <> d.[CanBeAvatar]
   or t.[MediaTypeGroupId] <> d.[MediaTypeGroupId]
   or t.[Enabled] <> d.[Enabled]

insert into [Cfg].[MediaType]([Name], [CanBeAvatar], [MediaTypeGroupId], [Enabled])
select [Name], [CanBeAvatar], [MediaTypeGroupId], [Enabled] 
from @dt where [Name] not in (select [Name] from [Cfg].[MediaType]);
go

USE [FLChat]
GO


INSERT INTO [Cfg].[MediaType] ([Name], [CanBeAvatar], [MediaTypeGroupId], [Enabled])
values 
--(5, 'image/tiff', 0, 1, 1)
-- Последний вставленный на 23.08.2019

('application/msword',  0, 2, 1 ),
('application/vnd.ms-powerpoint', 0, 2, 1 ),
('application/vnd.visio', 0, 2, 1 ) ,
('application/vnd.ms-excel', 0, 2, 1 ),
('application/vnd.openxmlformats-officedocument.wordprocessingml.document', 0, 2, 1 ),
('application/vnd.openxmlformats-officedocument.wordprocessingml.template', 0, 2, 1 ),
('application/vnd.ms-word.document.macroEnabled.10, 2, 1', 0, 2, 1 ),
('application/vnd.ms-word.template.macroEnabled.10, 2, 1', 0, 2, 1 ),
('application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 0, 2, 1 ),
('application/vnd.openxmlformats-officedocument.spreadsheetml.template', 0, 2, 1 ),
('application/vnd.ms-excel.sheet.macroEnabled.10, 2, 1', 0, 2, 1 ),
('application/vnd.ms-excel.template.macroEnabled.10, 2, 1', 0, 2, 1 ),
('application/vnd.ms-excel.addin.macroEnabled.10, 2, 1', 0, 2, 1 ),
('application/vnd.ms-excel.sheet.binary.macroEnabled.10, 2, 1', 0, 2, 1 ),
('application/vnd.openxmlformats-officedocument.presentationml.presentation', 0, 2, 1 ),
('application/vnd.openxmlformats-officedocument.presentationml.template', 0, 2, 1 ),
('application/vnd.openxmlformats-officedocument.presentationml.slideshow', 0, 2, 1 ),
('application/vnd.ms-powerpoint.addin.macroEnabled.10, 2, 1', 0, 2, 1 ),
('application/vnd.ms-powerpoint.presentation.macroEnabled.10, 2, 1', 0, 2, 1 ),
('application/vnd.ms-powerpoint.template.macroEnabled.10, 2, 1', 0, 2, 1 ),
('application/vnd.ms-powerpoint.slideshow.macroEnabled.10, 2, 1', 0, 2, 1 ),
('application/pdf', 0, 2, 1 )
         

GO

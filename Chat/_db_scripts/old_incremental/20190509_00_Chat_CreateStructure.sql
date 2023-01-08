use [FLChat]
go

declare @main_name varchar
set @main_name = 'Основной ' + CONVERT(VARCHAR,SYSDATETIME())
insert into [Usr].[User] ([Enabled], [FullName], [IsConsultant]) VALUES (1,  @main_name , 1)
declare @userid_main uniqueidentifier
select top 1 @userid_main = i.[Id]   from [Usr].[User] i where [FullName] = @main_name 

declare @maxnum int
set @maxnum = 25
declare @i int
set @i = 0

while @i < @maxnum
begin
insert into [Usr].[User] ([Enabled], [FullName], [OwnerUserId]) VALUES (1, 'Test ' + str(@i,5,0), @userid_main)
set @i = @i + 1
end

declare @SegmentId uniqueidentifier
INSERT [Usr].[Segment] ([IsDeleted], [Name], [Descr]) VALUES (0, N'3-5%', N'3-5%')
select top 1 @SegmentId = i.[Id]   from [Usr].[Segment] i where [Name] = N'3-5%'

insert into [Usr].[SegmentMember] 
SELECT @SegmentId as SegmentId, [Id] as UserId FROM  [Usr].[User]
 where [OwnerUserId] = @userid_main 
 order by Id desc
OFFSET 0 ROWS
FETCH NEXT 3 ROWS ONLY

INSERT [Usr].[Segment] ([IsDeleted], [Name], [Descr]) VALUES (0, N'6-8%', N'6-8%')
select top 1 @SegmentId = i.[Id]   from [Usr].[Segment] i where [Name] = N'6-8%'
insert into [Usr].[SegmentMember] 
SELECT @SegmentId as SegmentId, [Id] as UserId FROM  [Usr].[User] 
 where [OwnerUserId] = @userid_main 
order by Id desc 
OFFSET 3 ROWS 
FETCH NEXT 3 ROWS ONLY 

INSERT [Usr].[Segment] ([IsDeleted], [Name], [Descr]) VALUES (0, N'9-23%', N'9-23%')
select top 1 @SegmentId = i.[Id]   from [Usr].[Segment] i where [Name] = N'9-23%'
insert into [Usr].[SegmentMember] 
SELECT @SegmentId as SegmentId, [Id] as UserId FROM  [Usr].[User]
 where [OwnerUserId] = @userid_main 
 order by Id desc
OFFSET 6 ROWS
FETCH NEXT 3 ROWS ONLY

INSERT [Usr].[Segment] ([IsDeleted], [Name], [Descr]) VALUES (0, N'Потенциальные участники СП', N'Потенциальные участники СП')
select top 1 @SegmentId = i.[Id]   from [Usr].[Segment] i where [Name] = N'Потенциальные участники СП'
insert into [Usr].[SegmentMember] 
SELECT @SegmentId as SegmentId, [Id] as UserId FROM  [Usr].[User]
 where [OwnerUserId] = @userid_main 
 order by Id desc
OFFSET 9 ROWS
FETCH NEXT 3 ROWS ONLY

INSERT [Usr].[Segment] ([IsDeleted], [Name], [Descr]) VALUES (0, N'Шаги СП', N'Шаги СП')
select top 1 @SegmentId = i.[Id]   from [Usr].[Segment] i where [Name] = N'Шаги СП'
insert into [Usr].[SegmentMember] 
SELECT @SegmentId as SegmentId, [Id] as UserId FROM  [Usr].[User]
 where [OwnerUserId] = @userid_main 
 order by Id desc
OFFSET 11 ROWS
FETCH NEXT 3 ROWS ONLY

INSERT [Usr].[Segment] ([IsDeleted], [Name], [Descr]) VALUES (0, N'УД3', N'УД3')
select top 1 @SegmentId = i.[Id]   from [Usr].[Segment] i where [Name] = N'УД3'
insert into [Usr].[SegmentMember] 
SELECT @SegmentId as SegmentId, [Id] as UserId FROM  [Usr].[User]
 where [OwnerUserId] = @userid_main 
 order by Id desc
OFFSET 12 ROWS
FETCH NEXT 4 ROWS ONLY

INSERT [Usr].[Segment] ([IsDeleted], [Name], [Descr]) VALUES (0, N'Faberlic-клуб', N'Faberlic-клуб')
select top 1 @SegmentId = i.[Id]   from [Usr].[Segment] i where [Name] = N'Faberlic-клуб'
insert into [Usr].[SegmentMember] 
SELECT @SegmentId as SegmentId, [Id] as UserId FROM  [Usr].[User]
 where [OwnerUserId] = @userid_main 
 order by Id desc
OFFSET 16 ROWS
FETCH NEXT 4 ROWS ONLY

INSERT [Usr].[Segment] ([IsDeleted], [Name], [Descr]) VALUES (0, N'Временно-активные', N'Временно-активные')
select top 1 @SegmentId = i.[Id]   from [Usr].[Segment] i where [Name] = N'Временно-активные'
insert into [Usr].[SegmentMember] 
SELECT @SegmentId as SegmentId, [Id] as UserId FROM  [Usr].[User]
 where [OwnerUserId] = @userid_main 
 order by Id desc
OFFSET 19 ROWS
FETCH NEXT 3 ROWS ONLY

INSERT [Usr].[Segment] ([IsDeleted], [Name], [Descr]) VALUES (0, N'Лояльные', N'Лояльные')
select top 1 @SegmentId = i.[Id]   from [Usr].[Segment] i where [Name] = N'Лояльные'
insert into [Usr].[SegmentMember] 
SELECT @SegmentId as SegmentId, [Id] as UserId FROM  [Usr].[User]
 where [OwnerUserId] = @userid_main 
 order by Id desc
OFFSET 22 ROWS
FETCH NEXT 3 ROWS ONLY

go

--


use [FLChat]
go

declare @parent_node uniqueidentifier
select top 1 @parent_node = i.[Id]   from [Ui].[StructureNode] i where [ParentNodeId] is null
insert into [Ui].[StructureNode] ([Name], [ParentNodeId], [IsShowSegments]) VALUES ('Участники МП', @parent_node, 0)
insert into [Ui].[StructureNode] ([Name], [ParentNodeId], [IsShowSegments]) VALUES ('Потребители', @parent_node, 0)

select top 1 @parent_node = i.[Id]   from [Ui].[StructureNode] i where [Name] =  'Потребители'
insert into [Ui].[StructureNode] ([Name], [ParentNodeId], [IsShowSegments]) VALUES ('Новички', @parent_node, 1)
insert into [Ui].[StructureNode] ([Name], [ParentNodeId], [IsShowSegments]) VALUES ('Активные', @parent_node, 1)

go

--


use [FLChat]
go

insert into [Ui].[StructureNodeSegment] ([NodeId], [SegmentId]) 
VALUES (
(select top 1 i.[Id] from [Ui].[StructureNode] i where [Name] =  'Участники МП'),
(select top 1 i.[Id] from [Usr].[Segment] i where [Name] = N'3-5%')
)

insert into [Ui].[StructureNodeSegment] ([NodeId], [SegmentId]) 
VALUES (
(select top 1 i.[Id] from [Ui].[StructureNode] i where [Name] =  'Участники МП'),
(select top 1 i.[Id] from [Usr].[Segment] i where [Name] = N'6-8%')
)

insert into [Ui].[StructureNodeSegment] ([NodeId], [SegmentId]) 
VALUES (
(select top 1 i.[Id] from [Ui].[StructureNode] i where [Name] =  'Участники МП'),
(select top 1 i.[Id] from [Usr].[Segment] i where [Name] = N'9-23%')
)

insert into [Ui].[StructureNodeSegment] ([NodeId], [SegmentId]) 
VALUES (
(select top 1 i.[Id] from [Ui].[StructureNode] i where [Name] =  'Новички'),
(select top 1 i.[Id] from [Usr].[Segment] i where [Name] = N'Потенциальные участники СП')
)

insert into [Ui].[StructureNodeSegment] ([NodeId], [SegmentId]) 
VALUES (
(select top 1 i.[Id] from [Ui].[StructureNode] i where [Name] =  'Новички'),
(select top 1 i.[Id] from [Usr].[Segment] i where [Name] = N'Шаги СП')
)

insert into [Ui].[StructureNodeSegment] ([NodeId], [SegmentId]) 
VALUES (
(select top 1 i.[Id] from [Ui].[StructureNode] i where [Name] =  'Новички'),
(select top 1 i.[Id] from [Usr].[Segment] i where [Name] = N'УД3')
)

insert into [Ui].[StructureNodeSegment] ([NodeId], [SegmentId]) 
VALUES (
(select top 1 i.[Id] from [Ui].[StructureNode] i where [Name] =  'Активные'),
(select top 1 i.[Id] from [Usr].[Segment] i where [Name] = N'Временно-активные')
)

insert into [Ui].[StructureNodeSegment] ([NodeId], [SegmentId]) 
VALUES (
(select top 1 i.[Id] from [Ui].[StructureNode] i where [Name] =  'Активные'),
(select top 1 i.[Id] from [Usr].[Segment] i where [Name] = N'Faberlic-клуб')
)

insert into [Ui].[StructureNodeSegment] ([NodeId], [SegmentId]) 
VALUES (
(select top 1 i.[Id] from [Ui].[StructureNode] i where [Name] =  'Активные'),
(select top 1 i.[Id] from [Usr].[Segment] i where [Name] = N'Лояльные')
)

go


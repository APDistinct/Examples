use [FLChat]
go

--root node id
declare @root uniqueidentifier;
set @root = '00000000-0000-0000-0000-000000000000';

--prepare data for [Ui].[StructureNode]
declare @dt as table (
	[Id] uniqueidentifier NOT NULL,
	[ParentNodeId] uniqueidentifier NOT NULL,
	[Name] nvarchar(255) NOT NULL, 
	[Order] smallint NOT NULL);

insert into @dt ([Id], [Name], [ParentNodeId], [Order])
values 
	 ('51AA3D63-6E75-E911-A2C0-9F888BB5FDE6', N'Лояльные', @root, 0)
	;

--prepare data for [Usr].[Segment] and [Ui].[StructureNodeSegment]
declare @seg as table (
	[Id] uniqueidentifier NOT NULL,
	[IsDeleted] bit NOT NULL default 0,
	[Name] nvarchar(255) NOT NULL,
	[Descr] nvarchar(255),
	[ShowInShortProfile] bit NOT NULL default 0,
	[Tag] nvarchar(250) NULL,
	[PartnerName] nvarchar(100) NULL,
	[NodeId] uniqueidentifier,
	[Order] smallint
);

insert into @seg ([Id], [IsDeleted], [Name], [Descr], [ShowInShortProfile], [Tag], [PartnerName], [NodeId], [Order])
values 
 (N'066912aa-d83d-ea11-a2ce-d2f5e6be8ef8', 0, N'Новички',            N'Новички',         0, NULL, 'Newcomers', @root, 0)
,(N'4cd028cd-d83d-ea11-a2ce-d2f5e6be8ef8', 0, N'Платиновые',         N'Платиновые',      0, NULL, 'Gold', '51aa3d63-6e75-e911-a2c0-9f888bb5fde6', 0)
,(N'162383d8-d83d-ea11-a2ce-d2f5e6be8ef8', 0, N'Золотые',            N'Золотые',         0, NULL, 'Silver', '51aa3d63-6e75-e911-a2c0-9f888bb5fde6', 0)
,(N'a21aa8df-d83d-ea11-a2ce-d2f5e6be8ef8', 0, N'Серебряные',         N'Серебряные',      0, NULL, 'Bronze', '51aa3d63-6e75-e911-a2c0-9f888bb5fde6', 0)
,(N'bd4c9be8-d83d-ea11-a2ce-d2f5e6be8ef8', 0, N'Участники акции',    N'Участники акции', 0, NULL, 'Campaign participans', @root, 0)

--updating [Ui].[StructureNode] data
update sn
set
	 [Name] = d.[Name]
	,[Order] = d.[Order]
	,[ParentNodeId] = d.[ParentNodeId]
from [Ui].[StructureNode] sn
inner join @dt d on sn.[Id] = d.[Id]
where 
	   sn.[Name] <> d.[Name] 
	or sn.[Order] <> d.[Order]
	or sn.[ParentNodeId] <> d.[ParentNodeId];

--inserting new [Ui].[StructureNode] data
insert into [Ui].[StructureNode] ([Id], [Name], [ParentNodeId], [Order])
select [Id], [Name], [ParentNodeId], [Order]
from @dt
where [Id] not in (select [Id] from [Ui].[StructureNode]);

--deleting [Ui].[StructureNode] data
delete from [Ui].[StructureNode] 
where [Id] not in (select [Id] from @dt)
  and [Id] <> @root;

-- update segments
update s
set	
	[Descr] = coalesce(d.[Descr], d.[Name]),
	[IsDeleted] = d.[IsDeleted],
	[Name] = d.[Name],
	[PartnerName] = d.[PartnerName],
	[ShowInShortProfile] = d.[ShowInShortProfile],
	[Tag] = d.[Tag]
from [Usr].[Segment] s
inner join @seg d on s.[Id] = d.[Id]
where 
	   s.[Descr] <> coalesce(d.[Descr], d.[Name])
	or s.[IsDeleted] <> d.[IsDeleted]
	or s.[Name] <> d.[Name] 	 
	or coalesce(s.[PartnerName], '') <> coalesce(d.[PartnerName], '')
	or s.[ShowInShortProfile] <> d.[ShowInShortProfile]
	or coalesce(s.[Tag], '') <> coalesce(d.[Tag], '');

--insert segments
insert into [Usr].[Segment] ([Id], [IsDeleted], [Name], [Descr], [ShowInShortProfile], [Tag], [PartnerName])
select [Id], [IsDeleted], [Name], coalesce([Descr], [Name]), [ShowInShortProfile], [Tag], [PartnerName]
from @seg
where [Id] not in (select [Id] from [Usr].[Segment]);

-- update [Ui].[StructureNodeSegment]
update t
set 
  [NodeId] = d.[NodeId],
  [Order] = d.[Order]
from [Ui].[StructureNodeSegment] t
inner join @seg d on t.[SegmentId] = d.[Id]
where t.[NodeId] <> d.[NodeId]
   or t.[Order] <> d.[Order]

-- inserting [Ui].[StructureNodeSegment]
insert into [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order])
select [NodeId], [Id], [Order]
from @seg 
where [Id] not in (select [SegmentId] from [Ui].[StructureNodeSegment])

-- deleting [Ui].[StructureNodeSegment]
delete from [Ui].[StructureNodeSegment]
where [SegmentId] not in (select [Id] from @seg)

select * from [Ui].[StructureNode]
select s.*, n.[Name], ns.[Order] from [Usr].[Segment] s
inner join [Ui].[StructureNodeSegment] ns on s.[Id] = ns.[SegmentId]
inner join [Ui].[StructureNode] n on n.[Id] = ns.[NodeId]

GO
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
	 ('51AA3D63-6E75-E911-A2C0-9F888BB5FDE6', N'Участники МП', @root, 0)
	,('52AA3D63-6E75-E911-A2C0-9F888BB5FDE6', N'Потребители', @root, 0)
	,('53AA3D63-6E75-E911-A2C0-9F888BB5FDE6', N'Новички', '52AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
	,('54AA3D63-6E75-E911-A2C0-9F888BB5FDE6', N'Активные', '52AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
	,('55AA3D63-6E75-E911-A2C0-9F888BB5FDE6', N'Временные акции', '52AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
	,('EA784FE6-1315-EA11-A2C3-DCF6A6FC5B19', N'Неактивные', '52AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 1)
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
 ('48AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0, N'3%', null, 0, NULL, N'VD3', '51AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('49AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0, N'6%',	null, 0, NULL, N'VD6', '51AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('4BAA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0, N'Потенциальные участники СП', null, 0, NULL, 'STARTPROGRAMPOTENTIAL', '53AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('4CAA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0, N'Шаги СП', null, 0, NULL, 'STARTPROGRAMSTEP', '53AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('4DAA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0, N'УД6', null, 0, NULL, 'DEL6', '53AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('4EAA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0, N'Faberlic-клуб', null, 0, NULL, 'FLCLUB', '54AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('4FAA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0, N'Временно-активные', null, 0, NULL, 'TEMPACTIVE', '52AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('50AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0, N'Регулярные', null, 0, NULL, 'REGULARACTIVE', '54AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('C5DB4FCE-0FC4-E911-A2C0-9F888BB5FDE6', 0, N'Неактивные', null, 0, NULL, 'NOTACTIVE', 'EA784FE6-1315-EA11-A2C3-DCF6A6FC5B19', 0)
,('656722F6-CBC4-E911-A2C0-9F888BB5FDE6', 0, N'Testers', N'Тестировщики', 0, NULL, NULL, @root, 100)
,('824D39B8-D0C4-E911-A2C0-9F888BB5FDE6', 0, N'9%', null, 0, NULL, 'VD9', '51AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('834D39B8-D0C4-E911-A2C0-9F888BB5FDE6', 0, N'12%', null, 0, NULL, 'VD12', '51AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('844D39B8-D0C4-E911-A2C0-9F888BB5FDE6', 0, N'15%', null, 0, NULL, 'VD15', '51AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('854D39B8-D0C4-E911-A2C0-9F888BB5FDE6', 0, N'19%', null, 0, NULL, 'VD19', '51AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('864D39B8-D0C4-E911-A2C0-9F888BB5FDE6', 0, N'23%', null, 0, N'P23', 'VD23', '51AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('874D39B8-D0C4-E911-A2C0-9F888BB5FDE6', 0, N'УД18', null, 0, NULL, 'DEL18', '52AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('F8DF6E7B-48E4-E911-A2C1-F0A3CA6AFF09', 0, N'... лет в Компании', null, 0, NULL, 'YEARINCOMPANY', '54AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
--,('FD5BF0DD-48E4-E911-A2C1-F0A3CA6AFF09', 0, N'Желтенькие', null, 0, NULL, 'REACTIVE', 'EA784FE6-1315-EA11-A2C3-DCF6A6FC5B19', 0)
,('20471539-7574-EA11-A2C4-00090FAA0001', 0, N'Новички без заказа', null, 0, NULL, 'NEWNOORDER', '53AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('21471539-7574-EA11-A2C4-00090FAA0001', 0, N'Новички с заказом', null, 0, NULL, 'NEWHASORDER', '53AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('22471539-7574-EA11-A2C4-00090FAA0001', 0, N'Статус 1', null, 0, NULL, 'REACTIVESTATUS1', '55AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('23471539-7574-EA11-A2C4-00090FAA0001', 0, N'Статус 2', null, 0, NULL, 'REACTIVESTATUS2', '55AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('24471539-7574-EA11-A2C4-00090FAA0001', 0, N'Статус 3', null, 0, NULL, 'REACTIVESTATUS3', '55AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('25471539-7574-EA11-A2C4-00090FAA0001', 0, N'Статус 4', null, 0, NULL, 'REACTIVESTATUS4', '55AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('26471539-7574-EA11-A2C4-00090FAA0001', 0, N'Статус 5', null, 0, NULL, 'REACTIVESTATUS5', '55AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)
,('27471539-7574-EA11-A2C4-00090FAA0001', 0, N'Неоплаченные заказы', null, 0, NULL, 'NOTPAID', '52AA3D63-6E75-E911-A2C0-9F888BB5FDE6', 0)

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
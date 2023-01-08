use [FLChat]
go

CREATE SCHEMA [Ui]
GO

create table [Ui].[StructureNode] 
(
  [Id] uniqueidentifier NOT NULL default newsequentialid(),
  [Name] nvarchar(255) NOT NULL,
  [ParentNodeId] uniqueidentifier NULL,
  [IsShowSegments] bit NOT NULL default 0,
  [IsShowParentUsers] bit NOT NULL default 0,
  constraint [PK__UiStructureNode] primary key ([Id]),
  constraint [FK__UiStructureNode__UiStructureNode] foreign key ([ParentNodeId])
    references [Ui].[StructureNode]([Id])
)
go

create table [Ui].[StructureNodeSegment]
(
  [NodeId] uniqueidentifier NOT NULL,
  [SegmentId] uniqueidentifier NOT NULL,
  constraint [PK__UiStructureNodeSegment] primary key ([NodeId], [SegmentId]),
  constraint [FK__UiStructureNodeSegment__UsrSegment] foreign key ([SegmentId])
    references [Usr].[Segment]([Id])
	on delete cascade,
  constraint [FK__UiStructureNodeSegment__UiStructureNode] foreign key ([NodeId])
    references [Ui].[StructureNode] ([Id])
	on delete cascade
)
go

alter table [Usr].[Segment]
add [Descr] nvarchar(255) NOT NULL default ''
go
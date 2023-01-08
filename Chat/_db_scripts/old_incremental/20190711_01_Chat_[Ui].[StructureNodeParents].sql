use [FLChat]
go

create table [Ui].[StructureNodeParents] (
  [ParentNodeId] uniqueidentifier NOT NULL,
  [Name] nvarchar(255) NOT NULL,
  [Order] smallint NOT NULL,
  constraint [PK__StructureNodeParents] primary key ([ParentNodeId]),
  constraint [FK__StructureNodeParents] foreign key ([ParentNodeId])
    references [Ui].[StructureNode] ([Id])
	on delete cascade
)
go

insert into [Ui].[StructureNodeParents] ([ParentNodeId], [Name], [Order])
values ('00000000-0000-0000-0000-000000000000', 'Наставники', -1)
go
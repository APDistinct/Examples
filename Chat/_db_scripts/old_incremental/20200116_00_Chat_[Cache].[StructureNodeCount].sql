use [FLChat]
go

create schema [Cache]
go

create table [Cache].[StructureNodeCount] (
  [NodeId] uniqueidentifier NOT NULL,
  [UserId] uniqueidentifier NOT NULL,
  [Count] int NOT NULL,
  constraint [PK__Cache_StructureNodeCount] primary key ([NodeId], [UserId]),
  constraint [FK__Cache_StructureNodeCount_NodeId] foreign key ([NodeId])
    references [Ui].[StructureNode] ([Id])
	on delete cascade,
  constraint [FK__Cache_StructureNodeCount_UserId] foreign key ([UserId])
    references [Usr].[User] ([Id])
	on delete cascade
)
go
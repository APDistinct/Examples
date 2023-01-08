use [FLChat]
go

if exists(select * from [Ui].[StructureNode] where [Id] = '00000000-0000-0000-0000-000000000000')
begin
  update [Ui].[StructureNode]
  set 
    [Name] = 'Root node',
	[ParentNodeId] = null,
	[IsShowSegments] = 1,
	[IsShowParentUsers] = 0,
	[Order] = 0
  where
    [Id] = '00000000-0000-0000-0000-000000000000';
end
else
begin
  insert into [Ui].[StructureNode] ([Id], [Name], [ParentNodeId], [IsShowSegments], [IsShowParentUsers])
  values ('00000000-0000-0000-0000-000000000000', 'Root node', null, 1, 0);
end

delete from [Ui].[StructureNodeSegment] where [SegmentId] = '00000000-0000-0000-0000-000000000000';

go

update [Ui].[StructureNode]
set [ParentNodeId] = '00000000-0000-0000-0000-000000000000'
where [ParentNodeId] is null and [Id] <> '00000000-0000-0000-0000-000000000000';
go

alter table [Ui].[StructureNode]
alter column [ParentNodeId] uniqueidentifier NULL;
go

alter table [Ui].[StructureNode] 
add default ('00000000-0000-0000-0000-000000000000') FOR [ParentNodeId]
GO

alter table [Ui].[StructureNode] 
add constraint [CHK__Ui_StructureNode__ParentNodeId] check 
 ([ParentNodeId] is not null or [Id] = '00000000-0000-0000-0000-000000000000')
GO
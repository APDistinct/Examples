use [FLChat]
go

drop function if exists [Usr].[User_UiNodesCount]
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [Usr].[User_UiNodesCount] (
	@userId uniqueidentifier
)
RETURNS @result TABLE (
  [NodeId] uniqueidentifier NOT NULL UNIQUE,
  [Count] integer NOT NULL
)
AS
BEGIN
    --root node
	declare @nodeId uniqueidentifier;
	set @nodeId = '00000000-0000-0000-0000-000000000000';
	
	--all nodes hierarchy
	declare @nodesDeep table (
		[NodeId] uniqueidentifier NOT NULL,
		[InheritFromNodeId] uniqueidentifier NOT NULL);

	DECLARE icc CURSOR FORWARD_ONLY FAST_FORWARD
	FOR
		select [Id] from [Ui].[StructureNode]
	open icc;
	fetch next from icc into @nodeId;
	while @@FETCH_STATUS = 0
	begin
	  insert into @nodesDeep
	  select [NodeId], @nodeId from [Ui].[StructureNode_GetNestedNodes](@nodeId);

	  fetch next from icc into @nodeId;
	end;
	
	with [Nodes] as (
		select [NodeId], [InheritFromNodeId] from @nodesDeep
	),
	[LowSegments] as (
		select 
			n.[InheritFromNodeId] as [NodeId],
			ns.[SegmentId]	
		from [Nodes] n
		left join [Ui].[StructureNodeSegment] ns on n.[NodeId] = ns.[NodeId]
	)
	insert into @result ([NodeId], [Count])
	select 
		ls.[NodeId],
		coalesce(count(DISTINCT childs.[UserId]), 0) as [Count]
	from [LowSegments] ls
	left join [Usr].[SegmentMember] sm on sm.[SegmentId] = ls.[SegmentId]
	left join [Usr].[User_GetChildsSimple](@userId, default, default) childs on sm.[UserId] = childs.[UserId]
	group by ls.[NodeId];

	RETURN;
END
GO
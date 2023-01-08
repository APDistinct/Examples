USE [FLChat]
GO

/****** Object:  StoredProcedure [Ui].[StructureNode_GetInfo_Node]    Script Date: 16.01.2020 13:20:28 ******/
DROP PROCEDURE IF EXISTS [Ui].[StructureNode_GetInfo_Node]
GO

/****** Object:  StoredProcedure [Ui].[StructureNode_GetInfo_Node]    Script Date: 16.01.2020 13:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE procedure [Ui].[StructureNode_GetInfo_Node]
		@nodeId uniqueidentifier,
		@userId uniqueidentifier
as

if not exists(select 1 from [Ui].[StructureNode] where [Id] = @nodeId)
  return -1;

declare @result table (
  [Type] integer NOT NULL,
  [Id] uniqueidentifier NOT NULL,
  [Name] nvarchar(255) NOT NULL, 
  [Count] integer NOT NULL,
  [Order] smallint NOT NULL);

if (select [IsUseDeepChilds] from [Usr].[User] where [Id] = @userId) = 1 
begin

	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 0, sn.[Id], sn.[Name], coalesce(snc.[Count], 0), sn.[Order]
	from [Ui].[StructureNode] sn
	left join [Cache].[StructureNodeCount] snc on sn.[Id] = snc.[NodeId] and snc.[UserId] = @userId
	where sn.[ParentNodeId] = @nodeId;

	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 0, sn.[Id], sn.[Name], coalesce(snc.[Count], 0), sn.[Order]
	from [Ui].[StructureNode] sn
	left join [Cache].[StructureNodeCount] snc on sn.[Id] = snc.[NodeId] and snc.[UserId] = @userId
	where sn.[Id] = @nodeId;

  	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 
	  1,
	  s.[Id],
	  s.[Descr], 
	  coalesce(count(childs.[UserId]), 0) as [Count],
	  ns.[Order]  
	from [Ui].[StructureNodeSegment] ns
	inner join [Usr].[Segment] s on ns.[SegmentId] = s.[Id] and s.[IsDeleted] = 0
	left join [Usr].[SegmentMember] sm on sm.[SegmentId] = s.[Id]
	left join [Usr].[User_GetChildsSimple](@userId, default, default) childs on sm.[UserId] = childs.[UserId]
	where ns.[NodeId] = @nodeId
	group by s.[Id], s.[Descr], ns.[Order];

end
else
begin
	declare @nodesDeep table (
	  [NodeId] uniqueidentifier NOT NULL,
	  [InheritFromNodeId] uniqueidentifier NOT NULL);

	insert into @nodesDeep
	select [NodeId], [InheritFromNodeId] 
	from [Ui].[StructureNode_GetChilds](@nodeId);

	if @nodeId <> '00000000-0000-0000-0000-000000000000'
	begin
	  insert into @nodesDeep
	  select [NodeId], @nodeId as [InheritFromNodeId]
	  from @nodesDeep;

	  insert into @nodesDeep
	  values (@nodeId, @nodeId)
	end;

	with [NodesL1] as (
	  select n.[Id], n.[Name], n.[Order]
	  from [Ui].[StructureNode] n where n.[ParentNodeId] = @nodeId
	  union 
	  select n.[Id], n.[Name], n.[Order]
	  from [Ui].[StructureNode] n where n.[Id] = @nodeId),
	[LowSegments] as (
	  select 
		0 as [Type],
		n.[Id] as [Id],
		n.[Name],     
		n.[Order],
		ns.[SegmentId]	
	  from [NodesL1] n
	  left join @nodesDeep nd on n.[Id] = nd.[InheritFromNodeId] 
	  left join [Ui].[StructureNodeSegment] ns on nd.[NodeId] = ns.[NodeId]
	  union
	  select     
		1 as [Type],
		s.[Id] as [Id],
		s.[Descr] as [Name],     
		ns.[Order],
		ns.[SegmentId]
	  from [Ui].[StructureNodeSegment] ns
	  inner join [Usr].[Segment] s on ns.[SegmentId] = s.[Id] and s.[IsDeleted] = 0
	  where ns.[NodeId] = @nodeId)
	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 
	  ls.[Type],
	  ls.[Id],
	  ls.[Name], 
	  coalesce(count(DISTINCT childs.[UserId]), 0) as [Count],
	  ls.[Order]  
	from [LowSegments] ls
	left join [Usr].[SegmentMember] sm on sm.[SegmentId] = ls.[SegmentId]
	left join [Usr].[User_GetChildsSimple](@userId, default, default) childs on sm.[UserId] = childs.[UserId]
	group by ls.[Type], ls.[Id], ls.[Name], ls.[Order]
	order by ls.[Order];

	--get information about parents' node
	insert into @result ([Type], [Id], [Name], [Count], [Order])
	select 
	  2,
	  p.[ParentNodeId],
	  p.[Name],
	  (select count([UserId]) from [Usr].[User_GetParents](@userId, default)),
	  p.[Order]
	from [Ui].[StructureNodeParents] p
	where p.[ParentNodeId] = @nodeId;

end

-- result set #1: information about current node
select 
  'nod-' + cast(r.[Id] as nvarchar(255)) as [NodeId],
  r.[Name],  
  r.[Count] as [Count]
from @result r
where r.[Id] = @nodeId and r.[Type] = 0

--result set #2: information about child nodes
select 
  case when r.[Type] = 0 then 'nod-' + cast(r.[Id] as nvarchar(250))
       when r.[Type] = 1 then 'seg-' + cast(r.[Id] as nvarchar(250))
	   when r.[Type] = 2 then 'parents'
  end as [NodeId], 
  r.[Name], 
  r.[Count],
  cast(case when r.[Type] = 0 then 0
       else 1
  end as bit) as [Final]
from @result r
where r.[Id] <> @nodeId or (r.[Id] = @nodeId and r.[Type] <> 0)
order by r.[Order];

--return set #3: information about users is empty result set 
select top 0 * from [Usr].[User];

--return total count of users
select 0 as [TotalCount];

GO



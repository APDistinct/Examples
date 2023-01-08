USE [FLChat]
GO

/****** Object:  StoredProcedure [Ui].[StructureNode_GetInfo_Node]    Script Date: 17.12.2019 21:33:53 ******/
DROP PROCEDURE IF EXISTS [Ui].[StructureNode_GetInfo_Node]
GO

/****** Object:  StoredProcedure [Ui].[StructureNode_GetInfo_Node]    Script Date: 17.12.2019 21:33:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [Ui].[StructureNode_GetInfo_Node]
		@nodeId uniqueidentifier,
		@userId uniqueidentifier
as

declare @users [dbo].[GuidList];

--get all childs users from user's structure
insert into @users
select [UserId] from [Usr].[User_GetChildsSimple](@userId, default, default);
--select [Id] from [Usr].[User];

--segment and users
declare @nodeUsers as table (
  [NodeId] uniqueidentifier,
  [SegmentId] uniqueidentifier,
  [UserId] uniqueidentifier);

--get user from all segments linked to this node
with [Nodes] as (
  --unite childs node and current node
  select * from [Ui].[StructureNode_GetChilds](@nodeId)
  union
  select [Id], [Id], 0 from [Ui].[StructureNode] where [Id] = @nodeId
)
insert into @nodeUsers
--select all users from all segments from childs nodes
select nodes.[InheritFromNodeId], sm.[SegmentId], sm.[UserId]
from [Nodes] nodes --[Ui].[StructureNode_GetChilds](@nodeId) nodes
inner join [Ui].[StructureNodeSegment] ns on nodes.[NodeId] = ns.[NodeId]
inner join [Usr].[Segment] s on ns.[SegmentId] = s.[Id] and s.[IsDeleted] = 0
inner join [Usr].[SegmentMember] sm on sm.[SegmentId] = s.[Id]
where 
  sm.[UserId] in (select * from @users); 

--calculate total count users in current and child nodes
declare @nodeTotalCount integer;
if @nodeId <> '00000000-0000-0000-0000-000000000000' 
  set @nodeTotalCount = (select count(DISTINCT [UserId]) as [Count] from @nodeUsers);

-- result set #1: information about current node
select 
  'nod-' + cast(n.[Id] as nvarchar(255)) as [NodeId],
  n.[Name],  
  @nodeTotalCount as [Count]
from [Ui].[StructureNode] n
where n.[Id] = @nodeId;

-- break if not found
if @@ROWCOUNT = 0
  return -1;

--type for result set #2: information about direct child nodes
declare @nodes as [Ui].[StructureNodeInfo];

--get information about direct child nodes
with [NodeCount] as (
  --calculate count of users in child nodes
  select 
    nu.[NodeId] as [NodeId], count(DISTINCT nu.[UserId]) as [Count]
  from @nodeUsers nu
  where nu.[NodeId] <> @nodeId
  group by nu.[NodeId]
)
insert into @nodes
--get all direct childs
select 
  'nod-' + cast(n.[Id] as nvarchar(255)), 
  n.[Name], 
  coalesce(nc.[Count], 0) as [Count], 
  n.[Order]
from [Ui].[StructureNode] n
left join [NodeCount] nc on n.[Id] = nc.[NodeId]
where n.[ParentNodeId] = @nodeId
order by n.[Order];

--get information about segments which linked to this node
with [SegmentCount] as (
  --calculate count of users in segments
  select nu.[SegmentId], count(DISTINCT nu.[UserId]) as [Count]
  from @nodeUsers nu
  where nu.NodeId = @nodeId
  group by nu.[SegmentId]
)
insert into @nodes
--select information about segments which linked to this node
select 
  'seg-' + cast(s.[Id] as nvarchar(255)), 
  s.[Descr] as [Name],
  coalesce(sc.[Count], 0) as [Count], 
  ns.[Order]
from [Ui].[StructureNodeSegment] ns
inner join [Usr].[Segment] s on ns.[SegmentId] = s.[Id] and s.[IsDeleted] = 0
left join [SegmentCount] sc on sc.[SegmentId] = ns.[SegmentId]
where 
  ns.[NodeId] = @nodeId;

--get information about parents' node
insert into @nodes
select 
  'parents',
  p.[Name],
  (select count([UserId]) from [Usr].[User_GetParents](@userId, default)),
  p.[Order]
from [Ui].[StructureNodeParents] p
where p.[ParentNodeId] = @nodeId;

--result set #2: information about child nodes
select [NodeId], [Name], [Count] from @nodes order by [Order];

--return set #3: information about users is empty result set 
select top 0 * from [Usr].[User];

--return total count of users
select 0 as [TotalCount];

GO



use [FLChat]
go

IF OBJECT_ID (N'Ui.StructureNode_GetInfo_Segment', N'P') IS NOT NULL  
    DROP PROCEDURE [Ui].[StructureNode_GetInfo_Segment];  
GO 

create procedure [Ui].[StructureNode_GetInfo_Segment]
		@segmentId uniqueidentifier,
		@userId uniqueidentifier
as

declare @users [dbo].[GuidList];

--get all childs users from user's structure
insert into @users
select [UserId] from [Usr].[Segment_GetMembers](@segmentId, @userId, default, default);

--result set#1: information about current segment, include count of users
with cnt as (select count(*) as [Count] from @users)
select 
  'seg-' + cast(s.[Id] as nvarchar(255)) as [NodeId],
  s.[Descr] as [Name],  
  cnt.[Count]
from [Usr].[Segment] s
inner join cnt on 1=1
where s.[Id] = @segmentId;

-- if not found then break
if @@ROWCOUNT = 0
  return -1;

--data type for nodes result set
declare @nodes as [Ui].[StructureNodeInfo];

--result set #2: information about child nodes is empty result set 
select [NodeId], [Name], [Count] from @nodes;

--return all users from segment
select * from [Usr].[User] where [Id] in (select * from @users);

go
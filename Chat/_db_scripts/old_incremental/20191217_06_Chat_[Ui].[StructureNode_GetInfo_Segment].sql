USE [FLChat]
GO

/****** Object:  StoredProcedure [Ui].[StructureNode_GetInfo_Segment]    Script Date: 17.12.2019 21:35:13 ******/
DROP PROCEDURE [Ui].[StructureNode_GetInfo_Segment]
GO

/****** Object:  StoredProcedure [Ui].[StructureNode_GetInfo_Segment]    Script Date: 17.12.2019 21:35:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [Ui].[StructureNode_GetInfo_Segment]
		@segmentId uniqueidentifier,
		@userId uniqueidentifier,
		@offset int = null,
		@count int = null
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
if @offset is null and @count is null
	select * from [Usr].[User] where [Id] in (select * from @users) 
	order by 
		case when [FullName] is not null then 1
			 else 0 end desc, 
		case when UNICODE([FullName]) >= 1024 and UNICODE([FullName]) <= 1279 then 1
			 else 0 end desc,
		[FullName];
else
	select * from [Usr].[User] where [Id] in (select * from @users)
	order by 
		case when [FullName] is not null then 1
			 else 0 end desc, 
		case when UNICODE([FullName]) >= 1024 and UNICODE([FullName]) <= 1279 then 1
			 else 0 end desc,
		[FullName]
    OFFSET @offset ROWS FETCH NEXT @count ROWS ONLY;

--return total count of users
select count(DISTINCT [Guid]) as [TotalCount] from @users;
GO



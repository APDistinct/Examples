USE [FLChat]
GO

/****** Object:  StoredProcedure [Ui].[StructureNode_GetInfo_Parents]    Script Date: 22.12.2019 20:22:55 ******/
DROP PROCEDURE [Ui].[StructureNode_GetInfo_Parents]
GO

/****** Object:  StoredProcedure [Ui].[StructureNode_GetInfo_Parents]    Script Date: 22.12.2019 20:22:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [Ui].[StructureNode_GetInfo_Parents]	
		@userId uniqueidentifier
as

declare @users table ([UserId] uniqueidentifier, [Deep] int);

--get all childs users from user's structure
insert into @users
select [UserId], [Deep] from [Usr].[User_GetParents](@userId, default);

--result set#1: information about current segment, include count of users
select 
  'parents',
  (select top 1 [Name] from [Ui].[StructureNodeParents]) as [Name],  
  (select count(*) as [Count] from @users) as [Count];

--data type for nodes result set
declare @nodes as [Ui].[StructureNodeInfo];

--result set #2: information about child nodes is empty result set 
select [NodeId], [Name], [Count], [Final] from @nodes;

--return all users from segment
select u.* from @users p
inner join [Usr].[User] u on p.[UserId] = u.[Id]
order by p.[Deep] desc

--return total count of users
select count(DISTINCT [UserId]) as [TotalCount] from @users;

GO



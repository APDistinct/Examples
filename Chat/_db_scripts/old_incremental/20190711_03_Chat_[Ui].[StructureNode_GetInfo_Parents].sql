USE [FLChat]
GO

/****** Object:  StoredProcedure [Ui].[StructureNode_GetInfo_Segment]    Script Date: 11.07.2019 15:37:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID (N'Ui.StructureNode_GetInfo_Parents', N'P') IS NOT NULL  
    DROP PROCEDURE [Ui].[StructureNode_GetInfo_Parents];  
GO 

create procedure [Ui].[StructureNode_GetInfo_Parents]	
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
select [NodeId], [Name], [Count] from @nodes;

--return all users from segment
select u.* from @users p
inner join [Usr].[User] u on p.[UserId] = u.[Id]
order by p.[Deep] desc

GO



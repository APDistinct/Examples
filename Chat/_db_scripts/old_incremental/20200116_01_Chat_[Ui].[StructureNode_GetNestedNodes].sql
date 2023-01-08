USE [FLChat]
GO

/****** Object:  UserDefinedFunction [Ui].[StructureNode_GetChilds]    Script Date: 16.01.2020 14:57:07 ******/
DROP FUNCTION IF EXISTS [Ui].[StructureNode_GetNestedNodes]
GO

/****** Object:  UserDefinedFunction [Ui].[StructureNode_GetChilds]    Script Date: 16.01.2020 14:57:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create function [Ui].[StructureNode_GetNestedNodes](
   @nodeId uniqueidentifier)
RETURNS @ids TABLE (
  [NodeId] uniqueidentifier NOT NULL UNIQUE,
  [Deep] int NOT NULL
)
as
begin
  declare @deep integer;
  set @deep = 1;

  insert into @ids ([NodeId], [Deep])
  select @nodeId, 0;

  insert into @ids ([NodeId], [Deep])
  select [Id], @deep 
  from [Ui].[StructureNode]   
  where [ParentNodeId] = @nodeId;

  while 1 = 1
  begin
    insert into @ids ([NodeId], [Deep])
	select n.[Id], @deep + 1 
	from [Ui].[StructureNode] n
	inner join @ids i on n.[ParentNodeId] = i.[NodeId] and i.[Deep] = @deep;	
	      	  
	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  RETURN
end;
GO



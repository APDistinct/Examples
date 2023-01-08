use [FLChat]
go

IF OBJECT_ID (N'Ui.StructureNode_GetChilds', N'TF') IS NOT NULL  
    DROP FUNCTION [Ui].[StructureNode_GetChilds];  
GO  

create function [Ui].[StructureNode_GetChilds](
   @nodeId uniqueidentifier)
RETURNS @ids TABLE (
  [NodeId] uniqueidentifier NOT NULL UNIQUE,
  [InheritFromNodeId] uniqueidentifier,
  [Deep] int NOT NULL
)
as
begin
  --declare @ids [Usr].[UserIdDeep];
  declare @deep integer;
  set @deep = 1;

  insert into @ids ([NodeId], [InheritFromNodeId], [Deep])
  select [Id], [Id], @deep 
  from [Ui].[StructureNode]   
  where [ParentNodeId] = @nodeId;

  while 1 = 1
  begin
    insert into @ids ([NodeId], [InheritFromNodeId], [Deep])
	select n.[Id], i.[InheritFromNodeId], @deep + 1 
	from [Ui].[StructureNode] n
	inner join @ids i on n.[ParentNodeId] = i.[NodeId] and i.[Deep] = @deep;	
	      	  
	if @@ROWCOUNT = 0 break;

	set @deep = @deep + 1;
  end

  RETURN
end;
go
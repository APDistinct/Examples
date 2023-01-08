use [FLChat]
go

IF OBJECT_ID (N'Ui.StructureNode_GetInfo', N'P') IS NOT NULL  
    DROP PROCEDURE [Ui].[StructureNode_GetInfo];  
GO 

create procedure [Ui].[StructureNode_GetInfo]
		@sid nvarchar(255),
		@userId uniqueidentifier
as

declare @prefix nvarchar(4);
declare @id uniqueidentifier;

if @sid is not null begin
  set @prefix = SUBSTRING (@sid, 1, 4);
  set @id = cast(SUBSTRING (@sid, 5, 500) as uniqueidentifier);
end else begin
  set @prefix = 'nod-';
  set @id = '00000000-0000-0000-0000-000000000000';
end

if @prefix = 'nod-'
  exec [Ui].[StructureNode_GetInfo_Node] @nodeId = @id, @userId = @userId;
else if @prefix = 'seg-'
  exec [Ui].[StructureNode_GetInfo_Segment] @segmentId = @id, @userId = @userId;

go
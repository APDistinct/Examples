USE [FLChat]
GO
/****** Object:  StoredProcedure [Ui].[StructureNode_GetInfo]    Script Date: 11.07.2019 14:54:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [Ui].[StructureNode_GetInfo]
		@sid nvarchar(255),
		@userId uniqueidentifier
as

declare @prefix nvarchar(4);
declare @id uniqueidentifier;

if @sid = 'parents' 
  exec [Ui].[StructureNode_GetInfo_Parents] @userId = @userId;
else
begin
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
end

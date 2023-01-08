use [FLChat]
go

alter table [Ui].[StructureNode]
add [Order] smallint NOT NULL default 0
go

alter table [Ui].[StructureNodeSegment]
add [Order] smallint NOT NULL default 0
go

alter table [Ui].[StructureNodeSegment]
add [IsBelongToNode] bit NOT NULL default 1
go

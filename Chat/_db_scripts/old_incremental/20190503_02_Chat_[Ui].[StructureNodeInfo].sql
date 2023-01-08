use [FLChat]
go

create type [Ui].[StructureNodeInfo] as table (
  [NodeId] nvarchar(255) NOT NULL,
  [Name] nvarchar(255) NOT NULL,
  [Count] int  NOT NULL,
  [Order] smallint  NOT NULL);
go
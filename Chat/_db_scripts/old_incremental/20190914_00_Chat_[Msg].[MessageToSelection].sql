use [FLChat]
go

create table [Msg].[MessageToSelection] (
  [Idx] bigint NOT NULL IDENTITY(1,1),
  [MsgId] uniqueidentifier NOT NULL,
  [UserId] uniqueidentifier NOT NULL,
  [WithStructure] bit NOT NULL,
  [Include] bit NOT NULL,
  [StructureDeep] int,
  constraint [PK__MsgMessageToSelection_Idx] primary key clustered ([Idx]),
  constraint [FK__MsgMessageToSelection__MsgMessage] 
    foreign key ([MsgId])
	references [Msg].[Message] ([Id])
	on delete cascade,
  constraint [FK__MsgMessageToSelection__UsrUser]
    foreign key ([UserId])
	references [Usr].[User]([Id])
	on delete cascade
)
go
use [FLChat]
go

create table [Usr].[Segment] (
  [Id] uniqueidentifier NOT NULL default newsequentialid(),
  [IsDeleted] bit NOT NULL default 0,
  [Name] nvarchar(255) NOT NULL,
  constraint [PK__UsrSegment] primary key ([Id]),
)
go

create unique index [UNQ__UsrSegment__Name]
  on [Usr].[Segment] ([Name])
  where [IsDeleted] = 0
go

create table [Usr].[SegmentMember] (
  [SegmentId] uniqueidentifier NOT NULL,
  [UserId] uniqueidentifier NOT NULL,
  constraint [PK__UsrSegmentMember] primary key ([SegmentId], [UserId]),
  constraint [FK__UsrSegmentMember__UsrSegment] foreign key ([SegmentId])
    references [Usr].[Segment]([Id]),
  constraint [FK__UsrSegmentMember__UsrUser] foreign key ([UserId])
    references [Usr].[User]([Id])
)
go
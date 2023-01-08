use [FLChat]
go

create schema [Reg]
go

create table [Reg].[LastUsedTransport] (
  [UserId] uniqueidentifier NOT NULL,
  [TransportTypeId] int NOT NULL,
  constraint [PK__RegLastTransport] primary key([UserId]),
  constraint [FK__RegLastTransport__UsrUser] 
    foreign key ([UserId])
	references [Usr].[User] ([Id])
	on delete cascade,
  constraint [FK__RegLastTransport__CfgTransportType] 
    foreign key ([TransportTypeId])
	references [Cfg].[TransportType] ([Id])
)
go
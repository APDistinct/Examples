use [FLChat]
go

alter table [Msg].[Message]
add [FileId] uniqueidentifier NULL
go

alter table [Msg].[Message]
add constraint [FK__MsgMesage__FileInfoFile]
  foreign key ([FileId])
  references [File].[FileInfo] ([Id])
  on delete set null
go
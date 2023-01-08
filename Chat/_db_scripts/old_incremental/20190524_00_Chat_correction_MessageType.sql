use [FLChat]
go

update [Msg].[Message] set [MessageTypeId] = 2 where [MessageTypeId] = 3
go

delete from [Cfg].[MessageType] where [Id] = 3
go
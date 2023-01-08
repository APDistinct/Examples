use [FLChat]
go

alter table [Cfg].[MessageType]
add [LimitForDay] int NULL
go

alter table [Cfg].[MessageType]
add [LimitForOnce] int NULL
go

update [Cfg].[MessageType]
set [LimitForDay] = 1000, [LimitForOnce] = 1000
where [Id] = /**Broadcast**/2
go

update [Cfg].[MessageType]
set [LimitForDay] = 5000, [LimitForOnce] = 10000
where [Id] = /**Mailing**/4
go
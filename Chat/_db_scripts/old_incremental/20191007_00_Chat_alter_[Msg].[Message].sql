use [FLChat]
go

alter table [Msg].[Message]
add [NeedToChangeText] bit default 0 NOT NULL
go


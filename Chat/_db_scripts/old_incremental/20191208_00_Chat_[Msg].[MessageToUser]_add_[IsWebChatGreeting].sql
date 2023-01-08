use [FLChat]
go

alter table [Msg].[MessageToUser]
add [IsWebChatGreeting] bit NOT NULL default 0
go
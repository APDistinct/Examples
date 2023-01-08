use [FLChat]
go

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[Message]')
	and [name] = 'DelayedStart')
alter table [Msg].[Message]
add [DelayedStart] datetime NULL
go

if not exists(select 1 from sys.columns where Object_ID = Object_ID(N'[Msg].[Message]')
	and [name] = 'DalayedCancelled')
alter table [Msg].[Message]
add [DalayedCancelled] datetime NULL
go

if not exists(select 1 from sysobjects where xtype = 'C'
	and [name] = 'CHK_MsgMessage_DelayedCancelled' and [uid]=SCHEMA_ID('Msg'))
alter table [Msg].[Message]
add constraint [CHK_MsgMessage_DelayedCancelled] 
	check (([DalayedCancelled] is not null and [DelayedStart] is not null)
		 or [DalayedCancelled] is null)
go

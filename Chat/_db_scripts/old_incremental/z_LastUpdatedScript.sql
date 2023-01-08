use [FLChat]
go

delete from [dbo].[LastUpdatedScript];

insert into [dbo].[LastUpdatedScript] ([ScriptName]) 
values (N'20200128_00_alter_[Msg].[Message]_Start.sql');
go
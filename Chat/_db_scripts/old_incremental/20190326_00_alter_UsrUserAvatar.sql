use [FLChat]
go

alter table [Usr].[UserAvatar]
alter column [Data] varbinary(max) NOT NULL
go
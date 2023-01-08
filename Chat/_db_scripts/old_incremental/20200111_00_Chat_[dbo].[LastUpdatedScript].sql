use [FLChat]
go

create table [dbo].[LastUpdatedScript] (
  [ScriptName] nvarchar(500) NOT NULL,
  [ExecutedDate] datetime NOT NULL default GETUTCDATE()
)
go
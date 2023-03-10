USE [FLChat]
GO

declare @name nvarchar(100)
select @name = [name] from sys.objects 
where type = 'UQ' 
  and parent_object_id = object_id('[Cfg].[MediaType]')
  and [name] like '%UQ__MediaTyp%'

if (@name is not null)
  begin
     exec ('alter table [Cfg].[MediaType] drop constraint [' + @name +']')
  end

ALTER TABLE [Cfg].[MediaType]
alter column [Name] varchar(500) NOT NULL
go

ALTER TABLE [Cfg].[MediaType] 
ADD constraint [UNQ__CfgMediaType__Name] UNIQUE NONCLUSTERED ([Name] ASC)
go


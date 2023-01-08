use FLChat
go

/****** Table [Usr].[MatchedPhones] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MatchedPhones' and [uid] = SCHEMA_ID('Usr'))
create table [Usr].[MatchedPhones] (
  [UserId] uniqueidentifier NOT NULL,
  [AddrUserId] uniqueidentifier NOT NULL,
  constraint [PK__UsrMatchedPhones] primary key ([UserId], [AddrUserId]),
  constraint [FK__UsrMatchedPhones__UsrUser] foreign key ([UserId]) references [Usr].[User] ([Id])
	on delete cascade,
  constraint [FK__UsrMatchedPhones__AddrUserId] foreign key ([AddrUserId]) references [Usr].[User] ([Id])	
)
go




if TYPE_ID('[dbo].[StringList]') is null
create type [dbo].[StringList] as table (
  [String] nvarchar(max) NOT NULL
)
go
  

create or alter function [Usr].[User_GetChildsWithPhone](
   @usersPhone [dbo].[StringList] readonly,
   @userId uniqueidentifier)
RETURNS @ids TABLE (  [UserId] uniqueidentifier NOT NULL )
as
begin

 insert into @ids ([UserId])

 select [UserId] from [Usr].[User_GetChildsSimple](@userId, default, default) ugc
  inner join [Usr].[User] u on (ugc.[UserId] = u.[Id])
  inner join @usersPhone p on (p.[String] = u.[Phone])

  RETURN
end;
GO


/****** Table [Usr].[PersonalProhibition] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='PersonalProhibition' and [uid] = SCHEMA_ID('Usr'))
create table [Usr].[PersonalProhibition] (
  [UserId] uniqueidentifier NOT NULL,
  [ProhibitionUserId] uniqueidentifier NOT NULL,
  constraint [PK__UsrPersonalProhibition] primary key ([UserId], [ProhibitionUserId]),
  constraint [FK__UsrPersonalProhibition__UserId]
    foreign key ([UserId])
	references [Usr].[User] ([Id])
	on delete cascade,
  constraint [FK__UsrPersonalProhibition__ProhibitionUserId]
    foreign key ([ProhibitionUserId])
	references [Usr].[User] ([Id])
	--on delete cascade
)
go


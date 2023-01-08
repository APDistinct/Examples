use [FLChat]
go

create procedure [Test].[GetUsers]
	@count integer
as
begin

  SET NOCOUNT ON;

  declare @users [dbo].[GuidList];
  
  insert into @users 
  select Top (@count) [Id] from [Usr].[User] where [Enabled] = 1;

  if @@ROWCOUNT < @count
  begin
    declare @realcnt integer;
    set @realcnt = (select count(*) from @users);
    while @realcnt < @count
	begin
	  insert into @users
	  select * from (
	    insert into [Usr].[User] ([FullName]) 
	    output inserted.[Id]
	    values ('Created by [Test].[GetUsers]')
	  ) as t;	  
	end
  end

  select * from @users;
end;
go
declare @users as table (
  [UserId] uniqueidentifier NOT NULL,
  [ChildsCount] int NOT NULL,
  [TimeMs] int NOT NULL
);

declare @id uniqueidentifier;
declare @cnt int;
declare @tm datetime;

DECLARE icc CURSOR FORWARD_ONLY FAST_FORWARD
FOR
  select [Id] from [Usr].[User] where [LastGetEvents] is not null
open icc;
fetch next from icc into @id;

while @@FETCH_STATUS = 0
begin

  set @tm = GETDATE();
  set @cnt = (select count(*) from [Usr].[User_GetChilds](@id, default, default));

  insert into @users values (@id, @cnt, DATEDIFF(ms, @tm, GETDATE()));

  fetch next from icc into @id;
end;

close icc;
deallocate icc;


select * from @users
order by [ChildsCount] desc
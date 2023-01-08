use [FLChat]
go

drop function if exists [Usr].[BroadcastProhibition_Structure]
go

create function [Usr].[BroadcastProhibition_Structure] 
(
  @userId uniqueidentifier
)
returns @ids TABLE (
  [UserId] uniqueidentifier NOT NULL UNIQUE
) as
begin

  declare @bh [dbo].[GuidList];
  insert into @bh
  select [ProhibitionUserId] 
  from [Usr].[BroadcastProhibition]
  where [UserId] = @userId;

  insert into @ids ([UserId])
  select DISTINCT [UserId] 
  from [Usr].[User_GetChildsMulti](@bh, default, default);

  return
end
go
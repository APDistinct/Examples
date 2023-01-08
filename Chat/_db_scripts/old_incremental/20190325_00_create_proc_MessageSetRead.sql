use [FLChat]
go

create type [dbo].[GuidList] as table (
  [Guid] uniqueidentifier NOT NULL
)
go

create procedure [Msg].[MessagesSetDelivered]
	@userId uniqueidentifier,
	@ids [dbo].[GuidList] readonly
as
begin
  update [Msg].[MessageToUser] 
  set [IsDelivered] = 1 
  where [ToUserId] = @userId and [MsgId] in (select * from @ids);
end
go

create procedure [Msg].[MessagesSetRead]
	@userId uniqueidentifier,
	@ids [dbo].[GuidList] readonly
as
begin
  update [Msg].[MessageToUser] 
  set [IsRead] = 1 
  where [ToUserId] = @userId and [MsgId] in (select * from @ids);
end
go
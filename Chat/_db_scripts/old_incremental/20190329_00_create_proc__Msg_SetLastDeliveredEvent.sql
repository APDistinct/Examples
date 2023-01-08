use [FLChat]
go

create procedure [Msg].[SetLastDeliveredEvent]
	@userId uniqueidentifier,
	@eventId bigint
as
begin
	update [Msg].[EventDelivered] 
	set [LastEventId] = @eventId 
	where [UserId] = @userId;

	if @@ROWCOUNT = 0
	begin
		insert into [Msg].[EventDelivered] ([UserId], [LastEventId])
		values (@userId, @eventId);
	end
end
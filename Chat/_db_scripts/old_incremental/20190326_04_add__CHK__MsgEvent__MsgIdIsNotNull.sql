use [FLChat]
go

alter table [Msg].[Event]
  add constraint [CHK__MsgEvent__MsgIdIsNotNull] 
  check ( ([MsgId] is not null and [EventTypeId] >= 1 and [EventTypeId] < 20)
       or ([MsgId] is null     and [EventTypeId] >= 20)
	   or ([EventTypeId] = 0) )
go
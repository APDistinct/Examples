use [FLChat]
GO

alter table [Msg].[MessageToUser]
add constraint [CHK_MessageToUser_FLChat]
  check (([ToTransportTypeId] = 0 and [IsFailed] = 0 and [IsSent] = 1) or [ToTransportTypeId] <> 0)
go
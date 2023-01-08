use [FLChat]
go

drop view if exists [Msg].[MessageStatsGroupedView]
go

create view [Msg].[MessageStatsGroupedView]
as
select 
  [MsgId], --message id
  [MsgIdx],
  [MessageTypeId], --message type id
  [FromUserId], --message sender
  count (DISTINCT [ToUserId]) as [RecipientCount] --total count of recipients
  , sum([IsWebChat]) as [WebChatCount] --total messages to WebChat
  , sum([IsFailed]) as [FailedCount] --total failed messages
  , sum([IsSent]) as [SentCount] --total sent messages
  , sum([IsQuequed]) as [QuequedCount] --total quequed messages
  , sum([CantSendToWebChat]) as [CantSendToWebChatCount] --total messages can't be sent to webchat
  , sum([IsWebChatAccepted]) as [WebChatAcceptedCount] --total accepted web chat messages
  , sum([IsSmsUrlOpened]) as [SmsUrlOpenedCount] --total recipients who was opened sms url
from [Msg].[MessageStatsRowsView]
group by [MsgId], [MsgIdx], [MessageTypeId], [FromUserId]


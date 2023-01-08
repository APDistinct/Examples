use [FLChat]
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

drop trigger if exists [Msg].[MessageToUser_OnInsert_WebChat]
go

CREATE TRIGGER [Msg].[MessageToUser_OnInsert_WebChat]
   ON  [Msg].[MessageToUser]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @msgId uniqueidentifier;
    declare @toId uniqueidentifier;	
	declare @link nvarchar(20);
	declare @msgIdTbl table (ID uniqueidentifier);
	declare @sender nvarchar(255);	
	declare @text nvarchar(max);
	declare @sms bit;

	DECLARE icc CURSOR FORWARD_ONLY FAST_FORWARD
    FOR
      select 
	    i.[MsgId], i.[ToUserId], u.[FullName],
		case when sms.[UserId] is not null then 1 else 0 end
	  from inserted i	
	  inner join [Msg].[Message] m on m.[Id] = i.[MsgId]
	  inner join [Usr].[User] u on u.[Id] = m.[FromUserId]
	  left  join [Usr].[Transport] sms on sms.[UserId] = i.[ToUserId] 
	                                  and sms.[TransportTypeId] = /**SMS**/150
	  where i.[ToTransportTypeId] = /**WebChat**/100        
    open icc;
    fetch next from icc into @msgId, @toId, @sender, @sms;

	while @@FETCH_STATUS = 0
	begin
	  
	  set @link = (select [dbo].[RandomString](20));

	  insert into [Msg].[WebChatDeepLink] 
	    ([MsgId], [ToUserId], [ToTransportTypeId], [Link], [ExpireDate])
	  values
	    (@msgId, @toId, /**WebChat**/100 , @link, DATEADD(month, 1, GETUTCDATE()));  

	  if @sms = 1 
	  begin
	    set @text = (select [Msg].[WebChat_GetSmsText](@sender, @link));

	    insert into [Msg].[Message] 
	      ([MessageTypeId], [FromUserId], [FromTransportTypeId], [Text], [Specific], [ForwardMsgId])
	    output inserted.[Id] into @msgIdTbl
	    values 
	      (/**Personal**/0, '00000000-0000-0000-0000-000000000000', /**FLChat**/0, @text, 'WEBCHAT', @msgId);	  

	    insert into [Msg].[MessageToUser] 	    
	      ([MsgId], [ToUserId], [ToTransportTypeId])
	    values 
	      ((select top 1 ID from @msgIdTbl), @toId, /**SMS**/150);
      end;

	  fetch next from icc into @msgId, @toId, @sender, @sms;
	end
	
	close icc;
	deallocate icc;
END
GO

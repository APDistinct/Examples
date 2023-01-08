USE [FLChat]
GO

/****** Object:  Trigger [MessageToUser_OnInsert_WebChat]    Script Date: 10.01.2020 14:07:13 ******/
DROP TRIGGER [Msg].[MessageToUser_OnInsert_WebChat]
GO

/****** Object:  Trigger [Msg].[MessageToUser_OnInsert_WebChat]    Script Date: 10.01.2020 14:07:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



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

	declare @text_t nvarchar(max);
    declare @url_t nvarchar(100);
    set @text_t = (select [Value] from [Cfg].[Settings] where [Name] = 'WEB_CHAT_SMS');
    set @url_t = (select [Value] from [Cfg].[Settings] where [Name] = 'WEB_CHAT_DEEP_URL');

	DECLARE icc CURSOR FORWARD_ONLY FAST_FORWARD
    FOR
      select 
	    i.[MsgId], i.[ToUserId]
	  from inserted i	
	  inner join [Msg].[Message] m on m.[Id] = i.[MsgId]

	  where i.[ToTransportTypeId] = /**WebChat**/100        
    open icc;
    fetch next from icc into @msgId, @toId;

	while @@FETCH_STATUS = 0
	begin
	  
	  set @link = (select [dbo].[RandomString](20));

	  insert into [Msg].[WebChatDeepLink] 
	    ([MsgId], [ToUserId], [ToTransportTypeId], [Link], [ExpireDate])
	  values
	    (@msgId, @toId, /**WebChat**/100 , @link, DATEADD(month, 1, GETUTCDATE()));  


	  fetch next from icc into @msgId, @toId;
	end
	
	close icc;
	deallocate icc;
END
GO

ALTER TABLE [Msg].[MessageToUser] ENABLE TRIGGER [MessageToUser_OnInsert_WebChat]
GO



USE [FLChat]
GO
/****** Object:  Trigger [Msg].[Message_OnInsert_LastUsedTransport]    Script Date: 18.08.2019 17:16:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER TRIGGER [Msg].[Message_OnInsert_LastUsedTransport]
   ON  [Msg].[Message]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @ids [dbo].[GuidList];

    -- Insert statements for trigger here
    update [Reg].[LastUsedTransport]
	set [TransportTypeId] = t.[TransportTypeId] 
	output inserted.[UserId] into @ids ([Guid])
	from 
	(
	  select DISTINCT
	    i.[FromUserId],
		LAST_VALUE(i.[FromTransportTypeId]) OVER (order by [Idx]) as [TransportTypeId] 
	  from inserted i
	inner join [Cfg].[MessageType] mt on i.[MessageTypeId] = mt.[Id]
	where mt.[ShowInHistory] = 1
	) t
	where [UserId] = t.[FromUserId]	  

	insert [Reg].[LastUsedTransport] ([UserId], [TransportTypeId])
	select DISTINCT
	    i.[FromUserId],
		LAST_VALUE(i.[FromTransportTypeId]) OVER (order by [Idx]) as [TransportTypeId] 
	from inserted i
	inner join [Cfg].[MessageType] mt on i.[MessageTypeId] = mt.[Id]
	where i.[FromUserId] not in (select [Guid] from @ids)
	  and mt.[ShowInHistory] = 1;
	
END

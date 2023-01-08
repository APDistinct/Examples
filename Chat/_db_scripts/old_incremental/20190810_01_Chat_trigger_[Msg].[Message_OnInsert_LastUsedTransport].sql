use [FLChat]
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [Msg].[Message_OnInsert_LastUsedTransport]
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
	) t
	where [UserId] = t.[FromUserId];

	insert [Reg].[LastUsedTransport] ([UserId], [TransportTypeId])
	select DISTINCT
	    i.[FromUserId],
		LAST_VALUE(i.[FromTransportTypeId]) OVER (order by [Idx]) as [TransportTypeId] 
	from inserted i
	where i.[FromUserId] not in (select [Guid] from @ids);
	
END
GO

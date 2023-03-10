USE [FLChat]
GO
/****** Object:  Trigger [Usr].[User_ManageEmailTransport]    Script Date: 08.10.2019 20:25:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [Usr].[User_ManageEmailTransport]
   ON  [Usr].[User]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- insert Email transport for users without Email transport
	insert into [Usr].[Transport] ([UserId], [TransportTypeId])
	select i.[Id], /**Email**/151
	from inserted i
	left join deleted d on i.[Id] = d.[Id]
	left join [Usr].[Transport] t on i.[Id] = t.[UserId] and t.[TransportTypeId] = /**Email**/151
	where i.[Email] is not null 
	  and d.[Email] is null
	  and t.[UserId] is null;

	-- enabled Email transport, if user already has disabled transport
    update [Usr].[Transport] set [Enabled] = 1
	where [UserId] in (
	  select i.[Id]
	  from inserted i
	  inner join [Usr].[Transport] t on i.[Id] = t.[UserId] and t.[TransportTypeId] = /**Email**/151
	  left join deleted d on i.[Id] = d.[Id]
	where i.[Email] is not null 
	  and d.[Email] is null
	  and t.[Enabled] = 0
	) and [TransportTypeId] = /**Email**/151;

	--disable Email transport if user set Email = null
	update [Usr].[Transport] set [Enabled] = 0
	where [UserId] in (
	  select i.[Id]
	  from inserted i
	  inner join deleted d on i.[Id] = d.[Id]
	  inner join [Usr].[Transport] t on i.[Id] = t.[UserId] and t.[TransportTypeId] = /**Email**/151
	where i.[Email] is null 
	  and d.[Email] is not null
	  and t.[Enabled] = 1
	) and [TransportTypeId] = /**Email**/151;
END

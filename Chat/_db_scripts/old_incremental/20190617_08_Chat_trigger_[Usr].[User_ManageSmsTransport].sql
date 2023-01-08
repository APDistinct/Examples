use [FLChat]
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID (N'Usr.User_ManageSmsTransport', N'TR') IS NOT NULL  
    DROP TRIGGER [Usr].[User_ManageSmsTransport];  
GO

CREATE TRIGGER [Usr].[User_ManageSmsTransport]
   ON  [Usr].[User]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- insert sms transport for users without sms transport
	insert into [Usr].[Transport] ([UserId], [TransportTypeId])
	select i.[Id], /**Sms**/150
	from inserted i
	left join deleted d on i.[Id] = d.[Id]
	left join [Usr].[Transport] t on i.[Id] = t.[UserId] and t.[TransportTypeId] = /**Sms**/150
	where i.[Phone] is not null 
	  and d.[Phone] is null
	  and t.[UserId] is null;

	-- enabled sms transport, if user already has disabled transport
    update [Usr].[Transport] set [Enabled] = 1
	where [UserId] in (
	  select i.[Id]
	  from inserted i
	  inner join [Usr].[Transport] t on i.[Id] = t.[UserId] and t.[TransportTypeId] = /**Sms**/150
	  left join deleted d on i.[Id] = d.[Id]
	where i.[Phone] is not null 
	  and d.[Phone] is null
	  and t.[Enabled] = 0
	);

	--disable sms transport if user set phone = null
	update [Usr].[Transport] set [Enabled] = 0
	where [UserId] in (
	  select i.[Id]
	  from inserted i
	  inner join deleted d on i.[Id] = d.[Id]
	  inner join [Usr].[Transport] t on i.[Id] = t.[UserId] and t.[TransportTypeId] = /**Sms**/150
	where i.[Phone] is null 
	  and d.[Phone] is not null
	  and t.[Enabled] = 1
	);
END
GO

use [FLChat]
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [Usr].[User_OnInsert_AddWebChatTransport]
   ON  [Usr].[User]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	insert into [Usr].[Transport] ([UserId], [TransportTypeId])
	select i.[Id], /**WebChat**/100
	from inserted i;
END
GO

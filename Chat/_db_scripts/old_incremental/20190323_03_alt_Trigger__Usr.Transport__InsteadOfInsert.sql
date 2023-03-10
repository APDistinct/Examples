USE [FLChat]
GO
/****** Object:  Trigger [Usr].[Transport__InsteadOfInsert]    Script Date: 23.03.2019 16:55:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER trigger [Usr].[Transport__InsteadOfInsert]
on [Usr].[Transport]
instead of insert
as
  -- set OuterTransportId for inner transport
  insert into [Usr].[Transport] ([UserId], [TransportTypeId], [TransportOuterId], [Enabled])
  select 
    i.[UserId],
	i.[TransportTypeId],
	case when i.[TransportTypeId] = /**FLChat**/0 then cast(i.[UserId] as nvarchar(255))
	     else i.[TransportOuterId]
    end,
	i.[Enabled]
  from inserted i

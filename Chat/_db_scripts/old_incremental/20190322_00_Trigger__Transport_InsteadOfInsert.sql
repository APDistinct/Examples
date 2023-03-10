USE [FLChat]
GO
/****** Object:  Trigger [Usr].[Transport__OnInsertFLChat]    Script Date: 22.03.2019 12:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

drop trigger [Usr].[Transport__OnInsertFLChat]
GO

create trigger [Usr].[Transport_InsteadOfInsert]
on [Usr].[Transport]
instead of insert
as
  -- set OuterTransportId for inner transport
  insert into [Usr].[Transport] ([UserId], [TransportTypeId], [TransportOuterId])
  select 
    i.[UserId],
	i.[TransportTypeId],
	case when i.[TransportTypeId] = /**FLChat**/0 then cast(i.[UserId] as nvarchar(255))
	     else i.[TransportOuterId]
    end
  from inserted i

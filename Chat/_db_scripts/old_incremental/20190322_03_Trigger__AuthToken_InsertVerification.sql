USE [FLChat]
GO
/****** Object:  Trigger [Auth].[AuthTokenInsertVerification]    Script Date: 22.03.2019 14:52:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create trigger [Auth].[AuthToken_InsertVerification]
on [Auth].[AuthToken]
for insert
as
  if exists(
    select 1 
	from inserted i
	inner join [Usr].[User] u on i.[UserId] = u.[Id]
	where u.[Enabled] = 0)
  begin
    THROW 51000, 'Can not insert auth token for disabled user', 1;
    --RAISERROR('Can not insert auth token for disabled user', 10, 10)	

	--ROLLBACK TRANSACTION
	--RETURN
  end

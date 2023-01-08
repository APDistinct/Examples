use [FLChat]
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

drop procedure if exists [Usr].[Update_OwnerUserId_By_ParentFLUserNumber]
go

CREATE PROCEDURE [Usr].[Update_OwnerUserId_By_ParentFLUserNumber]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @cnt int;

    with [ForUpd] as (
      select 
        u.[Id], 
        own.[Id] as [OwnerId]
     from [Usr].[User] u 
     inner join [Usr].[User] own on u.[ParentFLUserNumber] = own.[FLUserNumber]
     where u.[FLUserNumber] is not null 
      and (u.[OwnerUserId] is null or u.[OwnerUserId] <> own.Id)
    )
    update u
    set u.[OwnerUserId] = fu.[OwnerId]
    from [Usr].[User] as u
    inner join [ForUpd] as fu on u.[Id] = fu.[Id];

	set @cnt = @@ROWCOUNT;

    update [Usr].[User]
    set [OwnerUserId] = null
    where [FLUserNumber] is not null 
	   and [ParentFLUserNumber] is null
       and [OwnerUserId] is not null;

	set @cnt = @cnt + @@ROWCOUNT;

	select @cnt
END
GO

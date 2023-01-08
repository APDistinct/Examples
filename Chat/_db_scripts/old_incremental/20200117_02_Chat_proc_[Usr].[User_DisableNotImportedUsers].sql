Use [FLChat]
go

drop procedure if exists [Usr].[User_DisableNotImportedUsers]
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Usr].[User_DisableNotImportedUsers]
	-- Add the parameters for the stored procedure here
	@userId uniqueidentifier,
	@userNumber int,
	@passedHours int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if @userNumber is not null 
	begin
	  set @userId = (select [Id] from [Usr].[User] where [FLUserNumber] = @userNumber);
	  if @@ROWCOUNT = 0
      begin
	    select -1 as [Absence];
		return;
	  end
	end

    -- Insert statements for procedure here
	update u
	set [Enabled] = 0
	from [Usr].[User] u
	inner join [Usr].[User_GetChilds](@userId, default, default) c on u.[Id] = c.[UserId]
	where (u.[LastImportDate] is null or DATEDIFF(hour, u.[LastImportDate], GETUTCDATE()) > @passedHours)
	  and u.[Enabled] = 1
	  and u.[FLUserNumber] is not null;

	select @@ROWCOUNT as [Absence];
END
GO

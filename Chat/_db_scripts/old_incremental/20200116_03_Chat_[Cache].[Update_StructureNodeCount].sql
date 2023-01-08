use [FLChat]
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
CREATE PROCEDURE [Cache].[Update_StructureNodeCount]
	-- Add the parameters for the stored procedure here
	@userId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @result TABLE (
		[NodeId] uniqueidentifier NOT NULL UNIQUE,
		[Count] integer NOT NULL
	);

	insert into @result
	select [NodeId], [Count] from [Usr].[User_UiNodesCount](@userId); 

	delete from [Cache].[StructureNodeCount] where [UserId] = @userId;
	insert into [Cache].[StructureNodeCount]
	select [NodeId], @userId, [Count] from @result;
END
GO

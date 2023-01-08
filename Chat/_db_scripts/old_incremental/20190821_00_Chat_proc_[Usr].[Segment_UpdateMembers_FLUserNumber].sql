use [FLChat]
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Create the data type
CREATE TYPE [dbo].[IntList] AS TABLE 
(
    [Value] int NOT NULL	    
)
GO

CREATE PROCEDURE [Usr].[Segment_UpdateMembers_FLUserNumber]
	-- Add the parameters for the stored procedure here
	@segmentId uniqueidentifier, 
	@FLUserNumberList [dbo].[IntList] readonly
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    declare @guidList [dbo].[GuidList];
	insert into @guidList
	select u.[Id]
	from [Usr].[User] u
	inner join @FLUserNumberList l on u.[FLUserNumber] = l.[Value];
	
	exec [Usr].[Segment_UpdateMembers] @segmentId = @segmentId, @newMembersIds = @guidList;	
END
GO

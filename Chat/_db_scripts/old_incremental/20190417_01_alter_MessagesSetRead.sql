USE [FLChat]
GO
/****** Object:  StoredProcedure [Msg].[MessagesSetRead]    Script Date: 17.04.2019 20:43:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [Msg].[MessagesSetRead]
	@userId uniqueidentifier,
	@ids [dbo].[GuidList] readonly,
	@transportTypeId integer
as
begin
  update [Msg].[MessageToUser] 
  set [IsRead] = 1 
  where [ToUserId] = @userId 
    and [ToTransportTypeId] = @transportTypeId
    and [MsgId] in (select * from @ids);
end

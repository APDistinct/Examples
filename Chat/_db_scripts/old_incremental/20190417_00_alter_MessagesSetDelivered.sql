USE [FLChat]
GO
/****** Object:  StoredProcedure [Msg].[MessagesSetDelivered]    Script Date: 17.04.2019 20:42:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [Msg].[MessagesSetDelivered]
	@userId uniqueidentifier,
	@ids [dbo].[GuidList] readonly,
	@transportTypeId integer
as
begin
  update [Msg].[MessageToUser] 
  set [IsDelivered] = 1 
  where [ToUserId] = @userId 
    and [ToTransportTypeId] = @transportTypeId
    and [MsgId] in (select * from @ids);
end

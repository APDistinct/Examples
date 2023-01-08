use [FLChat]
go

drop procedure if exists [Test].[SendMessage]
go

create procedure [Test].[SendMessage]
   @from uniqueidentifier,
   @fromTrans integer,
   @to uniqueidentifier,
   @toTrans integer,
   @text nvarchar = 'Test message'

as
begin
  SET NOCOUNT ON;

  declare @msg uniqueidentifier;
  set @msg = NEWID();

  insert into [Msg].[Message] ([Id], [MessageTypeId], [FromUserId], [FromTransportTypeId], [Text])
  values (@msg, 0, @from, @fromTrans, @text);

  insert into [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId], [IsSent])
  values (@msg, @to, @toTrans, case when @toTrans = 0 then 1 else 0 end);

  select m.[Id], m.[Idx], mtu.[Idx]
  from [Msg].[Message] m
  inner join [Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId]
  where m.[Id] = @msg;

end
go
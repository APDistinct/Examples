use [FLChat]
go

--select * from [Usr].[Segment]
select * from [Usr].[SegmentMember] where [SegmentId] = '656722F6-CBC4-E911-A2C0-9F888BB5FDE6'
insert into [Usr].[SegmentMember] ([SegmentId], [UserId])
values 
--('656722F6-CBC4-E911-A2C0-9F888BB5FDE6', '4a9949ac-12c8-e911-a2c0-9f888bb5fde6'),
('656722F6-CBC4-E911-A2C0-9F888BB5FDE6', '0E35FD72-F280-E911-A2C0-9F888BB5FDE6');

update [Usr].[Transport] set [Enabled] = 0
where [UserId] in (select [UserId] from [Usr].[SegmentMember] where [SegmentId] = '656722F6-CBC4-E911-A2C0-9F888BB5FDE6')
  and [TransportTypeId] < 100;

select * from [Usr].[Transport]
where [UserId] in (select [UserId] from [Usr].[SegmentMember] where [SegmentId] = '656722F6-CBC4-E911-A2C0-9F888BB5FDE6')
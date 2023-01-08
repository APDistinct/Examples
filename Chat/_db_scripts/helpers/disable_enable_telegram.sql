select * from [Usr].[Transport]
where [UserId] in ('41C8775B-F380-E911-A2C0-9F888BB5FDE6', 'fd90cdfd-59a2-e911-a2c0-9f888bb5fde6');

update [Usr].[Transport] set [Enabled] = 0
where [UserId] = 'fd90cdfd-59a2-e911-a2c0-9f888bb5fde6' and [TransportTypeId] = 1;

update [Usr].[Transport] set [Enabled] = 1
where [UserId] = '41C8775B-F380-E911-A2C0-9F888BB5FDE6' and [TransportTypeId] = 1;

select * from [Usr].[Transport]
where [UserId] in ('41C8775B-F380-E911-A2C0-9F888BB5FDE6', 'fd90cdfd-59a2-e911-a2c0-9f888bb5fde6');
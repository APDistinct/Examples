use [FLChat]
go

declare @uid uniqueidentifier;
set @uid = '765E9DE8-C25A-E911-A2BF-CE1FDA20D24A';

select * from [Usr].[User] where [Id] = @uid;

--update [Usr].[Transport]
--set [Enabled] = 0, [TransportOuterId] = N''
--where [UserId] = @uid and [TransportTypeId] in (1, 3, 4);

select * from  [Usr].[Transport] where [UserId] = @uid;

select dt.*, tt.[Name] from [Usr].[UserDefaultTransportView] dt
inner join [Cfg].[TransportType] tt on dt.[DefaultTransportTypeId] = tt.[Id]
where [UserId] = @uid 
go
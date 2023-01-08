use [FLChat]
GO

alter table [Usr].[Transport]
drop [UNQ__UsrTransport]
GO

alter table [Usr].[Transport]
alter column [TransportOuterId] nvarchar(255) NULL
GO

update [Usr].[Transport] 
set [TransportOuterId] = null 
where [TransportTypeId] = /**FLCHat**/0
GO

CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrTransport_OuterId] 
ON [Usr].[Transport] ([TransportTypeId], [TransportOuterId])
  where ([TransportTypeId] <> 0 AND [Enabled] = 1)
GO

alter table [Usr].[Transport] 
add constraint [CHK_UsrTransport_OuterId]
  check (([TransportTypeId] = 0  AND [TransportOuterId] IS NULL)
      or ([TransportTypeId] <> 0 AND [TransportOuterId] IS NOT NULL))
GO

drop trigger if exists [Usr].[Transport__InsteadOfInsert]
GO
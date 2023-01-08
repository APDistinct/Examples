use [FLChat]
go

update [Usr].[Transport] set [Enabled] = 1 
where [Enabled] = 0 and [TransportTypeId] = /**Web-chat**/100;

update [Usr].[Transport] set [Enabled] = 1 
where [Enabled] = 0 and [TransportTypeId] = /**Sms**/150
  and [UserId] in (select [Id] from [Usr].[User] where [Phone] is not null);

update [Usr].[Transport] set [Enabled] = 1 
where [Enabled] = 0 and [TransportTypeId] = /**Email**/151
  and [UserId] in (select [Id] from [Usr].[User] where [Email] is not null);
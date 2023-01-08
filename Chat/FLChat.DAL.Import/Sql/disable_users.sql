update [Usr].[User] 
set [Enabled] = 0
where [DontDisableOnUpdateStructure] = 0
  and [Enabled] = 1
  and [Id] in (select [UserId] from [Usr].[User_GetChilds](@userId, default, default))
  and [Id] not in (select * from @updated)

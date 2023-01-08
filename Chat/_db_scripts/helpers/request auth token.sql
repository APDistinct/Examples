use [FLChat]
go

select t.*, u.[FullName], u.[FLUserNumber]
from [Auth].[AuthToken] t
inner join [Usr].[User] u on t.[UserId] = u.[Id]
where u.[FullName] like N'%Венедиктов%'
order by t.[Id] desc

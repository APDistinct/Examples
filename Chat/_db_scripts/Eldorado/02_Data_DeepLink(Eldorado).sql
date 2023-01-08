use [FLChat]

declare @dt as table (
	[Id] integer NOT NULL PRIMARY KEY,
	[DeepLink] nvarchar(255) NULL
)

insert into @dt ([Id], [DeepLink])
values 
 (1, N'tg://resolve?domain=Vicadobot&start=%code%') --telegram
,(3, N'viber://pa?chatURI=vicado&context=%code%') --viber
,(4, N'https://vk.com/write-189060928?ref=%code%&ref_source=webchat') --VK

update t
set [DeepLink] = d.[DeepLink]
from [Cfg].[TransportType] t
left join @dt d on t.[Id] = d.[Id]
where t.[DeepLink] <> d.[DeepLink]
  or (t.[DeepLink] is null and d.[DeepLink] is not null)

select [Id], [DeepLink], [DeepLink] from [Cfg].[TransportType]

GO
use [FLChatProt]
go

select top 100 * 
from [dbo].[TransportLog]
--where 
	--[TransportTypeId] = /**TG**/1
	--[TransportTypeId] = /**Viber**/3
	--[TransportTypeId] = /**VK**/4
	--[TransportTypeId] = /**WebChat**/100
	--[TransportTypeId] = /**SMS**/150
order by [Id] desc
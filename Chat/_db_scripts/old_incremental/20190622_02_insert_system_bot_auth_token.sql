use [FLChat]
go

update [Usr].[User] set [Enabled] = 1 where [IsBot] = 1
go

delete from [Auth].[AuthToken] where [UserId] = '00000000-0000-0000-0000-000000000000'
go

insert into [Auth].[AuthToken] ([UserId], [Token], [IssueDate], [ExpireBy])
values ('00000000-0000-0000-0000-000000000000', 
  'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjAwMDAwMDAwLTAwMDAtMDAwMC0wMDAwLTAwMDAwMDAwMDAwMCIsImlzcyI6IjIwMTktMDYtMjJUMTE6NDE6NTYuNTUzMzcyMyswODowMCIsImV4cCI6NjMwNzIwMDAwfQ.kxR4cp4DoEDTL4POsmBxxBeFGiDXHSVMcjKvpRVxoSc',
  convert(datetime, '2019-06-22 11:41:56.553', 20), 630720000)
go
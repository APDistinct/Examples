/*
Missing Index Details from SQLQuery2.sql - localhost.FLChat (flchat (56))
The Query Processor estimates that implementing the following index could improve the query cost by 47.5448%.
*/


USE [FLChat]
GO

if not exists(select * from sysindexes where [name] = 'Idx__MsgMessageToUser__ToUserId')
CREATE NONCLUSTERED INDEX [Idx__MsgMessageToUser__ToUserId]
ON [Msg].[MessageToUser] ([ToUserId])

GO


USE [FLChat]
GO

ALTER TABLE [Usr].[Transport] DROP CONSTRAINT [CHK_UsrTransport_OuterId]
GO

ALTER TABLE [Usr].[Transport]  WITH CHECK ADD  CONSTRAINT [CHK_UsrTransport_OuterId] 
  CHECK  ([TransportTypeId] IN (0, 100, 150, 151) AND [TransportOuterId] IS NULL 
    OR    [TransportTypeId] NOT IN (0, 100, 150, 151) AND [TransportOuterId] IS NOT NULL)
GO

ALTER TABLE [Usr].[Transport] CHECK CONSTRAINT [CHK_UsrTransport_OuterId]
GO



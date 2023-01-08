USE [FLChat]
GO

/****** Object:  Index [UNQ__UsrTransport_OuterId]    Script Date: 12.06.2019 15:53:45 ******/
DROP INDEX [UNQ__UsrTransport_OuterId] ON [Usr].[Transport]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [UNQ__UsrTransport_OuterId]    Script Date: 12.06.2019 15:53:45 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UNQ__UsrTransport_OuterId] ON [Usr].[Transport]
(
	[TransportTypeId] ASC,
	[TransportOuterId] ASC
)
WHERE ([TransportOuterId] is not null AND [Enabled]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



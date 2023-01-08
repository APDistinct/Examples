USE [FLChat]
GO

/****** Object:  View [Usr].[UserDefaultTransportView]    Script Date: 15.08.2019 14:45:32 ******/
DROP VIEW IF EXISTS [Usr].[UserMailingTransportView]
GO

/****** Object:  View [Usr].[UserDefaultTransportView]    Script Date: 15.08.2019 14:45:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [Usr].[UserMailingTransportView]
AS
SELECT DISTINCT u.Id AS UserId, t.TransportTypeId AS DefaultTransportTypeId
FROM            Usr.[User] AS u 
		INNER JOIN Usr.Transport AS t ON u.Id = t.UserId AND t.Enabled = 1 
		INNER JOIN Cfg.TransportType AS tt ON t.TransportTypeId = tt.Id AND tt.Enabled = 1
WHERE        (u.Enabled = 1) AND (t.TransportTypeId IN (151))
GO



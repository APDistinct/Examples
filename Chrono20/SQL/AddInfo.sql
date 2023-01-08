USE [TimeTrack2_0]
GO
--  [Cfg].[PMSystem]
declare @dt as table (
  [Id] uniqueidentifier NOT NULL,
  [Name] varchar(max) NULL
);

INSERT INTO @dt 
([Id], [Name])
VALUES 
('00000000-0000-0000-0000-000000000000', 'Test'),
('00000000-0001-0000-0000-000000000000', 'Azure DevOps Server'),
('00000000-0002-0000-0000-000000000000', 'Jira');


update t
set t.[Name] = d.[Name]
from [Cfg].[PMSystem] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name]

insert into [Cfg].[PMSystem]
([Id], [Name])
select [Id], [Name]
from @dt where [Id] not in (select [Id] from [Cfg].[PMSystem]);
GO

--  [Cfg].[BusinessLine]
declare @dt as table (
  [Id] uniqueidentifier NOT NULL,
  [Name] varchar(max) NULL
);

INSERT INTO @dt 
([Id], [Name])
VALUES 
('00000000-0000-0000-0000-000000000000', 'Test'),
('00000000-0001-0000-0000-000000000000', 'Line 1'),
('00000000-0002-0000-0000-000000000000', 'Line 2');


update t
set t.[Name] = d.[Name]
from [Cfg].[BusinessLine] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name]

insert into [Cfg].[BusinessLine]
([Id], [Name])
select [Id], [Name]
from @dt where [Id] not in (select [Id] from [Cfg].[BusinessLine]);
GO

--  [Cfg].[Country]
declare @dt as table (
  [Id] uniqueidentifier NOT NULL,
  [Name] varchar(max) NULL
);

INSERT INTO @dt 
([Id], [Name])
VALUES 
('00000000-0000-0000-0000-000000000000', 'Test'),
('00000000-0001-0000-0000-000000000000', 'RB'),
('00000000-0002-0000-0000-000000000000', 'RF');


update t
set t.[Name] = d.[Name]
from [Cfg].[Country] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name]

insert into [Cfg].[Country]
([Id], [Name])
select [Id], [Name]
from @dt where [Id] not in (select [Id] from [Cfg].[Country]);
GO

--  [Cfg].[Department]
declare @dt as table (
  [Id] uniqueidentifier NOT NULL,
  [Name] varchar(max) NULL
);

INSERT INTO @dt 
([Id], [Name])
VALUES 
('00000000-0000-0000-0000-000000000000', 'Test'),
('00000000-0001-0000-0000-000000000000', 'IT Infrastructure Services'),
('00000000-0002-0000-0000-000000000000', '.Net'),
('00000000-0003-0000-0000-000000000000', 'QA'),
('00000000-0004-0000-0000-000000000000', 'Ruby');


update t
set t.[Name] = d.[Name]
from [Cfg].[Department] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name]

insert into [Cfg].[Department]
([Id], [Name])
select [Id], [Name]
from @dt where [Id] not in (select [Id] from [Cfg].[Department]);
GO

--  [Cfg].[Position]
declare @dt as table (
  [Id] uniqueidentifier NOT NULL,
  [Name] varchar(max) NULL
);

INSERT INTO @dt 
([Id], [Name])
VALUES 
('00000000-0000-0000-0000-000000000000', 'Test'),
('00000000-0001-0000-0000-000000000000', '.Net Developer'),
('00000000-0002-0000-0000-000000000000', '.Net Team Lead'),
('00000000-0003-0000-0000-000000000000', 'Account Assistant'),
('00000000-0004-0000-0000-000000000000', 'Account Manager');


update t
set t.[Name] = d.[Name]
from [Cfg].[Position] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name]

insert into [Cfg].[Position]
([Id], [Name])
select [Id], [Name]
from @dt where [Id] not in (select [Id] from [Cfg].[Position]);
GO

--  [Cfg].[ReportStatus]
declare @dt as table (
  [Id] integer NOT NULL,
  [Status] varchar(max) NULL
);

INSERT INTO @dt 
([Id], [Status])
VALUES 
(0, 'Edited'),
(1, 'WaitingForApprove'),
(2, 'Declined'),
(3, 'Approved'),
(4, 'Billed');


update t
set t.[Status] = d.[Status]
from [Cfg].[ReportStatus] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Status] <> d.[Status];

SET IDENTITY_INSERT [Cfg].[ReportStatus] ON
insert into [Cfg].[ReportStatus]
([Id], [Status])
select [Id], [Status]
from @dt where [Id] not in (select [Id] from [Cfg].[ReportStatus]);
SET IDENTITY_INSERT [Cfg].[ReportStatus] OFF
GO

--  [Cfg].[WorkItemStatus]
declare @dt as table (
  [Id] integer NOT NULL,
  [Status] varchar(max) NULL
);

INSERT INTO @dt 
([Id], [Status])
VALUES 
(0, 'Edited'),
(1, 'WaitingForApprove'),
(2, 'Declined'),
(3, 'Approved'),
(4, 'Billed');

update t
set t.[Status] = d.[Status]
from [Cfg].[WorkItemStatus] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Status] <> d.[Status]
SET IDENTITY_INSERT [Cfg].[WorkItemStatus] ON
insert into [Cfg].[WorkItemStatus]
([Id], [Status])
select [Id], [Status]
from @dt where [Id] not in (select [Id] from [Cfg].[WorkItemStatus]);
SET IDENTITY_INSERT [Cfg].[WorkItemStatus] OFF
GO

--  [Cfg].[WorkItemType]
declare @dt as table (
  [Id] int NOT NULL,
  [Name] varchar(max) NULL
);

INSERT INTO @dt 
([Id], [Name])
VALUES 
(0, 'Test'),
(1, 'HelpDesk'),
(2, 'Task');


update t
set t.[Name] = d.[Name]
from [Cfg].[WorkItemType] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name]

SET IDENTITY_INSERT [Cfg].[WorkItemType] ON
insert into [Cfg].[WorkItemType]
([Id], [Name])
select [Id], [Name]
from @dt where [Id] not in (select [Id] from [Cfg].[WorkItemType]);
SET IDENTITY_INSERT [Cfg].[WorkItemType] OFF
GO

-- ProjectRole
declare @dt as table (
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[Type] [int] NOT NULL
);

INSERT INTO @dt 
([Id], [Name], [ParentId], [Type])
VALUES 
('00000000-0000-0000-0000-000000000001', 'Resource Manager', null, 1),
('00000000-0000-0000-0000-000000000002', 'Account manager', null, 1),
('00000000-0000-0000-0000-000000000003', 'Project Manager', '00000000-0000-0000-0000-000000000001', 3),
('00000000-0000-0000-0000-000000000004', 'User', '00000000-0000-0000-0000-000000000003', 3);


update t
set t.[Name] = d.[Name], t.[ParentId] = d.[ParentId], t.[Type] = d.[Type]
from [dbo].[ProjectRole] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name]
  or t.[ParentId] <> d.[ParentId]
  or t.[Type] <> d.[Type]

insert into [dbo].[ProjectRole]
([Id], [Name], [ParentId], [Type])
select [Id], [Name], [ParentId], [Type]
from @dt where [Id] not in (select [Id] from [dbo].[ProjectRole]);
GO

--  Location
declare @dt as table (
  [Id] [uniqueidentifier] NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Type] [int] NOT NULL
);

INSERT INTO @dt 
([Id], [Name], [Type])
VALUES 
('00000000-0000-0000-0000-000000000000', 'Test', 1);


update t
set t.[Name] = d.[Name],  t.[Type] = d.[Type]
from [dbo].[Location] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name]
  or t.[Type] <> d.[Type]

insert into [dbo].[Location]
([Id], [Name], [Type])
select [Id], [Name], [Type]
from @dt where [Id] not in (select [Id] from [dbo].[Location]);
GO

--  [dbo].[ProjectCollection]
declare @dt as table (
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[SubscriptionId] [int] NOT NULL,
	[TfsId] [uniqueidentifier] NOT NULL
);

INSERT INTO @dt 
([Id], [Name], [IsDeleted], [SubscriptionId], [TfsId])
VALUES 
('00000000-0000-0000-0000-000000000000', 'Test', 0, 1, '00000000-0000-0000-0000-000000000000');


update t
set t.[Name] = d.[Name]
from [dbo].[ProjectCollection] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name]
 or t.[IsDeleted] <> d.[IsDeleted]
 or t.[SubscriptionId] <> d.[SubscriptionId]
 or t.[TfsId] <> d.[TfsId]

insert into [dbo].[ProjectCollection]
([Id], [Name], [IsDeleted], [SubscriptionId], [TfsId])
select [Id], [Name], [IsDeleted], [SubscriptionId], [TfsId]
from @dt where [Id] not in (select [Id] from [dbo].[ProjectCollection]);
GO

--  [dbo].[Project]
declare @dt as table (
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Uri] [nvarchar](max) NOT NULL,
	[OutId] [nvarchar](256) NOT NULL,
	[PMSystemId] [uniqueidentifier] NOT NULL,
	[ProjectCollectionId] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL
);

INSERT INTO @dt 
([Id], [Name], [Uri], [OutId], [PMSystemId], [ProjectCollectionId], [IsDeleted])
VALUES 
('00000000-0000-0000-0000-000000000001', 'Test1', '---', '0001', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0),
('00000000-0000-0000-0000-000000000002', 'Test2', '---', '0002', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 0),
('00000000-0000-0000-0000-000000000003', 'Test3', '---', '0003', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 1);


update t
set t.[Name] = d.[Name], t.[Uri] = d.[Uri], t.[OutId] = d.[OutId], t.[PMSystemId] = d.[PMSystemId], t.[ProjectCollectionId] = d.[ProjectCollectionId], t.[IsDeleted] = d.[IsDeleted]
from [dbo].[Project] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name]
  or t.[Uri] <> d.[Uri]
  or t.[OutId] <> d.[OutId]
  or t.[PMSystemId] <> d.[PMSystemId]
  or t.[ProjectCollectionId] <> d.[ProjectCollectionId]
  or t.[IsDeleted] <> d.[IsDeleted]

insert into [dbo].[Project]
([Id], [Name], [Uri], [OutId], [PMSystemId], [ProjectCollectionId], [IsDeleted])
select [Id], [Name], [Uri], [OutId], [PMSystemId], [ProjectCollectionId], [IsDeleted]
from @dt where [Id] not in (select [Id] from [dbo].[Project]);
GO

--  [dbo].[WorkItem]
declare @dt as table (
	[Id] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[OutId] [nvarchar](256) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[WorkItemTypeId] [int] NULL,
	[Estimate] [float] NULL
);

INSERT INTO @dt 
([Id], [ProjectId], [OutId], [Name], [WorkItemTypeId], [Estimate])
VALUES 
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000001', '0001', 'Test1-1', 0, 0),
('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000001', '0002', 'Test1-2', 0, 0),
('00000000-0000-0000-0000-000000000003', '00000000-0000-0000-0000-000000000001', '0003', 'Test1-3', 0, 0),
('00000000-0000-0000-0000-000000000004', '00000000-0000-0000-0000-000000000002', '0004', 'Test2-1', 1, 0),
('00000000-0000-0000-0000-000000000005', '00000000-0000-0000-0000-000000000002', '0005', 'Test2-2', 1, 0),
('00000000-0000-0000-0000-000000000006', '00000000-0000-0000-0000-000000000002', '0006', 'Test2-3', 1, 0);


update t
set t.[ProjectId] = d.[ProjectId], t.[OutId] = d.[OutId], t.[Name] = d.[Name], t.[WorkItemTypeId] = d.[WorkItemTypeId], t.[Estimate] = d.[Estimate]
from [dbo].[WorkItem] t
inner join @dt d on t.[Id] = d.[Id]
where t.[Name] <> d.[Name]
  or t.[ProjectId] <> d.[ProjectId]
  or t.[OutId] <> d.[OutId]
  or t.[WorkItemTypeId] <> d.[WorkItemTypeId]
  or t.[Estimate] <> d.[Estimate]

insert into [dbo].[WorkItem]
([Id], [ProjectId], [OutId], [Name], [WorkItemTypeId], [Estimate])
select [Id], [ProjectId], [OutId], [Name], [WorkItemTypeId], [Estimate]
from @dt where [Id] not in (select [Id] from [dbo].[WorkItem]);
GO

--  [dbo].[User]
declare @dt as table (
	[UserId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[DisplayName] [nvarchar](255) NULL,
	[PositionId] [uniqueidentifier] NOT NULL,
	[CountryId] [uniqueidentifier] NULL,
	[LocationId] [uniqueidentifier] NOT NULL,
	[LocationFrom] [datetime] NOT NULL,
	[Sid] [nvarchar](256) NOT NULL,
	[Deleted] [bit] NOT NULL
);

declare @datel datetime = DATEFROMPARTS(1990,1,1);
INSERT INTO @dt 
([UserId], [UserName], [Email], [DisplayName], [PositionId], [CountryId], [LocationId], [LocationFrom], [Sid], [Deleted])
VALUES 
('00000000-0000-0000-0000-000000000001', 'Test1-1','000000000001', 'Test1-1',  '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', @datel,'0001', 0),
('00000000-0000-0000-0000-000000000002', 'Test1-2','000000000002', 'Test1-2',  '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', @datel,'0002', 0),
('00000000-0000-0000-0000-000000000003', 'Test1-3','000000000003', 'Test1-3',  '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', @datel,'0003', 1),
('00000000-0000-0000-0000-000000000004', 'Test2-1','000000000004', 'Test2-1',  '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', @datel,'0004', 0),
('00000000-0000-0000-0000-000000000005', 'Test2-2','000000000005', 'Test2-2',  '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', @datel,'0005', 0),
('00000000-0000-0000-0000-000000000006', 'Test2-3','000000000006', 'Test2-3',  '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', @datel,'0006', 1);


update t
set t.[UserName] = d.[UserName], t.[Email] = d.[Email], t.[DisplayName] = d.[DisplayName], t.[PositionId] = d.[PositionId], t.[CountryId] = d.[CountryId], t.[LocationId] = d.[LocationId], t.[LocationFrom] = d.[LocationFrom], t.[Sid] = d.[Sid], t.[Deleted] = d.[Deleted]
--t.[ProjectId] = d.[ProjectId], t.[OutId] = d.[OutId], t.[Name] = d.[Name], t.[WorkItemTypeId] = d.[WorkItemTypeId], t.[Estimate] = d.[Estimate]
from [dbo].[User] t
inner join @dt d on t.[UserId] = d.[UserId]
where t.[UserName] <> d.[UserName]
  or t.[Email] <> d.[Email]
  or t.[DisplayName] <> d.[DisplayName]
  or t.[PositionId] <> d.[PositionId]
  or t.[CountryId] <> d.[CountryId]
  or t.[LocationId] <> d.[LocationId]
  or t.[LocationFrom] <> d.[LocationFrom]
  or t.[Sid] <> d.[Sid]
  or t.[Deleted] <> d.[Deleted]

insert into [dbo].[User]
([UserId], [UserName], [Email], [DisplayName], [PositionId], [CountryId], [LocationId], [LocationFrom], [Sid], [Deleted])
select [UserId], [UserName], [Email], [DisplayName], [PositionId], [CountryId], [LocationId], [LocationFrom], [Sid], [Deleted]
from @dt where [UserId] not in (select [UserId] from [dbo].[User]);
GO

-- RoleMember
declare @dt as table (
	[UserId] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL
);

INSERT INTO @dt 
([UserId], [ProjectId], [RoleId])
VALUES 
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000001'),
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000001'),
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000004'),
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000004'),
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000003', '00000000-0000-0000-0000-000000000001'),
('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000004'),
('00000000-0000-0000-0000-000000000003', '00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000004'),
('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000004'),
('00000000-0000-0000-0000-000000000003', '00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000004'),
('00000000-0000-0000-0000-000000000004', '00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000004');


MERGE [dbo].[RoleMember] AS tgt  
USING (select * from @dt) AS src ([UserId], [ProjectId], [RoleId])
ON (tgt.UserId = src.[UserId] and tgt.[ProjectId] = src.[ProjectId] and tgt.[RoleId] = src.[RoleId])  
WHEN NOT MATCHED BY TARGET THEN  
INSERT ([UserId], [ProjectId], [RoleId]) VALUES (src.[UserId], src.[ProjectId], src.[RoleId]);
GO

-- TimeReport
declare @dt as table (
	[UserId] [uniqueidentifier] NOT NULL,
	[WorkItemId] [uniqueidentifier] NOT NULL,
	[ReportDate] [date] NOT NULL,
	[Hours] [float] NOT NULL,
	[BillHours] [float] NOT NULL,
	[ReportStatusId] [int] NOT NULL,
	[Type] [int] NOT NULL
);
declare @datel datetime = DATEFROMPARTS(2022,12,1);
INSERT INTO @dt 
([UserId], --6
 [WorkItemId],  --6
 [ReportDate], 
 [Hours],
 [BillHours],
 [ReportStatusId],  -- 0 - 4
 [Type])  --  0 - 1

VALUES 
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000001', @datel, 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000001', DATEADD(day,1,@datel), 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000001', DATEADD(day,2,@datel), 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000001', DATEADD(day,3,@datel), 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000002', @datel, 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000002', DATEADD(day,1,@datel), 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000002', DATEADD(day,2,@datel), 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000002', DATEADD(day,3,@datel), 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000001', @datel, 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000001', DATEADD(day,1,@datel), 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000001', DATEADD(day,2,@datel), 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000001', DATEADD(day,3,@datel), 8, 0, 0, 0),

('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000002', @datel, 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000002', DATEADD(day,1,@datel), 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000002', DATEADD(day,2,@datel), 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000002', DATEADD(day,3,@datel), 8, 0, 0, 0),

('00000000-0000-0000-0000-000000000006', '00000000-0000-0000-0000-000000000001', @datel, 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000006', '00000000-0000-0000-0000-000000000001', DATEADD(day,1,@datel), 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000006', '00000000-0000-0000-0000-000000000002', DATEADD(day,2,@datel), 8, 0, 0, 0),
('00000000-0000-0000-0000-000000000006', '00000000-0000-0000-0000-000000000002', DATEADD(day,3,@datel), 8, 0, 0, 0)
;

MERGE [dbo].[TimeReport] AS tgt  
USING (select * from @dt) AS src ([UserId], [WorkItemId], [ReportDate], [Hours], [BillHours], [ReportStatusId],  [Type])
ON (tgt.UserId = src.[UserId] and tgt.[WorkItemId] = src.[WorkItemId] and tgt.[ReportDate] = src.[ReportDate])  
WHEN MATCHED THEN
 UPDATE SET [Hours] = src.[Hours], [BillHours] = src.[BillHours], [ReportStatusId] = src.[ReportStatusId],  [Type] = src.[Type]
WHEN NOT MATCHED BY TARGET THEN  
INSERT ([UserId], [WorkItemId], [ReportDate], [Hours], [BillHours], [ReportStatusId],  [Type])
 VALUES (src.[UserId], src.[WorkItemId], src.[ReportDate], src.[Hours], src.[BillHours], src.[ReportStatusId],  src.[Type]);
GO

--ALTER TABLE [dbo].[TimeReport] ADD  DEFAULT (getutcdate()) FOR [LastUpdated]
GO

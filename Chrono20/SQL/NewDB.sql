USE [TimeTrack2_0]
GO

if SCHEMA_ID('Cfg') is null
exec ('CREATE SCHEMA [Cfg]')
GO

if SCHEMA_ID('Import') is null
exec ('CREATE SCHEMA [Import]')
GO

CREATE TABLE [Cfg].[BusinessLine](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK__BusinessLine] PRIMARY KEY CLUSTERED ([Id] ASC)
 )
 GO

CREATE TABLE [Cfg].[Country](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK__Country] PRIMARY KEY CLUSTERED ([Id] ASC)
 )
 GO

CREATE TABLE [Cfg].[Department](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK__Department] PRIMARY KEY CLUSTERED ([Id] ASC)
 )
 GO

CREATE TABLE [Cfg].[PMSystem](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK__PMSystem] PRIMARY KEY CLUSTERED ([Id] ASC)
 )
 GO

CREATE TABLE [Cfg].[Position](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK__Position] PRIMARY KEY CLUSTERED ([Id] ASC)
 )
 GO

CREATE TABLE [Cfg].[ReportStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](max) NULL,
 CONSTRAINT [PK__ReportStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
 )
 GO

CREATE TABLE [Cfg].[Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](255) NOT NULL,
	[Value] [nvarchar](1000) NULL,
	[HasUserInteface] [bit] NOT NULL,
	[Type] [int] NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__Settings__Id] PRIMARY KEY CLUSTERED ([Id] ASC)
 )
GO

CREATE TABLE [Cfg].[WorkItemStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK__WorkItemStatus] PRIMARY KEY CLUSTERED  ([Id] ASC)
)
GO

CREATE TABLE [Cfg].[WorkItemType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK__WorkItemType] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO

----  [Id] uniqueidentifier NOT NULL DEFAULT NEWSEQUENTIALID(),

CREATE TABLE [dbo].[Bill](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[DateBill] [datetime] NOT NULL,
	[IsExternal] [bit] NOT NULL,
 CONSTRAINT [PK__Bill] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[DefaultTimetable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Monday] [float] NOT NULL,
	[Tuesday] [float] NOT NULL,
	[Wednesday] [float] NOT NULL,
	[Thursday] [float] NOT NULL,
	[Friday] [float] NOT NULL,
	[Saturday] [float] NOT NULL,
	[Sunday] [float] NOT NULL,
	[ValidFrom] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK__DefaultTimetable] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Location](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO

CREATE TABLE [dbo].[LocationHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[LocationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[DateFrom] [datetime] NOT NULL,
	[DateTill] [datetime] NOT NULL,
 CONSTRAINT [PK_LocationHistory] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO

CREATE TABLE [dbo].[Project](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Uri] [nvarchar](max) NOT NULL,
	[OutId] [nvarchar](256) NOT NULL,
	[PMSystemId] [uniqueidentifier] NOT NULL,
	[ProjectCollectionId] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Project__Id] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[ProjectCollection](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[SubscriptionId] [int] NOT NULL,
	[TfsId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK__ProjectCollection__Id] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[ProjectRole](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK__ProjectRole__Id] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[RoleMember](
	[UserId] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK__RoleMember] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ProjectId] ASC,
	[RoleId] ASC
))
GO

CREATE TABLE [dbo].[SentForApproveLog](
	[UserId] [uniqueidentifier] NOT NULL,
	[SentDate] [datetime] NOT NULL,
 CONSTRAINT [PK__SentForApproveLog] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[SentDate] ASC
))
GO

/***
CREATE TABLE [dbo].[ServerRoleMembers](
	[ServerRoleId] [int] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK__ServerRoleMembers] PRIMARY KEY CLUSTERED 
(
	[ServerRoleId] ASC,
	[UserId] ASC
)
GO

CREATE TABLE [dbo].[ServerRoleNames](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK__ServerRoleName__Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO

***/

/***
CREATE TABLE [dbo].[TaskStatus](
	[Id] [uniqueidentifier] NOT NULL,
	[TaskId] [int] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK__TaskStatus] PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC,
	[UserId] ASC,
	[StartDate] ASC,
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
***/

CREATE TABLE [dbo].[TimeReport](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[WorkItemId] [uniqueidentifier] NOT NULL,
	[ReportDate] [date] NOT NULL,
	[Hours] [float] NOT NULL,
	[BillHours] [float] NOT NULL,
	[ReportStatusId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[IsSynchronized] [bit] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[ExternalBillId] [uniqueidentifier] NULL,
	[InternalBillId] [uniqueidentifier] NULL,
	CONSTRAINT [PK__TimeReport__Id] PRIMARY KEY NONCLUSTERED ([Id]),
	CONSTRAINT [UNQ__TimeReport_Main] UNIQUE CLUSTERED 
(
	[UserId] ASC,
	[WorkItemId] ASC,
	[ReportDate] ASC,
	[Type] ASC
)
)
GO

CREATE TABLE [dbo].[TimeReportHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[WorkItemId] [uniqueidentifier] NOT NULL,
	[ReportDate] [date] NOT NULL,
	[Hours] [float] NOT NULL,
	[BillHours] [float] NOT NULL,
	[Type] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateUserId] [uniqueidentifier] NOT NULL,
	[ExternalBillId] [uniqueidentifier] NULL,
	[InternalBillId] [uniqueidentifier] NULL,
	CONSTRAINT [PK__TimeReportHistory__Id] PRIMARY KEY NONCLUSTERED ([Id]),
	CONSTRAINT [UNQ__TimeReportHistory_Main] UNIQUE CLUSTERED 
(
	[UserId] ASC,
	[WorkItemId] ASC,
	[ReportDate] ASC,
	[Type] ASC
)
)
GO

CREATE TABLE [dbo].[Timetable](
	[UserId] [uniqueidentifier] NOT NULL,
	[Monday] [float] NOT NULL,
	[Tuesday] [float] NOT NULL,
	[Wednesday] [float] NOT NULL,
	[Thursday] [float] NOT NULL,
	[Friday] [float] NOT NULL,
	[Saturday] [float] NOT NULL,
	[Sunday] [float] NOT NULL,
	[ValidFrom] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK__Timetable] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[User](
	[UserId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[DisplayName] [nvarchar](255) NULL,
	[PositionId] [uniqueidentifier] NOT NULL,
--	[DepartmentId] [int] NOT NULL,
	[CountryId] [uniqueidentifier] NULL,
	[LocationId] [uniqueidentifier] NOT NULL,
	[LocationFrom] [datetime] NOT NULL,
	[Sid] [nvarchar](256) NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK__User] PRIMARY KEY CLUSTERED ([UserId] ASC)
)
GO

CREATE TABLE [dbo].[UserInfo](
	[UserId] [uniqueidentifier] NOT NULL,
	[SynchronizeName] [nvarchar](255) NULL,
 CONSTRAINT [PK__UserInfo] PRIMARY KEY CLUSTERED ([UserId] ASC)
)
GO

CREATE TABLE [dbo].[UserBusinessLine](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[BusinessLineId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK__UserBusinessLine] PRIMARY KEY CLUSTERED ([Id] ASC)
 )
 GO

CREATE TABLE [dbo].[UserDepartment](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[DepartmentId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK__UserDepartment] PRIMARY KEY CLUSTERED ([Id] ASC)
 )
 GO

CREATE TABLE [dbo].[WorkItem](
	[Id] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[OutId] [nvarchar](256) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[WorkItemTypeId] [int] NULL,
	[Estimate] [float] NULL,
	CONSTRAINT [PK__WorkItem__Id] PRIMARY KEY NONCLUSTERED ([Id]),
	CONSTRAINT [UNQ__WorkItem_Main] UNIQUE CLUSTERED 
(
	[ProjectId] ASC,
	[OutId] ASC
)
)
GO

CREATE TABLE [dbo].[WorkItemState](
	[Id] [uniqueidentifier] NOT NULL,
	[WorkItemId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[WorkItemStatusId] [int] NOT NULL,
	CONSTRAINT [PK__WorkItemState__Id] PRIMARY KEY NONCLUSTERED ([Id]),
	CONSTRAINT [UNQ__WorkItemState_Main] UNIQUE CLUSTERED 
(
	[WorkItemId] ASC,
	[UserId] ASC,
	[StartDate] ASC
)
)
GO

CREATE TABLE [dbo].[WorkItemStateHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[WorkItemStateId] [uniqueidentifier] NOT NULL,
	[ChangeUserId] [uniqueidentifier] NOT NULL,
	[ChangeDate] [datetime] NOT NULL,
	[WorkItemStatusId] [int] NOT NULL,
	CONSTRAINT [PK__WorkItemStateHistory__Id] PRIMARY KEY CLUSTERED ([Id])
)
GO

CREATE TABLE [Import].[OutProjectInfoPattern](
	[Id] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[ProjectName] [nvarchar](255) NULL,
	[DateName] [nvarchar](255) NOT NULL,
	[TimeName] [nvarchar](255) NOT NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[WorkItemName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__OutProjectInfoPattern__Id] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [Import].[OutTimeReportData](
	[id] [uniqueidentifier] NOT NULL,
	[OutTimeReportInfoId] [uniqueidentifier] NOT NULL,
	[TimeReportId] [uniqueidentifier] NULL,
	[DateValue] [datetime] NOT NULL,
	[TimeValue] [float] NOT NULL,
	[UserValue] [nvarchar](255) NOT NULL,
	[WorkItemValue] [nvarchar](255) NOT NULL,
	[Mark] [int] NULL,
 CONSTRAINT [PK__OutTimeReportData__Id] PRIMARY KEY CLUSTERED ([id] ASC)
)
GO

CREATE TABLE [Import].[OutTimeReportInfo](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[LoadDate] [datetime] NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__OutTimeReportInfo__Id] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [Import].[OutUser](
	[UserId] [uniqueidentifier] NOT NULL,
	[OutName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__OutUser__Id] PRIMARY KEY CLUSTERED ([UserId] ASC),
 CONSTRAINT [UNQ__OutUser__OutName] UNIQUE NONCLUSTERED 
(
	[OutName] ASC
)
)
GO

CREATE TABLE [Import].[OutWorkItem](
	[Id] [uniqueidentifier] NOT NULL,
	[WorkItemId] [uniqueidentifier] NOT NULL,
	[OutName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__OutWorkItem__Id] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

ALTER TABLE [Cfg].[BusinessLine] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [Cfg].[Country] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [Cfg].[Department] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [Cfg].[PMSystem] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [Cfg].[Position] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Bill] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Location] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[LocationHistory] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Project] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF_Project_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ProjectCollection] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[ProjectCollection] ADD  CONSTRAINT [DF_ProjectCollection_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ProjectRole] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[TimeReport] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[TimeReport] ADD  DEFAULT ((0)) FOR [BillHours]
GO
ALTER TABLE [dbo].[TimeReport] ADD  CONSTRAINT [DF_TimeReport_IsSynchronized]  DEFAULT ((0)) FOR [IsSynchronized]
GO
ALTER TABLE [dbo].[TimeReport] ADD  DEFAULT (getutcdate()) FOR [LastUpdated]
GO
ALTER TABLE [dbo].[TimeReportHistory] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[TimeReportHistory] ADD  DEFAULT ((0)) FOR [BillHours]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT (newsequentialid()) FOR [UserId]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[UserBusinessLine] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[UserDepartment] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[WorkItem] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[WorkItemState] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[WorkItemStateHistory] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [Import].[OutProjectInfoPattern] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [Import].[OutTimeReportData] ADD  DEFAULT (newsequentialid()) FOR [id]
GO
ALTER TABLE [Import].[OutTimeReportData] ADD  DEFAULT (getutcdate()) FOR [DateValue]
GO
ALTER TABLE [Import].[OutTimeReportInfo] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [Import].[OutTimeReportInfo] ADD  DEFAULT (getutcdate()) FOR [LoadDate]
GO
ALTER TABLE [Import].[OutWorkItem] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[LocationHistory]  WITH CHECK ADD  CONSTRAINT [FK_LocationHistory_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[LocationHistory] CHECK CONSTRAINT [FK_LocationHistory_Location]
GO
ALTER TABLE [dbo].[LocationHistory]  WITH CHECK ADD  CONSTRAINT [FK_LocationHistory_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[LocationHistory] CHECK CONSTRAINT [FK_LocationHistory_User]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK__Project_ProjectCollection] FOREIGN KEY([ProjectCollectionId])
REFERENCES [dbo].[ProjectCollection] ([Id])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK__Project_ProjectCollection]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_PMSystem] FOREIGN KEY([PMSystemId])
REFERENCES [Cfg].[PMSystem] ([Id])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_PMSystem]
GO
ALTER TABLE [dbo].[ProjectRole]  WITH CHECK ADD  CONSTRAINT [FK__ProjectRole_ProjectRole] FOREIGN KEY([ParentId])
REFERENCES [dbo].[ProjectRole] ([Id])
GO
ALTER TABLE [dbo].[ProjectRole] CHECK CONSTRAINT [FK__ProjectRole_ProjectRole]
GO
ALTER TABLE [dbo].[RoleMember]  WITH CHECK ADD  CONSTRAINT [FK_RoleMember_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[RoleMember] CHECK CONSTRAINT [FK_RoleMember_Project]
GO
ALTER TABLE [dbo].[RoleMember]  WITH CHECK ADD  CONSTRAINT [FK_RoleMember_ProjectRole] FOREIGN KEY([RoleId])
REFERENCES [dbo].[ProjectRole] ([Id])
GO
ALTER TABLE [dbo].[RoleMember] CHECK CONSTRAINT [FK_RoleMember_ProjectRole]
GO
ALTER TABLE [dbo].[RoleMember]  WITH CHECK ADD  CONSTRAINT [FK_RoleMember_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[RoleMember] CHECK CONSTRAINT [FK_RoleMember_User]
GO
ALTER TABLE [dbo].[TimeReport]  WITH CHECK ADD  CONSTRAINT [FK_TimeReport_Bill_External] FOREIGN KEY([ExternalBillId])
REFERENCES [dbo].[Bill] ([Id])
GO
ALTER TABLE [dbo].[TimeReport] CHECK CONSTRAINT [FK_TimeReport_Bill_External]
GO
ALTER TABLE [dbo].[TimeReport]  WITH CHECK ADD  CONSTRAINT [FK_TimeReport_Bill_Internal] FOREIGN KEY([InternalBillId])
REFERENCES [dbo].[Bill] ([Id])
GO
ALTER TABLE [dbo].[TimeReport] CHECK CONSTRAINT [FK_TimeReport_Bill_Internal]
GO
ALTER TABLE [dbo].[TimeReport]  WITH CHECK ADD  CONSTRAINT [FK_TimeReport_ReportStatus] FOREIGN KEY([ReportStatusId])
REFERENCES [Cfg].[ReportStatus] ([Id])
GO
ALTER TABLE [dbo].[TimeReport] CHECK CONSTRAINT [FK_TimeReport_ReportStatus]
GO
ALTER TABLE [dbo].[TimeReport]  WITH CHECK ADD  CONSTRAINT [FK_TimeReport_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TimeReport] CHECK CONSTRAINT [FK_TimeReport_User]
GO
ALTER TABLE [dbo].[TimeReport]  WITH CHECK ADD  CONSTRAINT [FK_TimeReport_WorkItem] FOREIGN KEY([WorkItemId])
REFERENCES [dbo].[WorkItem] ([Id])
GO
ALTER TABLE [dbo].[TimeReport] CHECK CONSTRAINT [FK_TimeReport_WorkItem]
GO
ALTER TABLE [dbo].[TimeReportHistory]  WITH CHECK ADD  CONSTRAINT [FK_TimeReportHistory_UpdateUser] FOREIGN KEY([UpdateUserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TimeReportHistory] CHECK CONSTRAINT [FK_TimeReportHistory_UpdateUser]
GO
ALTER TABLE [dbo].[TimeReportHistory]  WITH CHECK ADD  CONSTRAINT [FK_TimeReportHistory_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TimeReportHistory] CHECK CONSTRAINT [FK_TimeReportHistory_User]
GO
ALTER TABLE [dbo].[TimeReportHistory]  WITH CHECK ADD  CONSTRAINT [FK_TimeReportHistory_WorkItem] FOREIGN KEY([WorkItemId])
REFERENCES [dbo].[WorkItem] ([Id])
GO
ALTER TABLE [dbo].[TimeReportHistory] CHECK CONSTRAINT [FK_TimeReportHistory_WorkItem]
GO
ALTER TABLE [dbo].[Timetable]  WITH CHECK ADD  CONSTRAINT [FK_Timetable_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Timetable] CHECK CONSTRAINT [FK_Timetable_User]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Country] FOREIGN KEY([CountryId])
REFERENCES [Cfg].[Country] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Country]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Location]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Position] FOREIGN KEY([PositionId])
REFERENCES [Cfg].[Position] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Position]
GO
ALTER TABLE [dbo].[UserBusinessLine]  WITH CHECK ADD  CONSTRAINT [FK_UserBusinessLine_BusinessLine] FOREIGN KEY([BusinessLineId])
REFERENCES [Cfg].[BusinessLine] ([Id])
GO
ALTER TABLE [dbo].[UserBusinessLine] CHECK CONSTRAINT [FK_UserBusinessLine_BusinessLine]
GO
ALTER TABLE [dbo].[UserBusinessLine]  WITH CHECK ADD  CONSTRAINT [FK_UserBusinessLine_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserBusinessLine] CHECK CONSTRAINT [FK_UserBusinessLine_User]
GO
ALTER TABLE [dbo].[UserDepartment]  WITH CHECK ADD  CONSTRAINT [FK_UserDepartment_Department] FOREIGN KEY([DepartmentId])
REFERENCES [Cfg].[Department] ([Id])
GO
ALTER TABLE [dbo].[UserDepartment] CHECK CONSTRAINT [FK_UserDepartment_Department]
GO
ALTER TABLE [dbo].[UserDepartment]  WITH CHECK ADD  CONSTRAINT [FK_UserDepartment_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserDepartment] CHECK CONSTRAINT [FK_UserDepartment_User]
GO
ALTER TABLE [dbo].[UserInfo]  WITH CHECK ADD  CONSTRAINT [FK_UserInfo_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserInfo] CHECK CONSTRAINT [FK_UserInfo_User]
GO
ALTER TABLE [dbo].[WorkItem]  WITH CHECK ADD  CONSTRAINT [FK_WorkItem_WorkItemType] FOREIGN KEY([WorkItemTypeId])
REFERENCES [Cfg].[WorkItemType] ([Id])
GO
ALTER TABLE [dbo].[WorkItem] CHECK CONSTRAINT [FK_WorkItem_WorkItemType]
GO
ALTER TABLE [dbo].[WorkItemState]  WITH CHECK ADD  CONSTRAINT [FK_WorkItemState_WorkItem] FOREIGN KEY([WorkItemId])
REFERENCES [dbo].[WorkItem] ([Id])
GO
ALTER TABLE [dbo].[WorkItemState] CHECK CONSTRAINT [FK_WorkItemState_WorkItem]
GO
ALTER TABLE [dbo].[WorkItemState]  WITH CHECK ADD  CONSTRAINT [FK_WorkItemState_WorkItemStatus] FOREIGN KEY([WorkItemStatusId])
REFERENCES [Cfg].[WorkItemStatus] ([Id])
GO
ALTER TABLE [dbo].[WorkItemState] CHECK CONSTRAINT [FK_WorkItemState_WorkItemStatus]
GO
ALTER TABLE [dbo].[WorkItemStateHistory]  WITH CHECK ADD  CONSTRAINT [FK_WorkItemStateHistory_User] FOREIGN KEY([ChangeUserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[WorkItemStateHistory] CHECK CONSTRAINT [FK_WorkItemStateHistory_User]
GO
ALTER TABLE [dbo].[WorkItemStateHistory]  WITH CHECK ADD  CONSTRAINT [FK_WorkItemStateHistory_WorkItemState] FOREIGN KEY([WorkItemStateId])
REFERENCES [dbo].[WorkItemState] ([Id])
GO
ALTER TABLE [dbo].[WorkItemStateHistory] CHECK CONSTRAINT [FK_WorkItemStateHistory_WorkItemState]
GO
ALTER TABLE [dbo].[WorkItemStateHistory]  WITH CHECK ADD  CONSTRAINT [FK_WorkItemStateHistory_WorkItemStatus] FOREIGN KEY([WorkItemStatusId])
REFERENCES [Cfg].[WorkItemStatus] ([Id])
GO
ALTER TABLE [dbo].[WorkItemStateHistory] CHECK CONSTRAINT [FK_WorkItemStateHistory_WorkItemStatus]
GO
ALTER TABLE [Import].[OutProjectInfoPattern]  WITH CHECK ADD  CONSTRAINT [FK__OutProjectInfoPattern_ProjectId__Project_Id] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [Import].[OutProjectInfoPattern] CHECK CONSTRAINT [FK__OutProjectInfoPattern_ProjectId__Project_Id]
GO
ALTER TABLE [Import].[OutTimeReportData]  WITH CHECK ADD  CONSTRAINT [FK__OutTimeReportData_OutTimeReportInfoId__OutTimeReportInfo_Id] FOREIGN KEY([OutTimeReportInfoId])
REFERENCES [Import].[OutTimeReportInfo] ([Id])
GO
ALTER TABLE [Import].[OutTimeReportData] CHECK CONSTRAINT [FK__OutTimeReportData_OutTimeReportInfoId__OutTimeReportInfo_Id]
GO
ALTER TABLE [Import].[OutTimeReportData]  WITH CHECK ADD  CONSTRAINT [FK__OutTimeReportsData_TimeReportId__TimeReport_Id] FOREIGN KEY([TimeReportId])
REFERENCES [dbo].[TimeReport] ([Id])
GO
ALTER TABLE [Import].[OutTimeReportData] CHECK CONSTRAINT [FK__OutTimeReportsData_TimeReportId__TimeReport_Id]
GO
ALTER TABLE [Import].[OutTimeReportInfo]  WITH CHECK ADD  CONSTRAINT [FK__OutTimeReportInfo_UserId__User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [Import].[OutTimeReportInfo] CHECK CONSTRAINT [FK__OutTimeReportInfo_UserId__User_UserId]
GO
ALTER TABLE [Import].[OutUser]  WITH CHECK ADD  CONSTRAINT [FK__OutUser_UserId__User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [Import].[OutUser] CHECK CONSTRAINT [FK__OutUser_UserId__User_UserId]
GO
ALTER TABLE [Import].[OutWorkItem]  WITH CHECK ADD  CONSTRAINT [FK__OutWorkItem_WorkItemId__WorkItem_Id] FOREIGN KEY([WorkItemId])
REFERENCES [dbo].[WorkItem] ([Id])
GO
ALTER TABLE [Import].[OutWorkItem] CHECK CONSTRAINT [FK__OutWorkItem_WorkItemId__WorkItem_Id]
GO

use [FLChat]
go

/******* [Usr].[User_UpdateSegments]  ********/
CREATE OR ALTER PROCEDURE [Usr].[User_UpdateSegments] 
	-- Add the parameters for the stored procedure here
	@userId uniqueidentifier,
	@segments [dbo].[GuidList] readonly,
	@userFLNumber int = NULL,
	@removeFromPartnerOnly bit = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if @userId is NULL and @userFLNumber is not null 
	begin
	  set @userId = (select [Id] from [Usr].[User] where [FLUserNumber] = @userFLNumber);
	end

	if @userId is NULL
	  THROW 50001, 'User must be present', 1;
    
	declare @deleted int;
	declare @inserted int;

	delete from [Usr].[SegmentMember] 
	where [UserId] = @userId
	  and [SegmentId] not in (select * from @segments)
	  and (@removeFromPartnerOnly = 0 
	   or [SegmentId] in (select [Id] from [Usr].[Segment] where [PartnerName] is not null));

	set @deleted = @@ROWCOUNT;

	insert into [Usr].[SegmentMember] ([SegmentId], [UserId])
	select [Guid], @userId 
	from @segments
	where [Guid] not in (
	  select [SegmentId] from [Usr].[SegmentMember] where [UserId] = @userId);

	set @inserted = @@ROWCOUNT;

	select @deleted as [Deleted], @inserted as [Inserted];
END
GO

/******* [Usr].[Update_OwnerUserId_By_ParentFLUserNumber]	 *********/
CREATE OR ALTER PROCEDURE [Usr].[Update_OwnerUserId_By_ParentFLUserNumber]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @cnt int;

    with [ForUpd] as (
      select 
        u.[Id], 
        own.[Id] as [OwnerId]
     from [Usr].[User] u 
     inner join [Usr].[User] own on u.[ParentFLUserNumber] = own.[FLUserNumber]
     where u.[FLUserNumber] is not null 
      and (u.[OwnerUserId] is null or u.[OwnerUserId] <> own.Id)
    )
    update u
    set u.[OwnerUserId] = fu.[OwnerId]
    from [Usr].[User] as u
    inner join [ForUpd] as fu on u.[Id] = fu.[Id];

	set @cnt = @@ROWCOUNT;

    update [Usr].[User]
    set [OwnerUserId] = null
    where [FLUserNumber] is not null 
	   and [ParentFLUserNumber] is null
       and [OwnerUserId] is not null;

	set @cnt = @cnt + @@ROWCOUNT;

	select @cnt
END
GO

if TYPE_ID('[dbo].[IntGuidList]') is null
create type [dbo].[IntGuidList] as table (
  [Int] integer NOT NULL,
  [Guid] uniqueidentifier NOT NULL,
  primary key ([Int], [Guid])
)
go

/******* [Usr].[UpdateSegmentsBatch] *********/
CREATE OR ALTER PROCEDURE [Usr].[UpdateSegmentsBatch]
	@users [dbo].[IntList] readonly,
	@segments [dbo].[IntGuidList] readonly
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @deleted int;
	declare @inserted int;

	delete sm 
	from [Usr].[SegmentMember] sm
	inner join [Usr].[User] u on sm.[UserId] = u.[Id]
	inner join [Usr].[Segment] s on s.[Id] = sm.[SegmentId]
	left join @segments ins on ins.[Guid] = sm.[SegmentId] and ins.[Int] = u.[FLUserNumber]
	where s.[PartnerName] is not null
	  and u.[FLUserNumber] in (select * from @users)
	  and ins.[Guid] is null;
	  
	set @deleted = @@ROWCOUNT;

	insert into [Usr].[SegmentMember] ([UserId], [SegmentId])
	select u.[Id], ins.[Guid]
	from @segments ins
	inner join [Usr].[User] u on ins.[Int] = u.[FLUserNumber]
	left join [Usr].[SegmentMember] sm on sm.[UserId] = u.[Id] and sm.[SegmentId] = ins.[Guid]
	where sm.[UserId] is null;

	set @inserted = @@ROWCOUNT;

	select @inserted as [Inserted], @deleted as [Deleted];
END
GO

if TYPE_ID('[Usr].[UserFLImportTable]') is null
create type [Usr].[UserFLImportTable] as table (
  [SURNAME] nvarchar(250) NULL,
  [NAME] nvarchar(250) NULL,
  [PATRONYMIC] nvarchar(250) NULL,
  [BIRTHDAY] datetime2 NULL,
  [MOBILE] varchar(20) NULL,
  [EMAIL] nvarchar(250) NULL,
  [TITLE] nvarchar(250) NULL,
  [ZIP] nvarchar(20) NULL,
  [COUNTRY] nvarchar(250) NULL,
  [REGION] nvarchar(250) NULL,
  [CITY] nvarchar(250) NULL,
  [REGISTRATIONDATE] datetime2 NULL,
  [EMAILPERMISSION] smallint NULL,
  [SMSPERMISSION] smallint NULL,
  [ISDIRECTOR] smallint NULL,
  [LASTORDERDATE] datetime2 NULL,
  [LO] decimal(12,2) NULL,
  [PERIODSWOLO] decimal(12,2) NULL,
  [OLG] decimal(12,2) NULL,
  [GO] decimal(12,2) NULL,
  [CASHBACKBALANCE] decimal(12,2) NULL,
  [FLCLUBPOINTS] decimal(12,2) NULL,
  [FLCLUBPOINTSBURN] decimal(12,2) NULL,
  [ROWNUMBER] decimal(12,0) NULL,
  [CONSULTANTNUMBER] decimal(12,0) NULL,
  [MENTORNUMBER] decimal(12,0) NULL,
  [CONSULTANTSTATE] nvarchar(255) NULL
)
go

/******* [Usr].[UpdateFLUsersBatch] *********/
CREATE OR ALTER PROCEDURE [Usr].[UpdateFLUsersBatch]
	-- Add the parameters for the stored procedure here
	@table [Usr].[UserFLImportTable] readonly
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @updated int;
	declare @inserted int;
	declare @clearedPhone int;
	declare @clearedEmail int;

	declare @clearPhone [dbo].[IntList]; 
	declare @clearMail [dbo].[IntList];	

	--prepare [Dir].[Rank]
	insert into [Dir].[Rank] ([Name])
	select DISTINCT t.[TITLE] 
	from @table t
	left join [Dir].[Rank] r on t.[TITLE] = r.[Name]
	where r.[Id] is null
	  and coalesce(t.[TITLE], N'') <> N'';

	--prepare country
	insert into [Dir].[Country] ([Name])
	select DISTINCT t.[COUNTRY]
	from @table t
	left join [Dir].[Country] c on t.[COUNTRY] = c.[Name]
	where c.[Id] is null
	  and coalesce(t.[COUNTRY], N'') <> N'';

	--prepare region
	insert into [Dir].[Region] ([CountryId], [Name])
	select DISTINCT c.[Id], coalesce(t.[REGION], t.[CITY]) 
	from @table t
	inner join [Dir].[Country] c on t.[COUNTRY] = c.[Name]
	left join [Dir].[Region] r on coalesce(t.[REGION], t.[CITY]) = r.[Name] and r.[CountryId] = c.[Id]
	where r.[Id] is null
	  and (t.[REGION] is not null or t.[CITY] is not null);

	--prepare city
	insert into [Dir].[City] ([RegionId], [Name])
	select DISTINCT r.[Id], t.[CITY]
	from @table t
	inner join [Dir].[Country] c on t.[COUNTRY] = c.[Name]
	inner join [Dir].[Region] r on coalesce(t.[REGION], t.[CITY]) = r.[Name] and r.[CountryId] = c.[Id]
	left join [Dir].[City] city on t.[CITY] = city.[Name] and city.[RegionId] = r.[Id]
	where city.[Id] is null
	  and t.[CITY] is not null;

	--select users with dublicate phones in input data
	insert into @clearPhone
	select [CONSULTANTNUMBER] from (
	  select 
	    [CONSULTANTNUMBER],
		row_number() over (partition by [MOBILE] order by [ROWNUMBER]) as rn
	  from @table
	  where [MOBILE] is not null) as t
	where rn > 1;

	set @clearedPhone = @@ROWCOUNT;

	----select users from data who has dublicate phones in main table
	insert into @clearPhone
	select [CONSULTANTNUMBER]
	from @table t
	where t.[MOBILE] is not null
	  and exists (select 0 from [Usr].[User] u where t.[MOBILE] = u.[Phone] and u.[Enabled] = 1 and u.[Phone] is not null
	  and (t.[CONSULTANTNUMBER] <> u.[FLUserNumber] or u.[FLUserNumber] is null))
	  and t.[CONSULTANTNUMBER] not in (select [Value] from @clearPhone)

	--insert into @clearPhone
	--select [CONSULTANTNUMBER]
	--from @table t
	--inner join [Usr].[User] u on t.[MOBILE] = u.[Phone]
	--where t.[MOBILE] is not null
	--  and u.[Enabled] = 1
	--  and (t.[CONSULTANTNUMBER] <> u.[FLUserNumber] or u.[FLUserNumber] is null)
	--  and t.[CONSULTANTNUMBER] not in (select [Value] from @clearPhone);

	set @clearedPhone = @clearedPhone + @@ROWCOUNT;

	--select users with dublicate emails in input data
	insert into @clearMail
	select [CONSULTANTNUMBER] from (
	  select 
	    [CONSULTANTNUMBER],
		row_number() over (partition by [EMAIL] order by [ROWNUMBER]) as rn
	  from @table
	  where [EMAIL] is not null) as t
	where rn > 1;

	set @clearedEmail = @@ROWCOUNT;

	--select users from data who has dublicate emails in main table
	insert into @clearMail
	select [CONSULTANTNUMBER]
	from @table t
	where t.[EMAIL] is not null
	  and exists (select 0 from [Usr].[User] u where t.[EMAIL] = u.[Email] and u.[Enabled] = 1
	  and (t.[CONSULTANTNUMBER] <> u.[FLUserNumber] or u.[FLUserNumber] is null))
	  and t.[CONSULTANTNUMBER] not in (select [Value] from @clearMail);

	--insert into @clearMail
	--select [CONSULTANTNUMBER]
	--from @table t
	--inner join [Usr].[User] u on t.[EMAIL] = u.[Email]
	--where t.[EMAIL] is not null
	--  and u.[Enabled] = 1
	--  and (t.[CONSULTANTNUMBER] <> u.[FLUserNumber] or u.[FLUserNumber] is null)
	--  and t.[CONSULTANTNUMBER] not in (select [Value] from @clearMail);

	set @clearedEmail = @clearedEmail + @@ROWCOUNT;

    -- Insert statements for procedure here
	update u
	set 
	  [FullName] =	RTRIM(CONCAT(
	                 coalesce(t.[SURNAME] + N' ', N''),
	                 coalesce(t.[NAME] + N' ', N''),
				     coalesce(t.[PATRONYMIC] + N' ', N'')
				)),
	  [Birthday] = convert(date, t.[BIRTHDAY]),
	  [Phone] = case when cp.[Value] is null then t.[MOBILE] else null end,
	  [Email] = case when cm.[Value] is null then t.[EMAIL] else null end, 
	  [ZipCode] = t.[ZIP],
	  [RegistrationDate] = convert(date, t.[REGISTRATIONDATE]),
	  [EmailPermission] = case when t.[EMAILPERMISSION] <> 0 then 1 else 0 end,
	  [SmsPermission] = case when t.[SMSPERMISSION] <> 0 then 1 else 0 end,
	  [IsDirector] = case when t.[ISDIRECTOR] <> 0 then 1 else 0 end,
	  [LastOrderDate] = convert(date, t.[LASTORDERDATE]),
	  [LoBonusScores] = t.[LO],
	  [PeriodsWolo] = t.[PERIODSWOLO],
	  [OlgBonusScores] = t.[OLG],
	  [GoBonusScores] = t.[GO],
	  [CashBackBalance] = t.[CASHBACKBALANCE],
	  [FLClubPoints] = t.[FLCLUBPOINTS],
	  [FLClubPointsBurn] = t.[FLCLUBPOINTSBURN],
	  [ParentFLUserNumber] = t.[MENTORNUMBER],
	  [Enabled] = case when t.[CONSULTANTSTATE] = N'Удален' then 0 else 1 end,
	  [RankId] = r.[Id],
	  [CityId] = city.[Id]
	from [Usr].[User] u 
	inner join @table t on u.[FLUserNumber] = cast (t.[CONSULTANTNUMBER] as int)
	left join @clearPhone cp on t.[CONSULTANTNUMBER] = cp.[Value]
	left join @clearMail cm on t.[CONSULTANTNUMBER] = cm.[Value]
	left join [Dir].[Rank] r on t.[TITLE] = r.[Name]
	left join [Dir].[Country] country on t.[COUNTRY] = country.[Name]
	left join [Dir].[Region] region on coalesce(t.[REGION], t.[CITY]) = region.[Name] 
	                               and region.[CountryId] = country.[Id]
	left join [Dir].[City] city on t.[CITY] = city.[Name]
	                           and city.[RegionId] = region.[Id]
	where
	  coalesce([FullName], '') <>	RTRIM(CONCAT(
	                 coalesce(t.[SURNAME] + N' ', N''),
	                 coalesce(t.[NAME] + N' ', N''),
				     coalesce(t.[PATRONYMIC] + N' ', N'')
				))				 
	  or coalesce(u.[Birthday], '1753.01.01') <> convert(date, coalesce(t.[BIRTHDAY], '1753.01.01'))
	  or coalesce(u.[Phone], '') <> case when cp.[Value] is null then coalesce(t.[MOBILE], '') else '' end
	  or coalesce(u.[Email], '') <> case when cm.[Value] is null then coalesce(t.[EMAIL], '') else '' end
	  or coalesce(u.[ZipCode], '') <> coalesce(t.[ZIP], '')
	  or coalesce(u.[RegistrationDate], '1753.01.01') <> convert(date, coalesce(t.[REGISTRATIONDATE], '1753.01.01'))
	  or u.[EmailPermission] <> case when t.[EMAILPERMISSION] <> 0 then 1 else 0 end
	  or u.[SmsPermission] <> case when t.[SMSPERMISSION] <> 0 then 1 else 0 end
	  or u.[IsDirector] <> case when t.[ISDIRECTOR] <> 0 then 1 else 0 end
	  or coalesce(u.[LastOrderDate], '1753.01.01') <> convert(date, coalesce(t.[LASTORDERDATE], '1753.01.01'))
	  or coalesce(u.[LoBonusScores], -1) <> coalesce(t.[LO], -1)
	  or coalesce(u.[PeriodsWolo], -1) <> coalesce(t.[PERIODSWOLO], -1)
	  or coalesce(u.[OlgBonusScores], -1) <> coalesce(t.[OLG], -1)
	  or coalesce(u.[GoBonusScores], -1) <> coalesce(t.[GO], -1)
	  or coalesce(u.[CashBackBalance], -1) <> coalesce(t.[CASHBACKBALANCE], -1)
	  or coalesce(u.[FLClubPoints], -1) <> coalesce(t.[FLCLUBPOINTS], -1)
	  or coalesce(u.[FLClubPointsBurn], -1) <> coalesce(t.[FLCLUBPOINTSBURN], -1)
	  or coalesce(u.[ParentFLUserNumber], 0) <> coalesce(t.[MENTORNUMBER], 0)
	  or u.[Enabled] <> case when t.[CONSULTANTSTATE] = N'Удален' then 0 else 1 end
	  or coalesce(u.[RankId], 0) <> coalesce(r.[Id], 0)
	  or coalesce(u.[CityId], 0) <> coalesce(city.[Id], 0)
	  ;
	set @updated = @@ROWCOUNT;

	insert into [Usr].[User] (
	  [FullName],
	  [FLUserNumber],
	  [Birthday],
	  [Phone],
	  [Email], 
	  [ZipCode],
	  [RegistrationDate],
	  [EmailPermission],
	  [SmsPermission],
	  [IsDirector],
	  [LastOrderDate],
	  [LoBonusScores],
	  [PeriodsWolo],
	  [OlgBonusScores],
	  [GoBonusScores],
	  [CashBackBalance],
	  [FLClubPoints],
	  [FLClubPointsBurn],
	  [ParentFLUserNumber],
	  [Enabled],
	  [RankId],
	  [CityId],
	  [IsConsultant]
	) 
	select 
	  RTRIM(CONCAT(
	                 coalesce(t.[SURNAME] + N' ', N''),
	                 coalesce(t.[NAME] + N' ', N''),
				     coalesce(t.[PATRONYMIC] + N' ', N'')
				)),
	  t.[CONSULTANTNUMBER],
	  convert(date, t.[BIRTHDAY]),
	  case when cp.[Value] is null then t.[MOBILE] else null end,
	  case when cm.[Value] is null then t.[EMAIL] else null end,
	  t.[ZIP],
	  convert(date, t.[REGISTRATIONDATE]),
	  case when t.[EMAILPERMISSION] <> 0 then 1 else 0 end,
	  case when t.[SMSPERMISSION] <> 0 then 1 else 0 end,
	  case when t.[ISDIRECTOR] <> 0 then 1 else 0 end,
	  convert(date, t.[LASTORDERDATE]),
	  t.[LO],
	  t.[PERIODSWOLO],
	  t.[OLG],
	  t.[GO],
	  t.[CASHBACKBALANCE],
	  t.[FLCLUBPOINTS],
	  t.[FLCLUBPOINTSBURN],
	  t.[MENTORNUMBER],
	  case when t.[CONSULTANTSTATE] = N'Удален' then 0 else 1 end,
	  r.[Id],
	  city.[Id],
	  0 --[IsConsultant]
	from @table t
	left join [Usr].[User] u on u.[FLUserNumber] = cast (t.[CONSULTANTNUMBER] as int)
	left join @clearPhone cp on t.[CONSULTANTNUMBER] = cp.[Value]
	left join @clearMail cm on t.[CONSULTANTNUMBER] = cm.[Value]
	left join [Dir].[Rank] r on t.[TITLE] = r.[Name]
	left join [Dir].[Country] country on t.[COUNTRY] = country.[Name]
	left join [Dir].[Region] region on coalesce(t.[REGION], t.[CITY]) = region.[Name] 
	                               and region.[CountryId] = country.[Id]
	left join [Dir].[City] city on t.[CITY] = city.[Name]
	                           and city.[RegionId] = region.[Id]
	where u.[Id] is null;

	set @inserted = @@ROWCOUNT;

	--update owners
	declare @ownerUpdated int;

	update u
	set
	  [OwnerUserId] = uw.[Id]
	from [Usr].[User] u
	inner join @table t on t.[CONSULTANTNUMBER] = u.[FLUserNumber]
	left join [Usr].[User] uw on t.[MENTORNUMBER] = uw.[FLUserNumber]
	where (u.[OwnerUserId] <> uw.[Id] 
	   or (u.[OwnerUserId] is null and uw.[Id] is not null)
	   or (u.[OwnerUserId] is not null and uw.[Id] is null))
	  ;--and t.[MENTORNUMBER] is not null;
	
	set @ownerUpdated = @@ROWCOUNT;

	declare @missedOwner as [dbo].[IntList];
	declare @missedOwnerCount as int;

	insert into @missedOwner
	select [CONSULTANTNUMBER]
	from @table t
	left join [Usr].[User] u on t.[MENTORNUMBER] = u.[FLUserNumber]	
	where u.[Id] is null
	  and t.[MENTORNUMBER] is not null;

	set @missedOwnerCount = @@ROWCOUNT;

	update [Usr].[User]
	set [LastImportDate] = GETUTCDATE()
	where [FLUserNumber] in (select [CONSULTANTNUMBER] from @table);

	--totals
	select 
	  @updated as [Updated], 
	  @inserted as [Inserted], 
	  @clearedPhone as [ClearedPhone],
	  @clearedEmail as [ClearedEmail],
	  @ownerUpdated as [OwnerUpdated],
	  @missedOwnerCount as [MissedOwner];

    --rejected phone number
    select cast([CONSULTANTNUMBER] as int), [MOBILE]
	from @table
	where cast([CONSULTANTNUMBER] as int) in (select [Value] from @clearPhone);

	--rejected mail number
	select cast([CONSULTANTNUMBER] as int), [EMAIL]
	from @table
	where cast([CONSULTANTNUMBER] as int) in (select [Value] from @clearMail);

	--missed owner
	select [Value]
	from @missedOwner;
END
GO

/********* [Usr].[User_DisableNotImportedUsers] **********/
CREATE OR ALTER PROCEDURE [Usr].[User_DisableNotImportedUsers]
	-- Add the parameters for the stored procedure here
	@userId uniqueidentifier,
	@userNumber int,
	@passedHours int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if @userNumber is not null 
	begin
	  set @userId = (select [Id] from [Usr].[User] where [FLUserNumber] = @userNumber);
	  if @@ROWCOUNT = 0
      begin
	    select -1 as [Absence];
		return;
	  end
	end

    -- Insert statements for procedure here
	update u
	set [Enabled] = 0
	from [Usr].[User] u
	inner join [Usr].[User_GetChilds](@userId, default, default) c on u.[Id] = c.[UserId]
	where (u.[LastImportDate] is null or DATEDIFF(hour, u.[LastImportDate], GETUTCDATE()) > @passedHours)
	  and u.[Enabled] = 1
	  and u.[FLUserNumber] is not null;

	select @@ROWCOUNT as [Absence];
END
GO

/****** Object:  UserDefinedTableType [Usr].[UserFLImportTable]    Script Date: 29.01.2020 12:56:20 ******/
if TYPE_ID('[Usr].[ImportUsersSalesForceTable]') is null begin
PRINT '[Usr].[ImportUsersSalesForceTable]: create type'
CREATE TYPE [Usr].[ImportUsersSalesForceTable] AS TABLE(
	[SURNAME] [nvarchar](250) NULL,
	[NAME] [nvarchar](250) NULL,
	[PATRONYMIC] [nvarchar](250) NULL,
	[BIRTHDAY] [datetime2] NULL,
	[MOBILE] [nvarchar](20) NULL,
	[EMAIL] [nvarchar](250) NULL,
	[TITLE] [nvarchar](250) NULL,
	[COUNTRY] [nvarchar](250) NULL,
	[REGION] [nvarchar](250) NULL,
	[CITY] [nvarchar](250) NULL,
	[REGISTRATIONDATE] [datetime2] NULL,
	[EMAILPERMISSION] [bit] NULL,
	[SMSPERMISSION] [bit] NULL,
	[LASTORDERDATE] [datetime2] NULL,
	[FLCLUBPOINTS] [decimal](12, 2) NULL,
	[ROWNUMBER] int NULL,
	[ForeignID] [nvarchar](100) NOT NULL,
	[ForeignOwnerID] [nvarchar](100) NULL,
	[Enabled] [bit] NOT NULL default 1
)
end
GO

/********* [Usr].[ImportUsersSalesForce] **********/
CREATE OR ALTER PROCEDURE [Usr].[ImportUsersSalesForce]
	-- Add the parameters for the stored procedure here
	@table [Usr].[ImportUsersSalesForceTable] readonly
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @updated int;
	declare @inserted int;
	declare @clearedPhone int;
	declare @clearedEmail int;

	declare @clearPhone as table ([Value] nvarchar(100) NOT NULL UNIQUE); 
	declare @clearMail as table ([Value] nvarchar(100) NOT NULL UNIQUE);	

	--prepare [Dir].[Rank]
	insert into [Dir].[Rank] ([Name])
	select DISTINCT t.[TITLE] 
	from @table t
	left join [Dir].[Rank] r on t.[TITLE] = r.[Name]
	where r.[Id] is null
	  and coalesce(t.[TITLE], N'') <> N'';

	--prepare country
	insert into [Dir].[Country] ([Name])
	select DISTINCT t.[COUNTRY]
	from @table t
	left join [Dir].[Country] c on t.[COUNTRY] = c.[Name]
	where c.[Id] is null
	  and coalesce(t.[COUNTRY], N'') <> N'';

	--prepare region
	insert into [Dir].[Region] ([CountryId], [Name])
	select DISTINCT c.[Id], coalesce(t.[REGION], t.[CITY]) 
	from @table t
	inner join [Dir].[Country] c on t.[COUNTRY] = c.[Name]
	left join [Dir].[Region] r on coalesce(t.[REGION], t.[CITY]) = r.[Name] and r.[CountryId] = c.[Id]
	where r.[Id] is null
	  and (t.[REGION] is not null or t.[CITY] is not null);

	--prepare city
	insert into [Dir].[City] ([RegionId], [Name])
	select DISTINCT r.[Id], t.[CITY]
	from @table t
	inner join [Dir].[Country] c on t.[COUNTRY] = c.[Name]
	inner join [Dir].[Region] r on coalesce(t.[REGION], t.[CITY]) = r.[Name] and r.[CountryId] = c.[Id]
	left join [Dir].[City] city on t.[CITY] = city.[Name] and city.[RegionId] = r.[Id]
	where city.[Id] is null
	  and t.[CITY] is not null;

	--select users with dublicate phones in input data
	insert into @clearPhone
	select [ForeignID] from (
	  select 
	    [ForeignID],
		row_number() over (partition by [MOBILE] order by [ROWNUMBER]) as rn
	  from @table
	  where [MOBILE] is not null) as t
	where rn > 1;

	set @clearedPhone = @@ROWCOUNT;

	--select users from data who has dublicate phones in main table
	insert into @clearPhone
	select t.[ForeignID]
	from @table t
	inner join [Usr].[User] u on t.[MOBILE] = u.[Phone]
	where t.[MOBILE] is not null
	  and u.[Enabled] = 1
	  and (t.[ForeignID] <> u.[ForeignID] or u.[ForeignID] is null)
	  and t.[ForeignID] not in (select [Value] from @clearPhone);

	set @clearedPhone = @clearedPhone + @@ROWCOUNT;

	--select users with dublicate emails in input data
	insert into @clearMail
	select [ForeignID] from (
	  select 
	    [ForeignID],
		row_number() over (partition by [EMAIL] order by [ROWNUMBER]) as rn
	  from @table
	  where [EMAIL] is not null) as t
	where rn > 1;

	set @clearedEmail = @@ROWCOUNT;

	--select users from data who has dublicate emails in main table
	insert into @clearMail
	select t.[ForeignID]
	from @table t
	inner join [Usr].[User] u on t.[EMAIL] = u.[Email]
	where t.[EMAIL] is not null
	  and u.[Enabled] = 1
	  and (t.[ForeignID] <> u.[ForeignID] or u.[ForeignID] is null)
	  and t.[ForeignID] not in (select [Value] from @clearMail)

	set @clearedEmail = @clearedEmail + @@ROWCOUNT;

    -- Insert statements for procedure here
	update u
	set 
	  [FullName] =	RTRIM(CONCAT(
	                 coalesce(t.[SURNAME] + N' ', N''),
	                 coalesce(t.[NAME] + N' ', N''),
				     coalesce(t.[PATRONYMIC] + N' ', N'')
				)),
	  [Birthday] = convert(date, t.[BIRTHDAY]),
	  [Phone] = case when cp.[Value] is null then t.[MOBILE] else null end,
	  [Email] = case when cm.[Value] is null then t.[EMAIL] else null end, 
	  [RegistrationDate] = convert(date, t.[REGISTRATIONDATE]),
	  [EmailPermission] = coalesce(t.[EMAILPERMISSION], 1),
	  [SmsPermission] = coalesce(t.[SMSPERMISSION], 1),
	  [LastOrderDate] = convert(date, t.[LASTORDERDATE]),
	  [FLClubPoints] = t.[FLCLUBPOINTS],
	  [ForeignOwnerId] = t.[ForeignOwnerID],
	  [Enabled] = t.[Enabled],
	  [RankId] = r.[Id],
	  [CityId] = city.[Id]
	from [Usr].[User] u 
	inner join @table t on u.[ForeignID] = t.[ForeignID]
	left join @clearPhone cp on t.[ForeignID] = cp.[Value]
	left join @clearMail cm on t.[ForeignID] = cm.[Value]
	left join [Dir].[Rank] r on t.[TITLE] = r.[Name]
	left join [Dir].[Country] country on t.[COUNTRY] = country.[Name]
	left join [Dir].[Region] region on coalesce(t.[REGION], t.[CITY]) = region.[Name] 
	                               and region.[CountryId] = country.[Id]
	left join [Dir].[City] city on t.[CITY] = city.[Name]
	                           and city.[RegionId] = region.[Id]
	where
	  coalesce([FullName], '') <>	RTRIM(CONCAT(
	                 coalesce(t.[SURNAME] + N' ', N''),
	                 coalesce(t.[NAME] + N' ', N''),
				     coalesce(t.[PATRONYMIC] + N' ', N'')
				))				 
	  or coalesce(u.[Birthday], '1753.01.01') <> convert(date, coalesce(t.[BIRTHDAY], '1753.01.01'))
	  or coalesce(u.[Phone], '') <> case when cp.[Value] is null then coalesce(t.[MOBILE], '') else '' end
	  or coalesce(u.[Email], '') <> case when cm.[Value] is null then coalesce(t.[EMAIL], '') else '' end
	  or coalesce(u.[RegistrationDate], '1753.01.01') <> convert(date, coalesce(t.[REGISTRATIONDATE], '1753.01.01'))
	  or u.[EmailPermission] <> coalesce(t.[EMAILPERMISSION], 1)
	  or u.[SmsPermission] <> coalesce(t.[SMSPERMISSION], 1)
	  or coalesce(u.[LastOrderDate], '1753.01.01') <> convert(date, coalesce(t.[LASTORDERDATE], '1753.01.01'))
	  or coalesce(u.[FLClubPoints], -1) <> coalesce(t.[FLCLUBPOINTS], -1)
	  or coalesce(u.[ForeignOwnerId], '') <> coalesce(t.[ForeignOwnerID], '')
	  or u.[Enabled] <> t.[Enabled]
	  or coalesce(u.[RankId], 0) <> coalesce(r.[Id], 0)
	  or coalesce(u.[CityId], 0) <> coalesce(city.[Id], 0)
	  ;
	set @updated = @@ROWCOUNT;

	insert into [Usr].[User] (
	  [FullName],
	  [ForeignId],
	  [Birthday],
	  [Phone],
	  [Email], 
	  [RegistrationDate],
	  [EmailPermission],
	  [SmsPermission],
	  [LastOrderDate],
	  [FLClubPoints],
	  [ForeignOwnerId],
	  [Enabled],
	  [RankId],
	  [CityId],
	  [IsConsultant]
	) 
	select 
	  RTRIM(CONCAT(
	                 coalesce(t.[SURNAME] + N' ', N''),
	                 coalesce(t.[NAME] + N' ', N''),
				     coalesce(t.[PATRONYMIC] + N' ', N'')
				)),
	  t.[ForeignID],
	  convert(date, t.[BIRTHDAY]),
	  case when cp.[Value] is null then t.[MOBILE] else null end,
	  case when cm.[Value] is null then t.[EMAIL] else null end,
	  convert(date, t.[REGISTRATIONDATE]),
	  coalesce(t.[EMAILPERMISSION], 1),
	  coalesce(t.[SMSPERMISSION], 1),
	  convert(date, t.[LASTORDERDATE]),
	  t.[FLCLUBPOINTS],
	  t.[ForeignOwnerID],
	  t.[Enabled],
	  r.[Id],
	  city.[Id],
	  1 --[IsConsultant]
	from @table t
	left join [Usr].[User] u on u.[ForeignID] = t.[ForeignID]
	left join @clearPhone cp on t.[ForeignID] = cp.[Value]
	left join @clearMail cm on t.[ForeignID] = cm.[Value]
	left join [Dir].[Rank] r on t.[TITLE] = r.[Name]
	left join [Dir].[Country] country on t.[COUNTRY] = country.[Name]
	left join [Dir].[Region] region on coalesce(t.[REGION], t.[CITY]) = region.[Name] 
	                               and region.[CountryId] = country.[Id]
	left join [Dir].[City] city on t.[CITY] = city.[Name]
	                           and city.[RegionId] = region.[Id]
	where u.[Id] is null;

	set @inserted = @@ROWCOUNT;

	--update owners
	declare @ownerUpdated int;

	update u
	set
	  [OwnerUserId] = uw.[Id]
	from [Usr].[User] u
	inner join @table t on t.[ForeignID] = u.[ForeignID]
	left join [Usr].[User] uw on t.[ForeignOwnerID] = uw.[ForeignID]
	where (u.[OwnerUserId] <> uw.[Id] 
	   or (u.[OwnerUserId] is null and uw.[Id] is not null)
	   or (u.[OwnerUserId] is not null and uw.[Id] is null))
	  ;--and t.[MENTORNUMBER] is not null;
	
	set @ownerUpdated = @@ROWCOUNT;

	declare @missedOwner as table ([Value] nvarchar(100) NOT NULL UNIQUE);
	declare @missedOwnerCount as int;

	insert into @missedOwner
	select t.[ForeignID]
	from @table t
	left join [Usr].[User] u on t.[ForeignOwnerID] = u.[ForeignID]	
	where u.[Id] is null
	  and t.[ForeignOwnerID] is not null;

	set @missedOwnerCount = @@ROWCOUNT;

	update [Usr].[User]
	set [LastImportDate] = GETUTCDATE()
	where [ForeignID] in (select [ForeignID] from @table);

	--totals
	select 
	  @updated as [Updated], 
	  @inserted as [Inserted], 
	  @clearedPhone as [ClearedPhone],
	  @clearedEmail as [ClearedEmail],
	  @ownerUpdated as [OwnerUpdated],
	  @missedOwnerCount as [MissedOwner];

    --rejected phone number
    select [ForeignID], [MOBILE]
	from @table
	where [ForeignID] in (select [Value] from @clearPhone);

	--rejected mail number
	select [ForeignID], [EMAIL]
	from @table
	where [ForeignID] in (select [Value] from @clearMail);

	--missed owner
	select [Value]
	from @missedOwner;
END
GO

if TYPE_ID('[Usr].[ForeignIdList]') is null begin
PRINT '[Usr].[ForeignIdList]: create type'
CREATE TYPE [Usr].[ForeignIdList] as table (
	[ForeignId] nvarchar(100) NOT NULL UNIQUE
)
end
GO

if TYPE_ID('[Usr].[ForeignIdGuidList]') is null begin
PRINT '[Usr].[ForeignIdGuidList]: create type'
CREATE TYPE [Usr].[ForeignIdGuidList] as table (
	[ForeignId] nvarchar(100) NOT NULL,
	[Guid] uniqueidentifier NOT NULL
)
end
GO

/******* [Usr].[UpdateSegmentsBatchByForeignId] *********/
CREATE OR ALTER  PROCEDURE [Usr].[UpdateSegmentsBatchByForeignId]
	@users [Usr].[ForeignIdList] readonly,
	@segments [Usr].[ForeignIdGuidList] readonly
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @deleted int;
	declare @inserted int;

	delete sm 
	from [Usr].[SegmentMember] sm
	inner join [Usr].[User] u on sm.[UserId] = u.[Id]
	inner join [Usr].[Segment] s on s.[Id] = sm.[SegmentId]
	left join @segments ins on ins.[Guid] = sm.[SegmentId] and ins.[ForeignId] = u.[ForeignId]
	where s.[PartnerName] is not null
	  and u.[ForeignId] in (select * from @users)
	  and ins.[Guid] is null;
	  
	set @deleted = @@ROWCOUNT;

	insert into [Usr].[SegmentMember] ([UserId], [SegmentId])
	select u.[Id], ins.[Guid]
	from @segments ins
	inner join [Usr].[User] u on ins.[ForeignId] = u.[ForeignId]
	left join [Usr].[SegmentMember] sm on sm.[UserId] = u.[Id] and sm.[SegmentId] = ins.[Guid]
	where sm.[UserId] is null;

	set @inserted = @@ROWCOUNT;

	select @inserted as [Inserted], @deleted as [Deleted];
END
GO

/****** 2021.04.16 ******/
if SCHEMA_ID('Imp') is null
exec ('CREATE SCHEMA [Imp]')
GO

/****** 2021.04.16 ******/
/****** Table [Imp].[ImportList] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='ImportList' and [uid] = SCHEMA_ID('Imp')) 
begin
create table [Imp].[ImportList] (
  [FLUserNumber] int NOT NULL,
  [UserId] uniqueidentifier NULL,
  [ToProcess] bit NOT NULL default 1,
  [SourceCode] int NOT NULL default 0,
  [CreateDate] datetime NOT NULL DEFAULT GETUTCDATE(),
  [UploadDate] datetime NULL,
  constraint [PK__ImpImportList] primary key ([FLUserNumber])
 -- ,  constraint [FK__ImpImportList__UsrUser]
	--foreign key ([FLUserNumber])
	--references [Usr].[User] ([FLUserNumber])
)
PRINT '[Imp].[ImportList]: create table'
end
go

/****** 2021.04.16 ******/
/****** Процедура добавления в список закачки ******/
CREATE OR ALTER  procedure [Imp].[AppendList]
   @usersId [dbo].[IntList] readonly,
   @SourceCode int = 0
as
begin

DECLARE @MyTableVar [dbo].[IntList];  

UPDATE l
SET [ToProcess] = 1,  [UploadDate] = GETUTCDATE(), [SourceCode] = @SourceCode, [UserId] =  u.[Id]
OUTPUT inserted.[FLUserNumber]  
INTO @MyTableVar
from [Imp].[ImportList] l
inner join @usersId ui on l.[FLUserNumber] = ui.[Value]
left join [Usr].[User] u on u.[FLUserNumber] = l.[FLUserNumber]


insert into [Imp].[ImportList] ([FLUserNumber], [SourceCode], [ToProcess], [UserId])
select DISTINCT ui.[Value], @SourceCode, 1, u.[Id]
from @usersId ui
left join [Usr].[User] u on u.[FLUserNumber] = ui.[Value]
where ui.[Value] NOT IN (select * from @MyTableVar)
end;
GO

/****** 2021.04.16 ******/
/****** Процедура обновления списка закачки ******/
/****** Вычленение корневых пользователей и отключение всех, кто под ними ******/
CREATE OR ALTER  procedure [Imp].[UpdateList]
as
begin

--  Заполнение [UserId] по реестру пользователей
  update l set  [UserId] =  u.[Id]
  from [Imp].[ImportList] l
  inner join [Usr].[User] u on u.[FLUserNumber] = l.[FLUserNumber];

   WITH [UGP] as
   ( select DISTINCT il.[UserId] ,
    (select TOP 1 ugp.[UserId] from [Usr].[User_GetParents](il.[UserId], default) ugp
     inner join [Imp].[ImportList] ill on ugp.[UserId] = ill.[UserId]
	 order by ugp.[Deep])  as [ownerUserId]
    from [Imp].[ImportList] il
--    inner join [Usr].[User] u on (u.[Id] = il.[UserId] and u.[FLUserNumber] is not null)
    where [UserId] is not null and [ToProcess] = 1
   )

-- Всех, у кого есть наставник, пометить как неактивный для загрузки
--  update [Imp].[ImportList] set [ToProcess] = 0, [SourceCode] = 1 where [UserId] in 
  update [Imp].[ImportList] set [ToProcess] = 0 where [UserId] in 
      (select [UserId] from [UGP] where ownerUserId is not null	 );


end;
GO

/****** 2021.04.16 ******/
/****** Процедура обновления списка закачки ******/
/****** Добавлене пользователей, которые долго не обновляются ******/
CREATE OR ALTER  procedure [Imp].[AddFromUser]
  @days INT = 7
as
begin
  declare @usersId [dbo].[IntList];
  insert into @usersId select [FLUserNumber] from [Usr].[User] 
    where [OwnerUserId] is null and ISNULL([LastImportDate], DATEFROMPARTS ( 2019,03,13)) < DATEADD(day, -@days, GETUTCDATE()) and [IsBot] = 0 and [FLUserNumber] is not null;
  exec [Imp].[AppendList] @usersId, 2

end;
GO


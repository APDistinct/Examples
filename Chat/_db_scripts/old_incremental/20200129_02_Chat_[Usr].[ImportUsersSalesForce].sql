USE [FLChat]
GO

/****** Object:  StoredProcedure [Usr].[UpdateFLUsersBatch]    Script Date: 29.01.2020 17:52:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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



use [FLChat]
go

drop type if exists [Usr].[UserFLImportTable]
go

create type [Usr].[UserFLImportTable] as table (
  [SURNAME] nvarchar(250) NULL,
  [NAME] nvarchar(250) NULL,
  [PATRONYMIC] nvarchar(250) NULL,
  [BIRTHDAY] datetime2 NULL,
  [MOBILE] nvarchar(20) NULL,
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
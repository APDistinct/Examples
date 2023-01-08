use FLChat
go

/****** Table [Usr].[UserDataKey] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MessageDataKey' and [uid] = SCHEMA_ID('Msg'))
begin
CREATE TABLE [Msg].[MessageDataKey] (
  [Id] bigint NOT NULL IDENTITY(1,1),
  [Key] nvarchar(255) NOT NULL,
  constraint [PK__MsgMessageDataKey] primary key ([Id])
)
 PRINT 'CREATE TABLE [Msg].[MessageDataKey]'
end

GO

if not exists(select 1 from sysindexes where [name] = 'UNQ__MessageDataKey__Key')
CREATE UNIQUE NONCLUSTERED INDEX [UNQ__MessageDataKey__Key]
ON [Msg].[MessageDataKey]([Key]);

GO

/****** Table [Msg].[MessageData] ******/
if not exists(select 1 from sysobjects where xtype='U' 
	and [name]='MessageData' and [uid] = SCHEMA_ID('Msg'))
begin
CREATE TABLE [Msg].[MessageData] (
  [MessageId] uniqueidentifier NOT NULL,
  [KeyId] bigint NOT NULL,
  [Data] nvarchar(max) NOT NULL,
  constraint [PK__MsgMessageData] primary key ([MessageId], [KeyId]),
  constraint [FK__MsgMessageData__MsgMessage] foreign key ([MessageId])
    references [Msg].[Message] ([Id]),
  constraint [FK__MsgMessageData__MsgMessageDataKey] foreign key ([KeyId])
    references [Msg].[MessageDataKey] ([Id])
)
 PRINT 'CREATE TABLE [Msg].[MessageData]'
end
GO

CREATE OR ALTER PROCEDURE [Msg].[GetMessageDataKey]
	-- Add the parameters for the stored procedure here
	@Key nvarchar(255),
	@ID bigint out
AS
BEGIN
    --DECLARE @ID bigint
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SET @ID = (SELECT TOP 1 [ID] FROM [Msg].[MessageDataKey] WHERE [Key] = @Key);
	IF @ID IS NULL BEGIN
	  INSERT INTO [Msg].[MessageDataKey] ([Key]) VALUES (@Key);
	  SET @ID = @@IDENTITY;
	END
END
GO

CREATE OR ALTER FUNCTION [Msg].[GetMessageData]
(
	@MessageId uniqueidentifier,
	@Key nvarchar(255)
)
returns nvarchar(max)
AS
BEGIN   
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

--	DECLARE @KeyId bigint;
--	exec [Msg].[GetMessageDataKey] @Key, @KeyId = @KeyId OUTPUT;

DECLARE @Data nvarchar(max);
	SET @Data = 
(SELECT TOP 1 [Data] FROM [Msg].[MessageData] d 
inner join  [Msg].[MessageDataKey] k on(d.[KeyId] = k.[Id])
WHERE d.[MessageId] = @MessageId and k.[Key] = @Key) ;
--);
return @Data;
END
GO

CREATE OR ALTER PROCEDURE [Msg].[SetMessageData]
	-- Add the parameters for the stored procedure here
	@MessageId uniqueidentifier,
	@Key nvarchar(255),
	@Data nvarchar(max)
AS
BEGIN
   
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @KeyId bigint;
	IF @Data is null 
	begin
	  DELETE d FROM [Msg].[MessageData] d 
	  inner join  [Msg].[MessageDataKey] k on(d.[KeyId] = k.[Id])
	  WHERE d.[MessageId] = @MessageId and k.[Key] = @Key
	end
	else
	begin
	  exec [Msg].[GetMessageDataKey] @Key, @Id = @KeyId OUTPUT;
	  UPDATE [Msg].[MessageData] set [Data] = @Data where [MessageId] = @MessageId and [KeyId] = @KeyId;
 	  IF @@ROWCOUNT = 0
	  begin
 	    INSERT INTO [Msg].[MessageData] ([MessageId], [KeyId], [Data]) VALUES (@MessageId, @KeyId, @Data);
	  end
	end
END
GO

CREATE OR ALTER PROCEDURE [Msg].[DelMessageData]
	-- Add the parameters for the stored procedure here
	@MessageId uniqueidentifier,
	@Key nvarchar(255)
AS
BEGIN   
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE d FROM [Msg].[MessageData] d 
	  inner join  [Msg].[MessageDataKey] k on(d.[KeyId] = k.[Id])
	  WHERE d.[MessageId] = @MessageId and k.[Key] = @Key
END
GO

if not exists(select * from [Msg].[MessageDataKey] where [Key] = 'To-Notify-Via-Firebase')
insert into [Msg].[MessageDataKey]
([Key]) VALUES('To-Notify-Via-Firebase');
GO
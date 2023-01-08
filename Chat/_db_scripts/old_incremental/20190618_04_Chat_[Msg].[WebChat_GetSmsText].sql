use [FLChat]
go

drop function if exists [Msg].[WebChat_GetSmsText]
go

create function [Msg].[WebChat_GetSmsText](
  @sender_name nvarchar(500),
  @code nvarchar(100))
returns nvarchar(max)
as
begin

  declare @text_t nvarchar(max);
  declare @url_t nvarchar(100);
  set @text_t = (select [Value] from [Cfg].[Settings] where [Name] = 'WEB_CHAT_SMS');
  set @url_t = (select [Value] from [Cfg].[Settings] where [Name] = 'WEB_CHAT_DEEP_URL');

  return REPLACE( 
    REPLACE(@text_t, '%sender_name%', @sender_name),
	'%url%', REPLACE(@url_t, '%code%', @code));
end
go
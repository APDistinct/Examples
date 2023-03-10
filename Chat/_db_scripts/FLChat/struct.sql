USE [FLChat]
GO
/****** Object:  Table [Cfg].[ExternalTransportButton]    Script Date: 6/10/2020 7:23:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Cfg].[ExternalTransportButton](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Caption] [nvarchar](100) NOT NULL,
	[Command] [nvarchar](255) NOT NULL,
	[Row] [int] NOT NULL,
	[Col] [int] NOT NULL,
	[HideForTemporary] [bit] NOT NULL,
 CONSTRAINT [PK__CfgExternalTransportButton] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Cfg].[Settings]    Script Date: 6/10/2020 7:23:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Cfg].[Settings](
	[Name] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[Descr] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Cfg].[TransportType]    Script Date: 6/10/2020 7:23:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Cfg].[TransportType](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[VisibleForUser] [bit] NOT NULL,
	[CanSelectAsDefault] [bit] NOT NULL,
	[Prior] [tinyint] NOT NULL,
	[DeepLink] [nvarchar](255) NULL,
	[InnerTransport] [bit] NOT NULL,
	[SendGreetingMessage] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Ui].[StructureNode]    Script Date: 6/10/2020 7:23:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Ui].[StructureNode](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[ParentNodeId] [uniqueidentifier] NULL,
	[IsShowSegments] [bit] NOT NULL,
	[IsShowParentUsers] [bit] NOT NULL,
	[Order] [smallint] NOT NULL,
 CONSTRAINT [PK__UiStructureNode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Ui].[StructureNodeSegment]    Script Date: 6/10/2020 7:23:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Ui].[StructureNodeSegment](
	[NodeId] [uniqueidentifier] NOT NULL,
	[SegmentId] [uniqueidentifier] NOT NULL,
	[Order] [smallint] NOT NULL,
	[IsBelongToNode] [bit] NOT NULL,
 CONSTRAINT [PK__UiStructureNodeSegment] PRIMARY KEY CLUSTERED 
(
	[NodeId] ASC,
	[SegmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [Cfg].[ExternalTransportButton] ON 

INSERT [Cfg].[ExternalTransportButton] ([Id], [Caption], [Command], [Row], [Col], [HideForTemporary]) VALUES (8, N'Выбрать адресата', N'cmd:select_addressee', 0, 0, 1)
INSERT [Cfg].[ExternalTransportButton] ([Id], [Caption], [Command], [Row], [Col], [HideForTemporary]) VALUES (9, N'Мои баллы', N'cmd:score', 1, 0, 1)
INSERT [Cfg].[ExternalTransportButton] ([Id], [Caption], [Command], [Row], [Col], [HideForTemporary]) VALUES (10, N'Мой профиль', N'cmd:profile', 1, 1, 1)
INSERT [Cfg].[ExternalTransportButton] ([Id], [Caption], [Command], [Row], [Col], [HideForTemporary]) VALUES (11, N'Акции', N'url:https://new.faberlic.com/ru/c/1?q=%3Arelevance%3AperiodShields%3Apromo', 2, 0, 0)
INSERT [Cfg].[ExternalTransportButton] ([Id], [Caption], [Command], [Row], [Col], [HideForTemporary]) VALUES (12, N'Новый каталог', N'url:https://new.faberlic.com/ru/c/1?q=%3Arelevance%3AperiodShields%3Anew', 2, 1, 0)
INSERT [Cfg].[ExternalTransportButton] ([Id], [Caption], [Command], [Row], [Col], [HideForTemporary]) VALUES (13, N'Как заказать', N'url:https://new.faberlic.com/ru/howtoaddproducttocart', 3, 0, 0)
INSERT [Cfg].[ExternalTransportButton] ([Id], [Caption], [Command], [Row], [Col], [HideForTemporary]) VALUES (14, N'Где получить', N'url:https://new.faberlic.com/ru/delivery', 3, 1, 0)
SET IDENTITY_INSERT [Cfg].[ExternalTransportButton] OFF
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'AVATAR_DEFAULT', N'https://chat.faberlic.com/user_m.png', N'Ссылка получения аватара для неимеющих')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'COMMAND_GETAVATAR', N'users/%id%/avatar', N'Команда получения аватара')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'COMMAND_GETFILE', N'file/%id%', N'Команда получения файла')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'EMAIL_DEVINO_HTML_TEMPLATE_1', N'<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml" xmlns:v="urn:schemas-microsoft-com:vml" xmlns:o="urn:schemas-microsoft-com:office:office" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; margin: 0 auto; padding: 0; height: 100%; width: 100%;">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="x-apple-disable-message-reformatting">
    <title>Stationary - [Plain HTML]</title>
    <!--[if mso]>
        <style>
            * {
                font-family: Arial, sans-serif !important;
            }
        </style>
    <![endif]-->

        <link href="https://fonts.googleapis.com/css?family=Montserrat:300,500" rel="stylesheet">
    <style>
@media only screen and (min-device-width: 375px) and (max-device-width: 413px) {
  .email-container {
    min-width: 375px !important;
  }
}
</style>
    <style>
@media screen and (max-width: 480px) {
  .fluid {
    width: 100% !important;
    max-width: 100% !important;
    height: auto !important;
    margin-left: auto !important;
    margin-right: auto !important;
  }

  .stack-column,
.stack-column-center {
    display: block !important;
    width: 100% !important;
    max-width: 100% !important;
    direction: ltr !important;
  }

  .stack-column-center {
    text-align: center !important;
  }

  .center-on-narrow {
    text-align: center !important;
    display: block !important;
    margin-left: auto !important;
    margin-right: auto !important;
    float: none !important;
  }

  table.center-on-narrow {
    display: inline-block !important;
  }

  .email-container p {
    font-size: 14px !important;
    line-height: 18px !important;
  }
}
</style>
    <!--[if gte mso 9]>
    <xml>
        <o:OfficeDocumentSettings>
            <o:AllowPNG/>
            <o:PixelsPerInch>96</o:PixelsPerInch>
        </o:OfficeDocumentSettings>
    </xml>
    <![endif]-->

</head>
<body width="100%" bgcolor="#F1F1F1" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; mso-line-height-rule: exactly; margin: 0 auto; padding: 0; height: 100%; width: 100%;">
    <center style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; width: 100%; background: #F1F1F1; text-align: left;">

      
        <div style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; max-width: 680px; margin: auto;" class="email-container">
            <!--[if mso]>
            <table role="presentation" cellspacing="0" cellpadding="0" border="0" width="680" align="center">
            <tr>
            <td>
            <![endif]-->

            <!-- Email Body : BEGIN -->
            <table role="presentation" cellspacing="0" cellpadding="0" border="0" align="center" width="100%" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; max-width: 680px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-spacing: 0; border-collapse: collapse; table-layout: fixed; margin: 0 auto;" class="email-container">


                <!-- HEADER : BEGIN -->
                <tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
                    <td style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; mso-table-lspace: 0pt; mso-table-rspace: 0pt;">
                        <table role="presentation" cellspacing="0" cellpadding="0" border="0" width="100%" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-spacing: 0; border-collapse: collapse; table-layout: fixed; margin: 0 auto;">
                            <tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
                                <td style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding: 30px 40px 30px 40px; text-align: left; mso-table-lspace: 0pt; mso-table-rspace: 0pt;" align="left">
                                    <img src="https://new.faberlic.com/medias/new-lg-original.png?context=bWFzdGVyfGltYWdlc3w1MTE2fGltYWdlL3BuZ3xzeXMtbWFzdGVyL2ltYWdlcy9oMzEvaDkyL2gwMC84ODE3NjMzNTkxMzI2L25ld19sZ19vcmlnaW5hbC5wbmd8MjMzYzgxODU1NTEyMjE4MjNhODRhZmE0MjliNTc4OTdlYjgzNTI2MDdmZDI4YTVhNzVhMmYyMTgxYjk1YjlmOQ" width="120" alt="alt_text" border="0" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; -ms-interpolation-mode: bicubic; height: auto; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;">
</td>
</tr>
</table>
</td>
</tr>
<tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
<td bgcolor="#ffffff" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; border: 1px solid #dddddd; mso-table-lspace: 0pt; mso-table-rspace: 0pt;">
<table role="presentation" cellspacing="0" cellpadding="0" border="0" width="100%" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-spacing: 0; border-collapse: collapse; table-layout: fixed; margin: 0 auto;">
<tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
<td style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding: 20px 40px 20px 40px; font-family: sans-serif; font-size: 13px; line-height: 16px; color: #555555; text-align: left; font-weight: normal; mso-table-lspace: 0pt; mso-table-rspace: 0pt;" align="left">

<!-- Начало текста сообщения -->

<p style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; margin: 0; padding-top: 20px;">
%text%
</p>
<p > %senderfile% </p>
<!-- Конец текста сообщения -->
</td>
</tr>
<tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
<td style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding: 0px 40px 20px 40px; font-family: sans-serif; font-size: 13px; line-height: 16px; color: #555555; text-align: left; font-weight: normal; mso-table-lspace: 0pt; mso-table-rspace: 0pt;" align="left">
<p style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; margin: 0;">С уважением,</p>
</td>
</tr>
<tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
<td align="left" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding: 0px 40px 40px 40px; mso-table-lspace: 0pt; mso-table-rspace: 0pt;">

<table width="180" align="left" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-spacing: 0; border-collapse: collapse; table-layout: fixed; margin: 0 auto;">
<tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
<td width="70" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; mso-table-lspace: 0pt; mso-table-rspace: 0pt;">
<img src=%senderavatar% width="62" height="62" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; -ms-interpolation-mode: bicubic; margin: 0; padding: 0; border: none; display: block;" border="0" alt="">
</td>  
<td width="210" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; mso-table-lspace: 0pt; mso-table-rspace: 0pt;">
                                            
<table width="" cellpadding="0" cellspacing="0" border="0" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-spacing: 0; border-collapse: collapse; table-layout: fixed; margin: 0 auto;">
<tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
<td align="left" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; font-family: sans-serif; font-size: 14px; line-height: 20px; color: #222222; font-weight: bold; mso-table-lspace: 0pt; mso-table-rspace: 0pt;" class="body-text">
<p style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; font-family: "Montserrat", sans-serif; font-size: 14px; line-height: 20px; color: #222222; font-weight: bold; padding: 0; margin: 0;" class="body-text">%sendername%</p>
</td>
</tr>
<tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
<td align="left" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; font-family: sans-serif; font-size: 14px; line-height: 20px; color: #666666; mso-table-lspace: 0pt; mso-table-rspace: 0pt;" class="body-text">
<p style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; font-family: sans-serif; font-size: 14px; line-height: 20px; color: #666666; padding: 0; margin: 0;" class="body-text">%senderrank%</p>
</td>
</tr>
</table>

</td>
</tr>
</table>

</td>
</tr>

</table>
</td>
</tr>
<!-- HERO : END -->
<!-- FOOTER : BEGIN -->
<tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
<td style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; mso-table-lspace: 0pt; mso-table-rspace: 0pt;">
<table role="presentation" cellspacing="0" cellpadding="0" border="0" width="100%" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-spacing: 0; border-collapse: collapse; table-layout: fixed; margin: 0 auto;">
<tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
<td style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding: 40px 40px 10px 40px; font-family: sans-serif; font-size: 12px; line-height: 18px; color: #666666; text-align: left; font-weight: normal; mso-table-lspace: 0pt; mso-table-rspace: 0pt;" align="left">
<p style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; margin: 0;">Письмо отправлено из Faberlic Chat </p>
<p style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; margin: 0;"><a href="[Unsubscribe]">Отписаться</a> от получения Email-сообщений.</p>
</td>
</tr>

</table>
</td>
</tr>
<!-- FOOTER : END -->

</table>
<!-- Email Body : END -->

<!--[if mso]>
</td>
</tr>
</table>
<![endif]-->
</div>

</center>
</body>
</html>
', N'Шаблон для рассылки Email через Devino')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'EMAIL_SUBJECT_TEMPLATE', N'Сообщение от личного консультанта Faberlic %sendername%', N'Шаблон темы письма')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'INVITE_LINK', N'https://chat.faberlic.com/external/%code%', N'Ссылка на страницу сайта для приглашения пользователя в прямое подчинение')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'LITE_LINK_DEEP_URL', N'https://chat.faberlic.com/external/%code%', N'Шаблон ссылки на страницу, где отображаются лёгкие ссылки')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'MAINSERVER_NAME', N'https://srvchat.faberlic.com/FLChat/', N'Адрес сервера')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'ONLINE_PERIOD_SEC', N'300', N'?????? ? ???????? ? ?????????? ??????? ???????, ? ??????? ???????? ???????????? ????????? ??????')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'TEXT_CHANGE_MESSAGE_ADDRESSEE', N'Сообщения будут отправлены консультанту: %FullName%', N'Сообщение при смене ')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'TEXT_DEEPLINK_GREETING_MSG_REJECTED', N'Добрый день!
Оставайтесь на связи и получайте актуальную информацию об акциях и новинках.', N'??????????? ??? ???????????? ????????????, ??????????? ?? ??????')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'TEXT_GREETING_MSG', N'Добро пожаловать в Faberlic Chat. Общайтесь с наставниками из Faberlic, получайте советы и помощь экспертов.', N'Приветственное сообщение при подключении нового канала')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'TEXT_LITELINK_GREETING_MSG', N'Добрый день, #ФИО.', N'Приветствие для известного пользователя, перешедшего по лёгкой ссылке.')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'TEXT_LITELINK_GREETING_MSG_VIBER', N'Добрый день, #ФИО. Общайтесь с личным консультантом FABERLIC и получайте мгновенно ответы на все вопросы.', N'?????? ????? ?????????? ??? ???????????? ??????????? ?? ?????? ?????? ? ??????')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'TEXT_LITELINK_ROUTED', N'Ваш личный консультант %addressee%. Вы можете задать ему вопрос прямо сейчас.', N'Вторая часть приветствия для известного пользователя, перешедшего по лёгкой ссылке.')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'TEXT_LITELINK_UNROUTED', N'Оставайтесь на связи и получайте актуальную информацию о своих баллах и акциях.', N'??????????? ??? ?????????? ????????????, ??????????? ?? ?????? ??????, ??? ???????? ??? ?????????? ? ???????')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'TEXT_TG_START_MSG', N'Оффициальный канал Faberlic. Здесь вы можете общаться со своими наставниками', N'Сообщение на команду /start введённую старым пользователем')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'TEXT_TG_SWITCH_TO_SENDER', N'Написать ответ', N'Сообщение показывается под входящим сообщением от пользователя, который не является текущим адресатом')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'VIBER_WELCOME_MESSAGE', N'Добро пожаловать в Faberlic Chat. Общайтесь с наставниками из Faberlic, получайте советы и помощь экспертов.', N'Приветственное сообщение при подключении при открытии диалога в Viber (событие ConversationStarted)')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'VK_FILELINK_TEMPLATE', N'https://vk.com/away.php?to=#FileLink&cc_key=#FileText', N'Шаблон ссылки файла')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'WEB_CHAT_DEEP_URL', N'https://chat.faberlic.com/external/%code%', N'?????? ?????? ?? ???-???')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'WEB_CHAT_SMS', N'%sender_name% приглашает Вас в официальный чат компании Faberlic. Продолжите общение в удобном мессенджере: %url%', N'????? ??? ????????? ??? ???-????')
INSERT [Cfg].[Settings] ([Name], [Value], [Descr]) VALUES (N'WEB_CHAT_VIBER', N'#sendername, ваш личный консультант Faberlic, отправил вам сообщение:', N'Шаблон сообщения СМС для Viber')
INSERT [Cfg].[TransportType] ([Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior], [DeepLink], [InnerTransport], [SendGreetingMessage]) VALUES (0, N'FLChat', 1, 1, 1, 255, NULL, 1, 0)
INSERT [Cfg].[TransportType] ([Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior], [DeepLink], [InnerTransport], [SendGreetingMessage]) VALUES (1, N'Telegram', 1, 1, 1, 10, N'tg://resolve?domain=FaberlicChat_bot&start=%code%', 0, 0)
INSERT [Cfg].[TransportType] ([Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior], [DeepLink], [InnerTransport], [SendGreetingMessage]) VALUES (2, N'WhatsApp', 1, 1, 1, 10, NULL, 0, 0)
INSERT [Cfg].[TransportType] ([Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior], [DeepLink], [InnerTransport], [SendGreetingMessage]) VALUES (3, N'Viber', 1, 1, 1, 10, N'viber://pa?chatURI=prototypefl&context=%code%', 0, 0)
INSERT [Cfg].[TransportType] ([Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior], [DeepLink], [InnerTransport], [SendGreetingMessage]) VALUES (4, N'VK', 1, 1, 1, 10, N'https://vk.com/write-179649792?ref=%code%&ref_source=webchat', 0, 0)
INSERT [Cfg].[TransportType] ([Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior], [DeepLink], [InnerTransport], [SendGreetingMessage]) VALUES (5, N'OK', 1, 1, 1, 10, NULL, 0, 0)
INSERT [Cfg].[TransportType] ([Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior], [DeepLink], [InnerTransport], [SendGreetingMessage]) VALUES (100, N'WebChat', 1, 0, 1, 0, NULL, 1, 0)
INSERT [Cfg].[TransportType] ([Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior], [DeepLink], [InnerTransport], [SendGreetingMessage]) VALUES (150, N'Sms', 1, 0, 0, 0, NULL, 0, 0)
INSERT [Cfg].[TransportType] ([Id], [Name], [Enabled], [VisibleForUser], [CanSelectAsDefault], [Prior], [DeepLink], [InnerTransport], [SendGreetingMessage]) VALUES (151, N'Email', 1, 0, 0, 0, NULL, 0, 0)


INSERT [Ui].[StructureNode] ([Id], [Name], [ParentNodeId], [IsShowSegments], [IsShowParentUsers], [Order]) VALUES (N'00000000-0000-0000-0000-000000000000', N'Root node', NULL, 1, 0, 0)
INSERT [Ui].[StructureNode] ([Id], [Name], [ParentNodeId], [IsShowSegments], [IsShowParentUsers], [Order]) VALUES (N'51aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'Участники МП', N'00000000-0000-0000-0000-000000000000', 0, 0, 0)
INSERT [Ui].[StructureNode] ([Id], [Name], [ParentNodeId], [IsShowSegments], [IsShowParentUsers], [Order]) VALUES (N'52aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'Потребители', N'00000000-0000-0000-0000-000000000000', 0, 0, 0)
INSERT [Ui].[StructureNode] ([Id], [Name], [ParentNodeId], [IsShowSegments], [IsShowParentUsers], [Order]) VALUES (N'53aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'Новички', N'52aa3d63-6e75-e911-a2c0-9f888bb5fde6', 1, 0, 0)
INSERT [Ui].[StructureNode] ([Id], [Name], [ParentNodeId], [IsShowSegments], [IsShowParentUsers], [Order]) VALUES (N'54aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'Активные', N'52aa3d63-6e75-e911-a2c0-9f888bb5fde6', 1, 0, 0)
INSERT [Ui].[StructureNode] ([Id], [Name], [ParentNodeId], [IsShowSegments], [IsShowParentUsers], [Order]) VALUES (N'55aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'Временные акции', N'52aa3d63-6e75-e911-a2c0-9f888bb5fde6', 0, 0, 0)
INSERT [Ui].[StructureNode] ([Id], [Name], [ParentNodeId], [IsShowSegments], [IsShowParentUsers], [Order]) VALUES (N'ea784fe6-1315-ea11-a2c3-dcf6a6fc5b19', N'Неактивные', N'52aa3d63-6e75-e911-a2c0-9f888bb5fde6', 1, 0, 1)


INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'00000000-0000-0000-0000-000000000000', N'656722f6-cbc4-e911-a2c0-9f888bb5fde6', 100, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'51aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'48aa3d63-6e75-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'51aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'49aa3d63-6e75-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'51aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'824d39b8-d0c4-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'51aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'834d39b8-d0c4-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'51aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'844d39b8-d0c4-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'51aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'854d39b8-d0c4-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'51aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'864d39b8-d0c4-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'52aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'27471539-7574-ea11-a2c4-00090faa0001', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'52aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'4faa3d63-6e75-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'52aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'874d39b8-d0c4-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'53aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'20471539-7574-ea11-a2c4-00090faa0001', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'53aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'21471539-7574-ea11-a2c4-00090faa0001', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'53aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'4baa3d63-6e75-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'53aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'4caa3d63-6e75-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'53aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'4daa3d63-6e75-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'54aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'4eaa3d63-6e75-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'54aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'50aa3d63-6e75-e911-a2c0-9f888bb5fde6', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'54aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'f8df6e7b-48e4-e911-a2c1-f0a3ca6aff09', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'55aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'22471539-7574-ea11-a2c4-00090faa0001', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'55aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'23471539-7574-ea11-a2c4-00090faa0001', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'55aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'24471539-7574-ea11-a2c4-00090faa0001', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'55aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'25471539-7574-ea11-a2c4-00090faa0001', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'55aa3d63-6e75-e911-a2c0-9f888bb5fde6', N'26471539-7574-ea11-a2c4-00090faa0001', 0, 1)
INSERT [Ui].[StructureNodeSegment] ([NodeId], [SegmentId], [Order], [IsBelongToNode]) VALUES (N'ea784fe6-1315-ea11-a2c3-dcf6a6fc5b19', N'c5db4fce-0fc4-e911-a2c0-9f888bb5fde6', 0, 1)
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Transpor__737584F610B105EC]    Script Date: 6/10/2020 7:23:50 AM ******/
ALTER TABLE [Cfg].[TransportType] ADD UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER TABLE [Cfg].[ExternalTransportButton] ADD  DEFAULT ((0)) FOR [HideForTemporary]
GO
ALTER TABLE [Cfg].[TransportType] ADD  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [Cfg].[TransportType] ADD  DEFAULT ((1)) FOR [VisibleForUser]
GO
ALTER TABLE [Cfg].[TransportType] ADD  DEFAULT ((1)) FOR [CanSelectAsDefault]
GO
ALTER TABLE [Cfg].[TransportType] ADD  DEFAULT ((10)) FOR [Prior]
GO
ALTER TABLE [Cfg].[TransportType] ADD  DEFAULT ((0)) FOR [InnerTransport]
GO
ALTER TABLE [Cfg].[TransportType] ADD  DEFAULT ((0)) FOR [SendGreetingMessage]
GO
ALTER TABLE [Ui].[StructureNode] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [Ui].[StructureNode] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [ParentNodeId]
GO
ALTER TABLE [Ui].[StructureNode] ADD  DEFAULT ((0)) FOR [IsShowSegments]
GO
ALTER TABLE [Ui].[StructureNode] ADD  DEFAULT ((0)) FOR [IsShowParentUsers]
GO
ALTER TABLE [Ui].[StructureNode] ADD  DEFAULT ((0)) FOR [Order]
GO
ALTER TABLE [Ui].[StructureNodeSegment] ADD  DEFAULT ((0)) FOR [Order]
GO
ALTER TABLE [Ui].[StructureNodeSegment] ADD  DEFAULT ((1)) FOR [IsBelongToNode]
GO
ALTER TABLE [Ui].[StructureNode]  WITH CHECK ADD  CONSTRAINT [FK__UiStructureNode__UiStructureNode] FOREIGN KEY([ParentNodeId])
REFERENCES [Ui].[StructureNode] ([Id])
GO
ALTER TABLE [Ui].[StructureNode] CHECK CONSTRAINT [FK__UiStructureNode__UiStructureNode]
GO
ALTER TABLE [Ui].[StructureNodeSegment]  WITH CHECK ADD  CONSTRAINT [FK__UiStructureNodeSegment__UiStructureNode] FOREIGN KEY([NodeId])
REFERENCES [Ui].[StructureNode] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [Ui].[StructureNodeSegment] CHECK CONSTRAINT [FK__UiStructureNodeSegment__UiStructureNode]
GO
ALTER TABLE [Ui].[StructureNodeSegment]  WITH CHECK ADD  CONSTRAINT [FK__UiStructureNodeSegment__UsrSegment] FOREIGN KEY([SegmentId])
REFERENCES [Usr].[Segment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [Ui].[StructureNodeSegment] CHECK CONSTRAINT [FK__UiStructureNodeSegment__UsrSegment]
GO
ALTER TABLE [Ui].[StructureNode]  WITH CHECK ADD  CONSTRAINT [CHK__Ui_StructureNode__ParentNodeId] CHECK  (([ParentNodeId] IS NOT NULL OR [Id]='00000000-0000-0000-0000-000000000000'))
GO
ALTER TABLE [Ui].[StructureNode] CHECK CONSTRAINT [CHK__Ui_StructureNode__ParentNodeId]
GO

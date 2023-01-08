use [FLChat]
go

update [Cfg].[Settings]  set [Value]= 
'<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml" xmlns:v="urn:schemas-microsoft-com:vml" xmlns:o="urn:schemas-microsoft-com:office:office" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; margin: 0 auto; padding: 0; height: 100%; width: 100%;">
<head>
    <meta charset="utf-8"> <!-- utf-8 works for most cases -->
    <meta name="viewport" content="width=device-width"> <!-- Forcing initial-scale shouldn t be necessary -->
    <meta http-equiv="X-UA-Compatible" content="IE=edge"> <!-- Use the latest (edge) version of IE rendering engine -->
    <meta name="x-apple-disable-message-reformatting">  <!-- Disable auto-scale in iOS 10 Mail entirely -->
    <title>Stationary - [Plain HTML]</title> <!-- The title tag shows in email notifications, like Android 4.4. -->

    <!-- Web Font / @font-face : BEGIN -->
    <!-- NOTE: If web fonts are not required, lines 10 - 27 can be safely removed. -->

    <!-- Desktop Outlook chokes on web font references and defaults to Times New Roman, so we force a safe fallback font. -->
    <!--[if mso]>
        <style>
            * {
                font-family: Arial, sans-serif !important;
            }
        </style>
    <![endif]-->

    <!-- All other clients get the webfont reference; some will render the font and others will silently fail to the fallbacks. More on that here: http://stylecampaign.com/blog/2015/02/webfont-support-in-email/ -->
    <!--[if !mso]><!-->
        <link href="https://fonts.googleapis.com/css?family=Montserrat:300,500" rel="stylesheet">
    <!--<![endif]-->

    <!-- Web Font / @font-face : END -->

    <!-- CSS Reset -->
<style>
@media only screen and (min-device-width: 375px) and (max-device-width: 413px) {
  .email-container {
    min-width: 375px !important;
  }
}
</style>

<!-- Progressive Enhancements -->
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

    <!-- What it does: Makes background images in 72ppi Outlook render at correct size. -->
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
<img src=https://chat.faberlic.com/logo.png width="120" alt="alt_text" border="0" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; -ms-interpolation-mode: bicubic; height: auto; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;">
</td>
</tr>
</table>
</td>
</tr>
<!-- HEADER : END -->

<!-- HERO : BEGIN -->
<tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
<td bgcolor="#ffffff" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; border: 1px solid #dddddd; mso-table-lspace: 0pt; mso-table-rspace: 0pt;">
<table role="presentation" cellspacing="0" cellpadding="0" border="0" width="100%" style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-spacing: 0; border-collapse: collapse; table-layout: fixed; margin: 0 auto;">

<tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
<td style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding: 20px 40px 20px 40px; font-family: sans-serif; font-size: 13px; line-height: 16px; color: #555555; text-align: left; font-weight: normal; mso-table-lspace: 0pt; mso-table-rspace: 0pt;" align="left">
								
<!-- Начало текста сообщения -->
<p style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; margin: 0; padding-top: 20px;">%text%</p>
<p > %senderfile% </p>

									
<!-- Конец текста сообщения -->
</td>
</tr>
<tr style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;">
<td style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding: 0px 40px 20px 40px; font-family: sans-serif; font-size: 13px; line-height: 16px; color: #555555; text-align: left; font-weight: normal; mso-table-lspace: 0pt; mso-table-rspace: 0pt;" align="left">
<p style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; margin: 0;">Kind Regards,</p>
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
<p style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; margin: 0;">This email has been sent by your personal consultant from Vicado. Please contact the sender via messenger if necessary.</p>
<p style="-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; margin: 0;"><a href="[Unsubscribe]">Unsubscribe</a></p>
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
'
where [Name] = N'EMAIL_DEVINO_HTML_TEMPLATE_1'

go

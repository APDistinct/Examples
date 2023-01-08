using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
namespace DevinoTest
{
    public class EmailSmtpClient
    {
        public void Send()
        {
            using (var smtpClient = new SmtpClient())
            {
                var sourceEmail = "noreplay@devinotele.com";
                var subject = "Test from smtp";
                var messageText = "Привет! <a href=\"http://www.devinotele.com\">Кликни меня</a>";
                var email = "apdistinct@gmail.com";

                smtpClient.Host = "integrationapi.net";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("Faberlic_test1", "NA=BOx$GtP");

                var message = new MailMessage(sourceEmail, email) { Sender = new MailAddress(sourceEmail), Subject = subject, Body = messageText };
                try
                {
                    smtpClient.Send(message);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }
    }
}

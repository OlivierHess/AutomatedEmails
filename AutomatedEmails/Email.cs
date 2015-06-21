using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mime;

namespace DomainNameRenewal
{
    public static class Email
    {
        public static void SendRenewalEmail(String emailTo, String subject, String body)
        {
            SendEmail(emailTo, subject, body);
        }


        public static void SendRenewalEmailWithCost(String clientName, String clientEmail, String domainName, String expiryDate, Double? cost)
        {
            String subject = String.Format("Domain renewal for {0}", domainName);

            String body = String.Format("<html><body><p style=\"font-family:&quot;Calibri&quot;,sans-serif; color:black\">Dear {0},</p>", clientName);
            body += String.Format("<p style=\"font-family:&quot;Calibri&quot;,sans-serif; color:black\">I am writing to inform you that the domain name you have registered with us, {0} is due for renewal on {1}. The cost of renewal is the same as in previous years, namely &pound;{2} plus VAT for a further two years of service.</p>", domainName, expiryDate, cost);

            SendEmail(clientEmail, subject, body);            
        }

        public static void AlertAdminNoDomainCost(String domainName)
        {
            String subject = String.Format("Important: no domain cost found for {0}", domainName);

            String body = "<p>Dear Domain Admin,</p>";
            body += String.Format("<p>The automated domain name renewal system could not found a cost for {0}.</p>", domainName);
            body += "<p>Please rectify this issue asap.</p>";
            body += "<p>Thanks,</p>";
            SendEmail(ConfigurationManager.AppSettings["adminEmail"], subject, body);
        }

        public static void AlertAdminNoClientEmail(String clientName)
        {
            String subject = String.Format("Important: no email record found for client {0}", clientName);

            String body = "<p>Dear Domain Admin,</p>";
            body += String.Format("<p>The automated domain name renewal system could not found an email for {0}.</p>", clientName);
            body += "<p>Please rectify this issue asap.</p>";
            body += "<p>Thanks,</p>";
            SendEmail(ConfigurationManager.AppSettings["adminEmail"], subject, body);
        }

        public static void AlertAdminDomainAboutToExpire(String domainName, String domainExpiry)
        {
            String subject = String.Format("Important: domain {0} is about to expire", domainName);

            String body = "<p>Dear Domain Admin,</p>";
            body += String.Format("<p>The domain {0} will expire on {1}.</p>", domainName, domainExpiry);
            body += "<p>Please contact the client asap.</p>";
            body += "<p>Thanks,</p>";
            SendEmail(ConfigurationManager.AppSettings["adminEmail"], subject, body);
        }

        public static void AlertAdminException(String ex)
        {
            String subject = "Important: renewal email exe has thrown an exception";

            String body = "<p>Dear Domain Admin,</p>";
            body += String.Format("<p>The automated domain name renewal system has thrown an exception: {0}</p>", ex);
            body += "<p>Please consult the log and rectify this issue asap.</p>";
            body += "<p>Thanks,</p>";
            SendEmail(ConfigurationManager.AppSettings["adminEmail"], subject, body);
        }

        private static void SendEmail(string strTo, string strSubject, string strBody)
        {

            try
            {
                string strFrom = ConfigurationManager.AppSettings["emailFrom"];

                //for testing
                strTo = "test@test.com";
                

                MailMessage mail = new MailMessage(strFrom, strTo);
                mail.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();

                //Local settings
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "localhost";
                client.Port = 25;

                mail.Bcc.Add(ConfigurationManager.AppSettings["adminEmail"]);
                mail.Subject = strSubject;
      
                /*Add text at the footer*/
                strBody = Email.AddEmailFooter(strBody);

                /*Add logo pic at the footer*/
                AlternateView altView = AlternateView.CreateAlternateViewFromString(strBody, null, MediaTypeNames.Text.Html);
                LinkedResource logoRes = new LinkedResource(AppDomain.CurrentDomain.BaseDirectory + "logo.jpg", MediaTypeNames.Image.Jpeg);
                logoRes.ContentId = "logoID";
                altView.LinkedResources.Add(logoRes);
                mail.AlternateViews.Add(altView);

                mail.Body = strBody;
                client.Send(mail);
                Log.EmailSentLog(String.Format("Email sent to: {0}", strTo));
            }

            catch (Exception ex)
            {
                Log.WriteErrorLog(String.Format("Unable to send email: {0}", ex));
                Email.AlertAdminException(ex.Message);
            }
           
        }

        private static string AddEmailFooter(String strBody)
        {
            /*Footer*/
            strBody += "<hr size=\"1\" width=\"100%\" style=\"color:#486975\" align=\"center\">";
            strBody += "<table border=\"0\" cellpadding=\"0\">";
            strBody += "<tbody><tr><td valign=\"top\">";  
            strBody += "</td>";

            strBody += "<td valign=\"top\">";
            strBody += "</td></tr></tbody>";
            strBody += "</table>";
            strBody += "</body></html>";

            return strBody;
        }


    }
}

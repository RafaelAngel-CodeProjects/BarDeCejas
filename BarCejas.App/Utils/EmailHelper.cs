using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace BarCejas.App.Utils
{
    public class EmailHelper
    {
        public static bool EnviarEmail(string Email, string Asunto, string Cuerpo)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient();

                mail.From = new MailAddress(Email);

                mail.To.Add(Email);

                var subject = Asunto;
                mail.Subject = subject;
                mail.Body = Cuerpo;
                mail.IsBodyHtml = true;

                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
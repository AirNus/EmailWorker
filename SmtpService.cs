using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace send_mail_signa
{
    internal class SmtpService
    {
        internal static void SendMessage(MailMessage message)
        {

            SmtpClient smtpClient = new SmtpClient()
            {
                Host = "smtp.mail.ru",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(message.Sender.Address, "ezpvjvNEUkitaLTY2jMi")
                
            };

            smtpClient.Send(message);

        }
    }
}

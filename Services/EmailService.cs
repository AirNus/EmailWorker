using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using send_mail_signa.EDS;


namespace send_mail_signa.Services
{
    internal class EmailService
    {
        public MailMessage message { get; set; }

        public void CreateEmail(string emailFrom, string emailTo, string title, string text)
        {
            MailAddress fromAddress = new MailAddress(emailFrom);
            MailAddress toAddress = new MailAddress(emailTo);


            message = new MailMessage(fromAddress, toAddress)
            {
                Subject = title,
                Body = text,
                Sender = fromAddress,
            };
        }

        public void AddAttachment(string path)
        {
            message.Attachments.Add(new Attachment(path));
        }

        public void AddAttachments(string[] paths)
        {
            foreach (var path in paths)
            {
                message.Attachments.Add(new Attachment(path));
            }
        }

        public bool CheckEmailSignature(Dictionary<string, string> paths)
        {
            //Здесь можно поменять текст сообщения чтоб проверить работает ли подпись
            var textEmail = FileService.ReadFile(paths["EmailData.txt"]);
            var textEmailHash = Crypto.GetHash(textEmail);


            var textSignature = FileService.ReadSignature(paths["EmailSignature.txt"]);

            var textKey = FileService.ReadFile(paths["EmailPublicKey.txt"]);
            var newKey = JsonConvert.DeserializeObject<RSAParameters>(textKey);

            Signature signature = new Signature();
            signature._publicKey = newKey;

            var decision = signature.VerifySignature(textEmailHash, textSignature);
            return decision;
        }
    }
}

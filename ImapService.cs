using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace send_mail_signa
{
    internal class ImapService
    {
        internal static Dictionary<string,string> GetLastEmail(string sender)
        {
            var pathList = new Dictionary<string,string>();
            var emailOwner = new ImapClient();

            emailOwner.Connect("imap.mail.ru", 993, true);
            emailOwner.Authenticate("ainur.mazitov@mail.ru", "bZJxgg0fyd2Uiq1RrfYG");

            emailOwner.Inbox.Open(MailKit.FolderAccess.ReadOnly);

            var uids = emailOwner.Inbox.Search(SearchQuery.SentSince(DateTime.Now.AddDays(-7)));
            var uid = emailOwner.Inbox.FirstUnread;

            var messages = emailOwner.Inbox.Fetch(uids, MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure);

            if (messages is not null && messages.Count > 0)
            {
                var msg = messages.Where(x => x.Envelope.From.ToString() == sender && x.Index == uid).FirstOrDefault();
                if (msg != null)
                {
                    foreach (var att in msg.Attachments.OfType<BodyPartBasic>())
                    {
                        var part = (MimePart)emailOwner.Inbox.GetBodyPart(msg.UniqueId, att);

                        var attachmentsDirectory = Path.Combine(Environment.CurrentDirectory, "Emails");

                        if (!Directory.Exists(attachmentsDirectory))
                        {
                            Directory.CreateDirectory(attachmentsDirectory);
                        }

                        var path = Path.Combine(attachmentsDirectory, part.FileName);

                        if (!File.Exists(path))
                        {
                            using (var st = File.Create(path))
                            {
                                part.Content.DecodeTo(st);
                            }
                        }
                        pathList.Add(part.FileName, path);

                    }

                    return pathList;
                }
                else
                    return null;
            }
            else
                return null;
        }
    }
}

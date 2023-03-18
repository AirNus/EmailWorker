using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace send_mail_signa
{
    internal class Crypto
    {
        internal static byte[] GetHash(string data)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);

            var hash = SHA256.Create().ComputeHash(dataBytes);

            return hash;
        }
    }
}

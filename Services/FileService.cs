using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace send_mail_signa
{
    internal class FileService
    {
        internal static string ReadFile(string path)
        {
            var result = File.ReadAllText(path, Encoding.UTF8);

            return result;
        }

        internal static byte[] ReadSignature(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] buffer = new byte[fs.Length];

                fs.Read(buffer, 0, buffer.Length);

                return buffer;
            }
        }

        internal static void WriteFile(string path, string data) 
        {
            File.WriteAllText(path, data, Encoding.UTF8);
        }

        internal static void WriteFile(string path, byte[] data)
        {
            using(FileStream fs = new FileStream(path,FileMode.OpenOrCreate))
            {
                fs.Write(data, 0, data.Length);
            }
        }
    }
}

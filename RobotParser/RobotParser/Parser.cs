using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace RobotParser
{
    public class Parser
    {
        public Parser(string url, string crawlerName)
        {
            this.robotsTxt = url + "/robots.txt";
            this.crawlerName = crawlerName;

            RobotManager();
        }

        private string robotsTxt;
        private string crawlerName;
        private List<string> allowList = new List<string>();
        private List<string> disallowList = new List<string>();

        void RobotManager()
        {

            RobotReaderNoCC(robotsTxt,crawlerName);

            Console.WriteLine("Allowed: ");
            foreach (var item in allowList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Disallowed: ");
            foreach (var item in disallowList)
            {
                Console.WriteLine(item);
            }
        }

        void RobotReaderNoCC(string url, string crawler)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //request.Headers.Add("ParserBot");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    Stream receiveStream = response.GetResponseStream();

                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    using (readStream)
                    {
                        if (!RobotChooser(readStream, crawler))
                        {
                            response.Close();
                            readStream.Close();

                            RobotReaderNoCC(url, "*");
                        }
                    }

                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        bool RobotChooser(System.IO.StreamReader r, string crawlerName)
        {
            string txt;

            bool nameFound = false;
            while (!r.EndOfStream)
            {
                txt = r.ReadLine().ToLower();

                if (txt.StartsWith("user-agent: " + crawlerName))
                {
                    nameFound = true;

                    while (!String.IsNullOrEmpty(txt))
                    {
                        txt = r.ReadLine().ToLower();
                        if (txt.StartsWith("d"))
                        {
                            txt = txt.Replace("disallow: ", "");
                            disallowList.Add(txt);
                        }
                        else if (txt.StartsWith("a"))
                        {
                            txt = txt.Replace("allow: ", "");
                            allowList.Add(txt);
                        }
                    }
                }
            }
            return nameFound;
        }
    }
}

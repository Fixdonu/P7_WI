using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Microsoft.Win32;

namespace MyCrawler
{
    public class RobotTxtParser
    {
        private string crawlerName;

        public List<string> allowList = new List<string>();
        public List<string> disallowList = new List<string>();
        public List<string> rulesList = new List<string>();

        public RobotTxtParser(string crawlerName)
        {
            this.crawlerName = crawlerName;
        }

        public bool IgnoreCheck(string txt)
        {
            string[] ignoreArray = { "#", "host", "sitemap" };
            txt = txt.Replace(" ", "");

            foreach (var x in ignoreArray)
            {
                if (txt.StartsWith(x))
                {
                    return true;
                }
            }
            return false;
        }

        public void RequestRobotTxt(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = crawlerName;
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
                        ParseRobotTxt(readStream);
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

        void ParseRobotTxt(System.IO.StreamReader r)
        {
            string txt;

            while (!r.EndOfStream)
            {
                txt = r.ReadLine().ToLower();
                if (IgnoreCheck(txt))
                    continue;

                if (txt.StartsWith("user-agent: " + "*") && !r.EndOfStream)
                {
                    do
                    {
                        txt = r.ReadLine();
                        if (String.IsNullOrEmpty(txt) || IgnoreCheck(txt.ToLower()))
                            continue;

                        txt = txt.ToLower();

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
                        else
                        {
                            if (txt.StartsWith("user-agent"))
                                continue;
                            rulesList.Add(txt);
                        }
                    }
                    while (!txt.StartsWith("user-agent: ") && !r.EndOfStream);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace MyCrawler
{
    class Crawler
    {
        int crawledPages = 0;
        RobotTxtParser p;
        string seedP = "https://www.nike.com";
        string NotBaseP = "";
        LinkedList<string> frontierP = new LinkedList<string>();
        int crawlerDelay = 1000;

        public Crawler(RobotTxtParser p)
        {
            this.p = p;

            RequestPage("https://en.wikipedia.org/wiki/1986%E2%80%9387_UCLA_Bruins_men%27s_basketball_team");
        }

        void Crawl()
        {
            if (crawledPages < 40)
            {
                string val = frontierP.First.Value;
                frontierP.RemoveFirst();
                RequestPage(val);
            }
            Console.WriteLine("Done with crawling for now");

        }

        string CreateBaseP(string url)
        {
            url = url.Replace("http://", "").Replace("https://", "").Replace("www.", "");
            if (url.Contains("/"))
            {
                url = url.Remove(url.IndexOf("/"));
            }
            if (!url.Contains("."))
            {
                url = url + ".com";
            }
            url = "http://www." + url;

            return url;
        }

        void FindRobotTxt(string notBaseP)
        {
            seedP = CreateBaseP(NotBaseP);
            p.RequestRobotTxt(seedP);
        }

        void RequestPage(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "ParserBot";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream);

                    using (readStream)
                    {
                        ParsePage(readStream, url);
                    }
                    response.Close();
                    readStream.Close();
                    crawledPages++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        void ParsePage(StreamReader r, string url)
        {
            Page page = new Page(url, r.ReadToEnd());

            string regex = "<a .*?href=([\"'])(?<Link>.*?)\\1.*?>";

            Regex reg = new Regex(regex);
            MatchCollection match = reg.Matches(page.txt);

            if (match.Count > 0)
            {
                foreach (var item in match)
                {

                    string tmp = item.ToString();

                    frontierP.AddLast(tmp.Split('"')[1]);
                    Console.WriteLine(WebUtility.UrlDecode(tmp.Split('"')[1]));
                }
            }
            Crawl();
        }
    }
}

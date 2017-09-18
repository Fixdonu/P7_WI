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

        LinkParser lp;
        int crawledPages = 0;
        RobotTxtParser rtp;
        string seedP = "https://www.nike.com";
        string NotBaseP = "";
        string crawlerName;
        LinkedList<string> frontierP = new LinkedList<string>();
        int crawlerDelay = 1000;

        public Crawler(RobotTxtParser rtp, string crawlerName)
        {
            this.crawlerName = crawlerName;
            this.rtp = rtp;
            lp = new LinkParser(rtp);
            frontierP.AddLast("http://www.chilkatsoft.com");
            Crawl();
        }

        void Crawl()
        {
            //while (crawledPages < 40 || frontierP.Count == 0)
            {
                string val = frontierP.First.Value;
                frontierP.RemoveFirst();
                RequestPage(val);
                Console.WriteLine("Crawled: " + crawledPages);
                System.Threading.Thread.Sleep(crawlerDelay);
            }
            Console.WriteLine(frontierP.Count);

            for (int i = 0; i < frontierP.Count; i++)
            {
                Console.WriteLine(frontierP.ToString());
            }

            Console.WriteLine("Done with crawling for now");
        }

        void RequestPage(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = crawlerName;
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
            //string regex2 = "href =\"[a-zA-Z./:&\\d_-]+\"";

            Regex reg = new Regex(regex);
            MatchCollection matches = reg.Matches(page.txt);

            if (matches.Count > 0)
            {
                foreach (Match item in matches)
                {

                    string extractedUrl = WebUtility.UrlDecode(item.ToString().Split('"')[1]);

                    lp.ParseLink(page, extractedUrl);
                    //Console.WriteLine(extractedUrl);
                }
            }
        }
    }
}

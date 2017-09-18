﻿using System;
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

        string crawlerName;
        LinkedList<string> frontierP = new LinkedList<string>();
        int crawlerDelay = 1000;

        public Crawler(RobotTxtParser rtp, string crawlerName)
        {
            this.crawlerName = crawlerName;
            this.rtp = rtp;
            lp = new LinkParser(rtp);
            frontierP.AddLast("http://wikipedia.org");
            Crawl();
        }

        void Crawl()
        {
            while (crawledPages < 40 || frontierP.Count == 0)
            {
                string url = frontierP.First.Value;
                frontierP.RemoveFirst();

                //Verify with robot.txt before requesting entry
                Console.WriteLine("verifing robot.txt: " + url);
                if (!rtp.IsDisallowed(url))
                {
                    System.Threading.Thread.Sleep(crawlerDelay);
                    RequestPage(url);
                }

                Console.WriteLine("Crawled: " + crawledPages);
                System.Threading.Thread.Sleep(crawlerDelay);
            }        

            Console.WriteLine(frontierP.Count);

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

            string parsedLink;
            string regex = "<a .*?href=([\"'])(?<Link>.*?)\\1.*?>";
            //string regex2 = "href =\"[a-zA-Z./:&\\d_-]+\"";

            Regex reg = new Regex(regex);
            MatchCollection matches = reg.Matches(page.txt);

            if (matches.Count > 0)
            {
                foreach (Match item in matches)
                {
                    string extractedUrl = WebUtility.UrlDecode(item.ToString().Split('"')[1]);
                    parsedLink = lp.ParseLink(page, extractedUrl);

                    if (!String.IsNullOrWhiteSpace(parsedLink))
                    {
                        frontierP.AddLast(parsedLink);
                    }
                    
                }

                System.Threading.Thread.Sleep(1000);
            }

        }
    }
}

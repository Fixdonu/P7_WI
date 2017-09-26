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
        int n = 0;
        string crawlerName;
        LinkedList<string> frontierP = new LinkedList<string>();
        int crawlerDelay = 1000;

        public Crawler(RobotTxtParser rtp, string crawlerName)
        {
            this.crawlerName = crawlerName;
            this.rtp = rtp;
            lp = new LinkParser(rtp);
            frontierP.AddLast("http://cknotes.com");
            //Crawl();
            HTMLExterminator();
        }

        void Crawl()
        {
            while (crawledPages < 1000 || frontierP.Count == 0)
            {
                string url = frontierP.First.Value;
                frontierP.RemoveFirst();

                //Verify with robot.txt before requesting entry
                Console.WriteLine("verifing robot.txt: " + url);
                if (!rtp.IsDisallowed(lp.FindRobotTxt(url)))
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
                //request.Credentials
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
                    response.Dispose();
                    readStream.Dispose();
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

            string path = @"..\..\Resources\RawCrawl";
            string filename = @"\file" + n + url.Replace(":", "").Replace("/", "") + ".txt";
            Console.WriteLine("creating file:" + path + filename);
            if (!File.Exists(path + filename))
            {
                // Create a file to write to.
                string createText = page.txt;
                File.WriteAllText(path + filename, createText);

            }

            n++;
            string parsedLink;
            string regex = "<a .*?href=([\"'])(?<Link>.*?)\\1.*?>";

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
        
        
        void HTMLExterminator()
        {
            string rawPath = @"..\..\Resources\RawCrawl";
            //string rawPath = @"C:\Users\Ejer\Documents\GitHub\P7_WI\MyCrawler\MyCrawler\Resources\RawCrawl";
            string[] rawFiles = Directory.GetFiles(rawPath);
            string noHTMLPath = @"..\..\Resources\NoHTML";
            //string noHTMLPath = @"C:\Users\Ejer\Documents\GitHub\P7_WI\MyCrawler\MyCrawler\Resources\NoHTML";
            
            Regex regexRipHTML = new Regex(" < body(?:[ ][^>]*)?>(?<contents>.*?)</body>",
                                 RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

            string currentDoc;
            int i = 0;
            foreach (var x in rawFiles)
            {
                Console.WriteLine(x); 
                using (StreamReader sr = new StreamReader(x))
                {
                    currentDoc = sr.ReadToEnd();
                    currentDoc = regexRipHTML.Replace(currentDoc, " ");

                    //if (!File.Exists(noHTMLPath + @"\" + Path.GetFileName(x)))
                    //{
                        Console.WriteLine("entered");
                        Console.WriteLine(noHTMLPath + @"\" + Path.GetFileName(x));
                        //File.WriteAllText(@"C:\Users\Ejer\Documents\GitHub\P7_WI\MyCrawler\MyCrawler\Resources\NoHTML\text" + i, currentDoc);
                        File.WriteAllText(noHTMLPath + @"\" + i +Path.GetFileName(x), currentDoc);
                        System.Threading.Thread.Sleep(599);
                    //}

                    sr.Close();
                    sr.Dispose();
                }
                i++;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Configuration;

namespace MyCrawler
{
    class LinkParser
    {
        public LinkParser(RobotTxtParser rtp)
        {
            this.rtp = rtp;
        }

        RobotTxtParser rtp;
        //string linkRegex = "<a .*?href=([\"'])(?<Link>.*?)\\1.*?>";

        List<string> goodUrls = new List<string>();
        List<string> newUrls;

        public List<string> GoodUrls { get; }
        

        string CreateBaseP(string url)
        {
            url = url.Replace("http://", "").Replace("https://", "").Replace("www.", "");
            if (url.Contains("/"))
            {
                url = url.Remove(url.IndexOf("/"));
            }
            if (!url.Contains("."))
            {
                url = url + ".com/";
            }
            url = "http://www." + url;

            return url;
        }

        public string FindRobotTxt(string notBaseP)
        {
            string basePage = CreateBaseP(notBaseP);
            return (basePage + "/robots.txt");
        }

        public string ParseLink(Page page, string sourceUrl)
        {
            //MatchCollection matches = Regex.Matches(page.txt, linkRegex);
            string parsedLink = "";

            if (sourceUrl == null)
            {

            }
            if (rtp.IgnoreCheck(sourceUrl))
            {

            }
            else if (sourceUrl.Contains(".asp") || sourceUrl.Contains("@") || sourceUrl.EndsWith(".jpg"))
            {

            }
            else
            {

                if (!goodUrls.Contains(sourceUrl))
                {
                    if (IsExternalUrl(sourceUrl))
                    {

                        goodUrls.Add(sourceUrl);
                        parsedLink = sourceUrl;
                        Console.WriteLine(sourceUrl + " added to the frontier");
                    }
                    else if (!IsAWebPage(sourceUrl))
                    {

                    }
                    else
                    {
                        Console.WriteLine(" ");
                        Console.WriteLine("Combined: " + CreateBaseP(page.url) + sourceUrl);
                        Console.WriteLine(sourceUrl);
                        Console.WriteLine(CreateBaseP(page.url));
                        
                        goodUrls.Add(CreateBaseP(page.url) + sourceUrl);
                        parsedLink = CreateBaseP(page.url) + sourceUrl;
                    }
                }
            }

            return parsedLink;
        }

        bool IsExternalUrl(string url)
        {
            if (url.Substring(0, 7) == "http://" || url.Substring(0, 3) == "www" || url.Substring(0, 7) == "https://")
            {
                return true;
            }

            return false;
        }

        bool IsAWebPage(string url)
        {
            if (url.IndexOf("javascript:") == 0)
                return false;

            string extension = url.Substring(url.LastIndexOf(".") + 1, url.Length - url.LastIndexOf(".") - 1);
            switch (extension)
            {
                case "jpg":
                case "css":
                    return false;
                default:
                    return true;
            }
        }
    }
}

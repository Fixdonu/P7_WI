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
                url = url + ".com";
            }
            url = "http://www." + url + "/";

            return url;
        }

        void FindRobotTxt(string notBaseP)
        {
            string basePage = CreateBaseP(notBaseP);
            //rtp.RequestRobotTxt(basePage);
        }

        public string ParseLink(Page page, string sourceUrl)
        {
            //MatchCollection matches = Regex.Matches(page.txt, linkRegex);
            string parsedLink = "";

            if (sourceUrl == null)
            {

            }
            else if (sourceUrl.Contains(".asp") || sourceUrl.Contains("@"))
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
                    }
                    else if (!IsAWebPage(sourceUrl))
                    {

                    }
                    else
                    {
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

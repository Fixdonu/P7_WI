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
        string linkRegex = "<a .*?href=([\"'])(?<Link>.*?)\\1.*?>";
        //string linkRegex = "href=\"[a-zA-Z./:&\\d_-]+\"";
        List<string> goodUrls = new List<string>();

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
            string basePage = CreateBaseP(notBaseP);
            rtp.RequestRobotTxt(basePage);
        }

        public void ParseLink (Page page, string sourceUrl)
        {
            MatchCollection matches = Regex.Matches(page.txt, linkRegex);

            foreach  (Match item in matches)
            {
                if (item.Value == string.Empty)
                {
                    // Bad url
                    continue;
                }

                if (item.Value.Contains(".asp"))
                {
                    continue;
                }

                string foundHref = null;

                try
                {
                    foundHref = item.Value.Replace("href=\"", "");
                    foundHref = foundHref.Substring(0, foundHref.IndexOf("\""));
                }
                catch (Exception e)
                {
                    //exceptions.Add(e.Message);
                }

                if (!goodUrls.Contains(foundHref))
                {
                    if (IsExternalUrl(foundHref))
                    {
                        goodUrls.Add(foundHref);
                    }
                    else if (!IsAWebPage(foundHref))
                    {

                    }
                    else
                    {
                        goodUrls.Add(CreateBaseP(page.url)+ sourceUrl);
                    }
                }
            }

            foreach (var item in goodUrls)
            {
                Console.WriteLine(item);
            }

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

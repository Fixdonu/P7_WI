using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MyCrawler
{
    class LinkParser
    {
        LinkParser() {}

        //string linkRegex = "<a .*?href=([\"'])(?<Link>.*?)\\1.*?>";
        string linkRegex = "href=\"[a-zA-Z./:&\\d_-]+\"";
        List<string> goodUrls = new List<string>();
        List<string> externalUrls = new List<string>();
        List<string> exceptions = new List<string>();



        void ParseLink (Page page, string sourceUrl)
        {
            MatchCollection matches = Regex.Matches(page.txt, linkRegex);

            foreach  (Match item in matches)
            {
                if (item.Value == string.Empty)
                {
                    // Bad string
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
                    exceptions.Add(e.Message);
                }

                if (!goodUrls.Contains(foundHref))
                {
                    if (IsExternalUrl(foundHref))
                    {
                        externalUrls.Add(foundHref);
                    }
                    else if (!IsAWebPage(foundHref))
                    {

                    }
                    else
                    {
                        goodUrls.Add(foundHref);
                    }
                }
            }
        }

        bool IsExternalUrl(string url)
        {


            return false;
        }

        bool IsAWebPage(string url)
        {
            return false;
        }





    }
}

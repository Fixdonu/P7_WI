using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCrawler
{
    class Tester
    {
        public Tester(string url)
        {
            this.url = url;

            Console.WriteLine(CreateBaseP(url));
        }

        string url;




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


    }
}

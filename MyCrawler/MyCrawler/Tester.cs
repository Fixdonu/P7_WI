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
            IsContained(url);
            //Console.WriteLine(CreateBaseP(url));
           
            
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
        void IsContained(string url)
        {
            bool isDisallowed = false;
            List<string> disallowList = new List<string>();
            disallowList.Add("/ajax/");
            disallowList.Add("/album.php");
            //disallowList.Add("/");



            if (disallowList != null || disallowList.Count != 0)
            {
                foreach (string item in disallowList)
                {
                    if (url.Contains(item))
                    {
                        isDisallowed = true;
                        break;
                    }
                }
            }
            Console.WriteLine(isDisallowed);
        }


    }
}

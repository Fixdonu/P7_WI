using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCrawler
{
    public class Page
    {
        public string url;
        public string txt;


        public Page(string url, string txt)
        {
            this.url = url;
            this.txt = txt;
        }
    }
}

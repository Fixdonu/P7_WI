using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            string test = "https://en.wikipedia.org/wiki/Pezizales";
            Tester t = new Tester(test);
            */
            
            string crawlerName = "sw709Bot";

            RobotTxtParser rtp = new RobotTxtParser(crawlerName);
            Crawler c = new Crawler(rtp, crawlerName);

        }
    }
}

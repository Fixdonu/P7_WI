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
            RobotTxtParser rtp = new RobotTxtParser("Sw709");
            Crawler c = new Crawler(rtp);
        }
    }
}

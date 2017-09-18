using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotParser
{
    class Program
    {
        static void Main(string[] args)
        {
          Console.WriteLine("Insert target");
          string target = Console.ReadLine();
          Parser p = new Parser(target, "googlebot-ihrrhrmage");
        }
    }
}


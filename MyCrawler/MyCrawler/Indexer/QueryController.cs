using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCrawler
{
    class QueryController
    {
        LangProcessor lp;

        public QueryController(LangProcessor lp)
        {
            this.lp = lp;

            string q = Console.ReadLine();
            BinQuery(q);

        }

        void BinQuery(string query)
        {
            string[] tokens = query.Split(' ');

            HashSet<int> docListPos = new HashSet<int>();
            HashSet<int> docListNeg = new HashSet<int>();

            HashSet<int> res = new HashSet<int>();

            Int64 res2 = rec(tokens, 0);



            


            Console.WriteLine("Allowed: ");
            foreach (var item in docListPos)
            {
                Console.Write(item + " ");
            }

            Console.WriteLine("Disallowed: ");
            foreach (var item in docListNeg)
            {
                Console.Write(item + " ");
            }

            //remove docs from pos if they are on neg
            docListPos.ExceptWith(docListNeg);

            Console.WriteLine("Removed: ");
            foreach (var item in docListPos)
            {
                Console.Write(item + " ");
            }

            Console.ReadLine();
            
        }

        int rec(string[] tokens, int i)
        {
            if (tokens.Count() > 1)
            {
                return (rec(tokens, i + 1) & Int32.Parse(tokens[i]));
            }
            else return Int32.Parse(tokens[i]);
        }


    }
}

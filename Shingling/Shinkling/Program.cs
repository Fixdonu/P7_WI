using System;
using System.Collections.Generic;
using System.Linq;

namespace Shinkling
{
    class Program
    {
        static void Main(string[] args)
        {
            string s1 = "do not worry about your difficulties in mathematics";
            string s2 = "i would not worry about your difficulties you can easily learn what is needed";
            List<string> tmp1 = Shingling(3, s1);
            List<string> tmp2 = Shingling(3, s2);

            #region

            List<int> tmp1int = new List<int>();
            List<int> tmp2int = new List<int>();

            foreach (var item in tmp1)
            {
                tmp1int.Add(SimpleHash(item, 7, 31));
            }
            foreach (var item in tmp2)
            {
                tmp2int.Add(SimpleHash(item, 7, 31));
            }


            List<int> tmp21int = new List<int>();
            List<int> tmp22int = new List<int>();

            foreach (var item in tmp1)
            {
                tmp21int.Add(SimpleHash(item, 4, 12));
            }
            foreach (var item in tmp2)
            {
                tmp22int.Add(SimpleHash(item, 4, 12));
            }

            List<int> tmp31int = new List<int>();
            List<int> tmp32int = new List<int>();

            foreach (var item in tmp1)
            {
                tmp31int.Add(SimpleHash(item, 25, 42));
            }
            foreach (var item in tmp2)
            {
                tmp32int.Add(SimpleHash(item, 25, 42));
            }

            List<int> tmp41int = new List<int>();
            List<int> tmp42int = new List<int>();

            foreach (var item in tmp1)
            {
                tmp41int.Add(SimpleHash(item, 5, 13));
            }
            foreach (var item in tmp2)
            {
                tmp42int.Add(SimpleHash(item, 5, 13));
            }

            #endregion

            List<int> minList1 = new List<int>();
            List<int> minList2 = new List<int>();

            minList1.Add(tmp1int.Min());
            minList1.Add(tmp21int.Min());
            minList1.Add(tmp31int.Min());
            minList1.Add(tmp41int.Min());

            minList2.Add(tmp2int.Min());
            minList2.Add(tmp22int.Min());
            minList2.Add(tmp32int.Min());
            minList2.Add(tmp42int.Min());

            Console.WriteLine(Jaccard(minList1, minList2));

            Console.ReadLine();
        }

        static List<string> Shingling(int noShingles, string s)
        {
            List<string> listShingles = new List<string>();
            string[] sSplit = s.ToLower().Split(' ');

            for (int i = 0; i < sSplit.Count()-noShingles+1; i++)
            {
                string tmp = "";
                for (int j = 0; j < noShingles; j++)
                {
                    tmp += sSplit[i+j] + ' ';
                }
                tmp.TrimEnd(' ');
                listShingles.Add(tmp);
            }
            return listShingles;
        }

        static double Jaccard(List<int> l1, List<int> l2)
        {
            int ret = 0;

            int minListLength = Math.Min(l1.Count, l2.Count);

            for (int i = 0; i < minListLength; i++)
            {
                if (l1[i] == l2[i])
                {
                    ret += 1;
                }
            }

            return (double)ret/minListLength;
        }

        static int SimpleHash(string shingle, int hash, int mult)
        {
            int tmp = hash;
            for (int i = 0; i < shingle.Length; i++)
            {
                tmp = tmp * mult + shingle[i];
            }
            return tmp;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyCrawler
{
    class LangProcessor
    {
        Dictionary<string, int> stopDict;

        public LangProcessor(string fp)
        {
            stopDict =  new Dictionary<string, int>();
            CreateStopDict();
            RemoveStopWord(TokenFromFile(fp));
        }

        void CreateStopDict()
        {
            stopDict.Add("a", 0);
            stopDict.Add("about", 1);
            stopDict.Add("above", 2);
            stopDict.Add("after", 3);
            stopDict.Add("again", 4);
            stopDict.Add("against", 5);
            stopDict.Add("all", 6);
            stopDict.Add("am", 7);
            stopDict.Add("an", 8);
            stopDict.Add("and", 9);
            stopDict.Add("any", 10);
            stopDict.Add("are", 11);
            stopDict.Add("aren't", 12);
            stopDict.Add("as", 13);
            stopDict.Add("at", 14);
            stopDict.Add("be", 15);
            stopDict.Add("because", 16);
            stopDict.Add("been", 17);
            stopDict.Add("before", 18);
            stopDict.Add("being", 19);
            stopDict.Add("below", 20);
            stopDict.Add("between", 21);
            stopDict.Add("both", 22);
            stopDict.Add("but", 23);
            stopDict.Add("by", 24);
            stopDict.Add("can't", 25);
            stopDict.Add("cannot", 26);
            stopDict.Add("could", 27);
            stopDict.Add("couldn't", 28);
            stopDict.Add("did", 29);
            stopDict.Add("didn't", 30);
            stopDict.Add("do", 31);
            stopDict.Add("does", 32);
            stopDict.Add("doesn't", 33);
            stopDict.Add("doing", 34);
            stopDict.Add("don't", 35);
            stopDict.Add("down", 36);
            stopDict.Add("during", 37);
            stopDict.Add("each", 38);
            stopDict.Add("few", 39);
            stopDict.Add("for", 40);
            stopDict.Add("from", 41);
            stopDict.Add("further", 42);
            stopDict.Add("had", 43);
            stopDict.Add("hadn't", 44);
            stopDict.Add("has", 45);
            stopDict.Add("hasn't", 46);
            stopDict.Add("have", 47);
            stopDict.Add("haven't", 48);
            stopDict.Add("having", 49);
            stopDict.Add("he", 50);
            stopDict.Add("he'd", 51);
            stopDict.Add("he'll", 52);
            stopDict.Add("he's", 53);
            stopDict.Add("her", 54);
            stopDict.Add("here", 55);
            stopDict.Add("here's", 56);
            stopDict.Add("hers", 57);
            stopDict.Add("herself", 58);
            stopDict.Add("him", 59);
            stopDict.Add("mustn't", 60);
            stopDict.Add("my", 61);
            stopDict.Add("myself", 62);
            stopDict.Add("no", 63);
            stopDict.Add("nor", 64);
            stopDict.Add("not", 65);
            stopDict.Add("of", 66);
            stopDict.Add("off", 67);
            stopDict.Add("on", 68);
            stopDict.Add("once", 69);
            stopDict.Add("only", 70);
            stopDict.Add("or", 71);
            stopDict.Add("other", 72);
            stopDict.Add("ought", 73);
            stopDict.Add("our", 74);
            stopDict.Add("ours", 75);
            stopDict.Add("ourselves", 76);
            stopDict.Add("out", 77);
            stopDict.Add("over", 78);
            stopDict.Add("own", 79);
            stopDict.Add("same", 80);
            stopDict.Add("shan't", 81);
            stopDict.Add("she", 82);
            stopDict.Add("she'd", 83);
            stopDict.Add("she'll", 84);
            stopDict.Add("she's", 85);
            stopDict.Add("should", 86);
            stopDict.Add("shouldn't", 87);
            stopDict.Add("so", 88);
            stopDict.Add("some", 89);
            stopDict.Add("such", 90);
            stopDict.Add("than", 91);
            stopDict.Add("that", 92);
            stopDict.Add("that's", 93);
            stopDict.Add("the", 94);
            stopDict.Add("their", 95);
            stopDict.Add("theirs", 96);
            stopDict.Add("them",97);
            stopDict.Add("themselves",98);
            stopDict.Add("then",99);
            stopDict.Add("there",100);
            stopDict.Add("there's",101);
            stopDict.Add("these",102);
            stopDict.Add("they",103);
            stopDict.Add("they'd", 104);
            stopDict.Add("they'll",105);
            stopDict.Add("they're", 106);
            stopDict.Add("they've", 107);
            stopDict.Add("this", 108);
            stopDict.Add("those", 109);
            stopDict.Add("through", 110);
            stopDict.Add("to", 111);
            stopDict.Add("too", 112);
            stopDict.Add("under", 113);
            stopDict.Add("until", 114);
            stopDict.Add("up", 115);
            stopDict.Add("very", 116);
            stopDict.Add("was", 117);
            stopDict.Add("wasn't", 118);
            stopDict.Add("we", 119);
            stopDict.Add("we'd", 120);
            stopDict.Add("we'll", 121);
            stopDict.Add("we're", 122);
            stopDict.Add("we've", 123);
            stopDict.Add("were", 124);
            stopDict.Add("weren't", 125);
            stopDict.Add("what", 126);
            stopDict.Add("what's", 127);
            stopDict.Add("when", 128);
            stopDict.Add("when's", 129);
            stopDict.Add("where", 130);
            stopDict.Add("where's", 131);
            stopDict.Add("which", 132);
            stopDict.Add("while", 133);
            stopDict.Add("who", 134);
            stopDict.Add("who's", 135);
            stopDict.Add("whom", 136);
            stopDict.Add("why", 137);
            stopDict.Add("why's", 138);
            stopDict.Add("with", 139);
            stopDict.Add("won't", 140);
            stopDict.Add("would", 141);
            stopDict.Add("wouldn't", 142);
            stopDict.Add("you", 143);
            stopDict.Add("you'd", 144);
            stopDict.Add("you'll", 145);
            stopDict.Add("you're", 146);
            stopDict.Add("you've", 147);
            stopDict.Add("your", 148);
            stopDict.Add("yours", 149);
            stopDict.Add("yourself", 150);
            stopDict.Add("yourselves", 151);
        }

        string[] TokenFromFile(string filepath)
        {
            string[] tokenList = File.ReadAllText(filepath).Split(' ');

            return tokenList;
        }

        void RemoveStopWord(string[] tokenList)
        {
            List<string> noStopTokenList = new List<string>();
            int v = 0;
            foreach  (string item in tokenList)
            {
                if(stopDict.TryGetValue(item, out v))
                {
                }
                noStopTokenList.Add(item);
            }
        }

    }
}

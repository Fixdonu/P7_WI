using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Microsoft.Win32;

namespace RobotParser
{
  public class Parser
  {
    public Parser(string url, string crawlerName)
    {
      this.robotsTxt = url + "/robots.txt";
      this.crawlerName = crawlerName;

      RobotManager();
    }

    private string robotsTxt;
    private string crawlerName;
    private List<string> allowList = new List<string>();
    private List<string> disallowList = new List<string>();
    private List<string> rulesList = new List<string>();

    void RobotManager()
    {

      RobotReaderNoCC(robotsTxt, crawlerName);

      if (allowList.Count > 0)
      {
        Console.WriteLine("Allowed: ");
        foreach (var item in allowList)
          Console.WriteLine(item);
      }

      if (disallowList.Count > 0)
      {
        Console.WriteLine("Disallowed: ");
        foreach (var item in disallowList)
          Console.WriteLine(item);
      }

      if (rulesList.Count > 0)
      {
        Console.WriteLine("Rules: ");
        foreach (var item in rulesList)
          Console.WriteLine(item);
      } 
    }

    void RobotReaderNoCC(string url, string crawler)
    {
      try
      {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.UserAgent = "ParserBot";
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();


        if (response.StatusCode == HttpStatusCode.OK)
        {

          Stream receiveStream = response.GetResponseStream();

          StreamReader readStream = null;

          if (response.CharacterSet == null)
          {
            readStream = new StreamReader(receiveStream);
          }
          else
          {
            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
          }

          using (readStream)
          {
            if (!RobotChooser(readStream, crawler))
            {
              response.Close();
              readStream.Close();

              RobotReaderNoCC(url, "*");
            }
          }

          response.Close();
          readStream.Close();
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }

    bool RobotChooser(System.IO.StreamReader r, string crawlerName)
    {
      string txt;

      bool nameFound = false;
      while (!r.EndOfStream)
      {
        txt = r.ReadLine().ToLower();
        if (txt.StartsWith("#"))
          continue;

        if (txt.StartsWith("user-agent: " + crawlerName) && !r.EndOfStream)
        {
          nameFound = true;

          do
          {

            txt = r.ReadLine();
            if (String.IsNullOrEmpty(txt) || txt.StartsWith("#"))
              continue;

            txt = txt.ToLower();

            if (txt.StartsWith("d"))
            {
              txt = txt.Replace("disallow: ", "");
              disallowList.Add(txt);
            }
            else if (txt.StartsWith("a"))
            {
              txt = txt.Replace("allow: ", "");
              allowList.Add(txt);
            }
            else
            {
              if (txt.StartsWith("user-agent"))
                continue;
              rulesList.Add(txt);
            }
          }
          while (!txt.StartsWith("user-agent: ") && !r.EndOfStream);
        }
      }
      return nameFound;
    }
  }
}

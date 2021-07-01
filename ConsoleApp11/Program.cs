using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Home.Project.PasingNewSSite
{
    
    class MyAction
    {
        public static void Main()
        {

            List<Parsing.DicResultTags> result = Parsing.GetAsyncWebSitesResult();
            Parsing.Print.PrintConsole(result);
            Console.ReadLine();


        }
    }

    public class Parsing
    {
        private class WebsAndTagsBase
        {
            public static List<string> ListWebSites = new List<string>()
            {
             "https://mail.ru/",
             "https://www.pochta.ru/",
             "Невалид1",//test
             "Невалид2",
             "https://www.rbc.ru/crypto/news/60d374339a79475d1e47e43f",
             "https://1prime.ru/finance/20210624/834018218.html",
             "https://www.rbc.ru/"
             };

            public static Dictionary<string, int> PositiveTags = new Dictionary<string, int>()
            {
              {"mail",0},//test
              {"подни",0},
              {"взлет",0},
              {"укреп",0}
            };

            public static Dictionary<string, int> NegativeTags = new Dictionary<string, int>()
            {
             {"опусти",0},
             {"обвал",0},
             {"ослаб",0}
            };
        }

        public class DicResultTags
        {

            public DicResultTags()
            {
                ResultPositiveTags = new Dictionary<string, int>(WebsAndTagsBase.PositiveTags);
                ResultNegativeTags = new Dictionary<string, int>(WebsAndTagsBase.NegativeTags);
            }
            public string NameSite { get; set; }
            public bool isValid { get; set; }
            public Dictionary<string, int> ResultPositiveTags { get; set; }
            public Dictionary<string, int> ResultNegativeTags { get; set; }
        }

        public static List<DicResultTags> GetAsyncWebSitesResult()
        {

            List<DicResultTags> dicResultsWebsSites = new List<DicResultTags>();
            string[] siteBody = GetAsyncWeb();

            for (int i = 0; i < siteBody.Length; i++)
            {
                DicResultTags site = GetKeyValuePairs(WebsAndTagsBase.ListWebSites[i], siteBody[i]);
                dicResultsWebsSites.Add(site);
            }

            return dicResultsWebsSites;
        }

        private static DicResultTags GetKeyValuePairs(string Name, string SiteBody)
        {
            DicResultTags myResult = new DicResultTags();
            if (SiteBody == "")
            {
                myResult.NameSite = Name;
                myResult.isValid = false;
                return myResult;
            }

            myResult.NameSite = Name;
            string[] positiveTags = new string[WebsAndTagsBase.PositiveTags.Count];
            string[] negativeTags = new string[WebsAndTagsBase.NegativeTags.Count];
            WebsAndTagsBase.PositiveTags.Keys.CopyTo(positiveTags, 0);
            WebsAndTagsBase.NegativeTags.Keys.CopyTo(negativeTags, 0);

            for (int index = 0; index < positiveTags.Length; index++)
            {
                Regex myregex = new Regex($@"{positiveTags[index]}(\w*)", RegexOptions.IgnoreCase);
                MatchCollection mymatches = myregex.Matches(SiteBody);

                myResult.ResultPositiveTags[positiveTags[index]] = mymatches.Count;

            }
            for (int i = 0; i < negativeTags.Length; i++)
            {
                Regex myregex = new Regex($@"{negativeTags[i]}(\w*)", RegexOptions.IgnoreCase);
                MatchCollection mymatches = myregex.Matches(SiteBody);

                myResult.ResultNegativeTags[negativeTags[i]] = mymatches.Count;

            }
            myResult.isValid = true;
            return myResult;
        }

        private static string[] GetAsyncWeb()
        {
            string[] resultMasSting = GetHTTPStack();
            string[] websites = new string[WebsAndTagsBase.ListWebSites.Count];
            websites = resultMasSting;
            return websites;
        }


        private static string[] GetHTTPStack()
        {
            string[] MyWebSites = new string[WebsAndTagsBase.ListWebSites.Count];

            Parallel.For(0, WebsAndTagsBase.ListWebSites.Count, (i, state) =>
            {

                try
                {
                    using HttpClient client = new HttpClient();
                    System.Net.WebClient oWebClient = new System.Net.WebClient();
                    oWebClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0");
                    Uri uriSiteListPath = new Uri(WebsAndTagsBase.ListWebSites[i]);
                    string strStationList = oWebClient.DownloadString(uriSiteListPath);
                    MyWebSites[i] = strStationList;
                }
                catch
                {
                    MyWebSites[i] = "";
                }
            });
 
            return MyWebSites;

        }






        public class Print
        {
            public static void PrintConsole(List<DicResultTags> ResultWebsites)
            {
                foreach (DicResultTags Dic in ResultWebsites)
                {
                    Console.WriteLine($"{Environment.NewLine}Имя сайта: {Dic.NameSite}");
                    Console.WriteLine($"Сайт загрузился:{Dic.isValid}{Environment.NewLine}");
                    Console.WriteLine($"Теги позитив:");
                    foreach (KeyValuePair<string, int> item in Dic.ResultPositiveTags)
                    {
                        Console.WriteLine($"{item.Key} = {item.Value}");
                    }
                    Console.WriteLine($"{Environment.NewLine}Теги негатив:");
                    foreach (KeyValuePair<string, int> item in Dic.ResultNegativeTags)
                    {
                        Console.WriteLine($"{item.Key} = {item.Value}");
                    }
                }
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home.Project.PasingNewsSite
{
    public partial class Parsing
    {
        public class DicQueryGoogle
        {
            public string WebRequest { get; } = SearchQueryGoogle;
            public List<WebSite> ResultWebRequest;
            public DicQueryGoogle(List<WebSite> dicResulTtags)
            {
                ResultWebRequest = dicResulTtags;
            }
            public void PrintConsole()
            {
                Console.WriteLine("");
                Console.WriteLine($"Поисковый запрос: {WebRequest}{Environment.NewLine}");
                Console.WriteLine("#####Результаты#####");
                foreach (WebSite Dic in ResultWebRequest)
                {
                    if (Dic.IsValid == false)
                    {
                        Console.WriteLine($"{Environment.NewLine}Url сайта: {Dic.NameSite}");
                        Console.WriteLine($"Сайт загрузился:{Dic.IsValid}{Environment.NewLine}");

                    }
                    else
                    {


                        Console.WriteLine($"{Environment.NewLine}Url сайта: {Dic.NameSite}");
                        Console.WriteLine($"Сайт загрузился:{Dic.IsValid}{Environment.NewLine}");
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

        public class WebSite
        {
            public WebSite()
            {
                ResultPositiveTags = new Dictionary<string, int>(WebsAndTagsBase.PositiveTags);
                ResultNegativeTags = new Dictionary<string, int>(WebsAndTagsBase.NegativeTags);
            }
            public string NameSite { get; set; }
            public bool IsValid { get; set; }        
            public Dictionary<string, int> ResultPositiveTags { get; set; }
            public Dictionary<string, int> ResultNegativeTags { get; set; }

            public void PrintConsole()
            {

                if (IsValid == false)
                {
                    Console.WriteLine($"{Environment.NewLine}Url сайта: {NameSite}");
                    Console.WriteLine($"Сайт не был загружен:{IsValid}{Environment.NewLine}");

                }
                else
                {
                    Console.WriteLine($"{Environment.NewLine}Url сайта: {NameSite}");
                    Console.WriteLine($"Сайт загружен:{IsValid}{Environment.NewLine}");
                    Console.WriteLine($"Теги позитив:");
                    foreach (KeyValuePair<string, int> item in ResultPositiveTags)
                    {
                        Console.WriteLine($"{item.Key} = {item.Value}");
                    }
                    Console.WriteLine($"{Environment.NewLine}Теги негатив:");
                    foreach (KeyValuePair<string, int> item in ResultNegativeTags)
                    {
                        Console.WriteLine($"{item.Key} = {item.Value}");
                    }
                }


            }
        }
        private static Task<List<WebSite>> GetAsyncWebSitesResult()
        {

            List<WebSite> dicResultsWebsSites = new List<WebSite>();
            string[] siteBody = GetWebBody().Result;
            Console.WriteLine("#####Обработка тегов на сайтах #####");
            for (int i = 0; i < siteBody.Length; i++)
            {
                Console.WriteLine($"Обрабатывается {i} их {siteBody.Length}");
                WebSite site = GetKeyValuePairs(WebsAndTagsBase.ListWebSites[i], siteBody[i]);
                dicResultsWebsSites.Add(site);
            }
            return Task.FromResult(dicResultsWebsSites);
        }


        







    }

}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using HtmlAgilityPack;
using System.Collections.Concurrent;

namespace Home.Project.PasingNewsSite
{

    public partial class Parsing
    {
        private static string SearchQueryGoogle = "новости";
        private static readonly object Mylock = new object();   
        private static Task<string[]> GetWebBody()
        {
            string[] MyWebSites = new string[WebsAndTagsBase.ListWebSites.Count];
            Stopwatch watch = new Stopwatch();

            void RefreshSync(int numbermass, string webbody)
            {
                lock (Mylock)
                {
                    MyWebSites[numbermass] = webbody;
                }
            }
            Console.WriteLine($"Начата обработка коллекции из {WebsAndTagsBase.ListWebSites.Count} сайтов");
            watch.Start();
            Parallel.For(0, WebsAndTagsBase.ListWebSites.Count, (i) =>
            {

                Console.WriteLine($"Обработка сайта №{i+1}");
                try
                {
                    using System.Net.WebClient oWebClient = new System.Net.WebClient();
                    oWebClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0");
                    Uri uriSiteListPath = new Uri(WebsAndTagsBase.ListWebSites[i]);

                    string strStationList = oWebClient.DownloadStringTaskAsync(uriSiteListPath).Result;
                    RefreshSync(i, strStationList);


                }
                catch
                {
                    RefreshSync(i, "");
                }

            });
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
            return Task.FromResult(MyWebSites);
        }
        private static string GetWebBody(string website)
        {
            try
            {

                using System.Net.WebClient oWebClient = new System.Net.WebClient();
                oWebClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0");
                Uri uriSiteListPath = new Uri(website);

                for (int count = 0; ; count++)
                {
                    try
                    {
                        string strStationList = oWebClient.DownloadString(uriSiteListPath);
                        System.Net.HttpWebRequest HttpWReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uriSiteListPath);
                        System.Net.HttpWebResponse HttpWResp = (System.Net.HttpWebResponse)HttpWReq.GetResponse();
                        Console.WriteLine($"Код ответа сервера: {HttpWResp.StatusCode}");
                        return strStationList;
                    }
                    catch (System.Net.WebException ex)
                    {

                        Console.WriteLine($"{ex.Message}{Environment.NewLine}Слишком много запросов...(426?)");
                    }
                    catch
                    {
                        if (count <= 5)
                        {

                            Console.WriteLine($"{count} Метод ожидает ");
                            Thread.Sleep(1000);

                        }
                        else
                        {
                            throw new Exception("GetWebBody");
                        }
                    }

                }
            }
            catch
            {
                return "";
            }
            throw new Exception("GetWebBody2");
        }
        private static WebSite GetKeyValuePairs(string Name, string SiteBody)
        {
            WebSite myResult = new WebSite();
            if (SiteBody == "")
            {
                myResult.Url = Name;
                myResult.IsValid = false;
                return myResult;
            }

            myResult.Url = Name;
            string[] positiveTags = new string[WebsAndTagsBase.PositiveTags.Count];
            string[] negativeTags = new string[WebsAndTagsBase.NegativeTags.Count];
            WebsAndTagsBase.PositiveTags.Keys.CopyTo(positiveTags, 0);
            WebsAndTagsBase.NegativeTags.Keys.CopyTo(negativeTags, 0);
            void UpdateSyncPos(int index,int count)
            {
                lock (Mylock)
                {
                    myResult.ResultPositiveTags[positiveTags[index]] = count;
                }
            }
            void UpdateSyncNeg(int index, int count)
            {
                lock (Mylock)
                {
                    myResult.ResultNegativeTags[negativeTags[index]] = count;
                }
            }
            Parallel.Invoke(() =>
            {
                Parallel.For(0, positiveTags.Length, (index) =>
                {
                    Regex myregex = new Regex($@"{positiveTags[index]}(\w*)", RegexOptions.IgnoreCase);
                    MatchCollection mymatches = myregex.Matches(SiteBody);

                   // myResult.ResultPositiveTags[positiveTags[index]] = mymatches.Count;
                    UpdateSyncPos(index, mymatches.Count);

                });

            },

            () =>
            {
                Parallel.For(0, negativeTags.Length, (i) =>
                {

                    Regex myregex = new Regex($@"{negativeTags[i]}(\w*)", RegexOptions.IgnoreCase);
                    MatchCollection mymatches = myregex.Matches(SiteBody);

                   // myResult.ResultNegativeTags[negativeTags[i]] = mymatches.Count;
                    UpdateSyncPos(i, mymatches.Count);
                });

            });

            myResult.IsValid = true;
            return myResult;
        }
        private static Task<List<string>> GetGcardUrl(string googlebody)
        {
            Console.WriteLine("Получение url карточек новостей со страницы");

            List<string> gcardurl = new List<string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(googlebody);
            HtmlNodeCollection result = doc.DocumentNode.SelectNodes("//g-card/div/div/a");

            if (result == null)
            {
                return Task.FromResult(gcardurl);
            }
            Parallel.ForEach(result, (node) =>
            {
                var res = node.Attributes[1].Value;
               
                gcardurl.Add(res);
            });

            return Task.FromResult(gcardurl);
        }
        private static string MyStringReplaser(string googlestring)
        {

            Regex regex = new Regex(@"(&start=)(\d*)(&sa)");


            foreach (Match item in regex.Matches(googlestring))
            {
                Console.WriteLine($"{item.Groups[2]}");
                string replacement = $"&start={Convert.ToInt32(item.Groups[2].Value) + 10}&sa";
                string result = regex.Replace(googlestring, replacement);
                Console.WriteLine("");
                return result;
            }
            throw new Exception("Неверный формат строки");
        }
        public static DicQueryGoogle GetSearchResult(string search)
        {
            SearchQueryGoogle = search;
            string lentaGoogle = $"https://www.google.ru/search?q={SearchQueryGoogle}&newwindow=1&tbs=qdr:d&tbm=nws&ei=y2ldYufFONySwPAPwr-FmAQ&start=0&sa=N&ved=2ahUKEwin18_83Z33AhVcCRAIHcJfAUM4ChDy0wN6BAgBEDw&biw=1920&bih=1007&dpr=1";

            for (string mods = lentaGoogle; ; mods = MyStringReplaser(mods))
            {

                string sitebody = GetWebBody(mods);
                if (sitebody == "")
                {
                    throw new Exception($"Страница {mods} не загрузилась?");
                }
                try
                {

                    List<string> resultgcards = GetGcardUrl(sitebody).Result;
                    if (resultgcards.Count == default)
                    {
                        break;
                    }
                    WebsAndTagsBase.ListGcardWebUrl.AddRange(resultgcards);

                }
                catch (Exception ex)
                {
                    // Console.WriteLine($"{ex.Message} + {ex.StackTrace}");

                    break;
                }

            }
            WebsAndTagsBase.ListWebSites.AddRange(WebsAndTagsBase.ListGcardWebUrl);
            List<WebSite> resultWebSite = GetAsyncWebSitesResult().Result;

            DicQueryGoogle result = new DicQueryGoogle(resultWebSite);


            return result;
        }

    }
}

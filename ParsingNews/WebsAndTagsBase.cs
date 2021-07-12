using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home.Project.PasingNewsSite
{
    public partial class Parsing
    {
        private class WebsAndTagsBase
        {    
              
            public static List<string> ListGcardWebUrl = new List<string>();

            public static List<string> ListWebSites = new List<string>()
            {
             //"https://mail.ru/",
             //"https://www.pochta.ru/",
             //"Невалид1",//test
             //"Невалид2",
             //"https://www.rbc.ru/crypto/news/60d374339a79475d1e47e43f",
             //"https://1prime.ru/finance/20210624/834018218.html",
             //"https://www.rbc.ru/"
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
             {"опус",0},
             {"обвал",0},
             {"ослаб",0}
            };
        }
    }
}

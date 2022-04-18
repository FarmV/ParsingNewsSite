using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home.Project.PasingNewsSite
{
    public partial class Parsing
    {
        internal class WebsAndTagsBase
        {    
              
            public static List<string> ListGcardWebUrl = new List<string>();

            public static List<string> ListWebSites = new List<string>()
            {

            };

            public static Dictionary<string, int> PositiveTags = new Dictionary<string, int>()
            {
              //{"mail",0},//test
              //{"подни",0},
              //{"взлет",0},
              //{"укреп",0}
            };

            public static Dictionary<string, int> NegativeTags = new Dictionary<string, int>()
            {
             //{"опус",0},
             //{"обвал",0},
             //{"ослаб",0}
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home.Project.PasingNewsSite
{
    class Program
    {
        public static void Main()
        {

           Parsing.DicQueryGoogle resultSearch = Parsing.GetSearchResult("новости");   
           resultSearch.PrintConsole();
          
           Console.ReadLine();    
        }
    }
}

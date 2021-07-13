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

           Parsing.DicQueryGoogle ResultQuerySearch = Parsing.GetSearchResult("пфр");         
           ResultQuerySearch.PrintConsole();


          
           Console.ReadLine();    
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home.Project.PasingNewsSite
{
    class Program
    {

        internal static string queryBase = string.Empty;
        internal static string[] query;
        public static void Main()
        {
            Console.WriteLine("Слово или словосочетание для запроса. Выборка за 24ч");
            Console.WriteLine("Для составных слов нужный формат слово1+слово2");
            queryBase = Console.ReadLine();
            Console.WriteLine("Введите слова для поиска через +");
            string[] str = Console.ReadLine().Split("+",StringSplitOptions.RemoveEmptyEntries| StringSplitOptions.TrimEntries);
            query = str;
            Array.ForEach(str, (s) => {Parsing.WebsAndTagsBase.PositiveTags.Add(s,0); });
           
            Parsing.DicQueryGoogle ResultQuerySearch = Parsing.GetSearchResult(queryBase);
            ResultQuerySearch.PrintConsole();

            Console.ReadLine();
        }
    }
}

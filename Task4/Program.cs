using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task4.Model;

namespace Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            Test2();
        }
        private static void Work()
        {
            HtmlParser parser = new HtmlParser();
            WebClient webclient = new WebClient();
            DownloadText test = new DownloadText(webclient, parser);
            var task1 = test.Download("качай текст");
            var task2 = test.Download("качай текст 2");
            Console.WriteLine("Ждем результатов");
            var result1 = task1.Result;
            Console.WriteLine($"Результат 1 {result1}");
            var result2 = task2.Result;
            Console.WriteLine($"Результат 1 {result2}");
            Console.WriteLine("Готово");
            Console.ReadKey();
        }
        private static void Test()
        {
            HtmlParser parser = new HtmlParser();
            WebClient webclient = new WebClient();
            DownloadText test = new DownloadText(webclient, parser);
            var task1 = test.Test("качай текст");
            var task2 = test.Test("качай текст 2");
            Console.WriteLine("Ждем результатов");
            var result1 = (HtmlDocuments)task1.Result;
            Console.WriteLine($"Результат 1 {result1.HmtlText}");
            var result2 = (HtmlDocuments)task2.Result;
            Console.WriteLine($"Результат 2 {result2.HmtlText}");
            Console.WriteLine("Готово");
            Console.ReadKey();
        }

        private static void Test2()
        {
            HtmlParser parser = new HtmlParser();
            WebClient webclient = new WebClient();
            DownloadText test = new DownloadText(webclient, parser);
            var task1 = test.DownloadAndParse("качай текст");
            var task2 = test.DownloadAndParse("качай текст 2");
            var task3 = test.DownloadAndParse("качай текст 3");
            Console.WriteLine("Ждем результатов");
            var result1 = (HtmlDocuments)task1.Result;
            Console.WriteLine($"Результат 1 {result1.HmtlText}");
            var result2 = (HtmlDocuments)task2.Result;
            Console.WriteLine($"Результат 2 {result2.HmtlText}");
            var result3 = (HtmlDocuments)task3.Result;
            Console.WriteLine($"Результат 2 {result3.HmtlText}");
            Console.WriteLine("Готово");
            Console.ReadKey();
        }

    }
}

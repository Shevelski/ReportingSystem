using HtmlAgilityPack;
using ReportingSystem.Models.News;

namespace ReportingSystem.Utils
{
    public class News
    {
        public List<NewsModel> GetNews(string category)
        {

            return new List<NewsModel>();
        }
        public List<string> GetCategoriesNews()
        {
            string searchUrl = "https://www.ukr.net";
            List<string> strings = [];

            HtmlWeb web = new();
            HtmlDocument document = web.Load(searchUrl.ToString());



            var titleElements = document.DocumentNode.SelectNodes("//span[contains(@class, 'feed__section--title')]");

            // Перевірка, чи елементи були знайдені
            if (titleElements != null)
            {
                foreach (var titleElement in titleElements)
                {
                    // Обробка кожного знайденого елемента
                    string innerText = titleElement.InnerText;
                    Console.WriteLine(innerText);
                    strings.Add(innerText);
                }
            }
            else
            {
                Console.WriteLine("Елементи не знайдені за заданим класом.");
            }


            return strings;
        }
    }
}

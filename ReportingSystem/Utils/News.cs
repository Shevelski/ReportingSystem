using HtmlAgilityPack;
using ReportingSystem.Models.News;
using System.Security.Policy;
using System.Text;

namespace ReportingSystem.Utils
{
    public class News
    {
        public List<NewsModel> GetNews(string url)
        {
            HtmlWeb web1 = new HtmlWeb();
            HtmlDocument doc1 = web1.Load(url.ToString());
            HtmlNodeCollection nodes1 = doc1.DocumentNode.SelectNodes("//*[@class='list-thumbs__info']");

            List<NewsModel> news = new List<NewsModel>();

            if (nodes1 != null)
            {
                foreach (HtmlNode node in nodes1)
                {
                    NewsModel new1 = new NewsModel();
                    new1.Id = Guid.NewGuid();
                    new1.Name = node.InnerText.Replace("&quot;", "").Replace("&#039;", "`");

                    HtmlDocument doc0 = new HtmlDocument();
                    doc0.LoadHtml(node.InnerHtml);
                    HtmlNode linkNode = doc0.DocumentNode.SelectSingleNode("//a[@href]");

                    if (linkNode != null)
                    {
                        new1.Url = linkNode.GetAttributeValue("href", "");
                    }
                    else
                    {
                        new1.Url = "";
                    }
                    news.Add(new1);
                }
            }
            return news;
        }

        public bool CheckIsNull(string url)
        {
            var cat = GetNews(url);
            if (cat.Count > 0) { 
                return false;
            }
            return true;
        }
    
        public List<CategoryNewsModel> GetCategoriesNews()
        {
            string searchUrl = "https://www.unian.ua/";
            List<CategoryNewsModel> categories = new List<CategoryNewsModel>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(searchUrl.ToString());

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*[@class='main-menu__item']");

            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    // Ваш код для обработки каждого элемента
                    Console.OutputEncoding = Encoding.Unicode;

                    HtmlDocument doc0 = new HtmlDocument();
                    doc0.LoadHtml(node.InnerHtml);
                    HtmlNode linkNode = doc0.DocumentNode.SelectSingleNode("//a[@href]");

                    string hrefValue = "";
                    if (linkNode != null)
                    {
                        hrefValue = linkNode.GetAttributeValue("href", "");
                    }
                    else
                    {
                        hrefValue = "";
                    }

                    CategoryNewsModel category = new CategoryNewsModel();
                    category.Id = Guid.NewGuid();
                    category.Name = node.InnerText.Replace("&#039;","`");
                    category.Url = hrefValue;
                    if (!CheckIsNull(category.Url))
                    {
                        categories.Add(category);
                    };
                    

                }
            }
            return categories;
        }

    }
}

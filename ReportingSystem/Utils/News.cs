using HtmlAgilityPack;
using ReportingSystem.Models.News;
using System.Security.Policy;
using System.Text;

namespace ReportingSystem.Utils
{
    public class News
    {
        private readonly HtmlWeb web = new HtmlWeb();

        public List<NewsModel> GetNews(string url)
        {
            HtmlDocument doc = web.Load(url);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*[@class='list-thumbs__info']");

            List<NewsModel> news = new List<NewsModel>();

            if (nodes != null)
            {
                Parallel.ForEach(nodes, node =>
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

                    lock (news)
                    {
                        if (news.Count < 5)
                        {
                            news.Add(new1);
                        }
                    }
                });
            }
            return news;
        }

        public List<NewsModel> CheckIsNull(string url)
        {
            var cat = GetNews(url);
            if (cat.Count > 0)
            {
                return cat;
            }
            return new List<NewsModel>();
        }

        public List<CategoryNewsModel> GetCategoriesNews()
        {
            string searchUrl = "https://www.unian.ua/";
            List<CategoryNewsModel> categories = new List<CategoryNewsModel>();

            HtmlDocument doc = web.Load(searchUrl);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*[@class='main-menu__item']");

            if (nodes != null)
            {
                Parallel.ForEach(nodes, node =>
                {
                    HtmlDocument doc0 = new HtmlDocument();
                    doc0.LoadHtml(node.InnerHtml);
                    HtmlNode linkNode = doc0.DocumentNode.SelectSingleNode("//a[@href]");

                    string hrefValue = (linkNode != null) ? linkNode.GetAttributeValue("href", "") : "";

                    CategoryNewsModel category = new CategoryNewsModel();
                    category.Id = Guid.NewGuid();
                    category.Name = node.InnerText.Replace("&#039;", "`");
                    category.Url = hrefValue;
                    var news = CheckIsNull(category.Url);

                    lock (categories)
                    {
                        if (news.Count > 0)
                        {
                            category.news = news;
                            categories.Add(category);
                        }
                    }
                });
            }
            return categories;
        }
    }
}

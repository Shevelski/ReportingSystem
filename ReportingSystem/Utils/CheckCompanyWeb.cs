using HtmlAgilityPack;
using ReportingSystem.Models.Company;
using System.Text;
using System.Xml;

namespace ReportingSystem.Utils
{
    static public class CheckCompanyWeb
    {
        static public CompanyModel ByCode(string searchString)
        {
            CompanyModel company = new CompanyModel();
            StringBuilder searchUrl = new StringBuilder();

            bool containsOnlyDigits = searchString.All(char.IsDigit);
            if (containsOnlyDigits)
            {
                searchUrl.Append($"https://youcontrol.com.ua/catalog/company_details/{searchString}/");
                
                
                HtmlWeb web = new HtmlWeb();
                HtmlDocument document = web.Load(searchUrl.ToString());


                HtmlNode spanElement = document.DocumentNode.SelectSingleNode("//*[@id='catalog-company-file']/div[2]/div[2]/div[2]/span");
                if (spanElement != null)
                {
                    company.name = (spanElement.InnerText.ToString().Replace("&quot;", "\"").Replace("&#039", "\'"));
                }

                //edrpu
                spanElement = document.DocumentNode.SelectSingleNode("//*[@id='catalog-company-file']/div[2]/div[5]/div[2]/span");
                if (spanElement != null)
                {
                    company.code =(spanElement.InnerText.ToString().Replace("&quot;", "\"").Replace("&#039", "\'"));
                }

                //dateregistration
                spanElement = document.DocumentNode.SelectSingleNode("//*[@id='catalog-company-file']/div[2]/div[6]/div[2]/div[1]");
                if (spanElement != null)
                {
                    company.registrationDate = DateTime.Parse(spanElement.InnerText.ToString().Trim().Substring(0, 15).Trim());
                }

                //header
                spanElement = document.DocumentNode.SelectSingleNode("//*[@id='catalog-company-file']/div[2]/div[7]/div[2]/div[1]/ul/li/a");
                if (spanElement != null)
                {
                    string fullName = spanElement.InnerText.ToString().Replace("&quot;", "\"").Replace("&#039;", "\'").Trim();
                    string[] nameParts = fullName.Split(' ');
                    string firstName = "";
                    string secondName = "";
                    string thirdName = "";

                    if (nameParts.Length >= 3)
                    {
                        firstName = nameParts[0];
                        secondName = nameParts[1];
                        thirdName = nameParts[2];
                    }

                    company.chief = new Models.User.EmployeeModel()
                    {
                        firstName = firstName,
                        secondName = secondName,
                        thirdName = thirdName,
                    };
                        
                }

                //statut capital
                spanElement = document.DocumentNode.SelectSingleNode("//*[@id='catalog-company-file']/div[2]/div[8]/div[2]");
                if (spanElement != null)
                {
                    company.statutCapital = spanElement.InnerText.ToString().Trim();
                }

                //address
                spanElement = document.DocumentNode.SelectSingleNode("//*[@id='catalog-company-file']/div[2]/div[12]/div[2]/table/tbody/tr[1]/td[2]");
                if (spanElement != null)
                {
                    company.address = spanElement.InnerText.ToString().Trim();
                }

                //email
                spanElement = document.DocumentNode.SelectSingleNode("//*[@id='catalog-company-file']/div[2]/div[12]/div[2]/table/tbody/tr[2]/td[2]/text()");
                if (spanElement != null)
                {
                    company.email = spanElement.InnerText.ToString().Trim();
                }
                
                //phone
                spanElement = document.DocumentNode.SelectSingleNode("//*[@id='catalog-company-file']/div[2]/div[12]/div[2]/table/tbody/tr[3]/td[2]/a");
                if (spanElement != null)
                {
                    company.phone = spanElement.InnerText.ToString().Trim();
                }

                //main action
                spanElement = document.DocumentNode.SelectSingleNode("//*[@id='catalog-company-file']/div[2]/div[11]/div[2]/span");
                if (spanElement != null)
                {
                    company.actions = spanElement.InnerText.ToString().Replace("&quot;", "\"").Replace("&#039;", "\'");
                }

                //other action
                List<string> actionsOther = new List<string>();
                int i = 1;
                while (true)
                {
                    spanElement = document.DocumentNode.SelectSingleNode($"//*[@id='catalog-company-file']/div[2]/div[11]/div[2]/ul/li[{i}]");
                    if (spanElement != null)
                    {
                        company.actions += "\n" + (spanElement.InnerText.ToString().Replace("&quot;", "\"").Replace("&#039;", "\'"));
                        i++;
                    }
                    else
                    {
                        i = 0; break;
                    }
                }
            }
            return company;
        }
    }
}

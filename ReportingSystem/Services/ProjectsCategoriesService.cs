using Microsoft.AspNetCore.Mvc;
using ReportingSystem;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;

namespace ReportingSystem.Services
{
    public class ProjectsCategoriesService
    {
        public List<ProjectCategoryModel>? GetCategories(string idCu, string idCo)
        {
            List<ProjectCategoryModel> projectCategoryModels = new List<ProjectCategoryModel>();
            ProjectCategoryModel categoryModel = new ProjectCategoryModel();
            
            if (DatabaseMoq.Customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = DatabaseMoq.Customers.First(cu => cu.id.Equals(idCustomer));

            if (customer.companies == null)
            {
                return null;
            }

            if (!Guid.TryParse(idCo, out Guid idCompany))
            {
                return null;
            }

            var company = customer.companies.First(co => co.id.Equals(idCompany));

            if (company.categories == null)
            {
                return null;
            }

            projectCategoryModels = company.categories;

            return projectCategoryModels;


        }

        public List<ProjectCategoryModel> CreateCategory(string[] ar)
        {

            if (DatabaseMoq.Customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return new List<ProjectCategoryModel>();
            }

            var customer = DatabaseMoq.Customers.First(cu => cu.id.Equals(idCustomer));

            if (customer.companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return new List<ProjectCategoryModel>();
            }

            var company = customer.companies.First(co => co.id.Equals(idCompany));

            //-----------------------------------------------------------------------------0

            if (ar[2] == null && company.categories != null)
            {
                ProjectCategoryModel categoryModel = new ProjectCategoryModel();
                categoryModel.id = Guid.NewGuid();
                categoryModel.name = ar[6];
                categoryModel.projects = new List<Guid>();
                categoryModel.categoriesLevel1 = new List<ProjectCategoryModel1>();
                company.categories.Add(categoryModel);
                DatabaseMoq.UpdateJson();
                return new List<ProjectCategoryModel>();
            }

            //-----------------------------------------------------------------------------1

            if (ar[2] != null && ar[3] == null && company.categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.categories.First(ca1 => ca1.id.Equals(idCatLevel1));

                if (cat0.categoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }
                ProjectCategoryModel1 categoryModel1 = new ProjectCategoryModel1();
                categoryModel1.id = Guid.NewGuid();
                categoryModel1.name = ar[6];
                categoryModel1.projects = new List<Guid>();
                categoryModel1.categoriesLevel2 = new List<ProjectCategoryModel2>();
                cat0.categoriesLevel1.Add(categoryModel1);
                DatabaseMoq.UpdateJson();
                return company.categories;
            }

            //-----------------------------------------------------------------------------2

            if (ar[2] != null && ar[3] != null && ar[4] == null && company.categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.categories.First(ca1 => ca1.id.Equals(idCatLevel1));
                if (cat0.categoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.categoriesLevel1.First(ca1 => ca1.id.Equals(idCatLevel2));

                if (cat1.categoriesLevel2 == null)
                {
                    return new List<ProjectCategoryModel>();
                }
                ProjectCategoryModel2 categoryModel2 = new ProjectCategoryModel2();
                categoryModel2.id = Guid.NewGuid();
                categoryModel2.name = ar[6];
                categoryModel2.projects = new List<Guid>();
                categoryModel2.categoriesLevel3 = new List<ProjectCategoryModel3>();
                cat1.categoriesLevel2.Add(categoryModel2);
                DatabaseMoq.UpdateJson();
                return company.categories;

            }

            //-----------------------------------------------------------------------------3

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null && company.categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.categories.First(ca1 => ca1.id.Equals(idCatLevel1));
                if (cat0.categoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.categoriesLevel1.First(ca1 => ca1.id.Equals(idCatLevel2));
                if (cat1.categoriesLevel2 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat2 = cat1.categoriesLevel2.First(ca1 => ca1.id.Equals(idCatLevel3));
                if (cat2.categoriesLevel3 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                ProjectCategoryModel3 categoryModel3 = new ProjectCategoryModel3();
                categoryModel3.id = Guid.NewGuid();
                categoryModel3.name = ar[6];
                categoryModel3.projects = new List<Guid>();
                cat2.categoriesLevel3.Add(categoryModel3);
                DatabaseMoq.UpdateJson();
                return company.categories;

            }

            return new List<ProjectCategoryModel>();
        }


        public List<ProjectCategoryModel>? EditNameCategory(string[] ar)
        {
            if (DatabaseMoq.Customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return new List<ProjectCategoryModel>();
            }

            var customer = DatabaseMoq.Customers.First(cu => cu.id.Equals(idCustomer));

            if (customer.companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return new List<ProjectCategoryModel>();
            }

            var company = customer.companies.First(co => co.id.Equals(idCompany));

            //---------------------------------------------------------

            if (ar[2] != null && ar[3] == null && company.categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.categories.First(ca1 => ca1.id.Equals(idCatLevel1));


                if (cat0.categoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                cat0.name = ar[6];
                DatabaseMoq.UpdateJson();
                return company.categories;
            }

            //-----------------------------------------------------------------------------1

            if (ar[2] != null && ar[3] != null && ar[4] == null && company.categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.categories.First(ca1 => ca1.id.Equals(idCatLevel1));
                

                if (cat0.categoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.categoriesLevel1.First(ca1 => ca1.id.Equals(idCatLevel2));

                cat1.name = ar[6];
                DatabaseMoq.UpdateJson();
                return company.categories;
            }

            //-----------------------------------------------------------------------------2

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null && company.categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.categories.First(ca1 => ca1.id.Equals(idCatLevel1));
                if (cat0.categoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.categoriesLevel1.First(ca1 => ca1.id.Equals(idCatLevel2));

                if (cat1.categoriesLevel2 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat2 = cat1.categoriesLevel2.First(ca1 => ca1.id.Equals(idCatLevel3));
                cat2.name = ar[6];
                DatabaseMoq.UpdateJson();
                return company.categories;

            }

            //-----------------------------------------------------------------------------3

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] != null && company.categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3) || !Guid.TryParse(ar[5], out Guid idCatLevel4))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.categories.First(ca1 => ca1.id.Equals(idCatLevel1));
                if (cat0.categoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.categoriesLevel1.First(ca1 => ca1.id.Equals(idCatLevel2));
                if (cat1.categoriesLevel2 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat2 = cat1.categoriesLevel2.First(ca1 => ca1.id.Equals(idCatLevel3));
                if (cat2.categoriesLevel3 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat3 = cat2.categoriesLevel3.First(ca1 => ca1.id.Equals(idCatLevel4));
                if (cat3 == null)
                {
                    return new List<ProjectCategoryModel>();
                }
                cat3.name = ar[6];
                DatabaseMoq.UpdateJson();
                return company.categories;

            }

            return new List<ProjectCategoryModel>();
        }

        public List<ProjectCategoryModel>? DeleteCategory(string[] ar)
        {
            if (DatabaseMoq.Customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return new List<ProjectCategoryModel>();
            }

            var customer = DatabaseMoq.Customers.First(cu => cu.id.Equals(idCustomer));

            if (customer.companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return new List<ProjectCategoryModel>();
            }

            var company = customer.companies.First(co => co.id.Equals(idCompany));

            //---------------------------------------------------------

            if (ar[2] != null && ar[3] == null && company.categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.categories.First(ca1 => ca1.id.Equals(idCatLevel1));


                if (cat0.categoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                company.categories.Remove(cat0);
                DatabaseMoq.UpdateJson();
                return company.categories;
            }

            //-----------------------------------------------------------------------------1

            if (ar[2] != null && ar[3] != null && ar[4] == null && company.categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.categories.First(ca1 => ca1.id.Equals(idCatLevel1));


                if (cat0.categoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.categoriesLevel1.First(ca1 => ca1.id.Equals(idCatLevel2));

                cat0.categoriesLevel1.Remove(cat1);

                DatabaseMoq.UpdateJson();
                return company.categories;
            }

            //-----------------------------------------------------------------------------2

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null && company.categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.categories.First(ca1 => ca1.id.Equals(idCatLevel1));
                if (cat0.categoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.categoriesLevel1.First(ca1 => ca1.id.Equals(idCatLevel2));

                if (cat1.categoriesLevel2 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat2 = cat1.categoriesLevel2.First(ca1 => ca1.id.Equals(idCatLevel3));
                cat1.categoriesLevel2.Remove(cat2);
                DatabaseMoq.UpdateJson();
                return company.categories;

            }

            //-----------------------------------------------------------------------------3

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] != null && company.categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3) || !Guid.TryParse(ar[5], out Guid idCatLevel4))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.categories.First(ca1 => ca1.id.Equals(idCatLevel1));
                if (cat0.categoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.categoriesLevel1.First(ca1 => ca1.id.Equals(idCatLevel2));
                if (cat1.categoriesLevel2 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat2 = cat1.categoriesLevel2.First(ca1 => ca1.id.Equals(idCatLevel3));
                if (cat2.categoriesLevel3 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat3 = cat2.categoriesLevel3.First(ca1 => ca1.id.Equals(idCatLevel4));
                if (cat3 == null)
                {
                    return new List<ProjectCategoryModel>();
                }
                cat2.categoriesLevel3.Remove(cat3);
                DatabaseMoq.UpdateJson();
                return company.categories;

            }

            return new List<ProjectCategoryModel>();
        }
    }
}






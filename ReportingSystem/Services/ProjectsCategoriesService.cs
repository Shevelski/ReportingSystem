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

            var customer = DatabaseMoq.Customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null)
            {
                return null;
            }

            if (!Guid.TryParse(idCo, out Guid idCompany))
            {
                return null;
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            if (company.Categories == null)
            {
                return null;
            }

            projectCategoryModels = company.Categories;

            return projectCategoryModels;


        }

        public List<ProjectCategoryModel> CreateCategory(string[] ar)
        {

            if (DatabaseMoq.Customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return new List<ProjectCategoryModel>();
            }

            var customer = DatabaseMoq.Customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return new List<ProjectCategoryModel>();
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            //-----------------------------------------------------------------------------0

            if (ar[2] == null && company.Categories != null)
            {
                ProjectCategoryModel categoryModel = new ProjectCategoryModel();
                categoryModel.Id = Guid.NewGuid();
                categoryModel.Name = ar[6];
                categoryModel.Projects = new List<Guid>();
                categoryModel.CategoriesLevel1 = new List<ProjectCategoryModel1>();
                company.Categories.Add(categoryModel);
                DatabaseMoq.UpdateJson();
                return new List<ProjectCategoryModel>();
            }

            //-----------------------------------------------------------------------------1

            if (ar[2] != null && ar[3] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));

                if (cat0.CategoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }
                ProjectCategoryModel1 categoryModel1 = new ProjectCategoryModel1();
                categoryModel1.Id = Guid.NewGuid();
                categoryModel1.Name = ar[6];
                categoryModel1.Projects = new List<Guid>();
                categoryModel1.CategoriesLevel2 = new List<ProjectCategoryModel2>();
                cat0.CategoriesLevel1.Add(categoryModel1);
                DatabaseMoq.UpdateJson();
                return company.Categories;
            }

            //-----------------------------------------------------------------------------2

            if (ar[2] != null && ar[3] != null && ar[4] == null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                if (cat1.CategoriesLevel2 == null)
                {
                    return new List<ProjectCategoryModel>();
                }
                ProjectCategoryModel2 categoryModel2 = new ProjectCategoryModel2();
                categoryModel2.Id = Guid.NewGuid();
                categoryModel2.Name = ar[6];
                categoryModel2.Projects = new List<Guid>();
                categoryModel2.CategoriesLevel3 = new List<ProjectCategoryModel3>();
                cat1.CategoriesLevel2.Add(categoryModel2);
                DatabaseMoq.UpdateJson();
                return company.Categories;

            }

            //-----------------------------------------------------------------------------3

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));
                if (cat1.CategoriesLevel2 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                if (cat2.CategoriesLevel3 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                ProjectCategoryModel3 categoryModel3 = new ProjectCategoryModel3();
                categoryModel3.Id = Guid.NewGuid();
                categoryModel3.Name = ar[6];
                categoryModel3.Projects = new List<Guid>();
                cat2.CategoriesLevel3.Add(categoryModel3);
                DatabaseMoq.UpdateJson();
                return company.Categories;

            }

            return new List<ProjectCategoryModel>();
        }


        public List<ProjectCategoryModel>? EditNameCategory(string[] ar)
        {
            if (DatabaseMoq.Customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return new List<ProjectCategoryModel>();
            }

            var customer = DatabaseMoq.Customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return new List<ProjectCategoryModel>();
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            //---------------------------------------------------------

            if (ar[2] != null && ar[3] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                cat0.Name = ar[6];
                DatabaseMoq.UpdateJson();
                return company.Categories;
            }

            //-----------------------------------------------------------------------------1

            if (ar[2] != null && ar[3] != null && ar[4] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                

                if (cat0.CategoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                cat1.Name = ar[6];
                DatabaseMoq.UpdateJson();
                return company.Categories;
            }

            //-----------------------------------------------------------------------------2

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                if (cat1.CategoriesLevel2 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                cat2.Name = ar[6];
                DatabaseMoq.UpdateJson();
                return company.Categories;

            }

            //-----------------------------------------------------------------------------3

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] != null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3) || !Guid.TryParse(ar[5], out Guid idCatLevel4))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));
                if (cat1.CategoriesLevel2 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                if (cat2.CategoriesLevel3 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat3 = cat2.CategoriesLevel3.First(ca1 => ca1.Id.Equals(idCatLevel4));
                if (cat3 == null)
                {
                    return new List<ProjectCategoryModel>();
                }
                cat3.Name = ar[6];
                DatabaseMoq.UpdateJson();
                return company.Categories;

            }

            return new List<ProjectCategoryModel>();
        }

        public List<ProjectCategoryModel>? DeleteCategory(string[] ar)
        {
            if (DatabaseMoq.Customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return new List<ProjectCategoryModel>();
            }

            var customer = DatabaseMoq.Customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return new List<ProjectCategoryModel>();
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            //---------------------------------------------------------

            if (ar[2] != null && ar[3] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                company.Categories.Remove(cat0);
                DatabaseMoq.UpdateJson();
                return company.Categories;
            }

            //-----------------------------------------------------------------------------1

            if (ar[2] != null && ar[3] != null && ar[4] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                cat0.CategoriesLevel1.Remove(cat1);

                DatabaseMoq.UpdateJson();
                return company.Categories;
            }

            //-----------------------------------------------------------------------------2

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                if (cat1.CategoriesLevel2 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                cat1.CategoriesLevel2.Remove(cat2);
                DatabaseMoq.UpdateJson();
                return company.Categories;

            }

            //-----------------------------------------------------------------------------3

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] != null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3) || !Guid.TryParse(ar[5], out Guid idCatLevel4))
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));
                if (cat1.CategoriesLevel2 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                if (cat2.CategoriesLevel3 == null)
                {
                    return new List<ProjectCategoryModel>();
                }

                var cat3 = cat2.CategoriesLevel3.First(ca1 => ca1.Id.Equals(idCatLevel4));
                if (cat3 == null)
                {
                    return new List<ProjectCategoryModel>();
                }
                cat2.CategoriesLevel3.Remove(cat3);
                DatabaseMoq.UpdateJson();
                return company.Categories;

            }

            return new List<ProjectCategoryModel>();
        }
    }
}






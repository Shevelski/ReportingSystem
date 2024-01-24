using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;

namespace ReportingSystem.Services
{
    public class ProjectsService
    {

        public async Task<List<ProjectModel>?> GetProjects(string idCu, string idCo)
        {
            List<ProjectModel> projects = [];

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

            if (company.Projects == null)
            {
                return null;
            }

            projects = company.Projects;

            return projects;


        }

        public List<ProjectCategoryModel> CreateProject(string[] ar)
        {

            if (DatabaseMoq.Customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return [];
            }

            var customer = DatabaseMoq.Customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return [];
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            //-----------------------------------------------------------------------------0

            if (ar[2] == null && company.Categories != null)
            {
                ProjectCategoryModel categoryModel = new()
                {
                    Id = Guid.NewGuid(),
                    Name = ar[6],
                    Projects = [],
                    CategoriesLevel1 = []
                };
                company.Categories.Add(categoryModel);
                DatabaseMoq.UpdateJson();
                return [];
            }

            //-----------------------------------------------------------------------------1

            if (ar[2] != null && ar[3] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));

                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
                }
                ProjectCategoryModel1 categoryModel1 = new()
                {
                    Id = Guid.NewGuid(),
                    Name = ar[6],
                    Projects = [],
                    CategoriesLevel2 = []
                };
                cat0.CategoriesLevel1.Add(categoryModel1);
                DatabaseMoq.UpdateJson();
                return company.Categories;
            }

            //-----------------------------------------------------------------------------2

            if (ar[2] != null && ar[3] != null && ar[4] == null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2))
                {
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                if (cat1.CategoriesLevel2 == null)
                {
                    return [];
                }
                ProjectCategoryModel2 categoryModel2 = new()
                {
                    Id = Guid.NewGuid(),
                    Name = ar[6],
                    Projects = [],
                    CategoriesLevel3 = []
                };
                cat1.CategoriesLevel2.Add(categoryModel2);
                DatabaseMoq.UpdateJson();
                return company.Categories;

            }

            //-----------------------------------------------------------------------------3

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3))
                {
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));
                if (cat1.CategoriesLevel2 == null)
                {
                    return [];
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                if (cat2.CategoriesLevel3 == null)
                {
                    return [];
                }

                ProjectCategoryModel3 categoryModel3 = new()
                {
                    Id = Guid.NewGuid(),
                    Name = ar[6],
                    Projects = []
                };
                cat2.CategoriesLevel3.Add(categoryModel3);
                DatabaseMoq.UpdateJson();
                return company.Categories;

            }

            return [];
        }


        public List<ProjectCategoryModel>? EditProject(string[] ar)
        {
            if (DatabaseMoq.Customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return [];
            }

            var customer = DatabaseMoq.Customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return [];
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            //---------------------------------------------------------

            if (ar[2] != null && ar[3] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
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
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
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
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                if (cat1.CategoriesLevel2 == null)
                {
                    return [];
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
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));
                if (cat1.CategoriesLevel2 == null)
                {
                    return [];
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                if (cat2.CategoriesLevel3 == null)
                {
                    return [];
                }

                var cat3 = cat2.CategoriesLevel3.First(ca1 => ca1.Id.Equals(idCatLevel4));
                if (cat3 == null)
                {
                    return [];
                }
                cat3.Name = ar[6];
                DatabaseMoq.UpdateJson();
                return company.Categories;

            }

            return [];
        }

        public List<ProjectCategoryModel>? DeleteProject(string[] ar)
        {
            if (DatabaseMoq.Customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return [];
            }

            var customer = DatabaseMoq.Customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return [];
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            //---------------------------------------------------------

            if (ar[2] != null && ar[3] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
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
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
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
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                if (cat1.CategoriesLevel2 == null)
                {
                    return [];
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
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));
                if (cat1.CategoriesLevel2 == null)
                {
                    return [];
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                if (cat2.CategoriesLevel3 == null)
                {
                    return [];
                }

                var cat3 = cat2.CategoriesLevel3.First(ca1 => ca1.Id.Equals(idCatLevel4));
                if (cat3 == null)
                {
                    return [];
                }
                cat2.CategoriesLevel3.Remove(cat3);
                DatabaseMoq.UpdateJson();
                return company.Categories;

            }

            return [];
        }

        public List<ProjectCategoryModel>? ArchiveProject(string[] ar)
        {
            if (DatabaseMoq.Customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return [];
            }

            var customer = DatabaseMoq.Customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return [];
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            //---------------------------------------------------------

            if (ar[2] != null && ar[3] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
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
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
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
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                if (cat1.CategoriesLevel2 == null)
                {
                    return [];
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
                    return [];
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return [];
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));
                if (cat1.CategoriesLevel2 == null)
                {
                    return [];
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                if (cat2.CategoriesLevel3 == null)
                {
                    return [];
                }

                var cat3 = cat2.CategoriesLevel3.First(ca1 => ca1.Id.Equals(idCatLevel4));
                if (cat3 == null)
                {
                    return [];
                }
                cat2.CategoriesLevel3.Remove(cat3);
                DatabaseMoq.UpdateJson();
                return company.Categories;

            }

            return [];
        }
    }
}

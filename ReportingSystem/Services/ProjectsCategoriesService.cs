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

        //public ProjectCategoryModel? CreateCategory(string[] ar)
        //{
        //    ProjectCategoryModel categoryModel = new ProjectCategoryModel();

        //    if (DatabaseMoq.Customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
        //    {
        //        return null;
        //    }

        //    var customer = DatabaseMoq.Customers.First(cu => cu.id.Equals(idCustomer));

        //    if (customer.companies == null)
        //    {
        //        return null;
        //    }

        //    if (!Guid.TryParse(ar[1], out Guid idCompany))
        //    {
        //        return null;
        //    }

        //    var company = customer.companies.First(co => co.id.Equals(idCompany));

        //    if (company.categories == null)
        //    {
        //        //company.categories = new ProjectCategoryModel();
        //        //categoryModel = company.categories;
        //    }


        //    //const v0 = this.selectedCustomerId;
        //    //const v1 = this.selectedCompanyId;
        //    //const v2 = navigLevel;
        //    //const v3 = this.navigLevel2;
        //    //const v4 = this.navigLevel3;
        //    //const v5 = this.editCategoryName;
        //    //const v6 = id1;


        //    if (ar[2].Equals("-1"))
        //    {
        //        //categoryModel
        //        //ProjectCategoryModel projectCategoryModel = new ProjectCategoryModel();
        //        //projectCategoryModel.id = Guid.NewGuid();
        //        //projectCategoryModel.name = ar[5];
        //        //if (DatabaseMoq.ProjectsCategories != null)
        //        //{
        //        //    DatabaseMoq.ProjectsCategories.Add(projectCategoryModel);
        //        //    DatabaseMoq.UpdateJson();
        //        //    return categoryModel;
        //        //}
        //    }
        //    else
        //    {
        //        if (DatabaseMoq.ProjectsCategories != null)
        //        {
        //            ProjectCategoryModel? categoryLevel1 = DatabaseMoq.ProjectsCategories.FirstOrDefault(c => c.id.Equals(idLevel1));
        //            if (ar[3].Equals("-1"))
        //            {
        //                ProjectCategoryModel2 projectCategoryModel2 = new ProjectCategoryModel2();
        //                projectCategoryModel2.id = Guid.NewGuid();
        //                projectCategoryModel2.name = ar[5];
        //                if (categoryLevel1 != null)
        //                {
        //                    var categoriesLevel2 = categoryLevel1.categoriesLevel2;
        //                    if (categoriesLevel2 != null)
        //                    {
        //                        categoriesLevel2.Add(projectCategoryModel2);
        //                        DatabaseMoq.UpdateJson();
        //                        //return Json(categoryLevel1.categoriesLevel2);
        //                    }
        //                }

        //            }
        //            if (ar[4].Equals("-1"))
        //            {
        //                ProjectCategoryModel3 projectCategoryModel3 = new ProjectCategoryModel3();
        //                projectCategoryModel3.id = Guid.NewGuid();
        //                projectCategoryModel3.name = ar[5];
        //                if (categoryLevel1 != null)
        //                {
        //                    var categoriesLevel2 = categoryLevel1.categoriesLevel2;
        //                    if (categoriesLevel2 != null)
        //                    {
        //                        var categoryLevel2 = categoriesLevel2[Int32.Parse(ar[3])];
        //                        if (categoryLevel2 != null)
        //                        {
        //                            var categoriesLevel3 = categoryLevel2.categoriesLevel3;
        //                            if (categoriesLevel3 != null)
        //                            {
        //                                categoriesLevel3.Add(projectCategoryModel3);
        //                                DatabaseMoq.UpdateJson();
        //                                //return Json(categoriesLevel3);
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }


        //    return categoryLevel1 == null ? null : categoryLevel1;
        //}


        public ProjectCategoryModel? EditNameCategory(string[] ar)
        {
            ProjectCategoryModel? categoryLevel1 = new ProjectCategoryModel();
            //if (DatabaseMoq.ProjectsCategories != null)
            //{
            //    categoryLevel1 = DatabaseMoq.ProjectsCategories.FirstOrDefault(c => c.id.Equals(idLevel1));
            //}

            //if (categoryLevel1 != null)
            //{
            //    if (levels[1] != -1 && levels[2] != -1)
            //    {
            //        if (categoryLevel1 != null)
            //        {
            //            var categoriesLevel2 = categoryLevel1.categoriesLevel2;
            //            if (categoriesLevel2 != null)
            //            {
            //                var categoryLevel2 = categoriesLevel2[levels[1]];
            //                if (categoryLevel2 != null)
            //                {
            //                    var categoriesLevel3 = categoryLevel2.categoriesLevel3;
            //                    if (categoriesLevel3 != null)
            //                    {
            //                        var categoryLevel3 = categoriesLevel3[levels[2]];
            //                        if (categoriesLevel3 != null)
            //                        {
            //                            categoryLevel3.name = newName;
            //                            return Json(categoryLevel3);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else if (levels[1] != -1 && levels[2] == -1 && categoryLevel1.categoriesLevel2 != null)
            //    {
            //        categoryLevel1.categoriesLevel2[levels[1]].name = newName;
            //        return Json(categoryLevel1.categoriesLevel2[levels[1]]);
            //    }
            //    else
            //    {
            //        categoryLevel1.name = newName;
            //        DatabaseMoq.UpdateJson();
            //        return Json(categoryLevel1);
            //    }
            //}
            return categoryLevel1;
        }
    }
}






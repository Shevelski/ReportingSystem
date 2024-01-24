using ReportingSystem.Models.Project;

namespace ReportingSystem.Data.Generate
{
    public class GenerateCategories
    {
        public List<ProjectCategoryModel> Categories()
        {
            List<ProjectCategoryModel> models = new List<ProjectCategoryModel>();

            string[] listCategories1 = { "Основні", "Допоміжні", "Адміністративні", "Соціальні" };
            string[] listCategories11 = { "Розробка проекту", "Технічна підтримка", "Консультацій послуги" };
            string[] listCategories12 = { "Офісна інфраструктура", "Хмарна інфраструктура" };
            string[] listCategories13 = { "Бюджетна оцінка", "Внутрішня розробка", "Корпоративний захід", "Маркетинг", "Навчання", "Офісне навчання", "Продажі", "Простій" };
            string[] listCategories14 = { "Відпустка", "Лікарняний", "Відгул", "Прогул" };
            string[] listCategories111 = { "Проектування", "Розгортання", "Налаштування", "Тестування" };
            string[] listCategories112 = { "Актуалізація ринку", "Проектування", "Розгортання", "Налаштування", "Тестування" };

            foreach (string category in listCategories1)
            {
                ProjectCategoryModel model = new ProjectCategoryModel
                {
                    Id = Guid.NewGuid(),
                    Name = category,
                    Projects = new List<Guid>(),
                    CategoriesLevel1 = new List<ProjectCategoryModel1>()
                };
                models.Add(model);

                if (category == "Основні")
                {
                    foreach (string subCategory in listCategories11)
                    {
                        ProjectCategoryModel1 model1 = new ProjectCategoryModel1
                        {
                            Id = Guid.NewGuid(),
                            Name = subCategory,
                            CategoriesLevel2 = new List<ProjectCategoryModel2>()
                        };
                        model.CategoriesLevel1.Add(model1);
                    }
                }
                else if (category == "Допоміжні")
                {
                    foreach (string subCategory in listCategories12)
                    {
                        ProjectCategoryModel1 model1 = new ProjectCategoryModel1
                        {
                            Id = Guid.NewGuid(),
                            Name = subCategory,
                            CategoriesLevel2 = new List<ProjectCategoryModel2>()
                        };
                        model.CategoriesLevel1.Add(model1);
                    }
                }
                else if (category == "Адміністративні")
                {
                    foreach (string subCategory in listCategories13)
                    {
                        ProjectCategoryModel1 model1 = new ProjectCategoryModel1
                        {
                            Id = Guid.NewGuid(),
                            Name = subCategory,
                            CategoriesLevel2 = new List<ProjectCategoryModel2>()
                        };
                        model.CategoriesLevel1.Add(model1);
                    }
                }
                else if (category == "Соціальні")
                {
                    foreach (string subCategory in listCategories14)
                    {
                        ProjectCategoryModel1 model1 = new ProjectCategoryModel1
                        {
                            Id = Guid.NewGuid(),
                            Name = subCategory,
                            CategoriesLevel2 = new List<ProjectCategoryModel2>()
                        };
                        model.CategoriesLevel1.Add(model1);
                    }
                }
            }

            foreach (string category in listCategories111)
            {
                ProjectCategoryModel2 model2 = new ProjectCategoryModel2
                {
                    Id = Guid.NewGuid(),
                    Name = category,
                    CategoriesLevel3 = new List<ProjectCategoryModel3>()
                };
                var model0 = models[0];
                var model01 = model0.CategoriesLevel1;
                var model02 = model01 != null ? model01[0] : null;
                var model03 = model02 != null ? model02.CategoriesLevel2 : null;
                if (model03 != null)
                {
                    model03.Add(model2);
                }

                //models[0].categoriesLevel1[0].categoriesLevel2.Add(model2);
                
            }

            foreach (string category in listCategories112)
            {
                ProjectCategoryModel2 model2 = new ProjectCategoryModel2
                {
                    Id = Guid.NewGuid(),
                    Name = category,
                    CategoriesLevel3 = new List<ProjectCategoryModel3>()
                };
                var model0 = models[0];
                var model01 = model0.CategoriesLevel1;
                var model02 = model01 != null ? model01[1] : null;
                var model03 = model02 != null ? model02.CategoriesLevel2 : null;
                if (model03 != null)
                {
                    model03.Add(model2);
                }
                //models[0].categoriesLevel1[1].categoriesLevel2.Add(model2);
            }

            return models;
        }
    }
}

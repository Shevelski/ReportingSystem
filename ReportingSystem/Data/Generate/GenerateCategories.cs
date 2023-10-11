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
                    id = Guid.NewGuid(),
                    name = category,
                    projects = new List<Guid>(),
                    categoriesLevel1 = new List<ProjectCategoryModel1>()
                };
                models.Add(model);

                if (category == "Основні")
                {
                    foreach (string subCategory in listCategories11)
                    {
                        ProjectCategoryModel1 model1 = new ProjectCategoryModel1
                        {
                            id = Guid.NewGuid(),
                            name = subCategory,
                            categoriesLevel2 = new List<ProjectCategoryModel2>()
                        };
                        model.categoriesLevel1.Add(model1);
                    }
                }
                else if (category == "Допоміжні")
                {
                    foreach (string subCategory in listCategories12)
                    {
                        ProjectCategoryModel1 model1 = new ProjectCategoryModel1
                        {
                            id = Guid.NewGuid(),
                            name = subCategory,
                            categoriesLevel2 = new List<ProjectCategoryModel2>()
                        };
                        model.categoriesLevel1.Add(model1);
                    }
                }
                else if (category == "Адміністративні")
                {
                    foreach (string subCategory in listCategories13)
                    {
                        ProjectCategoryModel1 model1 = new ProjectCategoryModel1
                        {
                            id = Guid.NewGuid(),
                            name = subCategory,
                            categoriesLevel2 = new List<ProjectCategoryModel2>()
                        };
                        model.categoriesLevel1.Add(model1);
                    }
                }
                else if (category == "Соціальні")
                {
                    foreach (string subCategory in listCategories14)
                    {
                        ProjectCategoryModel1 model1 = new ProjectCategoryModel1
                        {
                            id = Guid.NewGuid(),
                            name = subCategory,
                            categoriesLevel2 = new List<ProjectCategoryModel2>()
                        };
                        model.categoriesLevel1.Add(model1);
                    }
                }
            }

            foreach (string category in listCategories111)
            {
                ProjectCategoryModel2 model2 = new ProjectCategoryModel2
                {
                    id = Guid.NewGuid(),
                    name = category,
                    categoriesLevel3 = new List<ProjectCategoryModel3>()
                };
                models[0].categoriesLevel1[0].categoriesLevel2.Add(model2);
            }

            foreach (string category in listCategories112)
            {
                ProjectCategoryModel2 model2 = new ProjectCategoryModel2
                {
                    id = Guid.NewGuid(),
                    name = category,
                    categoriesLevel3 = new List<ProjectCategoryModel3>()
                };
                models[0].categoriesLevel1[1].categoriesLevel2.Add(model2);
            }

            return models;
        }
    }
}

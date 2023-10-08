namespace ReportingSystem.Models.Project
{
    public class ProjectCategoryModel
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        public List<Guid>? projects { get; set; }
        public List<ProjectCategoryModel1>? categoriesLevel1 { get; set; }
        //public List<ProjectCategoryModel2>? categoriesLevel2 { get; set; }
        //public List<ProjectCategoryModel3>? categoriesLevel3 { get; set; }

        public ProjectCategoryModel()
        {
            id = Guid.NewGuid();
            projects = new List<Guid>();
            categoriesLevel1 = new List<ProjectCategoryModel1>();
            //categoriesLevel2 = new List<ProjectCategoryModel2>();
            //categoriesLevel3 = new List<ProjectCategoryModel3>();
        }
    }

    public class ProjectCategoryModel1
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        public List<Guid>? projects { get; set; }
        public List<ProjectCategoryModel2>? categoriesLevel2 { get; set; }

        public ProjectCategoryModel1()
        {
            id = Guid.NewGuid();
            projects = new List<Guid>();
            categoriesLevel2 = new List<ProjectCategoryModel2>();
        }
    }

    public class ProjectCategoryModel2
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        public List<Guid>? projects { get; set; }
        public List<ProjectCategoryModel3>? categoriesLevel3 { get; set; }

        public ProjectCategoryModel2()
        {
            id = Guid.NewGuid();
            projects = new List<Guid>();
            categoriesLevel3 = new List<ProjectCategoryModel3>();
        }
    }

    public class ProjectCategoryModel3
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        public List<Guid>? projects { get; set; }

        public ProjectCategoryModel3()
        {
            id = Guid.NewGuid();
            projects = new List<Guid>();
        }
    }
}

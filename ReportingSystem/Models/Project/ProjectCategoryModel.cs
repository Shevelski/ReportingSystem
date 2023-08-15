namespace ReportingSystem.Models.Project
{
    public class ProjectCategoryModel
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        public List<string>? projects { get; set; }
        public List<ProjectCategoryModel2>? categoriesLevel2 { get; set; }

        public ProjectCategoryModel()
        {
            id = Guid.NewGuid();
            projects = new List<string>();
            categoriesLevel2 = new List<ProjectCategoryModel2>();
        }
    }

    public class ProjectCategoryModel2
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        public List<string>? projects { get; set; }
        public List<ProjectCategoryModel3>? categoriesLevel3 { get; set; }

        public ProjectCategoryModel2()
        {
            id = Guid.NewGuid();
            projects = new List<string>();
            categoriesLevel3 = new List<ProjectCategoryModel3>();
        }
    }

    public class ProjectCategoryModel3
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        public List<string>? projects { get; set; }

        public ProjectCategoryModel3()
        {
            id = Guid.NewGuid();
            projects = new List<string>();
        }
    }


}

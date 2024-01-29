namespace ReportingSystem.Models.Project
{
    public class ProjectCategoryModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<Guid>? Projects { get; set; }
        public List<ProjectCategoryModel1>? CategoriesLevel1 { get; set; }

        public ProjectCategoryModel()
        {
            Id = Guid.NewGuid();
            Projects = new List<Guid>();
            CategoriesLevel1 = new List<ProjectCategoryModel1>();
        }
    }

    public class ProjectCategoryModel1
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<Guid>? Projects { get; set; }
        public List<ProjectCategoryModel2>? CategoriesLevel2 { get; set; }

        public ProjectCategoryModel1()
        {
            Id = Guid.NewGuid();
            Projects = new List<Guid>();
            CategoriesLevel2 = new List<ProjectCategoryModel2>();
        }
    }

    public class ProjectCategoryModel2
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<Guid>? Projects { get; set; }
        public List<ProjectCategoryModel3>? CategoriesLevel3 { get; set; }

        public ProjectCategoryModel2()
        {
            Id = Guid.NewGuid();
            Projects = new List<Guid>();
            CategoriesLevel3 = new List<ProjectCategoryModel3>();
        }
    }

    public class ProjectCategoryModel3
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<Guid>? Projects { get; set; }

        public ProjectCategoryModel3()
        {
            Id = Guid.NewGuid();
            Projects = new List<Guid>();
        }
    }
}

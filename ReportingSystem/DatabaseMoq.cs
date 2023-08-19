﻿using Bogus;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Project;
using ReportingSystem.Models;
using ReportingSystem.Test.GenerateData;
using ReportingSystem.Test.Generate;
using System.Diagnostics;
using Newtonsoft.Json;
using Bogus.DataSets;

namespace ReportingSystem
{
    public static class DatabaseMoq
    {
        public static List<CustomerModel>? Customers { get; set; }
        public static List<List<EmployeeModel>>? AllUsers { get; set; }
        public static List<EmployeeModel>? Users { get; set; }
        public static List<List<ProjectCategoryModel>>? AllProjectsCategories { get; set; }
        public static List<ProjectCategoryModel>? ProjectsCategories { get; set; }
        public static List<List<ProjectModel>>? AllProjects { get; set; }
        public static List<ProjectModel>? Projects{ get; set; }
        public static List<List<CompanyModel>>? AllCompanies { get; set; }
        public static List<CompanyModel>? Companies { get; set; }
        public static List<ProjectStatusModel> ProjectStatus { get; set; }
        public static List<EmployeePositionModel> UserPositions { get; set; }
        public static List<EmployeeRolModel> UserRolls { get; set; }
        public static List<CompanyStatusModel> CompanyStatus { get; set; }

        private const string DataFilePath = "data.json";
        static DatabaseMoq()
        {

            if (File.Exists(DataFilePath) && new FileInfo(DataFilePath).Length > 0)
            {
                string jsonData = File.ReadAllText(DataFilePath);
                Customers = JsonConvert.DeserializeObject<List<CustomerModel>>(jsonData);
            }
            else
            {

                string jsonData = JsonConvert.SerializeObject(DatabaseMoqGenerate.Customers, Formatting.Indented);
                File.WriteAllText(DataFilePath, jsonData);
            }


            

        }
    }
}

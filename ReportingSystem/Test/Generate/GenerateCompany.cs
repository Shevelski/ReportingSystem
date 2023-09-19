﻿using Bogus;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;
using ReportingSystem.Test.GenerateData;
using ReportingSystem.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ReportingSystem.Test.Generate
{
    static public class GenerateCompany
    {
        static Random random = new Random();

        static List<string> listEm = new List<string>();
        static public string StatusWeb()
        {
            List<string> statusCompanyFromWeb = new List<string> { "Зареєстровано", "На реєстрації", "Актуальна", "На перегляді" };
            return statusCompanyFromWeb[random.Next(statusCompanyFromWeb.Count)];
        }
        public static CompanyStatusModel Status()
        {
            CompanyStatusModel result = new CompanyStatusModel();
            CompanyStatus[] values = { CompanyStatus.Project, CompanyStatus.Actual, CompanyStatus.Archive };
            CompanyStatus status = values[random.Next(values.Length)];

            result.companyStatusType = status;
            result.companyStatusName = status.GetDisplayName();

            return result;
        }
        public static List<EmployeePositionModel> Positions()
        {
            List<EmployeePositionModel> UserPositions = new List<EmployeePositionModel>();

            List<string> positions = new List<string>
            {
                "Директор",
                "Адміністратор",
                "Заступник директора",
                "Економіст",
                "Юрист",
                "HR",
                "Проект-менеджер",
                "Розробник",
                "Тестувальник",
                "Графічний дизайнер"
            };

            int countPositions = 0;
            //директор
            EmployeePositionModel userPosition = new EmployeePositionModel();
            //userPosition.employees = new List<EmployeeModel>();
            userPosition.namePosition = positions[0];
            UserPositions.Add(userPosition);
            countPositions++;

            //адміністратор
            userPosition = new EmployeePositionModel();
            //userPosition.employees = new List<EmployeeModel>();
            userPosition.namePosition = positions[1];
            UserPositions.Add(userPosition);
            countPositions++;

            // заст директора
            int rnd = random.Next(2, 3);

            for (int i = 0; i < rnd; i++)
            {
                userPosition = new EmployeePositionModel();
                //userPosition.employees = new List<EmployeeModel>();
                userPosition.namePosition = positions[2];
                UserPositions.Add(userPosition);
                userPosition = new EmployeePositionModel();
                //userPosition.employees = new List<EmployeeModel>();
                userPosition.namePosition = positions[3];
                UserPositions.Add(userPosition);
                userPosition = new EmployeePositionModel();
                //userPosition.employees = new List<EmployeeModel>();
                userPosition.namePosition = positions[4];
                UserPositions.Add(userPosition);
                userPosition = new EmployeePositionModel();
                //userPosition.employees = new List<EmployeeModel>();
                userPosition.namePosition = positions[5];
                UserPositions.Add(userPosition);
            }
            countPositions += rnd;

            int countPos = rnd * 2;
            for (int i = 0; i < countPos; i++)
            {
                userPosition = new EmployeePositionModel();
                //userPosition.employees = new List<EmployeeModel>();
                userPosition.namePosition = positions[6];
                UserPositions.Add(userPosition);
            }
            countPositions += countPos;

            
            int rnd1 = random.Next(1, 4);
            countPos = rnd * rnd1;
            for (int i = 0; i < countPos; i++)
            {
                userPosition = new EmployeePositionModel();
                //userPosition.employees = new List<EmployeeModel>();
                userPosition.namePosition = positions[7];
                UserPositions.Add(userPosition);
            }

            countPositions += countPos;

            rnd1 = random.Next(1, 4);
            countPos = rnd * rnd1;
            for (int i = 0; i < countPos; i++)
            {
                userPosition = new EmployeePositionModel();
                //userPosition.employees = new List<EmployeeModel>();
                userPosition.namePosition = positions[8];
                UserPositions.Add(userPosition);
            }
            countPositions += countPos;


            rnd1 = random.Next(1, 4);
            countPos = rnd * rnd1;
            for (int i = 0; i < countPos; i++)
            {
                userPosition = new EmployeePositionModel();
                //userPosition.employees = new List<EmployeeModel>();
                userPosition.namePosition = positions[9];
                UserPositions.Add(userPosition);
            }
            countPositions += countPos;

            Debug.WriteLine($"All is position {countPositions}");
            return UserPositions;
        }
        static public List<EmployeeModel> Employees(CompanyModel company, Guid customerId)
        {
            List<EmployeeModel> results =  new List<EmployeeModel> ();

            if (company.positions != null)
            {
                int i = 0;
                foreach (EmployeePositionModel userPosition in company.positions)
                {
                    
                    EmployeeModel user = new EmployeeModel();
                    user = GenerateEmployee(company, customerId);
                    user.position = userPosition;
                    
                    user.rol = GenerateRol(user.position);
                    //userPosition.employees.Add(user);
                    results.Add(user);
                    Debug.WriteLine($"User {i} added from {company.positions.Count}");
                    i++;
                }
            }
            
            return results;
        }
        static public  EmployeeModel GenerateEmployee(CompanyModel company, Guid customerId)
        {

            EmployeeModel employee = new EmployeeModel();
            var faker = new Faker();

            employee.id = Guid.NewGuid();
            employee.companyId = company.id;
            employee.customerId = customerId;
            employee.firstName = faker.Name.FirstName();
            employee.secondName = faker.Name.LastName();

            employee.thirdName = faker.Name.JobType();
            employee.taxNumber = GenerateInfo.Code();
            employee.phoneSelf = GenerateInfo.MobilePhoneNumber();
            employee.phoneWork = GenerateInfo.PhoneNumber();
            employee.emailWork = (employee.secondName.ToLower() + "@gmail.ua").Replace(" ", "");

            while (listEm.Exists(e => e.Equals(employee.emailWork)))
            {
                employee.emailWork = (employee.secondName.ToLower() + random.Next(0,300) + "@gmail.ua").Replace(" ", "");
                Debug.WriteLine($"this data is exist");
            }
            listEm.Add(employee.emailWork);

            employee.emailSelf = (employee.firstName.ToLower() + "@gmail.ua").Replace(" ", "");
            employee.login = employee.secondName;
            employee.status = new EmployeeStatusModel()
            {
                employeeStatusType = EmployeeStatus.Actual,
                employeeStatusName = EmployeeStatus.Actual.GetDisplayName(),
            };
            employee.password = GenerateInfo.Password();
            employee.addressFact = faker.Address.FullAddress();
            employee.addressReg = faker.Address.FullAddress();
            employee.photo = "/img/UserPhoto/John1.jpg";
            employee.salary = random.Next(20000, 100000);
            employee.addSalary = employee.salary * random.Next(10, 60) / 100.0;
            employee.birthDate = GenerateDate.BetweenDates(DateTime.Today.AddYears(-60), DateTime.Today.AddYears(-20));
            employee.workStartDate = GenerateDate.BetweenDates(company.registrationDate, DateTime.Today);
            employee.holidayDate = GenerateDate.RangeDates(employee.workStartDate, 3, true);
            employee.taketimeoffDate = GenerateDate.RangeDates(employee.workStartDate, 1, false);
            employee.assignmentDate = GenerateDate.RangeDates(employee.workStartDate, 1, false);

            
            return employee;
        }
        static public EmployeeRolModel GenerateRol(EmployeePositionModel position)
        {
            EmployeeRolModel rol = new EmployeeRolModel();
            List<string> positions = new List<string>
            {
                "Директор",
                "Адміністратор",
                "Заступник директора",
                "Економіст",
                "Юрист",
                "HR",
                "Проект-менеджер",
                "Розробник",
                "Тестувальник",
                "Графічний дизайнер"
            };

            if (position.namePosition == positions[0] || position.namePosition == positions[2])
            {
                rol.rolType = EmployeeRolStatus.CEO;
                rol.rolName = EmployeeRolStatus.CEO.GetDisplayName();
                return rol;
            }
            if (position.namePosition == positions[1])
            {
                rol.rolType = EmployeeRolStatus.Administrator;
                rol.rolName = EmployeeRolStatus.Administrator.GetDisplayName();
            }
            if (position.namePosition == positions[3] || position.namePosition == positions[4] || position.namePosition == positions[5] || position.namePosition == positions[6])
            {
                rol.rolType = EmployeeRolStatus.ProjectManager;
                rol.rolName = EmployeeRolStatus.ProjectManager.GetDisplayName();
            }
            if (position.namePosition == positions[7] || position.namePosition == positions[8] || position.namePosition == positions[9])
            {
                rol.rolType = EmployeeRolStatus.User;
                rol.rolName = EmployeeRolStatus.User.GetDisplayName();
            }
            return rol;
        }
        static public CompanyModel? RandomCompany(CustomerModel customer)
        {
            var faker = new Faker();
            CompanyModel company = new CompanyModel();
            if (company != null)
            {
                company.id = Guid.NewGuid();
                company.name = faker.Company.CompanyName();
                company.address = faker.Address.FullAddress();
                company.code = GenerateInfo.Code();
                company.actions = faker.Commerce.ProductAdjective();
                company.statusWeb = StatusWeb();
                company.status = Status();
                company.phone = GenerateInfo.PhoneNumber();
                company.email = (Regex.Replace(company.name, "[^0-9a-zA-Z]+", "") + ".com.ua").Replace(" ", "").ToLower();
                company.statutCapital = random.Next(1000, 300000).ToString() + " UAH";
                company.registrationDate = GenerateDate.BetweenDates(new DateTime(2000, 01, 01), new DateTime(2010, 01, 01));
                company.positions = Positions();
                company.rolls = DefaultEmployeeRolls.GetForEmployee();
                company.employees = Employees(company, customer.id);
                company.chief = company.employees.First(u => u.position.namePosition.Equals("Директор"));

                company.categories = Categories();
                //company.projects = GenerateProjects();
                return company;
            }
            return null;
        }
        static public ProjectCategoryModel Categories()
        {
            List<ProjectCategoryModel1> models1 = new List<ProjectCategoryModel1>();
            List<ProjectCategoryModel2> models2 = new List<ProjectCategoryModel2>();
            List<ProjectCategoryModel3> models3 = new List<ProjectCategoryModel3>();

            string[] listCategories1 = {"Основні", "Допоміжні", "Адміністративні", "Соціальні"};
            string[] listCategories12 = { "Розробка проекту", "Технічна підтримка", "Консультацій послуги"};
            string[] listCategories22 = { "Офісна інфраструктура", "Хмарна інфраструктура" };
            string[] listCategories221 = { "Проектування", "Розгортання", "Налаштування", "Тестування" };
            string[] listCategories222 = { "Актуалізація ринку" ,"Проектування", "Розгортання", "Налаштування", "Тестування" };
            string[] listCategories23 = { "Бюджетна оцінка", "Внутрішня розробка", "Корпоративний захід", "Маркетинг", "Навчання", "Офісне навчання", "Продажі", "Простій"};
            string[] listCategories24 = { "Відпустка", "Лікарняний", "Відгул", "Прогул" };

            return new ProjectCategoryModel();
        }
        static public ProjectCategoryModel1 ProjectCategoryModel1()
        {
            return new ProjectCategoryModel1();
        }
    }
}

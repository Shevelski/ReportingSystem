using Bogus;
using ReportingSystem.Enum;
using ReportingSystem.Enum.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Test.GenerateData;
using ReportingSystem.Utils;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ReportingSystem.Test.Generate
{
    static public class GenerateCompany
    {
        static Random random = new Random();
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

            EmployeePositionModel userPosition = new EmployeePositionModel();
            userPosition.employees = new List<EmployeeModel>();
            userPosition.namePosition = positions[0];
            UserPositions.Add(userPosition);

            userPosition = new EmployeePositionModel();
            userPosition.namePosition = positions[1];
            UserPositions.Add(userPosition);

            int rnd = random.Next(2, 3);

            for (int i = 0; i < rnd; i++)
            {
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[2];
                UserPositions.Add(userPosition);
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[3];
                UserPositions.Add(userPosition);
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[4];
                UserPositions.Add(userPosition);
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[5];
                UserPositions.Add(userPosition);
            }

            for (int i = 0; i < rnd * 2; i++)
            {
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[6];
                UserPositions.Add(userPosition);
            }

            int rnd1 = random.Next(1, 4);
            for (int i = 0; i < rnd * rnd1; i++)
            {
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[7];
                UserPositions.Add(userPosition);
            }

            rnd1 = random.Next(1, 4);
            for (int i = 0; i < rnd * rnd1; i++)
            {
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[8];
                UserPositions.Add(userPosition);
            }
            rnd1 = random.Next(1, 4);
            for (int i = 0; i < rnd * rnd1; i++)
            {
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[9];
                UserPositions.Add(userPosition);
            }
            return UserPositions;
        }
        static public List<EmployeeModel> Employees(CompanyModel company)
        {
            List<EmployeeModel> results =  new List<EmployeeModel> ();

            foreach (EmployeePositionModel userPosition in company.positions)
            {
                EmployeeModel user = new EmployeeModel();
                user = GenerateUser(company);
                user.position = userPosition;
                user.rol = GenerateRol(user.position);
                results.Add(user);
                Debug.WriteLine("User added");
            }

            return results;
        }
        static public  EmployeeModel GenerateUser(CompanyModel company)
        {
            EmployeeModel employee = new EmployeeModel();
            var faker = new Faker();

            employee.id = Guid.NewGuid();
            employee.firstName = faker.Name.FirstName();
            employee.secondName = faker.Name.LastName();
            employee.thirdName = faker.Name.JobType();
            employee.phoneSelf = GenerateInfo.MobilePhoneNumber();
            employee.phoneWork = GenerateInfo.PhoneNumber();
            employee.emailWork = (employee.secondName + ".com.ua").Replace(" ", "");
            employee.emailSelf = (employee.firstName + ".com.ua").Replace(" ", "");
            employee.login = employee.secondName;
            employee.status = new EmployeeStatusModel()
            {
                employeeStatusType = EmployeeStatus.Actual,
                employeeStatusName = EmployeeStatus.Actual.GetDisplayName(),
            };
            employee.password = GenerateInfo.Password();
            employee.addressFact = faker.Address.FullAddress();
            employee.addressReg = faker.Address.FullAddress();
            employee.photo = "/img/EmptyPhoto.jpg";
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
                rol.rolType = EmployeeRolStatus.Director;
                rol.rolName = EmployeeRolStatus.Director.GetDisplayName();
                return rol;
            }
            if (position.namePosition == positions[1])
            {
                rol.rolType = EmployeeRolStatus.Administrator;
                rol.rolName = EmployeeRolStatus.Administrator.GetDisplayName();
            }
            if (position.namePosition == positions[3] || position.namePosition == positions[4] || position.namePosition == positions[5] || position.namePosition == positions[6])
            {
                rol.rolType = EmployeeRolStatus.Project;
                rol.rolName = EmployeeRolStatus.Project.GetDisplayName();
            }
            if (position.namePosition == positions[7] || position.namePosition == positions[8] || position.namePosition == positions[9])
            {
                rol.rolType = EmployeeRolStatus.Administrator;
                rol.rolName = EmployeeRolStatus.Administrator.GetDisplayName();
            }
            return rol;
        }

        static public CompanyModel RandomCompany(CustomerModel customer)
        {
            var faker = new Faker();
            CompanyModel company = new CompanyModel();
            company.id = Guid.NewGuid();
            company.name = faker.Company.CompanyName();
            company.address = faker.Address.FullAddress();
            company.code = GenerateInfo.Code();
            company.actions = faker.Commerce.ProductAdjective();
            company.statusWeb = GenerateCompany.StatusWeb();
            company.status = GenerateCompany.Status();
            company.phone = GenerateInfo.PhoneNumber();
            company.email = (Regex.Replace(company.name, "[^0-9a-zA-Z]+", "") + ".com.ua").Replace(" ", "").ToLower();
            company.statutCapital = random.Next(1000, 300000).ToString() + " UAH";
            company.registrationDate = GenerateDate.BetweenDates(new DateTime(2000, 01, 01), new DateTime(2010, 01, 01));
            company.positions = GenerateCompany.Positions();
            company.rolls = DefaultEmployeeRolls.Get();
            company.employees = GenerateCompany.Employees(company);
            company.chief = company.employees.FirstOrDefault(u => u.position.namePosition.Equals("Директор"));
            //company.projects = GenerateProjects();
            return company;
        }
    }
}

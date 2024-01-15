using Bogus;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.Diagnostics;

namespace ReportingSystem.Data.Generate
{
    public class GenerateEmployees
    {
        static List<string> listEm = [];
        public List<EmployeeModel> Employees(CompanyModel company, Guid customerId)
        {
            List<EmployeeModel> results = [];

            if (company.Positions != null)
            {
                int i = 0;
                foreach (EmployeePositionModel userPosition in company.Positions)
                {

                    EmployeeModel user = new();
                    user = GenerateEmployee(company, customerId);
                    user.position = userPosition;
                    user.rol = new GenerateRolls().GenerateRol(user.position);
                    //userPosition.employees.Add(user);
                    results.Add(user);
                    Debug.WriteLine($"User {i} added from {company.Positions.Count}");
                    i++;
                }
            }

            return results;
        }
        public EmployeeModel GenerateEmployee(CompanyModel company, Guid customerId)
        {

            EmployeeModel employee = new();
            Random rnd = new();
            var faker = new Faker();

            employee.id = Guid.NewGuid();
            employee.companyId = company.Id;
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
                employee.emailWork = (employee.secondName.ToLower() + rnd.Next(0, 300) + "@gmail.ua").Replace(" ", "");
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
            employee.password = EncryptionHelper.Encrypt(GenerateInfo.Password());
            employee.addressFact = faker.Address.FullAddress();
            employee.addressReg = faker.Address.FullAddress();
            employee.photo = "/img/UserPhoto/John1.jpg";
            employee.salary = rnd.Next(20000, 100000);
            employee.addSalary = employee.salary * rnd.Next(10, 60) / 100.0;
            employee.birthDate = GenerateDate.BetweenDates(DateTime.Today.AddYears(-60), DateTime.Today.AddYears(-20));
            employee.workStartDate = GenerateDate.BetweenDates(company.RegistrationDate, DateTime.Today);
            employee.workEndDate = employee.workStartDate.AddDays(-1);
            employee.holidayDate = GenerateDate.RangeDates(employee.workStartDate, 3, true);
            employee.taketimeoffDate = GenerateDate.RangeDates(employee.workStartDate, 1, false);
            employee.assignmentDate = GenerateDate.RangeDates(employee.workStartDate, 1, false);


            return employee;
        }
    }
}

using Bogus;
using Microsoft.AspNetCore.SignalR;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Hubs;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.Diagnostics;

namespace ReportingSystem.Data.Generate
{
    public class GenerateEmployees(IHubContext<StatusHub> hubContext)
    {
        static List<string> listEm = [];
        public async Task<List<EmployeeModel>> Employees(CompanyModel company, Guid customerId)
        {
            try
            {
                List<EmployeeModel> results = [];

                if (company.Positions != null)
                {
                    int i = 0;
                    foreach (EmployeePositionModel userPosition in company.Positions)
                    {

                        EmployeeModel user = new();
                        user = GenerateEmployee(company, customerId);
                        user.Position = userPosition;
                        user.Rol = new GenerateRolls().GenerateRol(user.Position);
                        //userPosition.employees.Add(user);
                        results.Add(user);
                        Debug.WriteLine($"User {i} added from {company.Positions.Count}");
                        await new StatusSignalR(hubContext).UpdateStatusGenerateData($"User {i} added from {company.Positions.Count}"); ;
                        i++;
                    }
                }

                return results;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public EmployeeModel GenerateEmployee(CompanyModel company, Guid customerId)
        {

            EmployeeModel employee = new();
            Random rnd = new();
            var faker = new Faker();

            employee.Id = Guid.NewGuid();
            employee.CompanyId = company.Id;
            employee.CustomerId = customerId;
            employee.FirstName = faker.Name.FirstName();
            employee.SecondName = faker.Name.LastName();

            employee.ThirdName = faker.Name.JobType();
            employee.TaxNumber = GenerateInfo.Code();
            employee.PhoneSelf = GenerateInfo.MobilePhoneNumber();
            employee.PhoneWork = GenerateInfo.PhoneNumber();
            employee.EmailWork = (employee.SecondName.ToLower() + "@gmail.ua").Replace(" ", "");

            int maxAttempts = 10;
            int attempts = 0;

            //while (listEm.Exists(e => e.Equals(employee.EmailWork)) && attempts < maxAttempts)
            //{
            //    employee.EmailWork = (employee.SecondName.ToLower() + rnd.Next(0, 300) + "@gmail.ua").Replace(" ", "");
            //    Debug.WriteLine($"this data is exist");
            //    attempts++;
            //};
            //listEm.Add(employee.EmailWork);

            while (listEm.Exists(e => e.Equals(employee.EmailWork)) && attempts < maxAttempts)
            {
                employee.EmailWork = (employee.SecondName.ToLower() + rnd.Next(0, 300) + "@gmail.ua").Replace(" ", "");
                Debug.WriteLine($"this data is exist");

                // Перевірка унікальності нового email у списку
                while (listEm.Contains(employee.EmailWork))
                {
                    employee.EmailWork = (employee.SecondName.ToLower() + rnd.Next(0, 300) + "@gmail.ua").Replace(" ", "");
                }

                attempts++;
            };

            listEm.Add(employee.EmailWork);

            employee.EmailSelf = (employee.FirstName.ToLower() + "@gmail.ua").Replace(" ", "");
            employee.Login = employee.SecondName;
            employee.Status = new EmployeeStatusModel()
            {
                EmployeeStatusType = EmployeeStatus.Actual,
                EmployeeStatusName = EmployeeStatus.Actual.GetDisplayName(),
            };
            employee.Password = EncryptionHelper.Encrypt(GenerateInfo.Password());
            employee.AddressFact = faker.Address.FullAddress();
            employee.AddressReg = faker.Address.FullAddress();
            employee.Photo = "/img/UserPhoto/John1.jpg";
            employee.Salary = rnd.Next(20000, 100000);
            employee.AddSalary = employee.Salary * rnd.Next(10, 60) / 100.0;
            try
            {
                employee.BirthDate = GenerateDate.BetweenDates(DateTime.Today.AddYears(-60), DateTime.Today.AddYears(-20));
                employee.WorkStartDate = GenerateDate.BetweenDates(company.RegistrationDate, DateTime.Today);
                employee.WorkEndDate = employee.WorkStartDate.AddDays(-1);
                employee.HolidayDate = GenerateDate.RangeDates(employee.WorkStartDate, 3, true);
                employee.TaketimeoffDate = GenerateDate.RangeDates(employee.WorkStartDate, 1, false);
                employee.AssignmentDate = GenerateDate.RangeDates(employee.WorkStartDate, 1, false);
            }
            catch (Exception)
            {

                throw;
            }
            


            return employee;
        }
    }
}

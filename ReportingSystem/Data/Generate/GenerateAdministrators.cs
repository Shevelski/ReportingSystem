using Bogus;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
namespace ReportingSystem.Data.Generate
{
    public class GenerateAdministrators
    {
        public List<EmployeeModel> Administrators()
        {
            var x = new List<EmployeeModel>
            {
                new() {
                    Id = Guid.NewGuid(),
                    BirthDate = DateTime.Parse("10.04.1991"),
                    FirstName = "Сергій",
                    SecondName = "Наку",
                    ThirdName = "------",
                    PhoneWork = "+380666666666",
                    EmailWork = "serhii@gmail.ua",
                    Photo = "",
                    Login = "serhii",
                    Password = EncryptionHelper.Encrypt("12345"),
                    Rol = new EmployeeRolModel()
                    {
                        RolType = EmployeeRolStatus.Developer,
                        RolName = EmployeeRolStatus.Developer.GetDisplayName()
                    },
                    Status = new EmployeeStatusModel()
                    {
                        EmployeeStatusType = EmployeeStatus.Actual,
                        EmployeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                    },
                    Position = new EmployeePositionModel()
                    {
                        NamePosition = EmployeeRolStatus.Developer.GetDisplayName(),
                    },
                    WorkStartDate = DateTime.Now, 
                    WorkEndDate = DateTime.Now,
                    
                },
                new() {
                    Id = Guid.NewGuid(),
                    BirthDate = DateTime.Parse("01.01.1990"),
                    FirstName = "Олександр",
                    SecondName = "Шевельський",
                    ThirdName = "------------",
                    PhoneWork = "+380666666666",
                    EmailWork = "alex@gmail.ua",
                    Photo = "",
                    Login = "alex",
                    Password = EncryptionHelper.Encrypt("12345"),
                    Rol = new EmployeeRolModel()
                    {
                        RolType = EmployeeRolStatus.Developer,
                        RolName = EmployeeRolStatus.Developer.GetDisplayName()
                    },
                    Status = new EmployeeStatusModel()
                    {
                        EmployeeStatusType = EmployeeStatus.Actual,
                        EmployeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                    },
                    Position = new EmployeePositionModel()
                    {
                        NamePosition = EmployeeRolStatus.Developer.GetDisplayName(),
                    },
                    WorkStartDate = DateTime.Now,
                    WorkEndDate = DateTime.Now,
                },
                new() {
                    Id = Guid.NewGuid(),
                    BirthDate = DateTime.Parse("01.01.1990"),
                    FirstName = "Голопупненко",
                    SecondName = "Іван",
                    ThirdName = "Петрович",
                    PhoneWork = "+380666666666",
                    EmailWork = "golo@gmail.ua",
                    Photo = "",
                    Login = "golo",
                    Password = EncryptionHelper.Encrypt("12345"),
                    Rol = new EmployeeRolModel()
                    {
                        RolType = EmployeeRolStatus.DevAdministrator,
                        RolName = EmployeeRolStatus.DevAdministrator.GetDisplayName()
                    },
                    Status = new EmployeeStatusModel()
                    {
                        EmployeeStatusType = EmployeeStatus.Actual,
                        EmployeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                    },
                    Position = new EmployeePositionModel()
                    {
                        NamePosition = EmployeeRolStatus.DevAdministrator.GetDisplayName(),
                    },
                    WorkStartDate = DateTime.Now,
                    WorkEndDate = DateTime.Now,
                },
                new() {
                    Id = Guid.NewGuid(),
                    BirthDate = DateTime.Parse("01.01.1990"),
                    FirstName = "Стерненко",
                    SecondName = "Сергій",
                    ThirdName = "Іванович",
                    PhoneWork = "+380666666666",
                    EmailWork = "ster@gmail.ua",
                    Photo = "",
                    Login = "ster",
                    Password = EncryptionHelper.Encrypt("12345"),
                    Rol = new EmployeeRolModel()
                    {
                        RolType = EmployeeRolStatus.DevAdministrator,
                        RolName = EmployeeRolStatus.DevAdministrator.GetDisplayName()
                    },
                    Status = new EmployeeStatusModel()
                    {
                        EmployeeStatusType = EmployeeStatus.Actual,
                        EmployeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                    },
                    Position = new EmployeePositionModel()
                    {
                        NamePosition = EmployeeRolStatus.DevAdministrator.GetDisplayName(),
                    },
                    WorkStartDate = DateTime.Now,
                    WorkEndDate = DateTime.Now,
                }
            };

            return x;
        }

    }
}

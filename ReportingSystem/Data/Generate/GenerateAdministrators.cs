﻿using Bogus;
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
                    id = Guid.NewGuid(),
                    birthDate = DateTime.Parse("10.04.1991"),
                    firstName = "Сергій",
                    secondName = "Наку",
                    thirdName = "------",
                    phoneWork = "+380666666666",
                    emailWork = "serhii@gmail.ua",
                    photo = "",
                    login = "serhii",
                    password = EncryptionHelper.Encrypt("12345"),
                    rol = new EmployeeRolModel()
                    {
                        rolType = EmployeeRolStatus.Developer,
                        rolName = EmployeeRolStatus.Developer.GetDisplayName()
                    },
                    status = new EmployeeStatusModel()
                    {
                        employeeStatusType = EmployeeStatus.Actual,
                        employeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                    },
                    position = new EmployeePositionModel()
                    {
                        NamePosition = EmployeeRolStatus.Developer.GetDisplayName(),
                    },
                    workStartDate = DateTime.Now, 
                    workEndDate = DateTime.Now,
                    
                },
                new() {
                    id = Guid.NewGuid(),
                    birthDate = DateTime.Parse("01.01.1990"),
                    firstName = "Олександр",
                    secondName = "Шевельський",
                    thirdName = "------------",
                    phoneWork = "+380666666666",
                    emailWork = "alex@gmail.ua",
                    photo = "",
                    login = "alex",
                    password = EncryptionHelper.Encrypt("12345"),
                    rol = new EmployeeRolModel()
                    {
                        rolType = EmployeeRolStatus.Developer,
                        rolName = EmployeeRolStatus.Developer.GetDisplayName()
                    },
                    status = new EmployeeStatusModel()
                    {
                        employeeStatusType = EmployeeStatus.Actual,
                        employeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                    },
                    position = new EmployeePositionModel()
                    {
                        NamePosition = EmployeeRolStatus.Developer.GetDisplayName(),
                    },
                    workStartDate = DateTime.Now,
                    workEndDate = DateTime.Now,
                },
                new() {
                    id = Guid.NewGuid(),
                    birthDate = DateTime.Parse("01.01.1990"),
                    firstName = "Голопупненко",
                    secondName = "Іван",
                    thirdName = "Петрович",
                    phoneWork = "+380666666666",
                    emailWork = "golo@gmail.ua",
                    photo = "",
                    login = "golo",
                    password = EncryptionHelper.Encrypt("12345"),
                    rol = new EmployeeRolModel()
                    {
                        rolType = EmployeeRolStatus.DevAdministrator,
                        rolName = EmployeeRolStatus.DevAdministrator.GetDisplayName()
                    },
                    status = new EmployeeStatusModel()
                    {
                        employeeStatusType = EmployeeStatus.Actual,
                        employeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                    },
                    position = new EmployeePositionModel()
                    {
                        NamePosition = EmployeeRolStatus.DevAdministrator.GetDisplayName(),
                    },
                    workStartDate = DateTime.Now,
                    workEndDate = DateTime.Now,
                },
                new() {
                    id = Guid.NewGuid(),
                    birthDate = DateTime.Parse("01.01.1990"),
                    firstName = "Стерненко",
                    secondName = "Сергій",
                    thirdName = "Іванович",
                    phoneWork = "+380666666666",
                    emailWork = "ster@gmail.ua",
                    photo = "",
                    login = "ster",
                    password = EncryptionHelper.Encrypt("12345"),
                    rol = new EmployeeRolModel()
                    {
                        rolType = EmployeeRolStatus.DevAdministrator,
                        rolName = EmployeeRolStatus.DevAdministrator.GetDisplayName()
                    },
                    status = new EmployeeStatusModel()
                    {
                        employeeStatusType = EmployeeStatus.Actual,
                        employeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                    },
                    position = new EmployeePositionModel()
                    {
                        NamePosition = EmployeeRolStatus.DevAdministrator.GetDisplayName(),
                    },
                    workStartDate = DateTime.Now,
                    workEndDate = DateTime.Now,
                }
            };

            return x;
        }

    }
}

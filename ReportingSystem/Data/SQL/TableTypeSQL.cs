using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;
using ReportingSystem.Models;

namespace ReportingSystem.Data.SQL
{
    public class TableTypeSQL
    {
        public class Administrator
        {
            public Guid Id;
            public string? FirstName;
            public string? SecondName;
            public string? ThirdName;
            public string? PhoneWork;
            public string? EmailWork;
            public string? Login;
            public string? Password;
            public Guid Status;
            public DateTime BirthDate;
            public Guid Rol;
        }

        public class Customer
        {
            public Guid Id;
            public string? FirstName;
            public string? SecondName;
            public string? ThirdName;
            public Guid StatusLicenceId;
            public string? Phone;
            public string? Email;
            public string? Login;
            public string? Password;
            public Guid Status;
            public DateTime EndTimeLicense;
            public DateTime DateRegistration;
        }

        public class Employee
        {
            public Guid Id;
            public Guid CompanyId;
            public Guid CustomerId;
            public string? FirstName;
            public string? SecondName;
            public string? ThirdName;
            public string? PhoneWork;
            public string? PhoneSelf;
            public string? EmailWork;
            public string? EmailSelf;
            public string? TaxNumber;
            public string? AddressReg;
            public string? AddressFact;
            public string? Photo;
            public string? Login;
            public string? Password;
            public double Salary;
            public double AddSalary;
            public Guid Status;
            public DateTime BirthDate;
            public DateTime WorkStartDate;
            public DateTime WorkEndDate;
            public Guid Position;
            public Guid Rol;
        }

        public class Company
        {
            public Guid Id;
            public Guid CustomerId;
            public string? Name;
            public string? Address;
            public string? Code;
            public string? Actions;
            public string? StatusWeb;
            public string? Phone;
            public string? Email;
            public string? StatutCapital;
            public DateTime RegistrationDate;
            public Guid Status;
            public Guid Chief;
        }

        public class HistoryOperation
        {
            public Guid Id;
            public Guid IdCustomer;
            public DateTime DateChange;
            public DateTime OldEndDateLicence;
            public DateTime NewEndDateLicence;
            public Guid OldStatus;
            public Guid NewStatus;
            public Double Price;
            public string? Period;
            public string? NameOperation;
        }

        public class EmployeePosition
        {
            public Guid Id;
            public Guid IdCustomer;
            public Guid IdCompany;
            public int Type;
            public string? Name;
        }
    }
}

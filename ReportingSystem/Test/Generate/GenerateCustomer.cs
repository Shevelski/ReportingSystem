using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Customer;
using System;

namespace ReportingSystem.Test.Generate
{
    public static class GenerateCustomer
    {
        static Random random = new Random();
        public static DateTime LicenceDate(CustomerLicenceStatusModel status)
        {
            DateTime generateDate = DateTime.Now;

            if (status.licenceType == LicenceType.Archive)
            {
                generateDate = DateTime.Now.AddDays(random.Next(-200, -100));
            };
            if (status.licenceType == LicenceType.Main)
            {
                generateDate = DateTime.Now.AddDays(random.Next(100, 1000));
            };
            if (status.licenceType == LicenceType.Nulled)
            {
                generateDate = DateTime.Now.AddDays(-1000);
            };
            if (status.licenceType == LicenceType.Test)
            {
                generateDate = DateTime.Now.AddDays(30);
            };
            if (status.licenceType == LicenceType.Expired)
            {
                generateDate = DateTime.Now.AddDays(random.Next(-30, -5));
            };

            return generateDate;
        }

        public static CustomerLicenceStatusModel Status()
        {
            CustomerLicenceStatusModel result = new CustomerLicenceStatusModel();
            LicenceType[] values = { LicenceType.Archive, LicenceType.Test, LicenceType.Main, LicenceType.Expired, LicenceType.Nulled };
            LicenceType status = values[random.Next(values.Length)];

            result.licenceType = status;
            result.licenceName = status.GetDisplayName();
            
            return result;
        }
    }
}

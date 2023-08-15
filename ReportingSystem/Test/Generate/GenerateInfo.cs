using Bogus;
using System;

namespace ReportingSystem.Test.Generate
{
    static public class GenerateInfo
    {
        static Random random = new Random();
        static public string MobilePhoneNumber()
        {
            var faker = new Faker();
            var countryCode = "+380";
            var mobileOperatorCodes = new string[] { "50", "66", "67", "68", "96", "97", "98", "99" };
            var mobileOperatorCode = mobileOperatorCodes[random.Next(mobileOperatorCodes.Length)];
            var phoneNumber = faker.Random.Replace("#########");
            return $"{countryCode}{mobileOperatorCode}{phoneNumber}";
        }

        static public string PhoneNumber()
        {
            var faker = new Faker();
            var countryCode = "+380";
            var cityCodes = new string[] { "11", "22", "33", "44", "55", "66", "77", "88" };
            var cityCode = cityCodes[random.Next(cityCodes.Length)];
            var phoneNumber = faker.Random.Replace("#########");
            return $"{countryCode}{cityCode}{phoneNumber}";
        }

        static public string Password()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static public string Code()
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

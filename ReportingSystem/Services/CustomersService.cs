using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;
using ReportingSystem.Models.Customer;
using ReportingSystem.Utils;

namespace ReportingSystem.Services
{
    public class CustomersService
    {
        public async Task<List<CustomerModel>?> GetCustomers()
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetCustomers() :
                      await new SQLRead().GetCustomers();
            return result;
        }
        
        public async Task<CustomerModel?> GetCustomer(string idCu)
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetCustomer(idCu) :
                      await new SQLRead().GetCustomer(new Guid(idCu));
            return result;
        }

        //реєстрація замовника
        public async Task RegistrationCustomer(string[] ar)
        {
            await new JsonWrite().RegistrationCustomer(ar);
            await new SQLWrite().RegistrationCustomer(ar);
        }

        //продовження ліцензії
        public async Task RenewalLicense(string[] ar)
        {
            await new JsonWrite().RenewalLicense(ar);
            await new SQLWrite().RenewalLicense(ar);
        }

        public async Task ArchivingLicence(string[] ar)
        {
            await new JsonWrite().ArchivingLicence(ar);
            await new SQLWrite().ArchivingLicence(ar);
        }

        public async Task DeleteLicence(string[] ar)
        {
            await new JsonWrite().DeleteLicence(ar);
            await new SQLWrite().DeleteLicence(ar);
        }

        public async Task CancellationLicence(string[] ar)
        {
            await new JsonWrite().CancellationLicence(ar);
            await new SQLWrite().CancellationLicence(ar);
        }

        public async Task EditCustomer(string[] ar)
        {
            await new JsonWrite().EditCustomer(ar);
            await new SQLWrite().EditCustomer(ar);
        }
    }
}

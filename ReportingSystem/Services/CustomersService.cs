using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;
using ReportingSystem.Models.Customer;
using ReportingSystem.Utils;

namespace ReportingSystem.Services
{
    public class CustomersService
    {
        bool mode = Settings.Source().Equals("json");
        public async Task<List<CustomerModel>?> GetCustomers()
        {
            
            return mode ? await new JsonRead().GetCustomers() : await new SQLRead().GetCustomers();
        }
        
        public async Task<CustomerModel?> GetCustomer(string idCu)
        {
            return mode ? await new JsonRead().GetCustomer(idCu) : await new SQLRead().GetCustomer(new Guid(idCu));
        }

        //реєстрація замовника
        public async Task<bool> RegistrationCustomer(string[] ar)
        {
            //await (mode ? new JsonWrite().RegistrationCustomer(ar) : 
                return await new SQLWrite().RegistrationCustomer(ar);
        }

        //продовження ліцензії
        public async Task RenewalLicense(string[] ar)
        {
            //await (mode ? new JsonWrite().RenewalLicense(ar) : 
                await new SQLWrite().RenewalLicense(ar);
        }

        public async Task ArchivingLicence(string[] ar)
        {
            await (mode ? new JsonWrite().ArchivingLicence(ar) : new SQLWrite().ArchivingLicence(ar));
        }

        public async Task DeleteLicence(string[] ar)
        {
            await (mode ? new JsonWrite().DeleteLicence(ar) : new SQLWrite().DeleteLicence(ar));
        }

        public async Task CancellationLicence(string[] ar)
        {
            await (mode ? new JsonWrite().CancellationLicence(ar) : new SQLWrite().CancellationLicence(ar));
        }

        public async Task EditCustomer(string[] ar)
        {
            await (mode ? new JsonWrite().EditCustomer(ar) : new SQLWrite().EditCustomer(ar));
        }
    }
}

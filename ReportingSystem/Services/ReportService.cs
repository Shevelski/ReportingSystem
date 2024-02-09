using ReportingSystem.Enums;
using ReportingSystem;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Utils;
using ReportingSystem.Models.User;
using ReportingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;
using ReportingSystem.Models.Report;

namespace ReportingSystem.Services
{
    public class ReportService
    {
        public async Task SendReport(string[] ar)
        {
               //return mode ? await new JsonWrite().SendReport(idCu, idCo) : 
                await new SQLWrite().SendReport(ar);
        }
        public async Task ClearReport(string[] ar)
        {
               //return mode ? await new JsonWrite().SendReport(idCu, idCo) : 
                await new SQLWrite().ClearReport(ar);
        }
        public async Task ClearDayReport(string[] ar)
        {
               //return mode ? await new JsonWrite().SendReport(idCu, idCo) : 
                await new SQLWrite().ClearDayReport(ar);
        }
        public async Task<List<ReportModel>> GetReports(string idCu, string idCo, string idEm, string startDate, string endDate)
        {
            return await new SQLRead().GetReports(idCu, idCo, idEm, startDate, endDate);
        }

     }
}

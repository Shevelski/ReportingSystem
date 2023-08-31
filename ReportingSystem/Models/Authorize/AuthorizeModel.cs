using ReportingSystem.Enums;
using ReportingSystem.Models.Authorize;
using System.ComponentModel.DataAnnotations;

namespace ReportingSystem.Models.User
{
    public class AuthorizeModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Login { get; set; }
        public EmployeeRolModel? Role { get; set; }
        public AuthorizeStatusModel? AuthorizeStatusModel { get; set; }
        public EmployeeModel? Employee { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CRM.Web.Models
{
    public class LoginViewModel
    {
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}

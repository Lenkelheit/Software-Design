using System.ComponentModel.DataAnnotations;

namespace Apartments_io.ViewModels.Home
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LoginApp.ViewModel
{
    public class LoginForm
    {
        [Required]
        [Description("Username")]
        public string Username { get; set; }
        
        [Required]
        [Description("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
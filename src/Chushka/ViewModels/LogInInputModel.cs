using System.ComponentModel.DataAnnotations;

namespace Chushka.Web.ViewModels
{
    public class LogInInputModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string password { get; set; }

        [Display(Name = "Recordarme?")]
        public bool RememberMe { get; set; }
    }
}

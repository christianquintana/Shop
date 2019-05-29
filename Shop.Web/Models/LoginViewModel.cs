namespace Shop.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    // Modelo de vista para iniciar sesión
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}

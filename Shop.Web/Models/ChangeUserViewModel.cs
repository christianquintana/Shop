namespace Shop.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    // Modelo de vista para modificar usuario
    public class ChangeUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

    }
}

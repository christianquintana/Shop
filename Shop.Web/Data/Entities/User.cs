namespace Shop.Web.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser // Implementa la interface IdentityUser, tabla AspNetUsers (usuarios)
    {
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Address { get; set; }

        public int CityId { get; set; } // Se coloca debido a que no accedemos directamente a la clase User sino a traves de la clase UserHelper

        public City City { get; set; } // relacion con tabla Ciudades (.NET Core)

        [Display(Name = "Full Name")]
        public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }

    }
}

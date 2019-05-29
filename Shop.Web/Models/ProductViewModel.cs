namespace Shop.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using Data.Entities;
    using Microsoft.AspNetCore.Http;

    // Modelo de vista, extension de clase entidad Product
    public class ProductViewModel : Product // Hereda propiedades y métodos de la clase entidad Product
    {

        // .... Propiedades heredadas de la clase Product

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; } // archivo (imagen) en memoria enviado con el HttpRequest

    }
}

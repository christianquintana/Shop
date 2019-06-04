namespace Shop.Web.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    // Modelo de vista para agregar productos a la orden temporal
    public class AddItemViewModel
    {
        [Display(Name = "Product")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
        public int ProductId { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "The quantiy must be a positive number")]
        public double Quantity { get; set; }

        // Coleccion de nombre Products que es el origen de datos del DropDownList
        public IEnumerable<SelectListItem> Products { get; set; }

    }
}

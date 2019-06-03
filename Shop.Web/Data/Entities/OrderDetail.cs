namespace Shop.Web.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class OrderDetail : IEntity // Implementa la interface IEntity, obliga que haya una propiedad Id  
    {
        public int Id { get; set; }

        [Required]
        public Product Product { get; set; } // Crea la relacion con la tabla Productos, aun asi debe colocarse la notación Required ya que por defecto las relaciones en .NET Core son opcionales

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value { get { return this.Price * (decimal)this.Quantity; } } // Esta columna no se crea en base de datos por ser de solo lectura

    }
}

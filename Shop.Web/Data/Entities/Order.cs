namespace Shop.Web.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class Order : IEntity // Implementa la interface IEntity, obliga que haya una propiedad Id  
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Delivery date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? DeliveryDate { get; set; }

        [Required]
        public User User { get; set; } // Crea la relacion con la tabla Usuarios, aun asi debe colocarse la notación Required ya que por defecto las relaciones en .NET Core son opcionales

        public IEnumerable<OrderDetail> Items { get; set; } // Crea la relacion con la tabla OrderDetail

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Lines { get { return this.Items == null ? 0 : this.Items.Count(); } }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get { return this.Items == null ? 0 : this.Items.Sum(i => i.Quantity); } } // Esta columna no se crea en base de datos por ser de solo lectura

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value { get { return this.Items == null ? 0 : this.Items.Sum(i => i.Value); } } // Esta columna no se crea en base de datos por ser de solo lectura

        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? OrderDateLocal
        {
            get
            {
                if (this.OrderDate == null)
                {
                    return null;
                }

                return this.OrderDate.ToLocalTime();
            }
        }

    }
}

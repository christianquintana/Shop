namespace Shop.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    // Modelo de vista para colocar fecha de entrega a la orden
    public class DeliveryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Delivery date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }

    }
}

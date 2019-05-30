namespace Shop.Common.Models
{
    using System;
    using Newtonsoft.Json;

    // Copiar el resultado del Api del primer producto (producto/usuario) mediante Postman (entorno de desarrollo API)
    // Instalar el paquete NuGet "Newtonsoft.Json" para que funcione el decorado "JsonProperty"
    // Convertir codigo json a clases c# y extraer todos sus atributos http://json2csharp.com/
    
    // Esta clase es la que se utiliza en la aplicacion Móbil

    public class Product
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("lastPurchase")]
        public DateTime LastPurchase { get; set; }

        [JsonProperty("lastSale")]
        public DateTime LastSale { get; set; }

        [JsonProperty("isAvailabe")]
        public bool IsAvailabe { get; set; }

        [JsonProperty("stock")]
        public double Stock { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("imageFullPath")]
        public Uri ImageFullPath { get; set; }

        public override string ToString()
        {
            return $"{this.Name} {this.Price:C2}";
        }
    }
}

namespace Shop.Web.Controllers.API
{
    using Microsoft.AspNetCore.Mvc;
    using Data;

    // API: acrónimo de Application Programming Interface (Interfaz de Programación de Aplicaciones). Tradicionalmente, de forma local, una API se expone a través de archivos DLL. En la Web, 
    // una API se expone a través de Servicios Web que permiten que las aplicaciones cliente obtengan y realicen operaciones con los datos que el servicio expone.

    // Existen distintos tipos de Servicios Web que se caracterizan principalmente por la forma en que realizan la comunicación con el cliente y el formato en que intercambian información. 
    // Un ejemplo de estos servicios son los Servicios Web que utilizan SOAP (Simple Object Access Protocol) como protocolo de comunicación e intercambio de datos en formato XML. 
    // En la actualidad, una opción para exponer APIs en la Web es mediante Servicios REST. REST es un acrónimo de Representational State Transfer (Transferencia de Estado Representacional) 
    // y es un estilo de arquitectura de software para crear APIs que utilicen HTTP como su método de comunicación subyacente. 

    // Característica introducida en el .NET Core framework conocido como token de ruta. El token [controlador] reemplaza los valores del nombre del controlador de la acción o clase donde se define la ruta.
    [Route("api/[Controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;

        // Constructor que toma un parametro IProductRepository que es interface de la clase ProductRepository para CRUD (Crear, Leer, Actualizar y Borrar) de Productos
        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        // Método GET para traer todos los Productos con sus respectivos Usuarios

        // [HttpGet]: identifica una acción que soporta el método HTTP GET. 
        // IActionResult: Define un contrato que representa el resultado de un método de acción.

        [HttpGet]
        public IActionResult GetProducts()
        {
            // Invoca la interface del método GetAllWithUsers que se implementa en su clase 
            // Ok: crea un objeto OkObjectResult que produce una respuesta de Microsoft.AspNetCore.Http.StatusCodes.Status200OK
            return this.Ok(this.productRepository.GetAllWithUsers());
        }

    }
}

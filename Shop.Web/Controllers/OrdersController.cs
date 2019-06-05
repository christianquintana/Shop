namespace Shop.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Data;
    using Data.Repositories;
    using Models;

    // Especifica que la clase o el método al que se aplica este atributo requiere credenciales 
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;

        // Constructor que inyecta la IOrderRepository que es interface de la clase OrderRepository  
        // y IProductRepository que es interface de la clase ProductRepository para entre otros traer el metodo GetComboProducts
        public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            // Método para traer todas las ordenes de un usuario
            var model = await orderRepository.GetOrdersAsync(this.User.Identity.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            // Método para traer todo el detalle (temporal) de las ordenes de un usuario
            var model = await this.orderRepository.GetDetailTempsAsync(this.User.Identity.Name);

            return this.View(model);
        }

        public IActionResult AddProduct()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                // Método para traer todos los Productos para cargar el Combo
                Products = this.productRepository.GetComboProducts()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(AddItemViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Método para agregar un item en una orden 
                await this.orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);

                return this.RedirectToAction("Create");
            }

            return this.View(model);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Método para eliminar un item en una orden (temporal)
            await this.orderRepository.DeleteDetailTempAsync(id.Value);

            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> Increase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Método para aumentar(+) la cantidad de un item en una orden 
            await this.orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, 1);

            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> Decrease(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Método para disminuir(-) la cantidad de un item en una orden 
            await this.orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1);

            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> ConfirmOrder()
        {
            // TODO: ConfirmOrderAsync deberia devolver una respuesta en lugar de un booleano para poder pintarle al usuario que paso
            // Método para confirmar la orden 
            var response = await this.orderRepository.ConfirmOrderAsync(this.User.Identity.Name);

            if (response)
            {
                return this.RedirectToAction("Index");
            }

            return this.RedirectToAction("Create");
        }

    }
}

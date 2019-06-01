namespace Shop.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models;

    // Especifica que la clase o el método al que se aplica este atributo requiere credenciales y Rol
    //[Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository; //private readonly IRepository repository;

        private readonly IUserHelper userHelper;

        // Constructor que inyecta la IProductRepository que es interface de la clase ProductRepository para CRUD (Crear, Leer, Actualizar y Borrar) de Productos
        // y IUserHelper que es interface de la clase UserHelper personalizada para administrar usuarios
        public ProductsController(IProductRepository productRepository, IUserHelper userHelper) //IRepository repository,
        {
            this.productRepository = productRepository;
            //this.repository = repository;
            this.userHelper = userHelper;
        }

        // IActionResult: Define un contrato que representa el resultado de un método de acción.

        // GET: Products
        public IActionResult Index()
        {
            // Método para traer todos los productos
            return View(this.productRepository.GetAll().OrderBy(p => p.Name));
        }
       
        // async Task: permite que las operaciones cortas se ejecuten de forma asíncrona en segundo plano, Async puede ejecutar tareas de forma asíncrona en nuevos subprocesos. 
        // Las tareas asíncronas utilizan: Parámetros (parámetros que se envían a la tarea durante la ejecución.)

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Método para buscar por id el producto
            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Acción GET para crear un producto

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Acción POST para crear un producto que toma un parametro ProductViewModel 

        // POST: Products/Create
        // No es necesario colocar [Authorize(Roles = "Admin")] ya que previamente requiere pasar por el GET 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel view) //Product product)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                // Si se a cargado un archivo imagen
                if (view.ImageFile != null && view.ImageFile.Length > 0) // Length: tamaño del archivo imagen
                {
                    // GUID: identificador único global, independientemente de como se llame el archivo este se grabara con un nombre generado por un string de caracteres que nunca se va repetir
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg"; // Interpolación de cadenas en C#

                    // Combina 3 string de caracteres para generar la ruta del archivo imagen
                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Products",
                        file); //view.ImageFile.FileName);

                    // Se crea el nuevo archivo, si existe lo sobreescribe, requiere permisos de escritura 
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        // Copia de forma asíncrona el contenido del archivo cargado al target(objetivo dirigido) de destino
                        await view.ImageFile.CopyToAsync(stream);
                    }

                    // Interpolación de cadenas en C#   ~: ruta relativa
                    path = $"~/images/Products/{file}"; //{view.ImageFile.FileName}"; 
                }

                // Convertimos el ProductViewModel a Product
                var product = this.ToProduct(view, path);

                // Se carga la entidad User mediante el método que valida y trae el usuario que se a logueado
                product.User = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name); // "ceqn_20@hotmail.com");

                // Método para crear el producto
                await this.productRepository.CreateAsync(product);

                // nameof: Se utiliza para obtener el nombre de cadena simple (no calificado) de una variable, tipo o miembro y no utilizar return RedirectToAction("Index");
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

        // Método para convertir un ProductViewModel a Product
        private Product ToProduct(ProductViewModel view, string path)
        {
            return new Product
            {
                Id = view.Id,
                ImageUrl = path,
                IsAvailabe = view.IsAvailabe,
                LastPurchase = view.LastPurchase,
                LastSale = view.LastSale,
                Name = view.Name,
                Price = view.Price,
                Stock = view.Stock,
                User = view.User
            };
        }

        // Acción GET para actualizar un producto

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Método para buscar por id el producto
            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            // Convertimos el Product a ProductViewModel
            var view = this.ToProductViewModel(product);

            return View(view);
        }

        // Método para convertir un Product a ProductViewModel
        private ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                ImageUrl = product.ImageUrl,
                IsAvailabe = product.IsAvailabe,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User
            };
        }

        // Acción POST para actualizar un producto que toma un parametro ProductViewModel 

        // POST: Products/Edit/5
        // No es necesario colocar [Authorize(Roles = "Admin")] ya que previamente requiere pasar por el GET 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = view.ImageUrl;

                    // Si se a cargado un archivo imagen
                    if (view.ImageFile != null && view.ImageFile.Length > 0) // Length: tamaño del archivo imagen
                    {
                        // GUID: identificador único global, independientemente de como se llame el archivo este se grabara con un nombre generado por un string de caracteres que nunca se va repetir
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg"; // Interpolación de cadenas en C#

                        // Combina 3 string de caracteres para generar la ruta del archivo imagen
                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\Products",
                            file); //view.ImageFile.FileName);

                        // Se crea el nuevo archivo, si existe lo sobreescribe, requiere permisos de escritura 
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            // Copia de forma asíncrona el contenido del archivo cargado al target(objetivo dirigido) de destino
                            await view.ImageFile.CopyToAsync(stream);
                        }

                        // Interpolación de cadenas en C#   ~: ruta relativa
                        path = $"~/images/Products/{file}"; //{view.ImageFile.FileName}"; 
                    }

                    // Convertimos el Product a ProductViewModel
                    var product = this.ToProduct(view, path);

                    // Se carga la entidad User mediante el método que valida y trae el usuario que se a logueado
                    product.User = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name); // "ceqn_20@hotmail.com");

                    // Método para actualizar el producto
                    await this.productRepository.UpdateAsync(product);
                }                
                catch (DbUpdateConcurrencyException) // Excepción lanzada por el DbContext si no se llega a actualizar la base de datos
                {
                    // Método para validar si el producto existe 
                    if (!await this.productRepository.ExistAsync(view.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // nameof: Se utiliza para obtener el nombre de cadena simple (no calificado) de una variable, tipo o miembro y no utilizar return RedirectToAction("Index");
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Método para buscar por id el producto
            var product = await this.productRepository.GetByIdAsync(id.Value);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        // No es necesario colocar [Authorize(Roles = "Admin")] ya que previamente requiere pasar por el GET 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Método para buscar por id el producto
            var product = await this.productRepository.GetByIdAsync(id);

            // Método para borrar el producto
            await this.productRepository.DeleteAsync(product);

            return RedirectToAction(nameof(Index));
        }

        //public IActionResult Index()
        //{
        //    return View(this.repository.GetProducts());
        //}

        //public IActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = this.repository.GetProduct(id.Value);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //TODO: Change for the logged user
        //        product.User = await this.userHelper.GetUserByEmailAsync("ceqn_20@hotmail.com");
        //        this.repository.AddProduct(product);
        //        await this.repository.SaveAllAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(product);
        //}


        //public IActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = this.repository.GetProduct(id.Value);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            //TODO: Change for the logged user
        //            product.User = await this.userHelper.GetUserByEmailAsync("ceqn_20@hotmail.com");
        //            this.repository.UpdateProduct(product);
        //            await this.repository.SaveAllAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!this.repository.ProductExists(product.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(product);
        //}

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = this.repository.GetProduct(id.Value);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var product = this.repository.GetProduct(id);
        //    this.repository.RemoveProduct(product);
        //    await this.repository.SaveAllAsync();
        //    return RedirectToAction(nameof(Index));
        //}

    }
}

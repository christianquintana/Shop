
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
    using Shop.Web.Models;

    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;
        //private readonly IRepository repository;
        private readonly IUserHelper userHelper;

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper) //IRepository repository,
        {
            this.productRepository = productRepository;
            //this.repository = repository;
            this.userHelper = userHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(this.productRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel view) //Product product)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (view.ImageFile != null && view.ImageFile.Length > 0) //Length: tamaño del archivo
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg"; // independientemente de como se llame el archivo este se grabara con un string de caracteres que nunca se va repetir

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Products",
                        file); //view.ImageFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Products/{file}"; //{view.ImageFile.FileName}"; //$: interpolar ~: ruta relativa
                }

                var product = this.ToProduct(view, path); 
                product.User = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name); // "ceqn_20@hotmail.com");
                await this.productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

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

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var view = this.ToProductViewModel(product);
            return View(view);
        }

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

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = view.ImageUrl;

                    if (view.ImageFile != null && view.ImageFile.Length > 0) //Length: tamaño del archivo
                    {
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg"; // independientemente de como se llame el archivo este se grabara con un string de caracteres que nunca se va repetir, esto para que no hayan dos archivos iguales

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\Products",
                            file); //view.ImageFile.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await view.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Products/{file}"; //{view.ImageFile.FileName}"; //$: interpolar ~: ruta relativa
                    }

                    var product = this.ToProduct(view, path); 
                    product.User = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name); // "ceqn_20@hotmail.com");
                    await this.productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.productRepository.ExistAsync(view.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await this.productRepository.GetByIdAsync(id);
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

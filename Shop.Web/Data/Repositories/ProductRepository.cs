namespace Shop.Web.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    // Clase Repositorio Producto para CRUD (Crear, Leer, Actualizar y Borrar) de Productos
    public class ProductRepository : GenericRepository<Product>, IProductRepository // Hereda de GenericRepository y interface IProductRepository
    {
        private readonly DataContext context;

        // Constructor que toma un parametro DataContext 
        public ProductRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
        
        // Método para traer todos los Productos con sus respectivos Usuarios
        // IQueryable: Proporciona funcionalidad para evaluar consultas contra un origen de datos específico en el que no se especifica el tipo de datos.
        public IQueryable GetAllWithUsers()
        {
            // La entidad Products incluye (Include) una propiedad de navegación que contiene la entidad User del usuario al que está asignado el producto.
            return this.context.Products.Include(p => p.User);
        }

        public IEnumerable<SelectListItem> GetComboProducts()
        {
            var list = this.context.Products.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a product...)",
                Value = "0"
            });

            return list;
        }

    }
}

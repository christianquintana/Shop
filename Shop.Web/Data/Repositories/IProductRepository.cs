namespace Shop.Web.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;

    // interface de la clase ProductRepository para CRUD (Crear, Leer, Actualizar y Borrar) de Productos
    public interface IProductRepository : IGenericRepository<Product> // Hereda métodos de interface de la clase GenericRepository para CRUD (Crear, Leer, Actualizar y Borrar) de entidades
    {
        // .... Métodos heredados de interface de la clase GenericRepository

        // interface del método para traer todos los Productos con sus respectivos Usuarios
        // IQueryable: Proporciona funcionalidad para evaluar consultas contra un origen de datos específico en el que no se especifica el tipo de datos.
        IQueryable GetAllWithUsers();

        // interface del método para traer todos los Productos para cargar el Combo
        // IEnumerable: Expone un enumerador, que admite una iteración simple sobre una colección de un tipo especificado.
        IEnumerable<SelectListItem> GetComboProducts();

    }
}

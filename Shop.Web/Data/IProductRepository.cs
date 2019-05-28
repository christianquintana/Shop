namespace Shop.Web.Data
{
    using Entities;
    using System.Linq;

    // interface de clase Repositorio Producto para CRUD (Crear, Leer, Actualizar y Borrar) de Productos
    public interface IProductRepository : IGenericRepository<Product> // Hereda métodos de interface de clase Repositorio genérico para CRUD (Crear, Leer, Actualizar y Borrar) de entidades
    {
        // .... Métodos heredados de interface de clase Repositorio genérico

        // interface del método para traer todos los Productos con sus respectivos Usuarios
        // IQueryable: Proporciona funcionalidad para evaluar consultas contra un origen de datos específico en el que no se especifica el tipo de datos.
        IQueryable GetAllWithUsers();
    }
}

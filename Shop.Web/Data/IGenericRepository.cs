namespace Shop.Web.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    // interface de clase Repositorio genérico para CRUD (Crear, Leer, Actualizar y Borrar) de entidades
    public interface IGenericRepository<T> where T : class
    {
        // interface del método para traer todos los elementos de la entidad
        // IQueryable: Proporciona funcionalidad para evaluar consultas contra un origen de datos específico en el que no se especifica el tipo de datos.
        IQueryable<T> GetAll();

        // interface del método para buscar por id
        // Task<T> Representa una operación asíncrona que puede devolver un valor.
        Task<T> GetByIdAsync(int id);

        // interface del método para crear entidad
        Task CreateAsync(T entity);

        // interface del método para actualizar entidad
        Task UpdateAsync(T entity);

        // interface del método para borrar entidad
        Task DeleteAsync(T entity);

        // interface del método para validar si existe por id
        Task<bool> ExistAsync(int id);
    }
}





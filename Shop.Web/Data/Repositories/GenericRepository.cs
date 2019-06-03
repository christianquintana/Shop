namespace Shop.Web.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.EntityFrameworkCore;

    // Clase Repositorio genérico para CRUD (Crear, Leer, Actualizar y Borrar) de entidades    
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity // Implementa la interface IGenericRepository, IEntity
    {
        private readonly DataContext context;

        // Constructor que toma un parametro DataContext 
        public GenericRepository(DataContext context)
        {
            this.context = context;
        }

        // Método para traer todos los elementos de la entidad
        public IQueryable<T> GetAll()
        {
            return this.context.Set<T>().AsNoTracking();
        }

        // Método para buscar por id
        public async Task<T> GetByIdAsync(int id)
        {
            return await this.context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Método para crear entidad
        public async Task CreateAsync(T entity)
        {
            await this.context.Set<T>().AddAsync(entity);
            await SaveAllAsync();
        }

        // Método para actualizar entidad
        public async Task UpdateAsync(T entity)
        {
            this.context.Set<T>().Update(entity);
            await SaveAllAsync();
        }

        // Método para borrar entidad
        public async Task DeleteAsync(T entity)
        {
            this.context.Set<T>().Remove(entity);
            await SaveAllAsync();
        }

        // Método para validar si existe por id
        public async Task<bool> ExistAsync(int id)
        {
            return await this.context.Set<T>().AnyAsync(e => e.Id == id);
        }

        // Método que devuelve un valor booleano de acuerdo a los cambios realizados en el contexto en la base de datos
        public async Task<bool> SaveAllAsync()
        {
            // Procede a guardar de forma asíncrona los cambios realizados en el contexto en la base de datos, devolviendo un valor
            return await this.context.SaveChangesAsync() > 0;
        }

    }
}
namespace Shop.Web.Data
{
    using System.Linq;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Entities;

    // El DataContext es la fuente de todas las entidades asignadas a través de una conexión de base de datos. Realiza un seguimiento de los cambios realizados en todas las entidades 
    // recuperadas y mantiene una "memoria caché de identidad" que garantiza que las entidades recuperadas más de una vez se representen utilizando la misma instancia de objeto.

    // IdentityDbContext<User> representa una clase que utiliza los tipos de entidad predeterminados para los usuarios de identidad de ASP.NET, roles, notificaciones(Claims), inicios de sesión. 
    // Utilice esta sobrecarga para agregar sus propios tipos de entidad.
    public class DataContext : IdentityDbContext<User> // DbContext
    {
        // DbSet representa una colección de todas las entidades en el contexto, o que se puede consultar desde la base de datos
        public DbSet<Product> Products { get; set; }

        // User no se declara porque ya se esta heredando del DbContext
        // Siempre el nombre del modelo se escribe en Singular y el nombre de la coleccion en Plural

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderDetailTemp> OrderDetailTemps { get; set; }
         
        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }

        // Constructor que toma un parametro DbContextOptions que se utiliza para pasar los ajustes de configuración al contexto a través de la inyección de dependencia.
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Método de la clase DbContext que toma una instancia de ModelBuilder como parámetro. El framework llama a este método cuando su contexto se crea por primera vez para construir el modelo y asignaciones en memoria. 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Evitar las advertencias en la base de datos de actualización
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Deshabilitar regla de eliminación en cascada
            var cascadeFKs = modelBuilder.Model
                .G­etEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Casca­de);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restr­ict;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

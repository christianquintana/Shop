namespace Shop.Web.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Shop.Web.Models;
    
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext context;

        // Constructor que inyecta el DataContext 
        public CountryRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        // Método para traer la colección de todos los paises y ciudades
        public IQueryable GetCountriesWithCities()
        {
            return this.context.Countries
                .Include(c => c.Cities)
                .OrderBy(c => c.Name);
        }

        // Método para traer la colección de un pais y sus ciudades
        public async Task<Country> GetCountryWithCitiesAsync(int id)
        {
            return await this.context.Countries
                .Include(c => c.Cities)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        // Método para traer una ciudad
        public async Task<City> GetCityAsync(int id)
        {
            // Buscamos por el Id en la entidad Cities
            return await this.context.Cities.FindAsync(id);
        }

        // Método para agregar una ciudad a un pais
        public async Task AddCityAsync(CityViewModel model)
        {
            // Trae la colección de un pais y sus ciudades
            var country = await this.GetCountryWithCitiesAsync(model.CountryId);

            if (country == null)
            {
                return;
            }

            // Se adiciona la ciudad al pais en el contexto
            country.Cities.Add(new City { Name = model.Name });

            // Se actualiza el cambio en el contexto
            this.context.Countries.Update(country);

            // Guardo los cambios del contexto a la base de datos 
            await this.context.SaveChangesAsync();
        }

        // Método para actualizar una ciudad
        public async Task<int> UpdateCityAsync(City city)
        {
            // Buscamos si existe la ciudad en algun pais
            var country = await this.context.Countries.Where(c => c.Cities.Any(ci => ci.Id == city.Id)).FirstOrDefaultAsync();

            if (country == null)
            {
                return 0;
            }

            // Se actualiza el cambio en el contexto
            this.context.Cities.Update(city);

            // Guardo los cambios del contexto a la base de datos 
            await this.context.SaveChangesAsync();

            return country.Id;
        }

        // Método para eliminar una ciudad
        public async Task<int> DeleteCityAsync(City city)
        {
            // Buscamos si existe la ciudad en algun pais
            var country = await this.context.Countries.Where(c => c.Cities.Any(ci => ci.Id == city.Id)).FirstOrDefaultAsync();

            if (country == null)
            {
                return 0;
            }

            // Se elimina la ciudad del contexto
            this.context.Cities.Remove(city);

            // Guardo los cambios del contexto a la base de datos 
            await this.context.SaveChangesAsync();

            return country.Id;
        }

    }
}

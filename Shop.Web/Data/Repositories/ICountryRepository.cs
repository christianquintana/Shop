namespace Shop.Web.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Shop.Web.Models;
    
    public interface ICountryRepository : IGenericRepository<Country>
    {
        // interface del método para traer la colección de todos los paises y ciudades
        IQueryable GetCountriesWithCities();

        // interface del método para traer la colección de un pais y sus ciudades
        Task<Country> GetCountryWithCitiesAsync(int id);

        // interface del método para traer una ciudad
        Task<City> GetCityAsync(int id);

        // interface del método para agregar una ciudad a un pais
        Task AddCityAsync(CityViewModel model);

        // interface del método para actualizar una ciudad
        Task<int> UpdateCityAsync(City city);

        // interface del método para eliminar una ciudad
        Task<int> DeleteCityAsync(City city);

    }
}

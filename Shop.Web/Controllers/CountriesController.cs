namespace Shop.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shop.Web.Data;
    using Shop.Web.Data.Entities;
    using Shop.Web.Models;

    // Especifica que la clase o el método al que se aplica este atributo requiere credenciales y Rol
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly ICountryRepository countryRepository;

        // Constructor que inyecta la ICountryRepository que es interface de la clase CountryRepository para CRUD (Crear, Leer, Actualizar y Borrar) de paises y ciudades
        public CountriesController(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }


        // ************** Ciudades **************

        public async Task<IActionResult> AddCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Método para buscar por id un pais
            var country = await this.countryRepository.GetByIdAsync(id.Value);

            if (country == null)
            {
                return NotFound();
            }

            var model = new CityViewModel { CountryId = country.Id };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(CityViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Método para agregar una ciudad a un pais
                await this.countryRepository.AddCityAsync(model);

                return this.RedirectToAction($"Details/{model.CountryId}");
            }

            return this.View(model);
        }

        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Método para traer una ciudad
            var city = await this.countryRepository.GetCityAsync(id.Value);

            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost]
        public async Task<IActionResult> EditCity(City city)
        {
            if (this.ModelState.IsValid)
            {
                // Método para actualizar una ciudad
                var countryId = await this.countryRepository.UpdateCityAsync(city);

                if (countryId != 0)
                {
                    return this.RedirectToAction($"Details/{countryId}");
                }
            }

            return this.View(city);
        }

        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Método para traer una ciudad
            var city = await this.countryRepository.GetCityAsync(id.Value);

            if (city == null)
            {
                return NotFound();
            }

            // Método para eliminar una ciudad
            var countryId = await this.countryRepository.DeleteCityAsync(city);

            return this.RedirectToAction($"Details/{countryId}");
        }



        // ************** Paises **************

        public IActionResult Index()
        {
            // Método para traer la colección de todos los paises y ciudades
            return View(this.countryRepository.GetCountriesWithCities());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Método para traer la colección de un pais y sus ciudades
            var country = await this.countryRepository.GetCountryWithCitiesAsync(id.Value);

            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                // Método para agregar un pais
                await this.countryRepository.CreateAsync(country);

                return RedirectToAction(nameof(Index));
            }

            return View(country);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Método para buscar por id un pais
            var country = await this.countryRepository.GetByIdAsync(id.Value);

            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                // Método para actualizar un pais
                await this.countryRepository.UpdateAsync(country);

                return RedirectToAction(nameof(Index));
            }

            return View(country);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Método para buscar por id un pais
            var country = await this.countryRepository.GetByIdAsync(id.Value);

            if (country == null)
            {
                return NotFound();
            }

            // Método para eliminar un pais
            await this.countryRepository.DeleteAsync(country);

            return RedirectToAction(nameof(Index));
        }

    }
}

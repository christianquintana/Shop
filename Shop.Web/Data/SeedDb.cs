namespace Shop.Web.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Helpers;
    using Microsoft.AspNetCore.Identity;

    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userhelper;
        // Proporciona las API para administrar usuarios en un almacén de persistencia.
        //private readonly UserManager<User> userManager;
        private Random random;

        // Constructor que toma un parametro DataContext y IUserHelper
        public SeedDb(DataContext context, IUserHelper userhelper) //UserManager<User> userManager) 
        {
            this.context = context;
            this.userhelper = userhelper;
            //this.userManager = userManager;
            this.random = new Random();
        }

        // Método asincrono que crea la base de datos y todo su esquema. 
        public async Task SeedAsync()
        {
            // Asincrónicamente asegura que la base de datos para el contexto exista. Si existe, no se toma ninguna acción. Si no existe, se crean la base de datos y todo su esquema. 
            // Si la base de datos existe, no se hace ningún esfuerzo para garantizar que sea compatible con el modelo para este contexto.
            await this.context.Database.EnsureCreatedAsync();

            // Método para verificar si existe un Rol y Crear Rol
            await this.userhelper.CheckRoleAsync("Admin");
            await this.userhelper.CheckRoleAsync("Customer");

            // Si la coleccion Countries no tiene elementos 
            if (!this.context.Countries.Any())
            {
                // Se crea una coleccion de City
                var cities = new List<City>();
                cities.Add(new City { Name = "Medellín" });
                cities.Add(new City { Name = "Bogotá" });
                cities.Add(new City { Name = "Calí" });

                // Se crea un Country que incluye la coleccion de City 
                this.context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Colombia"
                });

                // Procede a guardar de forma asíncrona los cambios realizados en el contexto en la base de datos 
                await this.context.SaveChangesAsync();
            }

            //var user = await this.userManager.FindByEmailAsync("ceqn_20@hotmail.com");

            // Invoca al método GetUserByEmailAsync de la IUserHelper para validar si existe y traer el usuario ingresado
            var user = await this.userhelper.GetUserByEmailAsync("ceqn_20@hotmail.com");
            if (user == null)
            {
                // Si no existe carga la entidad User 
                user = new User
                {
                    FirstName = "Christian",
                    LastName = "Quintana",
                    Email = "ceqn_20@hotmail.com",
                    UserName = "ceqn_20@hotmail.com",
                    PhoneNumber = "99999999",
                    Address = "Calle Luna Calle Sol",
                    CityId = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault()
                };

                //var result = await this.userManager.CreateAsync(user, "123456");

                // Invoca al método AddUserAsync para crear el usuario
                var result = await this.userhelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                // Agregamos usuario creado a un Rol 
                await this.userhelper.AddUserToRoleAsync(user, "Admin");

            }

            // Valida si usuario pertenece a un Rol
            var isInRole = await this.userhelper.IsUserInRoleAsync(user, "Admin");

            if (!isInRole)
            {
                // Agregamos usuario creado a un Rol 
                await this.userhelper.AddUserToRoleAsync(user, "Admin");
            }

            // Determina si la secuencia Products no contiene algún elemento 
            if (!this.context.Products.Any())
            {
                // Invoca al método que carga la entidad Product 
                this.AddProduct("First Product", user);
                this.AddProduct("Second Product", user);
                this.AddProduct("Third Product", user);
                // Procede a guardar de forma asíncrona los cambios realizados en el contexto en la base de datos 
                await this.context.SaveChangesAsync();
            }
        }

        // Método que carga la entidad Product 
        private void AddProduct(string name, User user)
        {
            this.context.Products.Add(new Product
            {
                Name = name,
                Price = this.random.Next(100),
                IsAvailabe = true,
                Stock = this.random.Next(100),
                User = user
            });
        }

    }
}

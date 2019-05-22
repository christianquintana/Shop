namespace Shop.Web.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Helpers;
    using Microsoft.AspNetCore.Identity;

    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userhelper;
        //private readonly UserManager<User> userManager;
        private Random random;

        public SeedDb(DataContext context, IUserHelper userhelper) //UserManager<User> userManager) //Acciones rapidas y refactorizaciones / crear e inicializar campo userManager
        {
            this.context = context;
            this.userhelper = userhelper;
            //this.userManager = userManager;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            //var user = await this.userManager.FindByEmailAsync("ceqn_20@hotmail.com");
            var user = await this.userhelper.GetUserByEmailAsync("ceqn_20@hotmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Christian",
                    LastName = "Quintana",
                    Email = "ceqn_20@hotmail.com",
                    UserName = "ceqn_20@hotmail.com",
                    PhoneNumber = "99999999"
                };

                //var result = await this.userManager.CreateAsync(user, "123456");
                var result = await this.userhelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            if (!this.context.Products.Any())
            {
                this.AddProduct("First Product", user);
                this.AddProduct("Second Product", user);
                this.AddProduct("Third Product", user);
                await this.context.SaveChangesAsync();
            }
        }

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

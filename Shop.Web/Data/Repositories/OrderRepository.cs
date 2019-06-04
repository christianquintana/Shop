namespace Shop.Web.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Helpers;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;

        // Constructor que inyecta el DataContext y IUserHelper para poder manipular usuarios 
        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
        }

        // Método para traer todas las ordenes de un usuario
        public async Task<IQueryable<Order>> GetOrdersAsync(string userName)
        {
            // Se carga la entidad User mediante el método que valida y trae el usuario que se a logueado
            var user = await this.userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            // Valida si usuario pertenece a un Rol
            if (await this.userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                // El método Include especifica los objetos relacionados para incluir en los resultados de la consulta. (Orders inner join User | Orders inner join Items)
                // El método ThenInclude permite profundizar en las relaciones para incluir múltiples niveles de datos relacionados. (Items inner join Product)
                return this.context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .OrderByDescending(o => o.OrderDate);
            }

            return this.context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }

        // Método para traer todo el detalle (temporal) de las ordenes de un usuario
        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        {
            // Se carga la entidad User mediante el método que valida y trae el usuario que se a logueado
            var user = await this.userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            return this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .OrderBy(o => o.Product.Name);
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            // Se carga la entidad User mediante el método que valida y trae el usuario que se a logueado
            var user = await this.userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return;
            }

            var product = await this.context.Products.FindAsync(model.ProductId);

            if (product == null)
            {
                return;
            }

            var orderDetailTemp = await this.context.OrderDetailTemps
                .Where(odt => odt.User == user && odt.Product == product)
                .FirstOrDefaultAsync();

            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailTemp
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = model.Quantity,
                    User = user,
                };

                this.context.OrderDetailTemps.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += model.Quantity;
                this.context.OrderDetailTemps.Update(orderDetailTemp);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(id);

            if (orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += quantity;

            if (orderDetailTemp.Quantity > 0)
            {
                this.context.OrderDetailTemps.Update(orderDetailTemp);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(id);

            if (orderDetailTemp == null)
            {
                return;
            }

            this.context.OrderDetailTemps.Remove(orderDetailTemp);

            await this.context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            // Se carga la entidad User mediante el método que valida y trae el usuario que se a logueado
            var user = await this.userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return false;
            }

            var orderTmps = await this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            var details = orderTmps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity
            }).ToList();

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details,
            };

            this.context.Orders.Add(order);

            this.context.OrderDetailTemps.RemoveRange(orderTmps);

            await this.context.SaveChangesAsync();

            return true;
        }

    }
}

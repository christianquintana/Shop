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

        // Método para agregar un item en una orden 
        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            // Se carga la entidad User mediante el método que valida y trae el usuario que se a logueado
            var user = await this.userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return;
            }

            // Buscamos el ProductId en la entidad Products
            var product = await this.context.Products.FindAsync(model.ProductId);

            if (product == null)
            {
                return;
            }

            // Buscamos si ya existe el producto en la orden temporal del usuario
            var orderDetailTemp = await this.context.OrderDetailTemps
                .Where(odt => odt.User == user && odt.Product == product)
                .FirstOrDefaultAsync();

            if (orderDetailTemp == null)
            {
                // Si no existe se crea un nuevo objeto OrderDetailTemp 
                orderDetailTemp = new OrderDetailTemp
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = model.Quantity,
                    User = user,
                };

                // Se adiciona el item al contexto
                this.context.OrderDetailTemps.Add(orderDetailTemp);
            }
            else
            {
                // Si existe se adiciona la cantidad al item 
                orderDetailTemp.Quantity += model.Quantity;

                // Se actualiza el cambio en el contexto
                this.context.OrderDetailTemps.Update(orderDetailTemp);
            }

            // Guardo los cambios del contexto a la base de datos 
            await this.context.SaveChangesAsync();
        }

        // Método para aumentar(+) disminuir(-) la cantidad de un item en una orden (temporal)
        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            // Buscamos el ProductId en la entidad Products
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(id);

            if (orderDetailTemp == null)
            {
                return;
            }

            // Si existe se aumenta(+) disminuye(-) la cantidad al item 
            orderDetailTemp.Quantity += quantity;

            // Solo si la cantidad es mayor a 0
            if (orderDetailTemp.Quantity > 0)
            {
                // Se actualiza el cambio en el contexto
                this.context.OrderDetailTemps.Update(orderDetailTemp);

                // Guardo los cambios del contexto a la base de datos 
                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            // Buscamos el ProductId en la entidad Products
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(id);

            if (orderDetailTemp == null)
            {
                return;
            }
            
            // Se elimina el item del contexto
            this.context.OrderDetailTemps.Remove(orderDetailTemp);

            // Guardo los cambios del contexto a la base de datos 
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

            // Creamos una lista con los productos de la orden temporal del usuario
            var orderTmps = await this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            // Se crea un nuevo objeto convirtiendo la lista de OrderDetailTemps a OrderDetail
            var details = orderTmps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity
            }).ToList();
             
            // Se crea un nuevo objeto Order 
            var order = new Order
            {
                // Se guarda en UtcNow y se recupera en Now (local time)
                OrderDate = DateTime.UtcNow, // Obtiene un objeto DateTime que se establece en la fecha y hora actual en esta computadora, expresada como el Tiempo Universal Coordinado (UTC).
                User = user,
                Items = details,
            };

            // Se adiciona la orden al contexto
            this.context.Orders.Add(order);

            // Se elimina los items de la orden del usuario en el contexto
            this.context.OrderDetailTemps.RemoveRange(orderTmps);

            // Guardo los cambios del contexto a la base de datos 
            await this.context.SaveChangesAsync();

            return true;
        }

    }
}

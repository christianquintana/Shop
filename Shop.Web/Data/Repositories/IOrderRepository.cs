namespace Shop.Web.Data.Repositories
{
    using Entities;
    using Models;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IOrderRepository : IGenericRepository<Order>
    {
        // interface del método para traer todas las ordenes de un usuario
        Task<IQueryable<Order>> GetOrdersAsync(string userName);

        // interface del método para traer todo el detalle (temporal) de las ordenes de un usuario
        Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName);

        //// interface del método para agregar un item a una ordenes de un usuario
        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        //// interface del método para modifica la cantidad a un item de una orden (temporal)
        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity);

        Task DeleteDetailTempAsync(int id);

        Task<bool> ConfirmOrderAsync(string userName);

    }
}

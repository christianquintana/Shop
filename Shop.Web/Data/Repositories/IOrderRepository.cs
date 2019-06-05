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

        // interface del método para agregar un item en una orden 
        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        // interface del método para aumentar(+) disminuir(-) la cantidad de un item en una orden (temporal)
        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity);

        // interface del método para eliminar un item en una orden (temporal)
        Task DeleteDetailTempAsync(int id);

        // interface del método para confirmar la orden 
        Task<bool> ConfirmOrderAsync(string userName);

        // interface del método para actualizar fecha de entrega de una orden
        Task DeliverOrder(DeliveryViewModel model);

        // interface del método para traer una orden (sobrecarga GetOrdersAsync(string userName))
        Task<Order> GetOrdersAsync(int id);


    }
}

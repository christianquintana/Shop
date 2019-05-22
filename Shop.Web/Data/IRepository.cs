namespace Shop.Web.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;

    //Delete the previous IRepository 
    public interface IRepository
    {
        void AddProduct(Product product);

        Product GetProduct(int id);

        IEnumerable<Product> GetProducts();

        bool ProductExists(int id);

        void RemoveProduct(Product product);

        Task<bool> SaveAllAsync();

        void UpdateProduct(Product product);
    }
}
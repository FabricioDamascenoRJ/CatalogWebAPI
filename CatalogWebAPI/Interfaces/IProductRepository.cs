using CatalogWebAPI.Models;

namespace CatalogWebAPI.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetProductsByCategory(int id);
}

using CatalogWebAPI.Models;
using CatalogWebAPI.Repositories;

namespace CatalogWebAPI.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetProductsByCategory(int id);
}

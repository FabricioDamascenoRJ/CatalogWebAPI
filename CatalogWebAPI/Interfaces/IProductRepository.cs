using CatalogWebAPI.Models;
using CatalogWebAPI.Pagination;

namespace CatalogWebAPI.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetProducts(ProductsParameters productsParams);
    IEnumerable<Product> GetProductsByCategory(int id);
}

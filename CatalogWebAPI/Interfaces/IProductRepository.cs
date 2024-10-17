using CatalogWebAPI.Models;
using CatalogWebAPI.Pagination;

namespace CatalogWebAPI.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    PagedList<Product> GetProducts(ProductsParameters productsParams);
    PagedList<Product> GetProductsFilterPrice(ProductsFilterPrice productsFilterParams);
    IEnumerable<Product> GetProductsByCategory(int id);
}

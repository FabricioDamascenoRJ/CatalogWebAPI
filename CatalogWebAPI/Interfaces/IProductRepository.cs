using CatalogWebAPI.Models;
using CatalogWebAPI.Pagination;
using X.PagedList;

namespace CatalogWebAPI.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParams);
    Task<IPagedList<Product>> GetProductsFilterPriceAsync(ProductsFilterPrice productsFilterParams);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id);
}

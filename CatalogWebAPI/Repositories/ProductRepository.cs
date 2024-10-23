using CatalogWebAPI.Context;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using CatalogWebAPI.Pagination;
using X.PagedList;

namespace CatalogWebAPI.Repositories;


public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {        
    }    

    public async Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParams)
    {
        var products = await GetAllAsync();

        var orderedProducts = products.OrderBy(p => p.Id).AsQueryable();

        var resul = await orderedProducts.ToPagedListAsync(productsParams.PageNumber, productsParams.PageSize);

        return resul;
    }

    public async Task<IPagedList<Product>> GetProductsFilterPriceAsync(ProductsFilterPrice productsFilterParams)
    {
        var prodcts = await GetAllAsync();

        if (productsFilterParams.Price.HasValue && !string.IsNullOrEmpty(productsFilterParams.PriceCriterion))
        {
            if (productsFilterParams.PriceCriterion.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                prodcts = prodcts.Where(p => p.Price > productsFilterParams.Price.Value).OrderBy(p => p.Price);
            }
            else if (productsFilterParams.PriceCriterion.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                prodcts = prodcts.Where(p => p.Price < productsFilterParams.Price.Value).OrderBy(p => p.Price);
            }
            else if (productsFilterParams.PriceCriterion.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                prodcts = prodcts.Where(p => p.Price == productsFilterParams.Price.Value).OrderBy(p => p.Price);
            }
        }
        var productsFiltered = await prodcts.ToPagedListAsync(productsFilterParams.PageNumber, productsFilterParams.PageSize);
        return productsFiltered;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id)
    {
        var products = await GetAllAsync();
        var productsCategory = products.Where(p => p.Id == id);

        return productsCategory;
    }
}


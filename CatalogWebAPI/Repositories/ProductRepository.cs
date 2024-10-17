using CatalogWebAPI.Context;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using CatalogWebAPI.Pagination;

namespace CatalogWebAPI.Repositories;


public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {        
    }    

    public PagedList<Product> GetProducts(ProductsParameters productsParams)
    {
        var products = GetAll().OrderBy(p => p.Id).AsQueryable();
        var productsOrdered = PagedList<Product>.ToPagedList(products, productsParams.PageNumber, productsParams.PageSize);

        return productsOrdered;
    }

    public IEnumerable<Product> GetProductsByCategory(int id)
    {
        return GetAll().Where(c => c.Id == id);
    }

    public PagedList<Product> GetProductsFilterPrice(ProductsFilterPrice productsFilterParams)
    {
        var prodcts = GetAll().AsQueryable();

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
        var produtosFiltrados = PagedList<Product>.ToPagedList(prodcts, productsFilterParams.PageNumber,
                                                                                              productsFilterParams.PageSize);
        return produtosFiltrados;
    }
}


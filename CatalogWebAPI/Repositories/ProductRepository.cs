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
}


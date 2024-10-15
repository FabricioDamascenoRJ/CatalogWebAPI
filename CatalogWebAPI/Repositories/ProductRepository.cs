using CatalogWebAPI.Context;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using CatalogWebAPI.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CatalogWebAPI.Repositories;


public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {        
    }

    public IEnumerable<Product> GetProducts(ProductsParameters productsParams)
    {
        return GetAll()
            .OrderBy(p => p.Name)
            .Skip((productsParams.PageNumber - 1) * productsParams.PageSize)
            .Take(productsParams.PageSize).ToList();
    }

    public IEnumerable<Product> GetProductsByCategory(int id)
    {
        return GetAll().Where(c => c.Id == id);
    }
}


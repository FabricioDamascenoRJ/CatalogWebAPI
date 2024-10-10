using CatalogWebAPI.Context;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebAPI.Repositories;


public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {        
    }

    public IEnumerable<Product> GetProductsByCategory(int id)
    {
        return GetAll().Where(c => c.Id == id);
    }
}


using CatalogWebAPI.Context;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;

namespace CatalogWebAPI.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }    
    
}

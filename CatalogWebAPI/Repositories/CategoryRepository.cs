using CatalogWebAPI.Context;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using CatalogWebAPI.Pagination;

namespace CatalogWebAPI.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public PagedList<Category> GetCategories(CategoriesParamaters categoriesParamaters)
    {
        var categories = GetAll().OrderBy(p => p.Id).AsQueryable();
        var categoriesOrdered = PagedList<Category>.ToPagedList(categories, 
            categoriesParamaters.PageNumber, categoriesParamaters.PageSize);

        return categoriesOrdered;
    }

}

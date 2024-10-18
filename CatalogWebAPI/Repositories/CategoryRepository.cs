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

    public PagedList<Category> GetCategoriesFilterName(CategoriesFilterName categoriesParams)
    {
        var catergories = GetAll().AsQueryable();

        if(!string.IsNullOrEmpty(categoriesParams.Name))
            catergories = catergories.Where(c => c.Name == categoriesParams.Name);

        var filteredCategories = PagedList<Category>.ToPagedList(catergories,
                categoriesParams.PageNumber, categoriesParams.PageSize);

        return filteredCategories;
    }
}

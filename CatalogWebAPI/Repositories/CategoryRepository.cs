using CatalogWebAPI.Context;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using CatalogWebAPI.Pagination;
using X.PagedList;

namespace CatalogWebAPI.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public async Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParamaters categoriesParamaters)
    {
        var categories = await GetAllAsync();

        var orderedCategories = categories.OrderBy(p => p.Id).AsQueryable();

        //var result = IPagedList<Category>.ToPagedListAsync(orderedCategories, 
        //    categoriesParamaters.PageNumber, categoriesParamaters.PageSize);

        var result = await orderedCategories.ToPagedListAsync(categoriesParamaters.PageNumber, categoriesParamaters.PageSize);

        return result;
    }

    public async Task<IPagedList<Category>> GetCategoriesFilterNameAsync(CategoriesFilterName categoriesParams)
    {
        var catergories = await GetAllAsync();

        if(!string.IsNullOrEmpty(categoriesParams.Name))
            catergories = catergories.Where(c => c.Name == categoriesParams.Name);

        //var filteredCategories = PagedList<Category>.ToPagedList(catergories.AsQueryable(),
        //        categoriesParams.PageNumber, categoriesParams.PageSize);

        var filteredCategories = await catergories.ToPagedListAsync(categoriesParams.PageNumber, categoriesParams.PageSize);
        return filteredCategories;
    }
}

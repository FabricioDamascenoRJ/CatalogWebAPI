using CatalogWebAPI.Models;
using CatalogWebAPI.Pagination;
using X.PagedList;

namespace CatalogWebAPI.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParamaters categoriesParamaters);
    Task<IPagedList<Category>> GetCategoriesFilterNameAsync(CategoriesFilterName categoriesParams);
}

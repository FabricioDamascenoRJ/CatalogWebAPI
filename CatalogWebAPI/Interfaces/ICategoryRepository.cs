using CatalogWebAPI.Models;
using CatalogWebAPI.Pagination;

namespace CatalogWebAPI.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    PagedList<Category> GetCategories(CategoriesParamaters categoriesParamaters);
}

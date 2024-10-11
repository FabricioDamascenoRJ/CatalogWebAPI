using CatalogWebAPI.Models;

namespace CatalogWebAPI.DTOs.Mappings;

public static class CategoryDTOMappingExtensions
{
    public static CategoryDTO? ToCategoryDTO(this Category category)
    {
        if (category is null)
            throw new ArgumentNullException("Falha ao localizar Categoria...");

        return new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = category.ImageUrl,
        };
    }

    public static Category? ToCategory(this CategoryDTO categoryDTO)
    {
        if(categoryDTO is null)
            throw new ArgumentNullException("Falha ao localizar Categoria...");

        return new Category
        { 
            Id = categoryDTO.Id, 
            Name = categoryDTO.Name,
            ImageUrl = categoryDTO.ImageUrl,
        };
    }

    public static IEnumerable<CategoryDTO> ToCategoryDTOList(this IEnumerable<Category> categories)
    {
        if(categories is null || !categories.Any())
            return new List<CategoryDTO>();

        return categories.Select(category => new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = category.ImageUrl,
        }).ToList();
    }

}

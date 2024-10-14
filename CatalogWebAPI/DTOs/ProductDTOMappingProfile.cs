using AutoMapper;
using CatalogWebAPI.Models;

namespace CatalogWebAPI.DTOs;

public class ProductDTOMappingProfile : Profile
{
    public ProductDTOMappingProfile()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<Category, CategoryDTO>().ReverseMap();
    }
}

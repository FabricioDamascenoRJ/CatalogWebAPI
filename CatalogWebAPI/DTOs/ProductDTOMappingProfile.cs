using AutoMapper;
using CatalogWebAPI.DTOs.Request;
using CatalogWebAPI.DTOs.Response;
using CatalogWebAPI.Models;

namespace CatalogWebAPI.DTOs;

public class ProductDTOMappingProfile : Profile
{
    public ProductDTOMappingProfile()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Product, ProductDTOUpdateRequest>().ReverseMap();
        CreateMap<Product, ProductDTOUpdateResponse>().ReverseMap();
    }
}

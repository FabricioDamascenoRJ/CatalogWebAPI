using CatalogWebAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CatalogWebAPI.DTOs.Response;

public class ProductDTOUpdateResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public float Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CategoryId { get; set; }
}

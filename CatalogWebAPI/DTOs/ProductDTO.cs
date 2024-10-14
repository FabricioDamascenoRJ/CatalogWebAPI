using CatalogWebAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CatalogWebAPI.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(20, ErrorMessage = "O nome deve ter entre 5 e 20 caracteres", MinimumLength = 5)]
    public string? Name { get; set; }
    [Required(ErrorMessage = "A Descrição é obrigatéria")]
    [StringLength(300)]
    public string? Description { get; set; }
    [Required]
    
    [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
    public decimal Price { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }   
    public int CategoryId { get; set; }
    
}

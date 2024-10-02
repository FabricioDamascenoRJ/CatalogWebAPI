using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CatalogWebAPI.Models;


public class Category
{
    public Category()
    {
        Products = new Collection<Product>();
    }   
    public int Id { get; set; }
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(80, ErrorMessage = "O nome deve ter entre 5 e 80 caracteres", MinimumLength = 5)]
    public string? Name { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}

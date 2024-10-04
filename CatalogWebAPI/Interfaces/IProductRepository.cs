using CatalogWebAPI.Models;

namespace CatalogWebAPI.Interfaces;

public interface IProductRepository
{
    IEnumerable<Product> GetProducts();
    Product GetProduct(int id);
    Product Create(Product product);
    Product Update(Product product);
    Product Delete(int id);

}

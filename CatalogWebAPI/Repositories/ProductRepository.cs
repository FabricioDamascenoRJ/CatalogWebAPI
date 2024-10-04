using CatalogWebAPI.Context;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebAPI.Repositories;


public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> GetProducts()
    {
        return _context.Products.ToList();
    }

    public Product GetProduct(int id)
    {
        return _context.Products.FirstOrDefault(p => p.Id == id);
    }

    public Product Create(Product product)
    {
        if(product is null)
            throw new ArgumentNullException(nameof(product));

        _context.Products.Add(product);
        _context.SaveChanges();

        return product;
    }

    public Product Update(Product product)
    {
        if(product is null)
            throw new ArgumentNullException(nameof(product));

        _context.Entry(product).State = EntityState.Modified;
        _context.SaveChanges();
        
        return product;
    }
    

    public Product Delete(int id)
    {
        var product = _context.Products.Find(id);
        
        if(product is null)
            throw new ArgumentNullException(nameof(product));

        _context.Products.Remove(product);
        _context.SaveChanges();

        return product;
    }      
}


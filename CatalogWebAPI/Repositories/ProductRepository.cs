﻿using CatalogWebAPI.Context;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebAPI.Repositories;


public class ProductRepository : IProductRepository
{
    public readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Product> GetProducts()
    {
        return _context.Products;
    }

    public Product GetProduct(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);

        if (product is null)
            throw new InvalidOperationException("Produto não localizado");

        return product;
    }

    public Product Create(Product product)
    {
        if (product is null)
            throw new InvalidOperationException("Produto não localizado");

        _context.Products.Add(product);
        _context.SaveChanges();

        return product;
    }

    public bool Update(Product product)
    {
        if (product is null)
            throw new InvalidOperationException("Produto não localizado");

        if(_context.Products.Any(p => p.Id == product.Id))
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public bool Delete(int id)
    {
        var product = _context.Products.Find(id);

        if(product is not null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }
        return false;
    }
    
}


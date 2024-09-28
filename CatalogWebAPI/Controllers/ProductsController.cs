using CatalogWebAPI.Context;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            var products = _context.Products
                .AsNoTracking()
                .ToList();
            if (products is null)
                return NotFound();
            return products;
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product is null)
                return NotFound("Produto não encontrado.");
            return Ok(product);
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product is null)
                return BadRequest();

            _context.Products.Add(product);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetProduct",
                new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product) 
        {
            if(id != product.Id)
                return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) 
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if(product is null)
                return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok(product);
        }
    }
}

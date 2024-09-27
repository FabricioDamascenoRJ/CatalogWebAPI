using CatalogWebAPI.Context;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _context?.Products?.ToList();
            if (products is null)
                return NotFound();
            return products;
        }
    }
}

using CatalogWebAPI.Context;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Products")]
        public ActionResult<IEnumerable<Category>> GetCategoriesProducts()
        {
            return _context.Categories
                .Include(p => p.Products)
                .AsNoTracking()
                .ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll()
        {
            return _context.Categories
                .AsNoTracking()
                .ToList();
        }

        [HttpGet("{id:int}", Name = "GetCategories")]
        public ActionResult<Category> GetById(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound("Categoria não encontrada.");
            return Ok(category);
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
                return BadRequest();

            _context.Categories.Add(category);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetCategory",
                new { id = category.Id }, category);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.Id)
                return BadRequest();

            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(p => p.Id == id);

            if (category == null)
                return NotFound("Categoria não encontrada.");

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok(category);
        }
    }
}

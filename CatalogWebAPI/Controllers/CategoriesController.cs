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
            try
            {
                return _context.Categories
                .Include(p => p.Products)
                .AsNoTracking()
                .ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }
            
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll()
        {
            try
            {
                return _context.Categories
                .AsNoTracking()
                .ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }            
        }

        [HttpGet("{id:int}", Name = "GetCategories")]
        public ActionResult<Category> GetById(int id)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(c => c.Id == id);

                if (category == null)
                    return NotFound($"Categoria com o id={id} não encontrada.");
                return Ok(category);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }
            
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            try
            {
                if (category is null)
                    return BadRequest("Dados inválidos");

                _context.Categories.Add(category);
                _context.SaveChanges();

                return new CreatedAtRouteResult("GetCategory",
                    new { id = category.Id }, category);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }
            
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            try
            {
                if (id != category.Id)
                    return BadRequest("Dados inválidos");

                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(category);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(c => c.Id == id);

                if (category == null)
                    return NotFound($"Categoria com id={id} não encontrada.");

                _context.Categories.Remove(category);
                _context.SaveChanges();

                return Ok(category);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }
        }
    }
}

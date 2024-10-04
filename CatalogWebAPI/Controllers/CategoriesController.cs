using CatalogWebAPI.Context;
using CatalogWebAPI.Filters;
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
        private readonly ILogger _logger;

        public CategoriesController(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesProducts()
        {
            try
            {
                _logger.LogInformation("================== GET api/categories/products ==========");

                return await _context.Categories
                    .Include(p => p.Products)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }            
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            try
            {
                return await _context.Categories
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }            
        }

        [HttpGet("{id:int}", Name = "GetCategories")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            try
            {
                //throw new Exception("Exceção ao retornar o produto pelo Id");
                //string[] teste = null;
                //if (teste.Length > 0)
                //{

                //}
                var category = await _context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    _logger.LogWarning("=======================================");
                    _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                    _logger.LogWarning("=======================================");
                    return NotFound($"Categoria com o id={id} não encontrada.");
                }
                return Ok(category);
            }
            catch (Exception)
            {
                _logger.LogError("=======================================================================");
                _logger.LogError($"{StatusCodes.Status500InternalServerError} - Ocorreu um problema ao tratar a sua solicitação");
                _logger.LogError("=======================================================================");

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
                    return BadRequest("Falha ao cadastrar Categoria.");

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
                    return BadRequest("Falha ao alterar Categoria.");

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

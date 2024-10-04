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
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            try
            {
                var products = await _context.Products
                    .AsNoTracking()
                    .ToListAsync();

                if (products is null)
                    return NotFound("Não foi encontrado nenhum produto.");

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a solicitação");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            try
            {
                var product = await _context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product is null)
                    return NotFound($"Produto com o id={id} não encontrado.");

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a solicitação");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            try
            {
                if (product is null)
                    return BadRequest("Falha ao cadastrar Produto.");

                _context.Products.Add(product);
                _context.SaveChanges();

                return new CreatedAtRouteResult("GetProduct",
                    new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a solicitação");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product) 
        {
            try
            {
                if (id != product.Id)
                    return BadRequest("Falha ao alterar Produto.");

                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a solicitação");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) 
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);

                if (product is null)
                    return NotFound("Falha ao deletar produto.");

                _context.Products.Remove(product);
                _context.SaveChanges();

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a solicitação");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro interno ao tratar a sua solicitação. Favor contactar o Adminstrador do sistema.");
            }
        }
    }
}

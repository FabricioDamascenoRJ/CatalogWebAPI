using CatalogWebAPI.Filters;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            var products = _repository.GetProducts().ToList();
            if(products is null)
                return NotFound();
            return Ok(products);            
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _repository.GetProduct(id);
            if (product is null)
            {
                _logger.LogWarning("Produto não encontrado...");
                return NotFound("Produto não encontrado...");
            }
            return Ok(product);
            
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {            
            if (product is null)
            {
                _logger.LogWarning($"Dados Inválidos...");
                return BadRequest("Falha ao cadastrar Produto.");
            }

            var newProduct = _repository.Create(product);

            return new CreatedAtRouteResult("GetProduct", 
                new { id = newProduct.Id }, newProduct);
            
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product) 
        {            
            if (id != product.Id)
            {
                _logger.LogWarning($"Falha ao alterar Produto.");
                return BadRequest("Falha ao alterar Produto.");
            }

            bool updated = _repository.Update(product);
            if(updated) 
                return Ok(product);
            else
                return StatusCode(500, $"Falha ao atualizar produto: {nameof(Product)}");            
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) 
        {
            bool deleted = _repository.Delete(id);          

            if (deleted)
                return Ok(deleted);
            else
                return StatusCode(500, $"Falha ao excluir produto: {nameof(Product)}");

        }
    }
}

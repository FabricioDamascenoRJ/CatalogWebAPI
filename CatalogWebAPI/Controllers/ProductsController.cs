using CatalogWebAPI.Context;
using CatalogWebAPI.Filters;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var products = _repository.GetProducts();
            return Ok(products);            
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _repository.GetProduct(id);
            if (product is null)
            {
                _logger.LogWarning($"Produto com o id= {id} não encontrado...");
                return NotFound($"Produto com o id={id} não encontrado.");
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

            var productCreated = _repository.Create(product);

            return new CreatedAtRouteResult("GetProduct", new { id = productCreated.Id }, productCreated);
            
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product) 
        {            
            if (id != product.Id)
            {
                _logger.LogWarning($"Falha ao alterar Produto.");
                return BadRequest("Falha ao alterar Produto.");
            }

            _repository.Update(product);
            return Ok(product);
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) 
        {
            var product = _repository.GetProduct(id);

            if (product is null)
            {
                _logger.LogWarning($"{nameof(Product)} não localizada");
                return NotFound("Falha ao deletar produto.");
            }
            
            var productDeleted = _repository.Delete(id);
            return Ok(productDeleted);
            
        }
    }
}

using CatalogWebAPI.Filters;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using CatalogWebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        private readonly IProductRepository _productRepository;
        public ProductsController(IRepository<Product> repository, 
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _repository = repository;
        }

        [HttpGet("products/{id}")]
        public ActionResult<IEnumerable<Product>> GetProductsCategory(int id)
        {
            var products = _productRepository.GetProductsByCategory(id);
            if(products is null)
                return NotFound();
            return Ok(products);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            var products = _repository.GetAll();
            if(products is null)
                return NotFound();
            return Ok(products);            
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _repository.Get(p => p.Id == id);
            if (product is null)            
                return NotFound("Produto não encontrado...");            
            return Ok(product);            
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {            
            if (product is null)
                return BadRequest("Falha ao cadastrar Produto.");            

            var newProduct = _repository.Create(product);

            return new CreatedAtRouteResult("GetProduct", 
                new { id = newProduct.Id }, newProduct);
            
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product) 
        {            
            if (id != product.Id)
                return BadRequest("Falha ao alterar Produto.");            

            var productUpdated = _repository.Update(product);
            
            return Ok(productUpdated);                       
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) 
        {
            var product = _repository.Get(p =>  id == p.Id);

            if (product is null)
                return NotFound("Produto não encontrado...");

            var productDeleted = _repository.Delete(product);
            return Ok(productDeleted);
        }
    }
}

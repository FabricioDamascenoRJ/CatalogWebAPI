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
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("products/{id}")]
        public ActionResult<IEnumerable<Product>> GetProductsCategory(int id)
        {
            var products = _unitOfWork.ProductRepository.GetProductsByCategory(id);
            if(products is null)
                return NotFound();
            return Ok(products);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            var products = _unitOfWork.ProductRepository.GetAll();
            if(products is null)
                return NotFound();
            return Ok(products);            
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _unitOfWork.ProductRepository.Get(p => p.Id == id);
            if (product is null)            
                return NotFound("Produto não encontrado...");            
            return Ok(product);            
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {            
            if (product is null)
                return BadRequest("Falha ao cadastrar Produto.");            

            var newProduct = _unitOfWork.ProductRepository.Create(product);
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("GetProduct", 
                new { id = newProduct.Id }, newProduct);
            
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product) 
        {            
            if (id != product.Id)
                return BadRequest("Falha ao alterar Produto.");            

            var productUpdated = _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Commit();
            return Ok(productUpdated);                       
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) 
        {
            var product = _unitOfWork.ProductRepository.Get(p =>  id == p.Id);

            if (product is null)
                return NotFound("Produto não encontrado...");

            var productDeleted = _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Commit();
            return Ok(productDeleted);
        }
    }
}

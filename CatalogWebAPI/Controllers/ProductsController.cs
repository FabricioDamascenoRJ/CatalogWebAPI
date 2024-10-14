using AutoMapper;
using CatalogWebAPI.DTOs;
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
        private readonly IMapper _mapper;
        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("products/{id}")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductsCategory(int id)
        {
            var products = _unitOfWork.ProductRepository.GetProductsByCategory(id);
            if(products is null)
                return NotFound();

            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDTO);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> GetAll()
        {
            var products = _unitOfWork.ProductRepository.GetAll();
            if(products is null)
                return NotFound();

            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDTO);            
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public ActionResult<ProductDTO> GetById(int id)
        {
            var product = _unitOfWork.ProductRepository.Get(p => p.Id == id);
            if (product is null)            
                return NotFound("Produto não encontrado...");
            var productDTO = _mapper.Map<ProductDTO>(product);
            return Ok(productDTO);            
        }

        [HttpPost]
        public ActionResult<ProductDTO> Post(ProductDTO productDto)
        {            
            if (productDto is null)
                return BadRequest("Falha ao cadastrar Produto.");      
            
            var product = _mapper.Map<Product>(productDto);

            var newProduct = _unitOfWork.ProductRepository.Create(product);
            _unitOfWork.Commit();

            var newProductDto = _mapper.Map<ProductDTO>(newProduct);

            return new CreatedAtRouteResult("GetProduct", 
                new { id = newProductDto.Id }, newProductDto);
            
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProductDTO> Put(int id, ProductDTO productDto) 
        {            
            if (id != productDto.Id)
                return BadRequest();

            var product = _mapper.Map<Product>(productDto);

            var productUpdated = _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Commit();

            var productUpdatedDto = _mapper.Map<ProductDTO>(productUpdated);
            return Ok(productUpdatedDto);                       
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProductDTO> Delete(int id) 
        {
            var product = _unitOfWork.ProductRepository.Get(p =>  id == p.Id);

            if (product is null)
                return NotFound("Produto não encontrado...");

            var productDeleted = _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Commit();

            var productDeletedDto = _mapper.Map<ProductDTO>(productDeleted);
            return Ok(productDeletedDto);
        }
    }
}

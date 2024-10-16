﻿using AutoMapper;
using CatalogWebAPI.DTOs;
using CatalogWebAPI.DTOs.Request;
using CatalogWebAPI.DTOs.Response;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using CatalogWebAPI.Pagination;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

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

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<ProductDTO>> Get([FromQuery] ProductsParameters productsParameters)
        {
            var products = _unitOfWork.ProductRepository.GetProducts(productsParameters);
            return GetProducts(products);
        }

        private ActionResult<IEnumerable<ProductDTO>> GetProducts(PagedList<Product> products)
        {
            return GetProducts(products);
        }

        [HttpGet("filter/price/pagination")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductsFilterPrice([FromQuery] ProductsFilterPrice productsFilterParameters)
        {
            var products = _unitOfWork.ProductRepository.GetProductsFilterPrice(productsFilterParameters);
            return GetProducts(products);
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

        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProductDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchProductDTO)
        {
            if (patchProductDTO is null || id <= 0)
                return BadRequest();

            var product = _unitOfWork.ProductRepository.Get(c => c.Id == id);

            if (product is null)
                return NotFound();

            var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);

            patchProductDTO.ApplyTo(productUpdateRequest, ModelState);

            if(!ModelState.IsValid || TryValidateModel(productUpdateRequest))
                return BadRequest(ModelState);

            _mapper.Map(productUpdateRequest, product);

            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Commit();

            return Ok(_mapper.Map<ProductDTOUpdateResponse>(product));
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

using CatalogWebAPI.DTOs;
using CatalogWebAPI.Filters;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CatalogWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ILogger<CategoriesController> logger, IUnitOfWork unitOfWork)
        {

            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public ActionResult<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = _unitOfWork.CategoryRepository.GetAll();

            if (categories == null)
                return NotFound("Não existem Categorias cadastras...");

            var categoriesDTO = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                var categoryDTO = new CategoryDTO()
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImageUrl = category.ImageUrl,
                };
                categoriesDTO.Add(categoryDTO);
            }

            return Ok(categoriesDTO);            
        }

        [HttpGet("{id:int}", Name = "GetCategories")]
        public ActionResult<CategoryDTO> GetById(int id)
        {
            var category = _unitOfWork.CategoryRepository.Get(c => c.Id == id);

            if (category is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com o id={id} não encontrada.");
            }

            var categotyDTO = new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageUrl,
            };

            return Ok(category);                   
        }

        [HttpPost]
        public ActionResult<CategoryDTO> Post(CategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
            {
                _logger.LogWarning($"Dados Inválidos...");
                return BadRequest("Falha ao cadastrar Categoria.");
            }

            var category = new Category()
            {
                Id = categoryDTO.Id,
                Name = categoryDTO.Name,
                ImageUrl = categoryDTO.ImageUrl,
            };

            var categoryCreated = _unitOfWork.CategoryRepository.Create(category);  
            _unitOfWork.Commit();

            var newCategoryDTO = new CategoryDTO()
            {
                Id = categoryCreated.Id,
                Name = categoryCreated.Name,
                ImageUrl = categoryCreated.ImageUrl,
            };

            return new CreatedAtRouteResult("GetCategories", 
                new { id = newCategoryDTO.Id },
                newCategoryDTO);
           
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDTO)
        {            
            if (id != categoryDTO.Id)
            {
                _logger.LogWarning($"Dado Inválidos...");
                return BadRequest("Falha ao alterar Categoria.");
            }

            var category = new Category()
            {
                Id = categoryDTO.Id,
                Name = categoryDTO.Name,
                ImageUrl = categoryDTO.ImageUrl,
            };

            var categoryUpdated = _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.Commit();

            var categoryUpdatedDTO = new CategoryDTO()
            {
                Id = categoryUpdated.Id,
                Name = categoryUpdated.Name,
                ImageUrl = categoryUpdated.ImageUrl,
            };

            return Ok(categoryUpdatedDTO);                     
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoryDTO> Delete(int id)
        {
            var category = _unitOfWork.CategoryRepository.Get(c => c.Id == id);

            if (category is null)
            {
                _logger.LogWarning($"{nameof(Category)} não localizada");
                return NotFound($"Categoria com id={id} não encontrada.");
            }

            var categoryDeleted = _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.Commit();

            var categoryDeletedDTO = new CategoryDTO()
            {
                Id = categoryDeleted.Id,
                Name = categoryDeleted.Name,
                ImageUrl = categoryDeleted.ImageUrl,
            };

            return Ok(categoryDeletedDTO);            
        }
    }
}

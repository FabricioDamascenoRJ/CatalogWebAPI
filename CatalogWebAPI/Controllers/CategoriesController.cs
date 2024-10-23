using CatalogWebAPI.DTOs;
using CatalogWebAPI.DTOs.Mappings;
using CatalogWebAPI.Filters;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using CatalogWebAPI.Pagination;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

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

        private ActionResult<IEnumerable<CategoryDTO>> GetCategories(IPagedList<Category> categories)
        {
            var metadata = new
            {
                categories.Count,
                categories.PageSize,
                categories.PageCount,
                categories.TotalItemCount,
                categories.HasNextPage,
                categories.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriesDTO = categories.ToCategoryDTOList();

            return Ok(categoriesDTO);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery]
                    CategoriesParamaters categoriesParamaters)
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategoriesAsync(categoriesParamaters);
            return GetCategories(categories);
        }        

        [HttpGet("filter/name/pagination")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetFilteredCategories(
                                    [FromQuery] CategoriesFilterName categoriesFilter)
        {
            var filteredCategories = await _unitOfWork.CategoryRepository.GetCategoriesFilterNameAsync(categoriesFilter);          

            return Ok(filteredCategories);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            if (categories == null)
                return NotFound("Não existem Categorias cadastras...");

            var categoriesDTO = categories.ToCategoryDTOList();

            return Ok(categoriesDTO);            
        }

        [HttpGet("{id:int}", Name = "GetCategories")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(c => c.Id == id);

            if (category is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com o id={id} não encontrada.");
            }

            var categoryDTO = category.ToCategoryDTO();
            return Ok(categoryDTO);                   
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
            {
                _logger.LogWarning($"Dados Inválidos...");
                return BadRequest("Falha ao cadastrar Categoria.");
            }

            var category = categoryDTO.ToCategory();

            if (category == null) // Verificação adicional para evitar nulo
            {
                _logger.LogWarning($"Falha na conversão de CategoryDTO para Category...");
                return BadRequest("Falha ao converter os dados da categoria.");
            }

            var categoryCreated = _unitOfWork.CategoryRepository.Create(category);  
            await _unitOfWork.CommitAsync();

            var newCategoryDTO = categoryCreated.ToCategoryDTO();

            return new CreatedAtRouteResult("GetCategories", 
                new { id = newCategoryDTO?.Id },
                newCategoryDTO);
           
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoryDTO)
        {            
            if (id != categoryDTO.Id)
            {
                _logger.LogWarning($"Dado Inválidos...");
                return BadRequest("Falha ao alterar Categoria.");
            }

            var category = categoryDTO.ToCategory();

            if (category == null) // Verificação adicional para evitar nulo
            {
                _logger.LogWarning($"Falha na conversão de CategoryDTO para Category...");
                return BadRequest("Falha ao converter os dados da categoria.");
            }

            var categoryUpdated = _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.CommitAsync();

            var categoryUpdatedDTO = categoryUpdated.ToCategoryDTO();

            return Ok(categoryUpdatedDTO);                     
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Delete(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(c => c.Id == id);

            if (category is null)
            {
                _logger.LogWarning($"{nameof(Category)} não localizada");
                return NotFound($"Categoria com id={id} não encontrada.");
            }

            var categoryDeleted = _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.CommitAsync();

            var categoryDeletedDTO = categoryDeleted.ToCategoryDTO();

            return Ok(categoryDeletedDTO);            
        }
    }
}

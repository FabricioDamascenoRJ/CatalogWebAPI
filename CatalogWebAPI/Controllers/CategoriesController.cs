using CatalogWebAPI.Filters;
using CatalogWebAPI.Interfaces;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<Category>> GetAll()
        {
            var categories = _unitOfWork.CategoryRepository.GetAll();
            return Ok(categories);            
        }

        [HttpGet("{id:int}", Name = "GetCategories")]
        public ActionResult<Category> GetById(int id)
        {
            var category = _unitOfWork.CategoryRepository.Get(c => c.Id == id);

            if (category is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com o id={id} não encontrada.");
            }

            return Ok(category);                   
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
            {
                _logger.LogWarning($"Dados Inválidos...");
                return BadRequest("Falha ao cadastrar Categoria.");
            }

            var categoryCreated = _unitOfWork.CategoryRepository.Create(category);  
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("GetCategories", new { id = categoryCreated.Id }, categoryCreated);
           
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {            
            if (id != category.Id)
            {
                _logger.LogWarning($"Dado Inválidos...");
                return BadRequest("Falha ao alterar Categoria.");
            }

            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.Commit();
            return Ok(category);                     
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _unitOfWork.CategoryRepository.Get(c => c.Id == id);

            if (category is null)
            {
                _logger.LogWarning($"{nameof(Category)} não localizada");
                return NotFound($"Categoria com id={id} não encontrada.");
            }

            var categoryDeleted = _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.Commit();
            return Ok(categoryDeleted);            
        }
    }
}

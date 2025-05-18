using API.Models.Category;
using Application.Contracts;
using Application.Dtos.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            try
            {
                var command = new CreateCategoryCommand
                {
                    Name = request.Name
                };
                var result = await _categoryService.CreateAsync(command);
                var response = new CategoryResponse
                {
                    Id = result.Id,
                    Name = result.Name,
                    CreatedBy = result.CreatedBy,
                    CreatedAt = result.CreatedAt,
                    ModifiedBy = result.ModifiedBy,
                    ModifiedAt = result.ModifiedAt
                };
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _categoryService.GetByIdAsync(id);
                var response = new CategoryResponse
                {
                    Id = result.Id,
                    Name = result.Name,
                    CreatedBy = result.CreatedBy,
                    CreatedAt = result.CreatedAt,
                    ModifiedBy = result.ModifiedBy,
                    ModifiedAt = result.ModifiedAt,
                    RowVersion = result.RowVersion
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var results = await _categoryService.GetAllAsync();
                var response = results.Select(r => new CategoryResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                    CreatedBy = r.CreatedBy,
                    CreatedAt = r.CreatedAt,
                    ModifiedBy = r.ModifiedBy,
                    ModifiedAt = r.ModifiedAt,
                    RowVersion = r.RowVersion
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request)
        {
            try
            {
                var command = new UpdateCategoryCommand
                {
                    Name = request.Name,
                    RowVersion = request.RowVersion
                };
                var result = await _categoryService.UpdateAsync(id, command);
                var response = new CategoryResponse
                {
                    Id = result.Id,
                    Name = result.Name,
                    CreatedBy = result.CreatedBy,
                    CreatedAt = result.CreatedAt,
                    ModifiedBy = result.ModifiedBy,
                    ModifiedAt = result.ModifiedAt,
                    RowVersion = result.RowVersion
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }
    }
}

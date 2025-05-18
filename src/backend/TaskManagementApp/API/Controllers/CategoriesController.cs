using API.Models.Category;
using Application.Common;
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
            var command = new CreateCategoryCommand
            {
                Name = request.Name
            };

            var result = await _categoryService.CreateAsync(command);

            if(!result.IsSuccess)
            {
                return BadRequest(new { result.Errors });
            }

            var response = new CategoryResponse
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                CreatedBy = result.Value.CreatedBy,
                CreatedAt = result.Value.CreatedAt,
                ModifiedBy = result.Value.ModifiedBy,
                ModifiedAt = result.Value.ModifiedAt
            };
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new { result.Errors });
            }

            var response = new CategoryResponse
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                CreatedBy = result.Value.CreatedBy,
                CreatedAt = result.Value.CreatedAt,
                ModifiedBy = result.Value.ModifiedBy,
                ModifiedAt = result.Value.ModifiedAt,
                RowVersion = result.Value.RowVersion
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var results = await _categoryService.GetAllAsync();

            if (!results.IsSuccess)
            {
                return BadRequest(new { results.Errors });
            }


            var response = results.Value.Select(r => new CategoryResponse
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request)
        {
            var command = new UpdateCategoryCommand
            {
                Name = request.Name,
                RowVersion = request.RowVersion
            };

            var result = await _categoryService.UpdateAsync(id, command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { result.Errors });
            }

            var response = new CategoryResponse
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                CreatedBy = result.Value.CreatedBy,
                CreatedAt = result.Value.CreatedAt,
                ModifiedBy = result.Value.ModifiedBy,
                ModifiedAt = result.Value.ModifiedAt,
                RowVersion = result.Value.RowVersion
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new { result.Errors });
            }

            return NoContent();            
        }
    }
}

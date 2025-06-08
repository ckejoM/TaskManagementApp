using API.Models;
using API.Models.Auth;
using API.Models.Category;
using Application.Common;
using Application.Contracts;
using Application.Dtos.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [ProducesResponseType(typeof(CategoryResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
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
        [ProducesResponseType(typeof(CategoryResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
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
        [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            var response = result.Value.Select(r => new CategoryResponse
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
        [ProducesResponseType(typeof(CategoryResponse), (int)HttpStatusCode.OK)] // For 200 OK
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)] // For 400 Bad Request
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
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
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
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            return NoContent();            
        }
    }
}

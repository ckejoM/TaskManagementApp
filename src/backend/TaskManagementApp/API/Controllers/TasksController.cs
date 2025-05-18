using API.Models.Task;
using Application.Contracts;
using Application.Dtos.Task;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class TasksController : BaseController
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
        {
            try
            {
                var command = new CreateTaskCommand
                {
                    Title = request.Title,
                    Description = request.Description,
                    ProjectId = request.ProjectId,
                    CategoryId = request.CategoryId
                };
                var result = await _taskService.CreateAsync(command);
                var response = new TaskResponse
                {
                    Id = result.Id,
                    Title = result.Title,
                    Description = result.Description,
                    ProjectId = result.ProjectId,
                    CategoryId = result.CategoryId,
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
                var result = await _taskService.GetByIdAsync(id);
                var response = new TaskResponse
                {
                    Id = result.Id,
                    Title = result.Title,
                    Description = result.Description,
                    ProjectId = result.ProjectId,
                    CategoryId = result.CategoryId,
                    CreatedBy = result.CreatedBy,
                    CreatedAt = result.CreatedAt,
                    ModifiedBy = result.ModifiedBy,
                    ModifiedAt = result.ModifiedAt
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
                var results = await _taskService.GetAllAsync();
                var response = results.Select(r => new TaskResponse
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    ProjectId = r.ProjectId,
                    CategoryId = r.CategoryId,
                    CreatedBy = r.CreatedBy,
                    CreatedAt = r.CreatedAt,
                    ModifiedBy = r.ModifiedBy,
                    ModifiedAt = r.ModifiedAt
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request)
        {
            try
            {
                var command = new UpdateTaskCommand
                {
                    Title = request.Title,
                    Description = request.Description,
                    ProjectId = request.ProjectId,
                    CategoryId = request.CategoryId
                };
                var result = await _taskService.UpdateAsync(id, command);
                var response = new TaskResponse
                {
                    Id = result.Id,
                    Title = result.Title,
                    Description = result.Description,
                    ProjectId = result.ProjectId,
                    CategoryId = result.CategoryId,
                    CreatedBy = result.CreatedBy,
                    CreatedAt = result.CreatedAt,
                    ModifiedBy = result.ModifiedBy,
                    ModifiedAt = result.ModifiedAt
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
                await _taskService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }
    }
}

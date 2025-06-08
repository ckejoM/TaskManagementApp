using API.Models;
using API.Models.Project;
using API.Models.Task;
using Application.Contracts;
using Application.Dtos.Task;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [ProducesResponseType(typeof(TaskResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
        {
            var command = new CreateTaskCommand
            {
                Title = request.Title,
                Description = request.Description,
                ProjectId = request.ProjectId,
                CategoryId = request.CategoryId,
            };
            var result = await _taskService.CreateAsync(command);

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            var resultValue = result.Value;

            var response = new TaskResponse
                {
                    Id = resultValue.Id,
                    Title = resultValue.Title,
                    Description = resultValue.Description,
                    ProjectId = resultValue.ProjectId,
                    CategoryId = resultValue.CategoryId,
                    CreatedBy = resultValue.CreatedBy,
                    CreatedAt = resultValue.CreatedAt,
                    ModifiedBy = resultValue.ModifiedBy,
                    ModifiedAt = resultValue.ModifiedAt,
                    RowVersion = resultValue.RowVersion
                };

            return CreatedAtAction(nameof(GetById), new { id = resultValue.Id }, response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _taskService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            var response = new TaskResponse
            {
                Id = result.Value.Id,
                Title = result.Value.Title,
                Description = result.Value.Description,
                ProjectId = result.Value.ProjectId,
                CategoryId = result.Value.CategoryId,
                CreatedBy = result.Value.CreatedBy,
                CreatedAt = result.Value.CreatedAt,
                ModifiedBy = result.Value.ModifiedBy,
                ModifiedAt = result.Value.ModifiedAt,
                RowVersion = result.Value.RowVersion
            };

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _taskService.GetAllAsync();

            if(!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }
            
            var response = result.Value.Select(r => new TaskResponse
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                ProjectId = r.ProjectId,
                CategoryId = r.CategoryId,
                CreatedBy = r.CreatedBy,
                CreatedAt = r.CreatedAt,
                ModifiedBy = r.ModifiedBy,
                ModifiedAt = r.ModifiedAt,
                RowVersion = r.RowVersion
            });

            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TaskResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request)
        {
            var command = new UpdateTaskCommand
            {
                Title = request.Title,
                Description = request.Description,
                ProjectId = request.ProjectId,
                CategoryId = request.CategoryId,
                RowVersion = request.RowVersion
            };
            var result = await _taskService.UpdateAsync(id, command);

            if(!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            var response = new TaskResponse
            {
                Id = result.Value.Id,
                Title = result.Value.Title,
                Description = result.Value.Description,
                ProjectId = result.Value.ProjectId,
                CategoryId = result.Value.CategoryId,
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
            var result = await _taskService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            return NoContent();            
        }
    }
}

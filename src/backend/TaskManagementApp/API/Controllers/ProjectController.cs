using API.Models;
using API.Models.Project;
using Application.Common;
using Application.Contracts;
using Application.Dtos.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProjectResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
        {
            var command = new CreateProjectCommand
            {
                Name = request.Name,
                Description = request.Description
            };

            var result = await _projectService.CreateAsync(command);

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            var response = new ProjectResponse
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Description = result.Value.Description,
                CreatedBy = result.Value.CreatedBy,
                CreatedOn = result.Value.CreatedOn,
                ModifiedBy = result.Value.ModifiedBy,
                ModifiedOn = result.Value.ModifiedOn
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetById(Guid id)
        {
             var result = await _projectService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            var response = new ProjectResponse
                {
                    Id = result.Value.Id,
                    Name = result.Value.Name,
                    Description = result.Value.Description,
                    CreatedBy = result.Value.CreatedBy,
                    CreatedOn = result.Value.CreatedOn,
                    ModifiedBy = result.Value.ModifiedBy,
                    ModifiedOn = result.Value.ModifiedOn,
                    RowVersion = result.Value.RowVersion
                };

            return Ok(response);            
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _projectService.GetAllAsync();

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            var response = result.Value.Select(r => new ProjectResponse
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                CreatedBy = r.CreatedBy,
                CreatedOn = r.CreatedOn,
                ModifiedBy = r.ModifiedBy,
                ModifiedOn = r.ModifiedOn,
                RowVersion = r.RowVersion
            });

            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProjectResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request)
        {
            var command = new UpdateProjectCommand
            {
                Name = request.Name,
                Description = request.Description,
                RowVersion = request.RowVersion
            };

            var result = await _projectService.UpdateAsync(id, command);

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            var response = new ProjectResponse
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Description = result.Value.Description,
                CreatedBy = result.Value.CreatedBy,
                CreatedOn = result.Value.CreatedOn,
                ModifiedBy = result.Value.ModifiedBy,
                ModifiedOn = result.Value.ModifiedOn,
                RowVersion = result.Value.RowVersion
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _projectService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            return NoContent();
        }
    }
}

using API.Models.Project;
using Application.Contracts;
using Application.Dtos.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
        {
            try
            {
                var command = new CreateProjectCommand
                {
                    Name = request.Name,
                    Description = request.Description
                };

                var result = await _projectService.CreateAsync(command);

                var response = new ProjectResponse
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    CreatedBy = result.CreatedBy,
                    CreatedOn = result.CreatedOn,
                    ModifiedBy = result.ModifiedBy,
                    ModifiedOn = result.ModifiedOn
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
                var result = await _projectService.GetByIdAsync(id);

                var response = new ProjectResponse
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    CreatedBy = result.CreatedBy,
                    CreatedOn = result.CreatedOn,
                    ModifiedBy = result.ModifiedBy,
                    ModifiedOn = result.ModifiedOn
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
                var results = await _projectService.GetAllAsync();

                var response = results.Select(r => new ProjectResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    CreatedBy = r.CreatedBy,
                    CreatedOn = r.CreatedOn,
                    ModifiedBy = r.ModifiedBy,
                    ModifiedOn = r.ModifiedOn
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request)
        {
            try
            {
                var command = new UpdateProjectCommand
                {
                    Name = request.Name,
                    Description = request.Description
                };

                var result = await _projectService.UpdateAsync(id, command);

                var response = new ProjectResponse
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    CreatedBy = result.CreatedBy,
                    CreatedOn = result.CreatedOn,
                    ModifiedBy = result.ModifiedBy,
                    ModifiedOn = result.ModifiedOn
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
                await _projectService.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }
    }
}

using Application.Dtos.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IProjectService
    {
        Task<ProjectDto> CreateAsync(CreateProjectCommand command);
        Task<ProjectDto> GetByIdAsync(Guid id);
        Task<List<ProjectDto>> GetAllAsync();
        Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectCommand command);
        Task DeleteAsync(Guid id);        
    }
}

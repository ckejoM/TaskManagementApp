using Application.Common;
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
        Task<Result<ProjectDto>> CreateAsync(CreateProjectCommand command);
        Task<Result<ProjectDto>> GetByIdAsync(Guid id);
        Task<Result<List<ProjectDto>>> GetAllAsync();
        Task<Result<ProjectDto>> UpdateAsync(Guid id, UpdateProjectCommand command);
        Task<Result<Guid>> DeleteAsync(Guid id);        
    }
}

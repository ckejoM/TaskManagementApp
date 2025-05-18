using Application.Common;
using Application.Dtos.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ITaskService
    {
        Task<Result<TaskDto>> CreateAsync(CreateTaskCommand command);
        Task<Result<TaskDto>> GetByIdAsync(Guid id);
        Task<Result<List<TaskDto>>> GetAllAsync();
        Task<Result<TaskDto>> UpdateAsync(Guid id, UpdateTaskCommand command);
        Task<Result<Guid>> DeleteAsync(Guid id);
    }
}

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
        Task<TaskDto> CreateAsync(CreateTaskCommand command);
        Task<TaskDto> GetByIdAsync(Guid id);
        Task<List<TaskDto>> GetAllAsync();
        Task<TaskDto> UpdateAsync(Guid id, UpdateTaskCommand command);
        Task DeleteAsync(Guid id);
    }
}

using Application.Common;
using Application.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ICategoryService
    {
        Task<Result<CategoryDto>> CreateAsync(CreateCategoryCommand command);
        Task<Result<CategoryDto>> GetByIdAsync(Guid id);
        Task<Result<List<CategoryDto>>> GetAllAsync();
        Task<Result<CategoryDto>> UpdateAsync(Guid id, UpdateCategoryCommand command);
        Task<Result<Guid>> DeleteAsync(Guid id);
    }
}

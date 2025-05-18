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
        Task<CategoryDto> CreateAsync(CreateCategoryCommand command);
        Task<CategoryDto> GetByIdAsync(Guid id);
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryCommand command);
        Task DeleteAsync(Guid id);
    }
}

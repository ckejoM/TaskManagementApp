using Application.Common;
using Application.Contracts;
using Application.Dtos.Category;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CategoryService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Result<CategoryDto>> CreateAsync(CreateCategoryCommand command)
        {
            
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return Result<CategoryDto>.Failure("Category name is required");
            }

            var category = new Category
            {
                Name = command.Name,
                CreatedOn = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Result<CategoryDto>.Success(CategoryDto.FromEntity(category));
        }

        public async Task<Result<CategoryDto>> GetByIdAsync(Guid id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if(category is null)
            {
                return Result<CategoryDto>.Failure("Category not found");
            }

            return Result<CategoryDto>.Success(CategoryDto.FromEntity(category));
        }

        public async Task<Result<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            if(categories is null)
            {
                return Result<List<CategoryDto>>.Failure("Categories not found");
            }

            return Result<List<CategoryDto>>.Success(categories.Select(CategoryDto.FromEntity).ToList());
        }

        public async Task<Result<CategoryDto>> UpdateAsync(Guid id, UpdateCategoryCommand command)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if(category is null)
            {
                return Result<CategoryDto>.Failure("Category not found");
            }

            if (!category.RowVersion.SequenceEqual(command.RowVersion))
            {
                return Result<CategoryDto>.Failure("There were changes on Category, please refresh and try again.");
            }

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return Result<CategoryDto>.Failure("Category name is required");
            }

            category.Name = command.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Category update failed due to concurrent changes. Please refresh and try again.");
            }

            return Result<CategoryDto>.Success(CategoryDto.FromEntity(category));
        }

        public async System.Threading.Tasks.Task<Result<Guid>> DeleteAsync(Guid id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if(category is null)
            {
                return Result<Guid>.Failure("Category not found");
            }

            category.IsDeleted = true;
            category.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Result<Guid>.Success(id);
        }
    }
}

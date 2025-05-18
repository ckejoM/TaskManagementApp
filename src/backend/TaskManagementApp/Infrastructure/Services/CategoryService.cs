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

        public async Task<CategoryDto> CreateAsync(CreateCategoryCommand command)
        {
            var userId = _currentUserService.GetCurrentUserId()
                ?? throw new Exception("User not authenticated");

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new Exception("Category name is required");

            var category = new Category
            {
                Name = command.Name,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CategoryDto.FromEntity(category);
        }

        public async Task<CategoryDto> GetByIdAsync(Guid id)
        {
            var userId = _currentUserService.GetCurrentUserId()
                ?? throw new Exception("User not authenticated");

            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted)
                ?? throw new Exception("Category not found");

            return CategoryDto.FromEntity(category);
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var userId = _currentUserService.GetCurrentUserId()
                ?? throw new Exception("User not authenticated");

            var categories = await _context.Categories
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            return categories.Select(CategoryDto.FromEntity).ToList();
        }

        public async Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryCommand command)
        {
            var userId = _currentUserService.GetCurrentUserId()
                ?? throw new Exception("User not authenticated");

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted && c.CreatedBy == userId)
                ?? throw new Exception("Category not found");

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new Exception("Category name is required");

            category.Name = command.Name;
            category.ModifiedBy = userId;
            category.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return CategoryDto.FromEntity(category);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid id)
        {
            var userId = _currentUserService.GetCurrentUserId()
                ?? throw new Exception("User not authenticated");

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted && c.CreatedBy == userId)
                ?? throw new Exception("Category not found");

            category.IsDeleted = true;
            category.ModifiedBy = userId;
            category.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}

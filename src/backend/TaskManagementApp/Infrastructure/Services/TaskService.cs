using Application.Contracts;
using Application.Dtos.Task;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    internal class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public TaskService(ICurrentUserService currentUserService, ApplicationDbContext context)
        {
            _currentUserService = currentUserService;
            _context = context;
        }

        public async Task<TaskDto> CreateAsync(CreateTaskCommand command)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == command.ProjectId && !p.IsDeleted);

            if(project == null)
            {
                throw new Exception("Project not found or not owned by user");
            }
            if (command.CategoryId.HasValue)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == command.CategoryId && !c.IsDeleted);

                if(category == null)
                {
                    throw new Exception("Category not found or not owned by user");
                }
            }

            var task = new Domain.Entities.Task
            {
                Title = command.Title,
                Description = command.Description,
                ProjectId = command.ProjectId,
                CategoryId = command.CategoryId,
                CreatedBy = _currentUserService.GetCurrentUserId(),
                CreatedOn = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return TaskDto.FromEntity(task);
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
            ?? throw new Exception("Task not found");

            task.IsDeleted = true;
            task.ModifiedBy = _currentUserService.GetCurrentUserId();
            task.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<List<TaskDto>> GetAllAsync()
        {
            var tasks = await _context.Tasks
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .ToListAsync();

            return tasks.Select(TaskDto.FromEntity).ToList();
        }

        public async Task<TaskDto> GetByIdAsync(Guid id)
        {
            var task = await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
                ?? throw new Exception("Task not found");

            return TaskDto.FromEntity(task);
        }

        public async Task<TaskDto> UpdateAsync(Guid id, UpdateTaskCommand command)
        {
            var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
            ?? throw new Exception("Task not found");

            // Validate ProjectId
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == command.ProjectId && !p.IsDeleted)
                ?? throw new Exception("Project not found or not owned by user");

            // Validate CategoryId (if provided)
            if (command.CategoryId.HasValue)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == command.CategoryId && !c.IsDeleted)
                    ?? throw new Exception("Category not found or not owned by user");
            }

            task.Title = command.Title;
            task.Description = command.Description;
            task.ProjectId = command.ProjectId;
            task.CategoryId = command.CategoryId;
            task.ModifiedBy = _currentUserService.GetCurrentUserId();
            task.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return TaskDto.FromEntity(task);
        }
    }
}

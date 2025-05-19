using Application.Common;
using Application.Contracts;
using Application.Dtos.Task;
using Application.Validators;
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
        private readonly IValidatorFactory _validatorFactory;

        public TaskService(
            ICurrentUserService currentUserService,
            ApplicationDbContext context, 
            IValidatorFactory validatorFactory)
        {
            _currentUserService = currentUserService;
            _context = context;
            _validatorFactory = validatorFactory;
        }

        public async Task<Result<TaskDto>> CreateAsync(CreateTaskCommand command)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == command.ProjectId && !p.IsDeleted);

            if(project == null)
            {
                return Result<TaskDto>.Failure("Project not found or not owned by user");
            }
            if (command.CategoryId.HasValue)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == command.CategoryId && !c.IsDeleted);

                if(category == null)
                {
                    return Result<TaskDto>.Failure("Category not found or not owned by user");
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

            return Result<TaskDto>.Success(TaskDto.FromEntity(task));
        }

        public async Task<Result<Guid>> DeleteAsync(Guid id)
        {
            var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            if(task is null)
            {
                return Result<Guid>.Failure("Task not found");
            }

            task.IsDeleted = true;
            task.ModifiedBy = _currentUserService.GetCurrentUserId();
            task.ModifiedOn = DateTime.UtcNow;

            return Result<Guid>.Success(id);
        }

        public async Task<Result<List<TaskDto>>> GetAllAsync()
        {
            var tasks = await _context.Tasks
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .ToListAsync();

            if(tasks is null)
            {
                return Result<List<TaskDto>>.Failure("Tasks not found");
            }

            return Result<List<TaskDto>>.Success(tasks.Select(TaskDto.FromEntity).ToList());
        }

        public async Task<Result<TaskDto>> GetByIdAsync(Guid id)
        {
            var task = await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            if(task is null)
            {
                return Result<TaskDto>.Failure("Task not found");
            }

            return Result<TaskDto>.Success(TaskDto.FromEntity(task));
        }

        public async Task<Result<TaskDto>> UpdateAsync(Guid id, UpdateTaskCommand command)
        {
            var validator = _validatorFactory.GetValidator<UpdateTaskCommand>();

            if(validator is not null)
            {
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    return Result<TaskDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
                }
            }

            var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            if(task is null)
            {
                return Result<TaskDto>.Failure("Task not found");
            }

            if(!task.RowVersion.SequenceEqual(command.RowVersion))
            {
                return Result<TaskDto>.Failure("There were changes on Task, please refresh and try again.");
            }

            // Validate ProjectId
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == command.ProjectId && !p.IsDeleted);

            if(project == null)
            {
                return Result<TaskDto>.Failure("Project not found or not owned by user");
            }

            // Validate CategoryId (if provided)
            if (command.CategoryId.HasValue)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == command.CategoryId && !c.IsDeleted);

                if(category == null)
                {
                    return Result<TaskDto>.Failure("Category not found or not owned by user");
                }
            }

            task.Title = command.Title;
            task.Description = command.Description;
            task.ProjectId = command.ProjectId;
            task.CategoryId = command.CategoryId;
            task.ModifiedBy = _currentUserService.GetCurrentUserId();
            task.ModifiedOn = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Task update failed due to concurrent changes. Please refresh and try again.");
            }

            return Result<TaskDto>.Success(TaskDto.FromEntity(task));
        }
    }
}

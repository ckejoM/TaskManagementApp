using Application.Common;
using Application.Contracts;
using Application.Dtos.Project;
using Application.Validators;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidatorFactory _validatorFactory;

        public ProjectService(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IValidatorFactory validatorFactory)
        {
            _context = context;
            _currentUserService = currentUserService;
            _validatorFactory = validatorFactory;
        }

        public async Task<Result<ProjectDto>> CreateAsync(CreateProjectCommand command)
        {
            var project = new Project
            {
                Name = command.Name,
                Description = command.Description,
                CreatedBy = _currentUserService.GetCurrentUserId(),
                CreatedOn = DateTime.UtcNow
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Result<ProjectDto>.Success(ProjectDto.FromEntity(project));
        }

        public async Task<Result<ProjectDto>> GetByIdAsync(Guid id)
        {
            var project = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if(project is null)
            {
                return Result<ProjectDto>.Failure("Project not found");
            }

            return Result<ProjectDto>.Success(ProjectDto.FromEntity(project));
        }

        public async Task<Result<List<ProjectDto>>> GetAllAsync()
        {
            throw new Exception();
            var userId = _currentUserService.GetCurrentUserId();
            var projects = await _context.Projects
                .AsNoTracking()
                .ToListAsync();

            if(projects is null)
            {
                return Result<List<ProjectDto>>.Failure("Projects not found");
            }

            return Result<List<ProjectDto>>.Success(projects.Select(ProjectDto.FromEntity).ToList());
        }

        public async Task<Result<ProjectDto>> UpdateAsync(Guid id, UpdateProjectCommand command)
        {
            var validator = _validatorFactory.GetValidator<UpdateProjectCommand>();

            if (validator is not null)
            {
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    return Result<ProjectDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
                }
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if(project is null)
            {
                return Result<ProjectDto>.Failure("Project not found");
            }

            if (!project.RowVersion.SequenceEqual(command.RowVersion))
            {
                return Result<ProjectDto>.Failure("There were changes on Project, please refresh and try again.");
            }

            project.Name = command.Name;
            project.Description = command.Description;
            project.ModifiedBy = _currentUserService.GetCurrentUserId();
            project.ModifiedOn = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Project update failed due to concurrent changes. Please refresh and try again.");
            }

            return Result<ProjectDto>.Success(ProjectDto.FromEntity(project));
        }

        public async Task<Result<Guid>> DeleteAsync(Guid id)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (project is null)
            {
                return Result<Guid>.Failure("Project not found");
            }

            project.IsDeleted = true;
            project.ModifiedBy = _currentUserService.GetCurrentUserId();
            project.ModifiedOn = DateTime.UtcNow;

            return Result<Guid>.Success(id);
        }
    }
}

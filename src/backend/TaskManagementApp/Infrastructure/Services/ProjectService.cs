using Application.Contracts;
using Application.Dtos.Project;
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

        public ProjectService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectCommand command)
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

            return ProjectDto.FromEntity(project);
        }

        public async Task<ProjectDto> GetByIdAsync(Guid id)
        {
            var project = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
                ?? throw new Exception("Project not found");

            return ProjectDto.FromEntity(project);
        }

        public async Task<List<ProjectDto>> GetAllAsync()
        {
            var userId = _currentUserService.GetCurrentUserId();
            var projects = await _context.Projects
                .AsNoTracking()
                .Where(p => p.IsDeleted == false && p.CreatedBy == userId)
                .ToListAsync();

            return projects.Select(ProjectDto.FromEntity).ToList();
        }

        public async Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectCommand command)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
                ?? throw new Exception("Project not found");

            project.Name = command.Name;
            project.Description = command.Description;
            project.ModifiedBy = _currentUserService.GetCurrentUserId();
            project.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ProjectDto.FromEntity(project);
        }

        public async Task DeleteAsync(Guid id)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
                ?? throw new Exception("Project not found");

            project.IsDeleted = true;
            project.ModifiedBy = _currentUserService.GetCurrentUserId();
            project.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}

using Application.Dtos.Project;
using Application.Dtos.Task;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Project
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Project name is required")
                .MaximumLength(100).WithMessage("Project name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Project description cannot exceed 500 characters");

            RuleFor(x => x.RowVersion)
                .NotEmpty().WithMessage("Row version is required");
        }
    }
}

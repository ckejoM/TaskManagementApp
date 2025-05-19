using Application.Dtos.Task;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Task
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Task title is required")
                .MaximumLength(100).WithMessage("Task title must not exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Task description must not exceed 500 characters");

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project ID is required");

            RuleFor(x => x.RowVersion)
                .NotNull().WithMessage("RowVersion is required");
        }
    }
}

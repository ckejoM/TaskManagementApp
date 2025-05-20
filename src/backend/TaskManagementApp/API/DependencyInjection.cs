using Application.Validators.Task;
using Application.Validators;
using FluentValidation;
using Serilog;

namespace API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Add Validator Factory
            services.AddScoped<Application.Validators.IValidatorFactory, ValidatorFactory>();

            // Add FluentValidation
            services.AddValidatorsFromAssemblyContaining<UpdateTaskCommandValidator>();

            return services;
        }
    }
}

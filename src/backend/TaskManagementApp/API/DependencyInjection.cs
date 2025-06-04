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

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}

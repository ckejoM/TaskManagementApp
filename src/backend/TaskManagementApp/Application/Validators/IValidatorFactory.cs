using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public interface IValidatorFactory
    {
        IValidator<T>? GetValidator<T>();
    }
}

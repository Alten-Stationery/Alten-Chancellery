using FluentValidation;
using ServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.FluentValidators
{
    public class OfficeValitor:AbstractValidator<OfficeDTO>
    {
        public OfficeValitor() 
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithMessage("Error: ID is mandatory");
            RuleFor(u => u.Name)
                .NotEmpty()
                .WithMessage("Error: Name is mandatory");

        }
    }
}

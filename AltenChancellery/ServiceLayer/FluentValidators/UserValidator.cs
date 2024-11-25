using FluentValidation;
using ServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.FluentValidators
{
    public class UserValidator : AbstractValidator<UserDTO>
    {
        private const string passwordRegex = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        public UserValidator() 
        {
            RuleFor(u => u.Id)
                .NotEmpty().WithMessage("Id is mandatory");
            
            RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is mandatory")
            .EmailAddress().WithMessage("Insert a valid email");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is mandatory")
                .Matches(passwordRegex).WithMessage("Password must be at least 8 characters long, include at least one uppercase letter, one lowercase letter, one number, and one special character.");

            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Name is mandatory");

            RuleFor(u => u.Surname)
                .NotEmpty().WithMessage("Surname is mandatory");


        }
    }
}

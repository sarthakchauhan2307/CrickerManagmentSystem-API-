using CrickerManagmentSystem_API_.Models;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace CrickerManagmentSystem_API_.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator() { 
            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Email is Required");

            RuleFor(x => x.Password)
                .NotEmpty().
                 WithMessage("Password is required")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,}$")
                .WithMessage("Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");


            RuleFor(x => x.Mobile).NotEmpty()
                .Length(10)
                .Matches("^[0-9]+$")
                .WithMessage("Length must be 10");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Name is Required");


            
         }
    }
}

using CrickerManagmentSystem_API_.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CrickerManagmentSystem_API_.Validator
{
    public class PlayerValidator : AbstractValidator<Player>
    {
        public PlayerValidator()
        {
            RuleFor(x=>x.PlayerName).NotEmpty()
                .MaximumLength(100)
                .WithMessage("Player name is required");

            RuleFor(x => x.DateOfBirth).NotEmpty()
                .WithMessage("Date Of Birth Is required");

            RuleFor(x => x.Gender).NotEmpty();




        }
    }
}

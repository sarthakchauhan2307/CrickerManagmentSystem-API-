using System.Text.RegularExpressions;
using FluentValidation;
using CrickerManagmentSystem_API_.Models;
using Match = CrickerManagmentSystem_API_.Models.Match;

namespace CrickerManagmentSystem_API_.Validator
{
    public class MatchValidator : AbstractValidator<Match>
    {
        public MatchValidator()
        {
            RuleFor(x => x.Venue).NotEmpty()
                .WithMessage("Match Venue is Required");

            RuleFor(x => x.StartTime).NotEmpty()
                .WithMessage("Start time is required");
        }
    }
}

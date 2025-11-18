using Application.Activities.DTOs;
using FluentValidation;

namespace Application.Activities.Validators;

public class BaseActivityValidator<T, TDto> : AbstractValidator<T> where TDto : BaseActivityDto
{
    public BaseActivityValidator(Func<T, TDto> selector)
    {
        RuleFor(x => selector(x).Title)
            .NotEmpty()
            .WithMessage("Title is requirered")
            .MaximumLength(100)
            .WithMessage("Title max 100 characters");

        RuleFor(x => selector(x).Description)
            .NotEmpty()
            .WithMessage("Description is requirered");

        RuleFor(x => selector(x).Date)
            .GreaterThan(DateTime.UtcNow.Date)
            .WithMessage("Date must be in the future");

        RuleFor(x => selector(x).Category)
            .NotEmpty()
            .WithMessage("Category is required");

        RuleFor(x => selector(x).City)
            .NotEmpty()
            .WithMessage("City is required");

        RuleFor(x => selector(x).Venue)
            .NotEmpty()
            .WithMessage("Venue is required");

        RuleFor(x => selector(x).Latitude)
            .NotEmpty()
            .WithMessage("Latitude is required")
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude invalid");

        RuleFor(x => selector(x).Longitude)
            .NotEmpty()
            .WithMessage("Longitude is required")
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude invalid");
    }
}

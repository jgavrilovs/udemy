using Application.Activities.Commands;
using FluentValidation;

namespace Application.Activities.Validators;

public class CreateActivityValidator : AbstractValidator<CreateActivity.Command>
{
    public CreateActivityValidator()
    {
        RuleFor(x => x.ActivityDto.Title)
            .NotEmpty()
            .WithMessage("Title is requirered");

        RuleFor(x => x.ActivityDto.Description)
            .NotEmpty()
            .WithMessage("Description is requirered");
    }
}
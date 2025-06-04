using FluentValidation;

namespace ConsoleApp1.DTOs.Request.Validators;

public class AddToBasketRequestValidator : AbstractValidator<AddToBasketRequest>
{
    public AddToBasketRequestValidator()
    {
        RuleFor(request => request.ProductId)
            .NotNull().WithMessage("ProductId bilgisi boş olamaz")
            .GreaterThan(0).WithMessage("ProductId 0 olamaz");
    }
}
using FluentValidation;

namespace ConsoleApp1.DTOs.Request.Validators;

public class ProductCreateRequestValidator: AbstractValidator<ProductCreateRequest>
{
    public ProductCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("ürün adı boş olamaz.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("fiyat alanı boş olamaz");

    }
}
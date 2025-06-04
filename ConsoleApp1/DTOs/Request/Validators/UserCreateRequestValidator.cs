using FluentValidation;

namespace ConsoleApp1.DTOs.Request.Validators;

public class UserCreateRequestValidator: AbstractValidator<UserCreateRequest>
{
    public UserCreateRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Name boş olamaz.")
            .MinimumLength(3).WithMessage("Name alanı en az 3 karakterli olmalıdır");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
    }
}
using FluentValidation;

namespace ConsoleApp1.DTOs.Request.Validators;

public class UserUpdateRequestValidator: AbstractValidator<UserUpdateRequest>
{
    public UserUpdateRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Name boş olamaz.")
            .MinimumLength(3).WithMessage("Name alanı en az 3 karakterli olmalıdır");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
    }
}
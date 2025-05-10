using ClassLibrary;
using FluentValidation;

namespace ChatsWebApi.Components.Validators
{
    public class LoginFieldsValidator : AbstractValidator<LoginFields>
    {
        public LoginFieldsValidator()
        {
            RuleFor(lf => lf.NickName).NotNull().Length(2, 50);
            RuleFor(lf => lf.Password).NotNull().Length(8, 100);
        }
    }
}

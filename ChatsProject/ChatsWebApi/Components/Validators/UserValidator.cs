using ChatsWebApi.Components.Types.Database;
using FluentValidation;

namespace ChatsWebApi.Components.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.NickName).NotNull().Length(1, 50);
            RuleFor(u => u.Password).NotNull().Length(8, 100);
        }
    }
}

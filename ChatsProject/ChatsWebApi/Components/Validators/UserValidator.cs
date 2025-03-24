using ChatsWebApi.Components.Types.Database;
using FluentValidation;

namespace ChatsWebApi.Components.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName).NotNull().Length(2, 50);
            RuleFor(u => u.LastName).NotNull().Length(2, 50);
            RuleFor(u => u.NickName).NotNull().Length(2, 50);
            RuleFor(u => u.Password).NotNull().Length(8, 100);
        }
    }
}

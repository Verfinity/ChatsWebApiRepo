using ClassLibrary;
using FluentValidation;

namespace ChatsWebApi.Components.Validators
{
    public class ChatsToUsersValidator : AbstractValidator<ChatsToUsers>
    {
        public ChatsToUsersValidator()
        {
            RuleFor(i => i.ChatId).NotNull().NotEmpty();
            RuleFor(i => i.UserId).NotNull().NotEmpty();
        }
    }
}

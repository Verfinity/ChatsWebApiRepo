using ClassLibrary;
using FluentValidation;

namespace ChatsWebApi.Components.Validators
{
    public class ChatsUsersValidator : AbstractValidator<ChatsUsers>
    {
        public ChatsUsersValidator()
        {
            RuleFor(i => i.ChatId).NotNull().NotEmpty();
            RuleFor(i => i.UserId).NotNull().NotEmpty();
        }
    }
}

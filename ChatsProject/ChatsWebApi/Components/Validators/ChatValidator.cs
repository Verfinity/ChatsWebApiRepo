using ChatsWebApi.Components.Types.Database;
using FluentValidation;

namespace ChatsWebApi.Components.Validators
{
    public class ChatValidator : AbstractValidator<Chat>
    {
        public ChatValidator()
        {
            RuleFor(c => c.Name).NotNull().Length(2, 50);
        }
    }
}

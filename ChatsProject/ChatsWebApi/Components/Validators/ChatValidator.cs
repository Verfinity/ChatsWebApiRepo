using ClassLibrary;
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

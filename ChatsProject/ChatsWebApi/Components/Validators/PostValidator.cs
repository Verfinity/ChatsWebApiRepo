using ClassLibrary;
using FluentValidation;

namespace ChatsWebApi.Components.Validators
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(p => p.Content).NotNull().NotEmpty();
            RuleFor(p => p.ChatId).NotNull().NotEmpty();
            RuleFor(p => p.UserId).NotNull().NotEmpty();
        }
    }
}

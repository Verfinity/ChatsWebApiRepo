using ChatsWebApi.Components.Types.Database;
using FluentValidation;

namespace ChatsWebApi.Components.Validators
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(p => p.Content).NotNull().Length(10, 500);
            RuleFor(p => p.ChatId).NotNull();
            RuleFor(p => p.UserId).NotNull();
        }
    }
}

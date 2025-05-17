using ClassLibrary;
using ChatsWebApi.Components.Types.JWT.Options;

namespace ChatsWebApi.Components.Types.JWT.Logic
{
    public interface IJWTCreator
    {
        public string CreateJWT(User user, IAuthOptions authOptions);
    }
}

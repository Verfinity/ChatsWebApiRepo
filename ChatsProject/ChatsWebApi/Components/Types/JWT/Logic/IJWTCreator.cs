using ChatsWebApi.Components.Types.JWT.Options;
using ChatsWebApi.Components.Types.Roles;

namespace ChatsWebApi.Components.Types.JWT.Logic
{
    public interface IJWTCreator
    {
        public string CreateJWT(string nickName, Role role, IAuthOptions authOptions);
    }
}

using ChatsWebApi.Components.Types.JWT;

namespace ChatsWebApi.Components.Types.Roles
{
    public interface IAdminLogs
    {
        public LoginFields[] AdminLogsList { get; init; }
        public bool IsAdmin(LoginFields adminLog);
    }
}

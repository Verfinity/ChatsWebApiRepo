using ChatsWebApi.Components.Types.JWT;

namespace ChatsWebApi.Components.Types.Roles
{
    public class AdminLogs : IAdminLogs
    {
        public required LoginFields[] AdminLogsList { get; init; }

        public bool IsAdmin(LoginFields adminLog)
        {
            return AdminLogsList.FirstOrDefault(a => a.NickName == adminLog.NickName && a.Password == adminLog.Password) != null;
        }
    }
}

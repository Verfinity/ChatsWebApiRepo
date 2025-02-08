namespace ChatsWebApi.Components.Types.Roles
{
    public class AdminLogs : IAdminLogs
    {
        public required AdminLog[] AdminLogsList { get; init; }

        public bool IsAdmin(AdminLog adminLog)
        {
            return AdminLogsList.FirstOrDefault(a => a.NickName == adminLog.NickName && a.Password == adminLog.Password) != null;
        }
    }
}
